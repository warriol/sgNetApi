# Especificación Formal del Dominio y Modelo de Datos - sgNet System

Este documento define la especificación formal del dominio, modelo relacional, reglas de negocio e infraestructura de seguridad para la plataforma **sgNet**. Sirve como fuente de verdad técnica para generadores de código y agentes de IA.

---

## 1. Módulo de Seguridad y Accesos (RBAC & CBAC)

El modelo de control de acceso combina roles con evaluación granular por claims (Claim-Based Access Control) y permisos directos por excepción.

### Entidades Principales

#### `Usuario`
Representa al funcionario o usuario administrativo autenticable en la plataforma.
* **`Ci`** (`long?`, PK/Unique, Nullable): Cédula de identidad uruguaya de **8 o 9 dígitos numéricos**, sin puntos ni guiones. Requerida para personas de nacionalidad uruguaya. El sistema conserva la cantidad de dígitos ingresada y no completa con ceros las cédulas de 8 dígitos.
* **`NombreUsuario`** (`string`, Varchar(50), Unique, Not Null): Credencial principal de acceso. Por regla de negocio:
  * Si `Nacionalidad` es **Uruguay**: debe coincidir exactamente con la representación textual de la `Ci` y puede tener 8 o 9 dígitos. Una CI de 8 dígitos permanece con 8 dígitos, sin agregar un cero inicial.
  * Si `Nacionalidad` es **Extranjera**: puede ser un código alfanumérico (ej. número de pasaporte o documento de identidad extranjero).
* **`Nombre`** (`string`, Varchar(100), Not Null): Nombre(s) del funcionario.
* **`Apellido`** (`string`, Varchar(100), Not Null): Apellido(s) del funcionario.
* **`Correo`** (`string`, Varchar(150), Unique, Not Null): Correo electrónico institucional.
* **`PasswordHash`** (`string`, Varchar(255), Not Null): Hash de contraseña (Argon2id / BCrypt).
* **`FechaNacimiento`** (`Date`, Not Null): Fecha de nacimiento del funcionario.
* **`Celular`** (`string?`, Varchar(20), Nullable): Teléfono celular de contacto.
* **`Telefono`** (`string?`, Varchar(20), Nullable): Teléfono fijo o de oficina.
* **`Habilitado`** (`bool`, Default: `true`): Estado del usuario (`true` = activo, `false` = bloqueado/inhabilitado).
* **`FechaCreacion`** (`DateTime`, UTC, Not Null, Default: `NOW()`): Timestamp de registro.
* **`IdNacionalidad`** (`int`, FK -> `Nacionalidad.IdNacionalidad`, Not Null): Referencia al país/nacionalidad.
* **`IdEstadoCivil`** (`int?`, FK -> `EstadoCivil.IdEstadoCivil`, Nullable): Referencia al estado civil.
* **`IdProfesion`** (`int?`, FK -> `Profesion.IdProfesion`, Nullable): Referencia a la profesión o título.
* **`IdGrado`** (`int?`, FK -> `Grado.IdGrado`, Nullable): Referencia al grado institucional.
* **`IdEscalafon`** (`int?`, FK -> `Escalafon.IdEscalafon`, Nullable): Referencia al escalafón.
* **`IdDependencia`** (`int?`, FK -> `Dependencia.IdDependencia`, Nullable): Referencia a la dependencia o unidad donde presta funciones.

> **Nota arquitectónica sobre `Usuario`:** La entidad `Usuario` guarda la relación directa con `IdDependencia`. Dado que cada `Dependencia` pertenece unívocamente a una sola `UnidadEjecutora`, la Unidad Ejecutora del usuario se deduce en backend mediante el grafo de navegación `Usuario.Dependencia.UnidadEjecutora`.

#### `Rol`
Agrupador funcional de permisos.
* **`IdRol`** (`int`, PK, Identity): Identificador secuencial.
* **`Nombre`** (`string`, Varchar(50), Unique, Not Null): Ej. `Administrador`, `Operador`, `Auditor`.
* **`Descripcion`** (`string?`, Varchar(255), Nullable): Explicación del propósito del rol.

