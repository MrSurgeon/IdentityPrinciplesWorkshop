using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityPrinciplesWorkshop.Client1.Models;
using IdentityPrinciplesWorkshop.Client1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IdentityPrinciplesWorkshop.Client1.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IApiResourceHttpClient _apiResourceHttpClient;
        private IConfiguration Configuration { get; }
        public ProductController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            Configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = await _apiResourceHttpClient.GetHttpClientAsync();
            var response = await httpClient.GetAsync("https://localhost:5006/api/product");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var productList = JsonConvert.DeserializeObject<List<Product>>(content);
                //başarılı
            }
            //başarısız
            return View();
        }
    }
}
