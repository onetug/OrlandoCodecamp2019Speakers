# Introduction to Event Sourcing and CQRS
Presented at Orlando Code Camp March 30th, 2019.

## Description:
Netflix does it. Banks do it. Perhaps you've heard about Event Sourcing and CQRS but the concepts just don't click or you haven't seen a simple enough example so that it does. Learn about the basics of Event Sourcing as we model a Bank Account. This presentation will focus mainly on tactical patterns and simple implementations. Live coding will be done using .NET Core.


# Bank CQRS and Event Sourcing Example
This example project is used to demonstrate some of the basic concepts of:

  - CQS - Command Query Seperation
  - CQRS - Command Query Responsibility Segregation
  - Event Sourcing
  - Domain Driven Design Aggregates

# Resources

## CQS - Command Query Seperation
  - https://en.wikipedia.org/wiki/Command%E2%80%93query_separation
  - https://github.com/jbogard/MediatR/wiki

## CQRS - Command Query Responsibility Segregation
  - https://github.com/gregoryyoung/m-r
  - https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/cqrs-microservice-reads
  - http://sensetecnic.com/wp-content/uploads/2017/09/EventSourcing-for-IoT.png

## Event Sourcing
  - https://github.com/SQLStreamStore/SQLStreamStore
  - https://eventstore.org/
  - https://github.com/gautema/CQRSlite
    - Great resource for a good implementation of an aggregate and an unordered event store 
  
## Domain Driven Design Aggregates
  - https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model

# Scripts and commands
This project uses MS SQL 2017 for linux running in Docker.  You can use an version of sql server that you like by changing the configuration in appsettings.json

## Startup
  - start-sql.sh will start a fresh new instance of sql server if one isn't already
  - reset-sql.sh will try to kill and remove an existing running instance and then respawn a fresh container
  - migrate-only.sh will create the clean database migration for running the project
  - dotnet run after that