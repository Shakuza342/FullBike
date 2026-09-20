namespace FullBike.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbonoCliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        VentaId = c.Int(nullable: false),
                        FechaAbono = c.DateTime(nullable: false),
                        ValorAbono = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MetodoPago = c.String(maxLength: 50),
                        Comprobante = c.String(maxLength: 100),
                        ObservacionesAbono = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Venta", t => t.VentaId)
                .Index(t => t.ClienteId)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Apellido = c.String(nullable: false, maxLength: 100),
                        Documento = c.String(nullable: false, maxLength: 20),
                        Email = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Direccion = c.String(maxLength: 200),
                        FechaNacimiento = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        TipoCliente = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaVenta = c.DateTime(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        EmpleadoId = c.Int(nullable: false),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(maxLength: 20),
                        MetodoPago = c.String(maxLength: 50),
                        Observaciones = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .Index(t => t.ClienteId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.DetalleVenta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VentaId = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repuesto", t => t.RepuestoId)
                .ForeignKey("dbo.Venta", t => t.VentaId, cascadeDelete: true)
                .Index(t => t.VentaId)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.Repuesto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Categoria = c.String(nullable: false, maxLength: 50),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 200),
                        Marca = c.String(maxLength: 50),
                        Modelo = c.String(maxLength: 50),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaActualizacion = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        ProveedorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(nullable: false, maxLength: 150),
                        Contacto = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        Nit = c.String(maxLength: 20),
                        Direccion = c.String(maxLength: 200),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empleado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Apellido = c.String(nullable: false, maxLength: 100),
                        Documento = c.String(nullable: false, maxLength: 20),
                        Email = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Direccion = c.String(maxLength: 200),
                        Cargo = c.String(nullable: false, maxLength: 50),
                        FechaContratacion = c.DateTime(nullable: false),
                        FechaTerminacion = c.DateTime(),
                        SalarioBase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoContrato = c.String(nullable: false, maxLength: 20),
                        Estado = c.String(nullable: false, maxLength: 20),
                        Banco = c.String(maxLength: 50),
                        NumeroCuenta = c.String(maxLength: 20),
                        TipoCuenta = c.String(maxLength: 10),
                        EPS = c.String(maxLength: 20),
                        Pension = c.String(maxLength: 20),
                        ARL = c.String(maxLength: 20),
                        CajaCompensacion = c.String(maxLength: 20),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        Usuario = c.String(maxLength: 100),
                        PasswordHash = c.String(maxLength: 255),
                        Rol = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Factura",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VentaId = c.Int(nullable: false),
                        NumeroFactura = c.String(maxLength: 50),
                        FechaEmision = c.DateTime(nullable: false),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DescuentoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetencionTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstadoFactura = c.String(maxLength: 50),
                        ObservacionesFactura = c.String(maxLength: 500),
                        NombreClienteFactura = c.String(maxLength: 100),
                        NitCliente = c.String(maxLength: 50),
                        DireccionCliente = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Venta", t => t.VentaId)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.AuditoriaVenta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TablaAfectada = c.String(maxLength: 50),
                        Accion = c.String(maxLength: 20),
                        RegistroId = c.Int(nullable: false),
                        Usuario = c.String(maxLength: 100),
                        FechaHora = c.DateTime(nullable: false),
                        DatosAnteriores = c.String(),
                        DatosNuevos = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Descripcion = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Compra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaCompra = c.DateTime(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        EmpleadoId = c.Int(nullable: false),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(maxLength: 20),
                        NumeroFactura = c.String(maxLength: 50),
                        Observaciones = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.DetalleCompra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompraId = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Compra", t => t.CompraId, cascadeDelete: true)
                .ForeignKey("dbo.Repuesto", t => t.RepuestoId)
                .Index(t => t.CompraId)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.Descuento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        TipoDescuento = c.String(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AplicaTodosClientes = c.Boolean(nullable: false),
                        ClienteId = c.Int(),
                        AplicaTodosProductos = c.Boolean(nullable: false),
                        CategoriaId = c.Int(),
                        MontoMinimo = c.Decimal(precision: 18, scale: 2),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        CreadoPor = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.DescuentoAplicado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VentaId = c.Int(nullable: false),
                        DescuentoId = c.Int(nullable: false),
                        ValorAplicado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoAplicado = c.String(),
                        FechaAplicacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descuento", t => t.DescuentoId)
                .ForeignKey("dbo.Venta", t => t.VentaId)
                .Index(t => t.VentaId)
                .Index(t => t.DescuentoId);
            
            CreateTable(
                "dbo.LiquidacionContrato",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpleadoId = c.Int(nullable: false),
                        FechaLiquidacion = c.DateTime(nullable: false),
                        TipoSalida = c.String(),
                        SalarioPendiente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VacacionesPendientes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cesantias = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresesCesantias = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrimaServicios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Indemnizacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalLiquidacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaludDeducir = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PensionDeducir = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetencionFuente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetoAPagar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observaciones = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.PagoNomina",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpleadoId = c.Int(nullable: false),
                        PeriodoInicio = c.DateTime(nullable: false),
                        PeriodoFin = c.DateTime(nullable: false),
                        FechaPago = c.DateTime(nullable: false),
                        SalarioBasico = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HorasExtras = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bonificaciones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comisiones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDevengado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaludEmpleado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PensionEmpleado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetencionFuente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Prestamos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OtrasDeducciones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDeducciones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetoAPagar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaludEmpresa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PensionEmpresa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ARLEmpresa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CajaCompensacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ICBF = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SENA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descripcion = c.String(maxLength: 1000),
                        Estado = c.String(maxLength: 50),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PagoNomina", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.LiquidacionContrato", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.DescuentoAplicado", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.DescuentoAplicado", "DescuentoId", "dbo.Descuento");
            DropForeignKey("dbo.Descuento", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Descuento", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.Compra", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.Compra", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.DetalleCompra", "RepuestoId", "dbo.Repuesto");
            DropForeignKey("dbo.DetalleCompra", "CompraId", "dbo.Compra");
            DropForeignKey("dbo.Factura", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.Venta", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.DetalleVenta", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.DetalleVenta", "RepuestoId", "dbo.Repuesto");
            DropForeignKey("dbo.Repuesto", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.Venta", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.AbonoCliente", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.AbonoCliente", "ClienteId", "dbo.Cliente");
            DropIndex("dbo.PagoNomina", new[] { "EmpleadoId" });
            DropIndex("dbo.LiquidacionContrato", new[] { "EmpleadoId" });
            DropIndex("dbo.DescuentoAplicado", new[] { "DescuentoId" });
            DropIndex("dbo.DescuentoAplicado", new[] { "VentaId" });
            DropIndex("dbo.Descuento", new[] { "CategoriaId" });
            DropIndex("dbo.Descuento", new[] { "ClienteId" });
            DropIndex("dbo.DetalleCompra", new[] { "RepuestoId" });
            DropIndex("dbo.DetalleCompra", new[] { "CompraId" });
            DropIndex("dbo.Compra", new[] { "EmpleadoId" });
            DropIndex("dbo.Compra", new[] { "ProveedorId" });
            DropIndex("dbo.Factura", new[] { "VentaId" });
            DropIndex("dbo.Repuesto", new[] { "ProveedorId" });
            DropIndex("dbo.DetalleVenta", new[] { "RepuestoId" });
            DropIndex("dbo.DetalleVenta", new[] { "VentaId" });
            DropIndex("dbo.Venta", new[] { "EmpleadoId" });
            DropIndex("dbo.Venta", new[] { "ClienteId" });
            DropIndex("dbo.AbonoCliente", new[] { "VentaId" });
            DropIndex("dbo.AbonoCliente", new[] { "ClienteId" });
            DropTable("dbo.Request");
            DropTable("dbo.PagoNomina");
            DropTable("dbo.LiquidacionContrato");
            DropTable("dbo.DescuentoAplicado");
            DropTable("dbo.Descuento");
            DropTable("dbo.DetalleCompra");
            DropTable("dbo.Compra");
            DropTable("dbo.Categoria");
            DropTable("dbo.AuditoriaVenta");
            DropTable("dbo.Factura");
            DropTable("dbo.Empleado");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Repuesto");
            DropTable("dbo.DetalleVenta");
            DropTable("dbo.Venta");
            DropTable("dbo.Cliente");
            DropTable("dbo.AbonoCliente");
        }
    }
}
