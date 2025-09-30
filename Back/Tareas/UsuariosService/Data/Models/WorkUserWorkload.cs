using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_user_workload")]
    public class WorkUserWorkload : BaseModel
    {
        [PrimaryKey("user_id")] public long UserId { get; set; }
        [Column("pendientes_total")] public int PendientesTotal { get; set; }
        [Column("pendientes_relevantes")] public int PendientesRelevantes { get; set; }
    }
}
