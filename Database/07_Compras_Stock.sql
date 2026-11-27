-- =============================================================
-- LRChickens - Compras a proveedores, stock de huevos y ajustes de formularios
-- Ejecutar sobre DBPollos después de 01 a 06. Se puede correr más de una vez.
--
-- Decisiones por defecto (a confirmar con el equipo):
--   * Comprar: se compra alimento (SUMINISTRO) a un proveedor, en kilogramos.
--     Al registrar la compra se suma al stock del alimento; al anularla se resta.
--   * Los huevos disponibles son lo producido menos lo vendido: no se pueden
--     vender más huevos de los que hay.
--   * El DNI de los usuarios y el CUIL de los proveedores no se pueden repetir.
--   * Al modificar un usuario, si la contraseña viene vacía (NULL) se conserva la anterior.
-- =============================================================


-- =====================  COMPRA  (Comprar, VerCompras)  =====================

IF OBJECT_ID('dbo.Compra', 'U') IS NULL
    CREATE TABLE dbo.Compra
    (
        IdCompra       int           NOT NULL CONSTRAINT PK_Compra PRIMARY KEY,
        Fecha          date          NOT NULL,
        IdAlimento     int           NOT NULL CONSTRAINT FK_Compra_Alimento REFERENCES dbo.Alimento (IdAlimento),
        IdProveedor    int           NOT NULL CONSTRAINT FK_Compra_Proveedor REFERENCES dbo.Proveedor (IdProveedor),
        Kilogramos     decimal(10,2) NOT NULL CONSTRAINT CK_Compra_Kilogramos CHECK (Kilogramos > 0),
        PrecioUnitario decimal(10,2) NOT NULL CONSTRAINT CK_Compra_Precio CHECK (PrecioUnitario > 0),
        Total          AS (Kilogramos * PrecioUnitario)
    );
GO

-- Registra la compra y suma los kilos al stock del alimento (las dos cosas en la misma transacción).
CREATE OR ALTER PROCEDURE AltaCompra
    @fecha date, @idAlimento int, @idProveedor int, @kilogramos decimal(10,2), @precioUnitario decimal(10,2)
AS
BEGIN
    INSERT INTO Compra (IdCompra, Fecha, IdAlimento, IdProveedor, Kilogramos, PrecioUnitario)
    VALUES ((SELECT ISNULL(MAX(IdCompra), 0) + 1 FROM Compra), @fecha, @idAlimento, @idProveedor, @kilogramos, @precioUnitario);

    UPDATE Alimento SET Kilogramos = Kilogramos + @kilogramos WHERE IdAlimento = @idAlimento;
END
GO

-- Solo se corrigen fecha y precio (los kilos ya se sumaron al stock).
CREATE OR ALTER PROCEDURE ModificarCompra
    @idCompra int, @fecha date, @precioUnitario decimal(10,2)
AS
    UPDATE Compra SET Fecha = @fecha, PrecioUnitario = @precioUnitario WHERE IdCompra = @idCompra;
GO

-- Anula la compra y resta los kilos del stock.
-- Si ya se usaron (el stock quedaría negativo), el CHECK de Alimento lo impide y no se anula nada.
CREATE OR ALTER PROCEDURE BajaCompra
    @idCompra int
AS
BEGIN
    UPDATE a SET a.Kilogramos = a.Kilogramos - c.Kilogramos
    FROM Alimento a
    INNER JOIN Compra c ON c.IdAlimento = a.IdAlimento
    WHERE c.IdCompra = @idCompra;

    DELETE FROM Compra WHERE IdCompra = @idCompra;
END
GO

CREATE OR ALTER PROCEDURE ListarCompras
AS
    SELECT c.IdCompra, c.Fecha, c.IdAlimento, a.NombreAlimento AS Alimento, c.IdProveedor, p.Nombre AS Proveedor,
           c.Kilogramos, c.PrecioUnitario, c.Total
    FROM Compra c
    INNER JOIN Alimento a  ON a.IdAlimento = c.IdAlimento
    INNER JOIN Proveedor p ON p.IdProveedor = c.IdProveedor
    ORDER BY c.Fecha DESC, c.IdCompra DESC;
GO
-- Busca por nombre del proveedor.
CREATE OR ALTER PROCEDURE BuscarCompra
    @proveedor varchar(100)
