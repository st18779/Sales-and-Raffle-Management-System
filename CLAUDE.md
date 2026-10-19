# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

ChineseSale: a gift-raffle site. Angular 20 client (`client/`), ASP.NET Core 8 Web API (`server/`), SQL Server 2022. Visitors browse gifts/donors, fill a basket and check out; Admin-role users manage gifts, donors, categories, view sales reports and draw raffle winners.

## Commands

| Task | Command | Directory |
| --- | --- | --- |
| Run everything (db + api + client); needs `.env` (copy from `.env.example`) | `docker compose up --build` | repo root |
| Start only the database | `docker compose up -d db` | repo root |
| Run API (port 5080) | `dotnet run --project server --launch-profile http` | repo root |
| Build API | `dotnet build server.csproj` | `server/` |
| Add EF migration | `dotnet ef migrations add <Name>` | `server/` |
| Run client (port 4200) | `npm ci && npm start` | `client/` |
| Build client | `npm run build` | `client/` |
| Client tests (Karma/Jasmine) | `npm test` | `client/` |
| Single client spec | `npx ng test --include src/app/services/gift.service.spec.ts` | `client/` |

There is no .NET test project. Under Docker Compose the API is at `http://localhost:8088` (Swagger at `/swagger`), the client at `http://localhost:4200`.

## Architecture

### Server (`server/`)
Layered: `Controllers` → `Services` → `Repositories` → `Data/ChineseSaleContext` (EF Core). Each service/repository has an interface in `Interfaces/` and is registered as scoped in `Program.cs` — new ones must be registered there. Controllers exchange `DTOs/`, not `Models/` entities. Routes are `api/[controller]/<ActionName>` (e.g. `/api/Gift/giftsList`, `/api/Purchases/GetBasket/{userId}`).

`Program.cs` pipeline: custom `RequestLoggingMiddleware` and `RateLimitingMiddleware` (in-memory, per client, configured via `RateLimiting:RequestLimit` / `RateLimiting:TimeWindowMinutes`) → CORS policy `AllowAngular` (only `http://localhost:4200`) → Swagger (if `EnableSwagger` or Development) → optional HTTPS redirect (`EnableHttpsRedirection`) → JWT auth. Pending EF migrations are applied automatically at startup; failures are logged and startup continues. JSON uses `ReferenceHandler.IgnoreCycles`. Serilog logs to console and `server/Logs/log-YYYYMMDD.txt`.

Auth: `TokenService` issues JWTs with `ClaimTypes.NameIdentifier` (user id) and `ClaimTypes.Role`; admin endpoints use `[Authorize(Roles = "Admin")]`.

### Secrets and configuration
- Secrets live only in the git-ignored root `.env` (template: `.env.example`): `MSSQL_SA_PASSWORD` and `JWT_SECRET_KEY`. `docker-compose.yml` reads them with `${VAR:?...}`, so Compose fails fast if `.env` is missing, and passes them to the API as `ConnectionStrings__DefaultConnection` (built from the password) and `JwtSettings__SecretKey`.
- `server/appsettings.json` intentionally has **no** connection string or `SecretKey` — only non-secret `JwtSettings` (`Issuer`, `Audience`, `ExpiryMinutes`). Never add secrets back there, to `docker-compose.yml`, or to Dockerfiles. For `dotnet run`, set those two env vars (or use `dotnet user-secrets`).
- `.env` is also in `.dockerignore`, so it is never copied into images.
- SQL Server applies `MSSQL_SA_PASSWORD` only when the `sql_data` volume is created. To rotate it without losing data, run `ALTER LOGIN sa WITH PASSWORD = ...` via `sqlcmd` in the running `chinese-sale-db` container first, then update `.env` and `docker compose up -d`.

### Client (`client/src/app/`)
Standalone components, configured in `app.config.ts` (`provideHttpClient(withInterceptors([jwtInterceptor]))`). Routes in `app.routes.ts`: `/gifts/*` and `/donors/*` are nested under `GiftsNav`/`DonorsNav` shells and protected by `authGuard`; `/basket/:userId`, `/login`, `/register` are top-level.

- Auth state lives in `localStorage` (`token`, `user`). `AuthService.login` stores them and exposes `user$`; `core/jwt.interceptor.ts` attaches `Authorization: Bearer` and logs out on a 401 (except `/api/Auth/` calls); `core/auth.guard.ts` checks that a non-expired token exists (role checks are enforced server-side).
- Each service in `services/` hardcodes its API base URL as `http://localhost:8088/api/<Controller>` (no environment files). Running the API via `dotnet run` (port 5080) requires changing these URLs.
- Shared state uses `BehaviorSubject`s in services (e.g. `PurchasesService.basket$`, `totalPrice$`).
- Some component folders/classes use camelCase names (`checkOut`, `endCheckOut`) unlike the kebab-case rest — match existing imports.

## Working guidelines
- Choose the simplest, shortest solution that solves the problem, and match the existing code style.
- Do not add new libraries or new layers/abstractions without asking first.
- Do not leave dead code, `console.log` calls, or duplicated code.
- When there are several reasonable approaches, present them briefly with the pro and con of each, and recommend one.

## Notes
- Some code comments are in Hebrew.
