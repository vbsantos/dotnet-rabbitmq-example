# Example.RabbitMq

This solution demonstrates a simple microservices architecture using RabbitMQ for message-based communication. It consists of two services: a Producer and a Consumer.

## Projects

### Example.RabbitMq.Producer
   - Purpose: Sends messages to a RabbitMQ queue.
   - Key Features:
     - Reads RabbitMQ host configuration from environment variables.
     - Configures logging using Serilog.
     - Establishes a connection to RabbitMQ.
     - Declares a queue named example-rabbitmq-queue-name.
     - Continuously sends messages to the queue at regular intervals (every second).

### Example.RabbitMq.Consumer
   - Purpose: Receives and processes messages from the RabbitMQ queue.
   - Key Features:
     - Reads RabbitMQ host configuration from environment variables.
     - Configures logging using Serilog.
     - Establishes a connection to RabbitMQ.
     - Declares the same queue named example-rabbitmq-queue-name.
     - Listens for messages on the queue and logs the received messages.

## Running the Solution with Docker Compose

Instead of running the services individually, you can use Docker Compose to manage the containers. Docker Compose allows you to define and run multi-container applications with a single command.

```sh
docker-compose up
```
