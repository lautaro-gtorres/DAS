-- =============================================================
-- LRChickens - Campos y tablas que piden los formularios de Rocío
-- Ejecutar sobre DBPollos (SSMS o editor de consultas de Azure).
-- Se puede correr más de una vez: cada cambio se hace solo si todavía no existe.
--
-- Las columnas nuevas en tablas que ya tienen filas aceptan NULL para no romper
-- los datos existentes; que sean obligatorias lo controla la BLL.
-- Los Id NO son IDENTITY (se cargan a mano con MAX + 1).
-- =============================================================


-- =====================  PROVEEDOR  (CargarProveedor)  =====================
-- Formulario: NOMBRE, N° DE CUIL, DIRECCION, LOCALIDAD, TELEFONO, E-MAIL,
--             ESTADO (ACTIVO / INACTIVO), SERVICIO/PRODUCTO

IF COL_LENGTH('dbo.Proveedor', 'CUIL') IS NULL
    ALTER TABLE dbo.Proveedor ADD CUIL varchar(13) NULL;          -- 20-12345678-9
GO
IF COL_LENGTH('dbo.Proveedor', 'Localidad') IS NULL
    ALTER TABLE dbo.Proveedor ADD Localidad varchar(50) NULL;
GO
IF COL_LENGTH('dbo.Proveedor', 'Email') IS NULL
    ALTER TABLE dbo.Proveedor ADD Email varchar(100) NULL;
GO
IF COL_LENGTH('dbo.Proveedor', 'ServicioProducto') IS NULL
    ALTER TABLE dbo.Proveedor ADD ServicioProducto varchar(100) NULL;
GO
-- ESTADO: los proveedores que ya existen quedan activos.
IF COL_LENGTH('dbo.Proveedor', 'Activo') IS NULL
    ALTER TABLE dbo.Proveedor ADD Activo bit NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT 1;
GO


-- =====================  USUARIO  (UsuarioNuevo)  =====================
-- Formulario: NOMBRE, APELLIDO, DNI, FECHA DE NACIMIENTO, ROL, TELEFONO, E-MAIL,
--             USUARIO, CONTRASEÑA, CONFIRMAR CONTRASEÑA (esta última no se guarda)

IF COL_LENGTH('dbo.Usuario', 'Nombre') IS NULL
    ALTER TABLE dbo.Usuario ADD Nombre varchar(50) NULL;
GO
IF COL_LENGTH('dbo.Usuario', 'Apellido') IS NULL
    ALTER TABLE dbo.Usuario ADD Apellido varchar(50) NULL;
GO
IF COL_LENGTH('dbo.Usuario', 'DNI') IS NULL
    ALTER TABLE dbo.Usuario ADD DNI varchar(10) NULL;
GO
IF COL_LENGTH('dbo.Usuario', 'FechaNacimiento') IS NULL
    ALTER TABLE dbo.Usuario ADD FechaNacimiento date NULL;
GO
IF COL_LENGTH('dbo.Usuario', 'Telefono') IS NULL
    ALTER TABLE dbo.Usuario ADD Telefono varchar(20) NULL;
GO
IF COL_LENGTH('dbo.Usuario', 'Email') IS NULL
    ALTER TABLE dbo.Usuario ADD Email varchar(100) NULL;
GO


-- =====================  UBICACION + CORRAL  (CargarCorrales)  =====================
-- Formulario: TIPO, NUMERO DE CORRAL, CAPACIDAD MAX., UBICACION (combo)
-- Ubicacion es un catálogo (Id y Nombre) que se carga directamente en la base.

IF OBJECT_ID('dbo.Ubicacion', 'U') IS NULL
    CREATE TABLE dbo.Ubicacion
    (
        IdUbicacion int         NOT NULL CONSTRAINT PK_Ubicacion PRIMARY KEY,
        Nombre      varchar(50) NOT NULL CONSTRAINT UQ_Ubicacion_Nombre UNIQUE
    );
GO
IF COL_LENGTH('dbo.Corral', 'IdUbicacion') IS NULL
    ALTER TABLE dbo.Corral ADD IdUbicacion int NULL
        CONSTRAINT FK_Corral_Ubicacion REFERENCES dbo.Ubicacion (IdUbicacion);
