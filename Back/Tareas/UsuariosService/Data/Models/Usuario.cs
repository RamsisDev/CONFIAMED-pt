using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("usuarios")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }
        [Column("username")] public string Username { get; set; } = default!;
        [Column("correo")] public string? Correo { get; set; }
        [Column("nombre")] public string? Nombre { get; set; }
        [Column("avatar")] public string? Avatar { get; set; }
        [Column("activo")] public bool Activo { get; set; }
    }
}
