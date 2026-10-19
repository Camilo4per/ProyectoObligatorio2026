using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Dominio.Entidades;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class EncontrarTodasLasLecturasCU : IEncontrarTodasLasLecturas
    {
        private IRepositorioLecturas repositorio;
        public EncontrarTodasLasLecturasCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }

        public List<LecturaDTO> Ejecutar()
        {
            List<Lectura> lecturas = repositorio.FindAll();
            List<LecturaDTO> aRetornar = new List<LecturaDTO>();
            foreach (Lectura lectura in lecturas)
            {
                aRetornar.Add(LecturaMapper.ToDTO(lectura));
            }
            return aRetornar;
        }
    }
}
