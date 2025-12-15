# 🚀 ERP System – .NET Multi‑Tier Architecture

<p align="center">
  <img src="https://img.shields.io/badge/.NET-7-blueviolet" />
  <img src="https://img.shields.io/badge/Architecture-3--Tier-success" />
  <img src="https://img.shields.io/badge/Frontend-ASP.NET%20MVC-blue" />
  <img src="https://img.shields.io/badge/API-RESTful-orange" />
</p>

<p align="center">
  <img src="https://media3.giphy.com/media/v1.Y2lkPTc5MGI3NjExcTNrajEwbnN4YjRrYmlpeGtlbGp1b3RucmV3NGkzMTVmMWUzeTR0NSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/26tn33aiTi1jkl6H6/giphy.gif" width="600" />
</p>

---

## 🧠 System Overview

This **ERP System** is built with **.NET** using a **clean 3‑Tier architecture** and is divided into **two main projects**:

1. **ERP‑API** → Business logic & data access (REST API)
2. **ERP‑MVC** → Frontend (UI) that consumes the API

The design ensures **scalability**, **maintainability**, and **separation of concerns**.

---

## 🏗️ High‑Level Architecture

```mermaid
graph TD
    UI[ASP.NET MVC Frontend] -->|HTTP / JSON| API[ERP API]
    API --> APP[Application Layer]
    APP --> DAL[Data Access Layer]
    DAL --> DB[(SQL Server)]
```

---

## 📁 Project Structure

```text
Project-main
│
├── ERP-API
│   ├── ERP-API.API          # Presentation Layer (Controllers)
│   ├── ERP-API.Application  # Business Logic
│   └── ERP-API.DataAccess   # Database & Repositories
│
├── ERP-MVC                  # Frontend (MVC)
│
├── DEPI-Project.sln
├── ProjectDoc.pdf
└── ERP-System.pptx
```

---

## 🔥 ERP‑API (3‑Tier Architecture)

<p align="center">
  <img src="https://media.giphy.com/media/26tn33aiTi1jkl6H6/giphy.gif" width="500" />
</p>

### 1️⃣ API Layer (Presentation)

📍 `ERP-API.API`

**Responsibilities:**

* Expose RESTful endpoints
* Handle HTTP requests & responses
* Authentication & Authorization

**Key Components:**

* Controllers
* Program.cs
* API Extensions

---

### 2️⃣ Application Layer (Business Logic)

📍 `ERP-API.Application`

**Responsibilities:**

* Business rules & workflows
* DTOs for clean data transfer
* Service interfaces & implementations

**Key Components:**

* DTOs
* Services
* Interfaces

---

### 3️⃣ Data Access Layer (DAL)

📍 `ERP-API.DataAccess`

**Responsibilities:**

* Database communication
* Entity Framework Core
* Repository pattern

**Key Components:**

* DbContext
* Entities & Enums
* Migrations
* Repositories
* Identity & Security

---

## 🎨 ERP‑MVC (Frontend Layer)

<p align="center">
  <img src="https://media.giphy.com/media/13HgwGsXF0aiGY/giphy.gif" width="500" />
</p>

**Responsibilities:**

* User Interface (UI)
* Consumes ERP‑API
* Handles user interaction

**Structure:**

* Controllers
* Views (Razor Pages)
* Services (API communication)
* Models (ViewModels & DTOs)
* wwwroot (CSS / JS / Assets)

---

## ✨ Key Features

✅ Modular & Scalable Architecture

✅ Clean Separation of Concerns

✅ RESTful API Integration

✅ Entity Framework Core + Migrations

✅ Identity & Role Management

✅ Secure Configuration (appsettings)

✅ Ready for Enterprise‑Level Expansion

---

## 🔄 Request Flow Animation

```mermaid
sequenceDiagram
    User->>MVC: Request Page
    MVC->>API: Call REST Endpoint
    API->>Application: Business Logic
    Application->>DAL: Fetch / Save Data
    DAL-->>Application: Result
    Application-->>API: DTO Response
    API-->>MVC: JSON Data
    MVC-->>User: Render View
```

---

## 🚀 How to Run

1️⃣ Open `DEPI-Project.sln`

2️⃣ Set **Multiple Startup Projects**:

* ERP-API.API
* ERP-MVC

3️⃣ Update connection string in:

* `appsettings.json`

4️⃣ Apply migrations:

```bash
dotnet ef database update
```

5️⃣ Run the solution 🎉

---

## 🧩 Future Enhancements

* 📊 Advanced Financial Reports
* 📦 Inventory Forecasting
* 👥 Role‑Based Dashboards
* 📱 Mobile App Integration
* ☁️ Cloud Deployment (Azure)

---

## 👨‍💻 Author

**Eng. Joe**
.NET | ERP | Clean Architecture

---

<p align="center">
  ⭐ If you like this project, give it a star!
</p>
