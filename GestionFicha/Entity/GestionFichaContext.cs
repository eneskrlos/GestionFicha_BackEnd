using GestionFicha.Utils;

namespace GestionFicha.Entity
{
    using GestionFicha.Entity.RH;
    using System.Data.Entity;
    using static Constants;

    public partial class GestionFichaContext : DbContext
    {
        public GestionFichaContext()
            : base("name=GestionFichaContext")
        {
            // Esto es solo para ver el código SQL que se genera
            Database.Log = s => log.Info(s);
        }

        public virtual DbSet<Administrador> Administradores { get; set; }
        public virtual DbSet<Gestor> Gestores { get; set; }
        public virtual DbSet<Personal> Personal { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Orden> Orden { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<GestionFichaContext>(null);

            modelBuilder.Entity<Administrador>()
                .Property(e => e.nInterno)
                .HasPrecision(5, 0);

            modelBuilder.Entity<Personal>()
                .Property(e => e.nInterno)
                .HasPrecision(5, 0);

            modelBuilder.Entity<Personal>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Personal>()
                .Property(e => e.apellidos)
                .IsUnicode(false);

            modelBuilder.Entity<Personal>()
                .Property(e => e.usuarioRed)
                .IsUnicode(false);

            modelBuilder.Entity<Personal>()
                .Property(e => e.nInternoResp)
                .HasPrecision(5, 0);

            modelBuilder.Entity<Personal>()
                .HasOptional(e => e.admin)
                .WithRequired(e => e.Persona);

            modelBuilder.Entity<Personal>()
                .HasOptional(e => e.gestor)
                .WithRequired(e => e.Persona);

            modelBuilder.Entity<Personal>()
                .HasMany(e => e.subordinados)
                .WithOptional(e => e.responsable)
                .HasForeignKey(e => e.nInternoResp);

            //entidad producto
            modelBuilder.Entity<Producto>()
                .HasKey(e => e.id_producto);

            modelBuilder.Entity<Producto>()
                .Property(e => e.nombre)
                .IsRequired();

            modelBuilder.Entity<Producto>()
                .Property(e => e.descripcion)
                .IsOptional();

            modelBuilder.Entity<Producto>()
                .Property(e => e.precio)
                .IsRequired();

            //entidad orden
            modelBuilder.Entity<Orden>()
                .HasKey(e => e.id_orden);

            modelBuilder.Entity<Orden>()
                .Property(e => e.fecha)
                .IsRequired();

            modelBuilder.Entity<Orden>()
                .Property(e => e.direccion)
                .IsRequired();

            modelBuilder.Entity<Orden>()
                .Property(e => e.cantidad)
                .IsRequired();

        }
    }
}