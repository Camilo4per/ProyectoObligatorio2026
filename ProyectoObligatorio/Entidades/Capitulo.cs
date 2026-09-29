using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public abstract class Capitulo : IValidable
    {
        #region Atributos
        public int Id { get; set; }

        public TituloCapitulo Titulo { get; set; }

        public TextoCapitulo Texto { get; set; }

        [ForeignKey(nameof(Historia))] public int? idHistoria { get; set; }
        #endregion

        public virtual void Validar()
        {
            if(Titulo == null) throw new UsuarioException("El título del capítulo no puede ser nulo.");
            Titulo.Validar();
            if(Texto == null) throw new UsuarioException("El texto del capítulo no puede ser nulo.");
            Texto.Validar();
        }
    }
}
