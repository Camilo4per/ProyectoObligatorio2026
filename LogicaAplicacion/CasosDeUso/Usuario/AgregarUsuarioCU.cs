using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuario;
using Dominio;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.CasosDeUso.Usuario
{
    public class AgregarUsuarioCU : IAgregarUsuario
    {
        private Sistema sistema;
        public AgregarUsuarioCU()
        {
            sistema = Sistema.Instancia;
        }
        public void Ejecutar(UsuarioDTO dto)
        {
            sistema.AgregarUsuario(UsuarioMapper.FromDTO(dto));
        }
    }
}
