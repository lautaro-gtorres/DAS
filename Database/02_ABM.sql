-- =============================================================
-- LRChickens - Stored procedures de ABM
-- Ejecutar sobre DBPollos (SSMS o editor de consultas de Azure).
-- Se puede correr más de una vez: todos son CREATE OR ALTER.
--
-- Convenciones:
--  * Los Id NO son IDENTITY: el alta calcula MAX(Id) + 1.
--  * Gallina y Usuario tienen baja lógica (Activo = 0); el resto, DELETE.
--  * Listar y Buscar de una tabla devuelven las mismas columnas,
--    con los mismos nombres que las propiedades del BE.
--  * Sin manejo de errores en SQL: los errores de FK / UNIQUE / CHECK
--    suben a Acceso.Escribir, que hace ROLLBACK y relanza.
-- =============================================================


-- =====================  CATÁLOGOS (para ComboBox)  =====================

CREATE OR ALTER PROCEDURE ListarTiposGallina
AS
    SELECT IdTipoGallina, Descripcion FROM TipoGallina ORDER BY Descripcion;
GO
CREATE OR ALTER PROCEDURE ListarTiposCorral
AS
    SELECT IdTipoCorral, Descripcion FROM TipoCorral ORDER BY Descripcion;
GO
CREATE OR ALTER PROCEDURE ListarRoles
AS
    SELECT IdRol, Descripcion FROM Rol ORDER BY Descripcion;
GO
CREATE OR ALTER PROCEDURE ListarTiposProducto
AS
    SELECT IdTipoProducto, Descripcion FROM TipoProducto ORDER BY Descripcion;
GO


-- =====================  RAZA  =====================

CREATE OR ALTER PROCEDURE AltaRaza
    @nombreRaza varchar(50)
AS
    INSERT INTO Raza (IdRaza, NombreRaza)
    VALUES ((SELECT ISNULL(MAX(IdRaza), 0) + 1 FROM Raza), @nombreRaza);
GO
CREATE OR ALTER PROCEDURE ModificarRaza
    @idRaza int, @nombreRaza varchar(50)
AS
    UPDATE Raza SET NombreRaza = @nombreRaza WHERE IdRaza = @idRaza;
GO
CREATE OR ALTER PROCEDURE BajaRaza
    @idRaza int
AS
    DELETE FROM Raza WHERE IdRaza = @idRaza;
GO
CREATE OR ALTER PROCEDURE ListarRazas
AS
    SELECT IdRaza, NombreRaza FROM Raza ORDER BY NombreRaza;
GO
CREATE OR ALTER PROCEDURE BuscarRaza
    @nombreRaza varchar(50)
AS
    SELECT IdRaza, NombreRaza FROM Raza
    WHERE NombreRaza LIKE '%' + @nombreRaza + '%'
    ORDER BY NombreRaza;
GO


-- =====================  CORRAL  =====================

CREATE OR ALTER PROCEDURE AltaCorral
    @nroCorral varchar(10), @idTipoCorral int, @capacidad int
AS
    INSERT INTO Corral (IdCorral, NroCorral, IdTipoCorral, Capacidad)
    VALUES ((SELECT ISNULL(MAX(IdCorral), 0) + 1 FROM Corral), @nroCorral, @idTipoCorral, @capacidad);
GO
CREATE OR ALTER PROCEDURE ModificarCorral
    @idCorral int, @nroCorral varchar(10), @idTipoCorral int, @capacidad int
AS
    UPDATE Corral SET NroCorral = @nroCorral, IdTipoCorral = @idTipoCorral, Capacidad = @capacidad
    WHERE IdCorral = @idCorral;
GO
-- Falla por FK si el corral tiene gallinas o producción cargada.
CREATE OR ALTER PROCEDURE BajaCorral
    @idCorral int
AS
    DELETE FROM Corral WHERE IdCorral = @idCorral;
