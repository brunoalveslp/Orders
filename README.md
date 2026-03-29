# Orders API

API REST para gerenciamento de pedidos, construída com **.NET 10**, seguindo os princípios de **Clean Architecture** e **DDD (Domain-Driven Design)**.

> 📚 Links e documentação de referência: [RECURSOS.md](./RECURSOS.md) · [ARQUITETURA.md](./ARQUITETURA.md)

---

## 📁 Estrutura do Projeto

```
Orders/
├── Orders.API/                  # Camada de apresentação (Controllers, Program.cs, Dockerfile)
├── Orders.Application/          # Camada de aplicação (Services, DTOs, Interfaces, Mappings)
├── Orders.Domain/               # Camada de domínio (Entities, Enums, Common)
├── Orders.Infrastructure/       # Camada de infraestrutura (EF Core, Repositórios, Migrations)
├── Orders.Worker/               # Worker Service (consumer RabbitMQ)
└── docker-compose.yml
```

### Responsabilidade de cada camada

| Camada | Responsabilidade |
|---|---|
| `Orders.Domain` | Entidades, regras de negócio, enums e objetos de valor |
| `Orders.Application` | Orquestração, DTOs, interfaces e mapeamentos |
| `Orders.Infrastructure` | Persistência com EF Core, repositórios e migrations |
| `Orders.API` | Controllers, configuração do pipeline HTTP e documentação |

---

## 🏛️ Padrões Arquiteturais

| Padrão | Onde | Descrição |
|---|---|---|
| **Repository Pattern** | `Orders.Infrastructure` | Abstrai o acesso ao banco de dados — `IOrderRepository` / `OrderRepository` |
| **Service Layer** | `Orders.Application` | Orquestra regras de negócio sem acoplamento à infraestrutura |
| **Result Pattern** | `Orders.Domain/Common` | `Result<T>` encapsula sucesso/falha sem lançar exceções |
| **Null Object** | `Orders.Worker` | `NullMessageBus` satisfaz `IMessageBus` no Worker sem publicar mensagens |
| **Thin Event** | `Orders.Application/Events` | `OrderCreatedEvent(Guid Id)` — evento mínimo, consumer busca os dados no banco |
| **Extension Method Mapping** | `Orders.Application/Mappings` | `Order.ToResponse()` converte entidade em DTO sem dependência de library externa |

---

## 🚀 Como executar

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Subir com Docker Compose

```bash
docker compose up --build
```

A API estará disponível em:

| URL | Descrição |
|---|---|
| `http://localhost:8080` | Redireciona para a documentação |
| `http://localhost:8080/scalar/v1` | Documentação interativa (Scalar UI) |
| `https://localhost:8081/scalar/v1` | Documentação interativa via HTTPS |
| `http://localhost:8080/order` | Endpoints da API |
| `http://localhost:15672` | RabbitMQ Management UI (guest / guest) |

> As migrations do banco de dados são aplicadas automaticamente no startup da API.

---

## 📋 Endpoints

| Método | Rota | Descrição | Resposta |
|---|---|---|---|
| `GET` | `/order` | Lista todos os pedidos | `200 OK` · `404 Not Found` |
| `GET` | `/order/{id}` | Busca pedido por ID | `200 OK` · `404 Not Found` |
| `POST` | `/order` | Cria um novo pedido | `201 Created` · `400 Bad Request` |

### Exemplo de criação de pedido

**Request:**
```json
POST /order
{
  "nomeCliente": "João Silva",
  "descricao": "Pedido de notebook",
  "valor": 4500.00
}
```

**Response `201 Created`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nomeCliente": "João Silva",
  "descricao": "Pedido de notebook",
  "valor": 4500.00,
  "status": "Pendente"
}
```

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10 | Framework principal |
| ASP.NET Core | 10 | API REST |
| Entity Framework Core | 10 | ORM e migrations |
| PostgreSQL | 15 | Banco de dados |
| RabbitMQ | 3 | Mensageria (publish/subscribe) |
| RabbitMQ.Client | 7.2.1 | Cliente .NET para RabbitMQ |
| Scalar | 2.6.0 | Documentação interativa da API |
| Docker / Docker Compose | — | Containerização e orquestração |

---

## 🐇 Mensageria — RabbitMQ

### Fluxo de mensagens

```
POST /order (API)
    → salva no banco
    → publica OrderCreatedEvent { Id } na fila "orders"

