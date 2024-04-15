using AutoMapper;
using MagicVilla_Utilidades;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IVillaService villaService, IMapper mapper)
        {
            _logger = logger;
            _villaService = villaService;
            _mapper = mapper;
        }

        //Original antes de agregar el paginado:

        //public async Task<IActionResult> Index()
        //{
        //    List<VillaDto> villaList = new();

        //    var token = HttpContext.Session.GetString(DS.SessionToken);
        //    var response = await _villaService.ObtenerTodos<APIResponse>(token);

        //    if (response != null && response.IsExitoso)
        //    {
        //        villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado));
        //    }

        //    return View(villaList);
        //}

        public async Task<IActionResult> Index(int pageNumber=1 )
        {
            List<VillaDto> villaList = new();
            VillaPaginadoViewModel villaVM = new VillaPaginadoViewModel();

            if(pageNumber < 1) pageNumber = 1;

            var token = HttpContext.Session.GetString(DS.SessionToken);
            var response = await _villaService.ObtenerTodosPaginado<APIResponse>(token,pageNumber,4);

            if (response != null && response.IsExitoso) 
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado));
                villaVM = new VillaPaginadoViewModel()
                {
                    VillaList = villaList,
                    PageNumber = pageNumber,
                    TotalPaginas = JsonConvert.DeserializeObject<int>(Convert.ToString(response.TotalPaginas))
                };

                if (pageNumber > 1) villaVM.Previo = "";
                if (villaVM.TotalPaginas <= pageNumber) villaVM.Siguiente = "disabled";

            }

            return View(villaVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
