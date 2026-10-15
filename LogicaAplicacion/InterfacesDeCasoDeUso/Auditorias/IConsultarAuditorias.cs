using DTOs.DTOs;
using System.Collections.Generic;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias
{
    public interface IConsultarAuditorias
    {
        List<AuditoriaDTO> Ejecutar(int? idHistoria, int? idUsuario);
    }
}
