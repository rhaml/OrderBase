
# Order Generator & Order Accumulator

Sistema distribuído para geração, validação, roteamento e controle de exposição financeira de ordens de negociação utilizando protocolo FIX 4.4.

## 📋 Sobre o Projeto

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
Exposição = (Preço × Quantidade Comprada)-(Preço × Quantidade Vendida)
```

Cada ativo possui um limite máximo de 100000000.

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
GET api/fix/status
```

Resposta:

```json
{
  "connected": true
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

- Docker
- Docker Compose

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

---

## Executando Localmente

### Pré-requisitos

- .NET SDK 8
- Node.js 22+
- PostgreSQL

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
  "clOrdId": "50837041-a9d7-4855-9ed2-e7085c55d623",
  "accepted": true,
  "message": null
}
```
Rejeitado:
```json
{
  "clOrdId": "48016b7e-4ee3-4269-8ff7-d76ca052b3a8",
  "accepted": false,
  "message": "Exposure limit exceeded"
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

conforme solicitado

---


> This is a challenge by Coodesh: https://coodesh.com/

---

## Autor

Desenvolvido por Andre Lucio Stockler Rodrigues Portes de Moura como solução para o desafio técnico utilizando C#, .NET 8, React(como front, de escolha do candidato) e protocolo FIX 4.4.

# Project Empty Template

Este é um repositório de exemplo para você começar a desenvolver a questão, leia com atenção os requisitos do enunciado da questão na plataforma e seguia as boas práticas sobre como utilizar este repositório.


## Readme do Repositório

- Deve conter o título do projeto
- Uma descrição sobre o projeto em frase
- Deve conter uma lista com linguagem, framework e/ou tecnologias usadas
- Como instalar e usar o projeto (instruções)
- Não esqueça o [.gitignore](https://www.toptal.com/developers/gitignore)
- Se está usando github pessoal, referencie que é um challenge by coodesh:  

>  This is a challenge by [Coodesh](https://coodesh.com/)

## Finalização e Instruções para a Apresentação

1. Adicione o link do repositório com a sua solução na questão na plataforma
2. Verifique se o Readme está bom e faça o commit final em seu repositório;
3. Envie e aguarde as instruções para seguir. Caso o teste tenha apresentação de vídeo, dentro da tela de entrega será possível gravar após adicionar o link do repositório. Sucesso e boa sorte. =)


## Suporte

Para tirar dúvidas sobre o processo envie uma mensagem diretamente a um especialista no chat da plataforma. 
