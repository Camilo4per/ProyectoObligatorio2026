using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IEncontrarTodosUsuarios
    {
        public List<UsuarioDTO> Ejecutar();
    }
}
