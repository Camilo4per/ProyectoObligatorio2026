
namespace Dominio.Excepciones
{
    public class HistoriaException : Exception
    {
        public HistoriaException() { }
        public HistoriaException(string message) : base(message) { }
        public HistoriaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
