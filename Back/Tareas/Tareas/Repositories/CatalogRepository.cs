using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Interfaces;
using System.Collections.Generic;
using Tareas.Contracts;
using Tareas.Data.Models;
using TareasApi.Services;
using static Supabase.Postgrest.Constants;
using Client = Supabase.Client;

namespace Tareas.Repositories
{
    public class CatalogRepository: ICatalogRepository
    {
        private readonly Client _supabase;
        public CatalogRepository(Client supabase) => _supabase = supabase;

        private readonly UsersClient _users;

        public async Task<IReadOnlyList<KanbanListDto>> GetKanbanListsAsync(CancellationToken ct)
        {
            var resp = await _supabase.From<KanbanList>()
                                     .Order("position", Ordering.Ascending)
                                     .Get(cancellationToken: ct);
            return resp.Models
                .Select(m => new KanbanListDto(m.Id, m.Code, m.Title, m.Position ?? 0))
                .ToList();
        }
        public async Task<IReadOnlyList<KanbanListDetailsDto>> GetKanbanListDetailAsync(long tasklistId,CancellationToken ct)
         {
            var q = _supabase
              .From<WorkItem>()
              .Order("severidad", Ordering.Descending)
              .Order("creado_en", Ordering.Ascending);

            if (tasklistId >= int.MinValue && tasklistId <= int.MaxValue)
                q = q.Filter<int>("list_id", Operator.Equals, (int)tasklistId);
            else
                q = q.Filter<string>("list_id", Operator.Equals, tasklistId.ToString());

            var resp = await q.Get(ct);

            return resp.Models
                .Select(m => new KanbanListDetailsDto(
                    m.Id,
                    m.ListId,
                    m.Titulo,
                    m.Descripcion,
                    m.StartDate,
                    m.DueDate,
                    m.Severidad,
                    m.EstadoId,
                    m.AsignadoAUsername,
                    m.Completed,
                    m.CreadoEn,
                    m.ActualizadoEn
                )).ToList();
        }
        public async Task<IReadOnlyList<CatEstadoItemDto>> GetStatesAsync(CancellationToken ct)
        {
            var resp = await _supabase
           .From<CatEstadoItem>()
           .Order("id", Ordering.Ascending)
           .Get(ct);

            return resp.Models
                       .Select(m => new CatEstadoItemDto(m.Id, m.Nombre))
                       .ToList();
        }
        public async Task MoverItem(MoveItemDto item, CancellationToken ct)
        {
            if (item.itemId <= 0 || item.listId <= 0)
                throw new ArgumentException("itemId y listId deben ser > 0.");

            var resp = await _supabase
                .From<WorkItem>()
                .Match(new Dictionary<string, string> { ["id"] = item.itemId.ToString() })
                .Get(ct);

            var wi = resp.Models.FirstOrDefault()
                     ?? throw new KeyNotFoundException($"work_item id={item.itemId} no existe.");

            if (wi.ListId == item.listId) return;

            wi.ListId = item.listId;
            wi.ActualizadoEn = DateTime.UtcNow; 

            var upd = await _supabase.From<WorkItem>().Update(wi);
            if (!upd.Models.Any())
                throw new InvalidOperationException("No se pudo actualizar el item.");
        }

        public async Task<long> CrearItem (TaskCreateDto tarea, CancellationToken ct)
        {
            if (tarea is null) throw new ArgumentException("Error datos incorrectos.");
            if (string.IsNullOrWhiteSpace(tarea.listCode)) throw new ArgumentException("Lista es requerida.");
            if (string.IsNullOrWhiteSpace(tarea.titulo)) throw new ArgumentException("titulo es requerido.");
            if (tarea.severidad is not (1 or 3)) throw new ArgumentException("severidad debe ser 1 o 3.");
            var hoy = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            if (tarea.dueDate < hoy) throw new ArgumentException("Fecha de entrega no puede ser en el pasado.");

            var listResp = await _supabase
            .From<KanbanList>()
            .Match(new Dictionary<string, string> { ["code"] = tarea.listCode })
            .Select("id")
            .Get(ct);

            var lista = listResp.Models.FirstOrDefault()
                        ?? throw new ArgumentException($"listCode '{tarea.listCode}' no existe.");

            string? assigneeUsername = null;
            if (tarea.asignadoAUsernameId is long uid)
            {
                UserById? user;
                try
                {
                    user = await _users.GetByIdAsync(uid, ct); //micro servicio Users
                }
                catch (HttpRequestException ex)
                {
                    throw new InvalidOperationException("Servicio de Usuarios no disponible.", ex);
                }

                if (user is null) throw new ArgumentException($"Usuario id={uid} no existe.");
                if (!user.activo) throw new ArgumentException($"Usuario id={uid} está inactivo.");
                assigneeUsername = user.username;
            }

            
            DateTime? startDate = tarea.startDate?.ToDateTime(TimeOnly.MinValue);
            DateTime dueDate = tarea.dueDate.ToDateTime(TimeOnly.MinValue);

            
            var wi = new WorkItem
            {
                ExternalId = tarea.externalId,
                ListId = lista.Id,
                Titulo = tarea.titulo,
                Descripcion = tarea.descripcion,
                StartDate = startDate,
                DueDate = dueDate,
                Severidad = tarea.severidad,
                AsignadoAUsername = assigneeUsername,
                EstadoId = lista.Id,
                Completed = false,
                CreadoEn = DateTime.UtcNow,
                ActualizadoEn = DateTime.UtcNow
            };

            try
            {
                var ins = await _supabase.From<WorkItem>().Insert(wi);
                var created = ins.Models.FirstOrDefault()
                              ?? throw new InvalidOperationException("Insert no devolvió la fila creada.");
                return created.Id;
            }
            catch (PostgrestException ex)
            {
                throw new ArgumentException($"No se pudo crear la tarea: {ex.Message}");
            }
        }
    }
}
