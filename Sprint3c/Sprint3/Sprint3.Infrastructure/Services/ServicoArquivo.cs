using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Sprint3.Core.Interfaces;
using Sprint3.Core.Models;

namespace Sprint3.Infrastructure.Services
{
    /// <summary>
    /// Serviço para importar ativos de arquivos JSON ou TXT.
    /// </summary>
    public class ServicoArquivo : IServicoArquivo
    {
        private readonly IRepositorioAtivo _repositorioAtivo;

        public ServicoArquivo(IRepositorioAtivo repositorioAtivo)
        {
            _repositorioAtivo = repositorioAtivo;
        }

        public void Importar(string caminho)
        {
            if (!File.Exists(caminho))
            {
                Console.WriteLine("Arquivo não encontrado.");
                return;
            }

            string ext = Path.GetExtension(caminho).ToLower();
            switch (ext)
            {
                case ".json":
                    ImportarJson(caminho);
                    break;
                case ".txt":
                    ImportarTxt(caminho);
                    break;
                default:
                    Console.WriteLine("Formato inválido (use .json ou .txt).");
                    break;
            }
        }

        private void ImportarJson(string caminho)
        {
            try
            {
                string json = File.ReadAllText(caminho);
                var lista = JsonSerializer.Deserialize<List<Ativo>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (lista == null)
                {
                    Console.WriteLine("JSON vazio ou inválido.");
                    return;
                }

                int inseridos = 0, atualizados = 0, erros = 0;
                foreach (var ativo in lista)
                {
                    if (string.IsNullOrWhiteSpace(ativo.Nome) || string.IsNullOrWhiteSpace(ativo.Classe) || string.IsNullOrWhiteSpace(ativo.Risco))
                    {
                        erros++;
                        continue;
                    }

                    var existente = _repositorioAtivo.ObterPorNome(ativo.Nome);
                    if (existente != null)
                    {
                        existente.Classe = ativo.Classe;
                        existente.Risco = ativo.Risco;
                        _repositorioAtivo.Atualizar(existente);
                        atualizados++;
                    }
                    else
                    {
                        _repositorioAtivo.Adicionar(new Ativo { Nome = ativo.Nome, Classe = ativo.Classe, Risco = ativo.Risco });
                        inseridos++;
                    }
                }

                Console.WriteLine($"Importação JSON concluída: inseridos {inseridos}, atualizados {atualizados}, erros {erros}.");
            }
            catch
            {
                Console.WriteLine("Erro ao processar JSON.");
            }
        }

        private void ImportarTxt(string caminho)
        {
            int inseridos = 0, atualizados = 0, erros = 0;
            foreach (var linha in File.ReadAllLines(caminho))
            {
                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                var partes = linha.Split('|');
                if (partes.Length < 3)
                {
                    erros++;
                    continue;
                }

                string nome = partes[0].Trim();
                string classe = partes[1].Trim();
                string risco = partes[2].Trim();

                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(classe) || string.IsNullOrWhiteSpace(risco))
                {
                    erros++;
                    continue;
                }

                var existente = _repositorioAtivo.ObterPorNome(nome);
                if (existente != null)
                {
                    existente.Classe = classe;
                    existente.Risco = risco;
                    _repositorioAtivo.Atualizar(existente);
                    atualizados++;
                }
                else
                {
                    _repositorioAtivo.Adicionar(new Ativo { Nome = nome, Classe = classe, Risco = risco });
                    inseridos++;
                }
            }

            Console.WriteLine($"Importação TXT concluída: inseridos {inseridos}, atualizados {atualizados}, erros {erros}.");
        }
    }
}
