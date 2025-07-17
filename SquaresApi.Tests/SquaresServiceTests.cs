using SquaresApi.Models;
using SquaresApi.Services;
using System.Collections.Generic;
using Xunit;

public class SquareServiceTests
{
    private readonly SquareService _service;

    public SquareServiceTests()
    {
        _service = new SquareService();
    }

    [Fact]
    public void FindSquares_ReturnsSingleSquare_WhenValidPointsGiven()
    {
        var points = new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 1, Y = 0 },
            new Point { X = 0, Y = 1 },
            new Point { X = 1, Y = 1 }
        };

        var result = _service.FindSquares(points);

        Assert.Single(result);
        Assert.Equal(4, result[0].Count);
    }

    [Fact]
    public void FindSquares_ReturnsMultipleSquares_WhenManyExist()
    {
        var points = new List<Point>
        {
            // First square
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 1 },
            new Point { X = 1, Y = 0 },
            new Point { X = 1, Y = 1 },

            // Second square
            new Point { X = 2, Y = 2 },
            new Point { X = 2, Y = 3 },
            new Point { X = 3, Y = 2 },
            new Point { X = 3, Y = 3 }
        };

        var result = _service.FindSquares(points);

        Assert.Equal(2, result.Count);
        Assert.All(result, square => Assert.Equal(4, square.Count));
    }

    [Fact]
    public void FindSquares_IgnoresDuplicatePoints()
    {
        var points = new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 1 },
            new Point { X = 1, Y = 0 },
            new Point { X = 1, Y = 1 },
            new Point { X = 1, Y = 1 }, // duplicate
            new Point { X = 0, Y = 1 }  // duplicate
        };

        var result = _service.FindSquares(points);

        Assert.Single(result);
        Assert.Equal(4, result[0].Count);
    }

    [Fact]
    public void FindSquares_ReturnsEmpty_WhenNoSquaresExist()
    {
        var points = new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 1, Y = 2 },
            new Point { X = 2, Y = 4 }
        };

        var result = _service.FindSquares(points);

        Assert.Empty(result);
    }

    [Fact]
    public void FindSquares_DetectsRotatedSquares()
    {
        var points = new List<Point>
        {
            new Point { X = 1, Y = 1 },
            new Point { X = 2, Y = 2 },
            new Point { X = 0, Y = 2 },
            new Point { X = 1, Y = 3 }
        };

        var result = _service.FindSquares(points);

        Assert.Single(result);
        Assert.Equal(4, result[0].Count);
    }
}
