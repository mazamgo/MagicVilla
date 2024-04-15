using static MagicVilla_Utilidades.DS;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET;

        public string Url { get; set; }

        public object Datos {  get; set; }

        public string Token { get; set; }

        public Parametros Parametros { get; set; }
    }

    public class Parametros
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }


}
