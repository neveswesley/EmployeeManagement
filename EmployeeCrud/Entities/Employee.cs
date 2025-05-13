namespace EmployeeCrud.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public double Salario { get; set; }
    public DateTime DataAdimissao { get; set; }
    public string Departamento { get; set; }

    public Employee()
    {
        
    }
    public Employee(string nome, string cargo, double salario, DateTime dataAdimissao, string departamento)
    {
        Nome = nome;
        Cargo = cargo;
        Salario = salario;
        DataAdimissao = dataAdimissao;
        Departamento = departamento;
    }
}