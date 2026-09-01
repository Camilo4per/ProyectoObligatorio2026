using DTOs.DTOs;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuario
{
    public interface IAgregarUsuario
    {
        public void Ejecutar(UsuarioDTO dto);
    }
}
