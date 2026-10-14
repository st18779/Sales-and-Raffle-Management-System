# Raffle-Sales-Management-System


ChineseSale is a web application for managing a gift raffle. Users can browse gifts, add tickets to a basket, and check out. Administrators can manage gifts, donors, and categories, and access raffle and sales operations.

## Project Components

- `client/` is an Angular 20 single-page application. It includes registration and login, gift browsing and search, donor listings, a basket, and checkout flows.
- `server/` is an ASP.NET Core 8 Web API. It uses Entity Framework Core 8 with SQL Server, JWT authentication, role-based authorization, Swagger, request logging, and rate limiting.
- `docker-compose.yml` starts the client, API, and SQL Server. SQL Server data is stored in the `sql_data` Docker volume.

The database contains users, donors, gifts, categories, and purchases. On startup, the API checks for pending EF Core migrations and attempts to apply them automatically.

## Requirements

For the containerized setup, install Docker Desktop (or Docker Engine) with the Compose plugin.

For local development without containerizing the client/API, install:

- .NET 8 SDK
- Node.js 20 or later and npm
- SQL Server, either locally or via Docker

## Run with Docker Compose

From the repository root, run:

```bash
docker compose up --build
```

Open the Angular application at [http://localhost:4200](http://localhost:4200). The API is available at [http://localhost:8088](http://localhost:8088), and Swagger UI is at [http://localhost:8088/swagger](http://localhost:8088/swagger).

Stop the services with `Ctrl+C`, or run `docker compose down` in another terminal. The database volume is retained. To delete the database data as well, run `docker compose down --volumes`; this is destructive and cannot be undone.

The Compose configuration currently contains development database credentials. Replace them before using the project beyond local development, and keep the API connection string in sync with the SQL Server password.

## Run Locally

Start SQL Server first. To use the SQL Server service from Compose, start only the database from the repository root:

```bash
docker compose up -d db
```

The API's default connection string is in `server/appsettings.json`; update it to match your local database. Also replace the JWT signing key with a sufficiently long, private value. Configuration can be supplied through .NET environment variables rather than the settings file. In PowerShell, for example:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=ChineseSale;User Id=sa;Password=<your-sql-password>;TrustServerCertificate=True;Encrypt=False;"
$env:JwtSettings__SecretKey = "<your-private-signing-key>"
```

In a terminal at the repository root, run the API:

```powershell
dotnet run --project server --launch-profile http
```

The HTTP launch profile listens on `http://localhost:5080` and opens Swagger. The Angular services currently use the fixed API base URL `http://localhost:8088/api`, and the API's CORS policy allows `http://localhost:4200`. For local client/API development, either keep the API reachable at port 8088 and the client at port 4200, or update those client service URLs and the API CORS origin to match your chosen ports.

In a second terminal, install and run the client:

```powershell
cd client
npm ci
npm start
```

Open [http://localhost:4200](http://localhost:4200). Angular rebuilds the app when source files change.

## Common Commands

Run these from the indicated directory:

| Task | Command | Directory |
| --- | --- | --- |
| Start client | `npm start` | `client/` |
| Build client | `npm run build` | `client/` |
| Run client unit tests | `npm test` | `client/` |
| Build API | `dotnet build server.csproj` | `server/` |
| Start all services | `docker compose up --build` | repository root |

The Angular unit tests use Karma/Jasmine. The repository does not currently include a separate .NET test project.

## API Overview

All API routes use the `/api` prefix. The main route groups are:

| Route | Purpose |
| --- | --- |
| `/api/Auth` | Register and log in (`POST /register`, `POST /login`) |
| `/api/Gift` | List, search, sort, and retrieve gifts; create, update, delete, and draw winners |
| `/api/Category` | List and retrieve categories; create, update, and delete categories |
| `/api/Donor` | List, search, and retrieve donors; create, update, and delete donors |
| `/api/Purchases` | Basket operations, checkout, and purchase reporting |
| `/api/User` | Retrieve and manage user records |

For example, `GET /api/Gift/giftsList` lists gifts and `GET /api/Category/categoriesList` lists categories. Consult Swagger for request/response schemas and the complete route list. Endpoints decorated with `[Authorize(Roles = "Admin")]` require a JWT for a user with the `Admin` role; Swagger includes a Bearer authorization option. The Angular routes for gifts and donors also use a client-side login guard.

## Configuration and Security

- The API reads the SQL Server connection string from `ConnectionStrings:DefaultConnection` and JWT options from `JwtSettings` (`SecretKey`, `Issuer`, `Audience`, and `ExpiryMinutes`). Environment variables use double underscores, for example `JwtSettings__SecretKey`.
- The API's CORS policy currently permits only `http://localhost:4200`.
- Compose sets the API to Development, enables Swagger, and disables HTTPS redirection for local HTTP use.
- Treat all credentials currently present in configuration files as development-only. Use private environment-specific configuration or a secret manager for real deployments; do not commit production database passwords or JWT signing keys.
- The API writes logs to the console and rolling files under `server/Logs/`.

## Repository Layout

```text
client/                 Angular application
server/Controllers/     HTTP API endpoints
server/Data/            Entity Framework database context
server/DTOs/            API request and response models
server/Models/          Database entities
server/Repositories/    Data access implementations
server/Services/        Application logic and token service
server/Migrations/      Entity Framework Core migrations
docker-compose.yml      Local client, API, and SQL Server services
```
