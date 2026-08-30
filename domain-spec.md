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
* **`IdDireccion`** (`int?`, FK -> `Direccion.IdDireccion`, Nullable hasta completar la ficha): Dirección exclusiva de la dependencia.
* **`NombreUsuarioJefe`** (`string?`, FK -> `Usuario.NombreUsuario`, Nullable hasta completar la ficha): Usuario que ocupa la jefatura.
* **`NombreUsuarioSegundoJefe`** (`string?`, FK -> `Usuario.NombreUsuario`, Nullable hasta completar la ficha): Usuario que ocupa la segunda jefatura.
* **`IdTurno`** (`int?`, FK -> `Turno.IdTurno`, Nullable hasta asignar un turno): Turno operativo de la dependencia.

Una Dependencia mantiene una lista de funcionarios mediante la relación 1:N inversa de `Usuario.IdDependencia`. La pertenencia efectiva de un usuario a una dependencia se restringe a una sola dependencia.

### `Direccion`
Representa la dirección exclusiva asociada a una dependencia. La estructura admite distintos formatos de captura según `TipoDireccion`.
* **`IdDireccion`** (`int`, PK, Identity): Identificador único.
* **`TipoDireccion`** (`int`, Not Null): Tipo de dirección permitido: `1` punto en mapa, `2` ruta y kilómetro, `3` calle y número, `4` calle y esquina, `5` calle, manzana y solar.
* **`Pais`** (`string`, Varchar(100), Not Null).
* **`Departamento`** (`string`, Varchar(100), Not Null).
* **`Localidad`** (`string`, Varchar(100), Not Null).
* **`Calle`** (`string?`, Varchar(150), Nullable según el tipo).
* **`Cruce1`** (`string?`, Varchar(150), Nullable): Primera calle de cruce.
* **`Cruce2`** (`string?`, Varchar(150), Nullable): Segunda calle de cruce.
* **`Numero`** (`string?`, Varchar(20), Nullable): Número de puerta.
* **`Apartamento`** (`string?`, Varchar(20), Nullable).
* **`Manzana`** (`string?`, Varchar(30), Nullable).
* **`Solar`** (`string?`, Varchar(30), Nullable).
* **`Ruta`** (`string?`, Varchar(100), Nullable).
* **`Km`** (`decimal?`, Nullable): Kilómetro de la ruta.
* **`Latitud`** (`decimal?`, Nullable): Latitud del punto geográfico.
* **`Longitud`** (`decimal?`, Nullable): Longitud del punto geográfico.
* **`CodigoPostal`** (`string?`, Varchar(20), Nullable).
* **`Telefono`** (`string?`, Varchar(20), Nullable).

Cuando `TipoDireccion = 1`, el usuario selecciona un punto en el mapa y se guardan `Latitud` y `Longitud`. El backend podrá consultar una API auxiliar de geocodificación para completar o sugerir país, departamento, localidad, calle, código postal y demás datos disponibles. La API auxiliar debe estar encapsulada detrás de una interfaz y sus errores no deben impedir guardar las coordenadas válidas.

### `Turno`
Catálogo de turnos y horarios disponibles para las dependencias.
* **`IdTurno`** (`int`, PK, Identity): Identificador único.
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null): Nombre del turno.
* **`TipoTurno`** (`string`, Varchar(20), Not Null): Variante operativa del turno (`4x6`, `3x8`, `2x12`, `1x24`).
* **`HoraInicio`** (`TimeOnly`, Not Null): Hora de inicio.
* **`HoraFin`** (`TimeOnly`, Not Null): Hora de finalización calculada automáticamente por el sistema.
* **`Descripcion`** (`string?`, Varchar(255), Nullable).
* **`Habilitado`** (`bool`, Default: `true`): Permite retirar un turno del selector sin eliminarlo físicamente.

Regla operativa del sistema: la hora de fin no se ingresa manualmente por el usuario. Se deriva a partir de `TipoTurno` + `HoraInicio`, con la duración del turno en minutos y el ajuste de cierre en el caso de que el turno cruce medianoche. Por ejemplo: un turno `3x8` que inicia a las `21:00` finaliza a las `04:59` del día siguiente; un turno `2x12` que inicia a las `08:00` finaliza a las `19:59`.

### Relaciones del módulo de Dependencias
* `Usuario.IdDependencia` referencia a `Dependencia.IdDependencia`; una dependencia puede tener muchos funcionarios y un usuario solo uno.
* `Dependencia.IdDireccion` referencia como máximo una dirección exclusiva.
* `Dependencia.NombreUsuarioJefe` y `Dependencia.NombreUsuarioSegundoJefe` referencian usuarios del sistema.
* `Dependencia.IdTurno` referencia un turno previamente creado.

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

