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
            IQueryable<Auditoria> query = _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Historia);

            if (idHistoria.HasValue && idHistoria.Value > 0)
            {
                query = query.Where(a => a.idHistoria == idHistoria.Value);
            }

            if (idUsuario.HasValue && idUsuario.Value > 0)
            {
                query = query.Where(a => a.idUsuario == idUsuario.Value);
            }

            return query.OrderByDescending(a => a.Fecha).ToList();
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
