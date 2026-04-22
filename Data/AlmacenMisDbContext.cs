using AlmacenMis.Dominio;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Data
{
    public class AlmacenMisDbContext : DbContext
    {
        public AlmacenMisDbContext(DbContextOptions<AlmacenMisDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<Almacen> Almacenes => Set<Almacen>();
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<ProductoProveedor> ProductoProveedores => Set<ProductoProveedor>();
        public DbSet<ProductoAlmacen> ProductoAlmacenes => Set<ProductoAlmacen>();
        public DbSet<ProductoProveedorAlmacen> ProductoProveedorAlmacenes => Set<ProductoProveedorAlmacen>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedor");
            modelBuilder.Entity<Almacen>().ToTable("Almacen");
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<ProductoProveedor>().ToTable("Producto_Proveedor");
            modelBuilder.Entity<ProductoAlmacen>().ToTable("Producto_Almacen");
            modelBuilder.Entity<ProductoProveedorAlmacen>().ToTable("Producto_Proveedor_Almacen");

            modelBuilder.Entity<Stock>()
                .HasOne<Producto>()
                .WithMany()
                .HasForeignKey(s => s.producto_id);

            modelBuilder.Entity<Stock>()
                .HasOne<Almacen>()
                .WithMany()
                .HasForeignKey(s => s.almacen_id);

            modelBuilder.Entity<ProductoProveedor>()
                .HasOne<Producto>()
                .WithMany()
                .HasForeignKey(p => p.producto_id);

            modelBuilder.Entity<ProductoProveedor>()
                .HasOne<Proveedor>()
                .WithMany()
                .HasForeignKey(p => p.proveedor_id);

            modelBuilder.Entity<ProductoAlmacen>()
                .HasOne<Producto>()
                .WithMany()
                .HasForeignKey(p => p.producto_id);

            modelBuilder.Entity<ProductoAlmacen>()
                .HasOne<Almacen>()
                .WithMany()
                .HasForeignKey(p => p.almacen_id);

            modelBuilder.Entity<ProductoProveedorAlmacen>()
                .HasOne<Producto>()
                .WithMany()
                .HasForeignKey(p => p.producto_id);

            modelBuilder.Entity<ProductoProveedorAlmacen>()
                .HasOne<Proveedor>()
                .WithMany()
                .HasForeignKey(p => p.proveedor_id);

            modelBuilder.Entity<ProductoProveedorAlmacen>()
                .HasOne<Almacen>()
                .WithMany()
                .HasForeignKey(p => p.almacen_id);
        }
    }
}
