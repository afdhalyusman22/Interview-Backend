using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Moduit.Interview.Models;

namespace Moduit.Interview.Controller
{
    [Route("api/backend")]
    [ApiController]
    public class BackEndController : ControllerBase
    {
        [HttpGet, Route("question/one")]
        public async Task<IActionResult> questionone()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://screening.moduit.id/backend/question/one"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<ClassOne>(apiResponse);
                        return Ok(result);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet, Route("question/two")]
        public async Task<IActionResult> questiontwo()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://screening.moduit.id/backend/question/two"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<List<ClassTwo>>(apiResponse);

                        result = result.Where(x => (x.description.Contains("Ergonomics") || x.title.Contains("Ergonomics")) && x.tags != null && x.tags.Any(s => s.Contains("Sport")))
                            .OrderByDescending(x => x.id).TakeLast(3).ToList();


                        return Ok(result);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet, Route("question/three")]
        public async Task<IActionResult> questionthree()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://screening.moduit.id/backend/question/three"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<List<ClassThree>>(apiResponse);

                        var flatList = result.Where(o => o.items != null).SelectMany(x => x.items.Select(y => new
                        {
                            id = x.id,
                            category = x.category,
                            title = y.title,
                            description = y.description,
                            footer = y.footer,
                            createdAt = x.createdAt
                        })).ToList();
                        return Ok(flatList);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
