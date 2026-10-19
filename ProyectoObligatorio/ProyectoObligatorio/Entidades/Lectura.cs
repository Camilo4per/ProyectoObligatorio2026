using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Lectura : IValidable
    {
        #region Atributos
        public int Id { get; set; }
        [ForeignKey(nameof(Usuario))] public int? idUsuario { get; set; }
        public Usuario Usuario { get; init; }
        [ForeignKey(nameof(Historia))] public int? idHistoria { get; set; }
        public Historia Historia { get; init; }
        public DateTime FechaInicio { get; init; }
        public DateTime? FechaFin { get; init; }
        [ForeignKey(nameof(FinalAlcanzado))] public int? idFinal { get; set; }
        public Final? FinalAlcanzado { get; init; }

        #endregion

        #region Validaciones
        public void Validar()
        {
            if (Usuario == null) throw new LecturaException("El usuario no puede ser nulo");
            Usuario.Validar();
            if (Historia == null) throw new LecturaException("La historia no puede ser nula");
            Historia.Validar();
            ValidarFechaInicio();
            if (FechaFin != null) ValidarFechaFin();
            if (FinalAlcanzado != null)
            {
                if (FinalAlcanzado is not Final)
                    throw new LecturaException("El capítulo alcanzado debe ser de tipo final");
                FinalAlcanzado.Validar();
            }
        }

        public void ValidarFechaInicio()
        {
            if (FechaInicio == default)
                throw new LecturaException("La fecha de inicio es requerida");
            if (FechaInicio > DateTime.Now)
                throw new LecturaException("La fecha de inicio no puede ser en el futuro");
        }

        public void ValidarFechaFin()
        {
            if (FechaFin != null && FechaFin > DateTime.Now)
                throw new LecturaException("La fecha de fin no puede ser en el futuro");
            if (FechaFin != null && FechaFin < FechaInicio)
                throw new LecturaException("La fecha de fin no puede ser anterior a la fecha de inicio");
            if (FechaFin != null && FinalAlcanzado == null)
                throw new LecturaException("Si la lectura tiene fecha de fin, debe tener un final alcanzado");
            if (FechaFin == null && FinalAlcanzado != null)
                throw new LecturaException("Si hay un final alcanzado, debe haber fecha de fin");
        }

        #endregion
    }
}
