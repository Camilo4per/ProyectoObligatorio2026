using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Capitulos
{
    public interface IAgregarCapitulo
    {
        void Ejecutar(CapituloDTO nuevoCapituloDto, int idCapituloAntecedente, string textoOpcion);
    }
}
