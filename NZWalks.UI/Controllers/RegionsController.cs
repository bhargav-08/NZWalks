using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // GET: Regions
        public async Task<ActionResult> Index()
        {
            var response = new List<RegionDto>();
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponse = await client.GetAsync("https://localhost:7210/api/regions");
                httpResponse.EnsureSuccessStatusCode();

                response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

            }
            catch (Exception ex)
            {
                // Log the Exception
                throw;
            }
            return View(response);

        }


        // GET: Regions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddRegionViewModel request)
        {

            var client = httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
                RequestUri = new Uri("https://localhost:7210/api/regions")
            };

            var httpResponse = await client.SendAsync(httpRequestMessage);
            httpResponse.EnsureSuccessStatusCode();

            var response = httpResponse.Content.ReadFromJsonAsync<AddRegionViewModel>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        // GET: RegionController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7210/api/regions/{id}");

            if (response is not null)
            {
                return View(response);
            }
            return View();
        }



        // POST: Regions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
                    RequestUri = new Uri($"https://localhost:7210/api/regions/{id}")
                };

                var httpResponse = await client.SendAsync(httpRequestMessage);
                httpResponse.EnsureSuccessStatusCode();

                var response = httpResponse.Content.ReadFromJsonAsync<AddRegionViewModel>();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(null);
            }
        }


        // POST: Regions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponse = await client.DeleteAsync($"https://localhost:7210/api/regions/{id}");
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
