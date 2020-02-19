using System;
using System.Net.Http;
using System.Threading.Tasks;
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
        public T Get<T>(int id)
        {
            var client = new HttpClient();           
            client.BaseAddress = new Uri(_BaseUrl);
            return (T)(object)client.GetAsync(string.Format("Get/{0}",id));

        }
        [HttpGet]
        [Route("GetList")]
        public async Task<string> GetListAsync() {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_BaseUrl)
            };
            var response = await client.GetStringAsync("GetList");
            return response;
        }
        [HttpPost]
        [Route("Add")]
        public void Add(string msg)      
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            
            client.PostAsJsonAsync("Add", msg);
        }

        [HttpDelete]
        [Route("Remove")]
        public T Delete<T>(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            return (T)(object)client.DeleteAsync(string.Format("Remove/{0}", id));
        }
        [HttpPost]
        [Route("Update")]
        public T Update<T>(T msg)
        {
            var client = new HttpClient();            
            client.BaseAddress = new Uri(_BaseUrl);
            return (T)(object)client.PostAsJsonAsync("Update", msg);
        }
    }
}
