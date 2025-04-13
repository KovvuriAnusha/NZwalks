using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> regions = new List<RegionDTO>();
            try
            {
                //Get all regions fromweb API

                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7204/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                ViewBag.responseBody = stringResponseBody;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    regions.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDTO>>());
                    return View(regions);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                }
                return View(regions);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(regions);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = httpClientFactory.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Post,
                            "https://localhost:7204/api/regions")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(model),
                    Encoding.UTF8,
                    "application/json")
            };

            var resp = await client.SendAsync(req);
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API call failed");
                return View(model);
            }

            // **Read a single RegionDTO** instead of a list
            var createdRegion = await resp.Content.ReadFromJsonAsync<RegionDTO>();
            if (createdRegion == null)
            {
                ModelState.AddModelError("", "Could not parse response");
                return View(model);
            }

            // Optionally: use createdRegion.Id or other data
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ////ViewBag.Id = id;
            ////return View();


            var client = httpClientFactory.CreateClient();
            //var httpResponseMessage = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7204/api/regions{id.ToString()}");
            //if(httpResponseMessage is not null)
            //{
            //    return View(httpResponseMessage);
            //}
            //return View(null);
            var req = new HttpRequestMessage(HttpMethod.Get,
                            $"https://localhost:7204/api/regions/{id}");
            var resp = await client.SendAsync(req);
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API call failed");
                return View(new RegionDTO());
            }
            var region = await resp.Content.ReadFromJsonAsync<RegionDTO>();
            if (region == null)
            {
                ModelState.AddModelError("", "Could not parse response");
                return View(new RegionDTO());
            }
            return View(region);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO request)
        {
            var client = httpClientFactory.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Put,
                            $"https://localhost:7204/api/regions/{request.Id}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json")
            };
            var httpResponseMessage = await client.SendAsync(req);
            httpResponseMessage.EnsureSuccessStatusCode();
            var responseBody = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (responseBody is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7204/api/regions/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }
            return View("Edit");
        }
    }
}