GO


-- =====================  ALIMENTO  (CargarAlimento)  =====================
-- Formulario: NOMBRE, PROVEEDOR, KILOGRAMOS (el campo CANTIDAD no se usa).
-- La cantidad disponible pasa a medirse en kilogramos: se renombra la columna
-- (la tabla está vacía). Azure no deja renombrar una columna que usa un CHECK,
-- así que se borra el CHECK, se renombra la columna y se vuelve a crear el CHECK (>= 0).

IF COL_LENGTH('dbo.Alimento', 'CantidadDisponible') IS NOT NULL AND COL_LENGTH('dbo.Alimento', 'Kilogramos') IS NULL
BEGIN
    IF OBJECT_ID('dbo.CK_Alimento_Cantidad', 'C') IS NOT NULL
        ALTER TABLE dbo.Alimento DROP CONSTRAINT CK_Alimento_Cantidad;
    IF OBJECT_ID('dbo.CK_Alimento_Kilogramos', 'C') IS NOT NULL
        ALTER TABLE dbo.Alimento DROP CONSTRAINT CK_Alimento_Kilogramos;
    EXEC sp_rename 'dbo.Alimento.CantidadDisponible', 'Kilogramos', 'COLUMN';
END
GO
IF OBJECT_ID('dbo.CK_Alimento_Kilogramos', 'C') IS NULL
    ALTER TABLE dbo.Alimento ADD CONSTRAINT CK_Alimento_Kilogramos CHECK (Kilogramos >= 0);
GO

-- Los SP de 02_ABM.sql usaban CantidadDisponible: se actualizan a Kilogramos.
CREATE OR ALTER PROCEDURE AltaAlimento
    @nombreAlimento varchar(100), @kilogramos decimal(10,2), @idProveedor int
AS
    INSERT INTO Alimento (IdAlimento, NombreAlimento, Kilogramos, IdProveedor)
    VALUES ((SELECT ISNULL(MAX(IdAlimento), 0) + 1 FROM Alimento), @nombreAlimento, @kilogramos, @idProveedor);
GO
CREATE OR ALTER PROCEDURE ModificarAlimento
    @idAlimento int, @nombreAlimento varchar(100), @kilogramos decimal(10,2), @idProveedor int
AS
    UPDATE Alimento SET NombreAlimento = @nombreAlimento, Kilogramos = @kilogramos, IdProveedor = @idProveedor
    WHERE IdAlimento = @idAlimento;
GO
CREATE OR ALTER PROCEDURE ListarAlimentos
AS
    SELECT a.IdAlimento, a.NombreAlimento, a.Kilogramos, a.IdProveedor, p.Nombre AS Proveedor
    FROM Alimento a
    INNER JOIN Proveedor p ON p.IdProveedor = a.IdProveedor
    ORDER BY a.NombreAlimento;
GO
CREATE OR ALTER PROCEDURE BuscarAlimento
    @nombreAlimento varchar(100)
AS
    SELECT a.IdAlimento, a.NombreAlimento, a.Kilogramos, a.IdProveedor, p.Nombre AS Proveedor
    FROM Alimento a
    INNER JOIN Proveedor p ON p.IdProveedor = a.IdProveedor
    WHERE a.NombreAlimento LIKE '%' + @nombreAlimento + '%'
    ORDER BY a.NombreAlimento;
GO


-- =====================  CLIENTE  (VerClientes, combo CLIENTE de Vender)  =====================
-- Campos de BE/Cliente.cs. Activo para dar de baja sin borrar
-- (un cliente con ventas no se puede borrar por la FK).

IF OBJECT_ID('dbo.Cliente', 'U') IS NULL
    CREATE TABLE dbo.Cliente
    (
        IdCliente int          NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
        Nombre    varchar(50)  NOT NULL,
        Apellido  varchar(50)  NOT NULL,
        Telefono  varchar(20)  NULL,
        Email     varchar(100) NULL,
        Direccion varchar(150) NULL,
        Activo    bit          NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT 1
    );
GO


