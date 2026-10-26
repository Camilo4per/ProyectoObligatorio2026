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
        public DateTime? FechaFin { get; set; }
        [ForeignKey(nameof(FinalAlcanzado))] public int? idFinal { get; set; }
        public Final? FinalAlcanzado { get; set; }

        #endregion

        #region Validaciones
        public void Validar()
        {
            if (Usuario == null) throw new LecturaException("El usuario no puede ser nulo");
            Usuario.Validar();
            if (Historia == null) throw new LecturaException("La historia no puede ser nula");
            Historia.Validar();
            ValidarFechaInicio();
            ValidarFechaFin();
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

        // Regla de negocio: una lectura ya finalizada es inmutable.
        public void ValidarModificacion()
        {
            if (FechaFin != null)
                throw new LecturaException("La lectura ya fue finalizada y no se puede modificar.");
        }

        // Registra el desenlace alcanzado. Centraliza la regla de que el final debe
        // pertenecer a la historia de la lectura y de que sólo se finaliza una vez.
        public void RegistrarFinal(Final final)
        {
            if (final == null)
                throw new LecturaException("El final alcanzado es requerido");

            ValidarModificacion();

            if (Historia == null)
                throw new LecturaException("La lectura no tiene una historia asociada.");
            if (final.idHistoria != Historia.Id)
                throw new LecturaException("El final alcanzado no pertenece a la historia de la lectura.");

            FinalAlcanzado = final;
            idFinal = final.Id;
            FechaFin = DateTime.Now;
            ValidarFechaFin();
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
