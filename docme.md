# Arquitectura
- Docker
    - App (.NET 10) IDE VSC
    - BD (PostgreSQL, EF Core)
    - PdAdmin
```bash
┌─────────────────────────────────────────┐      ┌──────────────────────────────────────────┐
│   Frontend 1: Panel Administrativo      │      │   Frontend 2: Sistema de Gestión         │
│   (Gestión de Usuarios/Roles/Permisos)  │      │   (Negocio, Reportes, Operaciones)       │
└────────────────────┬────────────────────┘      └────────────────────┬─────────────────────┘
                     │                                                │
                     │         HTTPS / JSON / JWT Tokens              │
                     └────────────────────┬───────────────────────────┘
                                          │
                                          ▼
                      ┌───────────────────────────────────────┐
                      │        API Central (.NET 10)          │
                      │             `sgNetApi`                │
                      ├───────────────────────────────────────┤
                      │  • Módulo de Autenticación / Roles    │
                      │  • Módulo de Gestión de Información   │
                      └───────────────────┬───────────────────┘
                                          │
                                          ▼
                      ┌───────────────────────────────────────┐
                      │    Base de Datos Unificada (Postgres) │
                      └───────────────────────────────────────┘
```

# Fases
[Fase 1: Infraestructura Local] 
 └── Definir docker-compose.yml con PostgreSQL.
 └── Verificar persistencia con volúmenes locales.

[Fase 2: Arquitectura del Proyecto .NET]
 └── Estructurar el proyecto (Web API + EF Core + PostgreSQL).
 └── Configurar variables de entorno y DbContext.

[Fase 3: Desarrollo de Funcionalidades]
 └── Modelado de entidades, Migraciones Code-First y Controladores/Minimal APIs.
 └── Pruebas unitarias/integración.

[Fase 4: Empaquetado y Despliegue Cloud]
 └── Crear el Dockerfile multi-stage optimizado para .NET.
 └── Aprovisionar PostgreSQL en Aiven / Neon.
 └── Conectar el repositorio de GitHub a Render / Azure y desplegar.

 
# Crear y entrar en carpeta backend
mkdir backend
cd backend

# Crear la solución
dotnet new sln -n sgNetApi

# Crear los proyectos dentro de /src
dotnet new webapi -n sgNetApi.Api -o src/sgNetApi.Api
dotnet new classlib -n sgNetApi.Application -o src/sgNetApi.Application
dotnet new classlib -n sgNetApi.Domain -o src/sgNetApi.Domain
dotnet new classlib -n sgNetApi.Infrastructure -o src/sgNetApi.Infrastructure

# Agregar proyectos a la solución
dotnet sln add src/sgNetApi.Api/sgNetApi.Api.csproj
dotnet sln add src/sgNetApi.Application/sgNetApi.Application.csproj
dotnet sln add src/sgNetApi.Domain/sgNetApi.Domain.csproj
dotnet sln add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj

# Api depende de Application e Infrastructure
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj reference src/sgNetApi.Application/sgNetApi.Application.csproj
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj reference src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj

# Infrastructure depende de Application (para implementar interfaces/repositorios)
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj reference src/sgNetApi.Application/sgNetApi.Application.csproj

# Application depende únicamente de Domain
dotnet add src/sgNetApi.Application/sgNetApi.Application.csproj reference src/sgNetApi.Domain/sgNetApi.Domain.csproj

# Infrastructure también requiere acceso a Domain (para mapear entidades)
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj reference src/sgNetApi.Domain/sgNetApi.Domain.csproj

# En la capa Infrastructure: Proveedor PostgreSQL y ASP.NET Core Identity para EF Core
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# En la capa Api: Herramientas de diseño EF Core para poder generar migraciones
dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj package Microsoft.EntityFrameworkCore.Design

## api de administracion:

datos que me interesa saber de los usuarios

# tabla usuarios
- ci (cédula de identidad del usuarios), será PK de la tabla usuarios, numerico y unico
- creado, ultimo_acceso, serán campos fecha
- correo, será unico
- usuario, por defecto será igual al campo ci, pero alfanumerico
- celular, numérico
- nombre, apellido (seran texto)
- grado, escalafon, unidad ejecutora, dependencia, (serán clave foranea a sus respectivas tablas)
- contraseña, es una hash sha512 se recuerdan las ultimas 5 para que no se repitan
- habilitado, bloqueado (serán buleanos)
- historial (sera clave foranea a la table hisotirla_usuarios, esta tala tendra campos id, fecha, tipo, observaciones, usuario)
- permisos (es una lista de permisos, existe una tabla de permisos que indica a que menues y vistas puede acceder el usuarios)
- role (es una lista de roles, existe una tabla de roles que contiene ya una lista de permisos predeterminados segun su rol)

# tabla grados
- id_grado
- numero
- texto
- abreviatura

