using CommandHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Udalost_Service.Repositories;

namespace Udalost_Service
{
    class Program
    {
        static void Main(string[] args)
        {
           
                //-------------Description: Načtení konfiguračního souboru
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                //-------------Description: Zápis o konzumaci RabbitMq Exchange pro konzumaci publikovaných zpráv
                //-------------Description: Název Exchange získán z konfiguračního souboru appsetting.json
                var consumer = new ServiceCollection()
                    .AddSingleton<ISubscriber>(s => new Subscriber(new ConnectionFactory() { HostName = "rabbitmq" }, config.GetValue<string>("Setting:Exchange")))
                    .BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
                consumer.Received += (model, ea) =>
                {
                    //-------------Description: Formátování přijaté zprávy
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //-------------Description: Vytvoření repositáře pro přístup k entitám služby.
                    //-------------Description: Název ConnectionString získán z konfiguračního souboru appsetting.json
                    var repository = new UdalostServiceRepository();
                    //-------------Description: Odeslání zprávy do repositáře
                  repository.AddCommand(message);
                };
           
        }
    }
}
