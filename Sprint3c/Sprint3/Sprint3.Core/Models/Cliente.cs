using Sprint3.Core.Enums;

namespace Sprint3.Core.Models
{
    /// <summary>
    /// Representa um cliente.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do cliente.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Perfil de investimento.
        /// </summary>
        public Perfil Perfil { get; set; }
    }
}
