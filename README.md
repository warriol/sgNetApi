# sgNetApi
Sistema de gestión institucional en .NET y Angular para administración de usuarios, dependencias, turnos, escalafón e inventario polical.

# Arquitectura
```bash
sgNetApi/
├── .gitignore
├── .env
├── README.md
├── LICENSE
├── docker-compose.yml
├── docme.md
├── domain-spec.md
├── backend/
│   ├── sgNetApi.sln
│   └── src/
│       ├── sgNetApi.Api/
│       ├── sgNetApi.Application/
│       ├── sgNetApi.Domain/
│       └── sgNetApi.Infrastructure/
└── frontend/
    └── src/
        └── app/
            ├── core/
            │   ├── guards/
            │   ├── interceptors/
            │   └── services/
            ├── features/
            │   ├── landing/
            │   └── admin/
            │       ├── usuarios/
            │       ├── roles/
            │       ├── auditoria/
            │       ├── catalogos/
            │       ├── dependencias/
            │       └── shell/
            ├── app.config.ts
            ├── app.routes.ts
            └── app.component.ts
```

# Funcionalidades actuales

## Seguridad y administración
- JWT con autenticación y autorización basada en permisos.
- Roles y permisos por usuario.
- Auditoría HTTP centralizada con middleware.
- Gestión de usuarios, estados, roles y permisos.

## Dependencias y turnos
- Administración de unidades ejecutoras y dependencias.
- Definición de turnos con tipos operativos:
  - 4 turnos de 6 hs
  - 3 turnos de 8 hs
  - 2 turnos de 12 hs
  - 1 turno de 24 hs
- Nombre explicativo de turnos para su selección posterior en la dependencia.
- La hora de fin se calcula automáticamente según el tipo de turno y la hora de inicio; no se ingresa manualmente.
- Si el turno cruza medianoche, el sistema lo informa claramente para evitar errores de configuración.
- Asignación de turno a la dependencia.

## Escalafón
- Asignación de funcionarios a dependencias.
- Turno inicial por defecto: No asignado.
- Cambio del turno del funcionario operativamente desde el escalafón.
- Regla de negocio:
  - 24 hs -> turno único
  - 4x6 / 3x8 / 2x12 -> Turno 1 a Turno 4

## Indumentaria y revista
- Gestión del inventario de armas, chalecos antibalas y esposas.
- Asignación por funcionario.
- Registro de revista y trazabilidad del equipamiento entregado.

# Requerimientos
- Docker Desktop
- .NET SDK 10
- Node.js + npm
- PostgreSQL / EF Core

# Ejecutar el proyecto

## Backend
```bash
cd backend
Set-Location 'j:\Docker\net\sgNetApi\backend'
dotnet restore
dotnet run --project src/sgNetApi.Api/sgNetApi.Api.csproj
```

Swagger:
- http://localhost:5283/swagger

## Frontend
```bash
cd frontend
npm install
npx ng serve
```

Aplicación:
- http://localhost:4200/

## Base de datos
```bash
docker compose up -d
```

# Módulos del panel administrativo
- Gestión de Usuarios
- Roles y Permisos
- Gestión de Catálogos
- Dependencia
- Gestionar Turnos
- Gestionar Escalafón
- Gestionar Indumentaria
- Auditoría

# Datos relevantes de la implementación
- Los turnos operativos se gestionan desde el catálogo de turnos.
- La dependencia usa un turno general de operación.
- El funcionario lleva su turno individual dentro del escalafón.
- El módulo de indumentaria se conecta a la API y persiste los cambios reales.
- La persistencia se valida con la base de datos y endpoints reales de la API.

# Validación actual
El proyecto ya quedó verificado con:
- `dotnet test --nologo` en backend
- `npm run build` en frontend

