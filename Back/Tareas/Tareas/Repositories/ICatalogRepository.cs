
using Microsoft.AspNetCore.Mvc;
using Tareas.Contracts;
using Tareas.Data.Models;

namespace Tareas.Repositories
{
    public interface ICatalogRepository
    {
        Task<IReadOnlyList<KanbanListDto>> GetKanbanListsAsync(CancellationToken ct);
        Task<IReadOnlyList<KanbanListDetailsDto>> GetKanbanListDetailAsync(long tasklistId, CancellationToken ct);
        Task<IReadOnlyList<CatEstadoItemDto>> GetStatesAsync(CancellationToken ct);
        Task MoverItem( MoveItemDto item, CancellationToken ct);
        Task <long> CrearItem(TaskCreateDto tarea, CancellationToken ct);
    }
}
