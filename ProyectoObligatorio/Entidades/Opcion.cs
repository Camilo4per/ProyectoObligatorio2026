using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.Entidades
{
    [Owned]
    public class Opcion : IValidable
    {
        // Capitulo al que pertenece la opción
        public int? idCapitulo { get; set; }  //FK
        public int? idCapDestino { get; set; } // FK
        public Capitulo? CapDestino { get; set; }
        public string Texto { get; set; }

        public void Validar()
        {
            if (Texto == null) throw new OpcionException("Texto requerido");
            if (CapDestino != null) CapDestino.Validar();
            if (string.IsNullOrWhiteSpace(Texto)) throw new OpcionException("Texto requerido");
            if (Texto.Length < 3) throw new OpcionException("Texto debe tener al menos 3 caracteres");
            if (Texto.Length > 2000) throw new OpcionException("Texto no puede exceder los 2000 caracteres");
            
        }
    }
}
