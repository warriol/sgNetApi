## Especificacion del modulo de ingreso de denuncias y partes

### Arquitectura

El modulo usa PostgreSQL mediante Npgsql. La estructura persistente se encuentra en
`database/sql/ingreso_denuncias.sql` y mantiene las tablas de seguridad y catalogos
existentes (`Usuarios`, `UnidadesEjecutoras`, `Dependencias`) como referencias externas.
Las tablas propias del modulo usan nombres `snake_case`.

El modelo separa `denuncia` de sus agregados: ubicacion, tipificacion, intervinientes,
narracion, equipo de trabajo y fichas complementarias. Las colecciones 1:N tienen tabla
propia. Se usa `jsonb` solo para estructuras variables o parametrizables del contrato,
como atributos de objetos, datos de integracion y catalogos extendidos.

La matriz `regla_tipificacion_ficha` determina visibilidad y cardinalidad minima de cada
ficha por clasificacion. La API debe devolver esas reglas al frontend y repetir la
validacion al guardar; la interfaz nunca es la unica barrera de integridad.

### Modelo y flujo frontend

`IngresoComponent` implementa un wizard de seis pasos: clasificacion, ubicacion,
intervinientes, fichas complementarias, narracion y revision. `IngresoService` contiene
una matriz local inicial con el mismo contrato de `regla_tipificacion_ficha`; debe
reemplazarse por una consulta HTTP cuando exista el endpoint de parametros.

Las fichas `Intervinientes`, `Armas`, `Vehiculos`, `Objetos`, `Transito` y `Multimedias`
se muestran segun la tipificacion. Las fichas obligatorias exhiben la cardinalidad
minima y disponen de grilla con modal de alta/baja para iniciar la captura [1:N].

Antes de avanzar se validan los datos propios del paso. Antes de confirmar se ejecuta
validacion cruzada: cada ficha obligatoria debe alcanzar `cant_minima`, ademas de exigir
tipo/clasificacion, fechas y narracion. El backend debera responder `400 Bad Request`
con `mensaje` cuando falle esta validacion.

### Reglas iniciales de ejemplo

* Hurto de objeto: interviniente victima y objeto, ambos con minimo uno.
* Hurto de vehiculo: interviniente victima y vehiculo, ambos con minimo uno.
* Siniestro de transito: dos intervinientes, un vehiculo y la ficha de transito.

Estas reglas son datos de parametrizacion, no condicionales permanentes del componente.

## Contrato de captura y persistencia

El JSON de `ingresarJSON.md` es un contrato de captura y transporte. Representa el
formulario completo que el usuario puede mantener como borrador en el frontend y que
se envia al backend para validacion, normalizacion y persistencia. No es el esquema
fisico de PostgreSQL y no debe almacenarse como un documento unico de la denuncia.

El backend recibira un DTO de ingreso con la forma del JSON, resolvera los catalogos y
validara sus relaciones, y luego guardara el agregado en las tablas del script
`database/sql/ingreso_denuncias.sql` dentro de una transaccion. La respuesta podra
devolver nuevamente una proyeccion JSON para continuar editando o consultar el parte.

## Raiz del agregado

`denuncia.id_sgsp` es el identificador tecnico y punto de partida de cada denuncia. Toda
entidad propia del parte debe relacionarse directa o indirectamente con ese valor. Las
claves de negocio o numeros visibles del formulario (`nro_formulario`) no sustituyen a
`id_sgsp` y no deben usarse como FK.

### Cardinalidades definitivas

