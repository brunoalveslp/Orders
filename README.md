# Estrutura

## API - Orders.API
- Controllers
  - OrderController
- Program.cs

↓

## Infrastructure - Orders.Infrastructure
- Messaging
  - RabbitMqService : IMessageBus
  - RabbitMqExtensions

- Persistence
  - AppDbContext
  - Configurations
    - OrderConfig
  - Repositories
    - OrderRepository : IOrderRepository

- DependencyInjection.cs

## Worker - Orders.Worker
- Workers
  - OrderWorker : BackgroundService
- Consumers
  - OrderConsumer
- Program.cs

↓

## Application - Orders.Application
- Services
  - OrderService
- DTOs
  - CreateOrderRequest
- Interfaces
  - IOrderRepository
  - IMessageBus

↓

## Domain - Orders.Domain
- Entities
  - Order
- Enums
  - OrderStatus
- Common
  - Result