### 4.5 Gestión de Dependencias
1. Cada dependencia pertenece obligatoriamente a una y solo una `UnidadEjecutora` mediante `IdUuee`.
2. Una dependencia debe tener al menos dos funcionarios asignados antes de poder completar o confirmar su ficha operativa.
3. Los funcionarios de una dependencia se seleccionan de la lista general de usuarios del sistema.
4. Un usuario solo puede pertenecer a una dependencia. El backend debe rechazar una asignación si el usuario ya pertenece a otra dependencia.
5. El jefe y el segundo jefe deben seleccionarse exclusivamente entre los funcionarios asignados a la misma dependencia.
6. `NombreUsuarioJefe` y `NombreUsuarioSegundoJefe` son obligatorios para completar la ficha, deben ser usuarios diferentes y no pueden ser nulos en una dependencia completa.
7. Un mismo usuario puede ser jefe de una o varias dependencias, siempre que pertenezca como funcionario a cada una de ellas.
8. No se permite eliminar físicamente una dependencia que tenga usuarios, dirección o historial relacionado. Debe utilizarse una estrategia de baja lógica si el ciclo de vida lo requiere.

### 4.6 Gestión de Direcciones
1. `TipoDireccion` es obligatorio y solo admite los valores `1` a `5`.
2. Para el tipo `1` son obligatorias `Latitud` y `Longitud`; el resto de datos puede ser completado mediante la API auxiliar.
3. Para el tipo `2` son obligatorios `Ruta` y `Km`.
4. Para el tipo `3` son obligatorios `Calle` y `Numero`.
5. Para el tipo `4` son obligatorios `Calle`, `Cruce1` y `Cruce2`.
6. Para el tipo `5` son obligatorios `Calle`, `Manzana` y `Solar`.
7. Los campos comunes `Pais`, `Departamento`, `Localidad` y `Telefono` deben validarse según las reglas institucionales; no deben guardarse valores incompatibles con el tipo seleccionado.
8. Una dirección pertenece a una sola dependencia y no debe reutilizarse entre dependencias.

### 4.7 Gestión de Turnos y Horarios
1. Los turnos se crean y administran previamente desde el módulo de Turnos.
2. Una dependencia no puede asignar un turno inexistente o deshabilitado.
3. No se puede asignar un turno a una dependencia hasta que exista al menos un turno habilitado en el catálogo.
4. `TipoTurno` es obligatorio y solo admite los valores `4x6`, `3x8`, `2x12` y `1x24`.
5. `HoraInicio` es obligatoria. `HoraFin` se calcula automáticamente a partir del tipo y la hora de inicio; no se debe solicitar ni guardar manualmente como dato de entrada.
6. La duración efectiva del turno se calcula con la regla operativa del sistema: `4x6 = 6 horas`, `3x8 = 8 horas`, `2x12 = 12 horas`, `1x24 = 24 horas`, y el cierre se calcula descontando un minuto del intervalo final para mantener continuidad operativa (ej.: `08:00` + `12 horas` = `19:59`).
7. Si el turno cruza la medianoche, el sistema debe mostrar que el cierre puede corresponder al día siguiente; la UI debe indicar este comportamiento para evitar confusión al operador.
8. Un turno utilizado por una dependencia no debe eliminarse físicamente; debe deshabilitarse.

### 4.8 Flujo de completitud de una Dependencia
Una dependencia se considera `Incompleta` mientras no cumpla todas las condiciones siguientes:
* nombre, siglas e `IdUuee` válidos;
* al menos dos funcionarios asignados;
* jefe y segundo jefe asignados, diferentes y pertenecientes a la dependencia;
* dirección creada y validada según su tipo;
* turno habilitado asignado.

El frontend debe mostrar el estado de completitud y bloquear la confirmación final mientras falte alguna condición. El backend debe repetir estas validaciones y no confiar únicamente en las restricciones de la interfaz.

### 4.9 Mensajes de validación del módulo
Los errores de negocio deben responderse con `400 Bad Request` y un campo `mensaje` claro. Como mínimo se contemplan:
* `Debe seleccionar una Unidad Ejecutora.`
* `La Unidad Ejecutora seleccionada no existe.`
* `La dependencia debe tener al menos dos funcionarios asignados.`
* `El jefe y el segundo jefe deben ser diferentes.`
* `El jefe seleccionado debe pertenecer a la dependencia.`
* `El usuario ya pertenece a otra dependencia.`
* `Debe seleccionar un tipo de dirección válido.`
* `Complete los campos obligatorios para el tipo de dirección seleccionado.`
* `Debe crear y habilitar al menos un turno antes de asignarlo.`
* `El turno seleccionado no existe o está deshabilitado.`

