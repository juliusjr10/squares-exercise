using Microsoft.AspNetCore.Mvc;
using SquaresApi.Models;
using SquaresApi.Services;
using SquaresApi.Data.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquaresApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly IPointsService _pointsService;
        private readonly ISquareService _squareService;

        public PointsController(IPointsService pointsService, ISquareService squareService)
        {
            _pointsService = pointsService;
            _squareService = squareService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var points = await _pointsService.GetAllAsync(cancellationToken);
            return Ok(points);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PointDTO pointDto, CancellationToken cancellationToken)
        {
            var point = new Point { X = pointDto.X, Y = pointDto.Y };
            await _pointsService.AddAsync(point, cancellationToken);
            return Ok(point);
        }


        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] List<PointDTO> pointDtos, CancellationToken cancellationToken)
        {
            var points = pointDtos.Select(dto => new Point { X = dto.X, Y = dto.Y });
            await _pointsService.AddRangeAsync(points, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _pointsService.DeleteAsync(id, cancellationToken);
            return deleted ? Ok() : NotFound();
        }

        [HttpGet("squares")]
        public async Task<IActionResult> GetSquares(CancellationToken cancellationToken)
        {
            var points = await _pointsService.GetAllAsync(cancellationToken);
            var squares = _squareService.FindSquares(points.ToList());
            return Ok(new { Count = squares.Count, Squares = squares });
        }
    }
}