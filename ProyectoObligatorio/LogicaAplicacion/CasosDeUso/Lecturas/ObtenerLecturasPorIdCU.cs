using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Dominio.InterfaceDeRepositorio;
using Dominio.Entidades;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerLecturasPorIdCU : IObtenerLecturasPorId
    {
        private IRepositorioLecturas repositorio;
        public ObtenerLecturasPorIdCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }
        public LecturaDTO Ejecutar(int id)
        {
            Lectura lectura = repositorio.FindById(id);
            return LecturaMapper.ToDTO(lectura);
        }
    }
}
