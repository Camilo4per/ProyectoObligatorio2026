using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAccesoDatos.EF
{
    public class HistoriasContext : DbContext
    {
        #region DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<Capitulo> Capitulos { get; set; }
        public DbSet<Lectura> Lecturas { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configura la cadena de conexión a la base de datos
            optionsBuilder.UseSqlServer(@"SERVER=(localdb)\MsSqlLocalDb;DATABASE=ObligatorioDB;Integrated Security=true;");
        }
    }
}
