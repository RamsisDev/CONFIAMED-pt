using Supabase;
using Tareas.Data.Models;
using UsuariosService.Contracts;
using static Supabase.Postgrest.Constants;

namespace UsuariosService.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly Client _supabase;
        public CatalogRepository(Client supabase) => _supabase = supabase;

        public async Task<bool> ValidarUsuario(string idUser, CancellationToken ct)
        {
            var resp = await _supabase.From<Usuario>()
                                     .Filter<string>("id", Supabase.Postgrest.Constants.Operator.Equals, idUser)
                                     .Get(cancellationToken: ct);
            return resp.Models.Any();
        }

        public async Task<IReadOnlyList<UserDto>> BuscaUserPorNombre(string nombre, CancellationToken ct)
        {
            var term = (nombre ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(term))
            {
                var top = await _supabase
                    .From<Usuario>()
                    .Order("username", Ordering.Ascending)
                    .Limit(10)
                    .Get(cancellationToken: ct);

                return top.Models
                    .Select(u => new UserDto(
                        Id: u.Id.ToString(),
                        Username: u.Username,
                        Nombre: u.Nombre,
                        correo: u.Correo,
                        avatarUrl: u.Avatar,
                        activo: u.Activo))
                    .ToList();
            }

            var pattern = $"%{term}%";

            // 1) por username
            var t1 = _supabase
                .From<Usuario>()
                .Filter("username", Operator.ILike, pattern)
                .Limit(10)
                .Get(cancellationToken: ct);

            // 2) por nombre
            var t2 = _supabase
                .From<Usuario>()
                .Filter("nombre", Operator.ILike, pattern)
                .Limit(10)
                .Get(cancellationToken: ct);

            var results = await Task.WhenAll(t1, t2);

            // Combina, quita duplicados por Id, ordena por username y toma 10
            var combined = results[0].Models
                .Concat(results[1].Models)
                .GroupBy(u => u.Id)
                .Select(g => g.First())
                .OrderBy(u => u.Username)
                .Take(10)
                .Select(u => new UserDto(
                    Id: u.Id.ToString(),
                    Username: u.Username,
                    Nombre: u.Nombre,
                    correo: u.Correo,
                    avatarUrl: u.Avatar,
                    activo: u.Activo))
                .ToList();

            return combined;
        }

    }
}