AS
    SELECT c.IdCompra, c.Fecha, c.IdAlimento, a.NombreAlimento AS Alimento, c.IdProveedor, p.Nombre AS Proveedor,
           c.Kilogramos, c.PrecioUnitario, c.Total
    FROM Compra c
    INNER JOIN Alimento a  ON a.IdAlimento = c.IdAlimento
    INNER JOIN Proveedor p ON p.IdProveedor = c.IdProveedor
    WHERE p.Nombre LIKE '%' + @proveedor + '%'
    ORDER BY c.Fecha DESC, c.IdCompra DESC;
GO


-- =====================  STOCK DE HUEVOS  =====================

-- Huevos disponibles = producidos - vendidos.
CREATE OR ALTER PROCEDURE StockHuevos
AS
    SELECT (SELECT ISNULL(SUM(CantidadHuevos), 0) FROM Produccion)
         - (SELECT ISNULL(SUM(Cantidad), 0) FROM Venta WHERE IdTipoProducto = 1) AS Disponibles;
GO

-- AltaVenta ahora también controla el stock de huevos (el resto queda igual que en 06).
CREATE OR ALTER PROCEDURE AltaVenta
    @fecha date, @idTipoProducto int, @cantidad int, @precioUnitario decimal(10,2), @idCliente int,
    @idTipoGallina int, @idRaza int, @sexo char(1), @idVenta int OUTPUT
AS
BEGIN
    IF @idTipoProducto = 1   -- Huevo
       AND (SELECT ISNULL(SUM(CantidadHuevos), 0) FROM Produccion)
         - (SELECT ISNULL(SUM(Cantidad), 0) FROM Venta WHERE IdTipoProducto = 1) < @cantidad
        THROW 50002, 'No hay suficientes huevos disponibles para esa venta.', 1;

    SET @idVenta = (SELECT ISNULL(MAX(IdVenta), 0) + 1 FROM Venta);

    INSERT INTO Venta (IdVenta, Fecha, IdTipoProducto, Cantidad, PrecioUnitario, IdCliente, IdTipoGallina, IdRaza, Sexo)
    VALUES (@idVenta, @fecha, @idTipoProducto, @cantidad, @precioUnitario, @idCliente, @idTipoGallina, @idRaza, @sexo);

    IF @idTipoProducto = 2   -- Gallina
    BEGIN
        WITH vendidas AS
        (
            SELECT TOP (@cantidad) Activo, IdVenta
            FROM Gallina
            WHERE Activo = 1 AND IdTipoGallina = @idTipoGallina AND IdRaza = @idRaza AND Sexo = @sexo
            ORDER BY IdGallina
        )
        UPDATE vendidas SET Activo = 0, IdVenta = @idVenta;

        IF @@ROWCOUNT < @cantidad
            THROW 50001, 'No hay suficientes gallinas disponibles para esa venta.', 1;
    END
END
GO


-- =====================  USUARIO  =====================

-- Contraseña NULL = se conserva la que tenía (para editar un usuario sin volver a escribirla).
CREATE OR ALTER PROCEDURE ModificarUsuario
    @idUsuario int, @nombreUsuario varchar(50), @contrasena varchar(256), @idRol int,
    @nombre varchar(50), @apellido varchar(50), @dni varchar(10), @fechaNacimiento date,
    @telefono varchar(20), @email varchar(100)
AS
    UPDATE Usuario SET NombreUsuario = @nombreUsuario, Contrasena = ISNULL(@contrasena, Contrasena), IdRol = @idRol,
                       Nombre = @nombre, Apellido = @apellido, DNI = @dni, FechaNacimiento = @fechaNacimiento,
                       Telefono = @telefono, Email = @email
    WHERE IdUsuario = @idUsuario;
GO


-- =====================  DNI y CUIL únicos  =====================
-- Índices únicos filtrados: se permiten varios NULL, pero no dos iguales.

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Usuario_DNI')
    CREATE UNIQUE INDEX UQ_Usuario_DNI ON dbo.Usuario (DNI) WHERE DNI IS NOT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Proveedor_CUIL')
    CREATE UNIQUE INDEX UQ_Proveedor_CUIL ON dbo.Proveedor (CUIL) WHERE CUIL IS NOT NULL;
GO


-- =====================  Verificación  =====================
SELECT name AS Procedimiento, modify_date
FROM sys.procedures
WHERE name IN ('AltaCompra', 'ModificarCompra', 'BajaCompra', 'ListarCompras', 'BuscarCompra',
               'StockHuevos', 'AltaVenta', 'ModificarUsuario')
ORDER BY name;
SELECT name AS Indice FROM sys.indexes WHERE name IN ('UQ_Usuario_DNI', 'UQ_Proveedor_CUIL');
GO