| Relacion | Cardinalidad | Implementacion |
| --- | --- | --- |
| `denuncia` -> `denuncia_ubicacion` | 1:1 | `denuncia_ubicacion.id_sgsp UNIQUE` |
| `denuncia` -> `denuncia_tipificacion` | 1:1 | `denuncia_tipificacion.id_sgsp PK/FK` |
| `denuncia` -> `denuncia_interviniente` | 1:N | FK `denuncia_interviniente.id_sgsp` |
| `denuncia_interviniente` -> `interviniente_policia` | 0:1 | `id_interviniente PK/FK` |
| `denuncia` -> `denuncia_arma` | 0:N | FK `denuncia_arma.id_sgsp` |
| `denuncia` -> `denuncia_vehiculo` | 0:N | FK `denuncia_vehiculo.id_sgsp` |
| `denuncia_vehiculo` -> recuperacion | 0:1 | `UNIQUE(denuncia_vehiculo_recuperacion.id_vehiculo)` |
| `denuncia` -> `denuncia_objeto` | 0:N | FK `denuncia_objeto.id_sgsp` |
| `denuncia` -> `denuncia_transito` | 0:1 | `denuncia_transito.id_sgsp UNIQUE` |
| `denuncia_transito` -> personas | 1:N | FK `id_transito` |
| `denuncia` -> `denuncia_narracion` | 1:N | PK propia `id_narracion` + FK `id_sgsp` |
| `denuncia` -> ampliaciones | 0:N | FK `denuncia_ampliacion.id_sgsp` |
| ampliacion -> narraciones | 1:N | FK `denuncia_ampliacion_narracion.id_ampliacion` |
| `denuncia` -> equipo, calidad y cambios | 0:N | FK directa a `id_sgsp` |

Las tablas 1:1 usan una FK unica o como clave primaria. Las tablas 1:N tienen una
identidad propia y una FK hacia su padre. Las eliminaciones en cascada solo aplican a
datos dependientes del agregado; las referencias a usuarios y catalogos usan
`RESTRICT` para preservar historial.

## Tipificacion unificada

La captura puede conservar el texto y la forma del JSON, pero el backend debe traducir
las secciones `ClasificacionDelitoFalta`, `ClasificacionHechoPolicial`,
`ClasificacionAccidente` y `ClasificacionCrimen` a una sola referencia:

* `tipificacion_clasificacion`: clasificacion asociada a `tipo_denuncia`, con jerarquia
	opcional mediante `id_clasificacion_padre`.
* `tipificacion_complemento`: complementos pertenecientes a una clasificacion.
* `denuncia_tipificacion`: guarda `id_clasificacion`, `id_complemento`, modalidad,
	gravedad y tentativa.

Esto evita cuatro columnas mutuamente excluyentes y permite que la matriz
`regla_tipificacion_ficha` dependa de una clasificacion estable.

## Direcciones y catalogos

`denuncia_ubicacion` mantiene la direccion del hecho con sus coordenadas y referencias
de localidad, departamento y jurisdiccion. El domicilio de una persona mantiene una
estructura equivalente en `interviniente_domicilio`; ambos deben ser capturados por un
componente frontend reutilizable, aunque no representan el mismo domicilio y por tanto
no se deben compartir accidentalmente.

Los valores identificables del JSON (`id_nacionalidad`, `id_tipo_documento`,
`id_ocupacion`, `id_grado`, `id_jefatura`, `id_seccional`, etc.) deben resolverse contra
catalogos mediante FK o servicio de catalogo. `JSONB` queda reservado para atributos
dinamicos como `denuncia_objeto.atributos`, cuyo formato persistente recomendado es un
objeto clave-valor, por ejemplo `{"color":"rojo","marca":"Samsung"}`.

## Reglas de procesamiento del DTO

1. Validar que el DTO contenga el tipo, clasificacion, fechas y usuario actuante.
2. Resolver clasificacion, complemento, UUEE, dependencia y demas catalogos.
3. Crear `denuncia` y obtener `id_sgsp` dentro de una transaccion.
4. Insertar las relaciones 1:1 y 1:N usando ese `id_sgsp`.
5. Validar la matriz `regla_tipificacion_ficha` y las cardinalidades antes del commit.
6. Persistir auditoria y cambios sin confiar en identificadores enviados por el cliente.

El endpoint debe rechazar referencias inexistentes, duplicados incompatibles y fichas
obligatorias ausentes con `400 Bad Request` y `mensaje`. La interfaz puede mostrar una
validacion anticipada, pero la validacion definitiva pertenece al backend.
