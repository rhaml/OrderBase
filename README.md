
# Order Generator & Order Accumulator

Sistema distribuído para geração, validação, roteamento e controle de exposição financeira de ordens de negociação utilizando protocolo FIX 4.4.

## Sobre o Projeto

Este projeto implementa uma solução composta por dois serviços independentes que se comunicam através do protocolo FIX 4.4 utilizando QuickFIX/N, simulando um fluxo simplificado de negociação de ativos financeiros.

A solução é responsável por:

- Receber ordens de compra e venda através de uma interface web.
- Validar regras de negócio e integridade dos dados.
- Enviar ordens através do protocolo FIX 4.4.
- Processar e acumular exposição financeira por ativo.
- Aplicar limites de exposição configuráveis.
- Garantir consistência em cenários concorrentes.
- Disponibilizar observabilidade através de logs e métricas.

---

## Arquitetura

A solução é composta por:

### OrderGenerator

Aplicação responsável por:

No Frontend:
- Disponibilizar interface web em React.

No Backend:
- Expor API REST para criação de ordens.
- Validar requisições utilizando FluentValidation.
- Enviar mensagens FIX para o OrderAccumulator.
- Receber respostas de aceitação ou rejeição.


### OrderAccumulator

Aplicação responsável por:

- Receber mensagens FIX.
- Calcular exposição financeira por símbolo.
- Validar limites configurados.
- Persistir exposição acumulada.
- Responder ao OrderGenerator.

---

## Tecnologias Utilizadas

### Backend

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- QuickFIX/N (FIX 4.4)
- FluentValidation
- Serilog
- Polly
- OpenTelemetry
- Swagger

### Frontend

- React
- Vite
- Axios

### Testes

- xUnit
- FluentAssertions

### Infraestrutura

- Docker
- Docker Compose

---

## Estrutura do Projeto

```text
OrderBase/

├── OrderGenerator
│   ├── OrderGenerator.Api
│   ├── OrderGenerator.Application
│   ├── OrderGenerator.Domain
│   ├── OrderGenerator.Infrastructure
│   └── Frontend

├── OrderAccumulator
│   ├── OrderAccumulator.Api
│   ├── OrderAccumulator.Application
│   ├── OrderAccumulator.Domain
│   └── OrderAccumulator.Infrastructure

tests/

├── OrderGenerator.UnitTests
└── OrderAccumulator.Domain.Tests
```

---

## Regras de Negócio

### Símbolos

- PETR4
- VALE3
- VIIA4

### Lados

- Venda
- Compra

### Quantidade

- Maior que 0
- Menor que 100.000

### Preço

- Maior que 0
- Menor que 1.000
- Múltiplo de 0,01

### Exposição Financeira

A exposição é calculada da seguinte forma:

```text
Exposição = Σ(Preço × Quantidade Comprada)-Σ(Preço × Quantidade Vendida)
```

Cada ativo possui um limite máximo de 100.000.000.

Quando o limite é excedido:

- Ordem rejeitada
- Exposição permanece inalterada
- Retorno para o front contendo a mensagem de limite excedido.

## Observabilidade

A solução possui:

### Logs

- Serilog
- CorrelationId por requisição

### Métricas

OpenTelemetry

### Health Check

```http
GET api/Fix/status
```

Resposta:

```json
{
  "status": "Connected"
}
```

---

## Fluxo da Aplicação

```text
React Frontend<---------------+
       |                      |
       v                      |
OrderGenerator API            |
       |                      |
       v                      |
QuickFIX/N                    |
       |                      |
       v                      |
OrderAccumulator API          |
       |                      |
       v                      |
Validação de Exposição        |
       |                      |
       +----> Rejeitada --->Retorno
       |                      ^
       +----> Aceita          |
                 |            |
                 v            |
         Atualização da Exposição
```

---

## Executando com Docker

### Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker-Compose](https://docs.docker.com/compose/install/)

### Clonar o projeto

```bash
git clone https://github.com/rhaml/OrderBase.git

cd OrderBase
```

### Subir containers

```bash
docker-compose up -d
```

### Remover containers
```bash
docker compose down -v
```

### Verificar containers

```bash
docker ps
```
### Verificar host:port

- Se necessário, verificar a porta e o host do OrderGenerator no docker
- Em caso de erro de comunicação do frontend com o backend, ajustar:

```text
arquivo OrderBase\OrderGenerator\Frontend\.env.production

VITE_API_URL=http://127.0.0.1:5000 ou VITE_API_URL=http://{Seu_docker_host}:{Sua_docker_port}
```
---

## Executando Localmente

### Pré-requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Node.js](https://nodejs.org/pt-br)
- [PostgreSQL](https://www.postgresql.org/download/)

### Banco de Dados

Atualizar connection string:

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=order;Username=postgres;Password=order"
  }
}
```

Executar migrations:

```bash
dotnet ef database update
```

---

### Backend

```bash
cd OrderGenerator/OrderGenerator.Api

dotnet run
```

```bash
cd OrderAccumulator/OrderAccumulator.Api

dotnet run
```

---

### Frontend

```bash
cd OrderGenerator/Frontend

npm install

npm run dev
```

Aplicação disponível em:

```text
http://localhost:5173
```

---

## Executando Testes

### Unitários

```bash
dotnet test
```

---

## API

### Criar Ordem

```http
POST api/orders
```

Payload da requisição

```json
{
  "symbol": "PETR4",
  "side": "BUY",
  "quantity": 100,
  "price": 25.50
}
```

Resposta

Aceito:
```json
{
    "clOrdId": "8b0fab85-a7aa-412d-a829-d82bcd6b6a7a",
    "symbol": "PETR4",
    "exposure": 5.00,
    "accepted": true,
    "message": null
}
```
Rejeitado:
```json
{
    "clOrdId": "0ca18afb-2e83-4d2e-9403-6c66d7dcf49d",
    "symbol": "PETR4",
    "exposure": 99899006.00,
    "accepted": false,
    "message": "Exposure limit exceeded for symbol PETR4. Current exposure: 99899006,00, New exposure: 199798007,00"
}
```


---

## .gitignore

Gerado a partir do template recomendado para:

- .NET
- Visual Studio
- Rider
- VS Code
- Node.js
- React
- Docker

Usado a Referência:

https://www.toptal.com/developers/gitignore

---

## Autor

Desenvolvido por Andre Lucio Stockler Rodrigues Portes de Moura como solução para o desafio técnico utilizando C#, .NET 8, React e protocolo FIX 4.4.

---

>  This is a challenge by [Coodesh](https://coodesh.com/)