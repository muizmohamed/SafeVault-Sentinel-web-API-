using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public sealed class FeedbackInput
{
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
}
