Hola. Vamos a iniciar desde cero el desarrollo del proyecto **sgNet**, un sistema integral para la gestión de usuarios, roles, auditoría y catálogos institucionales, desarrollado en **.NET 9 (Web API)** para el backend y **Angular 19 (Standalone Components y Bootstrap 5)** para el frontend.

### Contexto y Lo Realizado Hasta Ahora
En la etapa previa exploramos y validamos las siguientes funcionalidades clave:
1. **Backend (.NET 9):**
   - Arquitectura en capas limpia (`Domain`, `Application`, `Infrastructure`, `Api`).
   - Autenticación JWT y sistema de permisos RBAC flexibilizado (Roles + Permisos Directos).
   - Middleware global para auditoría HTTP (`AuditoriaLog`).
   - Módulo de parámetros/catálogos institucionales (`Catalogos.cs`: Grados, Escalafones, Unidades Ejecutoras y Dependencias).

2. **Frontend (Angular 19 + Bootstrap 5):**
   - Interfaz nativa responsive con navegación horizontal por roles/desplegables.
   - Vistas completas para Usuarios, Roles y Logs de Auditoría.
   - Formularios modales estilizados con Bootstrap 5 y selectores enlazados (Unidad Ejecutora -> Dependencia).

---

### Objetivos y Enfoque de esta Nueva Conversación
En esta primera etapa de reinicio, **NO generaremos código de la aplicación todavía**. Nos enfocaremos exclusivamente en sentar los cimientos, la arquitectura, la documentación y la infraestructura de contenedores.

Trabajaremos en definir e implementar:
1. **`domain-spec.md`**: Especificación formal del dominio, entidades (`Usuario`, `Rol`, `Permiso`, `Catalogos`, `AuditoriaLog`), reglas de negocio y relaciones de base de datos.
2. **Estructura de Archivos del Proyecto**: Definición del árbol de carpetas limpio tanto para la API (.NET) como para la SPA (Angular), sin escribir aún el código interno de los componentes o controladores.
3. **`README.md`**: Documentación principal del repositorio, instrucciones de inicialización, arquitectura y guía de uso.
4. **`docker-compose.yml` e Infraestructura Multi-Servidor**:
   - Servicio para la **API de .NET**.
   - Servicio para la **Base de Datos PostgreSQL** (o SQL Server).
   - Servicio para la **Aplicación Web Angular**.
   - **Nuevo Servicio AI Local:** Configuración de un servidor local (mediante Ollama o contenedor dedicado) que corra el modelo **Gemma 4** (o Gemma 2 / Llama 3) para actuar como asistente IA local con contexto completo del proyecto.

### Base del archivo domain-spec.md en la que trabajar
# Especificación del Dominio y Modelo de Datos - sgNet System

Este documento detalla la estructura del dominio, entidades de catálogo, usuarios, seguridad e historial de auditoría de la plataforma **sgNet**.

---

## 1. Módulo de Seguridad y Accesos (RBAC)

El modelo de control de acceso se basa en roles y permisos granulares por Claims (Claim-Based Access Control / RBAC).

### Entidades Principales

#### `Usuario`
Representa al funcionario o usuario con credenciales de acceso al sistema.
* **`Ci`** (`long`, PK): Cédula de identidad (sin puntos ni guiones).
* **`NombreUsuario`** (`string`, Único): Identificador para inicio de sesión.
* **`Nombre`** (`string`): Nombre(s) del funcionario.
* **`Apellido`** (`string`): Apellido(s) del funcionario.
* **`Correo`** (`string`): Correo institucional.
* **`PasswordHash`** (`string`): Hash de la contraseña.
* **`Habilitado`** (`bool`): Estado del usuario (`true` = activo, `false` = bloqueado).
* **`IdGrado`** (`int?`, FK -> `Grado.IdGrado`): Referencia al grado policial/administrativo.
* **`IdEscalafon`** (`int?`, FK -> `Escalafon.IdEscalafon`): Referencia al escalafón.
* **`IdDependencia`** (`int?`, FK -> `Dependencia.IdDependencia`): Referencia a la dependencia o unidad donde presta funciones.

#### `Rol`
Agrupador funcional de permisos del sistema.
* **`IdRol`** (`int`, PK): Identificador secuencial.
* **`Nombre`** (`string`): Ej. `Administrador`, `Operador`, `Auditor`.

