using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Biblioteca
{
    class ArquivoCSV
    {
        public string DirectoryPath { get; set; }
        public string PathCliente { get; set; }
        public string PathLivro { get; set; }
        public string PathEmprestimo { get; set; }
        public string PathIdCliente { get; set; }
        public string PathTomboLivro { get; set; }

        public bool ProcuraCPF(string cpf)
        {
            string[] lines = File.ReadAllLines(PathCliente);
            if (lines.Length > 1)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] clienteCSV = line.Split(';');
                    if (clienteCSV[1] == cpf)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ProcuraISBN(string isbn)
        {
            string[] lines = File.ReadAllLines(PathLivro);
            if (lines.Length > 1)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] livroCSV = line.Split(';');
                    if (livroCSV[1] == isbn)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ProcuraEmprestimo(long idbusca, long tombobusca)
        {
            string[] lines = File.ReadAllLines(PathEmprestimo);
            if (lines.Length > 1)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] emprestimoCSV = line.Split(';');
                    if (long.Parse(emprestimoCSV[0]) == idbusca && long.Parse(emprestimoCSV[1]) == tombobusca)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Cliente LeituraCliente(string cpf, long id)
        {
            string[] lines = File.ReadAllLines(PathCliente);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] clienteCSV = line.Split(';');
                if (clienteCSV[1] == cpf)
                {
                    Cliente cliente = new Cliente
                    {
                        IdCliente = id,
                        CPF = cpf,
                        Nome = clienteCSV[2],
                        DataNascimento = DateTime.ParseExact(clienteCSV[3], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Telefone = clienteCSV[4],
                        endereco = new Endereco
                        {
                            Logradouro = clienteCSV[5],
                            Bairro = clienteCSV[6],
                            Cidade = clienteCSV[7],
                            Estado = clienteCSV[8],
                            CEP = clienteCSV[9],
                        }
                    };
                }
            }
            return null;
        }

        public Livro LeituraLivro(string isbn, long tombo)
        {
            string[] lines = File.ReadAllLines(PathLivro);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] livroCSV = line.Split(';');
                if (livroCSV[1] == isbn)
                {
                    Livro livro = new Livro
                    {
                        NumeroTombo = tombo,
                        ISBN = isbn,
                        Titulo = livroCSV[2],
                        Genero = livroCSV[3],
                        DataPublicacao = DateTime.ParseExact(livroCSV[4], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Autor = livroCSV[5]
                    };
                }
            }
            return null;
        }

        public void SalvarCliente(Cliente cliente)
        {
            string linecliente = $"{cliente.IdCliente};" +
                                  $"{cliente.CPF};" +
                                  $"{cliente.Nome};" +
                                  $"{cliente.DataNascimento.ToString("dd/MM/yyyy")};" +
                                  $"{cliente.Telefone};" +
                                  $"{cliente.endereco.Logradouro};" +
                                  $"{cliente.endereco.Bairro};" +
                                  $"{cliente.endereco.Cidade};" +
                                  $"{cliente.endereco.Estado};" +
                                  $"{cliente.endereco.CEP}";
            string[] lines = File.ReadAllLines(PathCliente);
            StreamWriter sw = File.AppendText(PathCliente);
            sw.Write("\n" + linecliente);
            sw.Close();
        }

        public void SalvarLivro(Livro livro)
        {
            string lineLivro = $"{livro.NumeroTombo};" +
                               $"{livro.ISBN};" +
                               $"{livro.Titulo};" +
                               $"{livro.Genero};" +
                               $"{livro.DataPublicacao};" +
                               $"{livro.Autor}";
            string[] lines = File.ReadAllLines(PathLivro);
            StreamWriter sw = File.AppendText(PathLivro);
            sw.Write("\n" + lineLivro);
            sw.Close();
        }
        public Emprestimo LeituraEmprestimo(long idCliente, long tomboemprestimo)
        {
            string[] lines = File.ReadAllLines(PathEmprestimo);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] emprestimoCSV = line.Split(';');
                if (idCliente == long.Parse(emprestimoCSV[0]) && tomboemprestimo == long.Parse(emprestimoCSV[1]))
                {
                    Emprestimo emprestimo = new Emprestimo
                    {
                        IdCliente = idCliente,
                        NumeroTombo = tomboemprestimo,
                        DataEmprestimo = DateTime.ParseExact(emprestimoCSV[2], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DataDevolucao = DateTime.ParseExact(emprestimoCSV[3], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        StatusEmprestimo = int.Parse(emprestimoCSV[4])
                    };
                }
                    
            }
            return null;
        }

        public void SalvarEmprestimo(Emprestimo emprestimo)
        {
            string lineEmprestimo = $"{emprestimo.IdCliente};" +
                               $"{emprestimo.NumeroTombo};" +
                               $"{emprestimo.DataEmprestimo};" +
                               $"{emprestimo.DataDevolucao};" +
                               $"{emprestimo.StatusEmprestimo}";
            string[] lines = File.ReadAllLines(PathEmprestimo);
            StreamWriter sw = File.AppendText(PathEmprestimo);
            sw.Write("\n" + lineEmprestimo);
            sw.Close();
        }

    }
}
