using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CommandHandler
{
    public class Publisher : IPublisher
    {
        private ConnectionFactory _factory { get; set; }
        private IConnection _connection { get; set; }
        private IModel _channel { get; set; }
        private string _exchange { get; set; }
        private string _queue { get; set; }
        //Description: Metoda pro zaslání zprávy na konkrétní Exchange
        public async Task PushToExchange(string exchange,string message)
        {
            await Task.Run(() =>
            {
                    var body = Encoding.UTF8.GetBytes(message);
                     _channel.BasicPublish(
                     exchange: exchange,
                     routingKey: "",
                     basicProperties: null,
                     body: body);
            });
        }
        //Description: Metoda Publikace události
        public async Task Push(string message) {
            await Task.Run(() =>
                {
                    //Description: Rozložení zprávy
                    var body = Encoding.UTF8.GetBytes(message);
                    //Description: Zahájení publikovací pseudostransakce
                    _channel.TxSelect();
                    #region EventStore Exchange              
                    var args = new Dictionary<string, object>();
                    //Description: Nastavení životnosti zprávy v Exchange
                    args.Add("x-message-ttl", 432000);
                    //Description: Ověření existence Exchange Eventstore a jeho případná deklarace
                    _channel.ExchangeDeclare("eventstore.ex", ExchangeType.Fanout, false, false, args);
                    //Description: Publikace zprávy s událostí do Eventstore Exchange
                    _channel.BasicPublish(
                      exchange: "eventstore.ex",
                      routingKey: "",
                      basicProperties: null,
                      body: body);
                    #endregion
                    //Description: Publikace na exchange původce pro ověření
                    _channel.BasicPublish(
                         exchange: _exchange,                        
                         routingKey: "",
                         basicProperties: null,
                         body: body);
                    //Descripiton: Ukončení, ověření a zpracování pseudotransakce
                    _channel.TxCommit();
                }); 
        }    
        //Description: Vytvoření připojení Publikace
        public Publisher(ConnectionFactory connectionFactory, string exchange,string queue)
        {
            //Description: Parametry připojení servisy k publikaci
            this._exchange = exchange;
            this._queue = queue;
            this._factory = connectionFactory;
            this._factory.AutomaticRecoveryEnabled = true;
            this._factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            try
            {
                //Description: Připojení k Message broker
                this._connection = _factory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                //Description: V případě nedosažitelnosti MB, uloživ výjimku, počkat a připojti znovu.
                var exception = e;
                Thread.Sleep(5000);
                this._connection = _factory.CreateConnection();
            }           
            this._channel = _connection.CreateModel();            
            IBasicProperties props = _channel.CreateBasicProperties();
            //Description: nastavení životnosti zpráv v Exchange 
            props.Expiration = "432000";
            var args = new Dictionary<string, object>();
            args.Add("x-message-ttl", 432000);
            //Description: Ověření existence exchange nebo jeho založení
            _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout,false,false,args);

        }


    }
}