#### `Permiso`
Acción granular o capability del sistema.
* **`IdPermiso`** (`int`, PK): Identificador secuencial.
* **`Nombre`** (`string`): Clave única del permiso (ej. `admin.usuarios.crear`).
* **`Descripcion`** (`string`): Detalle explicativo de la acción.

#### Tablas de Relación (Muchos a Muchos)
* **`UsuarioRol`**: Asocia `Ci` con `IdRol`.
* **`RolPermiso`**: Asocia `IdRol` con `IdPermiso`.
* **`UsuarioPermiso`**: Asocia `Ci` con `IdPermiso` (Permisos directos/excepcionales asignados a un usuario sin pasar por un rol).

---

## 2. Módulo de Parámetros y Catálogos (`Catalogos.cs`)

Tablas maestras administradas exclusivamente por usuarios con rol `Administrador`.

### `Grado`
* **`IdGrado`** (`int`, PK): Identificador único.
* **`Numero`** (`int`): Orden de prelación o jerarquía del grado.
* **`Texto`** (`string`): Descripción formal (ej. *Comisario General*, *Oficial Principal*).
* **`Abreviatura`** (`string`): Sigla corta (ej. *Crio. Gral.*, *Of. Ppal.*).

### `Escalafon`
* **`IdEscalafon`** (`int`, PK): Identificador único.
* **`Nombre`** (`string`): Nombre completo del escalafón (ej. *Ejecutivo*, *Técnico Profesional*).
* **`Abreviatura`** (`string`): Sigla identificadora (ej. *PE*, *PT*).

### `UnidadEjecutora`
* **`IdUuee`** (`int`, PK): Código de la Unidad Ejecutora.
* **`Nombre`** (`string`): Nombre institucional completo.
* **`Siglas`** (`string`): Identificador corto de la UE (ej. *DNIC*, *INR*, *JPU*).

### `Dependencia`
Unidad jerárquica o comisaría/oficina dependiente de una Unidad Ejecutora.
* **`IdDependencia`** (`int`, PK): Identificador único.
* **`IdUuee`** (`int`, FK -> `UnidadEjecutora.IdUuee`): Clave foránea que agrupa las dependencias por Unidad Ejecutora.
* **`Nombre`** (`string`): Nombre formal de la dependencia.
* **`Siglas`** (`string`): Sigla de la dependencia.

---

## 3. Módulo de Auditoría HTTP (`AuditoriaLog`)

Registra de forma inmutable todas las transacciones y peticiones realizadas sobre los endpoints de la API.

* **`IdLog`** (`long`, PK): Identificador único secuencial.
* **`Fecha`** (`DateTime`): Timestamp UTC de la petición.
* **`UsuarioCi`** (`long?`): Cédula del usuario que ejecutó la acción (extraído del JWT). Null en solicitudes anónimas.
* **`IpOrigen`** (`string`): Dirección IP del cliente.
* **`MetodoHttp`** (`string`): `GET`, `POST`, `PUT`, `DELETE`, `PATCH`.
* **`Ruta`** (`string`): Path relativo del endpoint (ej. `/api/Usuarios`).
* **`CodigoEstado`** (`int`): Código de respuesta HTTP (ej. `200`, `401`, `403`, `500`).
* **`DuracionMs`** (`long`): Tiempo de procesamiento en milisegundos.

---

## 4. Reglas de Negocio y Restricciones de Integridad

1. **Jerarquía en Selectores Enlazados**:
   * Al crear o editar un usuario, la selección de **Dependencia** debe filtrarse obligatoriamente según la **Unidad Ejecutora (`IdUuee`)** seleccionada previamente.
2. **Asignación Flexibilizada de Permisos**:
   * Los permisos finales de un usuario son la unión de: `Permisos del Rol` $\cup$ `Permisos Directos`.
3. **Inmutabilidad de Auditoría**:
   * Los registros de la tabla `AuditoriaLog` no se pueden modificar ni eliminar a través de la API.
4. **Protección de Datos Institucionales**:
   * Los valores de `Grado`, `Escalafon`, `UnidadEjecutora` y `Dependencia` son opcionales (`nullable`) a nivel de entidad para soportar usuarios de prueba o administradores del sistema, pero requeridos según el perfil operativo.

Por favor, confirma que estás listo para comenzar paso a paso con la creación del `domain-spec.md` y la arquitectura inicial.