using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Dominio.InterfaceDeRepositorio;
using Dominio.Entidades;
using Dominio.Excepciones;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerLecturasPorIdCU : IObtenerLecturasPorId
    {
        private IRepositorioLecturas repositorio;
        public ObtenerLecturasPorIdCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }
        public LecturaDTO Ejecutar(int id, int idUsuario, bool esAdmin)
        {
            Lectura lectura = repositorio.FindById(id);

            // El dueño de una lectura (o un administrador) es el único que puede consultarla.
            if (!esAdmin && lectura.idUsuario != idUsuario)
                throw new LecturaException("La lectura no pertenece al usuario autenticado.");

            return LecturaMapper.ToDTO(lectura);
        }
    }
}
