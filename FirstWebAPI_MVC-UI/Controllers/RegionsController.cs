using FirstWebAPI_MVC_UI.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace FirstWebAPI_MVC_UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Regions/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // API endpoint URL
                string apiUrl = "https://localhost:44360/api/Regions"; // Update with your API URL
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var regions = JsonConvert.DeserializeObject<IEnumerable<RegionDto>>(responseContent);

                return View(regions);
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exception
                return BadRequest();
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44360/api/Regions");

                var jsonContent = JsonConvert.SerializeObject(model);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var region = JsonConvert.DeserializeObject<RegionDto>(responseContent);

                if (region != null)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exception
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:44360/api/Regions/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<RegionDto>(content);
                return View(model);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                // Handle other error cases, such as unauthorized, bad request, etc.
                // You can return a specific error view or handle the error as needed.
                return View("Error"); // Return an error view
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44360/api/Regions/{model.ID}");

                var jsonContent = JsonConvert.SerializeObject(model);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exception
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:44360/api/Regions/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // Region deleted successfully, redirect to Index
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Region not found, return NotFound view
                    return NotFound();
                }
                else
                {
                    // Handle other error cases, such as unauthorized, bad request, etc.
                    // You can return a specific error view or handle the error as needed.
                    return View("Error"); // Return an error view
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exception
                return BadRequest();
            }
        }
    }
}

