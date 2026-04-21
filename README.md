# ProgiInterview — Bid Calculation Tool

Car-auction bid calculator: given a vehicle base price and type, returns the total cost with a transparent per-segment fee breakdown (Basic buyer, Seller's special, Association, Storage).

Built as a DDD / CQRS reference implementation on .NET 10 + Vue 3.

## Tech stack

- **Backend:** .NET 10, C# 13, ASP.NET Core, MediatR, FluentValidation, AutoMapper
- **Frontend:** Vue 3 (`<script setup>`), TypeScript, Vite, Pinia, Vue Router, Axios
- **Tests:** xUnit + FluentAssertions + NSubstitute (backend), Vitest + Vue Test Utils (frontend)
- **Infra:** Docker Compose, nginx reverse proxy, GitHub Actions

## Project layout

```
src/
├── Domain/         Entities, enums, value objects — zero dependencies
├── Application/    MediatR queries + handlers, validators, AutoMapper profiles
├── API/            Thin controllers, middleware, composition root
└── Client/         Vue 3 SPA — centralized styles/, no inline CSS

tests/
├── Domain.UnitTests/
└── Application.UnitTests/

docker/             Dockerfiles + compose + nginx config
docs/               Architecture, API reference, fee rules, scalability, CI/CD, testing
```

Stateless — no database, no Persistence/Infrastructure projects.

## Run

```bash
# Full stack via Docker
docker compose -f docker/docker-compose.yml up --build
# API → http://localhost:5000   Client → http://localhost:3000
```

## Develop

```bash
# Backend
dotnet build
dotnet test
dotnet run --project src/API

# Frontend
cd src/Client
npm install
npm run dev
npm run build
npx vitest run
```

## API

Single endpoint. See [`docs/api-reference.md`](docs/api-reference.md) for the full contract and error envelope.

```
GET /api/bid/calculate?vehiclePrice=1800&vehicleType=Luxury
```

## Documentation

- [`docs/architecture.md`](docs/architecture.md) — layers, request flow, cross-cutting concerns
- [`docs/api-reference.md`](docs/api-reference.md) — endpoints, responses, validation layering
- [`docs/fee-rules.md`](docs/fee-rules.md) — canonical fee formulas + PDF reference test cases
- [`docs/scalability.md`](docs/scalability.md) — output caching, rate limiting, horizontal scaling
- [`docs/docker-and-cicd.md`](docs/docker-and-cicd.md) — containers, compose, nginx, GitHub Actions
- [`docs/unit-testing.md`](docs/unit-testing.md) — test stack and conventions
