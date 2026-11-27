-- =============================================================
-- LRChickens - Datos de ejemplo para probar la aplicación
-- Ejecutar sobre DBPollos después de 01 a 07. Se puede correr más de una vez:
-- cada bloque se carga solo si todavía no está.
-- Usa los mismos SP de alta que la aplicación (los Id se calculan solos).
--
-- Deja cargado: ubicaciones, razas, corrales, proveedores, alimentos (con stock),
-- clientes, gallinas, producción de la última semana, vacunaciones (con agenda),
-- una compra y dos ventas de huevos.
-- =============================================================

SET NOCOUNT ON;

-- =====================  UBICACIONES  =====================
IF NOT EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = 'Galpón Norte')
    INSERT INTO Ubicacion (IdUbicacion, Nombre) VALUES ((SELECT ISNULL(MAX(IdUbicacion), 0) + 1 FROM Ubicacion), 'Galpón Norte');
IF NOT EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = 'Galpón Sur')
    INSERT INTO Ubicacion (IdUbicacion, Nombre) VALUES ((SELECT ISNULL(MAX(IdUbicacion), 0) + 1 FROM Ubicacion), 'Galpón Sur');
IF NOT EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = 'Campo Este')
    INSERT INTO Ubicacion (IdUbicacion, Nombre) VALUES ((SELECT ISNULL(MAX(IdUbicacion), 0) + 1 FROM Ubicacion), 'Campo Este');
GO

-- =====================  RAZAS  =====================
IF NOT EXISTS (SELECT 1 FROM Raza WHERE NombreRaza = 'Rhode Island Red') EXEC AltaRaza @nombreRaza = 'Rhode Island Red';
IF NOT EXISTS (SELECT 1 FROM Raza WHERE NombreRaza = 'Leghorn')          EXEC AltaRaza @nombreRaza = 'Leghorn';
IF NOT EXISTS (SELECT 1 FROM Raza WHERE NombreRaza = 'Plymouth Rock')    EXEC AltaRaza @nombreRaza = 'Plymouth Rock';
IF NOT EXISTS (SELECT 1 FROM Raza WHERE NombreRaza = 'Sussex')           EXEC AltaRaza @nombreRaza = 'Sussex';
IF NOT EXISTS (SELECT 1 FROM Raza WHERE NombreRaza = 'Cobb 500')         EXEC AltaRaza @nombreRaza = 'Cobb 500';
GO

-- =====================  CORRALES  =====================
-- TipoCorral: 1 = Ponedoras, 2 = Engorde, 3 = Cria
DECLARE @norte int = (SELECT IdUbicacion FROM Ubicacion WHERE Nombre = 'Galpón Norte');
DECLARE @sur   int = (SELECT IdUbicacion FROM Ubicacion WHERE Nombre = 'Galpón Sur');
DECLARE @este  int = (SELECT IdUbicacion FROM Ubicacion WHERE Nombre = 'Campo Este');
IF NOT EXISTS (SELECT 1 FROM Corral WHERE NroCorral = 'C-01') EXEC AltaCorral @nroCorral = 'C-01', @idTipoCorral = 1, @capacidad = 50,  @idUbicacion = @norte;
IF NOT EXISTS (SELECT 1 FROM Corral WHERE NroCorral = 'C-02') EXEC AltaCorral @nroCorral = 'C-02', @idTipoCorral = 1, @capacidad = 50,  @idUbicacion = @norte;
IF NOT EXISTS (SELECT 1 FROM Corral WHERE NroCorral = 'C-03') EXEC AltaCorral @nroCorral = 'C-03', @idTipoCorral = 2, @capacidad = 80,  @idUbicacion = @sur;
IF NOT EXISTS (SELECT 1 FROM Corral WHERE NroCorral = 'C-04') EXEC AltaCorral @nroCorral = 'C-04', @idTipoCorral = 3, @capacidad = 100, @idUbicacion = @este;
GO

-- =====================  PROVEEDORES  =====================
IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = 'Agroinsumos del Oeste')
    EXEC AltaProveedor @nombre = 'Agroinsumos del Oeste', @cuil = '30-71234560-1', @direccion = 'Av. San Martín 1250',
         @localidad = 'Mercedes', @telefono = '02324-421100', @email = 'ventas@agroinsumos.com.ar',
         @servicioProducto = 'Alimento balanceado', @activo = 1;
IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = 'Molinos San Jorge')
    EXEC AltaProveedor @nombre = 'Molinos San Jorge', @cuil = '30-70987654-3', @direccion = 'Ruta 5 km 98',
         @localidad = 'Suipacha', @telefono = '02324-480200', @email = 'pedidos@molinossanjorge.com.ar',
         @servicioProducto = 'Maíz y cereales', @activo = 1;
IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = 'Veterinaria El Corral')
    EXEC AltaProveedor @nombre = 'Veterinaria El Corral', @cuil = '20-28765432-9', @direccion = 'Calle 24 n° 530',
         @localidad = 'Mercedes', @telefono = '02324-433344', @email = 'elcorral.vet@gmail.com',
         @servicioProducto = 'Vacunas y sanidad', @activo = 1;
GO

-- =====================  ALIMENTOS  =====================
DECLARE @agro    int = (SELECT IdProveedor FROM Proveedor WHERE Nombre = 'Agroinsumos del Oeste');
DECLARE @molinos int = (SELECT IdProveedor FROM Proveedor WHERE Nombre = 'Molinos San Jorge');
IF NOT EXISTS (SELECT 1 FROM Alimento WHERE NombreAlimento = 'Maíz partido')
    EXEC AltaAlimento @nombreAlimento = 'Maíz partido', @kilogramos = 500, @idProveedor = @molinos;
IF NOT EXISTS (SELECT 1 FROM Alimento WHERE NombreAlimento = 'Balanceado ponedoras')
    EXEC AltaAlimento @nombreAlimento = 'Balanceado ponedoras', @kilogramos = 300, @idProveedor = @agro;
IF NOT EXISTS (SELECT 1 FROM Alimento WHERE NombreAlimento = 'Balanceado engorde')
    EXEC AltaAlimento @nombreAlimento = 'Balanceado engorde', @kilogramos = 200, @idProveedor = @agro;
IF NOT EXISTS (SELECT 1 FROM Alimento WHERE NombreAlimento = 'Iniciador pollitos')
    EXEC AltaAlimento @nombreAlimento = 'Iniciador pollitos', @kilogramos = 80, @idProveedor = @agro;
GO

-- =====================  CLIENTES  =====================
IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Apellido = 'Pereyra' AND Nombre = 'Martín')
    EXEC AltaCliente @nombre = 'Martín', @apellido = 'Pereyra', @telefono = '11-4567-8901', @email = 'mpereyra@gmail.com', @direccion = 'Belgrano 340, Mercedes';
IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Apellido = 'Benítez' AND Nombre = 'Laura')
    EXEC AltaCliente @nombre = 'Laura', @apellido = 'Benítez', @telefono = '11-5123-4455', @email = 'laura.benitez@hotmail.com', @direccion = 'Rivadavia 1022, Mercedes';
IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Apellido = 'Ruiz' AND Nombre = 'Carlos')
    EXEC AltaCliente @nombre = 'Carlos', @apellido = 'Ruiz', @telefono = '02324-15-667788', @email = NULL, @direccion = 'Almacén Ruiz, Calle 9 n° 77';
IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Apellido = 'Molina' AND Nombre = 'Sofía')
    EXEC AltaCliente @nombre = 'Sofía', @apellido = 'Molina', @telefono = '11-3344-2211', @email = 'sofi.molina@gmail.com', @direccion = NULL;
GO

-- =====================  GALLINAS  =====================
-- TipoGallina: 1 = Ponedora, 2 = Engorde, 3 = Gallo
DECLARE @c01 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-01');
DECLARE @c02 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-02');
DECLARE @c03 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-03');
DECLARE @rir     int = (SELECT IdRaza FROM Raza WHERE NombreRaza = 'Rhode Island Red');
DECLARE @leghorn int = (SELECT IdRaza FROM Raza WHERE NombreRaza = 'Leghorn');
DECLARE @cobb    int = (SELECT IdRaza FROM Raza WHERE NombreRaza = 'Cobb 500');
DECLARE @i int, @cod varchar(20), @nac date;

