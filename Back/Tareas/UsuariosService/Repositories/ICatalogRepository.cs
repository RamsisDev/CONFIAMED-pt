using UsuariosService.Contracts;

namespace UsuariosService.Repositories
{
    public interface ICatalogRepository 
    {
        Task<bool> ValidarUsuario(string idUser, CancellationToken ct);

        Task<IReadOnlyList<UserDto>> BuscaUserPorNombre(string nombre, CancellationToken ct);

    }
}
