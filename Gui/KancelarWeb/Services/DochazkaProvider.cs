using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public bool Add(DochazkaModel dochazka)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DochazkaBase);
                var responseTask = client.PutAsJsonAsync("Add", dochazka);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DochazkaBase);
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
