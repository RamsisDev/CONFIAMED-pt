using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_item_tasks")]
    public class WorkItemTask : BaseModel
    {
        [PrimaryKey("id")] public long Id { get; set; }
        [Column("tasklist_id")] public long TasklistId { get; set; }
        [Column("texto")] public string Texto { get; set; } = default!;
        [Column("completed")] public bool Completed { get; set; }
        [Column("position")] public int? Position { get; set; }
    }
}
