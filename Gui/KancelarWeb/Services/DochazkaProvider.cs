using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using Microsoft.AspNetCore.Blazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class DochazkaProvider : IDochazkaProvider
    {
        public string baseUri = "http://dochazkaapi/Dochazka/";
        public async Task<List<DochazkaModel>> GetList()
        {
            IEnumerable<UdalostModel> res = new List<UdalostModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.GetAsync("GetList");
            
                if (response != null)
                {
                    var jsonString =await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<DochazkaModel>>(jsonString);
                }
            }
            return null;
        
        }
        public T Get<T>(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            object result = client.GetJsonAsync<T>(string.Format("Get/{0}",id));
            return (T)(object)result;
        }
        public async Task Add<T>(T model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                await client.PutAsJsonAsync("Add", model);
            }
        }
        public async Task Update<T>(T model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                await client.PostAsJsonAsync("Update", model);

            }
        }
        public async Task Remove(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                await client.DeleteAsync(string.Format("Remove/{0}", id));

            }

        }


    }
}
