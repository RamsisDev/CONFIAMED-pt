using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_item_tasklists")]
    public class WorkItemTasklist : BaseModel
    {
        [PrimaryKey("id")] public long Id { get; set; }
        [Column("work_item_id")] public long WorkItemId { get; set; }
        [Column("external_id")] public string? ExternalId { get; set; }
        [Column("title")] public string? Title { get; set; }
    }
}
