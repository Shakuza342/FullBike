using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace FullBike.Controllers
{
    public class NominaController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Nomina/Index
        public ActionResult Index()
        {
            try
            {
                var pagos = db.PagosNomina
                    .Include(p => p.Empleado)
                    .OrderByDescending(p => p.FechaPago)
                    .ToList();

                return View(pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar nómina: " + ex.Message;
                return View(new List<PagoNomina>());
            }
        }

        // GET: CrearPago
        public ActionResult CrearPago(int empleadoId)
        {
            try
            {
                var empleado = db.Empleados.FirstOrDefault(e => e.Id == empleadoId);

                if (empleado == null)
                {
                    TempData["Error"] = "Empleado no encontrado";
                    return RedirectToAction("Index", "Empleados");
                }

                var pago = new PagoNomina
                {
                    EmpleadoId = empleadoId,
                    Empleado = empleado,
                    SalarioBasico = empleado.SalarioBase,
                    PeriodoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    PeriodoFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                            DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                    FechaPago = DateTime.Now,
                    Estado = "Pendiente"
                };

                // Calcular valores iniciales
                CalcularPagoNomina(pago);

                return View(pago);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar pago de nómina: " + ex.Message;
                return RedirectToAction("Index", "Empleados");
            }
        }

        // POST: CrearPago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPago(PagoNomina pago)
        {
            try
            {
                // Deshabilitar validación temporalmente para campos calculados
                ModelState.Remove("Empleado");
                ModelState.Remove("TotalDevengado");
                ModelState.Remove("SaludEmpleado");
                ModelState.Remove("PensionEmpleado");
                ModelState.Remove("TotalDeducciones");
                ModelState.Remove("NetoAPagar");
                ModelState.Remove("SaludEmpresa");
                ModelState.Remove("PensionEmpresa");
                ModelState.Remove("ARLEmpresa");
                ModelState.Remove("CajaCompensacion");
                ModelState.Remove("ICBF");
                ModelState.Remove("SENA");

                if (ModelState.IsValid)
                {
                    // Recalcular antes de guardar
                    CalcularPagoNomina(pago);

                    // Asegurar que el estado esté establecido
                    pago.Estado = "Pagado";

                    db.PagosNomina.Add(pago);
                    db.SaveChanges();

                    TempData["Mensaje"] = "Pago de nómina registrado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Errores de validación en los datos básicos";
                }

                pago.Empleado = db.Empleados.Find(pago.EmpleadoId);
                return View(pago);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar pago: " + ex.Message;
                pago.Empleado = db.Empleados.Find(pago.EmpleadoId);
                return View(pago);
            }
        }
        // GET: DetallesPago
        public ActionResult DetallesPago(int id)
        {
            try
            {
                var pago = db.PagosNomina
                    .Include(p => p.Empleado)
                    .FirstOrDefault(p => p.Id == id);

                if (pago == null)
                {
                    TempData["Error"] = "Pago no encontrado";
                    return RedirectToAction("Index");
                }

                return View(pago);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar detalles: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: LiquidarContrato
        public ActionResult LiquidarContrato(int empleadoId)
        {
            try
            {
                var empleado = db.Empleados.FirstOrDefault(e => e.Id == empleadoId);

                if (empleado == null)
                {
                    TempData["Error"] = "Empleado no encontrado";
                    return RedirectToAction("Index", "Empleados");
                }

                var liquidacion = new LiquidacionContrato
                {
                    EmpleadoId = empleadoId,
                    Empleado = empleado,
                    FechaLiquidacion = DateTime.Now,
                    TipoSalida = "Renuncia"
                };

                CalcularLiquidacion(liquidacion, empleado);
                return View(liquidacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar liquidación: " + ex.Message;
                return RedirectToAction("Index", "Empleados");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LiquidarContrato(LiquidacionContrato liquidacion)
        {
            try
            {
                // LIMPIAR ERRORES DE VALIDACIÓN PARA CAMPOS CALCULADOS
                ModelState.Remove("SalarioPendiente");
                ModelState.Remove("VacacionesPendientes");
                ModelState.Remove("Cesantias");
                ModelState.Remove("InteresesCesantias");
                ModelState.Remove("PrimaServicios");
                ModelState.Remove("Indemnizacion");
                ModelState.Remove("TotalLiquidacion");
                ModelState.Remove("SaludDeducir");
                ModelState.Remove("PensionDeducir");
                ModelState.Remove("NetoAPagar");
                ModelState.Remove("Empleado");

                if (ModelState.IsValid)
                {
                    var empleado = db.Empleados.Find(liquidacion.EmpleadoId);
                    if (empleado == null)
                    {
                        TempData["Error"] = "Empleado no encontrado";
                        return RedirectToAction("Index", "Empleados");
                    }

                    CalcularLiquidacion(liquidacion, empleado);
                    db.LiquidacionesContrato.Add(liquidacion);

                    // Actualizar estado del empleado
                    empleado.Estado = "Retirado";
                    empleado.FechaTerminacion = liquidacion.FechaLiquidacion;
                    empleado.Activo = false;

                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Mensaje"] = "Contrato liquidado exitosamente";
                    return RedirectToAction("Index", "Empleados");
                }
                else
                {
                    // Mostrar errores específicos
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                         .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                    TempData["Error"] = "Errores de validación: " + string.Join("; ", errors.SelectMany(e => e.Value));
                }

                liquidacion.Empleado = db.Empleados.Find(liquidacion.EmpleadoId);
                return View(liquidacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al liquidar contrato: " + ex.Message;
                liquidacion.Empleado = db.Empleados.Find(liquidacion.EmpleadoId);
                return View(liquidacion);
            }
        }

        // MÉTODO DE CÁLCULO DE NÓMINA
        private void CalcularPagoNomina(PagoNomina pago)
        {
            // MANTENER TODOS TUS CÁLCULOS ORIGINALES EXACTAMENTE COMO ESTABAN

            // Calcular horas extras si existen
            decimal totalHorasExtras = 0;
            if (pago.HorasExtras > 0)
            {
                // Asumiendo que son horas extra diurnas (25% recargo)
                decimal valorHoraNormal = pago.SalarioBasico / 240m;
                decimal valorHoraExtra = valorHoraNormal * 1.25m;
                totalHorasExtras = valorHoraExtra * pago.HorasExtras;
            }

            // Total devengado
            pago.TotalDevengado = pago.SalarioBasico + totalHorasExtras + pago.Bonificaciones + pago.Comisiones;

            // Cálculo de seguridad social según normativa colombiana
            decimal baseCotizacion = pago.SalarioBasico * 0.4m;

            // Deducciones legales
            pago.SaludEmpleado = baseCotizacion * 0.04m; // 4% salud
            pago.PensionEmpleado = baseCotizacion * 0.04m; // 4% pensión

            // Total deducciones
            pago.TotalDeducciones = pago.SaludEmpleado + pago.PensionEmpleado +
                                  pago.RetencionFuente + pago.Prestamos + pago.OtrasDeducciones;

            // Neto a pagar
            pago.NetoAPagar = pago.TotalDevengado - pago.TotalDeducciones;

            // Aportes empresa
            pago.SaludEmpresa = baseCotizacion * 0.085m; // 8.5% salud empresa
            pago.PensionEmpresa = baseCotizacion * 0.12m; // 12% pensión empresa
            pago.ARLEmpresa = pago.SalarioBasico * 0.00522m; // 0.522% ARL
            pago.CajaCompensacion = pago.SalarioBasico * 0.04m; // 4% caja compensación
            pago.ICBF = pago.SalarioBasico * 0.03m; // 3% ICBF
            pago.SENA = pago.SalarioBasico * 0.02m; // 2% SENA

            // ✅ SOLO AGREGAR ESTAS LÍNEAS DE REDONDEO AL FINAL:
            pago.TotalDevengado = Math.Round(pago.TotalDevengado, 0);
            pago.SaludEmpleado = Math.Round(pago.SaludEmpleado, 0);
            pago.PensionEmpleado = Math.Round(pago.PensionEmpleado, 0);
            pago.TotalDeducciones = Math.Round(pago.TotalDeducciones, 0);
            pago.NetoAPagar = Math.Round(pago.NetoAPagar, 0);
            pago.SaludEmpresa = Math.Round(pago.SaludEmpresa, 0);
            pago.PensionEmpresa = Math.Round(pago.PensionEmpresa, 0);
            pago.ARLEmpresa = Math.Round(pago.ARLEmpresa, 0);
            pago.CajaCompensacion = Math.Round(pago.CajaCompensacion, 0);
            pago.ICBF = Math.Round(pago.ICBF, 0);
            pago.SENA = Math.Round(pago.SENA, 0);
        }


        // MÉTODO DE CÁLCULO DE LIQUIDACIÓN
        private void CalcularLiquidacion(LiquidacionContrato liquidacion, Empleado empleado)
        {
            // MANTENER TODOS TUS CÁLCULOS ORIGINALES

            // Calcular días trabajados en el mes actual
            var fechaInicioMes = new DateTime(liquidacion.FechaLiquidacion.Year, liquidacion.FechaLiquidacion.Month, 1);
            var fechaFin = liquidacion.FechaLiquidacion;
            var diasTrabajadosMes = (fechaFin - fechaInicioMes).Days + 1;

            // Salario pendiente (proporcional al mes)
            liquidacion.SalarioPendiente = (empleado.SalarioBase / 30) * diasTrabajadosMes;

            // Vacaciones (15 días por año)
            var mesesTrabajados = empleado.MesesTrabajados;
            var diasVacaciones = (15m * mesesTrabajados) / 12m;
            liquidacion.VacacionesPendientes = (empleado.SalarioBase / 30) * diasVacaciones;

            // Cesantías (un mes por año trabajado)
            liquidacion.Cesantias = empleado.SalarioBase * mesesTrabajados / 12;

            // Intereses sobre cesantías (12% anual)
            liquidacion.InteresesCesantias = liquidacion.Cesantias * 0.12m * mesesTrabajados / 12;

            // Prima de servicios (medio mes por semestre)
            liquidacion.PrimaServicios = empleado.SalarioBase * mesesTrabajados / 12;

            // Indemnización (solo para despido sin justa causa)
            if (liquidacion.TipoSalida == "Despido")
            {
                liquidacion.Indemnizacion = empleado.SalarioBase * Math.Max(mesesTrabajados / 12, 1);
            }
            else
            {
                liquidacion.Indemnizacion = 0;
            }

            // Total liquidación
            liquidacion.TotalLiquidacion = liquidacion.SalarioPendiente +
                                        liquidacion.VacacionesPendientes +
                                        liquidacion.Cesantias +
                                        liquidacion.InteresesCesantias +
                                        liquidacion.PrimaServicios +
                                        liquidacion.Indemnizacion;

            // Deducciones (4% salud y 4% pensión)
            liquidacion.SaludDeducir = liquidacion.TotalLiquidacion * 0.04m;
            liquidacion.PensionDeducir = liquidacion.TotalLiquidacion * 0.04m;

            // Neto a pagar
            liquidacion.NetoAPagar = liquidacion.TotalLiquidacion -
                                   liquidacion.SaludDeducir -
                                   liquidacion.PensionDeducir -
                                   liquidacion.RetencionFuente;

            // ✅✅✅ REDONDEO AGREGADO - A NÚMEROS ENTEROS
            liquidacion.SalarioPendiente = Math.Round(liquidacion.SalarioPendiente, 0);
            liquidacion.VacacionesPendientes = Math.Round(liquidacion.VacacionesPendientes, 0);
            liquidacion.Cesantias = Math.Round(liquidacion.Cesantias, 0);
            liquidacion.InteresesCesantias = Math.Round(liquidacion.InteresesCesantias, 0);
            liquidacion.PrimaServicios = Math.Round(liquidacion.PrimaServicios, 0);
            liquidacion.Indemnizacion = Math.Round(liquidacion.Indemnizacion, 0);
            liquidacion.TotalLiquidacion = Math.Round(liquidacion.TotalLiquidacion, 0);
            liquidacion.SaludDeducir = Math.Round(liquidacion.SaludDeducir, 0);
            liquidacion.PensionDeducir = Math.Round(liquidacion.PensionDeducir, 0);
            liquidacion.NetoAPagar = Math.Round(liquidacion.NetoAPagar, 0);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}