# tabla escalafon
- id_escalafon
- nombre
- abtreviatura

# tabla unidad ejecutora
- id_uuee
- nombre
- siglas

# tabla dependencia
- id_dependencia
- id_uuee
- nombre
- siglas

# tabla historial
- id_historial
- fecha
- tipo, tipo de acción
- observaciones
- usuario, quien realiza la acción, puede ser el sistema

# tabla permisos
- id_permiso
- nombre permiso
- descripcion

# tabla roles
- id_rol
- lista de permisos

RE: bloqueado: en true el usuarios podrá iniciar sesión, si pasan 30 dias sin que haya accedido se bloquea y pasa a flase; el usuario podra habilitarse nuevamente solicitando una contraseña nueva

RE: habilitado: en true el usuario podrá iniciar sesion, si falla 3 intentos de iniciar seison pasa a false, solo un admin podrá habilitarlo nuevamente; tb puede ser deshabilitado por desición de un admin


### Las tablas se diseñan en sgNetApi.Domain/Entities como clases agrupadas en archivo .cs
En estos archivos se defienen las clases que finalmente conformaran las diferentes tablas en la base de datos

### Traducir los archivo .cs a tablas reales de PostgreSQL en sgNetApi.Infraestructure/AppDbContext.cs
Aqui se indican las tablas y sus relaciones, utilizando los modelos de los archivos .cs de Entities

```bash
# Instalar la herramienta global dotnet-ef
dotnet tool install --global dotnet-ef
# generar archivos de migracion
dotnet ef migrations add MigracionInicial --project src/sgNetApi.Infrastructure --startup-project src/sgNetApi.Api
# aplicar migracion a la BD
dotnet ef database update --project src/sgNetApi.Infrastructure --startup-project src/sgNetApi.Api
## error en puerto
```

### Migracion de datos en EF Core, eso se realiza en SgNetApi.Api/Program.cs
Aqui construimos la aplicacion y generamos las tablas por primera vez

# Permisos y Roles en EF Core mediante tablas intermedias
```bash
                  ┌──────────────┐
                  │   permisos   │
                  └──────┬───────┘
                         │
            ┌────────────┴────────────┐
            │                         │
            ▼                         ▼
┌───────────────────────┐ ┌───────────────────────┐
│ roles_permisos (M:N)  │ │usuario_permisos (M:N) │
└───────────┬───────────┘ └───────────┬───────────┘
            │                         │
            ▼                         │
     ┌─────────────┐                  │
     │    roles    │                  │
     └──────┬──────┘                  │
            │                         │
            ▼                         │
┌───────────────────────┐             │
│ usuario_roles (M:N)   │             │
└───────────┬───────────┘             │
            │                         │
            └────────────┬────────────┘
                         │
                         ▼
                  ┌──────────────┐
                  │   usuarios   │
                  └──────────────┘
```

# Servicio de Hash y Salt

Paso 1: Crear la Interfaz en sgNetApi.Domain
Crea la carpeta backend/src/sgNetApi.Domain/Interfaces y dentro agrega el archivo IPasswordHasher.cs.
Paso 2: Crear la Implementación Criptográfica en sgNetApi.Infrastructure
Crea la carpeta backend/src/sgNetApi.Infrastructure/Services y dentro agrega el archivo PasswordHasher.cs.
Paso 3: Registrar el servicio en Program.cs
Para que .NET sepa inyectar automáticamente este servicio en cualquier controlador o clase que lo solicite, debemos registrarlo en el contenedor de dependencias.
Abre backend/src/sgNetApi.Api/Program.cs y agrega esta línea antes de builder.Build().

# DataSeeder

Para poblar la base de datos de manera profesional, crearemos una clase DataSeeder en la capa sgNetApi.Infrastructure.
Esta clase comprobará si existen los registros iniciales y, si no existen, insertará los catálogos base (Grados, Escalafones, Unidades Ejecutoras y Dependencias), la estructura de Permisos y Roles, y el primer usuario Administrador en PostgreSQL con su contraseña cifrada mediante IPasswordHasher.

Paso 1: Crear DataSeeder.cs en sgNetApi.Infrastructure
Crea el archivo en backend/src/sgNetApi.Infrastructure/Data/DataSeeder.cs.
Paso 2: Ejecutar el DataSeeder al iniciar la API en Program.cs
Para que el Seeder se ejecute automáticamente cada vez que inicie la aplicación (sin duplicar datos si ya existen), debemos agregarlo al pipeline de inicio de Program.cs.
Abre backend/src/sgNetApi.Api/Program.cs y añade el registro del DataSeeder y la llamada de ejecución.
Paso 3: Probar la ejecución
Ejecuta el backend desde la carpeta backend/src/sgNetApi.Api: dotnet run.

