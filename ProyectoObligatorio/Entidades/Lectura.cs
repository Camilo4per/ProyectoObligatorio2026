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
                throw new LecturaException("El usuario es requerido");
            if (Historia == null)
                throw new LecturaException("La historia es requerida");
            if (FechaInicio == default)
                throw new LecturaException("La fecha de inicio es requerida");
            if (FechaInicio > DateTime.Now)
                throw new LecturaException("La fecha de inicio no puede ser en el futuro");
            if (FechaFin != null && FechaFin > DateTime.Now)
                throw new LecturaException("La fecha de fin no puede ser en el futuro");
            if (FechaFin != null && FechaFin < FechaInicio)
                throw new LecturaException("La fecha de fin no puede ser anterior a la fecha de inicio");
            if (FechaFin != null && FinalAlcanzado == null)
                throw new LecturaException("Si la lectura tiene fecha de fin, debe tener un final alcanzado");
            if (FechaFin == null && FinalAlcanzado != null)
                throw new LecturaException("Si hay un final alcanzado, debe haber fecha de fin");

            FinalAlcanzado?.Validar();
        }
    }
}
