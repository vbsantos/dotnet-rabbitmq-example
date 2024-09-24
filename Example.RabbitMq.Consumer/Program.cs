using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Serilog;

namespace Example.RabbitMq.Consumer;

class Program
{
    static void Main(string[] args)
    {
        // Get RabbitMQ host name from environment variables or default to 'localhost'
        var rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQ__HostName") ?? "localhost";
        var seqUrl = Environment.GetEnvironmentVariable("Seq__Url") ?? "localhost";

        // Configure Serilog to send logs to Seq
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq(seqUrl)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Context", "Consumer")
            .CreateLogger();

        Log.Information("Logging configuration applied. Seq URL: {seqUrl}", seqUrl);

        try
        {
            // Create a connection factory
            var factory = new ConnectionFactory() { HostName = rabbitMQHost };

            // Create a connection
            using var connection = factory.CreateConnection();

            // Create a channel
            using var channel = connection.CreateModel();

            // Declare a queue
            channel.QueueDeclare(
                queue: "example-rabbitmq-queue-name",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Create a consumer
            var consumer = new EventingBasicConsumer(channel);

            // Handle received messages
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Log.Information("Received {0}", message);
            };

            // Start consuming messages
            channel.BasicConsume(
                queue: "example-rabbitmq-queue-name",
                autoAck: true,
                consumer: consumer
            );

            Log.Information("Consumer is running.");

            // Keep the consumer running indefinitely
            Thread.Sleep(Timeout.Infinite);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred");
        }

        Log.CloseAndFlush();
    }
}
