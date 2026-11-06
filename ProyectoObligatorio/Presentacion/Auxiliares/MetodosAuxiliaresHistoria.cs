using DTOs.DTOs;

namespace Presentacion.Auxiliares
{
    public static class MetodosAuxiliaresHistoria
    {
        public static string ArbolCapitulosAuxiliar(HistoriaDTO historia)
        {
            // Guarda los capítulos de la rama actual.
            List<int> capitulosVisitados = new List<int>();

            // Comienza el árbol desde el capítulo inicial.
            return MostrarCapitulo(historia.CapituloInicial, historia.Capitulos, 0, capitulosVisitados);
        }

        private static string MostrarCapitulo(CapituloDTO capitulo, List<CapituloDTO> capitulos, int nivel, List<int> capitulosVisitados)
        {
            // Construye el texto del capítulo actual.
            string arbol = "";
            string sufijo = "";
            bool yaVisitado = capitulo.Id > 0 && capitulosVisitados.Contains(capitulo.Id);

            if (yaVisitado)
            {
                sufijo = " (ya visitado)";
            }

            arbol += Espaciar(nivel) + capitulo.Titulo + sufijo + "\n";

            if (yaVisitado || capitulo.Tipo == 1)
            {
                // Detiene la rama si es un final o si ya fue visitado.
                if (capitulo.Tipo == 1 && !yaVisitado)
                {
                    arbol += Espaciar(nivel + 1) + "(Fin)" + "\n";
                }

                return arbol;
            }

            if (capitulo.Id > 0)
            {
                // Marca el capítulo para evitar ciclos.
                capitulosVisitados.Add(capitulo.Id);
            }

            if (capitulo.Opciones != null)
            {
                foreach (OpcionDTO opcion in capitulo.Opciones)
                {
                    // Muestra la decisión y busca su capítulo destino.
                    arbol += Espaciar(nivel + 1) + "Opción: " + opcion.Texto + "\n";

                    CapituloDTO? destino = opcion.CapDestino;
                    arbol += MostrarCapitulo(destino, capitulos, nivel + 2, capitulosVisitados);
                }
            }

            return arbol;
        }

        private static string Espaciar(int nivel)
        {
            // Genera cuatro espacios por nivel.
            return new string(' ', nivel * 4);
        }
    }
}
