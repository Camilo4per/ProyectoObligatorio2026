using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAccesoDatos.RepositorioEntityFramework
{
    public class DominioContext : DbContext
    {
        #region DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<Capitulo> Capitulos { get; set; }
        public DbSet<Intermedio> Intermedios { get; set; }
        public DbSet<Final> Finales { get; set; }
        public DbSet<Lectura> Lecturas { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configura la cadena de conexión a la base de datos
            optionsBuilder.UseSqlServer(@"SERVER=(localdb)\MsSqlLocalDb;DATABASE=ObligatorioDB;Integrated Security=true;");
        }

        // Configura las FKs para las opciones de los capítulos intermedios
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FK unica: Capitulo -> Historia
            modelBuilder.Entity<Capitulo>()
                .HasOne<Historia>()
                .WithMany(h => h.Capitulos)
                .HasForeignKey(c => c.idHistoria)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Intermedio>()
                .OwnsMany(i => i.Opciones, op =>
                {
                    op.WithOwner().HasForeignKey("idCapitulo");
                    op.HasOne(o => o.CapDestino).WithMany().HasForeignKey(o => o.idCapDestino);
                });
        }
    }
}
