using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerHistorialLecturasCU : IObtenerHistorialLecturas
    {
        private IRepositorioLecturas repositorio;

        public ObtenerHistorialLecturasCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }

        public List<HistorialLecturaDTO> Ejecutar(
            int idUsuario,
            int mes,
            int anio)
        {
            if (idUsuario <= 0)
                throw new LecturaException("El usuario no es válido.");

            if (mes < 1 || mes > 12)
                throw new LecturaException("El mes debe estar entre 1 y 12.");

            if (anio < 1)
                throw new LecturaException("El año no es válido.");

            List<Lectura> lecturas = repositorio
                .FindByUsuarioAndMonth(idUsuario, mes, anio);

            List<HistorialLecturaDTO> resultado = new List<HistorialLecturaDTO>();

            foreach (Lectura lectura in lecturas)
            {
                resultado.Add(new HistorialLecturaDTO
                {
                    IdLectura = lectura.Id,
                    Historia = lectura.Historia?.Titulo?.Valor
                        ?? "Historia sin título",
                    FechaInicio = lectura.FechaInicio,
                    FechaFin = lectura.FechaFin,
                    Completada = lectura.FechaFin.HasValue,
                    FinalAlcanzado = lectura.FinalAlcanzado?.Titulo?.Valor
                });
            }

            return resultado;
        }
    }
}
