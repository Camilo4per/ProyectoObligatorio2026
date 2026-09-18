using Dominio.Excepciones;

namespace Dominio.Entidades
{
    public class Intermedio : Capitulo
    {
        #region Atributos
        public List<Opcion> Opciones { get; set; }

        #endregion

        #region Validaciones
        public override void Validar()
        {
            base.Validar();
            ValidarOpciones();
        }

        public void ValidarOpciones()
        {
            if (Opciones == null || Opciones.Count == 0)
                throw new IntermedioException("Un capítulo intermedio debe tener al menos una opción");

            // Valido cada opción
            foreach (Opcion opcion in Opciones)
                opcion.Validar();
        }

        #endregion
    }
}
