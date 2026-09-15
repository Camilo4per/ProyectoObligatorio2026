using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Categoria;

namespace Dominio.Entidades
{
    public class Categoria : IValidable
    {
        public int Id { get; set; }
        // Agregado unicamente para poder mapear la relación con Historias
        public List<Historia> Historias { get; set; }

        // Value Objects
        public Nombre Nombre { get; set; }

        public void Validar()
        {
            if (Nombre == null)
                throw new CategoriaException("Nombre requerido");
            Nombre.Validar();
        }
    }
}
