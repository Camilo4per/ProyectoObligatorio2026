using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Historia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Historia : IValidable
    {
        public int Id { get; }
        public List<Categoria> Categorias { get; set; }
        public Capitulo Capitulo { get; set; }
        public EstadoHistoria Estado { get; set; }

        // Value Objects
        public Titulo Titulo { get; init; }
        public Sinopsis Sinopsis { get; init; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
