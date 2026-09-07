namespace Dominio.Excepciones
{
    public class LecturaException : Exception
    {
        public LecturaException() { }
        public LecturaException(string message) : base(message) { }
        public LecturaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}