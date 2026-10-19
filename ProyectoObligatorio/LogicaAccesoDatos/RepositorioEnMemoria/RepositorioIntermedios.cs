using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioIntermedios : IRepositorioIntermedios
    {
        private static List<Intermedio> intermedios = new List<Intermedio>();

        public void Add(Intermedio nuevo)
        {
            if (nuevo == null) { throw new IntermedioException("El capitulo intermedio esta vacio"); }
            nuevo.Validar();
            ExisteIntermedio(nuevo);
            intermedios.Add(nuevo);
        }

        public void ExisteIntermedio(Intermedio aVerificar)
        {
            foreach (Intermedio item in intermedios)
            {
                if (item.Id == aVerificar.Id)
                {
                    throw new IntermedioException("Capitulo intermedio ya existente");
                }
            }
        }

        public Intermedio FindById(int id)
        {
            foreach (Intermedio intermedio in intermedios)
            {
                if (intermedio.Id == id) return intermedio;
            }

            throw new IntermedioException("Capitulo intermedio inexistente");
        }

        public List<Intermedio> FindAll()
        {
            return new List<Intermedio>(intermedios);
        }

        public void Update(Intermedio nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}