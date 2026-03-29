# 📚 Recursos e Documentação de Referência

Links de referência utilizados no desenvolvimento da **Orders API**.

---

## 🐳 Dockerfile

| Recurso | Link |
|---|---|
| Referência de instruções (`FROM`, `COPY`, `RUN`, etc.) | https://docs.docker.com/reference/dockerfile/ |
| Boas práticas de Dockerfile | https://docs.docker.com/build/building/best-practices/ |
| Multi-stage builds | https://docs.docker.com/build/building/multi-stage/ |
| .NET com Docker — guia Microsoft | https://learn.microsoft.com/en-us/dotnet/core/docker/build-container |
| Imagens oficiais .NET no Docker Hub | https://hub.docker.com/_/microsoft-dotnet |
| Containerizar app ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images |

---

## 🐙 Docker Compose

| Recurso | Link |
|---|---|
| Referência completa do `docker-compose.yml` | https://docs.docker.com/compose/compose-file/ |
| Comandos do CLI (`up`, `down`, `build`, `logs`) | https://docs.docker.com/compose/reference/ |
| `depends_on` e ordem de inicialização | https://docs.docker.com/compose/how-tos/startup-order/ |
| Variáveis de ambiente no Compose | https://docs.docker.com/compose/how-tos/environment-variables/ |
| Health checks no Compose | https://docs.docker.com/compose/compose-file/05-services/#healthcheck |
| Docker Compose com .NET e banco de dados | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/multi-container-applications-docker-compose |

---

## 🌐 .NET e ASP.NET Core

| Recurso | Link |
|---|---|
| ASP.NET Core — visão geral | https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core |
| Roteamento em ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing |
| Injeção de Dependência no .NET | https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection |
| `IActionResult` e helpers (`Ok`, `Created`, `NotFound`) | https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types |
| `JsonStringEnumConverter` — serialização de enums | https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties |
| C# Records | https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record |
| Extension Methods — C# Guide | https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods |

---

## 🔤 System.Text.Json

| Recurso | Link |
|---|---|
| Visão geral — System.Text.Json | https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview |
| Como serializar e desserializar JSON | https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to |
| Customizar nomes e valores de propriedades | https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties |
| Configurar globalmente (`JsonSerializerOptions`) | https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/configure-options |

---

## 📦 Entity Framework Core e Migrations

| Recurso | Link |
|---|---|
| EF Core — visão geral | https://learn.microsoft.com/en-us/ef/core/ |
| Code First com EF Core | https://learn.microsoft.com/en-us/ef/core/modeling/ |
| Fluent API (`IEntityTypeConfiguration`) | https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many |
| Migrations — introdução | https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/ |
| Aplicar migrations em runtime (`Database.Migrate`) | https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying |
| `ApplyConfigurationsFromAssembly` | https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.modelbuilder.applyconfigurationsfromassembly |
| Pacote NuGet `Npgsql.EntityFrameworkCore.PostgreSQL` | https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL |

---

## 🐘 PostgreSQL e Npgsql

| Recurso | Link |
|---|---|
| Npgsql — documentação oficial | https://www.npgsql.org/efcore/ |
| Npgsql EF Core Provider | https://www.npgsql.org/efcore/index.html |
| Imagem Docker oficial do PostgreSQL | https://hub.docker.com/_/postgres |
| Connection string do PostgreSQL | https://www.connectionstrings.com/postgresql/ |
| `pg_isready` — health check | https://www.postgresql.org/docs/current/app-pg-isready.html |

---

## 🐇 RabbitMQ .NET Client

| Recurso | Link |
|---|---|
| Guia completo da API .NET | https://www.rabbitmq.com/client-libraries/dotnet-api-guide |
| Conectando ao RabbitMQ (`ConnectionFactory`) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#connecting |
| Publicando mensagens (`BasicPublishAsync`) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#publishing |
| Consumindo mensagens (`AsyncEventingBasicConsumer`) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#consuming-using-a-subscription |
| Consumer Memory Safety (`.ToArray()` obrigatório) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#consumer-memory-safety |
| Consumers e operações no mesmo canal | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#consumers-and-operations-on-the-same-channel |
| Ack / Nack — confirmações manuais | https://www.rabbitmq.com/docs/confirms |
| Automatic Recovery (reconexão automática) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#recovery |
| QoS / prefetchCount (`BasicQosAsync`) | https://www.rabbitmq.com/docs/consumer-prefetch |
| Declaração de filas (`QueueDeclareAsync`) | https://www.rabbitmq.com/client-libraries/dotnet-api-guide#using-exchanges-and-queues |
| Painel de administração (Management UI) | https://www.rabbitmq.com/docs/management |
| Imagem Docker oficial do RabbitMQ | https://hub.docker.com/_/rabbitmq |
| `rabbitmq-diagnostics ping` — health check | https://www.rabbitmq.com/docs/rabbitmq-diagnostics.8 |
| Pacote NuGet `RabbitMQ.Client` | https://www.nuget.org/packages/RabbitMQ.Client |

