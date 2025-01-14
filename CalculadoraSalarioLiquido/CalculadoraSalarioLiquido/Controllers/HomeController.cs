using System.Diagnostics;
using Application.DTO.Request;
using Application.Services;
using CalculadoraSalarioLiquido.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraSalarioLiquido.Controllers
{
    public class HomeController : Controller
    {

        #region Construtor
        private readonly ILogger<HomeController> _logger;
        private readonly CalculoService _calculoService;
        public HomeController(ILogger<HomeController> logger, CalculoService calculoService)
        {
            _calculoService = calculoService;
            _logger = logger;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CalculoSalarioRequest request)
        {
            var result = _calculoService.ObterValorSalarioLiquido(request);

            ViewBag.SalarioLiquido = result;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