Los conflictos de unicidad o referencias existentes deben responderse con `409 Conflict` y un mensaje que indique el dato que debe corregirse. Las excepciones inesperadas deben registrarse en logs y devolver un mensaje general sin exponer detalles internos.

### 4.10 Endpoints previstos para los nuevos módulos
Los siguientes endpoints forman parte del contrato previsto para la implementación:
* `GET/POST/PUT/DELETE /api/Dependencias`: CRUD de dependencias.
* `GET/POST/PUT/DELETE /api/Direcciones`: CRUD de direcciones asociadas a dependencias.
* `GET/POST/PUT/DELETE /api/Turnos`: CRUD del catálogo de turnos.
* `PUT /api/Dependencias/{idDependencia}/funcionarios`: Reemplaza la lista de funcionarios, validando pertenencia única.
* `PUT /api/Dependencias/{idDependencia}/jefaturas`: Asigna jefe y segundo jefe con validación de pertenencia y diferencia.
* `PUT /api/Dependencias/{idDependencia}/turno`: Asigna un turno habilitado.
* `GET /api/Dependencias/{idDependencia}/completitud`: Devuelve las condiciones cumplidas y pendientes.

Todos los endpoints de escritura de estos módulos requieren autenticación y el permiso `admin.dependencias.gestion`. La consulta de catálogos auxiliares para construir formularios puede tener permisos de lectura separados.

---

## 5. Módulo de Dependencias, Turnos y Escalafón

Este módulo cubre la administración operativa de cada dependencia, la definición del patrón de turno institucional y la asignación del turno por funcionario dentro del escalafón.

### 5.1 Reglas operativas de turno

La configuración del turno se basa en el patrón de división del día de una dependencia, con estas opciones válidas:

* `4x6`: cuatro turnos de 6 horas.
* `3x8`: tres turnos de 8 horas.
* `2x12`: dos turnos de 12 horas.
* `1x24`: un turno de 24 horas.

Cada registro de `Turno` debe informar:

* `Nombre`: nombre explícito y descriptivo para ser seleccionado luego en la dependencia, por ejemplo `Tarde 18:00-00:00` o `Turno 24 hs`.
* `TipoTurno`: valor del patrón operativo (`4x6`, `3x8`, `2x12`, `1x24`).
* `HoraInicio` y `HoraFin`: horario de inicio y fin del servicio.
* `Descripcion`: texto auxiliar para situación operativa o validación institucional.
* `Habilitado`: activo/inactivo para no eliminar registros en uso.

### 5.2 Relación dependencia - turno

Una dependencia puede tener asignado un `IdTurno` único que representa la forma en que se organiza su operación diaria. La dependencia debe elegir el turno que corresponde al esquema operativo del servicio. Si el turno no existe o está deshabilitado, el backend deben rechazar la asignación con `400 Bad Request`.

### 5.3 Regla de escalafón y turno del funcionario

Cuando un funcionario es asignado a una dependencia, su turno inicial debe quedar en estado de `No asignado`.

Luego, desde el módulo de escalafón se puede definir su turno específico de acuerdo con el patrón operativo de la dependencia:

* Si la dependencia usa `1x24`, el funcionario tendrá el valor `Turno único`.
* Si la dependencia usa `4x6`, `3x8` o `2x12`, el funcionario puede quedar en `Turno 1`, `Turno 2`, `Turno 3` o `Turno 4` según el esquema definido.

La entidad `Usuario` incorpora la relación:

* `IdTurnoAsignado`: FK a `Turno`.
* `TurnoAsignado`: navegación para cargar el turno asociado.

El flujo real esperado es:

1. Crear y habilitar el turno de la dependencia.
2. Asignar la dependencia al funcionario.
3. Dejar el turno del funcionario en `No asignado` por defecto.
4. Modificar el turno del funcionario desde escalafón según la operación de la dependencia.

### 5.4 Gestión de dependencias

La gestión de dependencias contempla los siguientes submenús del frontend:

* `Gestionar dependencia`
  * alta, edición, baja lógica y validación de completitud.
* `Gestionar turnos`
  * administración del catálogo, tipo, horarios de inicio/fin y estado habilitado.
* `Gestionar escalafón`
  * asignación de funcionarios por dependencia y turnos de cada funcionario.
* `Gestionar indumentaria`
  * inventario y asignación de chalecos, armas y esposas.

### 5.5 Flujos de front-end

El frontend debe exponer la estructura de navegación:

