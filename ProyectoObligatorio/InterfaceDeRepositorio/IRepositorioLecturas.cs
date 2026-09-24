using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioLecturas : IRepository<Lectura>
    {
        public void ExisteLectura(Lectura aVerificar);

        bool TieneLecturasDeHistoria(int idHistoria);
    }
}