using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;

namespace Dominio.Entidades
{
    public class Opcion : IValidable
    {
        public Capitulo CapDestino { get; set; }

        // Value Objects
        public Texto Texto { get; set; }
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
