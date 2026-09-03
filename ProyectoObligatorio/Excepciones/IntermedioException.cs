using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Excepciones
{
    public class IntermedioException : Exception
    {
        public IntermedioException() { }
        public IntermedioException(string message) : base(message) { }
        public IntermedioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
