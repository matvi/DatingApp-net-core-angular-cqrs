"# DatingApp-net-core-angular-cqrs" 

In the latest changes 28/Jul/2021 It was added MassTransit in order to start decopling the application into microservices.

To run the application it is necesary to have RabbitMQ running.
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
