using System.Reflection;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using SafeVault.Data;

namespace SafeVault.Tests;

[TestFixture]
public sealed class SqlInjectionTests
{
    [Test]
    public void ParameterizedLookup_TreatsSqlPayloadAsData()
    {
        using var connection = CreateInMemoryConnection();
        InitializeSchema(connection);
        InsertUser(connection, "alice", "alice@example.com", "hash", "User");

        const string payload = "alice' OR '1'='1";
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Username FROM Users WHERE Username = $username LIMIT 1;";
        command.Parameters.AddWithValue("$username", payload);

        using var reader = command.ExecuteReader();
        Assert.That(reader.Read(), Is.False);
    }

    [Test]
    public void ParameterizedInsert_PreservesPayloadAsLiteralData()
    {
        using var connection = CreateInMemoryConnection();
        InitializeSchema(connection);

        const string payload = "admin' OR '1'='1";
        InsertUser(connection, payload, "payload@example.com", "hash", "User");

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Username FROM Users WHERE Username = $username;";
        command.Parameters.AddWithValue("$username", payload);

        Assert.That(command.ExecuteScalar(), Is.EqualTo(payload));
    }

    private static SqliteConnection CreateInMemoryConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        return connection;
    }

    private static void InitializeSchema(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE Users (UserID INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL UNIQUE, Email TEXT NOT NULL UNIQUE, PasswordHash TEXT NOT NULL, Role TEXT NOT NULL);";
        command.ExecuteNonQuery();
    }

    private static void InsertUser(SqliteConnection connection, string username, string email, string hash, string role)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES ($username, $email, $hash, $role);";
        command.Parameters.AddWithValue("$username", username);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$hash", hash);
        command.Parameters.AddWithValue("$role", role);
        command.ExecuteNonQuery();
    }
}
