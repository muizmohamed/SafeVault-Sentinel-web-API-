namespace SafeVault.Models;

public sealed record AppUser(
    long UserId,
    string Username,
    string Email,
    string PasswordHash,
    string Role);
