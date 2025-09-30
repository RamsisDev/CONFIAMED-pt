
using Microsoft.AspNetCore.Mvc;
using UsuariosService.Contracts;
using UsuariosService.Repositories;

namespace UsuariosService.Controllers
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _repo;
        public CatalogController(ICatalogRepository repo) => _repo = repo;

        [HttpGet("/existeUser/{idUser}")]
        public async Task<ActionResult<bool>> ExisteUser([FromRoute] string idUser, CancellationToken ct)
        {
            var resp = await _repo.ValidarUsuario(idUser, ct);
            return Ok(resp);
        }

        [HttpGet("/buscardUser/{termino}")]
        public async Task<ActionResult<UserDto[]>> BuscarUsuario([FromRoute] string termino, CancellationToken ct)
        {
            var resp = await _repo.BuscaUserPorNombre(termino, ct);
            return Ok(resp);
        }
    }
}
