using System.ComponentModel.DataAnnotations;

namespace PetSearch.Data.Entities;

/// <summary>
/// Token entity to be stored and retrieve from our DB.
/// </summary>
public class Token
{
    public int Id { get; set; }
    [Required] public required string AccessToken { get; set; }
    [Required] public DateTime ExpiresIn { get; set; } = DateTime.Now.AddMinutes(55);
}