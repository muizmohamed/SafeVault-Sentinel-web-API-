using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public sealed class LoginInput
{
    [Required]
    [StringLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}
