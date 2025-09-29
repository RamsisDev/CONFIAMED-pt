using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models;

[Table("cat_estado_item")]
public class CatEstadoItem : BaseModel
{
    [PrimaryKey("id")] public short Id { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = default!;
}