GO
CREATE OR ALTER PROCEDURE ListarCorrales
AS
    SELECT c.IdCorral, c.NroCorral, c.IdTipoCorral, tc.Descripcion AS TipoCorral, c.Capacidad
    FROM Corral c
    INNER JOIN TipoCorral tc ON tc.IdTipoCorral = c.IdTipoCorral
    ORDER BY c.NroCorral;
GO
CREATE OR ALTER PROCEDURE BuscarCorral
    @nroCorral varchar(10)
AS
    SELECT c.IdCorral, c.NroCorral, c.IdTipoCorral, tc.Descripcion AS TipoCorral, c.Capacidad
    FROM Corral c
    INNER JOIN TipoCorral tc ON tc.IdTipoCorral = c.IdTipoCorral
    WHERE c.NroCorral LIKE '%' + @nroCorral + '%'
    ORDER BY c.NroCorral;
GO


-- =====================  GALLINA (baja lógica)  =====================

CREATE OR ALTER PROCEDURE AltaGallina
    @codGallina varchar(20), @idRaza int, @idTipoGallina int, @sexo char(1),
    @fechaNacimiento date, @idCorral int, @detalle varchar(500)
AS
    INSERT INTO Gallina (IdGallina, CodGallina, IdRaza, IdTipoGallina, Sexo, FechaNacimiento, IdCorral, Detalle, Activo)
    VALUES ((SELECT ISNULL(MAX(IdGallina), 0) + 1 FROM Gallina),
            @codGallina, @idRaza, @idTipoGallina, @sexo, @fechaNacimiento, @idCorral, @detalle, 1);
GO
CREATE OR ALTER PROCEDURE ModificarGallina
    @idGallina int, @codGallina varchar(20), @idRaza int, @idTipoGallina int, @sexo char(1),
    @fechaNacimiento date, @idCorral int, @detalle varchar(500)
AS
    UPDATE Gallina SET CodGallina = @codGallina, IdRaza = @idRaza, IdTipoGallina = @idTipoGallina,
           Sexo = @sexo, FechaNacimiento = @fechaNacimiento, IdCorral = @idCorral, Detalle = @detalle
    WHERE IdGallina = @idGallina;
GO
CREATE OR ALTER PROCEDURE BajaGallina
    @idGallina int
AS
    UPDATE Gallina SET Activo = 0 WHERE IdGallina = @idGallina;
GO
CREATE OR ALTER PROCEDURE ListarGallinas
AS
    SELECT g.IdGallina, g.CodGallina, g.IdRaza, r.NombreRaza AS Raza,
           g.IdTipoGallina, tg.Descripcion AS TipoGallina, g.Sexo, g.FechaNacimiento,
           g.IdCorral, c.NroCorral, g.Detalle, g.Activo
    FROM Gallina g
    INNER JOIN Raza r         ON r.IdRaza = g.IdRaza
    INNER JOIN TipoGallina tg ON tg.IdTipoGallina = g.IdTipoGallina
    INNER JOIN Corral c       ON c.IdCorral = g.IdCorral
    WHERE g.Activo = 1
    ORDER BY g.CodGallina;
GO
CREATE OR ALTER PROCEDURE BuscarGallina
    @codGallina varchar(20)
AS
    SELECT g.IdGallina, g.CodGallina, g.IdRaza, r.NombreRaza AS Raza,
           g.IdTipoGallina, tg.Descripcion AS TipoGallina, g.Sexo, g.FechaNacimiento,
           g.IdCorral, c.NroCorral, g.Detalle, g.Activo
    FROM Gallina g
    INNER JOIN Raza r         ON r.IdRaza = g.IdRaza
    INNER JOIN TipoGallina tg ON tg.IdTipoGallina = g.IdTipoGallina
    INNER JOIN Corral c       ON c.IdCorral = g.IdCorral
    WHERE g.Activo = 1
      AND g.CodGallina LIKE '%' + @codGallina + '%'
    ORDER BY g.CodGallina;
GO


