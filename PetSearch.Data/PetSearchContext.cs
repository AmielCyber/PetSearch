using Microsoft.EntityFrameworkCore;
using PetSearch.Data.Entity;

namespace PetSearch.Data;

public class PetSearchContext : DbContext
{
    public DbSet<Token> Tokens { get; set; }

    public PetSearchContext(DbContextOptions<PetSearchContext> options) : base(options)
    {
    }
}