namespace Sprint3.Core.Interfaces
{
 
    /// Contrato para importar ativos de arquivos.

    public interface IServicoArquivo
    {
        void Importar(string caminho);
    }
}
