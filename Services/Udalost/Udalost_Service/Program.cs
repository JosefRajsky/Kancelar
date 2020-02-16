using CommandHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Text;
using Udalost_Service.Repositories;

namespace Udalost_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //-------------Description: Načtení konfiguračního souboru
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                //-------------Description: Zápis o konzumaci RabbitMq Exchange pro konzumaci publikovaných zpráv
                //-------------Description: Název Exchange získán z konfiguračního souboru appsetting.json
                var consumer = new ServiceCollection()
                    .AddSingleton<IAcceptCommand>(s => new AcceptCommand(new ConnectionFactory() { HostName = "rabbitmq" }, config.GetValue<string>("Setting:Exchange")))
                    .BuildServiceProvider()
                    .GetService<IAcceptCommand>()
                    .Start();
                consumer.Received += (model, ea) =>
                {
                    //-------------Description: Formátování přijaté zprávy
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //-------------Description: Vytvoření repositáře pro přístup k entitám služby.
                    //-------------Description: Název ConnectionString získán z konfiguračního souboru appsetting.json
                    var repository = new UdalostRepository(config.GetValue<string>("Setting:ConnectionString"));
                    //-------------Description: Odeslání zprávy do repositáře
                    repository.AddMessage(message);
                };
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
