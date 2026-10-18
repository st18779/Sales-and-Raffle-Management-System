# Sales and Raffle Management System


## What is this project?

ChineseSale is a website for running a gift raffle. A visitor can look through the gifts, choose gifts to enter, and check out their basket. An administrator can maintain the gift, donor, and category information and draw raffle winners.

In short: the website is the part people use, the API is the part that processes their requests, and SQL Server stores the information.

## What can people do?

- **Visitors and signed-in users:** browse and search gifts, view donors, add gift entries to a basket, and check out.
- **Administrators:** manage gifts, donors, and categories, and use raffle and sales reports. These actions require an account with the Admin role.

## Start the whole project

The easiest way to try the project is with Docker Compose. Install Docker Desktop and open a terminal in the project folder.

First, create your private settings file. Copy `.env.example` to `.env` and replace the example values with your own database password and token-signing key (see the comments in the file for the rules). `.env` is ignored by git and must never be committed.

```bash
cp .env.example .env
```

Then run:

```bash
docker compose up --build
```

The first run may take a few minutes while Docker downloads and builds the required software. When it is running, open:

- **Website:** [http://localhost:4200](http://localhost:4200)
- **API documentation:** [http://localhost:8088/swagger](http://localhost:8088/swagger)

The API documentation is mainly useful to developers; visitors can use the website. To stop the project, press `Ctrl+C` in the terminal. The database is kept for the next run. To remove the database and all saved data too, run `docker compose down --volumes` from the project folder. This permanently deletes that data.

SQL Server only uses `MSSQL_SA_PASSWORD` when the database volume is first created. If you change it later, either change the `sa` password inside SQL Server as well, or (deleting all data) run `docker compose down --volumes`.

## What is included?

- `client/` - the website, built with Angular.
- `server/` - the API, built with ASP.NET Core. It handles sign-in, gift and donor information, baskets, checkout, and raffle operations.
- SQL Server - the database that stores users, gifts, donors, categories, and purchases.
- `docker-compose.yml` - starts the website, API, and database together.

When the API starts, it checks whether the database needs updates and attempts to apply them automatically.

## Run parts separately (development)

This section is for developers who want to run or change the website and API separately. You need Docker, .NET 8 SDK, and Node.js 20 or later with npm.

### 1. Start the database

From the project folder, run:

```bash
docker compose up -d db
```

### 2. Start the API

The database connection and token-signing key are not stored in `server/appsettings.json`; the API will not start without them. Provide them as environment variables, using the same values as in your `.env`:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=ChineseSale;User Id=sa;Password=<MSSQL_SA_PASSWORD from .env>;TrustServerCertificate=True;Encrypt=False;"
$env:JwtSettings__SecretKey = "<JWT_SECRET_KEY from .env>"
dotnet run --project server --launch-profile http
```

The API's local HTTP address is `http://localhost:5080`. The website's current configuration sends API requests to port `8088`, so the simplest way to run the entire application is the Docker Compose method above. If you run the website and API separately, update the website's API address to match the API port and allow the website's address in the API's CORS configuration.

### 3. Start the website

Open a second terminal:

```powershell
cd client
npm ci
npm start
```

Then open [http://localhost:4200](http://localhost:4200). The website refreshes as you change its source files.

## Developer reference

### Main API areas

All API addresses start with `/api`. For example, `/api/Gift/giftsList` returns the gift list.

| API area | What it handles |
| --- | --- |
| `/api/Auth` | Registering and signing in |
| `/api/Gift` | Browsing, searching, and managing gifts; drawing winners |
| `/api/Category` | Viewing and managing gift categories |
| `/api/Donor` | Viewing and managing donors |
| `/api/Purchases` | Baskets, checkout, and sales reports |
| `/api/User` | User records |

The complete list of requests and their data formats is available in Swagger at [http://localhost:8088/swagger](http://localhost:8088/swagger) when using Docker Compose. Requests marked for administrators require a valid sign-in token and the Admin role.

### Common commands

Run each command from the directory shown:

| Task | Command | Directory |
| --- | --- | --- |
| Start the website | `npm start` | `client/` |
| Build the website | `npm run build` | `client/` |
| Run website tests | `npm test` | `client/` |
| Build the API | `dotnet build server.csproj` | `server/` |
| Start all services | `docker compose up --build` | project folder |

Website tests use Karma and Jasmine. There is no separate .NET test project in the repository.

### Configuration and security

- Secrets live only in the git-ignored `.env` file (template: `.env.example`). Docker Compose passes them to the API as `ConnectionStrings__DefaultConnection` and `JwtSettings__SecretKey`. Non-secret token settings (`Issuer`, `Audience`, `ExpiryMinutes`) stay in `server/appsettings.json`.
- The API currently allows website requests from `http://localhost:4200` only.
- The Compose setup enables the API's development documentation and uses plain HTTP for local development.
- Do not use the credentials or signing key from local configuration for a public or production deployment. Use private secrets and a production-ready configuration.
- API logs are written to the terminal and to daily files under `server/Logs/`.

### Folder guide

```text
client/                 Website
server/Controllers/     API request handlers
server/Data/            Database setup
server/DTOs/            Data sent to and from the API
server/Models/          Database records
server/Repositories/    Database access
server/Services/        Application behavior
server/Migrations/      Database update history
docker-compose.yml      Local startup configuration
```
