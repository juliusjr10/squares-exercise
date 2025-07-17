using SquaresApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace SquaresApi.Services
{
    public class SquareService : ISquareService
    {
        public List<List<Point>> FindSquares(List<Point> points)
        {
            // Remove duplicates by (X, Y)
            points = points
                .GroupBy(p => (p.X, p.Y))
                .Select(g => g.First())
                .ToList();

            var result = new List<List<Point>>();
            var seen = new HashSet<string>();

            var pointSet = points
                .Select(p => (p.X, p.Y))
                .ToHashSet();

            // Map from (X, Y) to full Point object (to preserve Id)
            var pointMap = points
                .ToDictionary(p => (p.X, p.Y));

            for (int i = 0; i < points.Count; i++)
            {
                var p1 = points[i];
                for (int j = i + 1; j < points.Count; j++)
                {
                    var p2 = points[j];

                    int dx = p2.X - p1.X;
                    int dy = p2.Y - p1.Y;

                    // First rotation direction
                    var p3 = (p1.X - dy, p1.Y + dx);
                    var p4 = (p2.X - dy, p2.Y + dx);

                    if (pointSet.Contains(p3) && pointSet.Contains(p4))
                    {
                        var squarePoints = new[] {
                            pointMap[(p1.X, p1.Y)],
                            pointMap[(p2.X, p2.Y)],
                            pointMap[p3],
                            pointMap[p4]
                        };

                        if (squarePoints.Select(p => (p.X, p.Y)).Distinct().Count() == 4)
                        {
                            var square = squarePoints
                                .OrderBy(p => p.X)
                                .ThenBy(p => p.Y)
                                .ToList();

                            string key = string.Join(";", square.Select(p => $"{p.X},{p.Y}"));
                            if (!seen.Contains(key))
                            {
                                seen.Add(key);
                                result.Add(square);
                            }
                        }
                    }

                    // Second rotation direction
                    var p5 = (p1.X + dy, p1.Y - dx);
                    var p6 = (p2.X + dy, p2.Y - dx);

                    if (pointSet.Contains(p5) && pointSet.Contains(p6))
                    {
                        var squarePoints = new[] {
                            pointMap[(p1.X, p1.Y)],
                            pointMap[(p2.X, p2.Y)],
                            pointMap[p5],
                            pointMap[p6]
                        };

                        if (squarePoints.Select(p => (p.X, p.Y)).Distinct().Count() == 4)
                        {
                            var square = squarePoints
                                .OrderBy(p => p.X)
                                .ThenBy(p => p.Y)
                                .ToList();

                            string key = string.Join(";", square.Select(p => $"{p.X},{p.Y}"));
                            if (!seen.Contains(key))
                            {
                                seen.Add(key);
                                result.Add(square);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
