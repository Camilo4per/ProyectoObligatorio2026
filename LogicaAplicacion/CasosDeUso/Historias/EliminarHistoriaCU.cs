using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class EliminarHistoriaCU : IEliminarHistoria
    {
        private IRepositorioHistorias repositorioHistorias;
        private IRepositorioLecturas repositorioLecturas;
        public EliminarHistoriaCU(IRepositorioHistorias repoHistorias, IRepositorioLecturas repoLecturas)
        {
            repositorioHistorias = repoHistorias;
            repositorioLecturas = repoLecturas;
        }

        public void Ejecutar(int id)
        {
            // Valida que la historia exista (mensaje descriptivo si no).
            repositorioHistorias.FindById(id);

            // Regla de negocio: una historia con lecturas asociadas no se puede dar de baja.
            if (repositorioLecturas.TieneLecturasDeHistoria(id))
                throw new HistoriaException(
                    "No se puede eliminar la historia porque tiene lecturas asociadas. " +
                    "Debe eliminar las lecturas antes de darla de baja.");

            repositorioHistorias.Remove(id);
        }
    }
}