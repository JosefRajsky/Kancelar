using KancelarWeb.Interfaces;
using KancelarWeb.Models;
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
        public async Task<T> _convertResponse<T>(HttpResponseMessage response) {
            if (response != null)
            {
                var jsonString = await response.Content.ReadAsStringAsync();              
                return (T)(object)jsonString;
            }
            return (T)(object)string.Empty;
        }
        public async Task<T> List<T>(string baseUri)
        {           
                var client = new HttpClient();           
                client.BaseAddress = new Uri(baseUri);               
               return await _convertResponse<T>(await client.GetAsync("GetList"));    
        }
        public async Task<T> Get<T>(int id, string baseUri)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            return await _convertResponse<T>(await client.GetAsync(string.Format("Get/{0}", id)));            
        }
        public async Task Add<T>(T model, string baseUri)
        {
                var client = new HttpClient();           
                client.BaseAddress = new Uri(baseUri);
                await client.PutAsJsonAsync("Add", model);          
        }
        public async Task Update<T>(T model, string baseUri)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            await client.PostAsJsonAsync("Update", model);
        }
        public async Task Remove(int id, string baseUri)
        {
            var client = new HttpClient();          
            client.BaseAddress = new Uri(baseUri);
            await client.DeleteAsync(string.Format("Remove/{0}", id));
        }

       
    }
}
