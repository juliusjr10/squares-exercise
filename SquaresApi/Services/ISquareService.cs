using SquaresApi.Models;

namespace SquaresApi.Services
{
    public interface ISquareService
    {
        List<List<Point>> FindSquares(List<Point> points);
    }
}