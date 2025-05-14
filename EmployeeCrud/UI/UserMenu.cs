using System.Globalization;
using EmployeeCrud.Entities;
using EmployeeCrud.Repository;
using Microsoft.Data.SqlClient;

namespace EmployeeCrud.UI;

public class UserMenu
{
    public void UserMenuMain()
    {
        const string connectionStringEmployee =
            "Server=WESLEY\\SQLEXPRESS;Database=Employee;User Id=sa;Password=1q2w3e4r5t@#;Encrypt=True;TrustServerCertificate=True;";

        using (var connection = new SqlConnection(connectionStringEmployee))
        {
            connection.Open();
            MainMenu(connection);

            void MainMenu(SqlConnection connection)
            {
                bool running = true;
                while (running)
                {
                    Console.WriteLine("--- Menu principal ---");
                    Console.WriteLine("1) Gerenciar funcionários");
                    Console.WriteLine("2) Registros de Pontos");
                    Console.WriteLine("0) Sair");
                    Console.Write("Selecione uma opção: ");
                    var menuPrincipal = Console.ReadLine();

                    if (int.TryParse(menuPrincipal, out int opcao))
                        switch (opcao)
                        {
                            case 1: GerenciarFuncionarios(connection); break;
                            case 2: RegistrarPontos(connection); break;
                            case 0: running = false; break;
                            default: Console.WriteLine("Opção inválida!"); break;
                        }
                }
            }

            void GerenciarFuncionarios(SqlConnection connection)
            {
                bool employeeManagement = true;
                while (employeeManagement)
                {
                    Console.WriteLine();
                    Console.WriteLine("--- Gerenciar funcionários --- ");
                    Console.WriteLine("1) Criar funcionário");
                    Console.WriteLine("2) Buscar funcionário");
                    Console.WriteLine("3) Atualizar funcionário");
                    Console.WriteLine("4) Excluir funcionário");
                    Console.WriteLine("5) Mostrar todos os funcionários");
                    Console.WriteLine("0) Voltar ao menu principal");
                    Console.Write("Selecione uma opção: ");
                    var gerenciadorFuncionarios = Console.ReadLine();
                    Console.WriteLine();
                    if (int.TryParse(gerenciadorFuncionarios, out int escolha)) ;
                    switch (escolha)
                    {
                        case 1: CreateEmployeeMenu(connection); break;
                        case 2: ReadEmployeeMenu(connection); break;
                        case 3: FindUpdateEmployeeMenu(connection); break;
                        case 4: DeleteEmployeeMenu(connection); break;
                        case 5: ReadAllEmployeeMenu(connection); break;
                        case 0: return;
                        default: Console.WriteLine("Opção inválida!"); break;
                    }

                    void CreateEmployeeMenu(SqlConnection connection)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Inserir novo funcionário");

                        string nome;
                        do
                        {
                            Console.Write("Informe o nome do funcionário: ");
                            nome = Console.ReadLine();

                            if (string.IsNullOrEmpty(nome))
                                Console.Write("O nome não pode estar em branco. Tente novamente. ");
                        } while (string.IsNullOrWhiteSpace(nome));

                        string cargo;
                        do
                        {
                            Console.Write("Informe o cargo: ");
                            cargo = Console.ReadLine();

                            if (string.IsNullOrEmpty(cargo))
                                Console.Write("O cargo não pode estar em branco. Tente novamente. ");
                        } while (string.IsNullOrWhiteSpace(cargo));


                        double salario;
                        while (true)
                        {
                            Console.Write("Informe o salário: ");
                            string entrada = Console.ReadLine();

                            if (double.TryParse(entrada, out salario) && salario > 0)
                                break;
                            Console.Write("Salário inválido! Tente Novamente. ");
                        }

                        DateTime dataAdmissao;
                        while (true)
                        {
                            Console.Write("Informe a data de admissão (dd/MM/AAAA): ");
                            string input = Console.ReadLine();

                            if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out dataAdmissao) && dataAdmissao <= DateTime.Now)
                                break;
                            Console.WriteLine("Data inválida. Tente no formato dd/MM/AAAA. ");
                        }

                        string departamento;
                        do
                        {
                            Console.Write("Informe o departamento: ");
                            departamento = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(departamento))
                                Console.WriteLine("O departamento não pode estar em branco. Tente novamente: ");
                        } while (string.IsNullOrWhiteSpace(departamento));

                        var emp = new Employee(nome, cargo, salario, dataAdmissao, departamento);
                        var repository = new Repository.RepositoryEmployee(connection);
                        repository.CreateEmployee(emp);

                        Console.WriteLine();
                        Console.WriteLine("Funcionário cadastrado com sucesso!");
                        Console.WriteLine();
                    }

