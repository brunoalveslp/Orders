# Orders API

API REST para gerenciamento de pedidos, construÃ­da com **.NET 10**, seguindo os princÃ­pios de **Clean Architecture** e **DDD (Domain-Driven Design)**.

> ðŸ“š Links e documentaÃ§Ã£o de referÃªncia: [RECURSOS.md](./RECURSOS.md)

---

## ðŸ“ Estrutura do Projeto

```
Orders/
â”œâ”€â”€ Orders.API/                  # Camada de apresentaÃ§Ã£o (Controllers, Program.cs, Dockerfile)
â”œâ”€â”€ Orders.Application/          # Camada de aplicaÃ§Ã£o (Services, DTOs, Interfaces, Mappings)
â”œâ”€â”€ Orders.Domain/               # Camada de domÃ­nio (Entities, Enums, Common)
â”œâ”€â”€ Orders.Infrastructure/       # Camada de infraestrutura (EF Core, RepositÃ³rios, Migrations)
â”œâ”€â”€ Orders.Worker/               # Worker Service (consumer RabbitMQ)
â””â”€â”€ docker-compose.yml
```

### Responsabilidade de cada camada

| Camada | Responsabilidade |
|---|---|
| `Orders.Domain` | Entidades, regras de negÃ³cio, enums e objetos de valor |
| `Orders.Application` | OrquestraÃ§Ã£o, DTOs, interfaces e mapeamentos |
| `Orders.Infrastructure` | PersistÃªncia com EF Core, repositÃ³rios e migrations |
| `Orders.API` | Controllers, configuraÃ§Ã£o do pipeline HTTP e documentaÃ§Ã£o |

---

## ðŸ›ï¸ PadrÃµes Arquiteturais

| PadrÃ£o | Onde | DescriÃ§Ã£o |
|---|---|---|
| **Repository Pattern** | `Orders.Infrastructure` | Abstrai o acesso ao banco de dados â€” `IOrderRepository` / `OrderRepository` |
| **Service Layer** | `Orders.Application` | Orquestra regras de negÃ³cio sem acoplamento Ã  infraestrutura |
| **Result Pattern** | `Orders.Domain/Common` | `Result<T>` encapsula sucesso/falha sem lanÃ§ar exceÃ§Ãµes |
| **Null Object** | `Orders.Worker` | `NullMessageBus` satisfaz `IMessageBus` no Worker sem publicar mensagens |
| **Thin Event** | `Orders.Application/Events` | `OrderCreatedEvent(Guid Id)` â€” evento mÃ­nimo, consumer busca os dados no banco |
| **Extension Method Mapping** | `Orders.Application/Mappings` | `Order.ToResponse()` converte entidade em DTO sem dependÃªncia de library externa |

---

## ðŸš€ Como executar

### PrÃ©-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Subir com Docker Compose

```bash
docker compose up --build
```

A API estarÃ¡ disponÃ­vel em:

| URL | DescriÃ§Ã£o |
|---|---|
| `http://localhost:8080` | Redireciona para a documentaÃ§Ã£o |
| `http://localhost:8080/scalar/v1` | DocumentaÃ§Ã£o interativa (Scalar UI) |
| `https://localhost:8081/scalar/v1` | DocumentaÃ§Ã£o interativa via HTTPS |
| `http://localhost:8080/order` | Endpoints da API |
| `http://localhost:15672` | RabbitMQ Management UI (guest / guest) |

> As migrations do banco de dados sÃ£o aplicadas automaticamente no startup da API.

---

## ðŸ“‹ Endpoints

| MÃ©todo | Rota | DescriÃ§Ã£o | Resposta |
|---|---|---|---|
| `GET` | `/order` | Lista todos os pedidos | `200 OK` Â· `404 Not Found` |
| `GET` | `/order/{id}` | Busca pedido por ID | `200 OK` Â· `404 Not Found` |
| `POST` | `/order` | Cria um novo pedido | `201 Created` Â· `400 Bad Request` |

### Exemplo de criaÃ§Ã£o de pedido

**Request:**
```json
POST /order
{
  "nomeCliente": "JoÃ£o Silva",
  "descricao": "Pedido de notebook",
  "valor": 4500.00
}
```

**Response `201 Created`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nomeCliente": "JoÃ£o Silva",
  "descricao": "Pedido de notebook",
  "valor": 4500.00,
  "status": "Pendente"
}
```

---

## ðŸ› ï¸ Tecnologias

| Tecnologia | VersÃ£o | Uso |
|---|---|---|
| .NET | 10 | Framework principal |
| ASP.NET Core | 10 | API REST |
| Entity Framework Core | 10 | ORM e migrations |
| PostgreSQL | 15 | Banco de dados |
| RabbitMQ | 3 | Mensageria (publish/subscribe) |
| RabbitMQ.Client | 7.2.1 | Cliente .NET para RabbitMQ |
| Scalar | 2.6.0 | DocumentaÃ§Ã£o interativa da API |
| Docker / Docker Compose | â€” | ContainerizaÃ§Ã£o e orquestraÃ§Ã£o |

---

## ðŸ‡ Mensageria â€” RabbitMQ

### Fluxo de mensagens

```
POST /order (API)
    â†’ salva no banco
    â†’ publica OrderCreatedEvent { Id } na fila "orders"

