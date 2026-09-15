using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public abstract class Capitulo : IValidable
    {
        public int Id { get; set; }

        // Value Objects
        public TituloCapitulo Titulo { get; set; }
        public TextoCapitulo Texto { get; set; }
        [ForeignKey(nameof(Historia))] public int? idHistoria { get; set; }

        public virtual void Validar()
        {
            if (Titulo == null)
                throw new CapituloException("Título requerido");
            if (Texto == null)
                throw new CapituloException("Texto requerido");
            Titulo.Validar();
            Texto.Validar();
        }
    }
}
