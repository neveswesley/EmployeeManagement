using System.Threading.Channels;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EmployeeCrud.Repository;

public class RepositoryPonto
{
    private readonly SqlConnection _connection;

    public RepositoryPonto(SqlConnection connection)
    {
        _connection = connection;
    }

    public Ponto EncontrarRegistro(int id)
    {
        var query = "SELECT * FROM RegistroPonto WHERE Id=@Id";
        return _connection.QueryFirstOrDefault<Ponto>(query, new { Id = id });
    }
    public void AdicionarRegistro(Ponto ponto)
    {
        var query = "INSERT INTO RegistroPonto (FuncionarioId, DataHora, Tipo) VALUES (@FuncionarioId, @DataHora, @Tipo)";
        _connection.Execute(query, ponto);
    }

    public void AtualizarRegistro(Ponto ponto)
    {
        var query = "UPDATE RegistroPonto SET DataHora = @DataHora, Tipo = @Tipo WHERE Id = @Id";
        var linhasAfetadas = _connection.Execute(query, ponto);
        Console.WriteLine($"Linhas afetadas: {linhasAfetadas}");
    }
    
}