# 📈 Yield Tracker

**Yield Tracker** é uma API REST desenvolvida em .NET 8 para calcular o rendimento de investimentos pós-fixados com base em índices financeiros e cotações diárias armazenadas no banco de dados.

---

## 🚀 Funcionalidades

- Calcular o saldo atualizado de um investimento pós-fixado baseado em data inicial e final.
- Armazenar e consultar cotações de índices (como CDI, IPCA, etc.).
- Suporte a validação de dados e tratamento de erros via middleware.
- Cobertura de testes unitários e de integração.
- Rodando com PostgreSQL via Docker Compose.

---

## 🛠️ Tecnologias

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker](https://www.docker.com/)
- [xUnit](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)
- [Moq](https://github.com/moq/moq4)
- [ErrorOr](https://github.com/amantinband/error-or)

---

## 📂 Estrutura do Projeto

```
Yield.Tracker
├── src/
│   ├── Yield.Tracker.Api               # Camada de apresentação (Controllers, Middlewares)
│   ├── Yield.Tracker.Domain            # Entidades, DTOs, Interfaces
│   ├── Yield.Tracker.Application       # Serviços de aplicação
│   ├── Yield.Tracker.Infra.Data        # Repositórios e Contexto EF
│   └── Yield.Tracker.Infra.IoC         # Injeção de dependência e configuração
├── tests/
│   ├── Yield.Tracker.Unit.Test         # Testes unitários
│   └── Yield.Tracker.Integration.Test  # Testes de integração
└── docker-compose.yml
```

---

## 🐳 Como rodar o projeto com Docker

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### 🚦 Passos para rodar localmente

1. Clone o repositório:
   ```bash
   git clone https://github.com/FilhoReginaldo/yield_tracker
   cd yield-tracker
   ```

2. Suba os containers com Docker Compose:
   ```bash
   docker-compose up --build
   ```

3. Acesse a API no navegador ou Postman:
   ```
   http://localhost:5000/swagger
   ```

---

## 🧪 Executando os Testes

### Testes Unitários e de Integração

No diretório raiz do projeto, execute:

```bash
dotnet test
```

---

## 🔧 Configuração de Banco de Dados

A connection string usada é:

```
Host=postgres;Database=investimentos;Username=sqia;Password=sqia123
```

Ela é definida no `docker-compose.yml` como variável de ambiente e automaticamente consumida pela aplicação via `IConfiguration`.

---

## 🧠 Exemplos de Uso

### Endpoint de Cálculo

- **POST** `/v1/investment/calculate`

**Corpo da Requisição:**
```json
{
  "investedValue": 1000,
  "startDate": "2025-01-01",
  "endDate": "2025-01-10"
}
```

**Resposta Esperada:**
```json
{
  "updatedValue": 1012.30,
  "accumulatedFactor": 1.0123
}
```

---

## 🛡️ Tratamento de Erros

Todos os erros são padronizados via middleware (`ErrorHandlingMiddleware`) com resposta HTTP apropriada e mensagens claras.

---

## 🧳 Migração e Inicialização do Banco

Ao subir via Docker Compose, o banco é inicializado com o script `init.sql` presente na pasta `/db`. Certifique-se de que ele esteja configurado corretamente para inserir índices e cotações necessárias para teste.


---

## 👨‍💻 Autor

Feito com 💻 por Reginaldo Filho — Contribuições são bem-vindas!