-- =====================  PROVEEDOR  =====================

CREATE OR ALTER PROCEDURE AltaProveedor
    @nombre varchar(100), @telefono varchar(20), @direccion varchar(150)
AS
    INSERT INTO Proveedor (IdProveedor, Nombre, Telefono, Direccion)
    VALUES ((SELECT ISNULL(MAX(IdProveedor), 0) + 1 FROM Proveedor), @nombre, @telefono, @direccion);
GO
CREATE OR ALTER PROCEDURE ModificarProveedor
    @idProveedor int, @nombre varchar(100), @telefono varchar(20), @direccion varchar(150)
AS
    UPDATE Proveedor SET Nombre = @nombre, Telefono = @telefono, Direccion = @direccion
    WHERE IdProveedor = @idProveedor;
GO
-- Falla por FK si el proveedor tiene alimentos cargados.
CREATE OR ALTER PROCEDURE BajaProveedor
    @idProveedor int
AS
    DELETE FROM Proveedor WHERE IdProveedor = @idProveedor;
GO
CREATE OR ALTER PROCEDURE ListarProveedores
AS
    SELECT IdProveedor, Nombre, Telefono, Direccion FROM Proveedor ORDER BY Nombre;
GO
CREATE OR ALTER PROCEDURE BuscarProveedor
    @nombre varchar(100)
AS
    SELECT IdProveedor, Nombre, Telefono, Direccion FROM Proveedor
    WHERE Nombre LIKE '%' + @nombre + '%'
    ORDER BY Nombre;
GO


-- =====================  ALIMENTO  =====================
-- OJO: 03_CamposFormularios.sql renombra CantidadDisponible a Kilogramos y
-- reemplaza estos SP. Correr siempre 01, 02 y 03 en ese orden.

CREATE OR ALTER PROCEDURE AltaAlimento
    @nombreAlimento varchar(100), @cantidadDisponible decimal(10,2), @idProveedor int
AS
    INSERT INTO Alimento (IdAlimento, NombreAlimento, CantidadDisponible, IdProveedor)
    VALUES ((SELECT ISNULL(MAX(IdAlimento), 0) + 1 FROM Alimento), @nombreAlimento, @cantidadDisponible, @idProveedor);
GO
CREATE OR ALTER PROCEDURE ModificarAlimento
    @idAlimento int, @nombreAlimento varchar(100), @cantidadDisponible decimal(10,2), @idProveedor int
AS
    UPDATE Alimento SET NombreAlimento = @nombreAlimento, CantidadDisponible = @cantidadDisponible,
           IdProveedor = @idProveedor
    WHERE IdAlimento = @idAlimento;
GO
CREATE OR ALTER PROCEDURE BajaAlimento
    @idAlimento int
AS
    DELETE FROM Alimento WHERE IdAlimento = @idAlimento;
GO
CREATE OR ALTER PROCEDURE ListarAlimentos
AS
    SELECT a.IdAlimento, a.NombreAlimento, a.CantidadDisponible, a.IdProveedor, p.Nombre AS Proveedor
    FROM Alimento a
    INNER JOIN Proveedor p ON p.IdProveedor = a.IdProveedor
    ORDER BY a.NombreAlimento;
GO
CREATE OR ALTER PROCEDURE BuscarAlimento
    @nombreAlimento varchar(100)
AS
    SELECT a.IdAlimento, a.NombreAlimento, a.CantidadDisponible, a.IdProveedor, p.Nombre AS Proveedor
    FROM Alimento a
    INNER JOIN Proveedor p ON p.IdProveedor = a.IdProveedor
    WHERE a.NombreAlimento LIKE '%' + @nombreAlimento + '%'
    ORDER BY a.NombreAlimento;
GO


-- =====================  USUARIO (baja lógica, contraseña en texto plano)  =====================

CREATE OR ALTER PROCEDURE AltaUsuario
    @nombreUsuario varchar(50), @contrasena varchar(256), @idRol int
