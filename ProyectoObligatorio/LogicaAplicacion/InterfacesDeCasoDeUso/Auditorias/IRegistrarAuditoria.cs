namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias
{
    public interface IRegistrarAuditoria
    {
        void Ejecutar(string accion, int idHistoria, int idUsuarioAdmin);
    }
}
