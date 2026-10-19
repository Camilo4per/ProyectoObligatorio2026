using Dominio.Entidades;
using System.Collections.Generic;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioAuditorias : IRepository<Auditoria>
    {
        List<Auditoria> Buscar(int? idHistoria, int? idUsuario);
    }
}
