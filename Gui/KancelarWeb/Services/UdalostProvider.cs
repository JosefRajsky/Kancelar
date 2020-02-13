﻿using KancelarWeb.Interfaces;
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
        public IEnumerable<UdalostModel> GetList()
        {
            IEnumerable<UdalostModel> res = new List<UdalostModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var response = client.GetAsync("GetList");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<IList<UdalostModel>>();
                    readtask.Wait();

                    res = readtask.Result;
                }
            }
            return res;
        }
        public UdalostModel Get(int id)
        {
            UdalostModel udalost = new UdalostModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(udalostBase);
                var response = client.GetAsync("/get?id=" + id);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<UdalostModel>();
                    readtask.Wait();
                    udalost = readtask.Result;
                }
            }
            return udalost;
        }
        public bool Add(UdalostModel udalost)
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
        public bool Delete(int id)
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
