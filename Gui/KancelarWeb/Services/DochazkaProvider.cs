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
        public string DochazkaBase = "http://dochazkaapi/Dochazka/";
        public IEnumerable<DochazkaModel> GetList()
        {
            IEnumerable<DochazkaModel> res = new List<DochazkaModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DochazkaBase);
                var response = client.GetAsync("GetList");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<IList<DochazkaModel>>();
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
                client.BaseAddress = new Uri(DochazkaBase);
                var response = client.GetAsync("/get?id=" + id);
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
                client.BaseAddress = new Uri(DochazkaBase);
                await client.PutAsJsonAsync("Add", model);                
            }
        }
        public async Task Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DochazkaBase);
                await client.DeleteAsync(string.Format("Delete?id={0}", id));
            }
            //Odeslani Command Create
            //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            //var publisher = new PublishCommand(factory, "dochazka.ex");
            //var body = JsonConvert.SerializeObject(
            //     new EventDochazkaRemove()
            //     {
            //         DochazkaId = id
            //     });
            //publisher.Push(body);         
            
            

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(DochazkaBase);
            //    var responseTask = client.DeleteAsync(string.Format("Delete?id={0}", id));
            //    responseTask.Wait();                
            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        
    }
}
