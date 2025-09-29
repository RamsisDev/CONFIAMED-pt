using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_items")]
    public class WorkItem : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }
        [Column("external_id")] public string? ExternalId { get; set; }
        [Column("list_id")] public long ListId { get; set; }
        [Column("titulo")] public string Titulo { get; set; } = default!;
        [Column("descripcion")] public string? Descripcion { get; set; }
        [Column("start_date")] public DateTime? StartDate { get; set; }
        [Column("due_date")] public DateTime DueDate { get; set; }
        [Column("completed")] public bool Completed { get; set; }
        [Column("severidad")] public short Severidad { get; set; } // 1 ó 3
        [Column("asignado_a_username")] public string? AsignadoAUsername { get; set; }
        [Column("estado_id")] public short EstadoId { get; set; } // 1..4
        [Column("creado_en")] public DateTime? CreadoEn { get; set; }
        [Column("actualizado_en")] public DateTime? ActualizadoEn { get; set; }

        // Embedding: trae la fila de kanban_lists al hacer SELECT si lo deseas.
        [Reference(typeof(KanbanList), includeInQuery: true, useInnerJoin: false, columnName: "list_id")]
        public KanbanList? List { get; set; }
    }
}
