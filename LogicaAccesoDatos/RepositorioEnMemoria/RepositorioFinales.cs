using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioFinales : IRepositorioFinales
    {
        private static List<Final> finales = new List<Final>();

        public void Add(Final nuevo)
        {
            if (nuevo == null) { throw new FinalException("El capitulo final esta vacio"); }
            nuevo.Validar();
            ExisteFinal(nuevo);
            finales.Add(nuevo);
        }

        public void ExisteFinal(Final aVerificar)
        {
            foreach (Final item in finales)
            {
                if (item.Id == aVerificar.Id)
                {
                    throw new FinalException("Capitulo final ya existente");
                }
            }
        }

        public Final FindById(int id)
        {
            foreach (Final final in finales)
            {
                if (final.Id == id) return final;
            }

            throw new FinalException("Capitulo final inexistente");
        }

        public List<Final> FindAll()
        {
            return new List<Final>(finales);
        }

        public void Update(Final nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}