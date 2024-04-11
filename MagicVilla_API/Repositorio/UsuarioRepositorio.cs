﻿using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
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

        public UsuarioRepositorio(ApplicationDbContetxt db, IConfiguration configuration)
        {
            _db = db;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

		//No es asincrono.
        public bool IsUsuarioUnico(string userName)
		{
			var usuario = _db.Usuarios.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());

			if (usuario == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var usuario = await _db.Usuarios.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower() && 
			                         x.Password == loginRequestDTO.Password);

			if (usuario == null)
			{
				return new LoginResponseDTO() 
				{ 
					Token = "",
					Usuario = null
				};
			}

			//Si el usuario existe generamos JW token.
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, usuario.Id.ToString()),
					new Claim(ClaimTypes.Role, usuario.Rol)
				}),

				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			LoginResponseDTO loginResponseDTO = new() 
			{
				Token = tokenHandler.WriteToken(token),
				Usuario = usuario
			};

			return loginResponseDTO;

		}

		public async Task<Usuario> Registrar(RegistroRequestDTO registroRequestDTO)
		{
			Usuario usuario = new()
			{
				UserName = registroRequestDTO.UserName,
				Password = registroRequestDTO.Password,
				Nombres = registroRequestDTO.Nombres,
				Rol = registroRequestDTO.Rol
			};

			await _db.Usuarios.AddAsync(usuario);
			await _db.SaveChangesAsync();

			usuario.Password = "";

			return usuario;

		}
	}
}
