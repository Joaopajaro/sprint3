using Sprint3.Core.Enums;

namespace Sprint3.Core.Models
{

    /// Representa um cliente.
 
    public class Cliente
    { 
        /// Identificador.
     
        public int Id { get; set; }

        /// Nome do cliente.
 
        public string Nome { get; set; } = string.Empty;

        /// Perfil de investimento.
 
        public Perfil Perfil { get; set; }
    }
}
