using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerRankingUsuarios
    {
        List<UsuarioRankingDTO> Ejecutar();
    }
}
