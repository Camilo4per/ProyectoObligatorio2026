using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Opcion;

namespace Dominio.Entidades
{
    public class Opcion : IValidable
    {
        public Capitulo CapDestino { get; set; }

        // Value Objects
        public TextoOpcion Texto { get; set; }
        public void Validar()
        {
            Texto.Validar();
            if (Texto == null) throw new OpcionException("Texto requerido");
            if (CapDestino == null) throw new OpcionException("La opción debe llevar a otro capítulo");
        }
    }
}