#### `Permiso`
Acción granular o *capability* del sistema.
* **`IdPermiso`** (`int`, PK, Identity): Identificador secuencial.
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null): Clave de permiso estilo namespace (ej. `admin.usuarios.crear`, `catalogos.grado.editar`).
* **`Descripcion`** (`string`, Varchar(255), Not Null): Detalle explicativo de la operación.

#### Relaciones N:M (Tablas de Uniones)
* **`UsuarioRol`**:
  * `NombreUsuario` (`string`, FK -> `Usuario.NombreUsuario`, PK Compuesta)
  * `IdRol` (`int`, FK -> `Rol.IdRol`, PK Compuesta)
* **`RolPermiso`**:
  * `IdRol` (`int`, FK -> `Rol.IdRol`, PK Compuesta)
  * `IdPermiso` (`int`, FK -> `Permiso.IdPermiso`, PK Compuesta)
* **`UsuarioPermiso`**:
  * `NombreUsuario` (`string`, FK -> `Usuario.NombreUsuario`, PK Compuesta)
  * `IdPermiso` (`int`, FK -> `Permiso.IdPermiso`, PK Compuesta)

---

## 2. Módulo de Catálogos y Parámetros (`Catalogos.cs`)

Tablas maestras administradas exclusivamente por usuarios con el permiso `admin.catalogos.gestion`.

### `Nacionalidad`
* **`IdNacionalidad`** (`int`, PK, Identity): Identificador único.
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null): Nombre del país o gentilicio (ej. *Uruguaya*, *Argentina*, *Brasileña*).
* **`CodigoIso`** (`string`, Varchar(3), Unique, Not Null): Código ISO Alfa-2 o Alfa-3 (ej. *UY*, *AR*, *BR*).
* **`EsUruguaya`** (`bool`, Default: `false`): Flag para simplificar reglas de validación en frontend/backend (`true` para Uruguay).

### `EstadoCivil`
* **`IdEstadoCivil`** (`int`, PK, Identity): Identificador único.
* **`Nombre`** (`string`, Varchar(50), Unique, Not Null): Descripción (ej. *Soltero/a*, *Casado/a*, *Divorciado/a*, *Viudo/a*, *Unión Concubinaria*).

### `Profesion`
* **`IdProfesion`** (`int`, PK, Identity): Identificador único.
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null): Título o profesión (ej. *Abogado/a*, *Ingeniero/a en Sistemas*, *Administrativo/a*, *Contador/a*).

### `Grado`
* **`IdGrado`** (`int`, PK, Identity): Identificador único.
* **`Numero`** (`int`, Not Null): Orden de prelación/jerarquía (menor número = mayor jerarquía).
* **`Texto`** (`string`, Varchar(100), Not Null): Descripción formal (ej. *Comisario General*, *Oficial Principal*).
* **`Abreviatura`** (`string`, Varchar(20), Not Null): Sigla corta (ej. *Crio. Gral.*).

### `Escalafon`
* **`IdEscalafon`** (`int`, PK, Identity): Identificador único.
* **`Nombre`** (`string`, Varchar(100), Not Null): Nombre del escalafón (ej. *Ejecutivo*, *Técnico Profesional*).
* **`Abreviatura`** (`string`, Varchar(10), Not Null): Sigla identificadora (ej. *PE*, *PT*).

### `UnidadEjecutora` (UUEE)
Representa la unidad ejecutora principal (Relación 1 -> N con Dependencias).
* **`IdUuee`** (`int`, PK): Código formal de la Unidad Ejecutora.
* **`Nombre`** (`string`, Varchar(150), Not Null): Nombre institucional.
* **`Siglas`** (`string`, Varchar(20), Not Null): Identificador corto (ej. *DNIC*, *INR*, *JPU*).

### `Dependencia`
Unidad jerárquica u oficina dependiente de una Unidad Ejecutora específica.
* **`IdDependencia`** (`int`, PK, Identity): Identificador único.
* **`IdUuee`** (`int`, FK -> `UnidadEjecutora.IdUuee`, Not Null): Clave foránea que restringe la pertenencia a una única Unidad Ejecutora.
* **`Nombre`** (`string`, Varchar(150), Not Null): Nombre de la dependencia.
* **`Siglas`** (`string`, Varchar(20), Not Null): Sigla de la dependencia.

