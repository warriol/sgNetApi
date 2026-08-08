# sgNetApi
Sistema de Gestión en .net, API backend/frontend solution.

# Arquitectura
sgNetApi/                          <-- Raíz del Repositorio Git
├── .gitignore
├── .env
├── README.md
├── LICENSE
├── docker-compose.yml             <-- Servicios locales (PostgreSQL, PgAdmin, etc.)
└── backend/                       <-- Solución .NET
    ├── sgNetApi.sln
    └── src/
        ├── sgNetApi.Api/          <-- Controladores / Minimal APIs y Endpoints JWT
        ├── sgNetApi.Application/  <-- Casos de uso, DTOs, Lógica de Negocio
        ├── sgNetApi.Domain/       <-- Entidades (Usuarios, Roles, Permisos)
        └── sgNetApi.Infrastructure/ <-- DbContext (EF Core + Postgres), Repositorios
└── frontend/                       <-- 

# Requerimientos
- docker desktop
- Microsoft .NET SDK 10

# Crear Sln
```bash
# 1. Crear el archivo de solución dentro de backend/
dotnet new sln -n sgNetApi

# 2. Crear los proyectos .NET dentro de las carpetas que ya armaste
dotnet new webapi -n sgNetApi.Api -o src/sgNetApi.Api
dotnet new classlib -n sgNetApi.Application -o src/sgNetApi.Application
dotnet new classlib -n sgNetApi.Domain -o src/sgNetApi.Domain
dotnet new classlib -n sgNetApi.Infrastructure -o src/sgNetApi.Infrastructure

# 3. Vincular los proyectos al archivo de solución (.sln)
# Estando en J:\Docker\net\sgNetApi\backend
dotnet sln add src/sgNetApi.Api/sgNetApi.Api.csproj
dotnet sln add src/sgNetApi.Application/sgNetApi.Application.csproj
dotnet sln add src/sgNetApi.Domain/sgNetApi.Domain.csproj
dotnet sln add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj

# Inyección de dependencias
# Api depende de Application e Infrastructure
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj reference src/sgNetApi.Application/sgNetApi.Application.csproj
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj reference src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj

# Infrastructure depende de Application (para implementar interfaces/repositorios)
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj reference src/sgNetApi.Application/sgNetApi.Application.csproj

# Application depende únicamente de Domain
dotnet add src/sgNetApi.Application/sgNetApi.Application.csproj reference src/sgNetApi.Domain/sgNetApi.Domain.csproj

# Infrastructure también requiere acceso a Domain (para mapear entidades)
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj reference src/sgNetApi.Domain/sgNetApi.Domain.csproj

# Instalar paquetes NuGet
# En la capa Infrastructure: Proveedor PostgreSQL y ASP.NET Core Identity para EF Core
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# En la capa Api: Herramientas de diseño EF Core para poder generar migraciones
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj package Microsoft.EntityFrameworkCore.Design

# Verificar
dotnet build
```

# Comandos

- primero inicializar docker desktop

```bash
docker compose up -d
```