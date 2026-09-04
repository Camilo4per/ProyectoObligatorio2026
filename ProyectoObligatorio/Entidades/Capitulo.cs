using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;

namespace Dominio.Entidades
{
    public abstract class Capitulo : IValidable
    {
        public int Id { get; set; }

        // Value Objects
        public Titulo Titulo { get; set; }
        public Texto Texto { get; set; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
