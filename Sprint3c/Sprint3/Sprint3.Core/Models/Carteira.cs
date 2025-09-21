using System.Collections.Generic;

namespace Sprint3.Core.Models
{

    /// Entidade de carteira (não utilizada neste exercício).

    public class Carteira
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<Ativo> Ativos { get; set; } = new List<Ativo>();
    }
}
