# Desafio Técnico – Divisores (C# / .NET 8)

Sistema para **decompor um número em todos os seus divisores** e **enumerar os divisores primos**, com:
- **Console App** (I/O simples no terminal, igual ao exemplo do enunciado);
- **Web API** (serviço para integração com outros sistemas);
- **Biblioteca compartilhada** com a regra de negócio;
- **Testes unitários** com xUnit;
- **Dockerfile** e **GitHub Actions** (CI com build + testes).

> **Observação importante:** o enunciado mostra `1` dentro de “Divisores Primos”. Embora **1 não seja primo** pela definição matemática, **seguimos o enunciado** e, por padrão, listamos `1` junto com os divisores primos. Isso pode ser alterado facilmente via parâmetro.

---

## Estrutura

```
/
├─ apps/
│  ├─ ConsoleApp/          # Aplicativo de console
│  └─ DivisorsApi/         # Minimal API (.NET 8)
├─ src/
│  └─ NumberTools/         # Biblioteca com a regra de negócio
├─ tests/
│  └─ NumberTools.Tests/   # Testes xUnit
├─ .github/workflows/
│  └─ ci.yml               # Pipeline de build + test
├─ Directory.Build.props
├─ .editorconfig
├─ DivisorsChallenge.sln
└─ README.md
```

---

## Como rodar (Console)

```bash
cd apps/ConsoleApp
dotnet run -- 45
# ou sem argumento (entra no modo interativo)
dotnet run
```

Saída esperada para `45`:

```
Número de Entrada: 45
Números divisores: 1 3 5 9 15 45
Divisores Primos: 1 3 5
```

---

## Como rodar (Web API)

```bash
cd apps/DivisorsApi
dotnet run
```

A API sobe em `http://localhost:5181` (ou porta alocada). Endpoints:

- `GET /api/v1/divisors/{n}` → computa divisores e divisores primos
- `GET /health` → health-check

Exemplo:

```
GET http://localhost:5181/api/v1/divisors/45
```

Resposta:

```json
{
  "input": 45,
  "divisors": [1,3,5,9,15,45],
  "primeDivisors": [1,3,5],
  "elapsedMs": 0.06,
  "cached": false
}
```

### Docker

```bash
cd apps/DivisorsApi
docker build -t divisors-api:latest .
docker run -p 5181:8080 --rm divisors-api:latest
# Agora: http://localhost:5181/api/v1/divisors/45
```

---

## Testes

```bash
dotnet test
```

Cobrem casos: `1`, número primo, quadrado perfeito, composto comum (`12`, `45`), limites e validações.

---

## Decisões de projeto

- **Performance:** O cálculo de divisores é `O(√n)` varrendo apenas até `√n`. Para primalidade usamos um teste rápido `6k±1` também `O(√n)`; para o desafio é suficiente e evita bibliotecas externas.
- **Escalabilidade/Disponibilidade:** A API é **stateless**, suporta instâncias paralelas atrás de um load balancer. Inclui **rate limiting** e **cache em memória** (TTL curto) para aliviar recomputos de números repetidos.
- **Resiliência/Robustez:** validação de entrada, *timeouts* razoáveis, *health-check*. Código com **analisadores**, `nullable`, e **testes**.
- **Extensível:** a regra de negócio está isolada em `src/NumberTools` (pode ser publicada como pacote interno futuramente).

---

## Parâmetros de design

- Por padrão, **`1` aparece em “Divisores Primos”** para seguir o enunciado. Esse comportamento pode ser mudado em código passando `includeOneInPrimeDivisors: false`.
- Números válidos: `n >= 1` e até `long.MaxValue`. Para valores muito grandes, o tempo cresce com `√n`.

---

## Licença

MIT.
