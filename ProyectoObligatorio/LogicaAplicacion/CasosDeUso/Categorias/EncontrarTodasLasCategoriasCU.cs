using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

namespace LogicaAplicacion.CasosDeUso.Categorias
{
    public class EncontrarTodasLasCategoriasCU : IEncontrarTodasCategorias
    {
        private IRepositorioCategorias repositorio;
        public EncontrarTodasLasCategoriasCU(IRepositorioCategorias repo)
        {
            repositorio = repo;
        }

        public List<CategoriaDTO> Ejecutar()
        {
            List<Categoria> categorias = repositorio.FindAll();
            List<CategoriaDTO> aRetornar = new List<CategoriaDTO>();
            foreach (Categoria categoria in categorias)
            {
                aRetornar.Add(CategoriaMapper.ToDTO(categoria));
            }
            return aRetornar;
        }
    }
}