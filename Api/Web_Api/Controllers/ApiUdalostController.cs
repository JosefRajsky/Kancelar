using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdalostLibrary.Models;


namespace Web_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ApiUdalostController : ControllerBase
    {
        string _BaseUrl;
        public ApiUdalostController()
        {
            _BaseUrl = "http://udalostapi/udalost/";
        }
        [HttpGet]
        [Route("Get")]
        public T Get<T>(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            return (T)(object)client.GetAsync(string.Format("Get/{0}", id));
        }
        [HttpGet]
        [Route("GetList")]
        public async Task<string> GetListAsync()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_BaseUrl)
            };
            var response = await client.GetStringAsync("GetList");
            return response;
        }
        [HttpPost]
        [Route("Add")]
        public async Task Add(UdalostModel msg)
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
        public async Task Update(UdalostModel msg)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            await client.PostAsJsonAsync("Update", msg);
        }
    }
}
