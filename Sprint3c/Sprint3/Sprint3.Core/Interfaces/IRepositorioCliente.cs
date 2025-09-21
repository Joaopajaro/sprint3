using System.Collections.Generic;
using Sprint3.Core.Models;

namespace Sprint3.Core.Interfaces
{
    /// <summary>
    /// Contrato para operações de cliente.
    /// </summary>
    public interface IRepositorioCliente
    {
        void Adicionar(Cliente cliente);
        List<Cliente> Listar();
        void Atualizar(Cliente cliente);
        void Remover(Cliente cliente);
        Cliente? ObterPorId(int id);
    }
}
