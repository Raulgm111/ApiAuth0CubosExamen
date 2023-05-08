using ApiAuth0CubosExamen.Models;
using ApiAuth0CubosExamen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiAuth0CubosExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        // GET: api/Cubos
        /// <summary>
        /// Obtiene el conjunto de Cubos
        /// </summary>
        /// <remarks>
        /// Metodo para devolver los Cubos de la BBD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }

        // GET: api/Cubos/id
        /// <summary>
        /// Obtiene un Cubos por su Marca.
        /// </summary>
        /// <remarks>
        /// Permite buscar un Cubos por su Marca de empresa
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>        
        [HttpGet("{marca}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Cubo>>> FindCubo(string marca)
        {
            return await this.repo.FindCuboAsync(marca);
        }

        [HttpPost]
        public async Task<ActionResult<Cubo>> NewHospital(Cubo cubo)
        {
            await this.repo.NewCuboAsync(cubo.Nombre, cubo.Marca, cubo.Imagen, cubo.Precio);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> NewUsuario(Usuario usuario)
        {
            await this.repo.NewUsuarioAsync(usuario.Nombre, usuario.Email, usuario.Pass, usuario.Imagen);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<List<Pedido>>> GetPedidos()
        {
            Claim claim = HttpContext.User.Claims
    .SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario =
                claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>
                (jsonUsuario);
            return await this.repo.GetPedidosAsync(usuario.IdUsuario);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Usuario>> NewPedido(Pedido pedido)
        {
            Claim claim = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario =
                claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>
                (jsonUsuario);
            await this.repo.NewPedidoAsync(pedido.IdCubo, usuario.IdUsuario, pedido.FechaPedido);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> PerfilUsuario()
        {
            Claim claim = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario =
                claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>
                (jsonUsuario);
            return usuario;
        }
    }
}
