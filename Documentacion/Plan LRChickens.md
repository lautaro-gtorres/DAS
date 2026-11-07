# LRChickens — Evaluación y plan de trabajo

_Actualizado: 05/11/2026_

Fuentes revisadas:
- Google Doc "Proyecto pollos" (copia en esta carpeta: `Proyecto pollos (Google Doc).docx`)
- `Proyecto pollos visual.pdf` (diseño de pantallas)
- Rama `LRChickens` @ `5f280ee` ("gui/be", 01/11)
- Base de datos `DBPollos` (Azure SQL)

---

## 1. Estado del código (rama LRChickens)

- **No compila:** se borró `BE/Alumno.cs` pero `BLL/Alumno.cs` y `DAL/MapAlumno.cs` lo siguen usando.
- **BE completos:** Usuario, Gallina, Proveedor, Cliente, Venta.
- **BE vacíos (y `internal`):** Corral, Alimento, Alimentacion, Produccion, Vacuna, Vacunacion, Pedido, DetallePedido, DetalleVenta.
- **Restos de ejercicios de clase:** carpeta `Capas/`, `Class1.cs` en BLL y DAL, proyecto GUI llamado `ClaseCapas` / `Form1`.
- **GUI:** Login (usuario y contraseña escritos en el código), Inicio, Gallinas, Proveedores y Ventas. Solo navegan entre sí, con el mismo código duplicado en los 4 formularios. No hay formularios de alta ni de listado.
- **DAL:** `Acceso` apunta a una instancia local (catálogo "Prueba"), no a Azure. No es Singleton y no tiene try/catch.
- **No existen** la capa Mapper ni scripts SQL en el repo.

## 2. Estado de la base de datos (DBPollos)

| Tabla | Columnas | Relaciones / restricciones | Datos |
|---|---|---|---|
| Gallina | IdGallina, CodGallina, IdRaza, IdTipoGallina, Sexo, FechaNacimiento, IdCorral, Detalle, Activo | FK Raza, TipoGallina, Corral · Cod único · Sexo M/H | vacía |
| Corral | IdCorral, NroCorral, IdTipoCorral, Capacidad | FK TipoCorral · Nro único · Capacidad ≥ 0 | vacía |
| Produccion | IdProduccion, IdCorral, Fecha, CantidadHuevos | FK Corral · único por corral+fecha | vacía |
| Alimento | IdAlimento, NombreAlimento, CantidadDisponible, IdProveedor | FK Proveedor · cantidad ≥ 0 | vacía |
| Proveedor | IdProveedor, Nombre, Telefono, Direccion | — | vacía |
| Usuario | IdUsuario, NombreUsuario, Contrasena (256), IdRol, Activo | FK Rol · nombre único | vacía |
| Venta | IdVenta, Fecha, IdTipoProducto, Cantidad, PrecioUnitario, Total (calculado), Cliente (texto) | FK TipoProducto | vacía |
| Rol | Administrador, Operario | | cargada |
| TipoCorral | Ponedoras, Engorde, Cria | | cargada |
| TipoGallina | Ponedora, Engorde, Gallo | | cargada |
| TipoProducto | Huevo, Pollo | | cargada |
| Raza | IdRaza, NombreRaza | nombre único | vacía |

Problemas detectados:
1. **Ninguna PK es IDENTITY:** la aplicación tendría que generar los IDs a mano.
2. **Faltan tablas:** Cliente, DetalleVenta, Pedido, DetallePedido, Vacuna, Vacunación y Alimentación.
3. **Venta es de una sola línea:** el cliente se guarda como texto y no hay medio de pago.
4. **A Proveedor y Usuario les faltan campos del diseño:** mail, CUIL/DNI, localidad, estado, nombre y apellido.
5. **No hay stored procedures.**
6. **La aplicación y las herramientas usan el usuario administrador.**

## 3. Inconsistencias entre doc, diseño, BD y BE

