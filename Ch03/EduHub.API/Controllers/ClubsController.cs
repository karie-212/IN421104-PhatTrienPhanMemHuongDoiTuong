using EduHub.Application.Clubs;
using EduHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduHub.API.Controllers;

/// <summary>Quản lý câu lạc bộ. Controller chỉ nhận request, gọi handler của Application rồi trả status code.</summary>
[ApiController]
[Route("api/clubs")]
public class ClubsController : ControllerBase
{
    private readonly GetClubsQueryHandler _getClubsHandler;
    private readonly GetClubByIdQueryHandler _getClubByIdHandler;
    private readonly CreateClubCommandHandler _createClubHandler;
    private readonly DeleteClubCommandHandler _deleteClubHandler;

    public ClubsController(
        GetClubsQueryHandler getClubsHandler,
        GetClubByIdQueryHandler getClubByIdHandler,
        CreateClubCommandHandler createClubHandler,
        DeleteClubCommandHandler deleteClubHandler)
    {
        _getClubsHandler = getClubsHandler;
        _getClubByIdHandler = getClubByIdHandler;
        _createClubHandler = createClubHandler;
        _deleteClubHandler = deleteClubHandler;
    }

    /// <summary>Lấy danh sách câu lạc bộ.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Club>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? name, CancellationToken ct)
    {
        IEnumerable<Club> results = await _getClubsHandler.HandleAsync(new GetClubsQuery(), ct);

        if (!string.IsNullOrWhiteSpace(name))
        {
            results = results.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(results);
    }

    /// <summary>Lấy thông tin chi tiết câu lạc bộ theo Id.</summary>
    /// <param name="id">Mã Guid của câu lạc bộ</param>
    /// <response code="200">Trả về thông tin câu lạc bộ</response>
    /// <response code="404">Không tìm thấy câu lạc bộ</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Club), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var club = await _getClubByIdHandler.HandleAsync(new GetClubByIdQuery(id), ct);
        return club is null ? NotFound() : Ok(club);
    }

    /// <summary>Tạo câu lạc bộ mới.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Club), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateClubRequest request, CancellationToken ct)
    {
        try
        {
            var club = await _createClubHandler.HandleAsync(
                new CreateClubCommand(request.Name, request.Description), ct);
            return CreatedAtAction(nameof(GetById), new { id = club.Id }, club);
        }
        catch (ArgumentException ex)
        {
            // Dữ liệu đầu vào sai → 400 kèm lý do
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Tên câu lạc bộ đã tồn tại → 409 kèm lý do
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>Xoá một câu lạc bộ.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _deleteClubHandler.HandleAsync(new DeleteClubCommand(id), ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

/// <summary>Dữ liệu client gửi lên khi tạo CLB (DTO/mapping đầy đủ sẽ học ở Chương 4).</summary>
public record CreateClubRequest(string Name, string Description);
