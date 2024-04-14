using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    //[ApiVersion("1.0")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private APIResponse _response;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepo = usuarioRepositorio;
            _response = new();
        }

        //Se le va modificar la ruta de la siguiente manera: api/usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO modelo)
        {
            var loginResponse = await _usuarioRepo.Login(modelo);
            if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("UserName o Password son Incorrectos");
                return BadRequest(_response);
            }

            _response.IsExitoso = true;
            _response.statusCode = System.Net.HttpStatusCode.OK;
            _response.Resultado = loginResponse;
            return Ok(_response);


        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDTO modelo)
        {
            bool istUsuarioUnico = _usuarioRepo.IsUsuarioUnico(modelo.UserName);

            if (!istUsuarioUnico)
            {
                _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Usuario ya Existe!");
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepo.Registrar(modelo);

            if (usuario == null)
            {
                _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Error al registrar Usuario");
                return BadRequest(_response);
            }

            _response.statusCode = System.Net.HttpStatusCode.OK;
            _response.IsExitoso = true;
            return Ok(_response);
        }
    }
}