---

## ⚙️ Worker Service

| Recurso | Link |
|---|---|
| Worker Services no .NET | https://learn.microsoft.com/en-us/dotnet/core/extensions/workers |
| `BackgroundService` — classe base | https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.backgroundservice |
| `IHostedService` | https://learn.microsoft.com/en-us/dotnet/core/extensions/hosted-services |
| Worker com Docker | https://learn.microsoft.com/en-us/dotnet/core/docker/build-container |
| `IServiceScopeFactory` — escopos em BackgroundService | https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service |
| `dotnet new worker` — template | https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new |
| Graceful shutdown em Workers | https://learn.microsoft.com/en-us/dotnet/core/extensions/workers#graceful-shutdown |

---

## 🎨 Scalar e OpenAPI

| Recurso | Link |
|---|---|
| Integração Scalar com .NET | https://guides.scalar.com/scalar/scalar-api-references/dotnet-integration |
| GitHub do pacote `Scalar.AspNetCore` | https://github.com/scalar/scalar/tree/main/packages/scalar.aspnetcore |
| OpenAPI nativo .NET 9/10 | https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview |
| `AddOpenApi` / `MapOpenApi` | https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi |

---

## 🏛️ Clean Architecture e DDD

| Recurso | Link |
|---|---|
| .NET Microservices — livro gratuito Microsoft | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/ |
| DDD — Domain Model Layer Validations | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-model-layer-validations |
| DDD — Aggregate Design | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-domain-model |
| Clean Architecture no .NET (Ardalis) | https://github.com/ardalis/CleanArchitecture |
| Domain Events | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation |
| Thin vs Fat Events | https://www.eventstore.com/blog/whats-the-difference-between-a-command-and-an-event |

---

## 🗄️ Repository Pattern

| Recurso | Link |
|---|---|
| Infrastructure e Repository — Microsoft | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design |
| Repository Pattern com EF Core | https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application |
| Generic Repository — prós e contras | https://www.martinfowler.com/eaaCatalog/repository.html |

---

## 🔧 Service Layer e Application Layer

| Recurso | Link |
|---|---|
| Application Layer com Commands e Handlers | https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/application-layer-commands-handlers |
| Service Layer — Martin Fowler | https://www.martinfowler.com/eaaCatalog/serviceLayer.html |
| Por que evitar AutoMapper (quando não precisa) | https://jimmybogard.com/automapper-usage-guidelines |

---

## 💡 Result Pattern

| Recurso | Link |
|---|---|
| Result Pattern — visão geral | https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern |
| Railway-Oriented Programming | https://fsharpforfunandprofit.com/rop/ |
| Evitando exceções para fluxo de controle | https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions |
| Null Object Pattern | https://refactoring.guru/introduce-null-object |

---

## 🔒 HTTPS e Certificados Dev

| Recurso | Link |
|---|---|
| HTTPS no desenvolvimento com ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl |
| `dotnet dev-certs https` — gerar e confiar | https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs |
| Certificado HTTPS no Docker com ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core/security/docker-https |
| `UseHttpsRedirection` middleware | https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl#require-https |
| Kestrel — configurar endpoints HTTPS | https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints |

---

## ❤️ Health Checks

| Recurso | Link |
|---|---|
| Health Checks no ASP.NET Core | https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks |
| `depends_on` com `condition: service_healthy` | https://docs.docker.com/compose/how-tos/startup-order/ |
| `pg_isready` — verificação PostgreSQL | https://www.postgresql.org/docs/current/app-pg-isready.html |
| `rabbitmq-diagnostics ping` — verificação RabbitMQ | https://www.rabbitmq.com/docs/rabbitmq-diagnostics.8 |
