
namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepository<T> where T : class
    {
        public void Add(T nuevo);
        public void Update(T nuevo);
        public void Remove(int id);
        public T FindById(int id);
        public List<T> FindAll();
    }
}
