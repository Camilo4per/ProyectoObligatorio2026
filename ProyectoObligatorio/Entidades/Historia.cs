using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Historia;

namespace Dominio.Entidades
{
    public class Historia : IValidable
    {
        public int Id { get; set; }
        public List<Categoria> Categorias { get; set; }
        public Capitulo Capitulo { get; set; }
        public EstadoHistoria Estado { get; set; }

        // Value Objects
        public Titulo Titulo { get; init; }
        public Sinopsis Sinopsis { get; init; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
