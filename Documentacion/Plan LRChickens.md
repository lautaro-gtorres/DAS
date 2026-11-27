# LRChickens — Estado del proyecto

_Actualizado: 27/11/2026 · rama `LRChickens`_

Sistema de gestión de una granja avícola (gallinas, huevos, alimento, vacunas, ventas y compras) en C# WinForms por capas, sobre Azure SQL (`DBPollos`).

---

## 1. Cómo ponerlo en marcha

1. **Conexión:** copiar `ClaseCapas/ConnectionStrings.example.config` como `ClaseCapas/ConnectionStrings.config` y completar servidor, usuario y contraseña. Ese archivo está en `.gitignore`: **nunca se sube al repo**.
2. **Base de datos:** correr los scripts de `Database/` **en orden**, en SSMS o en el editor de consultas de Azure. Todos se pueden correr más de una vez.

   | Script | Qué hace |
   |---|---|
   | `01_Login.sql` | SP de login (`SP_ValidarUsuario`) |
   | `02_ABM.sql` | SP de alta, baja, modificación, listar y buscar de las tablas originales |
   | `03_CamposFormularios.sql` | Campos de los formularios de Rocío; tablas Ubicacion, Cliente y Vacunacion; Alimento en kilogramos |
   | `04_ABM_Formularios.sql` | SP de Proveedor, Usuario, Corral (con ubicación) y Cliente |
   | `05_TipoProductoGallina.sql` | El tipo de producto "Pollo" pasa a llamarse "Gallina" |
   | `06_VentaVacunacion.sql` | Venta de huevos y gallinas (da de baja las vendidas), Vacunacion y comprobante |
   | `07_Compras_Stock.sql` | Compras a proveedores, stock de huevos, DNI y CUIL únicos |
   | `08_DatosDePrueba.sql` | Datos de ejemplo para que todos los combos tengan opciones |

3. **Compilar:** abrir `ClaseCapas/ClaseCapas.slnx`. Si Visual Studio no toma los cambios, usar **Recompilar solución**.
4. **Entrar:** con un usuario de la tabla Usuario (contraseña en texto plano, decisión del equipo).

## 2. Arquitectura (consigna 12)

```
GUI (ClaseCapas) → BLL → MAPPERS → DAL → Azure SQL (solo stored procedures)
         ↘───────────── BE (entidades, las usan todas las capas) ─────────────↙
```

| Capa | Qué tiene |
|---|---|
| **BE** | Entidades con atributos privados y propiedades get/set. Herencia: `Persona` → `Cliente`, `Usuario`; `Movimiento` → `Venta`, `Compra` |
| **DAL** | `Acceso` (Singleton): conexión, `Leer`, `Escribir` (con transacción), transacciones de varias operaciones y XML. Reintenta si Azure está "despertando" |
| **MAPPERS** | Un mapper por entidad que implementa `IMapper<T>`: arma los parámetros de los SP y convierte filas en objetos. `Conversion` con los helpers comunes |
| **BLL** | Reglas de negocio y validaciones (`IABMeable<T>`). Traduce los errores de SQL (repetido, FK) a mensajes para el usuario. `Validaciones` con los helpers comunes. `Sesion` (Singleton) con el usuario logueado |
| **GUI** | Formularios. Solo leen los campos, llaman a la BLL y muestran mensajes. `Pantalla` con los mensajes y la configuración de grillas |

Reglas de la base: los Id **no son IDENTITY** (los SP de alta usan `MAX(Id) + 1`). Gallina, Usuario y Cliente tienen **baja lógica** (`Activo = 0`); el resto se borra.

## 3. Pantallas

| Menú | Botón | Pantalla | Qué hace |
|---|---|---|---|
| Inicio | GALLINAS / PROVEEDORES / VENTAS | Menús | Navegación |
| Inicio | PRODUCCIÓN | CargarProduccion | Huevos por corral y por día (ABM en una pantalla) y huevos disponibles |
| Inicio | REGISTROS | RegistrosVender (modo movimientos) | Ventas y compras juntas (`List<Movimiento>`) con el balance |
| Inicio | AGENDA | Agenda | Próximas vacunaciones |
| Inicio | USUARIO NUEVO / VER USUARIOS | UsuarioNuevo / VerUsuarios | Solo para el Administrador. "Olvidé mi contraseña" pone una nueva |
| Gallinas | Cargar gallina | CargarGallina | De a una o **por mayor** (prefijo + cantidad: GAL-001, GAL-002…) |
| Gallinas | Ver gallinas | VerGallinas | Modificar y dar de baja |
| Gallinas | Vender | Vender | Huevos o gallinas (ver Ventas) |
| Gallinas | Cargar alimento | CargarAlimento | Alimento con stock en kg |
| Gallinas | Cargar corrales | CargarCorrales | ABM en una pantalla, con tipo y ubicación |
| Gallinas | Cargar vacunas | CargarVacunas | Vacunación por corral, con próxima aplicación opcional |
| Proveedores | Cargar / Ver proveedores | CargarProveedor / VerProveedor | Con CUIL, localidad, e-mail, servicio y estado |
| Proveedores | Realizar pedido | Comprar | Compra de alimento por kilo: suma al stock |
| Proveedores | Consultar a proveedores | VerCompras | Historial de compras; anular resta del stock |
| Ventas | Cargar / Ver clientes | CargarCliente / VerClientes | ABM de clientes (baja lógica) |
| Ventas | Vender | Vender | Producto Huevo/Gallina, cliente opcional; al vender abre el **comprobante** |
| Ventas | Agenda | Agenda | Igual que desde Inicio |
| Ventas | Consultar stock | ConsultarStock | Alimentos, huevos y gallinas disponibles; "Reponer" abre Comprar |
| Ventas | Ver registro de ventas | RegistrosVender | Corregir (cliente y precio), anular y reimprimir el comprobante |