-- 12 ponedoras Rhode Island Red en C-01
SET @i = 1;
WHILE @i <= 12
BEGIN
    SET @cod = 'RIR-' + RIGHT('000' + CAST(@i AS varchar(3)), 3);
    SET @nac = DATEADD(MONTH, -8, CAST(GETDATE() AS date));
    IF NOT EXISTS (SELECT 1 FROM Gallina WHERE CodGallina = @cod)
        EXEC AltaGallina @codGallina = @cod, @idRaza = @rir, @idTipoGallina = 1, @sexo = 'H',
             @fechaNacimiento = @nac, @idCorral = @c01, @detalle = NULL;
    SET @i = @i + 1;
END

-- 1 gallo Rhode Island Red en C-01
IF NOT EXISTS (SELECT 1 FROM Gallina WHERE CodGallina = 'GALLO-001')
    EXEC AltaGallina @codGallina = 'GALLO-001', @idRaza = @rir, @idTipoGallina = 3, @sexo = 'M',
         @fechaNacimiento = NULL, @idCorral = @c01, @detalle = 'Gallo reproductor';

-- 10 ponedoras Leghorn en C-02
SET @i = 1;
WHILE @i <= 10
BEGIN
    SET @cod = 'LEG-' + RIGHT('000' + CAST(@i AS varchar(3)), 3);
    SET @nac = DATEADD(MONTH, -6, CAST(GETDATE() AS date));
    IF NOT EXISTS (SELECT 1 FROM Gallina WHERE CodGallina = @cod)
        EXEC AltaGallina @codGallina = @cod, @idRaza = @leghorn, @idTipoGallina = 1, @sexo = 'H',
             @fechaNacimiento = @nac, @idCorral = @c02, @detalle = NULL;
    SET @i = @i + 1;
END

-- 8 pollos de engorde Cobb 500 en C-03 (4 machos y 4 hembras), para vender
SET @i = 1;
WHILE @i <= 8
BEGIN
    SET @cod = 'COBB-' + RIGHT('000' + CAST(@i AS varchar(3)), 3);
    SET @nac = DATEADD(DAY, -40, CAST(GETDATE() AS date));
    IF NOT EXISTS (SELECT 1 FROM Gallina WHERE CodGallina = @cod)
    BEGIN
        IF @i <= 4
            EXEC AltaGallina @codGallina = @cod, @idRaza = @cobb, @idTipoGallina = 2, @sexo = 'M',
                 @fechaNacimiento = @nac, @idCorral = @c03, @detalle = NULL;
        ELSE
            EXEC AltaGallina @codGallina = @cod, @idRaza = @cobb, @idTipoGallina = 2, @sexo = 'H',
                 @fechaNacimiento = @nac, @idCorral = @c03, @detalle = NULL;
    END
    SET @i = @i + 1;
END
GO

-- =====================  PRODUCCIÓN (últimos 7 días)  =====================
DECLARE @c01 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-01');
DECLARE @c02 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-02');
DECLARE @dia int = 1, @fecha date;
WHILE @dia <= 7
BEGIN
    SET @fecha = DATEADD(DAY, -@dia, CAST(GETDATE() AS date));
    IF NOT EXISTS (SELECT 1 FROM Produccion WHERE IdCorral = @c01 AND Fecha = @fecha)
        EXEC AltaProduccion @idCorral = @c01, @fecha = @fecha, @cantidadHuevos = 10;
    IF NOT EXISTS (SELECT 1 FROM Produccion WHERE IdCorral = @c02 AND Fecha = @fecha)
        EXEC AltaProduccion @idCorral = @c02, @fecha = @fecha, @cantidadHuevos = 8;
    SET @dia = @dia + 1;
END
GO

