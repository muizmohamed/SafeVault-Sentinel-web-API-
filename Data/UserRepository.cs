using Microsoft.Data.Sqlite;
using SafeVault.Models;

namespace SafeVault.Data;

public sealed class UserRepository
{
    private readonly Database _database;

    public UserRepository(Database database) => _database = database;

    public AppUser? FindByUsername(string username)
    {
        using var connection = _database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT UserID, Username, Email, PasswordHash, Role
            FROM Users
            WHERE Username = $username
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$username", username);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return new AppUser(
            reader.GetInt64(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4));
    }

    public long Create(string username, string email, string passwordHash, string role = "User")
    {
        using var connection = _database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Users (Username, Email, PasswordHash, Role)
            VALUES ($username, $email, $passwordHash, $role);
            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$username", username);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$passwordHash", passwordHash);
        command.Parameters.AddWithValue("$role", role);
        return (long)(command.ExecuteScalar() ?? throw new InvalidOperationException("User creation failed."));
    }
}
