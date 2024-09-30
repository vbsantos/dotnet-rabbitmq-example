using System.Text;

using RabbitMQ.Client;

using Serilog;

// Get RabbitMQ host name from environment variables or default to 'localhost'
var rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQ__HostName") ?? "localhost";
var seqUrl = Environment.GetEnvironmentVariable("Seq__Url") ?? "localhost";

// Configure Serilog to send logs to Seq
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(seqUrl)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Context", "Producer")
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

    Log.Information("Producer is running.");

    while (true)
    {
        // Message to send
        var message = $"Hello RabbitMQ! Sent at {DateTime.Now}";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "",
            routingKey: "example-rabbitmq-queue-name",
            basicProperties: null,
            body: body
        );
        Log.Information("Sent message: {Message}", message);

        Thread.Sleep(1000);
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Exception occurred");
}
finally
{
    Log.CloseAndFlush();
}