-- =====================  VACUNACIONES (con próxima aplicación para la Agenda)  =====================
DECLARE @hoy date = CAST(GETDATE() AS date);
DECLARE @c01 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-01');
DECLARE @c03 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-03');
DECLARE @c04 int = (SELECT IdCorral FROM Corral WHERE NroCorral = 'C-04');
DECLARE @f1 date = DATEADD(DAY, -20, @hoy), @p1 date = DATEADD(DAY, 10, @hoy);
DECLARE @f2 date = DATEADD(DAY, -5, @hoy),  @p2 date = DATEADD(DAY, 25, @hoy);
DECLARE @f3 date = DATEADD(DAY, -30, @hoy), @p3 date = DATEADD(DAY, 60, @hoy);
IF NOT EXISTS (SELECT 1 FROM Vacunacion WHERE Nombre = 'Newcastle' AND IdCorral = @c01)
    EXEC AltaVacunacion @nombre = 'Newcastle', @cantidad = 13, @idCorral = @c01, @fechaAplicacion = @f1, @proximaAplicacion = @p1;
IF NOT EXISTS (SELECT 1 FROM Vacunacion WHERE Nombre = 'Gumboro' AND IdCorral = @c03)
    EXEC AltaVacunacion @nombre = 'Gumboro', @cantidad = 8, @idCorral = @c03, @fechaAplicacion = @f2, @proximaAplicacion = @p2;
IF NOT EXISTS (SELECT 1 FROM Vacunacion WHERE Nombre = 'Marek' AND IdCorral = @c04)
    EXEC AltaVacunacion @nombre = 'Marek', @cantidad = 20, @idCorral = @c04, @fechaAplicacion = @f3, @proximaAplicacion = @p3;
GO

-- =====================  UNA COMPRA Y DOS VENTAS  =====================
DECLARE @hoy date = CAST(GETDATE() AS date);
DECLARE @maiz int    = (SELECT IdAlimento FROM Alimento WHERE NombreAlimento = 'Maíz partido');
DECLARE @molinos int = (SELECT IdProveedor FROM Proveedor WHERE Nombre = 'Molinos San Jorge');
DECLARE @fc date = DATEADD(DAY, -3, @hoy);
IF NOT EXISTS (SELECT 1 FROM Compra WHERE IdAlimento = @maiz)
    EXEC AltaCompra @fecha = @fc, @idAlimento = @maiz, @idProveedor = @molinos, @kilogramos = 200, @precioUnitario = 180;

DECLARE @pereyra int = (SELECT IdCliente FROM Cliente WHERE Apellido = 'Pereyra' AND Nombre = 'Martín');
DECLARE @ruiz int    = (SELECT IdCliente FROM Cliente WHERE Apellido = 'Ruiz' AND Nombre = 'Carlos');
DECLARE @idVenta int, @fv1 date = DATEADD(DAY, -2, @hoy), @fv2 date = DATEADD(DAY, -1, @hoy);
IF NOT EXISTS (SELECT 1 FROM Venta WHERE IdCliente = @pereyra)
    EXEC AltaVenta @fecha = @fv1, @idTipoProducto = 1, @cantidad = 30, @precioUnitario = 150, @idCliente = @pereyra,
         @idTipoGallina = NULL, @idRaza = NULL, @sexo = NULL, @idVenta = @idVenta OUTPUT;
IF NOT EXISTS (SELECT 1 FROM Venta WHERE IdCliente = @ruiz)
    EXEC AltaVenta @fecha = @fv2, @idTipoProducto = 1, @cantidad = 24, @precioUnitario = 140, @idCliente = @ruiz,
         @idTipoGallina = NULL, @idRaza = NULL, @sexo = NULL, @idVenta = @idVenta OUTPUT;
GO

-- =====================  Verificación  =====================
SELECT 'Ubicaciones' AS Tabla, COUNT(*) AS Filas FROM Ubicacion
UNION ALL SELECT 'Razas', COUNT(*) FROM Raza
UNION ALL SELECT 'Corrales', COUNT(*) FROM Corral
UNION ALL SELECT 'Proveedores', COUNT(*) FROM Proveedor
UNION ALL SELECT 'Alimentos', COUNT(*) FROM Alimento
UNION ALL SELECT 'Clientes', COUNT(*) FROM Cliente
UNION ALL SELECT 'Gallinas activas', COUNT(*) FROM Gallina WHERE Activo = 1
UNION ALL SELECT 'Producción', COUNT(*) FROM Produccion
UNION ALL SELECT 'Vacunaciones', COUNT(*) FROM Vacunacion
UNION ALL SELECT 'Compras', COUNT(*) FROM Compra
UNION ALL SELECT 'Ventas', COUNT(*) FROM Venta;
GO
