using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioIntermedios : IRepository<Intermedio>
    {
        public void ExisteIntermedio(Intermedio aVerificar);
    }
}