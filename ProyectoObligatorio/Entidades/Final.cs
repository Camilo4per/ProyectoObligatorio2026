using Dominio.Excepciones;
using Dominio.ValueObjects.Final;

namespace Dominio.Entidades
{
    public class Final : Capitulo
    {
        public CantLlegaron CantLlegaron { get; set; }

        public override void Validar()
        {
            base.Validar();
            if (CantLlegaron == null)
                throw new FinalException("CantLlegaron requerido");
            CantLlegaron.Validar();
        }
    }
}
