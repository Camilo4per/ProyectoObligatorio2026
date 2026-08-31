using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Excepciones
{
    public class UsuarioException : Exception
    {
        public UsuarioException() { }
        public UsuarioException(string message) : base(message) { }
        public UsuarioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
