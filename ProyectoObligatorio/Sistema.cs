using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Sistema
    {
        public List<Entidades.Categoria> Categorias 
        { 
            get { return new List<Entidades.Categoria>(Categorias); } 
            private set; 
        }
        public List<Entidades.Capitulo> Capitulos
        {
            get { return new List<Entidades.Capitulo>(Capitulos); }
            private set;
        }
        public List<Entidades.Usuario> Usuarios
        {
            get { return new List<Entidades.Usuario>(Usuarios); }
            private set;
        }
        public List<Entidades.Historia> Historias
        {
            get { return new List<Entidades.Historia>(Historias); }
            private set;
        }

        
    }
}
