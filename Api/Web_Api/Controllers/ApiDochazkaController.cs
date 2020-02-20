using System;
using System.Net.Http;
using System.Threading.Tasks;

using DochazkaLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class ApiDochazkaController : ControllerBase
    {
        string _BaseUrl;
        public ApiDochazkaController()
        {
            _BaseUrl = "http://dochazkaapi/dochazka/";
        }
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult> Get(int id)
        {
            var client = new HttpClient();           
            client.BaseAddress = new Uri(_BaseUrl);
            var response = await client.GetAsync(string.Format("Get/{0}",id));
            return Ok(await response.Content.ReadAsStringAsync());
        }
        [HttpGet]
        [Route("GetList")]
        public async Task<ActionResult> GetListAsync() {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_BaseUrl)
            };
            var response = await client.GetAsync("GetList"); 
                return Ok(await response.Content.ReadAsStringAsync());
        }
        [HttpPost]
        [Route("Add")]
        public async Task Add(DochazkaModel msg)      
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            await client.PostAsJsonAsync("Add", msg);
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            await client.DeleteAsync(string.Format("Remove/{0}", id));
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(DochazkaModel msg)
        {
            var client = new HttpClient();            
            client.BaseAddress = new Uri(_BaseUrl);
            await client.PostAsJsonAsync("Update", msg);
        }
    }
}
