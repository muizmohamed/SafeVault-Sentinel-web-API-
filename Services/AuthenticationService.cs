using BCrypt.Net;
using SafeVault.Data;
using SafeVault.Models;

namespace SafeVault.Services;

public sealed class AuthenticationService
{
    private readonly UserRepository _users;

    public AuthenticationService(UserRepository users) => _users = users;

    public bool Register(RegisterInput input, out string? error)
    {
        var username = InputValidator.NormalizeUsername(input.Username);
        var email = InputValidator.NormalizeEmail(input.Email);

        if (!InputValidator.IsValidUsername(username))
        {
            error = "Invalid username.";
            return false;
        }

        if (!InputValidator.IsValidEmail(email))
        {
            error = "Invalid email address.";
            return false;
        }

        if (input.Password.Length < 12)
        {
            error = "Password must contain at least 12 characters.";
            return false;
        }

        if (_users.FindByUsername(username) is not null)
        {
            error = "Unable to create this account.";
            return false;
        }

        // BCrypt stores the salt inside the resulting hash.
        var passwordHash = BCrypt.HashPassword(input.Password, workFactor: 12);
        _users.Create(username, email, passwordHash, "User");
        error = null;
        return true;
    }

    public AppUser? Authenticate(string username, string password)
    {
        var normalizedUsername = InputValidator.NormalizeUsername(username);
        var user = _users.FindByUsername(normalizedUsername);

        // Deliberately do not reveal whether username or password was wrong.
        if (user is null)
            return null;

        return BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }
}
