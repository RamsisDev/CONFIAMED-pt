using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using System.Diagnostics.CodeAnalysis;
using Tareas.Contracts;
using Tareas.Repositories;
using TareasApi.Services;
namespace Tareas.Controllers;


[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _repo;

    private readonly UsersClient _userMs;
    public CatalogController(ICatalogRepository repo, UsersClient userMs)
    {
        _repo = repo;
        _userMs = userMs;
    }

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
        if (item is null) return BadRequest("Payload vacío.");
        if (item.asignadoAUsernameId is not long userId || userId <= 0)
            return BadRequest("El campo 'asignadoAUsernameId' es obligatorio y debe ser > 0.");
        var existeUser = await _userMs.GetByIdAsync(userId, ct);
        if(!existeUser)
            return UnprocessableEntity(new { message = $"No existe usuario con id={userId}." });
        
        await _repo.CrearItem(item, ct);

        return Ok();
    }



    [HttpGet("probarMs")]
    public async Task<ActionResult<string>> ProbarMs(CancellationToken ct)
    {
        var r = await _userMs.GetByIdAsync(1, ct);
        return Ok(r);
    }
}