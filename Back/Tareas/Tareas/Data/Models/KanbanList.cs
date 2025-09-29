
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models;

[Table("kanban_lists")]
public class KanbanList : BaseModel
{
    [PrimaryKey("id")] public short Id { get; set; }
    [Column("code")] public string Code { get; set; } = default!;
    [Column("title")] public string? Title { get; set; }
    [Column("position")] public int? Position { get; set; }
}
