using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
    public class UdalostProvider : IUdalostProvider
    {
        public string udalostBase = "http://udalostapi/Udalost/";
        public IEnumerable<Udalost> UdalostList()
        {
            IEnumerable<Udalost> res = new List<Udalost>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var response = client.GetAsync("list");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<IList<Udalost>>();
                    readtask.Wait();

                    res = readtask.Result;
                }
            }
            return res;
        }
        public Udalost UdalostGet(int id)
        {
            Udalost udalost = new Udalost();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var response = client.GetAsync("/get?id=" + id);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<Udalost>();
                    readtask.Wait();
                    udalost = readtask.Result;
                }
            }
            return udalost;
        }
        public bool UdalostAdd(Udalost udalost)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var responseTask = client.PutAsJsonAsync("Add", udalost);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
        public bool UdalostDelete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var responseTask = client.DeleteAsync(string.Format("Delete?id={0}", id));
                responseTask.Wait();                
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
