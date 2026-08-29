# provamarcusMazza — Teste .NET Senior

Projeto de referência para o teste prático de gestão de pedidos.

## Decisões arquiteturais

- **.NET 10**
- **Controllers**: escolhidos para deixar explícita a fronteira HTTP e manter os endpoints fáceis de localizar durante a entrevista.
- **Clean Architecture**: `Domain`, `Application`, `Infrastructure`, `Api`.
- **CQRS + MediatR**: Commands e Queries separados por caso de uso.
- **FluentValidation + MediatR Pipeline Behavior**.
- **EF Core + SQLite**.
- **Migrations aplicadas automaticamente na inicialização**.
- **JWT**.
- **xUnit** para testes unitários dos handlers.
- **Docker / docker-compose**.
- **Serilog** no pipeline do MediatR para request/response e tempo de execução.
- **WebApplicationFactory** com teste de integração do login.

## Extensões além do enunciado

Além do escopo original do teste, foram adicionados endpoints de cadastro/listagem
para `User` e `Customer` (`/api/users` e `/api/customers`), seguindo o mesmo padrão
CQRS + MediatR + FluentValidation já usado em `Order`. Isso não substitui o usuário
fixo exigido pelo teste — só facilita criar dados de teste sem mexer em seed.

## User e Customer

Foram adicionadas duas entidades simples:

- `User`: identidade autenticada que recebe o JWT.
- `Customer`: cliente comercial dono do pedido.

Elas permanecem separadas de propósito: o usuário autenticado não é necessariamente o Customer do Order.

O usuário exigido pelo teste é persistido automaticamente na inicialização:

- Email: `dev@martech.com`
- Senha: `Senha@123`

Novos clientes e usuários podem ser cadastrados via API (`POST /api/customers` e
`POST /api/users`) — veja o passo a passo em [Como testar](#como-testar).

## Estrutura

```text
src/
├── provamarcusMazza.Domain
├── provamarcusMazza.Application
├── provamarcusMazza.Infrastructure
└── provamarcusMazza.Api

tests/
├── provamarcusMazza.UnitTests
└── provamarcusMazza.IntegrationTests
```

## Fluxo CQRS

```text
HTTP
  ↓
Controller
  ↓
MediatR
  ↓
LoggingBehavior
  ↓
ValidationBehavior
  ↓
Command/Query Handler
  ↓
Domain
  ↓
Repository / EF Core / SQLite
```

## Regras de negócio

As regras centrais ficam no Domain:

- pedido precisa ter pelo menos 1 item;
- `Quantity > 0`;
- `UnitPrice > 0`;
- apenas `Pending` pode ser cancelado;
- `TotalAmount` é calculado pela entidade `Order`.

## Rodar local

Requisitos:

- .NET SDK 10

```bash
dotnet restore
dotnet test
dotnet run --project src/provamarcusMazza.Api
```

O SQLite `orders.db` é criado na inicialização e a migration é aplicada automaticamente.

Swagger:

```text
http://localhost:<porta>/swagger
```

## Rodar via Docker

```bash
docker compose up --build
```

API:

```text
http://localhost:8080
```

Swagger:

```text
http://localhost:8080/swagger
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| POST | `/auth/login` | Retorna JWT. Usuário fixo: `dev@martech.com` / `Senha@123` |
| POST | `/api/orders` | Cria um novo pedido (requer auth) |
| GET | `/api/orders?page=1&pageSize=10` | Lista pedidos com paginação (requer auth) |
| GET | `/api/orders/{id}` | Retorna pedido por ID (requer auth) |
| PATCH | `/api/orders/{id}/cancel` | Cancela um pedido (requer auth) |
| POST | `/api/customers` | Cadastra um cliente (requer auth) |
| GET | `/api/customers?page=1&pageSize=10` | Lista clientes com paginação (requer auth) |
| POST | `/api/users` | Cadastra um usuário (requer auth) |
| GET | `/api/users?page=1&pageSize=10` | Lista usuários com paginação (requer auth) |

## Como testar

Passo a passo para validar a aplicação do zero, via Swagger (`/swagger`) ou qualquer
cliente HTTP (Postman, Insomnia, `curl`, o arquivo `provamarcusMazza.Api.http`).

1. **Suba a aplicação** (local com `dotnet run` ou via `docker compose up --build`).
   A migration roda sozinha e o usuário fixo já fica disponível.

2. **Faça login** para obter o token:

   ```http
   POST /auth/login
   Content-Type: application/json

   {
     "email": "dev@martech.com",
     "password": "Senha@123"
   }
   ```

   Copie o `accessToken` da resposta. No Swagger, clique em **Authorize** e informe
   `Bearer <token>`; em outro cliente, envie o header `Authorization: Bearer <token>`
   em toda chamada abaixo.

3. **Cadastre um cliente** (necessário para criar pedidos):

   ```http
   POST /api/customers
   Authorization: Bearer <token>
   Content-Type: application/json

   {
     "name": "Cliente Teste",
     "email": "cliente.teste@exemplo.com"
   }
   ```

   Guarde o `id` retornado.

4. **Crie um pedido** para esse cliente:

   ```http
   POST /api/orders
   Authorization: Bearer <token>
   Content-Type: application/json

   {
     "customerId": "<id do cliente>",
     "items": [
       {
         "productName": "Notebook",
         "quantity": 1,
         "unitPrice": 4500.00
       }
     ]
   }
   ```

   Confira que `totalAmount` veio calculado (`quantity * unitPrice`).

5. **Liste os pedidos** e **busque por ID**:

   ```http
   GET /api/orders?page=1&pageSize=10
   GET /api/orders/{id}
   ```

6. **Cancele o pedido** e confirme o status:

   ```http
   PATCH /api/orders/{id}/cancel
   ```

   Tente cancelar de novo — deve retornar erro (`422`), pois só pedido `Pending`
   pode ser cancelado.

7. **Valide as regras de negócio** tentando cenários inválidos:
   - criar pedido sem itens → `400`;
   - `quantity` ou `unitPrice` menor ou igual a zero → `400`;
   - `customerId` inexistente → `404`.

8. **(Opcional) Cadastre e liste usuários**:

   ```http
   POST /api/users
   Authorization: Bearer <token>
   Content-Type: application/json

   {
     "email": "novo.usuario@exemplo.com",
     "password": "Senha@123"
   }

   GET /api/users?page=1&pageSize=10
   ```

9. **Rode os testes automatizados**:

   ```bash
   dotnet test
   ```

   Isso cobre os handlers (`CreateOrderHandler`, `CancelOrderHandler`), as regras de
   domínio (`OrderTests`) e o fluxo de login (`AuthEndpointTests`, integração com
   `WebApplicationFactory`).

## Observação

A solução foi mantida deliberadamente pequena. Não foi criado `IRepository<T>` genérico. Cada abstração tem responsabilidade concreta e pode ser explicada durante a entrevista.
