# NZwalks API – ASP.NET Core 8 Web API

A RESTful Web API built with ASP.NET Core 8 and Entity Framework Core to manage regions, walks, walk difficulties, and user roles. Implements JWT-based authentication and follows clean architecture best practices.

---

## 🔧 Tech Stack

- ASP.NET Core 8 Web API
- C# / .NET 8
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Authentication
- Swagger (OpenAPI)

---

## ✅ Features

- 🌍 CRUD operations for Regions and Walks
- 📏 Manage Walk Difficulty Levels
- 🔐 JWT-based Authentication
- 👥 Role-based Authorization (Admin/User)
- 🗃️ EF Core Code-First + Migrations
- 📄 Swagger for API Documentation

---

## 🚀 Getting Started


1. Clone the repo  
```bash
git clone https://github.com/KovvuriAnusha/NZwalks.git

## 🧩 Configure Database
In appsettings.json, update the connection strings if needed:
"ConnectionStrings": {
  "NZWalksConnectionString": "Server=YOUR_SERVER;Database=NZWalks;Trusted_Connection=True;",
  "NZWalksAuthConnectionString": "Server=YOUR_SERVER;Database=NZWalksAuth;Trusted_Connection=True;"
}
For example:
"Server=LAPTOP-H5Q0R8MA\\SQLEXPRESS;Database=NZWalks;Trusted_Connection=True;"

## 📦 Apply Migrations (If DB doesn't exist)
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext

## ▶️ Run the API
dotnet run
By default, the API will run at:
By default, the API will run at:

## 🔐 Auth Testing (Optional)
To test protected endpoints:

Register a new user

Use /api/token to get a JWT token

Add the token via Swagger's Authorize button
