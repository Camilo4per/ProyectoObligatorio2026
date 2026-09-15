using Dominio.Excepciones;

namespace Dominio.Entidades
{
    public class Intermedio : Capitulo
    {
        public List<Opcion> Opciones { get; set; }

        public override void Validar()
        {
            base.Validar();
            if (Opciones == null || Opciones.Count == 0)
                throw new IntermedioException("Un capítulo intermedio debe tener al menos una opción");

            // Valido cada opción
            foreach (Opcion opcion in Opciones)
                opcion.Validar();
        }
    }
}
