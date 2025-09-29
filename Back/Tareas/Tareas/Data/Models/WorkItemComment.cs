using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Tareas.Data.Models
{
    [Table("work_item_comments")]
    public class WorkItemComment : BaseModel
    {
        [PrimaryKey("id")] public long Id { get; set; }
        [Column("work_item_id")] public long WorkItemId { get; set; }
        [Column("author_id")] public long? AuthorId { get; set; }
        [Column("texto")] public string Texto { get; set; } = default!;
        [Column("creado_en")] public DateTime CreadoEn { get; set; }

        [Reference(typeof(Usuario), includeInQuery: false, useInnerJoin: false, columnName: "author_id")]
        public Usuario? Autor { get; set; }
    }
}
