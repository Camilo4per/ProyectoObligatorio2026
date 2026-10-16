using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Auditoria : IValidable
    {
        #region Atributos
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Accion { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int? idUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(Historia))]
        public int? idHistoria { get; set; }
        public Historia? Historia { get; set; }
        #endregion

        #region Validaciones
        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Accion))
            {
                throw new AuditoriaException("La acción de auditoría no puede estar vacía.");
            }

            if (Fecha == default) // TODO: Cambiar
            {
                throw new AuditoriaException("La fecha de auditoría no es válida.");
            }

            if (!idUsuario.HasValue && Usuario == null)
            {
                throw new AuditoriaException("El usuario administrador es requerido.");
            }

            if (!idHistoria.HasValue && Historia == null)
            {
                throw new AuditoriaException("La historia es requerida.");
            }
        }
        #endregion
    }
}
