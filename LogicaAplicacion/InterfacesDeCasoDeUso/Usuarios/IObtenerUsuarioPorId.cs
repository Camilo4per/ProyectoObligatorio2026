using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IObtenerUsuarioPorId
    {
        public UsuarioDTO Ejecutar(int id);
    }
}
