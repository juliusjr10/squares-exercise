using Microsoft.EntityFrameworkCore;
using SquaresApi.Models;
namespace SquaresApi.Data
{
    public class PointsDbContext : DbContext
    {
        public PointsDbContext(DbContextOptions<PointsDbContext> options) : base(options) { }

        public DbSet<Point> Points { get; set; }
    }
}
