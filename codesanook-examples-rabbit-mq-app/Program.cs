using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace Codesanook.Examples.RabbitMQApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "mq-server",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            const string queueName = "hello";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine("Sender is connected...");
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello", // It is required,  we need to check if we have a way to ignore it.
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($">>> Sent: {message}");
            }

            // wait for message sent
            Thread.Sleep(1 * 1000);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine("Receiver is connected...");
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($">>> Received: {message}");
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer
                );

                // Wait for message consumed
                Thread.Sleep(1 * 1000);
            }

            Console.WriteLine("Exit the app");
        }
    }
}