Worker (consumer)
    → recebe { Id }
    → busca Order no banco
    → OrderService.ProcessAsync()  →  Status: Pendente → Processado e salva no banc
    → BasicAck ✅  (ou BasicNack ❌ em caso de erro)
```

### Publish (Orders.Infrastructure)

| Classe | Responsabilidade |
|---|---|
| `MessagingService` | Implementa `IMessageBus` — conecta e publica mensagens no RabbitMQ |
| `MessagingExtentions` | Extension method `AddMessaging` — registra `IMessageBus` como singleton no DI |
| `MessagingInitializer` | `BackgroundService` — força a criação da conexão no startup (fail-fast) |
| `NullMessageBus` (Worker) | Implementação vazia de `IMessageBus` — usada no Worker que não publica |

### Consume (Orders.Worker)

| Conceito | Implementação |
|---|---|
| `AsyncEventingBasicConsumer` | Consumer event-driven — recebe mensagens sem polling |
| `autoAck: false` | Confirmação manual — garante que mensagem só é removida após processamento |
| `BasicAck` | Confirma processamento com sucesso — mensagem removida da fila |
| `BasicNack requeue: false` | Descarta mensagem inválida ou não processável |
| `BasicNack requeue: true` | Devolve mensagem à fila em caso de erro inesperado |
| `prefetchCount: 1` | Worker processa 1 mensagem por vez (QoS) |
| `IServiceScopeFactory` | Cria escopo DI por mensagem — necessário para `DbContext` e serviços `Scoped` |

---

## ⚙️ Worker Service

### Estrutura do Worker

```
Orders.Worker/
├── Worker.cs                    # BackgroundService — consumer RabbitMQ
├── Program.cs                   # Configuração de DI e host
├── Dockerfile                   # Multi-stage build para container
├── appsettings.json             # Configurações locais
└── Messaging/
    └── NullMessageBus.cs        # Null Object Pattern para IMessageBus
```

### Ciclo de vida do Worker

| Método | Quando executa | O que faz |
|---|---|---|
| `StartAsync` | Uma vez ao iniciar | Conecta RabbitMQ, declara fila, configura QoS |
| `ExecuteAsync` | Continuamente | Registra consumer e aguarda mensagens indefinidamente |
| `StopAsync` | Uma vez ao encerrar | Fecha canal e conexão graciosamente |

---

## 🧪 Testando com Postman

### Importando a coleção

1. Suba o ambiente com `docker compose up --build`
2. Abra o Postman → **File → Import**
3. Selecione o arquivo `Orders.postman_collection.json` na raiz do repositório
4. A coleção **Orders API** aparecerá no painel lateral

### Variáveis de coleção

| Variável | Valor padrão | Descrição |
|---|---|---|
| `baseUrl` | `http://localhost:8080` | URL base da API. Troque por `https://localhost:8081` para HTTPS |
| `orderId` | *(vazio)* | Preenchida automaticamente ao executar `POST /order` |

### Endpoints disponíveis

| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| `GET` | `/order` | Lista todos os pedidos | `200 OK` · `404 Not Found` |
| `GET` | `/order/{{orderId}}` | Busca pedido pelo ID | `200 OK` · `404 Not Found` |
| `POST` | `/order` | Cria um novo pedido | `201 Created` · `400 Bad Request` |

### Fluxo de teste recomendado

```
1. POST /order             → cria o pedido (orderId salvo automaticamente)
2. GET  /order/{{orderId}} → confirma o pedido com status "Pendente"
3. GET  /order             → lista todos os pedidos
4. Aguarde ~2s             → Worker processa via RabbitMQ
5. GET  /order/{{orderId}} → status atualizado para "Processado"
```

### Testes automatizados no POST /order

O request `POST /order` inclui os seguintes testes Postman integrados:

| Teste | Validação |
|---|---|
| `Status 201 Created` | Código de resposta é 201 |
| `Resposta contém id` | Campo `id` presente e não vazio |
| `Status do pedido é Pendente` | Campo `status` igual a `"Pendente"` |
| `Valor retornado corresponde ao enviado` | Campo `valor` igual ao enviado no body |