-- =====================  VENTA  (Vender)  =====================
-- Se venden gallinas y huevos (TipoProducto: 1 = Huevo, 2 = Pollo).
-- Formulario: CLIENTE (combo), TIPO GALLINA, RAZA GALLINA, SEXO GALLINA,
--             CANTIDAD, PRECIO UNITARIO, TOTAL (calculado).
-- Tipo, raza y sexo solo se cargan cuando se venden gallinas; en una venta de huevos quedan en NULL.
-- La tabla está vacía: el cliente pasa de texto a FK a Cliente.

IF COL_LENGTH('dbo.Venta', 'Cliente') IS NOT NULL
    ALTER TABLE dbo.Venta DROP COLUMN Cliente;
GO
IF COL_LENGTH('dbo.Venta', 'IdCliente') IS NULL
    ALTER TABLE dbo.Venta ADD IdCliente int NULL
        CONSTRAINT FK_Venta_Cliente REFERENCES dbo.Cliente (IdCliente);
GO
IF COL_LENGTH('dbo.Venta', 'IdTipoGallina') IS NULL
    ALTER TABLE dbo.Venta ADD IdTipoGallina int NULL
        CONSTRAINT FK_Venta_TipoGallina REFERENCES dbo.TipoGallina (IdTipoGallina);
GO
IF COL_LENGTH('dbo.Venta', 'IdRaza') IS NULL
    ALTER TABLE dbo.Venta ADD IdRaza int NULL
        CONSTRAINT FK_Venta_Raza REFERENCES dbo.Raza (IdRaza);
GO
IF COL_LENGTH('dbo.Venta', 'Sexo') IS NULL
    ALTER TABLE dbo.Venta ADD Sexo char(1) NULL
        CONSTRAINT CK_Venta_Sexo CHECK (Sexo IS NULL OR Sexo = 'M' OR Sexo = 'H');
GO


-- =====================  VACUNACION  (CargarVacunas)  =====================
-- Se vacuna por corral.
-- Formulario: NOMBRE, N° DE VACUNA, CANTIDAD, CORRAL (combo),
--             FECHA DE APLICACIÓN, PRÓXIMA APLICACIÓN

IF OBJECT_ID('dbo.Vacunacion', 'U') IS NULL
    CREATE TABLE dbo.Vacunacion
    (
        IdVacunacion      int          NOT NULL CONSTRAINT PK_Vacunacion PRIMARY KEY,
        NroVacuna         varchar(20)  NOT NULL,
        Nombre            varchar(100) NOT NULL,
        Cantidad          int          NOT NULL CONSTRAINT CK_Vacunacion_Cantidad CHECK (Cantidad > 0),
        IdCorral          int          NOT NULL CONSTRAINT FK_Vacunacion_Corral REFERENCES dbo.Corral (IdCorral),
        FechaAplicacion   date         NOT NULL,
        ProximaAplicacion date         NULL,
        CONSTRAINT CK_Vacunacion_Proxima CHECK (ProximaAplicacion IS NULL OR ProximaAplicacion >= FechaAplicacion)
    );
GO


-- =====================  Verificación  =====================
SELECT t.name AS Tabla, c.name AS Columna, ty.name AS Tipo, c.max_length, c.is_nullable
FROM sys.columns c
INNER JOIN sys.tables t ON t.object_id = c.object_id
INNER JOIN sys.types ty ON ty.user_type_id = c.user_type_id
WHERE (t.name = 'Proveedor' AND c.name IN ('CUIL', 'Localidad', 'Email', 'ServicioProducto', 'Activo'))
   OR (t.name = 'Usuario'   AND c.name IN ('Nombre', 'Apellido', 'DNI', 'FechaNacimiento', 'Telefono', 'Email'))
   OR (t.name = 'Corral'    AND c.name = 'IdUbicacion')
   OR (t.name = 'Alimento'  AND c.name = 'Kilogramos')
   OR (t.name = 'Venta'     AND c.name IN ('IdCliente', 'IdTipoGallina', 'IdRaza', 'Sexo'))
   OR t.name IN ('Ubicacion', 'Cliente', 'Vacunacion')
ORDER BY t.name, c.column_id;
GO
