using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Categoria;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class Categoria : IValidable
    {
        #region Atributos
        public int Id { get; set; }
        // Lista de Historias agregada unicamente para poder crear la tabla CategoriaHistoria
        // en la base de datos, ya que es una relación muchos a muchos
        public List<Historia>? Historias { get; set; }

        public Nombre Nombre { get; set; }
        #endregion
        public void Validar()
        {
            if (Nombre == null) throw new CategoriaException("El nombre de la categoría no puede ser nulo.");
            Nombre.Validar();
        }
    }
}
