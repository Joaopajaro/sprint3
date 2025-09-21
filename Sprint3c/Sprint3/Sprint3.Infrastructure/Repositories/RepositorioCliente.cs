using System.Collections.Generic;
using System.Linq;
using Sprint3.Core.Interfaces;
using Sprint3.Core.Models;
using Sprint3.Infrastructure.Data;

namespace Sprint3.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa operações de cliente usando EF Core.
    /// </summary>
    public class RepositorioCliente : IRepositorioCliente
    {
        private readonly AppDbContext _context;

        public RepositorioCliente(AppDbContext context)
        {
            _context = context;
        }

        public void Adicionar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public List<Cliente> Listar()
        {
            return _context.Clientes.OrderBy(c => c.Id).ToList();
        }

        public void Atualizar(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }

        public void Remover(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
        }

        public Cliente? ObterPorId(int id)
        {
            return _context.Clientes.Find(id);
        }
    }
}
