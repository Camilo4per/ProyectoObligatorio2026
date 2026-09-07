using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerLecturaPorIdCU : IObtenerLecturaPorId
    {
        private IRepositorioLecturas repositorio;

        public ObtenerLecturaPorIdCU(IRepositorioLecturas repo)
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