                    void ReadEmployeeMenu(SqlConnection connection)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Buscar funcionários: ");
                        Console.Write(
                            "1. Por Id | 2. Por nome | 3. Por cargo | 4. Por data de admissão | 5. Por faixa salarial | 0. Menu principal: ");
                        var input = int.Parse(Console.ReadLine());

                        if (input == 1)
                        {
                            Console.Write("Digite o id do funcionário: ");
                            string entrada = Console.ReadLine();
                            if (int.TryParse(entrada, out int id)) ;
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeById(id);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário encontrado: ");

                                Console.WriteLine(
                                    $"{emp.Id} - {emp.Nome} | Cargo: {emp.Cargo} | Salário : R${emp.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {emp.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {emp.Departamento}");
                                Console.WriteLine();
                            }


                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 2)
                        {
                            Console.Write("Digite o nome do funcionário: ");
                            var nome = Console.ReadLine();
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeByName(nome);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 3)
                        {
                            Console.Write("Digite o cargo do funcionário: ");
                            var cargo = Console.ReadLine();
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeByRole(cargo);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 4)
                        {
                            DateTime? dataInicial = null;
                            DateTime? dataFinal = null;
                            Console.Write("Digite a data inicial: ");
                            var dateInic = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(dateInic))
                            {
                                if (DateTime.TryParseExact(dateInic, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime novaData))
                                {
                                    dataInicial = novaData;
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida! O formato correto é dd/MM/yyyy.");
                                }
                            }

                            Console.Write("Digite a data de final: ");
                            var dateFin = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(dateFin))
                            {
                                if (DateTime.TryParseExact(dateFin, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime novaData))
                                {
                                    dataFinal = novaData;
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida! O formato correto é dd/MM/yyyy.");
                                }
                            }


                            var repository = new Repository.RepositoryEmployee(connection);


                            var emp = repository.ReadEmployeeByDate(dataInicial.Value, dataFinal.Value);

                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 5)
                        {
                            Console.Write("Digite o valor inicial: ");
                            double valorInicial = double.Parse(Console.ReadLine());
                            Console.Write("Digite o valor final: ");
                            double valorFinal = double.Parse(Console.ReadLine());
                            var repository = new Repository.RepositoryEmployee(connection);
                            var foundFuncionario = repository.ReadEmployeeBySalary(valorInicial, valorFinal);
                            if (foundFuncionario != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funciários encontrados: ");
                                foreach (var item in foundFuncionario)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine("Nenhum funcionário corresponde a essa faixa salarial.");
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }
                    }
                }


                void ReadAllEmployeeMenu(SqlConnection connection)
                {
                    Console.WriteLine();
                    Console.WriteLine("----- Mostrando todos os funcionários -----");
                    var repository = new Repository.RepositoryEmployee(connection);
                    repository.ReadAllEmployee();
                    Console.WriteLine();
                }

                void FindUpdateEmployeeMenu(SqlConnection connection)
                {
                    Console.WriteLine();
                    Console.WriteLine("Buscar funcionário");
                    Console.Write(
                        "1. Por Id | 2. Por nome | 3. Por cargo | 4. Por data de admissão | 5. Por faixa salarial | 0. Menu principal: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        if (input == 1)
                        {
                            Console.Write("Digite o id do funcionário: ");
                            var id = int.Parse(Console.ReadLine());
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeById(id);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário encontrado: ");

                                Console.WriteLine(
                                    $"{emp.Id} - {emp.Nome} | Cargo: {emp.Cargo} | Salário : R${emp.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {emp.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {emp.Departamento}");
                                Console.WriteLine();
                                UpdateEmployeeMenu();
                            }


                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                return;
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 2)
                        {
                            Console.Write("Digite o nome do funcionário: ");
                            var nome = Console.ReadLine();
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeByName(nome);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }

                                UpdateEmployeeMenu();
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                return;
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 3)
                        {
                            Console.Write("Digite o cargo do funcionário: ");
                            var cargo = Console.ReadLine();
                            var repository = new Repository.RepositoryEmployee(connection);
                            var emp = repository.ReadEmployeeByRole(cargo);
                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }

                                UpdateEmployeeMenu();
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                return;
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 4)
                        {
                            DateTime? dataInicial = null;
                            DateTime? dataFinal = null;
                            Console.Write("Digite a data inicial: ");
                            var dateInic = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(dateInic))
                            {
                                if (DateTime.TryParseExact(dateInic, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime novaData))
                                {
                                    dataInicial = novaData;
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida! O formato correto é dd/MM/yyyy.");
                                }

                                UpdateEmployeeMenu();
                            }

                            Console.Write("Digite a data de final: ");
                            var dateFin = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(dateFin))
                            {
                                if (DateTime.TryParseExact(dateFin, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime novaData))
                                {
                                    dataFinal = novaData;
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida! O formato correto é dd/MM/yyyy.");
                                }
                            }


                            var repository = new Repository.RepositoryEmployee(connection);


                            var emp = repository.ReadEmployeeByDate(dataInicial.Value, dataFinal.Value);

                            if (emp != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionários encontrados: ");

                                foreach (var item in emp)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funcionário não encontrado.");
                                return;
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }

                        else if (input == 5)
                        {
                            Console.Write("Digite o valor inicial: ");
                            double valorInicial = double.Parse(Console.ReadLine());
                            Console.Write("Digite o valor final: ");
                            double valorFinal = double.Parse(Console.ReadLine());
                            var repository = new Repository.RepositoryEmployee(connection);
                            var foundFuncionario = repository.ReadEmployeeBySalary(valorInicial, valorFinal);
                            if (foundFuncionario != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Funciários encontrados: ");
                                foreach (var item in foundFuncionario)
                                {
                                    Console.WriteLine(
                                        $"{item.Id} - {item.Nome} | Cargo: {item.Cargo} | Salário : R${item.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {item.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {item.Departamento}");
                                }
                            }

                            else
                            {
                                Console.WriteLine("Nenhum funcionário corresponde a essa faixa salarial.");
                                return;
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                            UpdateEmployeeMenu();
                        }
                    }

                    void UpdateEmployeeMenu()
                    {
                        Console.WriteLine();
                        Console.WriteLine("----- Confirme funcionário para atualizar -----");
                        Console.Write("Digite o id do funcionário: ");
                        var id = int.Parse(Console.ReadLine());
                        var repository = new Repository.RepositoryEmployee(connection);
                        var emp = repository.ReadEmployeeById(id);
                        if (emp != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Funcionário selecionado: ");
                            repository.ReadEmployeeById(id);
                            Console.WriteLine(
                                $"{emp.Id} - {emp.Nome} | Cargo: {emp.Cargo} | Salário : R${emp.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de admissão: {emp.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {emp.Departamento}");
                            Console.Write("Digite o novo nome (pressione Enter para manter): ");
                            var nome = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nome))
                            {
                                emp.Nome = nome;
                            }

                            Console.Write("Digite o novo cargo (pressione Enter para manter): ");
                            var cargo = Console.ReadLine();

                            if (!string.IsNullOrEmpty(cargo))
                            {
                                emp.Cargo = cargo;
                            }

                            Console.Write("Digite o novo salario (pressione Enter para manter): ");
                            var input = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out var novoSalario))
                            {
                                emp.Salario = novoSalario;
                            }

                            Console.Write("Digite o novo departamento (pressione Enter para manter): ");
                            var departamento = Console.ReadLine();

                            if (!string.IsNullOrEmpty(departamento))
                            {
                                emp.Departamento = departamento;
                            }

                            Console.Write("Digite a nova data de admissão (pressione Enter para manter): ");
                            var date = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(date))
                            {
                                if (DateTime.TryParseExact(date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime novaData))
                                {
                                    emp.DataAdimissao = novaData;
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida! O formato correto é dd/MM/yyyy.");
                                }
                            }

                            repository.UpdateEmployee(emp);

                            Console.WriteLine("Funcionário atualizado com sucesso!");
                            Console.WriteLine();
                        }
                    }
                }

                void DeleteEmployeeMenu(SqlConnection connection)
                {
                    Console.WriteLine();
                    Console.Write("Digite o Id do funcionário: ");
                    var id = int.Parse(Console.ReadLine());
                    var repository = new Repository.RepositoryEmployee(connection);
                    var rep = repository.ReadEmployeeById(id);
                    if (rep != null)
                    {
                        Console.WriteLine("Funcionário encontrado: ");
                        Console.WriteLine(
                            $"{rep.Id} - {rep.Nome} | Cargo: {rep.Cargo} | Salário : R${rep.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Departamento: {rep.Departamento}");
                        Console.WriteLine("Deseja excluir este funcionário? (s/n) ");
                        var input = Console.ReadLine();
                        if (input.ToLower() == "s")
                        {
                            repository.DeleteEmployee(rep);
                            Console.WriteLine("Funcinário excluído com sucesso!");
                            Console.WriteLine();
                        }
                        else if (input.ToLower() == "n")
                        {
                            Console.WriteLine("Operação cancelada!");
                            MainMenu(connection);
                            Console.WriteLine();
                        }
                    }
                }
            }

            void RegistrarPontos(SqlConnection connection)
            {
                Console.WriteLine();
                Console.WriteLine("--- Registro de Pontos ---");
                Console.WriteLine("1) Registrar ponto");
                Console.WriteLine("2) Atualizar registro de ponto");
                Console.WriteLine("3) Visualizar registro de ponto");
                Console.WriteLine("0) Voltar ao menu principal");
                Console.Write("Selecione uma opção: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int opcao)) ;
                switch (opcao)
                {
                    case 1: AdicionarRegistroMenu(connection); break;
                    case 2: AtualizarRegistroMenu(connection); break;
                    case 3: VisualizarRegistroMenu(connection); break;
                    case 0: return;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
            }

            void AdicionarRegistroMenu(SqlConnection connection)
            {
                Console.WriteLine();
                Console.WriteLine("--- Registrar ponto ---");
                Console.Write("Informe o Id do funcionário: ");
                var id = int.Parse(Console.ReadLine());
                var repositoryEmployee = new RepositoryEmployee(connection);
                var rep = repositoryEmployee.ReadEmployeeById(id);
                var repositoryPonto = new RepositoryPonto(connection);

                if (rep != null)
                {
                    Console.WriteLine("Funcionário encontrado: ");
                    Console.WriteLine(
                        $"{rep.Id} - {rep.Nome} | Cargo: {rep.Cargo}");
                    Console.WriteLine();
                }

                else
                {
                    Console.WriteLine("Funcionário não encontrado!");
                    RegistrarPontos(connection);
                }

                Console.Write("Registrar entrada ou saída? (e/s): ");
                var input = Console.ReadLine();

                if (input.ToLower() == "e")
                {
                    var entrada = DateTime.Now;
                    var ponto = new Ponto(id, entrada, "entrada");
                    repositoryPonto.AdicionarRegistro(ponto);
                    Console.WriteLine("Entrada registrada com sucesso.");
                    Console.WriteLine();
                }

                else if (input.ToLower() == "s")
                {
                    var saida = DateTime.Now;
                    var ponto = new Ponto(id, saida, "saida");
                    repositoryPonto.AdicionarRegistro(ponto);
                    Console.WriteLine("Saida registrada com sucesso.");
                    Console.WriteLine();
                }

                else
                {
                    Console.WriteLine("Opção inválida!");
                }
            }

            void AtualizarRegistroMenu(SqlConnection _connection)
            {
                Console.WriteLine();
                Console.WriteLine("--- Atualizar registro de ponto ---");
                Console.Write("Informe o Id registro do ponto: ");
                var id = int.Parse(Console.ReadLine());
                
                var repositoryEmployee = new RepositoryEmployee(_connection);
                var repositoryPonto = new RepositoryPonto(_connection);

                var point = repositoryPonto.EncontrarRegistro(id);

                if (point != null)
                {
                    var rep = repositoryEmployee.ReadEmployeeById(point.FuncionarioId);
                    Console.WriteLine();
                    Console.WriteLine("Registro encontrado: ");
                    Console.WriteLine(
                        $"{rep.Id} - {rep.Nome} | Cargo: {rep.Cargo} | Horário do registro: {point.DataHora.ToString("dd/MM/yyyy HH:mm")} | {point.Tipo}");
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Registro não encontrado");
                    RegistrarPontos(_connection);
                }

                Console.WriteLine();
                Console.Write("Atualizar esse registro? (s/n): ");
                var input = Console.ReadLine();

                if (input.ToLower() == "s")
                {
                    Console.WriteLine();
                    Console.Write("Informe o horário do registro (dd/MM/yyyy HH:mm): ");
                    var horario = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture);
                    Console.Write("Entrada ou saída? (e/s): ");
                    var tipo = Console.ReadLine();
                    if (tipo.ToLower() == "s")
                    {
                        var ponto = new Ponto(horario, id, "saída");
                        repositoryPonto.AtualizarRegistro(ponto);
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.WriteLine();
                    }

                    else if (tipo.ToLower() == "e")
                    {
                        Console.WriteLine();
                        var ponto = new Ponto(horario, id, "entrada");
                        repositoryPonto.AtualizarRegistro(ponto);
                        Console.WriteLine("Registro atualizado com sucesso!");
                        Console.WriteLine();
                    }
                }

                else if (input.ToLower() == "n")
                {
                    Console.WriteLine("Operação cancelada!");
                    Console.WriteLine();
                }
            }

            void VisualizarRegistroMenu(SqlConnection _connection)
            {
            }
        }
    }
}