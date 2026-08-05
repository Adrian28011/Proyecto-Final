using Microsoft.EntityFrameworkCore;
using Selenium.Models;

namespace Selenium.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<VideoGame> VideoGames => Set<VideoGame>();
    public DbSet<User> Users => Set<User>();
}