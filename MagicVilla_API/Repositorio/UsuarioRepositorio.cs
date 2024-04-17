using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositorio
{
	public class UsuarioRepositorio : IUsuarioRepositorio
	{
		private readonly ApplicationDbContetxt _db;
		private string secretKey;
		//Linea agregada despues de la inst. paquete Identity...
		private readonly UserManager<UsuarioAplicacion> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContetxt db, IConfiguration configuration, UserManager<UsuarioAplicacion> userManager,
			IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
			_userManager = userManager;	
			_mapper = mapper;
			_roleManager = roleManager;
        }

		//No es asincrono.
        public bool IsUsuarioUnico(string userName)
		{
			//var usuario = _db.Usuarios.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            var usuario = _db.UsuariosAplicacion.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            if (usuario == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
            //var usuario = await _db.Usuarios.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower() &&
            //                         x.Password == loginRequestDTO.Password);

            var usuario = await _db.UsuariosAplicacion.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

			//Se agrego nueva forma de validar el Password.
			bool IsValido = await _userManager.CheckPasswordAsync(usuario,loginRequestDTO.Password);

			if (usuario == null || IsValido == false)
			{
				return new LoginResponseDTO() 
				{ 
					Token = "",
					Usuario = null
				};
			}

			//Ahora con inst. Paquete Identity los Roles se guardan en una tabla Aparte.
			var roles = await _userManager.GetRolesAsync(usuario);

			//Si el usuario existe generamos JW token.
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
				{
					//new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName),
					//new Claim(ClaimTypes.Role, usuario.Rol)
					new Claim(ClaimTypes.Role, roles.FirstOrDefault())
				}),

				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			LoginResponseDTO loginResponseDTO = new() 
			{
				Token = tokenHandler.WriteToken(token),
				//Usuario = usuario
				Usuario = _mapper.Map<UsuarioDto>(usuario),
				//Rol = roles.FirstOrDefault()
			};

			return loginResponseDTO;

		}

		//Ahora el Retorno va hacer de UsuarioDto
		public async Task<UsuarioDto> Registrar(RegistroRequestDTO registroRequestDTO)
		{
		//	Usuario usuario = new()
		//	{
		//		UserName = registroRequestDTO.UserName,
		//		Password = registroRequestDTO.Password,
		//		Nombres = registroRequestDTO.Nombres,
		//		Rol = registroRequestDTO.Rol
		//	};

            UsuarioAplicacion usuario = new()
            {
                UserName = registroRequestDTO.UserName,
                Email = registroRequestDTO.UserName,
				NormalizedEmail = registroRequestDTO.UserName.ToUpper(),
                Nombres = registroRequestDTO.Nombres
             
            };

			// await _db.Usuarios.AddAsync(usuario);
			//await _db.SaveChangesAsync();
			//usuario.Password = "";
			//return usuario;

			try
			{
				var resultado = await _userManager.CreateAsync(usuario, registroRequestDTO.Password);
				if (resultado.Succeeded)
				{
					if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					{
						await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("cliente"));
                    }

					await _userManager.AddToRoleAsync(usuario, "admin");
					var usuarioAP = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName == registroRequestDTO.UserName);
					return _mapper.Map<UsuarioDto>(usuarioAP);
				}


			}
			catch (Exception )
			{

				throw;
			}
			return new UsuarioDto();

		}
	}
}
