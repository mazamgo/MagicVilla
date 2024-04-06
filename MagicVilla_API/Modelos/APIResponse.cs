using System.Net;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode;
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
