using MagicVilla_Utilidades;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Microsoft.Extensions.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient {  get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");

                //validar que nuestro parametros esten llenos.
                if(apiRequest.Parametros == null)
                {
                    message.RequestUri = new Uri(apiRequest.Url);
                }
                else 
                { 
                    //Esto es para que los parametros sean parte del url.
                    var builder = new UriBuilder(apiRequest.Url);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["PageNumber"] = apiRequest.Parametros.pageNumber.ToString();
                    query["PageSize"] = apiRequest.Parametros.pageSize.ToString();

                    builder.Query = query.ToString();
                    string url = builder.ToString();      //api/Villa/VillaPaginado/PageNumber=1&PageSize=4
                    message.RequestUri = new Uri(url);
                }


                

                if (apiRequest.Datos != null)
                {
                    //Se va tratar post o un pu y se necesita enviar contenido, entonces se convierte en json.
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos),
                        Encoding.UTF8, "application/json");
                }

                switch(apiRequest.APITipo)
                {
                    case DS.APITipo.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case DS.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    //Si el token no es null
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                //var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                //return APIResponse;

                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    //Linea Orginal antes del Versionado
                    //if (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound)

                    if (response != null && 
                        (apiResponse.StatusCode == HttpStatusCode.BadRequest 
                        || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.IsExitoso = false;
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);
                        return obj;

                    }
                
                }
                catch (Exception ex)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return errorResponse;                    
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsExitoso = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var responseEX = JsonConvert.DeserializeObject<T> (res);
                return responseEX;
            }
            
        }
    }
}
