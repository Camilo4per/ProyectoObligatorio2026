using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Excepciones
{
    public class FinalException : Exception
    {
        public FinalException() { }
        public FinalException(string message) : base(message) { }
        public FinalException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