**Comprobante:** no fiscal, tipo "X", número `0001-` + número de venta. Se imprime con vista previa (con "Microsoft Print to PDF" queda como PDF) y se puede guardar en XML.

**Reglas de negocio principales:**
- Vender gallinas da de baja las primeras N activas del tipo, raza y sexo elegidos; anular la venta las reactiva.
- No se venden más huevos de los disponibles (producidos − vendidos).
- Una compra suma kilos al stock; no se puede anular si el alimento ya se usó.
- DNI de usuarios y CUIL de proveedores no se repiten.
- Un corral no admite más gallinas que su capacidad.

## 4. Consignas

| # | Consigna | Dónde se cumple |
|---|---|---|
| 1 | Estructura clara, sin código redundante | Capas separadas; helpers comunes en `Pantalla`, `Validaciones` y `Conversion` |
| 2 | Encapsulamiento | Todos los BE: atributos privados + propiedades get/set |
| 3 | Constructores | Constructores con parámetros en BE (`Cliente`, `Usuario`, `Venta`, `Compra`), `protected` en las clases abstractas; formularios con constructor de alta y de edición |
| 4 | Herencia y polimorfismo | `Persona` y `Movimiento` abstractas, con métodos `abstract`/`virtual` redefinidos (`TipoPersona`, `NombreCompleto`, `Detalle`, `Importe`); interfaces `IMapper<T>` e `IABMeable<T>` |
| 5 | Listas polimórficas | `List<Movimiento>` con ventas y compras (REGISTROS y balance) |
| 6 | Manejo de excepciones | try/catch en Acceso (rollback), en la BLL (traduce errores de SQL) y en cada evento de la GUI |
| 7 | Interfaz clara | Diseño de Rocío; grillas de solo lectura, combos de lista, confirmación antes de borrar |
| 8 | Eventos separados de la lógica | Los formularios solo llaman a la BLL |
| 9 | Validaciones con MessageBox | Validaciones en la BLL; la GUI las muestra como aviso |
| 10 | Persistencia separada | DAL + stored procedures; XML del comprobante |
| 11 | Singleton para la conexión | `DAL.Acceso.Instancia` (y `BLL.Sesion.Instancia`) |
| 12 | Separación por capas | GUI / BLL / MAPPERS / DAL / BE |

## 5. Decisiones tomadas para confirmar con el equipo

Se eligieron para que todo funcione; son fáciles de cambiar.

- **Comprar:** se compra alimento por kilo y suma al stock. "Consultar a proveedores" es el historial de compras.
- **Agenda:** muestra las próximas vacunaciones.
- **"Olvidé mi contraseña":** el administrador le pone una contraseña nueva al usuario elegido. Al modificar un usuario, si la contraseña queda vacía se conserva la anterior.
- **Cambios en formularios de Rocío:** los dos botones "INICIO" de Inicio pasaron a PRODUCCIÓN y AGENDA; el botón de guardar de CargarGallina dice "Registrar" (decía "Vender"); en CargarAlimento se ocultó CANTIDAD (se usan kilogramos); en UsuarioNuevo se quitó el botón sin texto al lado de ROL.
- **Formularios nuevos (diseño propio):** CargarCliente, CargarProduccion y VerCompras.

## 6. Pendiente

- Confirmar con Rocío las decisiones de la sección 5 y revisar visualmente las pantallas (las pruebas automáticas corrieron con las ventanas fuera de la pantalla).
- Resolver lo que salga de las pruebas manuales.
- Pasar la rama `LRChickens` a `main` al terminar.

## 7. Pruebas

Se probaron contra Azure, con datos que empiezan con `ZZ`:
- **BLL:** todos los ABM, validaciones, ventas (gallinas y huevos), compras, carga por mayor, comprobante, herencia y lista polimórfica.
- **Formularios:** los 24 botones de los menús con su Salir, y el flujo principal de cada pantalla con clics reales.

Como la base es compartida, después de correr esas pruebas hay que borrar los datos `ZZ` que dejan (gallinas, usuarios y clientes tienen baja lógica y quedan en la base).

## 8. Seguridad

- No subir credenciales al repo (`ConnectionStrings.config` está en `.gitignore`).
- Recomendado: rotar la contraseña del usuario administrador de la BD y crear un usuario para la aplicación con permiso EXECUTE solo sobre los stored procedures.
