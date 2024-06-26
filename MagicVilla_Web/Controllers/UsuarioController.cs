﻿using MagicVilla_Utilidades;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly IUsuarioService _usuarioService;
        
		public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto modelo)
		{
			var response = await _usuarioService.Login<APIResponse>(modelo);
			if (response != null && response.IsExitoso)
			{
				LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Resultado));

				//Vamos a Leer el token. ya con el token obtendriamos el usuario y el Rol por lo que ya no seria necesario traerlos.
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(loginResponse.Token);

				//Claims para mantener username y el rol en todo momento en nuestra aplicacion.
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
               
				//identity.AddClaim(new Claim(ClaimTypes.Name, loginResponse.Usuario.UserName));
                //identity.AddClaim(new Claim(ClaimTypes.Role, loginResponse.Usuario.Rol));

                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
				identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				//Almacenar el Token en una variable de session.
				HttpContext.Session.SetString(DS.SessionToken, loginResponse.Token);              
                return RedirectToAction("Index", "Home");

			}
			else
			{
				ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
				return View(modelo);

			}


		}

		public IActionResult Registrar()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registrar(RegistroRequestDto modelo)
		{
			var response = await _usuarioService.Registrar<APIResponse>(modelo);

			if(response != null && response.IsExitoso ) 
			{ 
				return RedirectToAction("login");
			}

			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(DS.SessionToken, "");

			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccesoDenegado()
		{
			return View();
		}


	}
}
