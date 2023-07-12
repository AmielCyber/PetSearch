using System.ComponentModel.DataAnnotations;

namespace PetSearch.Data.Entity;

public class Token
{
    public int Id { get; set; }
    [Required] public required string AccessToken { get; set; }
    [Required] public DateTime ExpiresIn { get; set; } = DateTime.Now.AddMinutes(55);
}