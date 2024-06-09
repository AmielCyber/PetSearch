using Microsoft.EntityFrameworkCore;
using PetSearch.Data.Entities;

namespace PetSearch.Data;

/// <summary>
/// Our Context to store PetFinder's token in our DB.
/// </summary>
public class PetSearchContext : DbContext
{
    public DbSet<Token> Tokens { get; set; }

    public PetSearchContext(DbContextOptions<PetSearchContext> options) : base(options)
    {
    }
}