| Tema | Doc / BE | Diseño (PDF) | BD |
|---|---|---|---|
| Nombres de ID | `Nro*` | — | `Id*` |
| Gallina | Nombre, Raza (texto), Estado | Cod, Tipo, Raza, Sexo, Detalle | Igual al diseño |
| Venta | Venta + DetalleVenta + Cliente + MedioPago | Vende por tipo/raza/sexo de gallina | Una línea por TipoProducto (Huevo/Pollo) |
| Corral | Nombre, Descripción, Estado | Número, Sector, Ubicación | NroCorral, TipoCorral, Capacidad |
| Vacunas | Vacunación por gallina | Por corral, con cantidad | No existe |
| Personas | Pocos campos | DNI/CUIL, apellido, localidad, mail, estado | Pocos campos |
| Otras pantallas | — | Agenda, Consultar a proveedores, Olvidé mi contraseña | Sin entidad |
| Producción | Entidad | Sin pantalla | Tabla existe |

El doc también tiene inconsistencias internas: el árbol de carpetas lista 10 BLL/Mappers y la lista de abajo 14, y la sección "DBPollos" está vacía.

## 4. Propuesta de modelo (consignas 4 y 5: herencia y listas polimórficas)

- **`Persona` (abstracta) → Cliente, Proveedor, Usuario.** En la BD quedan como tablas separadas y la herencia vive en BE. La Agenda y las búsquedas usan `List<Persona>`.
- **`Suministro` (abstracta) → Alimento, Vacuna.** "Consultar stock" usa `List<Suministro>`.
- **`Operacion` (abstracta) → Venta, Pedido**, con detalles y un `CalcularTotal()` virtual. "Registros" usa `List<Operacion>`.
- **`FrmBase`** con la barra de navegación, de la que heredan los formularios. Elimina el código duplicado (consigna 1).
- **Enums** para Sexo, Estado y MedioPago. Rol y los tipos se leen de las tablas de catálogo.

## 5. Plan

0. **Sincronizar:** guardar el trabajo local pendiente, pasarse a `LRChickens` y hacer pull.
1. **Reunión de equipo:** definir la BD como fuente de verdad, cerrar las inconsistencias de la sección 3 y el alcance de la Entrega 1.
2. **Ajustar DBPollos** (las tablas están vacías, así que se pueden recrear sin perder nada):
   - IDENTITY en todas las PKs.
   - Agregar Cliente, Venta (cabecera) + DetalleVenta, Pedido + DetallePedido, Vacuna + AplicacionVacuna.
   - Completar Proveedor y Usuario.
   - Exportar el script a `/Database` en el repo.
   - Stored procedures de alta, baja, modificación, listar y buscar.
3. **Limpieza del repo:** borrar `Capas/`, Alumno y los `Class1`. Renombrar a LRChickens / GUI / FrmLogin. Agregar el proyecto Mapper (BE ← DAL ← Mapper ← BLL ← GUI). Alinear BE a la BD (`Id*`).
4. **DAL:** `Acceso` Singleton (consigna 11), connection string de Azure en `App.config`, try-catch y transacciones para ventas y pedidos.
5. **Caso completo de punta a punta:** Gallina + Corral, desde el BE hasta el formulario con validaciones. Queda como plantilla para el resto.
6. **Resto de los ABM:** Cliente, Proveedor, Usuario (login real con contraseña hasheada), Alimento y Vacuna.
7. **Operaciones:** Vender, Comprar, Registros y Consultar stock.
8. **Opcionales:** Agenda, Consultar a proveedores, Producción y Olvidé mi contraseña.
9. **Cierre:** checklist contra las 12 consignas y prueba completa.

## 6. Seguridad

- No subir credenciales al repo (ni en `App.config` versionado).
- Rotar la contraseña del usuario administrador de la BD.
- Crear un usuario de solo lectura para herramientas y otro para la aplicación con permiso EXECUTE sobre los stored procedures.
