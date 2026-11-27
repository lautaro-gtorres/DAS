-- =============================================================
-- LRChickens - Venta (huevos y gallinas) y Vacunacion (por corral)
-- Ejecutar sobre DBPollos después de 01 a 05. Se puede correr más de una vez.
--
-- Decisiones:
--   * Se venden huevos y gallinas (TipoProducto 1 = Huevo, 2 = Gallina).
--   * Vender gallinas las da de baja: se desactivan N gallinas activas del
--     tipo, raza y sexo elegidos, y quedan marcadas con el Id de la venta.
--   * El cliente es opcional.
--   * Vacunacion ya no tiene N° de vacuna.
-- =============================================================


-- =====================  VACUNACION  =====================

IF COL_LENGTH('dbo.Vacunacion', 'NroVacuna') IS NOT NULL
    ALTER TABLE dbo.Vacunacion DROP COLUMN NroVacuna;
GO

CREATE OR ALTER PROCEDURE AltaVacunacion
    @nombre varchar(100), @cantidad int, @idCorral int, @fechaAplicacion date, @proximaAplicacion date
AS
    INSERT INTO Vacunacion (IdVacunacion, Nombre, Cantidad, IdCorral, FechaAplicacion, ProximaAplicacion)
    VALUES ((SELECT ISNULL(MAX(IdVacunacion), 0) + 1 FROM Vacunacion), @nombre, @cantidad, @idCorral,
            @fechaAplicacion, @proximaAplicacion);
GO
CREATE OR ALTER PROCEDURE ModificarVacunacion
    @idVacunacion int, @nombre varchar(100), @cantidad int, @idCorral int, @fechaAplicacion date,
    @proximaAplicacion date
AS
    UPDATE Vacunacion SET Nombre = @nombre, Cantidad = @cantidad, IdCorral = @idCorral,
                          FechaAplicacion = @fechaAplicacion, ProximaAplicacion = @proximaAplicacion
    WHERE IdVacunacion = @idVacunacion;
GO
CREATE OR ALTER PROCEDURE BajaVacunacion
    @idVacunacion int
AS
    DELETE FROM Vacunacion WHERE IdVacunacion = @idVacunacion;
GO
CREATE OR ALTER PROCEDURE ListarVacunaciones
AS
    SELECT v.IdVacunacion, v.Nombre, v.Cantidad, v.IdCorral, c.NroCorral, v.FechaAplicacion, v.ProximaAplicacion
    FROM Vacunacion v
    INNER JOIN Corral c ON c.IdCorral = v.IdCorral
    ORDER BY v.FechaAplicacion DESC, c.NroCorral;
GO
-- Busca por nombre de la vacuna.
CREATE OR ALTER PROCEDURE BuscarVacunacion
    @nombre varchar(100)
AS
    SELECT v.IdVacunacion, v.Nombre, v.Cantidad, v.IdCorral, c.NroCorral, v.FechaAplicacion, v.ProximaAplicacion
    FROM Vacunacion v
    INNER JOIN Corral c ON c.IdCorral = v.IdCorral
    WHERE v.Nombre LIKE '%' + @nombre + '%'
    ORDER BY v.FechaAplicacion DESC, c.NroCorral;
GO


-- =====================  VENTA  =====================

-- Cada gallina vendida queda con el Id de su venta (si se anula la venta, vuelve a estar activa).
IF COL_LENGTH('dbo.Gallina', 'IdVenta') IS NULL
    ALTER TABLE dbo.Gallina ADD IdVenta int NULL
        CONSTRAINT FK_Gallina_Venta REFERENCES dbo.Venta (IdVenta);
GO

-- Si es una venta de gallinas, da de baja las primeras N gallinas activas del tipo, raza y sexo.
-- Si no hay suficientes, el THROW cancela todo (Acceso.Escribir hace rollback de la transacción).
-- Devuelve en @idVenta (OUTPUT) el número de la venta nueva, para emitir el comprobante.
CREATE OR ALTER PROCEDURE AltaVenta
    @fecha date, @idTipoProducto int, @cantidad int, @precioUnitario decimal(10,2), @idCliente int,
    @idTipoGallina int, @idRaza int, @sexo char(1), @idVenta int OUTPUT
AS
BEGIN
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

-- Solo se corrige fecha, cliente y precio: el producto y la cantidad no se cambian
-- (para eso se anula la venta y se carga de nuevo).
CREATE OR ALTER PROCEDURE ModificarVenta
    @idVenta int, @fecha date, @idCliente int, @precioUnitario decimal(10,2)
AS
    UPDATE Venta SET Fecha = @fecha, IdCliente = @idCliente, PrecioUnitario = @precioUnitario
    WHERE IdVenta = @idVenta;
GO

-- Anula la venta: las gallinas vendidas vuelven a estar activas.
CREATE OR ALTER PROCEDURE BajaVenta
    @idVenta int
AS
BEGIN
    UPDATE Gallina SET Activo = 1, IdVenta = NULL WHERE IdVenta = @idVenta;
    DELETE FROM Venta WHERE IdVenta = @idVenta;
