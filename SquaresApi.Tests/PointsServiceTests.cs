using Moq;
using SquaresApi.Models;
using SquaresApi.Repositories;
using SquaresApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

public class Tests
{
    private readonly Mock<IPointsRepository> _repositoryMock;
    private readonly PointsService _service;

    public Tests()
    {
        _repositoryMock = new Mock<IPointsRepository>();
        _service = new PointsService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_AddsPoint_WhenPointDoesNotExist()
    {
        // Arrange
        var point = new Point { X = 1, Y = 1 };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Point>());

        // Act
        await _service.AddAsync(point, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(point, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ThrowsException_WhenPointExists()
    {
        // Arrange
        var point = new Point { X = 1, Y = 1 };
        var existing = new List<Point> { new Point { X = 1, Y = 1 } };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existing);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.AddAsync(point, CancellationToken.None));
    }

    [Fact]
    public async Task AddRangeAsync_AddsPoints_WhenNoDuplicates()
    {
        // Arrange
        var newPoints = new List<Point>
        {
            new Point { X = 1, Y = 1 },
            new Point { X = 2, Y = 2 }
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Point>());

        // Act
        await _service.AddRangeAsync(newPoints, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddRangeAsync(newPoints, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddRangeAsync_ThrowsException_WhenDuplicatesExist()
    {
        // Arrange
        var existing = new List<Point> { new Point { X = 1, Y = 1 } };
        var input = new List<Point>
        {
            new Point { X = 1, Y = 1 },
            new Point { X = 2, Y = 2 }
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existing);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.AddRangeAsync(input, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenPointNotFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.FindByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Point)null);

        // Act
        var result = await _service.DeleteAsync(1, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesPoint_WhenPointExists()
    {
        // Arrange
        var point = new Point { Id = 1, X = 1, Y = 1 };
        _repositoryMock.Setup(r => r.FindByIdAsync(1, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(point);

        // Act
        var result = await _service.DeleteAsync(1, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteAsync(point), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
