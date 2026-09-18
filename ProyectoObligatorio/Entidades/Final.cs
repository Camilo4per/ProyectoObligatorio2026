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
            CantLlegaron.Validar();
        }
    }
}
