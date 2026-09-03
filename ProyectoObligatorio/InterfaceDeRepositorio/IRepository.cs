using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepository<T> where T : class
    {
        public void Add(T nuevo);
        public void Update(T nuevo);
        public void Remove(T nuevo);
        public void FindById(int id);
    }
}
