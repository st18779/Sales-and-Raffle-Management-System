# 🎁 GiftRaffle

A modern, clean, and fully responsive Chinese-auction (gift raffle) platform. Customers buy raffle tickets for gifts, and admins manage the catalog and draw the winners. Built with Angular, ASP.NET Core and SQL Server, and ready to run with Docker.

---

## ✨ Features
- 📱 **Fully Responsive:** Top navigation on desktop and a bottom navigation bar on mobile.
- 🎨 **Modern Design:** Clean gold-and-cream layout with a focus on high-quality gift imagery.
- 🛒 **Smart Basket:** Items are saved as a draft in the database and only become real purchases after a confirmed checkout.
- 🏆 **Winner Draw:** Admins draw a winner for each gift, weighted by the number of tickets each buyer purchased.
- 🔐 **Secure Access:** JWT authentication with role-based authorization (Admin and Customer).
- ✅ **Validation:** Rules enforced on both the client and the server.

### 👤 Customers
- Register and sign in
- Browse the gift catalog with category, donor and ticket price
- Add gifts to a floating basket and check out with an order summary
- See the winner on each gift after the raffle is drawn (drawn gifts can no longer be bought)

### 🛠️ Admins
- Add, edit and delete gifts (a gift that already has purchases cannot be deleted)
- Add, edit and delete donors (a donor who still has gifts cannot be deleted)
- Draw a winner for each gift
- Admin-only API endpoints for income and winners reports

---

## 📸 Screenshots

<img width="1180" height="1474" alt="home" src="https://github.com/user-attachments/assets/3eba43c3-b4ee-4ce3-a62c-cbb5c1a6d208" />

<img width="2822" height="1524" alt="gift" src="https://github.com/user-attachments/assets/a7ac28a2-0e19-46b8-a363-f9df25242966" />

<img width="2816" height="1508" alt="payment" src="https://github.com/user-attachments/assets/0bf0671b-f68f-432c-836b-5d1eff2f7530" />

<img width="1176" height="1474" alt="mobile gift" src="https://github.com/user-attachments/assets/17beea7e-5a03-4ca3-9365-d5a0c58553eb" />



---

## 🛠️ Technologies Used
- **Angular 20:** Standalone components, RxJS and TypeScript for the website.
- **ASP.NET Core 8:** Web API with Entity Framework Core (code-first), JWT, Serilog and Swagger.
- **SQL Server 2022:** The database, with migrations applied automatically on startup.
- **Docker & Docker Compose:** One command to run the database, API and website.

---

## 🚀 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Node.js](https://nodejs.org/) 20 or newer (only for running the website outside Docker)

### 1. Clone the repository
```bash
git clone https://github.com/st18779/Sales-and-Raffle-Management-System.git
cd Sales-and-Raffle-Management-System
```

### 2. Create your `.env` file
Secrets are **not** stored in the repository. Copy the example file and fill in your own values:
```bash
cp .env.example .env
```
Set a strong SQL Server password and a long random JWT signing key.

### 3. Run everything with Docker
```bash
docker compose up --build
```

| Service | URL |
| --- | --- |
| Website | http://localhost:4200 |
| API | http://localhost:8088 |
| Swagger | http://localhost:8088/swagger |

### Development mode
Run the database and API in Docker and the Angular dev server locally, so UI changes appear instantly:
```bash
docker compose up -d db api
cd client
npm ci
npm start
```
> Do not run the `client` container and `npm start` together: both use port 4200.

---

## 👑 Admin Users
There is no admin registration screen. Admin users are created directly in the database by setting `IsAdmin = 1` on a user in the `Users` table. Registering through the website always creates a regular customer.

---

## 📂 Project Structure
```
.
├── client/                  # Angular application
│   └── src/app/
│       ├── components/      # Pages and UI components
│       ├── services/        # HTTP services and shared state
│       ├── core/            # Auth guard, JWT interceptor, shared directives
│       └── models/          # TypeScript models
├── server/                  # ASP.NET Core Web API
│   ├── Controllers/         # HTTP endpoints
│   ├── Services/            # Business logic
│   ├── Repositories/        # Data access (EF Core)
│   ├── Interfaces/          # Service and repository contracts
│   ├── DTOs/                # Request and response models
│   ├── Models/              # Database entities
│   ├── Data/                # DbContext
│   ├── Migrations/          # EF Core migrations
│   └── Middleware/          # Request logging and rate limiting
├── docker-compose.yml
├── .env.example             # Template for required environment variables
└── README.md
```

---

## 🔌 API Overview
Open Swagger at `/swagger` for the full list. The main groups are:
- **Auth:** register and login (returns a JWT)
- **Gift:** list, details, create, update, delete and random winner
- **Donor:** list, details, create, update and delete
- **Category:** list and details
- **Purchases:** basket (add, remove, load), checkout, and income and winners reports (admin)

Admin endpoints require an `Authorization: Bearer <token>` header with an admin token.

---

## 🧰 Useful Commands

| Task | Command | Folder |
| --- | --- | --- |
| Run everything | `docker compose up --build` | repo root |
| Start only the database | `docker compose up -d db` | repo root |
| Stop everything | `docker compose down` | repo root |
| Run the website | `npm start` | `client/` |
| Build the website | `npm run build` | `client/` |
| Add an EF migration | `dotnet ef migrations add <Name>` | `server/` |

---

## 👩‍💻 Author
Built by [@st18779](https://github.com/st18779).