Worker (consumer)
    â†’ recebe { Id }
    â†’ busca Order no banco
    â†’ order.Process()  â†’  Status: Pendente â†’ Processado
    â†’ salva no banco
    â†’ BasicAck âœ…  (ou BasicNack âŒ em caso de erro)
```

### Publish (Orders.Infrastructure)

| Classe | Responsabilidade |
|---|---|
| `MessagingService` | Implementa `IMessageBus` â€” conecta e publica mensagens no RabbitMQ |
| `MessagingExtentions` | Extension method `AddMessaging` â€” registra `IMessageBus` como singleton no DI |
| `MessagingInitializer` | `BackgroundService` â€” forÃ§a a criaÃ§Ã£o da conexÃ£o no startup (fail-fast) |
| `NullMessageBus` (Worker) | ImplementaÃ§Ã£o vazia de `IMessageBus` â€” usada no Worker que nÃ£o publica |

### Consume (Orders.Worker)

| Conceito | ImplementaÃ§Ã£o |
|---|---|
| `AsyncEventingBasicConsumer` | Consumer event-driven â€” recebe mensagens sem polling |
| `autoAck: false` | ConfirmaÃ§Ã£o manual â€” garante que mensagem sÃ³ Ã© removida apÃ³s processamento |
| `BasicAck` | Confirma processamento com sucesso â€” mensagem removida da fila |
| `BasicNack requeue: false` | Descarta mensagem invÃ¡lida ou nÃ£o processÃ¡vel |
| `BasicNack requeue: true` | Devolve mensagem Ã  fila em caso de erro inesperado |
| `prefetchCount: 1` | Worker processa 1 mensagem por vez (QoS) |
| `IServiceScopeFactory` | Cria escopo DI por mensagem â€” necessÃ¡rio para `DbContext` e serviÃ§os `Scoped` |

---

## âš™ï¸ Worker Service

### Estrutura do Worker

```
Orders.Worker/
â”œâ”€â”€ Worker.cs                    # BackgroundService â€” consumer RabbitMQ
â”œâ”€â”€ Program.cs                   # ConfiguraÃ§Ã£o de DI e host
â”œâ”€â”€ Dockerfile                   # Multi-stage build para container
â”œâ”€â”€ appsettings.json             # ConfiguraÃ§Ãµes locais
â””â”€â”€ Messaging/
    â””â”€â”€ NullMessageBus.cs        # Null Object Pattern para IMessageBus
```

### Ciclo de vida do Worker

| MÃ©todo | Quando executa | O que faz |
|---|---|---|
| `StartAsync` | Uma vez ao iniciar | Conecta RabbitMQ, declara fila, configura QoS |
| `ExecuteAsync` | Continuamente | Registra consumer e aguarda mensagens indefinidamente |
| `StopAsync` | Uma vez ao encerrar | Fecha canal e conexÃ£o graciosamente |

---

## ðŸ§ª Testando com Postman

### Importando a coleÃ§Ã£o

1. Suba o ambiente com `docker compose up --build`
2. Abra o Postman â†’ **File â†’ Import**
3. Selecione o arquivo `Orders.postman_collection.json` na raiz do repositÃ³rio
4. A coleÃ§Ã£o **Orders API** aparecerÃ¡ no painel lateral

### VariÃ¡veis de coleÃ§Ã£o

| VariÃ¡vel | Valor padrÃ£o | DescriÃ§Ã£o |
|---|---|---|
| `baseUrl` | `http://localhost:8080` | URL base da API. Troque por `https://localhost:8081` para HTTPS |
| `orderId` | *(vazio)* | Preenchida automaticamente ao executar `POST /order` |

### Endpoints disponÃ­veis

| MÃ©todo | Rota | DescriÃ§Ã£o | Retorno |
|---|---|---|---|
| `GET` | `/order` | Lista todos os pedidos | `200 OK` Â· `404 Not Found` |
| `GET` | `/order/{{orderId}}` | Busca pedido pelo ID | `200 OK` Â· `404 Not Found` |
| `POST` | `/order` | Cria um novo pedido | `201 Created` Â· `400 Bad Request` |

### Fluxo de teste recomendado

```
1. POST /order             â†’ cria o pedido (orderId salvo automaticamente)
2. GET  /order/{{orderId}} â†’ confirma o pedido com status "Pendente"
3. GET  /order             â†’ lista todos os pedidos
4. Aguarde ~2s             â†’ Worker processa via RabbitMQ
5. GET  /order/{{orderId}} â†’ status atualizado para "Processado"
```

### Testes automatizados no POST /order

O request `POST /order` inclui os seguintes testes Postman integrados:

| Teste | ValidaÃ§Ã£o |
|---|---|
| `Status 201 Created` | CÃ³digo de resposta Ã© 201 |
| `Resposta contÃ©m id` | Campo `id` presente e nÃ£o vazio |
| `Status do pedido Ã© Pendente` | Campo `status` igual a `"Pendente"` |
| `Valor retornado corresponde ao enviado` | Campo `valor` igual ao enviado no body |