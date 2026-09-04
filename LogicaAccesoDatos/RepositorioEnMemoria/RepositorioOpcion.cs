using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioOpcion : IRepositorioOpcion
    {
        private static List<Opcion> opciones = new List<Opcion>();

        public void Add(Opcion nuevo)
        {
            if (nuevo == null) { throw new OpcionException("La opcion esta vacia"); }
            nuevo.Validar();
            opciones.Add(nuevo);
        }

        public void ExisteOpcion(Opcion aVerificar)
        {
            if (aVerificar == null) { throw new OpcionException("La opcion esta vacia"); }
            if (!opciones.Contains(aVerificar)) { throw new OpcionException("La opcion no existe"); }
        }

        public Opcion FindById(int id)
        {
            if (id >= 0 && id < opciones.Count) return opciones[id];

            throw new OpcionException("Opcion inexistente");
        }

        public List<Opcion> FindAll()
        {
            return new List<Opcion>(opciones);
        }

        public void Update(Opcion nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}