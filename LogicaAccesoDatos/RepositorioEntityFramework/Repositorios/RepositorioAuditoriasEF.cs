using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioAuditoriasEF : IRepositorioAuditorias
    {
        private DominioContext _context;

        public RepositorioAuditoriasEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Auditoria nuevo)
        {
            if (nuevo == null)
            {
                throw new AuditoriaException("La auditoría está vacía.");
            }

            nuevo.Validar();
            _context.Auditorias.Add(nuevo);
            _context.SaveChanges();
        }

        public List<Auditoria> Buscar(int? idHistoria, int? idUsuario)
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Historia)
                .Where(a => (idHistoria == null || idHistoria <= 0 || a.idHistoria == idHistoria)
                         && (idUsuario == null || idUsuario <= 0 || a.idUsuario == idUsuario))
                .OrderByDescending(a => a.Fecha)
                .ToList();
        }

        public List<Auditoria> FindAll()
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Historia)
                .OrderByDescending(a => a.Fecha)
                .ToList();
        }

        public Auditoria FindById(int id)
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Historia)
                .FirstOrDefault(a => a.Id == id)
                ?? throw new AuditoriaException("Auditoría inexistente");
        }

        public void Remove(int id)
        {
            throw new NotSupportedException("Los registros de auditoría no pueden ser eliminados.");
        }

        public void Update(Auditoria nuevo)
        {
            throw new NotSupportedException("Los registros de auditoría no pueden ser modificados.");
        }
    }
}
