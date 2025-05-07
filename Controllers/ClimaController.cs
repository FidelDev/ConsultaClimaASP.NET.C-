// Controllers/ClimaController.cs
using Microsoft.AspNetCore.Mvc;
using Clima.Models;
using System.Net.Http;
using System.Text.Json;

namespace Clima.Controllers
{
    public class ClimaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ClimaController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarClima(string ciudad)
        {
            if (string.IsNullOrWhiteSpace(ciudad))
            {
                ViewBag.Error = "Por favor, ingrese una ciudad.";
                return View("Index");
            }

            string apiKey = _configuration["OpenWeatherMap:ApiKey"];
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var clima = JsonSerializer.Deserialize<ClimaModel>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (clima == null || clima.Main == null || clima.Weather == null)
                {
                    ViewBag.Error = "No se pudo obtener la información del clima. Verifica el nombre de la ciudad.";
                    return View("Index");
                }

                return View("Resultado", clima);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo obtener el clima para la ciudad ingresada.";
                return View("Index");
            }
        }
    }
}
