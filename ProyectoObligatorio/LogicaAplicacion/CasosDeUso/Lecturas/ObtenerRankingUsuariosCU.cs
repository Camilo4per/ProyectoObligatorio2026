using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerRankingUsuariosCU : IObtenerRankingUsuarios
    {
        private IRepositorioLecturas repositorio;

        public ObtenerRankingUsuariosCU(IRepositorioLecturas repositorioCu)
        {
            repositorio = repositorioCu;
        }

        public List<UsuarioRankingDTO> Ejecutar()
        {
            List<Lectura> lecturasCompletadas =
                repositorio.FindAllCompletadas();

            return lecturasCompletadas
                .Where(lectura =>
                    lectura.Usuario != null &&
                    lectura.idUsuario.HasValue &&
                    lectura.idHistoria.HasValue)
                .GroupBy(lectura => new
                {
                    IdUsuario = lectura.idUsuario!.Value,
                    NombreUsuario = lectura.Usuario.NombreUsuario
                })
                .Select(grupo => new UsuarioRankingDTO
                {
                    IdUsuario = grupo.Key.IdUsuario,
                    NombreUsuario = grupo.Key.NombreUsuario,
                    CantidadHistoriasCompletadas = grupo
                        .Select(lectura => lectura.idHistoria!.Value)
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(usuario =>
                    usuario.CantidadHistoriasCompletadas)
                .ThenBy(usuario => usuario.NombreUsuario)
                .ToList();
        }
    }
}
