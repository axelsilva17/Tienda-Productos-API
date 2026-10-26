# Tienda Productos API — Bloque 2

Web API RESTful para la gestión de un catálogo de productos, construida en .NET con
**arquitectura en 3 capas**, **DTOs**, **Data Annotations** para validación y respuestas
**`IActionResult`** con códigos HTTP estandarizados. Datos en memoria.

## Arquitectura en 3 capas

```
HTTP request
     │
     ▼
Controllers/ProductosController.cs        ← Presentation: endpoints + IActionResult
     │  (IProductoService por DI)
     ▼
Services/ProductoService.cs               ← Business: reglas + mapeo Entidad <-> DTO
     │  (IProductoRepository por DI)
     ▼
Repositories/ProductoRepositoryInMemory.cs ← Data Access: List<Producto> en memoria
     │
     ▼
Models/Producto.cs                        ← Entidad de dominio (no sale de la API)
```

La dependencia siempre apunta **hacia adentro**: el controlador conoce el servicio, el
servicio conoce el repositorio, y nadie conoce la consola ni la base de datos. La entidad
`Producto` es la única pieza compartida por las tres capas.

## Estructura

```
Tienda-Productos-API/
├── Controllers/
│   └── ProductosController.cs       # 5 endpoints REST
├── Services/
│   ├── IProductoService.cs          # Contrato de negocio
│   └── ProductoService.cs           # Lógica + mapeo DTO <-> Entidad
├── Repositories/
│   ├── IProductoRepository.cs       # Contrato de acceso a datos
│   └── ProductoRepositoryInMemory.cs# List<Producto> privada + seed data
├── Models/
│   └── Producto.cs                  # Entidad de dominio
├── Dtos/
│   ├── CreateProductoDto.cs         # Entrada (con Data Annotations)
│   └── ProductoDto.cs               # Salida (nunca se expone la entidad)
├── Properties/launchSettings.json
├── Program.cs                       # DI + Swagger
└── TiendaProductos.Api.csproj
```

## Ejecutar

```bash
dotnet restore
dotnet run
```

Luego abrir:

```
http://localhost:5199/swagger
```

Swagger está habilitado **siempre** (no solo en Development) para que se pueda probar
cualquier endpoint apenas se clona el repo.

> Requiere .NET SDK 10. Si se necesita .NET 8, cambiar `<TargetFramework>net10.0</TargetFramework>`
> en el `.csproj` (Swashbuckle 10 funciona igual).

## Endpoints

| Verbo | Ruta | Éxito | Errores |
| --- | --- | --- | --- |
| `GET` | `/api/productos` | `200 OK` con `ProductoDto[]` | — |
| `GET` | `/api/productos/{id}` | `200 OK` con `ProductoDto` | `404 Not Found` |
| `POST` | `/api/productos` | `201 Created` + header `Location` | `400 Bad Request` |
| `PUT` | `/api/productos/{id}` | `200 OK` con `ProductoDto` | `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/productos/{id}` | `204 No Content` | `404 Not Found` |

### Ejemplos

```bash
# Listar
curl http://localhost:5199/api/productos

# Crear
curl -X POST http://localhost:5199/api/productos \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Silla gamer","precio":123456.78,"categoria":"Muebles","activo":true}'

# Actualizar
curl -X PUT http://localhost:5199/api/productos/5 \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Silla ergonomica","precio":150000,"categoria":"Muebles","activo":false}'

# Eliminar
curl -X DELETE http://localhost:5199/api/productos/5
```

## Validaciones (`CreateProductoDto`)

| Campo | Regla | Mensaje |
| --- | --- | --- |
| `Nombre` | `[Required]` + `[StringLength(100, MinimumLength = 3)]` | "El nombre debe tener entre 3 y 100 caracteres." |
| `Precio` | `[Required]` + `[Range(0.01, double.MaxValue)]` | "El precio debe ser mayor a 0." |
| `Categoria` | `[Required]` | "La categoría es obligatoria." |

Gracias a `[ApiController]`, si el modelo es inválido la acción **no se ejecuta** y la API
responde `400 Bad Request` con el detalle por campo (`ValidationProblemDetails`):

```json
{
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Nombre": ["El nombre debe tener entre 3 y 100 caracteres."],
    "Precio": ["El precio debe ser mayor a 0."]
  }
}
```

## Decisiones de diseño

- **DTOs en los bordes.** El controlador nunca retorna `Producto`: retorna `ProductoDto`.
  Así el contrato público no queda atado a la entidad interna (se puede agregar un campo
  al dominio sin filtrarlo al cliente) y se evita over-posting en la entrada.
- **Mapeo en el servicio.** `ProductoService` es el único que conoce ambas
  representaciones (`ADominio` / `ADto`). El controlador no mapea y el repositorio no
  sabe que existen DTOs.
- **El servicio no conoce HTTP.** Devuelve `null` o `false` cuando algo no existe; es el
  controlador el que traduce eso a `404`. Esa separación es la que permite reutilizar la
  lógica desde otra interfaz (consola, gRPC, tests).
- **`AddScoped` en servicios y repositorio** (como pide la consigna): una instancia por
  request HTTP.
- **Almacenamiento `static` en el repositorio.** Con `AddScoped`, un campo de instancia
  haría que cada request arranque con su propia copia del seed data: un `POST` seguido de
  un `GET` no encontraría el producto creado. La lista vive a nivel de proceso (compartida)
  y cada instancia Scoped la consume. Los accesos están protegidos con `lock` porque
  Kestrel atiende requests concurrentes y dos `POST` simultáneos podrían repetir Id.
- **`decimal` para precios**, nunca `double`: representa exacto los valores monetarios.
- **`Range` con sobrecarga numérica y no con `typeof(decimal)`.** La sobrecarga de tipo
  parsea los extremos usando la cultura del proceso: en `es-AR` el literal `"0.01"` es
  inválido (separador decimal = coma) y provoca un `500`. Con `[Range(0.01, double.MaxValue)]`
  no hay parsing de texto y el comportamiento es idéntico en cualquier cultura.
- **Seed data** de 4 productos en el constructor estático: la API arranca con datos
  probables desde Swagger, sin pasos previos.
- **Los datos se pierden al reiniciar** el proceso: es un repositorio en memoria, tal como
  pide la consigna. La interfaz `IProductoRepository` es el punto de reemplazo por EF Core.

## Verificación

Probado con la API corriendo en `http://localhost:5199`:

| Caso | Resultado esperado | Obtenido |
| --- | --- | --- |
| `GET /api/productos` | 200, 4 productos | ✅ |
| `GET /api/productos/2` | 200 | ✅ |
| `GET /api/productos/99` | 404 | ✅ |
| `POST` válido | 201 + `Location: /api/productos/5` | ✅ |
| `GET /api/productos/5` tras el POST | 200 (persiste entre requests) | ✅ |
| `POST` inválido | 400 con errores por campo | ✅ |
| `PUT /api/productos/5` | 200 con datos nuevos | ✅ |
| `PUT /api/productos/99` | 404 | ✅ |
| `DELETE /api/productos/5` | 204 | ✅ |
| `DELETE` repetido | 404 | ✅ |
| `GET /api/productos` final | 200, vuelve a 4 | ✅ |
| Swagger UI | 200 y OpenAPI 3.0.4 con las 5 operaciones | ✅ |
