using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Historia;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Historia : IValidable
    {
        #region Atributos
        public int Id { get; set; }

        public List<Categoria> Categorias { get; set; }

        [ForeignKey(nameof(CapituloInicial))] public int? idCapituloInicial { get; set; }

        public Capitulo CapituloInicial { get; set; }

        public List<Capitulo> Capitulos { get; set; }
        
        public EstadoHistoria Estado { get; set; }

        public TituloHistoria Titulo { get; set; }
        
        public Sinopsis Sinopsis { get; set; }

        #endregion

        #region Validaciones
        public void Validar()
        {
            Titulo.Validar();
            Sinopsis.Validar();
            ValidarCategorias();
            ValidarCapituloInicial();
            ValidarCapitulos();
        }

        public void ValidarCategorias()
        {
            if (Categorias == null || Categorias.Count == 0)
                throw new HistoriaException("La historia debe tener al menos una categoría");

            // Valido que todas las categorías sean válidas
            foreach (Categoria categoria in Categorias) categoria.Validar();
        }

        public void ValidarCapituloInicial()
        {
            CapituloInicial.Validar();

            if (CapituloInicial is Final)
                throw new HistoriaException("El capítulo inicial de una historia no puede ser de tipo final");
        }

        public void ValidarCapitulos()
        {
            if (Capitulos == null || Capitulos.Count == 0)
                throw new HistoriaException("La historia debe tener al menos un capítulo");

            // Valido que todos los capitulos sean válidos
            foreach (Capitulo cap in Capitulos) cap.Validar();

            if (!Capitulos.Contains(CapituloInicial))
                throw new HistoriaException("El capítulo inicial debe estar en la lista de capítulos");
        }

        #endregion
    }
}
