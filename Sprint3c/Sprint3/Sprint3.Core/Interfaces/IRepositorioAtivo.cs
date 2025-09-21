using System.Collections.Generic;
using Sprint3.Core.Models;

namespace Sprint3.Core.Interfaces
{
    /// <summary>
    /// Contrato para operações de ativo.
    /// </summary>
    public interface IRepositorioAtivo
    {
        void Adicionar(Ativo ativo);
        List<Ativo> Listar();
        void Atualizar(Ativo ativo);
        void Remover(Ativo ativo);
        Ativo? ObterPorId(int id);
        Ativo? ObterPorNome(string nome);
    }
}
