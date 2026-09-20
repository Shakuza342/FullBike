using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FullBike.Models
{
    public class ERPContext : DbContext
    {
        public ERPContext() : base("ERPConnection")
        {
            // SOLUCIÓN DEFINITIVA - Ignorar migraciones y usar creación automática
            Database.SetInitializer<ERPContext>(null);
            Configuration.ProxyCreationEnabled = false;
            // O si quieres que se recree en desarrollo:
            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ERPContext>());
        }

        // DbSets existentes
        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }
        public DbSet<Request> Requests { get; set; }

        // NUEVOS DbSets para nómina
        public DbSet<PagoNomina> PagosNomina { get; set; }
        public DbSet<LiquidacionContrato> LiquidacionesContrato { get; set; }
        public DbSet<AbonoCliente> AbonosClientes { get; set; }
        public DbSet<AuditoriaVenta> AuditoriaVentas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }   
        public DbSet<Descuento> Descuentos { get; set; }
        public DbSet<DescuentoAplicado> DescuentosAplicados { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();


            modelBuilder.Entity<DetalleVenta>()
                .HasRequired(d => d.Venta)
                .WithMany(v => v.DetallesVenta)
                .HasForeignKey(d => d.VentaId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DetalleCompra>()
                .HasRequired(d => d.Compra)
                .WithMany(c => c.DetallesCompra)
                .HasForeignKey(d => d.CompraId)
                .WillCascadeOnDelete(true);

            // Configuraciones para los nuevos modelos
            modelBuilder.Entity<PagoNomina>()
                .HasRequired(p => p.Empleado)
                .WithMany()
                .HasForeignKey(p => p.EmpleadoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LiquidacionContrato>()
                .HasRequired(l => l.Empleado)
                .WithMany()
                .HasForeignKey(l => l.EmpleadoId)
                .WillCascadeOnDelete(false);

            // ⚠️ NO USAR HasPrecision en ningún lado
        }
    }
}