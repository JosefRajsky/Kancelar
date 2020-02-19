using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class DochazkaProvider : IDochazkaProvider
    {
        public string baseUri = "http://dochazkaapi/Dochazka/";
        public async Task<IEnumerable<DochazkaModel>> GetList()
        {
           
                IEnumerable<DochazkaModel> result = new List<DochazkaModel>();

                using (var client = new HttpClient())
            {
                await Task.Run(() =>
                {
                    client.BaseAddress = new Uri(baseUri);
                    var response = client.GetAsync("GetList").Result; 

               
                        var readtask = response.Content.ReadAsAsync<IList<DochazkaModel>>();
                       

                        result = readtask.Result;
                   
                });
                    return result;
               
            }
         
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
                 await client.PutAsJsonAsync("Add", model); 
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
                var response = await client.DeleteAsync(string.Format("Remove?id={0}", id));
               
            }

        }


        
    }
}
