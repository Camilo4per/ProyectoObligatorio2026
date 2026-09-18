using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.Entidades
{
    [Owned]
    public class Opcion : IValidable
    {
        #region Atributos
        public int? idCapitulo { get; set; }  //FK
        public int? idCapDestino { get; set; } // FK
        public Capitulo? CapDestino { get; set; }
        public string Texto { get; set; }

        #endregion

        #region Validaciones
        public void Validar()
        {
            ValidarCapDestino();
            ValidarTexto();
        }

        public void ValidarCapDestino()
        {
            if (CapDestino != null) CapDestino.Validar();
        }

        public void ValidarTexto()
        {
            if (Texto == null) throw new OpcionException("Texto requerido");
            if (string.IsNullOrWhiteSpace(Texto)) throw new OpcionException("Texto requerido");
            if (Texto.Length < 3) throw new OpcionException("Texto debe tener al menos 3 caracteres");
            if (Texto.Length > 2000) throw new OpcionException("Texto no puede exceder los 2000 caracteres");
        }

        #endregion
    }
}