AS
    INSERT INTO Usuario (IdUsuario, NombreUsuario, Contrasena, IdRol, Activo)
    VALUES ((SELECT ISNULL(MAX(IdUsuario), 0) + 1 FROM Usuario), @nombreUsuario, @contrasena, @idRol, 1);
GO
CREATE OR ALTER PROCEDURE ModificarUsuario
    @idUsuario int, @nombreUsuario varchar(50), @contrasena varchar(256), @idRol int
AS
    UPDATE Usuario SET NombreUsuario = @nombreUsuario, Contrasena = @contrasena, IdRol = @idRol
    WHERE IdUsuario = @idUsuario;
GO
CREATE OR ALTER PROCEDURE BajaUsuario
    @idUsuario int
AS
    UPDATE Usuario SET Activo = 0 WHERE IdUsuario = @idUsuario;
GO
-- No devuelven la contraseña.
CREATE OR ALTER PROCEDURE ListarUsuarios
AS
    SELECT u.IdUsuario, u.NombreUsuario, u.IdRol, r.Descripcion AS Rol, u.Activo
    FROM Usuario u
    INNER JOIN Rol r ON r.IdRol = u.IdRol
    WHERE u.Activo = 1
    ORDER BY u.NombreUsuario;
GO
CREATE OR ALTER PROCEDURE BuscarUsuario
    @nombreUsuario varchar(50)
AS
    SELECT u.IdUsuario, u.NombreUsuario, u.IdRol, r.Descripcion AS Rol, u.Activo
    FROM Usuario u
    INNER JOIN Rol r ON r.IdRol = u.IdRol
    WHERE u.Activo = 1
      AND u.NombreUsuario LIKE '%' + @nombreUsuario + '%'
    ORDER BY u.NombreUsuario;
GO


-- =====================  PRODUCCION  =====================

CREATE OR ALTER PROCEDURE AltaProduccion
    @idCorral int, @fecha date, @cantidadHuevos int
AS
    INSERT INTO Produccion (IdProduccion, IdCorral, Fecha, CantidadHuevos)
    VALUES ((SELECT ISNULL(MAX(IdProduccion), 0) + 1 FROM Produccion), @idCorral, @fecha, @cantidadHuevos);
GO
CREATE OR ALTER PROCEDURE ModificarProduccion
    @idProduccion int, @idCorral int, @fecha date, @cantidadHuevos int
AS
    UPDATE Produccion SET IdCorral = @idCorral, Fecha = @fecha, CantidadHuevos = @cantidadHuevos
    WHERE IdProduccion = @idProduccion;
GO
CREATE OR ALTER PROCEDURE BajaProduccion
    @idProduccion int
AS
    DELETE FROM Produccion WHERE IdProduccion = @idProduccion;
GO
CREATE OR ALTER PROCEDURE ListarProducciones
AS
    SELECT p.IdProduccion, p.IdCorral, c.NroCorral, p.Fecha, p.CantidadHuevos
    FROM Produccion p
    INNER JOIN Corral c ON c.IdCorral = p.IdCorral
    ORDER BY p.Fecha DESC, c.NroCorral;
GO
-- Todos los filtros son opcionales: si vienen en NULL no filtran.
CREATE OR ALTER PROCEDURE BuscarProduccion
    @idCorral int = NULL, @fechaDesde date = NULL, @fechaHasta date = NULL
AS
    SELECT p.IdProduccion, p.IdCorral, c.NroCorral, p.Fecha, p.CantidadHuevos
    FROM Produccion p
    INNER JOIN Corral c ON c.IdCorral = p.IdCorral
    WHERE (@idCorral IS NULL OR p.IdCorral = @idCorral)
      AND (@fechaDesde IS NULL OR p.Fecha >= @fechaDesde)
      AND (@fechaHasta IS NULL OR p.Fecha <= @fechaHasta)
    ORDER BY p.Fecha DESC, c.NroCorral;
GO
