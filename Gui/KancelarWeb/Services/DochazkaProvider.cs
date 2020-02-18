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
using System.Text;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class DochazkaProvider : IDochazkaProvider
    {
        public string baseUri = "http://dochazkaapi/Dochazka/";
        public async Task<IEnumerable<DochazkaModel>> GetList()
        {
            IEnumerable<DochazkaModel> res = new List<DochazkaModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.GetAsync("GetList");
             
                if (response.IsSuccessStatusCode)
                {
                    var readtask = response.Content.ReadAsAsync<IList<DochazkaModel>>();
                    readtask.Wait();

                    res = readtask.Result;
                }
            }
            return res;
        }
        public DochazkaModel Get(int id)
        {
            DochazkaModel Dochazka = new DochazkaModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = client.GetAsync("Get" + id);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<DochazkaModel>();
                    readtask.Wait();
                    Dochazka = readtask.Result;
                }
            }
            return Dochazka;
        }
        public async Task Add(DochazkaModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var response = await client.PutAsJsonAsync("Add", model);
              
            }
        }
        public async Task Update(DochazkaModel model)
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
