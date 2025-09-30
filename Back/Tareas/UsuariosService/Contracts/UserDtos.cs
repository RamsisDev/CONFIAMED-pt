
namespace UsuariosService.Contracts;

public record UserDto(
    string Id,
    string Username,
    string? Nombre,
    string? correo,
    string? avatarUrl,
    bool activo
);

