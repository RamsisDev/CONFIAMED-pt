using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("capacidades_usuarios")]
    public class CapacidadUsuario : BaseModel
    {
        [PrimaryKey("user_id")]
        public long UserId { get; set; }

        [Column("max_relevantes")]
        public int MaxRelevantes { get; set; } = 3;
    }
}
