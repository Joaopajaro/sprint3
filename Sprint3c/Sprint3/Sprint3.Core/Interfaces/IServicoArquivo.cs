namespace Sprint3.Core.Interfaces
{
    /// <summary>
    /// Contrato para importar ativos de arquivos.
    /// </summary>
    public interface IServicoArquivo
    {
        void Importar(string caminho);
    }
}
