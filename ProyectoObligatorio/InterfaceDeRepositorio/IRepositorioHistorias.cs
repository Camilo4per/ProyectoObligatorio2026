using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioHistorias : IRepository<Historia>
    {
        public void ExisteHistoria(Historia aVerificar);
    }
}