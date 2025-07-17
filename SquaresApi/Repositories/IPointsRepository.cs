using SquaresApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquaresApi.Repositories
{
    public interface IPointsRepository
    {
        Task<IEnumerable<Point>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Point point, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<Point> points, CancellationToken cancellationToken);
        Task<Point?> FindByIdAsync(int id, CancellationToken cancellationToken);
        Task DeleteAsync(Point point);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}