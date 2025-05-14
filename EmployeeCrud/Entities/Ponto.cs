using System.Drawing;

namespace EmployeeCrud.Repository;

public class Ponto
{
    public int Id { get; set; }
    public int FuncionarioId { get; set; }
    public DateTime DataHora { get; set; }
    public string Tipo { get; set; }

    public Ponto()
    {
        
    }

    public Ponto(int funcionarioId, DateTime dataHora, string tipo)
    {
        FuncionarioId = funcionarioId;
        DataHora = dataHora;
        Tipo = tipo;
    }

    public Ponto(DateTime dataHora, int id, string tipo)
    {
        Id = id;
        DataHora = dataHora;
        Tipo = tipo;
    }
    
    
    
}