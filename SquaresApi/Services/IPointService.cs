using SquaresApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquaresApi.Services
{
    public interface IPointsService
    {
        Task<IEnumerable<Point>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Point point, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<Point> points, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}