* `Administración > Dependencia`
* `Administración > Gestionar Turnos`
* `Administración > Gestionar Escalafón`
* `Administración > Gestionar Indumentaria`

La lógica operativa debe mostrar:

* tipo de turno operativo de la dependencia;
* nombre explicativo del turno;
* patrón de división del día;
* selector de turno individual para cada funcionario;
* revisión e inventario de equipo asignado.

### 5.6 Endpoints del módulo de dependencias, turnos y escalafón

* `GET /api/Dependencias`
* `POST /api/Dependencias`
* `PUT /api/Dependencias/{id}`
* `DELETE /api/Dependencias/{id}`
* `PUT /api/Dependencias/{idDependencia}/funcionarios`
* `PUT /api/Dependencias/{idDependencia}/jefaturas`
* `PUT /api/Dependencias/{idDependencia}/turno`
* `GET /api/Dependencias/{idDependencia}/completitud`
* `GET /api/Turnos`
* `POST /api/Turnos`
* `PUT /api/Turnos/{id}`
* `DELETE /api/Turnos/{id}`
* `PATCH /api/Usuarios/{ci}/turno`

Todos los endpoints de escritura requieren autenticación y permiso `admin.dependencias.gestion`.

---

## 6. Módulo de Armamento, Chalecos Antibalas y Esposas

Este módulo administra el inventario y la asignación de equipamiento policial individual a cada funcionario. La intención es mantener una trazabilidad precisa del material entregado, su estado y su uso operativo.

### 5.1 `armaPolicial`

Representa cada arma policial registrada en el inventario.

* **`IdArmaPolicial`** (`int`, PK, Identity): Identificador único del arma.
* **`IdArmaTipo`** (`int`, FK -> `ArmaTipo.IdArmaTipo`, Not Null): Tipo del arma.
* **`IdArmaMarca`** (`int`, FK -> `ArmaMarca.IdArmaMarca`, Not Null): Marca del arma.
* **`IdArmaModelo`** (`int`, FK -> `ArmaModelo.IdArmaModelo`, Not Null): Modelo del arma.
* **`Serie`** (`string`, Varchar(100), Nullable): Número de serie del arma, cuando aplica.
* **`Numero`** (`string`, Varchar(50), Nullable): Número interno o legajo del arma según la normativa institucional.
* **`FechaEntrega`** (`DateTime?`, Nullable): Fecha en la que se entregó el arma al servicio o al funcionario.
* **`IdArmaEstado`** (`int`, FK -> `ArmaEstado.IdArmaEstado`, Not Null): Estado actual del arma.
* **`Observaciones`** (`string?`, Text, Nullable): Comentarios operativos, incidencias, mantenimiento, etc.

### Catálogos asociados a `armaPolicial`

#### `ArmaTipo`
* **`IdArmaTipo`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `ArmaMarca`
* **`IdArmaMarca`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `ArmaModelo`
* **`IdArmaModelo`** (`int`, PK, Identity)
* **`IdArmaTipo`** (`int`, FK -> `ArmaTipo.IdArmaTipo`, Not Null)
* **`IdArmaMarca`** (`int`, FK -> `ArmaMarca.IdArmaMarca`, Not Null)
* **`Nombre`** (`string`, Varchar(100), Not Null)

#### `ArmaEstado`
* **`IdArmaEstado`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(50), Unique, Not Null): Ej. `Disponible`, `Asignada`, `En mantenimiento`, `Baja`, `Perdida`.

### 5.2 `chalecoAntibalaPolicial`

Representa cada chaleco antibala registrado en el inventario.

* **`IdChalecoAntibalaPolicial`** (`int`, PK, Identity): Identificador único del chaleco.
* **`IdChalecoTipo`** (`int`, FK -> `ChalecoTipo.IdChalecoTipo`, Not Null): Tipo del chaleco.
* **`IdChalecoMarca`** (`int`, FK -> `ChalecoMarca.IdChalecoMarca`, Not Null): Marca del chaleco.
* **`IdChalecoModelo`** (`int`, FK -> `ChalecoModelo.IdChalecoModelo`, Not Null): Modelo del chaleco.
* **`IdChalecoTalle`** (`int`, FK -> `ChalecoTalle.IdChalecoTalle`, Not Null): Talle del chaleco.
* **`Color`** (`string`, Varchar(50), Nullable): Color distintivo del chaleco.
* **`FechaEntrega`** (`DateTime?`, Nullable): Fecha de entrega del chaleco.
* **`FechaVencimiento`** (`DateTime?`, Nullable): Fecha de vencimiento o revisión del chaleco.
* **`IdChalecoEstado`** (`int`, FK -> `ChalecoEstado.IdChalecoEstado`, Not Null): Estado del chaleco.
* **`Observaciones`** (`string?`, Text, Nullable): Comentarios adicionales.

