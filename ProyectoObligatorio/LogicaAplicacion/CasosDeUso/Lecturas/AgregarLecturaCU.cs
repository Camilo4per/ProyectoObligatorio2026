using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class AgregarLecturaCU : IAgregarLectura
    {
        private IRepositorioLecturas repositorio;
        public AgregarLecturaCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }
        public void Ejecutar(LecturaDTO dto)
        {
            repositorio.Add(LecturaMapper.FromDTO(dto));
        }
    }
}
