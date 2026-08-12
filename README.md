# Heracles

A small full-stack Todo application built with **C#/.NET, ASP.NET Core Minimal API, PostgreSQL and Angular**.

## Getting Started

### Prerequisites

- .NET SDK
- .NET EF Core CLI
- Node.js
- npm
- Podman

### 1. Clone the repository

```bash
git clone https://github.com/DrJBouille/heracles.git
cd Heracles
```

### 2. Start PostgreSQL

A template is available in docker/template, clone it in docker/heracles folder and modify variable

#### Podman
```bash
cd docker/heracles
podman compose up -d
cd ../..
```

or

#### Docker
```bash
cd docker/heracles
docker compose up -d
cd ../..
```

### 3. Configure .NET User Secrets

Initialize User Secrets for the API project:
```bash
dotnet user-secrets init --project src/Heracles.Api
```

Configure the PostgreSQL connection string:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<postgres-connection-string>" --project src/Heracles.Api
```

Configure the JWT secret:
```bash
dotnet user-secrets set "Jwt:Key" "<your-jwt-secret>" --project src/Heracles.Api
```


### 4. Apply database migrations

```bash
dotnet ef database update --project src/Heracles.Infrastructure --startup-project src/Heracles.Api
```

### 5. Start the API

```bash
dotnet run --project src/Heracles.Api
```

The API will be available at ```http://localhost:5174```

### 6. Start the Angular application

```bash
cd heracles-web-app
npm install
ng serve
```

The frontend will be available at ```http://localhost:4200```

### Stopping the project

#### Podman
```bash
podman compose down
```

#### Docker
```bash
docker compose down
```

### Project structure

Heracles/  
├── src/  
│   ├── Heracles.Api/  
│   ├── Heracles.Application/  
│   ├── Heracles.Domain/  
│   └── Heracles.Infrastructure/  
│  
└── heracles-web-app/