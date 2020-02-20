using KancelarWeb.Interfaces;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Blazor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class ApiProvider : IApiProvider
    {
        private string _apiUri;
        public ApiProvider() {
            _apiUri = "http://webapi/";
        }
        public async Task<T> _convertResponse<T>(HttpResponseMessage response) {
            if (response != null)
            {
                var jsonString = await response.Content.ReadAsStringAsync();              
                return (T)(object)jsonString;
            }
            return (T)(object)string.Empty;
        }
        public async Task<T> List<T>(string controller)
        {           
                var client = new HttpClient();
                var host = string.Format("{0}{1}/", _apiUri, controller);
                client.BaseAddress = new Uri(host);
           

               return await _convertResponse<T>(await client.GetAsync("GetList"));    
        }
        public async Task<T> Get<T>(int id, string controller)
        {
            var client = new HttpClient();
            var host = string.Format("{0}{1}/", _apiUri, controller);
            client.BaseAddress = new Uri(host);
            return await _convertResponse<T>(await client.GetAsync(string.Format("Get/{0}", id)));            
        }
        public async Task Add<T>(T model, string controller)
        {
            var client = new HttpClient();
            var host = string.Format("{0}{1}/", _apiUri, controller);
            client.BaseAddress = new Uri(host);    
            await client.PostAsJsonAsync("Add", model);          
        }
        public async Task Update<T>(T model, string controller)
        {
            var client = new HttpClient();
            var host = string.Format("{0}{1}/", _apiUri, controller);
            client.BaseAddress = new Uri(host);
            await client.PutAsJsonAsync("Update", model);
        }
        public async Task Remove(int id, string controller)
        {
            var client = new HttpClient();
            var host = string.Format("{0}{1}/", _apiUri, controller);
            client.BaseAddress = new Uri(host);
            await client.DeleteAsync(string.Format("Remove/{0}", id));
        }

       
    }
}
