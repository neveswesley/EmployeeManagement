using System.Globalization;
using Dapper;
using EmployeeCrud.Entities;
using Microsoft.Data.SqlClient;

namespace EmployeeCrud.Repository;

public class RepositoryEmployee
{
    private readonly SqlConnection _connection;

    public RepositoryEmployee(SqlConnection connection)
    {
        _connection = connection;
    }

    public void CreateEmployee(Employee employee)
    {
        var query =
            @"INSERT INTO Employees (Nome, Cargo, Salario, DataAdimissao, Departamento) VALUES (@Nome, @Cargo, @Salario, @DataAdimissao, @Departamento);";
        _connection.Execute(query, employee);
    }

    public Employee ReadEmployeeById(int id)
    {
        var query = @"SELECT * FROM Employees WHERE Id = @Id;";
        return _connection.QueryFirstOrDefault<Employee>(query, new { Id = id});
        
    }
    
    public IEnumerable<Employee> ReadEmployeeByName(string nome)
    {
        var query = @"SELECT * FROM Employees WHERE Nome Like @Nome;";
        return _connection.Query<Employee>(query, new { Nome = $"%{nome.ToLower()}%" });
        
    }
    
    public IEnumerable<Employee> ReadEmployeeByDate(DateTime dataInicial, DateTime dataFinal)
    {
        var query = @"SELECT * FROM Employees WHERE DataAdimissao between @DataInicial and @DataFinal;";
        return _connection.Query<Employee>(query, new { DataInicial = dataInicial, DataFinal = dataFinal });
        
    }
    
    public IEnumerable<Employee> ReadEmployeeByRole(string cargo)
    {
        var query = @"SELECT * FROM Employees WHERE Cargo Like @Cargo;";
        return _connection.Query<Employee>(query, new { Cargo = $"%{cargo.ToLower()}%" });
        
    }
    
    public IEnumerable<Employee> ReadEmployeeBySalary(double salaryInic, double salaryFin)
    {
        var query = @"SELECT * FROM Employees WHERE Salario between @SalaryInicial and @SalaryFinal;";
        return _connection.Query<Employee>(query, new { SalaryInicial = salaryInic, SalaryFinal = salaryFin });
    }

    public void UpdateEmployee(Employee employee)
    {
        var query =
            @"UPDATE Employees SET Nome = @Nome, Cargo = @Cargo, Salario = @Salario, DataAdimissao = @DataAdimissao, Departamento = @Departamento WHERE Id = @Id;";
        _connection.Execute(query, employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        var query = @"DELETE FROM Employees WHERE Id = @Id;";
        _connection.Execute(query, new { Id = employee.Id });
    }
    
    public void ReadAllEmployee()
    {
        var query = @"SELECT * FROM Employees";
        var employees = _connection.Query<Employee>(query);
        foreach (var emp in employees)
        {
            Console.WriteLine($"{emp.Id} - {emp.Nome} | Cargo: {emp.Cargo} | Salário : R${emp.Salario.ToString("F2", CultureInfo.InvariantCulture)} | Data de adimissão: {emp.DataAdimissao.ToString("MM/dd/yyyy")} | Departamento: {emp.Departamento}");
        }

    }
    
}