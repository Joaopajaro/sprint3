using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sprint3.Core.Enums;
using Sprint3.Core.Interfaces;
using Sprint3.Core.Models;
using Sprint3.Infrastructure.Data;
using Sprint3.Infrastructure.Repositories;
using Sprint3.Infrastructure.Services;

namespace Sprint3.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Carrega configuração
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            string? connString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connString))
            {
                Console.WriteLine("A ConnectionString está ausente em appsettings.json.");
                return;
            }

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connString)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repoCliente = new RepositorioCliente(context);
            var repoAtivo = new RepositorioAtivo(context);
            var servicoArquivo = new ServicoArquivo(repoAtivo);

            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("\n=== Menu ===");
                Console.WriteLine("1) Importar ativos (JSON/TXT)");
                Console.WriteLine("2) Cadastrar cliente");
                Console.WriteLine("3) Listar clientes");
                Console.WriteLine("4) Cadastrar ativo");
                Console.WriteLine("5) Listar ativos");
                Console.WriteLine("6) Atualizar cliente");
                Console.WriteLine("7) Excluir cliente");
                Console.WriteLine("8) Atualizar ativo");
                Console.WriteLine("9) Excluir ativo");
                Console.WriteLine("0) Sair");
                Console.Write("Escolha: ");

                string? opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        ImportarAtivos(servicoArquivo);
                        break;
                    case "2":
                        CadastrarCliente(repoCliente);
                        break;
                    case "3":
                        ListarClientes(repoCliente);
                        break;
                    case "4":
                        CadastrarAtivo(repoAtivo);
                        break;
                    case "5":
                        ListarAtivos(repoAtivo);
                        break;
                    case "6":
                        AtualizarCliente(repoCliente);
                        break;
                    case "7":
                        ExcluirCliente(repoCliente);
                        break;
                    case "8":
                        AtualizarAtivo(repoAtivo);
                        break;
                    case "9":
                        ExcluirAtivo(repoAtivo);
                        break;
                    case "0":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        static void ImportarAtivos(IServicoArquivo servico)
        {
            Console.Write("Informe o caminho do arquivo (.json ou .txt): ");
            string? caminho = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(caminho))
            {
                Console.WriteLine("Caminho vazio.");
                return;
            }
            try
            {
                servico.Importar(caminho);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao importar: {ex.Message}");
            }
        }

        static void CadastrarCliente(IRepositorioCliente repo)
        {
            Console.Write("Nome do cliente: ");
            string? nome = Console.ReadLine();
            Console.WriteLine("Perfil (0=Conservador, 1=Moderado, 2=Agressivo): ");
            string? perfilStr = Console.ReadLine();
            if (!int.TryParse(perfilStr, out int perfilInt) || perfilInt < 0 || perfilInt > 2)
            {
                Console.WriteLine("Perfil inválido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            var cliente = new Cliente { Nome = nome.Trim(), Perfil = (Perfil)perfilInt };
            repo.Adicionar(cliente);
            Console.WriteLine("Cliente cadastrado.");
        }

        static void ListarClientes(IRepositorioCliente repo)
        {
            var lista = repo.Listar();
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum cliente cadastrado.");
                return;
            }

            Console.WriteLine("\nClientes:");
            foreach (var c in lista)
            {
                Console.WriteLine($"Id: {c.Id}, Nome: {c.Nome}, Perfil: {c.Perfil}");
            }
        }

        static void AtualizarCliente(IRepositorioCliente repo)
        {
            Console.Write("Informe o ID do cliente a atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = repo.ObterPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            Console.Write("Novo nome (enter para manter): ");
            string? nome = Console.ReadLine();
            Console.Write("Novo perfil (0=Conservador, 1=Moderado, 2=Agressivo, enter para manter): ");
            string? perfilStr = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                cliente.Nome = nome.Trim();
            }

            if (int.TryParse(perfilStr, out int perfilInt) && perfilInt >= 0 && perfilInt <= 2)
            {
                cliente.Perfil = (Perfil)perfilInt;
            }

            repo.Atualizar(cliente);
            Console.WriteLine("Cliente atualizado.");
        }

        static void ExcluirCliente(IRepositorioCliente repo)
        {
            Console.Write("Informe o ID do cliente a excluir: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = repo.ObterPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            repo.Remover(cliente);
            Console.WriteLine("Cliente removido.");
        }

        static void CadastrarAtivo(IRepositorioAtivo repo)
        {
            Console.Write("Nome do ativo: ");
            string? nome = Console.ReadLine();
            Console.Write("Classe do ativo: ");
            string? classe = Console.ReadLine();
            Console.Write("Risco do ativo (Baixo, Médio, Alto): ");
            string? risco = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(classe) || string.IsNullOrWhiteSpace(risco))
            {
                Console.WriteLine("Informações inválidas.");
                return;
            }

            var ativo = new Ativo { Nome = nome.Trim(), Classe = classe.Trim(), Risco = risco.Trim() };
            repo.Adicionar(ativo);
            Console.WriteLine("Ativo cadastrado.");
        }

        static void ListarAtivos(IRepositorioAtivo repo)
        {
            var lista = repo.Listar();
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum ativo cadastrado.");
                return;
            }

            Console.WriteLine("\nAtivos:");
            foreach (var a in lista)
            {
                Console.WriteLine($"Id: {a.Id}, Nome: {a.Nome}, Classe: {a.Classe}, Risco: {a.Risco}");
            }
        }

        static void AtualizarAtivo(IRepositorioAtivo repo)
        {
            Console.Write("Informe o ID do ativo a atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var ativo = repo.ObterPorId(id);
            if (ativo == null)
            {
                Console.WriteLine("Ativo não encontrado.");
                return;
            }

            Console.Write("Novo nome (enter para manter): ");
            string? nome = Console.ReadLine();
            Console.Write("Nova classe (enter para manter): ");
            string? classe = Console.ReadLine();
            Console.Write("Novo risco (enter para manter): ");
            string? risco = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome)) ativo.Nome = nome.Trim();
            if (!string.IsNullOrWhiteSpace(classe)) ativo.Classe = classe.Trim();
            if (!string.IsNullOrWhiteSpace(risco)) ativo.Risco = risco.Trim();

            repo.Atualizar(ativo);
            Console.WriteLine("Ativo atualizado.");
        }

        static void ExcluirAtivo(IRepositorioAtivo repo)
        {
            Console.Write("Informe o ID do ativo a excluir: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var ativo = repo.ObterPorId(id);
            if (ativo == null)
            {
                Console.WriteLine("Ativo não encontrado.");
                return;
            }

            repo.Remover(ativo);
            Console.WriteLine("Ativo removido.");
        }
    }
}
