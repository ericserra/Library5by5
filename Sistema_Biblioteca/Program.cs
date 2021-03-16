using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Biblioteca
{
    class Program
    {
        static void Main(string[] args)
        {
            ArquivoCSV arquivo = new ArquivoCSV();
            Cliente cliente = new Cliente();
            Livro livro = new Livro();
            Emprestimo emprestimo = new Emprestimo();
            arquivo.DirectoryPath = @"C:\Arquivo";
            arquivo.PathCliente = @"C:\Arquivo\CLIENTE.csv";
            arquivo.PathEmprestimo = @"C:\Arquivo\EMPRESTIMO.csv";
            arquivo.PathLivro = @"C:\Arquivo\LIVRO.csv";
            arquivo.PathIdCliente = @"C:\Arquivo\IdCliente.txt";
            arquivo.PathTomboLivro = @"C:\Arquivo\TomboLivro.txt";
            if (!Directory.Exists(arquivo.DirectoryPath))
            {
                Directory.CreateDirectory(arquivo.DirectoryPath);
      
            }
            if (!File.Exists(arquivo.PathIdCliente))
            {
                FileStream sw = File.Create(arquivo.PathIdCliente);
                sw.Close();
            }
            if (!File.Exists(arquivo.PathTomboLivro))
            {
                FileStream sw = File.Create(arquivo.PathTomboLivro);
                sw.Close();
            }
            Menu(arquivo, cliente, livro, emprestimo);
            Console.Clear();
            Console.WriteLine("Aperte qualquer tecla para sair...");
            Console.ReadKey();
        }
        static void MenuGrafico()
        {
            Console.WriteLine("Menu da biblitoca");
            Console.WriteLine("1 - Cadastro de Cliente\n" +
                              "2 - Cadastro de Livro\n" +
                              "3 - Emprésitmo de Livro\n" +
                              "4 - Devolução de Livro\n" +
                              "5 - Relatório de Empréstimos e Devoluções\n" +
                              "0 - Sair");
        }
        static void Menu(ArquivoCSV arquivo, Cliente cliente, Livro livro, Emprestimo emprestimo)
        {
            string menu = " ";
            do
            {
                MenuGrafico();
                Console.Write("\nEscolha uma opção: ");
                menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Digite o CPF: ");
                        long id = LerIdCliente(arquivo);
                        string cpf = Console.ReadLine();
                        if (arquivo.ProcuraCPF(cpf))
                        {
                            Console.WriteLine("Cliente já cadastrado!");
                            Console.WriteLine("Aperte qualquer tecla para retornar ao menu principal");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            cliente = LeituraCliente(cpf, id, arquivo);
                            arquivo.SalvarCliente(cliente);
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Digite o ISBN: ");
                        long tombo = LerTomboLivro(arquivo);
                        string isbn = Console.ReadLine();
                        if (arquivo.ProcuraISBN(isbn))
                        {
                            Console.WriteLine("Livro já cadastrado!");
                            Console.WriteLine("Aperte qualquer tecla para retornar ao menu principal");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            livro = LeituraLivro(isbn, tombo, arquivo);
                            arquivo.SalvarLivro(livro);
                        }
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Digite o ID do Cliente: ");
                        long idcliente = long.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o Tombo do Livro para Emprestimo: ");
                        long tomboemprestimo = long.Parse(Console.ReadLine());
                        DateTime dataEmprestimo = DateTime.Now;
                        DateTime dataDevolucao = DateTime.Parse("01/01/0001");
                        do
                        {
                            Console.Write("Data de Devolucao(dd/mm/aaaa): ");
                            string dn = Console.ReadLine();

                            if (!DateTime.TryParseExact(dn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataDevolucao))
                            {
                                Console.WriteLine("Digite no formato especificado: dd/mm/aaaa");
                            }
                        } while (dataDevolucao == DateTime.Parse("01/01/0001"));
                        int status = 1;
                        emprestimo = new Emprestimo
                        {
                            IdCliente = idcliente,
                            NumeroTombo = tomboemprestimo,
                            DataEmprestimo = dataEmprestimo,
                            DataDevolucao = dataDevolucao,
                            StatusEmprestimo = status,
                        };
                        arquivo.SalvarEmprestimo(emprestimo);
                        break;
                    case "4":
                        Console.Write("Digite o ID do Cliente: ");
                        long idbusca = long.Parse(Console.ReadLine());
                        Console.WriteLine("Digite o Tombo do livro: ");
                        long tombobusca = long.Parse(Console.ReadLine());
                        int statusatt = 2;
                        if (arquivo.ProcuraEmprestimo(idbusca, tombobusca))
                        {
                            Console.Write("Alterando status de emprestimo!");
                            emprestimo.StatusEmprestimo = statusatt;
                            arquivo.SalvarEmprestimo(emprestimo);
                        };
                        break;
                    case "5":
                        break;
                    case "0":
                        break;
                    default:
                        break;
                }
                Console.Clear();
            } while (menu != "0");
        }
        
      
        public static void SalvarIdCliente(long id, ArquivoCSV arquivo)
        {
            FileStream fl = File.Create(arquivo.PathIdCliente);
            fl.Close();
            using (StreamWriter sw = new StreamWriter(arquivo.PathIdCliente, true))
            {
                sw.Write(id);
            }
        }
        public static int LerIdCliente(ArquivoCSV arquivo)
        {
            
            string[] lines = File.ReadAllLines(arquivo.PathIdCliente);
            if (lines.Length != 1)
            {
                return 0;
            }

            return int.Parse(lines[0]);
        }

        public static void SalvarTomboLivro(long tombo, ArquivoCSV arquivo)
        {
            FileStream fl = File.Create(arquivo.PathTomboLivro);
            fl.Close();
            using (StreamWriter sw = new StreamWriter(arquivo.PathTomboLivro, true))
            {
                sw.Write(tombo);
            }
        }
        public static int LerTomboLivro(ArquivoCSV arquivo)
        {
            string[] lines = File.ReadAllLines(arquivo.PathTomboLivro);
            if (lines.Length != 1)
            {
                return 0;
            }

            return int.Parse(lines[0]);
        }

        static Cliente LeituraCliente(string cpf, long id, ArquivoCSV arquivo)
        {
            id = LerIdCliente(arquivo);
            id++;
            SalvarIdCliente(id, arquivo);
            Console.WriteLine("Usuário não cadastrado. Insira os Dados: ");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            DateTime dataNascimento = DateTime.Parse("01/01/0001");
            do
            {
                Console.Write("Data de nascimento(dd/mm/aaaa): ");
                string dn = Console.ReadLine();

                if (!DateTime.TryParseExact(dn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimento))
                {
                    Console.WriteLine("Digite no formato especificado: dd/mm/aaaa");
                }
            } while (dataNascimento == DateTime.Parse("01/01/0001"));

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();
            Console.WriteLine("\n Agora cadastre o endereço");
            Console.Write("Logradouro: ");
            string logradouro = Console.ReadLine();
            Console.Write("\nBairro: ");
            string bairro = Console.ReadLine();
            Console.Write("\nCidade: ");
            string cidade = Console.ReadLine();
            Console.Write("\nEstado: ");
            string estado = Console.ReadLine();
            Console.Write("\nCEP: ");
            string cep = Console.ReadLine();
            
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                CPF = cpf,
                Nome = nome,
                DataNascimento = dataNascimento,
                Telefone = telefone,
                endereco = new Endereco
                {
                    Logradouro = logradouro,
                    Bairro = bairro,
                    Cidade = cidade,
                    Estado = estado,
                    CEP = cep
                }


            };
            Console.WriteLine($"\nID do cliente: {id}");
            Console.WriteLine("\nAperte qualquer tecla pra sair");
            Console.ReadKey();
            return cliente;
        }

        static Livro LeituraLivro(string isbn, long tombo, ArquivoCSV arquivo)
        {
            tombo = LerTomboLivro(arquivo);
            tombo++;
            SalvarTomboLivro(tombo, arquivo);
            Console.WriteLine("Livro não cadastrado. Insira os Dados: ");
            Console.Write("Digite o Titulo: ");
            string titulo = Console.ReadLine();
            Console.Write("Digite o Genero: ");
            string genero = Console.ReadLine();

            DateTime dataPublicacao = DateTime.Parse("01/01/0001");
            do
            {
                Console.Write("Data de Publicação(dd/mm/aaaa): ");
                string dn = Console.ReadLine();

                if (!DateTime.TryParseExact(dn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataPublicacao))
                {
                    Console.WriteLine("Digite no formato especificado: dd/mm/aaaa");
                }
            } while (dataPublicacao == DateTime.Parse("01/01/0001"));

            Console.Write("Digite o nome do Autor: ");
            string autor = Console.ReadLine();

            Livro livro = new Livro
            {
                NumeroTombo = tombo,
                ISBN = isbn,
                Titulo = titulo,
                Genero = genero,
                DataPublicacao = dataPublicacao,
                Autor = autor
            };
            Console.WriteLine($"\nTombo do livro: {tombo}");
            Console.WriteLine("\nAperte qualquer tecla pra sair");
            Console.ReadKey();
            return livro;
        }
    }
}