### Catálogos asociados a `chalecoAntibalaPolicial`

#### `ChalecoTipo`
* **`IdChalecoTipo`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `ChalecoMarca`
* **`IdChalecoMarca`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `ChalecoModelo`
* **`IdChalecoModelo`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `ChalecoTalle`
* **`IdChalecoTalle`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(20), Unique, Not Null): Ej. `S`, `M`, `L`, `XL`, `XXL`.

#### `ChalecoEstado`
* **`IdChalecoEstado`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(50), Unique, Not Null): Ej. `Disponible`, `Asignado`, `Vencido`, `En revisión`.

### 5.3 `esposasPolicial`

Representa cada juego de esposas del inventario.

* **`IdEsposasPolicial`** (`int`, PK, Identity): Identificador único de las esposas.
* **`IdEsposasTipo`** (`int`, FK -> `EsposasTipo.IdEsposasTipo`, Not Null): Tipo de esposas.
* **`IdEsposasMarca`** (`int`, FK -> `EsposasMarca.IdEsposasMarca`, Not Null): Marca de las esposas.
* **`IdEsposasModelo`** (`int`, FK -> `EsposasModelo.IdEsposasModelo`, Not Null): Modelo de las esposas.
* **`FechaEntrega`** (`DateTime?`, Nullable): Fecha de entrega.
* **`Observaciones`** (`string?`, Text, Nullable): Comentarios operativos o observaciones del equipo.

### Catálogos asociados a `esposasPolicial`

#### `EsposasTipo`
* **`IdEsposasTipo`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `EsposasMarca`
* **`IdEsposasMarca`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

#### `EsposasModelo`
* **`IdEsposasModelo`** (`int`, PK, Identity)
* **`Nombre`** (`string`, Varchar(100), Unique, Not Null)

### 5.4 Módulo de asignación por funcionario

Existe un módulo de asignación donde a cada funcionario se le asigna un arma, un chaleco antibala y un juego de esposas.

#### `FuncionarioEquipoPolicial`
* **`IdFuncionarioEquipoPolicial`** (`int`, PK, Identity): Identificador de la asignación.
* **`NombreUsuario`** (`string`, FK -> `Usuario.NombreUsuario`, Not Null): Funcionario al que se le asigna el equipo.
* **`IdArmaPolicial`** (`int`, FK -> `armaPolicial.IdArmaPolicial`, Not Null): Arma asignada al funcionario.
* **`IdChalecoAntibalaPolicial`** (`int`, FK -> `chalecoAntibalaPolicial.IdChalecoAntibalaPolicial`, Not Null): Chaleco asignado.
* **`IdEsposasPolicial`** (`int`, FK -> `esposasPolicial.IdEsposasPolicial`, Not Null): Esposas asignadas.
* **`FechaAsignacion`** (`DateTime`, UTC, Not Null): Fecha de asignación del equipo.
* **`FechaDevolucion`** (`DateTime?`, Nullable): Fecha de devolución o baja de la asignación.
* **`Observaciones`** (`string?`, Text, Nullable): Comentarios del estado operativo o devoluciones.

### Reglas de negocio del módulo

1. Un funcionario puede tener una sola asignación activa de arma, chaleco y esposas al mismo tiempo.
2. Un arma, un chaleco o un juego de esposas no pueden estar asignados simultáneamente a más de un funcionario activo.
3. La asignación debe registrarse con `FechaAsignacion` y debe validar que el equipo esté en estado disponible o apto para uso.
4. Si un equipo se devuelve, se debe registrar la `FechaDevolucion` y volver al stock disponible.
5. La baja o reemplazo de un elemento debe dejar trazabilidad histórica para auditoría y control institucional.
6. La asignación se considera parte del registro de equipamiento del funcionario y debe poder consultarse desde la ficha del usuario.

### Endpoints previstos para el módulo de asignación

Los endpoints relacionados con este módulo forman parte del contrato funcional del sistema:

* `GET/POST/PUT/DELETE /api/FuncionarioEquipoPolicial`: CRUD de asignaciones por funcionario.
* `GET /api/Funcionarios/{NombreUsuario}/equipo-policial`: Consulta el equipo asignado a un funcionario.
* `PUT /api/FuncionarioEquipoPolicial/{id}/devolucion`: Registra la devolución del equipo.

Todos los endpoints de escritura de este módulo requieren autenticación y permiso de administración del inventario o del personal operativo, según la política institucional.

---

