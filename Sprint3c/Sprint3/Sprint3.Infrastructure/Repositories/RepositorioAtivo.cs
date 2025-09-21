using System.Collections.Generic;
using System.Linq;
using Sprint3.Core.Interfaces;
using Sprint3.Core.Models;
using Sprint3.Infrastructure.Data;

namespace Sprint3.Infrastructure.Repositories
{
    /// Implementa operações de ativo usando EF Core.
   
    public class RepositorioAtivo : IRepositorioAtivo
    {
        private readonly AppDbContext _context;

        public RepositorioAtivo(AppDbContext context)
        {
            _context = context;
        }

        public void Adicionar(Ativo ativo)
        {
            _context.Ativos.Add(ativo);
            _context.SaveChanges();
        }

        public List<Ativo> Listar()
        {
            return _context.Ativos.OrderBy(a => a.Id).ToList();
        }

        public void Atualizar(Ativo ativo)
        {
            _context.Ativos.Update(ativo);
            _context.SaveChanges();
        }

        public void Remover(Ativo ativo)
        {
            _context.Ativos.Remove(ativo);
            _context.SaveChanges();
        }

        public Ativo? ObterPorId(int id)
        {
            return _context.Ativos.Find(id);
        }

        public Ativo? ObterPorNome(string nome)
        {
            return _context.Ativos.FirstOrDefault(a => a.Nome.ToLower() == nome.ToLower());
        }
    }
}