### En caso de tener que regenerar el seeder
1. Abre la terminal en J:\Docker\net\sgNetApi\backend.
2. Borra la migración anterior o quítala con:
    dotnet ef migrations remove --project src/sgNetApi.Infrastructure --startup-project src/sgNetApi.Api
3. Crea la nueva migración corregida:
    dotnet ef migrations add MigracionInicialCorregida --project src/sgNetApi.Infrastructure --startup-project src/sgNetApi.Api
4. Aplica la migración a la base de datos:
    dotnet ef database update --project src/sgNetApi.Infrastructure --startup-project src/sgNetApi.Api
5. Ejecutar nuevamente la API
    cd src/sgNetApi.Api
    dotnet run

# Tokens JWT (JSON Web Tokens)

1. Instalar el paquete de JWT en las capas necesarias.
       - dotnet add src/sgNetApi.Api/sgNetApi.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
2. Crear las DTOs de solicitud y respuesta de Login en la capa Domain (o Application).
       - en el archivo .env
3. Definir e implementar el Servicio JWT para generar los tokens incorporando los Claims del usuario (CI, correo, roles y permisos).
       - en backend/src/sgNetApi.Domain/DTOs/AuthDtos.cs
4. Crear servicio de generación de Token JWT
       1. Interfaz en sgNetApi.Domain/Interfaces/IJwtTokenGenerator.cs
       2. Implementación en sgNetApi.Infrastructure/Services/JwtTokenGenerator.cs
5. Crear el AuthController en la API para exponer el endpoint de inicio de sesión.
       - Crea el archivo backend/src/sgNetApi.Api/Controllers/AuthController.cs
6.  Configurar la autenticación JWT en Program.cs.

7. Prueba
       - http://localhost:5283/swagger
       {
       "ci": 43791806,
       "password": "Admin.123456"
       }

# CRUD de usuarios
1. Crear los DTOs de Usuario en la capa Domain.
       - backend/src/sgNetApi.Domain/DTOs/UsuarioDtos.cs
2. Crear las políticas / servicios de negocio para crear, editar, listar y cambiar estados de usuario.
       - backend/src/sgNetApi.Api/Controllers/UsuariosController.cs
3. Crear el UsuariosController en la capa Api protegiendo sus endpoints con [Authorize] y validando permisos/roles.

# Validación granular de permisos
1. Crear el Requirement y el Handler de Autorización
       - backend/src/sgNetApi.Api/Authorization/PermissionAuthorization.cs
2. Registrar el PolicyProvidder y Handler en Program.cs
3. Aplicar los permisos en los UsuariosController.cs

# Reestablecer contraseña
1. Crear DTOs e Interfaz en sgNetApi.Domain
       - backend/src/sgNetApi.Api.Domain/DTOs/PasswordDtos.cs
       - backend/src/sgNetApi.Api.Domain/Interfaces/IPasswordService.cs
2. implementar la Lógica en sgNetApi.Infraestructure
       - backend/src/sgNetApi.Infrastructure/Services/PasswordService.cs
3. Registrar el servicio en Program.cs
4. Agregar los EndPoints en AuthController.cs

# Creacion de Test y Prueba UNitarias del PasswordService
1. Crear el proyecto xUnit dentro de backend/tests/sgNetApi.Tests
dotnet new xunit -o tests/sgNetApi.Tests
2. Agregar referencias a las capas Domain, Infrastructure y Api
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj reference src/sgNetApi.Domain/sgNetApi.Domain.csproj
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj reference src/sgNetApi.Infrastructure/sgNetApi.Infrastructure.csproj
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj reference src/sgNetApi.Api/sgNetApi.Api.csproj
3. Instalar paquetes de soporte para Mocks, Base de datos en Memoria y Aserciones expresivas
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj package NSubstitute
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj package FluentAssertions
dotnet add tests/sgNetApi.Tests/sgNetApi.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
4. Vincular el proyecto de pruebas a la solucion
dotnet sln add tests/sgNetApi.Tests/sgNetApi.Tests.csproj
5. Para ejecutar el motor de test desde backend/:
       - dotnet test --logger "console;verbosity=detailed"

# test para bloqueo de cuenta tras tres intentos de inicio de sesion
1. Crear la clase AuthControllerTests.cs

# Implementaremos un Middleware centralizado
1. Crear la Entidad AuditoriaLog en sgNetApi.Domain
       - backend/src/sgNetApi.Domain/Entities/AuditoriaLog.cs
2. Crear el Middleware AuditAndExceptionMiddleware
       - backend/src/sgNetApi.Api/Middlewares/AuditAndExceptionMiddleware.cs:
3. Registrar el Middleware en Program.cs

# Consultas de Auditoria
1. Crear los DTOs de Auditoría y Paginación
       - backend/src/sgNetApi.Domain/DTOs/AuditoriaDtos.cs
2. Crear el Controlador AuditoriaController
       - backend/src/sgNetApi.Api/Controllers/AuditoriaController.cs