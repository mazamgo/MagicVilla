﻿using System.Net;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public APIResponse() 
        { 
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode statusCode;
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