---

## 3. Módulo de Auditoría HTTP (`AuditoriaLog`)

Almacena de forma inmutable todas las transacciones realizadas sobre la API.

* **`IdLog`** (`long`, PK, Identity BigInt): Identificador único secuencial.
* **`Fecha`** (`DateTime`, UTC, Not Null): Timestamp de la transacción.
* **`UsuarioNombreUsuario`** (`string?`, Varchar(50), Nullable): Identificador del usuario que ejecutó la acción (obtenida del claim JWT `sub`/`nameid`). `NULL` para solicitudes anónimas.
* **`IpOrigen`** (`string`, Varchar(45), Not Null): Dirección IPv4 o IPv6 del cliente.
* **`MetodoHttp`** (`string`, Varchar(10), Not Null): `GET`, `POST`, `PUT`, `DELETE`, `PATCH`.
* **`Ruta`** (`string`, Varchar(500), Not Null): Endpoint consultado.
* **`CodigoEstado`** (`int`, Not Null): Código de respuesta HTTP.
* **`DuracionMs`** (`long`, Not Null): Tiempo de ejecución del middleware en milisegundos.
* **`PayloadRequest`** (`string?`, Text/JSON, Nullable): Cuerpo de la petición (sanitizando campos sensibles).

---

## 4. Reglas de Negocio e Integridad de Datos

### 4.1 Identificación y Formato de Usuario (`Ci` / `NombreUsuario`)
1. **Nacionalidad Uruguaya (`EsUruguaya = true`)**:
  * La **`Ci`** es **obligatoria** y debe consistir en un número de **8 o 9 dígitos**.
  * El campo **`NombreUsuario`** se auto-completa e iguala obligatoriamente al valor textual de la **`Ci`**, conservando 8 dígitos cuando la CI ingresada tenga 8 y 9 dígitos cuando tenga 9. No se agrega un cero inicial automáticamente.
2. **Nacionalidad Extranjera (`EsUruguaya = false`)**:
   * La **`Ci`** es **opcional/null**.
   * El **`NombreUsuario`** se vuelve **editable manualmente** y permite caracteres alfanuméricos para ingresar el número de pasaporte o documento de identidad extranjero.
3. **Formulario Angular 19**:
   * Al cambiar la `Nacionalidad` seleccionada:
    * Si es Uruguay $\rightarrow$ Se habilita/exige la `Ci` (8 o 9 dígitos), se bloquea `NombreUsuario` y se igualan dinámicamente sin completar ceros.
     * Si es Extranjera $\rightarrow$ Se oculta o flexibiliza el validador de `Ci` y se habilita el campo `NombreUsuario` como texto alfanumérico.

### 4.2 Relación Jerárquica UUEE <-> Dependencia en Formularios (CRUD Usuarios)
1. **Modelado Relacional Strict:**
   * Una **Unidad Ejecutora (`UnidadEjecutora`)** posee 1 o N **Dependencias (`Dependencia`)**.
   * Una **Dependencia** pertenece obligatoriamente a una y solo una **Unidad Ejecutora** (`IdUuee` Not Null).
2. **Comportamiento en Frontend (Angular 19):**
   * El selector de **Dependencia** permanece **deshabilitado (`disabled`)** hasta que se elija una **Unidad Ejecutora (`IdUuee`)**.
   * Al cambiar la `UnidadEjecutora`, se borra la dependencia seleccionada previamente y se cargan las asociadas a la nueva UUEE.

### 4.3 Evaluación de Permisos Efectivos
$$\text{PermisosEfectivos}(u) = \left( \bigcup_{r \in \text{Roles}(u)} \text{Permisos}(r) \right) \cup \text{PermisosDirectos}(u)$$

### 4.4 Inmutabilidad y Restricciones
* `AuditoriaLog` no posee endpoints de actualización (`PUT/PATCH`) ni eliminación (`DELETE`).
* No se permite la eliminación física de un `Usuario` con registros vinculados en auditoría; utilizar borrado lógico vía `Habilitado = false`.
* En la edición de un `Usuario`, `NombreUsuario` y `Ci` son inmutables. El modal de modificación debe mostrarlos como solo lectura y no enviarlos en el DTO de actualización.