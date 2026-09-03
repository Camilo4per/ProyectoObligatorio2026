using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioCategorias : IRepository<Categoria>
    {
        public void ExisteCategoria(Categoria aVerificar);
    }
}