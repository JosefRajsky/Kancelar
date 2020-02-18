using CommandHandler;
using EventLibrary;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class UdalostProvider : IUdalostProvider
    {
        public string baseUri = "http://udalostapi/Udalost/";
        public async Task<IEnumerable<UdalostModel>> GetList()
        {
            IEnumerable<UdalostModel> res = new List<UdalostModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.GetAsync("GetList");

                if (response.IsSuccessStatusCode)
                {
                    var readtask = response.Content.ReadAsAsync<IList<UdalostModel>>();
                    readtask.Wait();

                    res = readtask.Result;
                }
            }
            return res;

        }
        public UdalostModel Get(int id)
        {
            UdalostModel Udalost = new UdalostModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = client.GetAsync("/get?id=" + id);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<UdalostModel>();
                    readtask.Wait();
                    Udalost = readtask.Result;
                }
            }
            return Udalost;
        }
        public async Task Add(UdalostModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.PutAsJsonAsync("Add", model);                
            }
        }
        public async Task Update(UdalostModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.PostAsJsonAsync("Update", model);
            }
        }
        public async Task Remove(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.DeleteAsync(string.Format("Remove", id));
                
            }

        }
    }
}
