namespace Sprint3.Core.Models
{
    /// <summary>
    /// Representa um ativo de investimento.
    /// </summary>
    public class Ativo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Classe { get; set; } = string.Empty;
        public string Risco { get; set; } = string.Empty;
    }
}
