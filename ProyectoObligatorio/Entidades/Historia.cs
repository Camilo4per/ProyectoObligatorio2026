using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Historia;

namespace Dominio.Entidades
{
    public class Historia : IValidable
    {
        public int Id { get; set; }
        public List<Categoria> Categorias { get; set; }
        public Capitulo CapituloInicial { get; set; }
        public List<Capitulo> Capitulos { get; set; }
        public EstadoHistoria Estado { get; set; }

        // Value Objects
        public TituloHistoria Titulo { get; init; }
        public Sinopsis Sinopsis { get; init; }

        public void Validar()
        {
            if (Titulo == null || Sinopsis == null)
                throw new HistoriaException("La historia debe tener título y sinopsis");
            if (Categorias == null || Categorias.Count == 0)
                throw new HistoriaException("La historia debe tener al menos una categoría");
            if (CapituloInicial == null)
                throw new HistoriaException("La historia debe tener un capítulo");
            if (CapituloInicial is Final)
                throw new HistoriaException("El capítulo inicial de una historia no puede ser de tipo final");
            if (Capitulos == null || Capitulos.Count == 0)
                throw new HistoriaException("La historia debe tener al menos un capítulo");
            if (!Capitulos.Contains(CapituloInicial))
                throw new HistoriaException("El capítulo inicial debe estar en la lista de capítulos");

            Titulo.Validar();
            Sinopsis.Validar();

            // Valido que todas las categorías sean válidas
            foreach (Categoria categoria in Categorias)
                categoria.Validar();

            // Valido que todas las opciones de los capítulos intermedios tengan un destino válido
            foreach (Capitulo cap in Capitulos)
            {
                if (cap is Intermedio intermedio)
                    foreach (Opcion opcion in intermedio.Opciones)
                    {
                        if (opcion.CapDestino == null)
                            throw new HistoriaException("Toda opción debe llevar a otro capítulo");
                        if (!Capitulos.Contains(opcion.CapDestino))
                            throw new HistoriaException("Los destinos de una opción deben pertenecer a la historia");
                    }
            }
        }
    }
}
