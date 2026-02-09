# Policy Management System

A comprehensive ASP.NET Core Web API application for managing insurance policies, users, and policy enrollments with JWT-based authentication and role-based authorization.

## 🚀 Features

- **User Management**: Create, read, update, and delete user accounts with role-based access control
- **Policy Management**: Full CRUD operations for insurance policies with search and filtering capabilities
- **Policy Enrollment**: Enroll users in policies and manage enrollment status
- **JWT Authentication**: Secure authentication using JSON Web Tokens
- **Role-Based Authorization**: Admin and User roles with different permission levels
- **PostgreSQL Database**: Reliable data persistence with Entity Framework Core
- **Response Filters**: Global response formatting and response time tracking
- **Swagger Documentation**: Interactive API documentation and testing interface

## 🛠️ Technology Stack

- **.NET 10.0**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API development
- **Entity Framework Core 10.0.2**: ORM for database operations
- **PostgreSQL**: Database with Npgsql provider
- **BCrypt.Net**: Password hashing and security
- **JWT Bearer Authentication**: Secure token-based authentication
- **Swagger/OpenAPI**: API documentation

## 📋 Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) (v12 or higher)
- A code editor (Visual Studio, VS Code, or Rider)

## 🔧 Installation & Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd sameer-dotnet-training/PolicyManagement
```

### 2. Configure Database Connection

Update the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "PolicyManagementDbConnectionString": "Host=localhost;Port=5432;Username=your_username;Password=your_password;Database=CSharp"
  }
}
```

### 3. Configure JWT Settings

Update JWT configuration in `appsettings.Development.json`:

```json
{
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters-long",
    "Issuer": "PolicyManagementApp",
    "Audience": "PolicyManagementAppUsers",
    "ExpiryMinutes": 60
  }
}
```

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000/swagger`

## 📚 API Endpoints

### Authentication

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/login` | Authenticate user and get JWT token | Public |

### Users

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/users` | Get all users | Admin only |
| GET | `/api/users/{id}` | Get user by ID | Authenticated |
| POST | `/api/users` | Create new user | Public |
| PUT | `/api/users/{id}` | Update user | Admin only |
| DELETE | `/api/users/{id}` | Delete user | Admin only |

### Policies

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/policies` | Get all policies | Authenticated |
| GET | `/api/policies/{id}` | Get policy by ID | Authenticated |
| GET | `/api/policies/search?minAmount=X&maxAmount=Y` | Search policies by amount range | Authenticated |
| GET | `/api/policies/status?isActive=true` | Get policies by status | Authenticated |
| POST | `/api/policies` | Create new policy | Admin only |
| PUT | `/api/policies/{id}` | Update policy | Admin only |
| DELETE | `/api/policies/{id}` | Delete policy | Admin only |

### Policy Enrollments

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/policyEnrollment` | Enroll user in policy | Authenticated |
| PUT | `/api/policyEnrollment/{userId}/{policyId}` | Update enrollment | Admin only |
| DELETE | `/api/policyEnrollment/{userId}/{policyId}` | Delete enrollment | Admin only |
| GET | `/api/policyEnrollment/user/{userId}` | Get user's enrollments | Authenticated |

## 🗄️ Database Schema

### User Table
- `Id` (int, PK)
- `Name` (string)
- `Email` (string)
- `PasswordHash` (string)
- `Role` (string)

### Policy Table
- `Id` (int, PK)
- `PolicyName` (string, 3-100 chars)
- `PremiumAmount` (int, 1-100000)
- `Description` (string, max 500 chars)
- `IsActive` (bool)
- `CreatedAt` (DateTime)

### UserPolicy Table
- `Id` (int, PK)
- `UserId` (int, FK)
- `PolicyId` (int, FK)
- `Status` (string, default: "Pending")
- `RequestedAt` (DateTime)
- `ApprovedAt` (DateTime, nullable)

## 🏗️ Project Structure

```
PolicyManagement/
├── Controllers/          # API Controllers
│   ├── LoginController.cs
│   ├── PolicyController.cs
│   ├── PolicyEnrollments.cs
│   └── UserController.cs
├── Data/                # Database context
│   └── DbContext.cs
├── Dto/                 # Data Transfer Objects
│   ├── Policy.cs
│   ├── User.cs
│   └── UserPolicy.cs
├── Entities/            # Database entities
│   ├── Policy.cs
│   ├── User.cs
│   └── UserPolicy.cs
├── Filters/             # Custom filters
│   ├── GlobalResponseFilter.cs
│   └── ResponseTimeFilter.cs
├── Repositeries/        # Repository pattern
│   ├── Policy/
│   ├── PolicyEnrollment/
│   └── User/
├── Services/            # Business logic
│   ├── Login/
│   ├── Policy/
│   ├── PolicyEnrollement/
│   └── User/
├── Scripts/             # Database scripts
│   └── Policy.sql
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

## 🔐 Authentication & Authorization

The application uses JWT (JSON Web Token) for authentication:

1. **Login**: Send credentials to `/api/login` to receive a JWT token
2. **Authorization**: Include the token in the `Authorization` header:
   ```
   Authorization: Bearer <your-token>
   ```
3. **Roles**: Two roles are supported:
   - `User`: Basic access to view and enroll in policies
   - `Admin`: Full access to all operations

## 🧪 Testing with Swagger

1. Navigate to `http://localhost:5078/swagger`
2. Click "Authorize" button
3. Enter your JWT token in the format: `Bearer <token>`
4. Test API endpoints interactively

## 📦 Dependencies

- `Microsoft.EntityFrameworkCore` (10.0.2)
- `Microsoft.EntityFrameworkCore.Design` (10.0.2)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (10.0.0)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (10.0.2)
- `BCrypt.Net-Next` (4.0.3)
- `Swashbuckle.AspNetCore` (10.1.1)

## 🔄 Migration Commands

Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Update database:
```bash
dotnet ef database update
```

Remove last migration:
```bash
dotnet ef migrations remove
```

## 🛡️ Security Features

- Password hashing using BCrypt
- JWT token-based authentication
- Role-based authorization
- Secure connection string management
- Input validation on all endpoints

## 📝 License

This project is part of a .NET training assignment.

## 👤 Author

**Sameer Divami**

## 🤝 Contributing

This is a training project. For any suggestions or improvements, please create an issue or pull request.

---

**Note**: Remember to update the database connection string and JWT secret key before running the application in production.