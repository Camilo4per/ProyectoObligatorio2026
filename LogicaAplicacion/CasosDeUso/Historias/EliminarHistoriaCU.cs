using Dominio.InterfaceDeRepositorio;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class EliminarHistoriaCU : IEliminarHistoria
    {
        private IRepositorioHistorias repositorio;
        public EliminarHistoriaCU(IRepositorioHistorias repo)
        {
            repositorio = repo;
        }

        public void Ejecutar(int id)
        {
            repositorio.Remove(id);
        }
    }
}