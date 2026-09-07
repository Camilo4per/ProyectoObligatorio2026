using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class AgregarLecturaCU : IAgregarLectura
    {
        private IRepositorioLecturas repositorioLecturas;

        public AgregarLecturaCU(IRepositorioLecturas repositorioLecturas)
        {
            this.repositorioLecturas = repositorioLecturas;
        }

        public void Ejecutar(LecturaDTO dto)
        {
            repositorioLecturas.Add(LecturaMapper.FromDTO(dto));
        }
    }
}