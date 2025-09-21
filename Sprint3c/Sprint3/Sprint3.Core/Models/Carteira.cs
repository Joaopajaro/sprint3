using System.Collections.Generic;

namespace Sprint3.Core.Models
{
    /// <summary>
    /// Entidade de carteira (não utilizada neste exercício).
    /// </summary>
    public class Carteira
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<Ativo> Ativos { get; set; } = new List<Ativo>();
    }
}
