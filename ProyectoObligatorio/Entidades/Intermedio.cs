using Dominio.InterfacesDelDominio;

namespace Dominio.Entidades
{
    public class Intermedio : Capitulo, IValidable
    {
        public List<Opcion> Opciones;
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
