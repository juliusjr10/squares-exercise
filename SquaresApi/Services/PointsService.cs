using SquaresApi.Models;
using SquaresApi.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquaresApi.Services
{
    public class PointsService : IPointsService
    {
        private readonly IPointsRepository _repository;

        public PointsService(IPointsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Point>> GetAllAsync(CancellationToken cancellationToken) =>
            await _repository.GetAllAsync(cancellationToken);

        public async Task AddAsync(Point point, CancellationToken cancellationToken)
        {
            var existingPoints = await _repository.GetAllAsync(cancellationToken);
            bool exists = existingPoints.Any(p => p.X == point.X && p.Y == point.Y);

            if (!exists)
            {
                await _repository.AddAsync(point, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            else throw new Exception("Point exists");
        }


        public async Task AddRangeAsync(IEnumerable<Point> points, CancellationToken cancellationToken)
        {
            var existingPoints = await _repository.GetAllAsync(cancellationToken);

            var duplicates = points.Where(newPoint =>
                existingPoints.Any(existing => existing.X == newPoint.X && existing.Y == newPoint.Y)).ToList();

            if (duplicates.Any())
            {
                throw new Exception("Duplicate found");
            }

            await _repository.AddRangeAsync(points, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }


        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var point = await _repository.FindByIdAsync(id, cancellationToken);
            if (point == null) return false;

            await _repository.DeleteAsync(point);
            await _repository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}