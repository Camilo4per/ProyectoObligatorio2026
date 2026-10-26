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

            // Cuando la historia está en estado Publicada, se valida que cumpla con los requisitos para ser publicada.
            if (Estado == EstadoHistoria.Publicada)
            {
                ValidarPublicacion();
            }
        }

        public void ValidarPublicacion()
        {
            if (Capitulos == null || Capitulos.Count == 0)
                throw new HistoriaException("La historia debe tener al menos un capítulo para ser publicada");

            // Valido que haya al menos un capítulo final.
            bool tieneFinal = Capitulos.Any(c => c is Final);
            if (!tieneFinal)
                throw new HistoriaException("La historia debe tener al menos un capítulo final para poder ser publicada");

            // Valido que todos los capítulos intermedios tengan al menos una opción y que todas las opciones conduzcan a un capítulo existente en la historia.
            foreach (Capitulo cap in Capitulos)
            {
                if (cap is Intermedio inter)
                {
                    if (inter.Opciones == null || inter.Opciones.Count == 0)
                    {
                        throw new HistoriaException(
                            $"El capítulo '{inter.Titulo?.Valor ?? inter.Id.ToString()}' debe tener al menos una opción para poder publicar la historia.");
                    }

                    foreach (Opcion op in inter.Opciones)
                    {
                        int? destinoId = op.idCapDestino;
                        if (destinoId == null) destinoId = op.CapDestino?.Id;

                        Capitulo? destino = Capitulos
                            .FirstOrDefault(c =>
                            (destinoId != null && destinoId != 0 && c.Id == destinoId) ||
                            (op.CapDestino != null && c == op.CapDestino)
                        );
                        
                        if (destino == null)
                        {
                            throw new HistoriaException(
                                $"La opción '{op.Texto}' del capítulo '{inter.Titulo?.Valor ?? inter.Id.ToString()}' no conduce a un capítulo existente en la historia.");
                        }

                        if (destino == inter || (inter.Id != 0 && destino.Id != 0 && destino.Id == inter.Id))
                        {
                            throw new HistoriaException(
                                $"La opción '{op.Texto}' del capítulo '{inter.Titulo?.Valor ?? inter.Id.ToString()}' debe conducir a otro capítulo.");
                        }

                        if (!(destino is Intermedio || destino is Final))
                        {
                            throw new HistoriaException(
                                $"La opción '{op.Texto}' del capítulo '{inter.Titulo?.Valor ?? inter.Id.ToString()}' debe conducir a un capítulo intermedio o final.");
                        }
                    }
                }
            }
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
            if (CapituloInicial == null)
                throw new HistoriaException("El capítulo inicial de una historia no puede ser nulo");
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

        // Resolución de capítulos del agregado: la usan los casos de uso de lectura
        // para no repetir búsquedas sobre la colección.
        public Capitulo? ObtenerCapitulo(int idCapitulo)
        {
            return Capitulos.FirstOrDefault(c => c.Id == idCapitulo);
        }

        // Una lectura sólo puede avanzar de un intermedio a un capítulo alcanzable
        // mediante una de sus opciones.
        public bool TieneOpcion(int idCapituloOrigen, int idCapDestino)
        {
            if (ObtenerCapitulo(idCapituloOrigen) is not Intermedio intermedio) return false;
            return intermedio.Opciones != null &&
                   intermedio.Opciones.Any(o => o.idCapDestino == idCapDestino);
        }

        #endregion
    }
}
