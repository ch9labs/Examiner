using System.ComponentModel.DataAnnotations;

namespace Examiner.Domain.Dtos.Authentication;

/// <summary>
/// Specifies data for an authentication request
/// </summary>
public record AuthenticationRequest(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(6)]
    string Password
);