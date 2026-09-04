using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Dominio.InterfaceDeRepositorio;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class ObtenerUsuarioPorIdCU : IObtenerUsuarioPorId
    {
        private IRepositorioUsuarios repositorio;
        public ObtenerUsuarioPorIdCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }
        public UsuarioDTO Ejecutar(int id)
        {
            Dominio.Entidades.Usuario usuario = repositorio.FindById(id);
            return UsuarioMapper.ToDTO(usuario);
        }
    }
}
