# Bulk Payment Server

Bulk Payment Server is a clean-architecture .NET 9 backend API for processing bulk payment files.  
Users upload CSV files, which are stored in Azure Blob Storage, parsed into strongly typed payment records, and stored in Azure SQL Database.

This project demonstrates domain-driven design (DDD), layered architecture, and cloud service integration as well.

---

## Features

### **1. CSV Upload & Processing**
- Accepts payment CSV files via REST API.
- Parses each row into a `Payment` domain entity.
- Validates data (amount > 0, required fields, etc.).
- Supports large files.

### **2. Azure Blob Storage Integration**
- Uploaded CSVs are saved directly to Azure Blob Storage.
- Each file is associated with:
  - UserId  
  - FileName  
  - BlobUrl  
  - Timestamp  

### **3. Azure SQL Database Persistence**
#### **Uploads Table**
Stores metadata for each uploaded file:
- UserId  
- FileName  
- BlobUrl  
- UploadedAt  

#### **Payments Table**
Stores parsed payment records:
- InvoiceNumber  
- RecipientName  
- RecipientBsb  
- RecipientAccount  
- Currency  
- Amount  
- CreatedAt  
- Status  
- UploadId (foreign key)

### **4. Clean Architecture**
Solution is structured into four independent layers:

```
BulkPaymentServer.Api           → API, controllers, middleware, DI
BulkPaymentServer.Application   → Use cases, services, interfaces, DTOs
BulkPaymentServer.Domain        → Entities and core business logic
BulkPaymentServer.Infrastructure→ EF Core, SQL, Blob Storage, repositories
```

Decoupled dependencies ensure testability and maintainability.

---

## Architecture Overview

### **Domain Layer**
Defines core business entities:
- `Payment`
- `Upload`

Entities enforce invariants using constructor validation.

---

### **Application Layer**
Contains:
- Service interfaces (`ICsvProcessor`, `IUploadService`, `IPaymentRepository`)
- DTOs for external interaction
- Application-level services (e.g., `UploadService`)

This layer has *no* dependency on EF Core or Azure SDKs.

---

### **Infrastructure Layer**
Implements:
- EF Core DbContext (`BulkPaymentDbContext`)
- Repository implementations (PaymentRepository)
- Azure Blob Storage service
- CSV parsing service

---

### **API Layer**
Provides:
- `/api/upload` endpoint for uploading CSV files
- Logging via Serilog
- Centralized error-handling middleware

---

## Database Schema

### **Uploads Table**

| Column     | Type              |
|------------|-------------------|
| Id         | uniqueidentifier  |
| UserId     | nvarchar(100)     |
| FileName   | nvarchar(255)     |
| BlobUrl    | nvarchar(2000)    |
| UploadedAt | datetime          |

---

### **Payments Table**

| Column           | Type              |
|------------------|-------------------|
| Id               | uniqueidentifier  |
| UploadId         | uniqueidentifier (FK) |
| InvoiceNumber    | int               |
| RecipientName    | nvarchar(200)     |
| RecipientBsb     | nvarchar(20)      |
| RecipientAccount | nvarchar(50)      |
| Currency         | nvarchar(10)      |
| Amount           | decimal(18,2)     |
| CreatedAt        | datetime          |
| Status           | nvarchar(50)      |

---

## CSV Format Example

```
InvoiceNumber,RecipientName,RecipientBsb,RecipientAccount,Currency,Amount
12345,John Doe,123123,999999,USD,5000
12346,Jane Doe,555555,888888,AUD,9000
```

---

## Running EF Core Migrations

### **Create Migration**
```bash
dotnet ef migrations add InitialCreate -p BulkPaymentServer.Infrastructure -s BulkPaymentServer.Api
```

### **Apply Migration**
```bash
dotnet ef database update -p BulkPaymentServer.Infrastructure -s BulkPaymentServer.Api
```

---

## Running the API

```bash
dotnet watch run --project BulkPaymentServer.Api
```

API will be available at:

```
https://localhost:<port>/api/upload
```

Use Postman or Swagger UI to upload CSVs.

---

## Technologies Used
- .NET 9
- Entity Framework Core 9 (Preview)
- Azure SQL Database
- Azure Blob Storage
- Serilog
- Clean Architecture
- C# 13 features
- Python as microservice for payment processor

---


