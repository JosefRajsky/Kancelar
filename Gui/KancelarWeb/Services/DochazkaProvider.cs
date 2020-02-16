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
        public void Add(DochazkaModel model)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "dochazka.ex");
            var body = JsonConvert.SerializeObject(
                   new EventDochazkaCreate()
                   {
                       Prichod = model.Prichod,
                       UzivatelId = model.UzivatelId,
                       CteckaId = model.CteckaId,
                       Datum = DateTime.Now,
                   });
            publisher.Push(body);


            //Odeslani Command Create
            //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
            //       new EventDochazkaCreate()
            //       {
            //           Prichod = model.Prichod,
            //           UzivatelId = model.UzivatelId,
            //           CteckaId = model.CteckaId,
            //           Datum = DateTime.Now,
            //       }));
            //    var ex = "dochazka.ex";
            //    channel.ExchangeDeclare(ex, ExchangeType.Fanout);
                
            //    channel.BasicPublish(
            //         exchange: ex,
            //         routingKey: "",
            //         basicProperties: null,
            //         body: body);
            //    var queueName = channel.QueueDeclare().QueueName;
            //    channel.QueueBind(queue: queueName,
            //      exchange: ex,
            //      routingKey: "");

            //    return;
            //}
            //Description: Původni komunikace na přímo.
            //puvodni komunikace
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(DochazkaBase);
            //    var responseTask = client.PutAsJsonAsync("Add", model);
            //    responseTask.Wait();
            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //}
            
        }
        public void Delete(int id)
        {
            //Odeslani Command Create
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "dochazka.ex");
            var body = JsonConvert.SerializeObject(
                 new EventDochazkaRemove()
                 {
                     DochazkaId = id
                 });
            publisher.Push(body);         
            
            

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
