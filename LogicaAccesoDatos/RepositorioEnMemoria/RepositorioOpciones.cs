using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioOpciones : IRepositorioOpciones
    {
        private static List<Opcion> opciones = new List<Opcion>();

        public void Add(Opcion nuevo)
        {
            if (nuevo == null) { throw new OpcionException("La opcion esta vacia"); }
            nuevo.Validar();
            opciones.Add(nuevo);
        }

        public Opcion FindById(int id)
        {
            throw new OpcionException("No requiere implementacion");
        }

        public List<Opcion> FindAll()
        {
            throw new OpcionException("No requiere implementacion");
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