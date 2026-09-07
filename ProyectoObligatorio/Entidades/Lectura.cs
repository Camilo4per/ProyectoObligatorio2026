using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.Entidades
{
    public class Lectura : IValidable
    {
        public int Id { get; set; }
        public Usuario Usuario { get; init; }
        public Historia Historia { get; init; }
        public DateTime FechaInicio { get; init; }
        public DateTime? FechaFin { get; init; }
        public Final? FinalAlcanzado { get; init; }

        public void Validar()
        {
            if (Usuario == null)
                throw new LecturaException("La lectura debe tener un usuario");
            if (Historia == null)
                throw new LecturaException("La lectura debe tener una historia");
            if (FechaInicio > DateTime.Now)
                throw new LecturaException("La fecha de inicio no puede ser en el futuro");
            if (FechaFin != null && FechaFin > DateTime.Now)
                throw new LecturaException("La fecha de fin no puede ser en el futuro");
            if (FechaFin != null && FechaFin < FechaInicio)
                throw new LecturaException("La fecha de fin no puede ser anterior a la fecha de inicio");
        }
    }
}
