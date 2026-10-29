using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioLecturas : IRepositorioLecturas
    {
        private static List<Lectura> lecturas = new List<Lectura>();

        public void Add(Lectura nueva)
        {
            if (nueva == null) { throw new LecturaException("La lectura esta vacia"); }
            nueva.Validar();
            ExisteLectura(nueva);
            lecturas.Add(nueva);
        }

        public void ExisteLectura(Lectura aVerificar)
        {
            foreach (Lectura item in lecturas)
            {
                if (item.Usuario.Id == aVerificar.Usuario.Id &&
                    item.Historia.Id == aVerificar.Historia.Id &&
                    item.FechaInicio == aVerificar.FechaInicio)
                {
                    throw new LecturaException("Lectura ya existente");
                }
            }
        }

        public bool TieneLecturasDeHistoria(int idHistoria)
        {
            foreach (Lectura item in lecturas)
            {
                if (item.idHistoria == idHistoria ||
                    (item.idFinal != null && item.FinalAlcanzado?.idHistoria == idHistoria))
                {
                    return true;
                }
            }

            return false;
        }

        public Lectura FindById(int id)
        {
            foreach (Lectura lectura in lecturas)
            {
                if (lectura.Id == id) return lectura;
            }

            throw new LecturaException("Lectura inexistente");
        }

        public List<Lectura> FindAll()
        {
            return new List<Lectura>(lecturas);
        }

        public List<Lectura> FindAllCompletadas()
        {
            List<Lectura> completadas = new List<Lectura>();
            foreach (Lectura item in lecturas)
            {
                if (item.FechaFin != null)
                {
                    completadas.Add(item);
                }
            }

            return completadas;
        }

        public List<Lectura> FindByUsuarioAndMonth(int idUsuario, int mes, int anio)
        {
            DateTime desde = new DateTime(anio, mes, 1);
            DateTime hasta = desde.AddMonths(1);

            return lecturas
                .Where(l => l.idUsuario == idUsuario &&
                            l.FechaInicio >= desde &&
                            l.FechaInicio < hasta)
                .OrderByDescending(l => l.FechaInicio)
                .ToList();
        }

        public void Update(Lectura nueva)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}