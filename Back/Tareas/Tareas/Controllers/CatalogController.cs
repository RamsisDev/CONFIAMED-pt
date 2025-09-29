using Microsoft.AspNetCore.Mvc;
using Tareas.Contracts;
using Tareas.Repositories;
namespace Tareas.Controllers;


[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _repo;
    public CatalogController(ICatalogRepository repo) => _repo = repo;

    [HttpGet("kanban-lists")]
    public async Task<ActionResult<IReadOnlyList<KanbanListDto>>> GetKanbanLists(CancellationToken ct)
    {
        var rows = await _repo.GetKanbanListsAsync(ct);
        return Ok(rows);
    }

    [HttpGet("kanban-list-detail/${ListId}")]
    public async Task<ActionResult<IReadOnlyList<KanbanListDetailsDto>>> GetKanbanDetails ([FromRoute]long ListId,CancellationToken ct)
    {
        var items = await _repo.GetKanbanListDetailAsync(ListId, ct);
        return Ok(items);
    }

    [HttpPost("moverItem")]
    public async Task<ActionResult> ActualizarItem ([FromBody] MoveItemDto body, CancellationToken ct)
    {
        if (body is null) return BadRequest(new { message = "Body requerido." });
        if (body.listId == null)
            return UnprocessableEntity(new { message = "Proporcione ListId." });
        try
        {
            await _repo.MoverItem(body, ct);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno al mover el item." });
        }
    }

    [HttpGet("states")]
    public async Task<ActionResult<IReadOnlyList<CatEstadoItemDto>>> GetStates(CancellationToken ct)
    {
        var rows = await _repo.GetStatesAsync(ct);
        return Ok(rows);
    }

    [HttpPost("newItem")]
    public async Task<ActionResult> NewWorkItem([FromBody]TaskCreateDto item,CancellationToken ct)
    {
        await _repo.CrearItem(item, ct);
        return Ok();
    }
}