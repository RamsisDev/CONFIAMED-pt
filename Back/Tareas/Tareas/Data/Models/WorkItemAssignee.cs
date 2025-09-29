using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_item_assignees")]
    public class WorkItemAssignee : BaseModel
    {
        [PrimaryKey("work_item_id")]
        public long WorkItemId { get; set; }

        [PrimaryKey("user_id")]
        public long UserId { get; set; }

        [Column("snapshots")] public string? Snapshots { get; set; }

        [Reference(typeof(Usuario), includeInQuery: false, useInnerJoin: false, columnName: "user_id")]
        public Usuario? Usuario { get; set; }

        [Reference(typeof(WorkItem), includeInQuery: false, useInnerJoin: false, columnName: "work_item_id")]
        public WorkItem? WorkItem { get; set; }
    }
}
