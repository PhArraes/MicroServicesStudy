# MicroServicesStudy
Micro services study with .NET Core API + RabbitMQ + Couchbase + express + Angular


# Setup
docker pull rabbitmq

docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management 

docker pull couchbase

docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase


create bucket and user http://localhost:8091