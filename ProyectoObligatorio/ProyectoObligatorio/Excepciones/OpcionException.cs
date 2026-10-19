
namespace Dominio.Excepciones
{
    public class OpcionException : Exception
    {
        public OpcionException() { }
        public OpcionException(string message) : base(message) { }
        public OpcionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
