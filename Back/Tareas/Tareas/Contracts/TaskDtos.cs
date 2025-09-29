using Supabase.Postgrest.Models;

namespace Tareas.Contracts;

public record TaskListQuery(
    string? list,
    string? assignee,
    int[]? state,
    short? severity,
    DateOnly? dueBefore
);

public record TaskCreateDto(
    string listCode,
    string titulo,
    string? descripcion,
    DateOnly? startDate,
    DateOnly dueDate,
    short severidad,
    long? asignadoAUsernameId,
    string? externalId
);

public record TaskPatchDto(
    string? titulo,
    string? descripcion,
    DateOnly? dueDate,
    short? severidad,
    string? asignadoAUsername
);

public record TaskMoveDto(string listCode);
public record TaskStateDto(short estadoId);
public record TaskAssignDto(string username);

public record TaskDto(
    long id,
    string titulo,
    string? descripcion,
    DateOnly? startDate,
    DateOnly dueDate,
    short severidad,
    short estadoId,
    string listCode,
    string? asignadoAUsername,
    bool completed,
    DateTime creadoEn,
    DateTime actualizadoEn
);

public record KanbanListDto(
    long Id,
    string? Code,
    string Title,
    int Position
);
public record KanbanListDetailsDto(
    long Id,
    long ListId,
    string Titulo,
    string? Descripcion,
    DateTime? StartDate,
    DateTime DueDate,
    short Severidad,
    short EstadoId,
    string? AsignadoAUsername,
    bool Completed,
    DateTime? CreadoEn,
    DateTime? ActualizadoEn
);
public record CatEstadoItemDto(short Id, string Nombre);
public record MoveItemDto(long itemId, long listId);