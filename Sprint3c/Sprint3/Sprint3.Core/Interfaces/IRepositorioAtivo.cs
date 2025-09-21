using System.Collections.Generic;
using Sprint3.Core.Models;

namespace Sprint3.Core.Interfaces
{
   
    /// Contrato para operações de ativo.

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
