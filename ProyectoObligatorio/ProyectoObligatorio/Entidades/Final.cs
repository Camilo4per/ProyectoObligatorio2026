using Dominio.Excepciones;
using Dominio.ValueObjects.Final;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class Final : Capitulo
    {
        #region Atributos
        public CantLlegaron CantLlegaron { get; set; }
        #endregion
        public override void Validar()
        {
            base.Validar();
            if (CantLlegaron == null) throw new FinalException("El valor de CantLlegaron no puede ser nulo.");
            CantLlegaron.Validar();
        }

        // Contabiliza un lector que alcanzó este final.
        public void RegistrarLlegada()
        {
            if (CantLlegaron == null)
                throw new FinalException("El valor de CantLlegaron no puede ser nulo.");

            CantLlegaron = new CantLlegaron(CantLlegaron.Valor + 1);
        }
    }
}
