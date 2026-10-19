using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioHistorias : IRepositorioHistorias
    {
        private static List<Historia> historias = new List<Historia>();

        public void Add(Historia nuevo)
        {
            if (nuevo == null) { throw new HistoriaException("La historia esta vacia"); }
            nuevo.Validar();
            ExisteHistoria(nuevo);
            historias.Add(nuevo);
        }

        public void ExisteHistoria(Historia aVerificar)
        {
            foreach (Historia item in historias)
            {
                if (item.Id == aVerificar.Id)
                {
                    throw new HistoriaException("Historia ya existente");
                }
            }
        }

        public Historia FindById(int id)
        {
            foreach (Historia historia in historias)
            {
                if (historia.Id == id) return historia;
            }

            throw new HistoriaException("Historia inexistente");
        }

        public List<Historia> FindAll()
        {
            return new List<Historia>(historias);
        }

        public void Update(Historia nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}