using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public sealed class RegisterInput
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage = "Username may contain only letters, numbers, and underscore.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(128, MinimumLength = 12)]
    public string Password { get; set; } = string.Empty;
}
