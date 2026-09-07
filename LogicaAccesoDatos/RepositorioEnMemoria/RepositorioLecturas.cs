using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioLecturas : IRepositorioLecturas
    {
        private static List<Lectura> lecturas = new List<Lectura>();

        public void Add(Lectura nueva)
        {
            if (nueva == null) { throw new LecturaException("La lectura esta vacia"); }
            nueva.Validar();
            ExisteLectura(nueva);
            lecturas.Add(nueva);
        }

        public void ExisteLectura(Lectura aVerificar)
        {
            foreach (Lectura item in lecturas)
            {
                if (item.Usuario.Id == aVerificar.Usuario.Id &&
                    item.Historia.Id == aVerificar.Historia.Id &&
                    item.FechaInicio == aVerificar.FechaInicio)
                {
                    throw new LecturaException("Lectura ya existente");
                }
            }
        }

        public Lectura FindById(int id)
        {
            foreach (Lectura lectura in lecturas)
            {
                if (lectura.Id == id) return lectura;
            }

            throw new LecturaException("Lectura inexistente");
        }

        public List<Lectura> FindAll()
        {
            return new List<Lectura>(lecturas);
        }

        public void Update(Lectura nueva)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}