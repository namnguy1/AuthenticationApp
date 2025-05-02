# 🍎 NutritionApp

**NutritionApp** is a **.NET 9.0-based application** designed to manage user authentication and nutrition-related features.  
It supports user registration, email verification, login (including Google), and JWT-based authentication.

---

## 📚 Table of Contents

- [✨ Features](#-features)
- [🏗 Project Structure](#-project-structure)
- [🧪 Technologies Used](#-technologies-used)
- [🚀 Getting Started](#-getting-started)
  - [🛠 Prerequisites](#-prerequisites)
  - [⚙️ Installation](#️-installation)
  - [▶️ Running the Application](#️-running-the-application)
- [🔐 Environment Variables](#-environment-variables)
- [📡 API Endpoints](#-api-endpoints)
- [🪪 License](#-license)

---

## ✨ Features

- ✅ User registration with email verification  
- 🔐 Secure password hashing using **BCrypt**  
- 🔑 JWT-based authentication  
- 🔄 Google login integration  
- 🧱 Modular Clean Architecture (based on Domain-Driven Design)

---

## 🏗 Project Structure
├── NutritionApp.sln # Solution file
├── NutritionApp.Api # API layer
├── NutritionApp.Application # Application layer (business logic)
├── NutritionApp.Domain # Domain layer (entities and interfaces)
├── NutritionApp.Infrastructure # Infrastructure layer (data access and services)
└── .github/ # GitHub workflows


### 🔍 Key Folders

- **NutritionApp.Api**: API controllers & startup configuration  
- **NutritionApp.Application**: Business logic, commands, queries, handlers  
- **NutritionApp.Domain**: Domain entities & interfaces  
- **NutritionApp.Infrastructure**: DB context, repositories, external services  

---

## 🧪 Technologies Used

- **.NET 9.0**
- **Entity Framework Core** (SQL Server)
- **MediatR** (CQRS pattern)
- **FluentValidation**
- **BCrypt.Net**
- **MailKit**
- **JWT**
- **Google Authentication**

---

## 🚀 Getting Started

### 🛠 Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server
- Visual Studio or Visual Studio Code
- Node.js *(optional – for frontend)*

