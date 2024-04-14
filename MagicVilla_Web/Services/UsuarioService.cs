using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
	public class UsuarioService : BaseService, IUsuarioService
	{
		//Para hacer la conexiones.
		public readonly IHttpClientFactory _httpClient;
		private string _villaUrl;

        public UsuarioService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
			_villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL"); //ServiceUrls:API_URL
        }

        public Task<T> Login<T>(LoginRequestDto dto)
		{
			return SendAsync<T>(new Models.APIRequest() { 
				
				APITipo = MagicVilla_Utilidades.DS.APITipo.POST,
				Datos = dto,
				Url = _villaUrl +"/api/v1/Usuario/login"
				
			});
		}

		public Task<T> Registrar<T>(RegistroRequestDto dto)
		{
			return SendAsync<T>(new Models.APIRequest()
			{

				APITipo = MagicVilla_Utilidades.DS.APITipo.POST,
				Datos = dto,
				Url = _villaUrl + "/api/v1/Usuario/registrar"

			});
		}
	}
}
