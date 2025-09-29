using System.Net;
using System.Net.Http.Json;

namespace TareasApi.Services;

public class UsersClient
{
    private readonly HttpClient _http;
    public UsersClient(HttpClient http) => _http = http;

    public async Task<UserById?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var res = await _http.GetAsync($"/users/{id}", ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<UserById>(cancellationToken: ct);
    }
}
public record UserById(long id, string username, string? nombre, string? correo, bool activo);
public record UserInfo(long id, string username, string? nombre, string? correo, bool activo, int? maxRelevantes);
