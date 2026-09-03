using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private static List<Categoria> categorias = new List<Categoria>();

        public void Add(Categoria nuevo)
        {
            if (nuevo == null) { throw new CategoriaException("La categoria esta vacia"); }
            nuevo.Validar();
            ExisteCategoria(nuevo);
            categorias.Add(nuevo);
        }

        public void ExisteCategoria(Categoria aVerificar)
        {
            foreach (Categoria item in categorias)
            {
                if (item.Nombre == aVerificar.Nombre)
                {
                    throw new CategoriaException("Categoria ya existente");
                }
            }
        }

        public Categoria FindById(int id)
        {
            foreach (Categoria categoria in categorias)
            {
                if (categoria.Id == id) return categoria;
            }

            throw new CategoriaException("Categoria inexistente");
        }

        public List<Categoria> FindAll()
        {
            return new List<Categoria>(categorias);
        }

        public void Update(Categoria nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}