

//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Web_Api.ApiProvider
//{
//    public class ApiProvider : IApiProvider
//    {
//        private string _apiUri;
//        public ApiProvider(string api, string controller) {
//            _apiUri = string.Format("http://{0}/{1}/", api, controller) ;
//        }
//        public async Task<T> _convertResponse<T>(HttpResponseMessage response) {
//            if (response != null)
//            {
//                var jsonString = await response.Content.ReadAsStringAsync();              
//                return (T)(object)jsonString;
//            }
//            return (T)(object)string.Empty;
//        }

      
//        public async Task<T> List<T>()
//        {           
//                var client = new HttpClient();
//                var host = string.Format(_apiUri);
//                client.BaseAddress = new Uri(host);
           

//               return await _convertResponse<T>(await client.GetAsync("GetList"));    
//        }

//        public async Task<T> Get<T>(int id)
//        {
//            var client = new HttpClient();
//            var host = string.Format(_apiUri);
//            client.BaseAddress = new Uri(host);
//            return await _convertResponse<T>(await client.GetAsync(string.Format("Get/{0}", id)));            
//        }
//        public async Task Add<T>(T model)
//        {
//            var client = new HttpClient();
//            var host = string.Format(_apiUri);
//            client.BaseAddress = new Uri(host);    
//            await client.PostAsJsonAsync("Add", model);          
//        }
//        public async Task Update<T>(T model)
//        {
//            var client = new HttpClient();
//            var host = string.Format(_apiUri);
//            client.BaseAddress = new Uri(host);
//            await client.PutAsJsonAsync("Update", model);
//        }
//        public async Task Remove(int id)
//        {
//            var client = new HttpClient();
//            var host = string.Format(_apiUri);
//            client.BaseAddress = new Uri(host);
//            await client.DeleteAsync(string.Format("Remove/{0}", id));
//        }

        
//    }
//}