END
GO

CREATE OR ALTER PROCEDURE ListarVentas
AS
    SELECT v.IdVenta, v.Fecha, v.IdTipoProducto, tp.Descripcion AS TipoProducto, v.Cantidad, v.PrecioUnitario,
           v.Total, v.IdCliente, cl.Apellido + ', ' + cl.Nombre AS Cliente,
           v.IdTipoGallina, tg.Descripcion AS TipoGallina, v.IdRaza, r.NombreRaza AS Raza, v.Sexo
    FROM Venta v
    INNER JOIN TipoProducto tp ON tp.IdTipoProducto = v.IdTipoProducto
    LEFT JOIN Cliente cl       ON cl.IdCliente = v.IdCliente
    LEFT JOIN TipoGallina tg   ON tg.IdTipoGallina = v.IdTipoGallina
    LEFT JOIN Raza r           ON r.IdRaza = v.IdRaza
    ORDER BY v.Fecha DESC, v.IdVenta DESC;
GO
-- Filtros opcionales (NULL = sin filtro): día, producto y cliente.
CREATE OR ALTER PROCEDURE BuscarVenta
    @fecha date, @idTipoProducto int, @idCliente int
AS
    SELECT v.IdVenta, v.Fecha, v.IdTipoProducto, tp.Descripcion AS TipoProducto, v.Cantidad, v.PrecioUnitario,
           v.Total, v.IdCliente, cl.Apellido + ', ' + cl.Nombre AS Cliente,
           v.IdTipoGallina, tg.Descripcion AS TipoGallina, v.IdRaza, r.NombreRaza AS Raza, v.Sexo
    FROM Venta v
    INNER JOIN TipoProducto tp ON tp.IdTipoProducto = v.IdTipoProducto
    LEFT JOIN Cliente cl       ON cl.IdCliente = v.IdCliente
    LEFT JOIN TipoGallina tg   ON tg.IdTipoGallina = v.IdTipoGallina
    LEFT JOIN Raza r           ON r.IdRaza = v.IdRaza
    WHERE (@fecha IS NULL OR v.Fecha = @fecha)
      AND (@idTipoProducto IS NULL OR v.IdTipoProducto = @idTipoProducto)
      AND (@idCliente IS NULL OR v.IdCliente = @idCliente)
    ORDER BY v.Fecha DESC, v.IdVenta DESC;
GO

-- Gallinas activas que se pueden vender de un tipo, raza y sexo.
CREATE OR ALTER PROCEDURE ContarGallinasDisponibles
    @idTipoGallina int, @idRaza int, @sexo char(1)
AS
    SELECT COUNT(*) AS Disponibles
    FROM Gallina
    WHERE Activo = 1 AND IdTipoGallina = @idTipoGallina AND IdRaza = @idRaza AND Sexo = @sexo;
GO


-- =====================  COMPROBANTE  =====================
-- Datos para el comprobante de una venta (no fiscal, tipo "X").
-- N° de comprobante = punto de venta 0001 + número de venta con 8 dígitos (ej: 0001-00000042).
CREATE OR ALTER PROCEDURE ComprobanteVenta
    @idVenta int
AS
    SELECT v.IdVenta,
           '0001-' + RIGHT('00000000' + CAST(v.IdVenta AS varchar(8)), 8) AS NroComprobante,
           v.Fecha,
           ISNULL(cl.Apellido + ', ' + cl.Nombre, 'Consumidor final') AS Cliente,
           CASE WHEN v.IdTipoProducto = 2
                THEN 'Gallina ' + tg.Descripcion + ' ' + r.NombreRaza + CASE v.Sexo WHEN 'M' THEN ' (macho)' ELSE ' (hembra)' END
                ELSE tp.Descripcion
           END AS Descripcion,
           v.Cantidad, v.PrecioUnitario, v.Total
    FROM Venta v
    INNER JOIN TipoProducto tp ON tp.IdTipoProducto = v.IdTipoProducto
    LEFT JOIN Cliente cl       ON cl.IdCliente = v.IdCliente
    LEFT JOIN TipoGallina tg   ON tg.IdTipoGallina = v.IdTipoGallina
    LEFT JOIN Raza r           ON r.IdRaza = v.IdRaza
    WHERE v.IdVenta = @idVenta;
GO


-- =====================  Verificación  =====================
SELECT name AS Procedimiento, modify_date
FROM sys.procedures
WHERE name IN ('AltaVacunacion', 'ModificarVacunacion', 'BajaVacunacion', 'ListarVacunaciones', 'BuscarVacunacion',
               'AltaVenta', 'ModificarVenta', 'BajaVenta', 'ListarVentas', 'BuscarVenta', 'ContarGallinasDisponibles',
               'ComprobanteVenta')
ORDER BY name;
SELECT COL_LENGTH('dbo.Vacunacion', 'NroVacuna') AS NroVacuna_tiene_que_ser_NULL,
       COL_LENGTH('dbo.Gallina', 'IdVenta')      AS Gallina_IdVenta;
GO
