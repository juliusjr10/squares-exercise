using Microsoft.EntityFrameworkCore;
using SquaresApi.Data;
using SquaresApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquaresApi.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly PointsDbContext _context;

        public PointsRepository(PointsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Point>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Points.ToListAsync(cancellationToken);

        public async Task AddAsync(Point point, CancellationToken cancellationToken) =>
            await _context.Points.AddAsync(point, cancellationToken);

        public async Task AddRangeAsync(IEnumerable<Point> points, CancellationToken cancellationToken) =>
            await _context.Points.AddRangeAsync(points, cancellationToken);

        public async Task<Point?> FindByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Points.FindAsync(new object[] { id }, cancellationToken);

        public Task DeleteAsync(Point point)
        {
            _context.Points.Remove(point);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
