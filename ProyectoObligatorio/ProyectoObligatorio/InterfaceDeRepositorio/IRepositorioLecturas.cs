using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioLecturas : IRepository<Lectura>
    {
        public void ExisteLectura(Lectura aVerificar);

        bool TieneLecturasDeHistoria(int idHistoria);

        List<Lectura> FindAllCompletadas();

        List<Lectura> FindByUsuarioAndMonth(int idUsuario, int mes, int anio);
    }
}