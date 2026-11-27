-- =============================================================
-- LRChickens - SP de ABM con los campos nuevos de los formularios
-- Ejecutar DESPUÉS de 01, 02 y 03. Reemplaza los SP de Proveedor, Usuario
-- y Corral de 02_ABM.sql y agrega los de Cliente y Ubicacion.
-- Se puede correr más de una vez (CREATE OR ALTER).
-- =============================================================


-- =====================  UBICACION  (catálogo, se carga a mano en la base)  =====================

CREATE OR ALTER PROCEDURE ListarUbicaciones
AS
    SELECT IdUbicacion, Nombre FROM Ubicacion ORDER BY Nombre;
GO


-- =====================  CORRAL  (TIPO, NUMERO, CAPACIDAD, UBICACION)  =====================

CREATE OR ALTER PROCEDURE AltaCorral
    @nroCorral varchar(10), @idTipoCorral int, @capacidad int, @idUbicacion int
AS
    INSERT INTO Corral (IdCorral, NroCorral, IdTipoCorral, Capacidad, IdUbicacion)
    VALUES ((SELECT ISNULL(MAX(IdCorral), 0) + 1 FROM Corral), @nroCorral, @idTipoCorral, @capacidad, @idUbicacion);
GO
CREATE OR ALTER PROCEDURE ModificarCorral
    @idCorral int, @nroCorral varchar(10), @idTipoCorral int, @capacidad int, @idUbicacion int
AS
    UPDATE Corral SET NroCorral = @nroCorral, IdTipoCorral = @idTipoCorral, Capacidad = @capacidad,
                      IdUbicacion = @idUbicacion
    WHERE IdCorral = @idCorral;
GO
-- La ubicación es opcional: LEFT JOIN para que también salgan los corrales sin ubicación.
CREATE OR ALTER PROCEDURE ListarCorrales
AS
    SELECT c.IdCorral, c.NroCorral, c.IdTipoCorral, tc.Descripcion AS TipoCorral, c.Capacidad,
           c.IdUbicacion, u.Nombre AS Ubicacion
    FROM Corral c
    INNER JOIN TipoCorral tc ON tc.IdTipoCorral = c.IdTipoCorral
    LEFT JOIN Ubicacion u ON u.IdUbicacion = c.IdUbicacion
    ORDER BY c.NroCorral;
GO
CREATE OR ALTER PROCEDURE BuscarCorral
    @nroCorral varchar(10)
AS
    SELECT c.IdCorral, c.NroCorral, c.IdTipoCorral, tc.Descripcion AS TipoCorral, c.Capacidad,
           c.IdUbicacion, u.Nombre AS Ubicacion
    FROM Corral c
    INNER JOIN TipoCorral tc ON tc.IdTipoCorral = c.IdTipoCorral
    LEFT JOIN Ubicacion u ON u.IdUbicacion = c.IdUbicacion
    WHERE c.NroCorral LIKE '%' + @nroCorral + '%'
    ORDER BY c.NroCorral;
GO


-- =====================  PROVEEDOR  (CargarProveedor)  =====================
-- ESTADO (Activo) se elige en el formulario. Baja sigue siendo DELETE
-- (falla por FK si tiene alimentos; en ese caso se lo pone INACTIVO).

CREATE OR ALTER PROCEDURE AltaProveedor
    @nombre varchar(100), @cuil varchar(13), @direccion varchar(150), @localidad varchar(50),
    @telefono varchar(20), @email varchar(100), @servicioProducto varchar(100), @activo bit
AS
    INSERT INTO Proveedor (IdProveedor, Nombre, CUIL, Direccion, Localidad, Telefono, Email, ServicioProducto, Activo)
    VALUES ((SELECT ISNULL(MAX(IdProveedor), 0) + 1 FROM Proveedor), @nombre, @cuil, @direccion, @localidad,
            @telefono, @email, @servicioProducto, @activo);
GO
CREATE OR ALTER PROCEDURE ModificarProveedor
    @idProveedor int, @nombre varchar(100), @cuil varchar(13), @direccion varchar(150), @localidad varchar(50),
    @telefono varchar(20), @email varchar(100), @servicioProducto varchar(100), @activo bit
AS
    UPDATE Proveedor SET Nombre = @nombre, CUIL = @cuil, Direccion = @direccion, Localidad = @localidad,
                         Telefono = @telefono, Email = @email, ServicioProducto = @servicioProducto, Activo = @activo
    WHERE IdProveedor = @idProveedor;
GO
-- Devuelven activos e inactivos (el formulario muestra el estado).
CREATE OR ALTER PROCEDURE ListarProveedores
AS
    SELECT IdProveedor, Nombre, CUIL, Direccion, Localidad, Telefono, Email, ServicioProducto, Activo
    FROM Proveedor
    ORDER BY Nombre;
GO
CREATE OR ALTER PROCEDURE BuscarProveedor
    @nombre varchar(100)
AS
    SELECT IdProveedor, Nombre, CUIL, Direccion, Localidad, Telefono, Email, ServicioProducto, Activo
    FROM Proveedor
    WHERE Nombre LIKE '%' + @nombre + '%'
    ORDER BY Nombre;
GO


-- =====================  USUARIO  (UsuarioNuevo)  =====================

CREATE OR ALTER PROCEDURE AltaUsuario
    @nombreUsuario varchar(50), @contrasena varchar(256), @idRol int,
    @nombre varchar(50), @apellido varchar(50), @dni varchar(10), @fechaNacimiento date,
    @telefono varchar(20), @email varchar(100)
AS
    INSERT INTO Usuario (IdUsuario, NombreUsuario, Contrasena, IdRol, Activo,
                         Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email)
    VALUES ((SELECT ISNULL(MAX(IdUsuario), 0) + 1 FROM Usuario), @nombreUsuario, @contrasena, @idRol, 1,
            @nombre, @apellido, @dni, @fechaNacimiento, @telefono, @email);
GO
CREATE OR ALTER PROCEDURE ModificarUsuario
    @idUsuario int, @nombreUsuario varchar(50), @contrasena varchar(256), @idRol int,
    @nombre varchar(50), @apellido varchar(50), @dni varchar(10), @fechaNacimiento date,
    @telefono varchar(20), @email varchar(100)
AS
    UPDATE Usuario SET NombreUsuario = @nombreUsuario, Contrasena = @contrasena, IdRol = @idRol,
                       Nombre = @nombre, Apellido = @apellido, DNI = @dni, FechaNacimiento = @fechaNacimiento,
                       Telefono = @telefono, Email = @email
    WHERE IdUsuario = @idUsuario;
GO
-- No devuelven la contraseña.
CREATE OR ALTER PROCEDURE ListarUsuarios
AS
    SELECT u.IdUsuario, u.NombreUsuario, u.IdRol, r.Descripcion AS Rol, u.Activo,
           u.Nombre, u.Apellido, u.DNI, u.FechaNacimiento, u.Telefono, u.Email
    FROM Usuario u
    INNER JOIN Rol r ON r.IdRol = u.IdRol
    WHERE u.Activo = 1
    ORDER BY u.NombreUsuario;
GO
CREATE OR ALTER PROCEDURE BuscarUsuario
    @nombreUsuario varchar(50)
AS
    SELECT u.IdUsuario, u.NombreUsuario, u.IdRol, r.Descripcion AS Rol, u.Activo,
           u.Nombre, u.Apellido, u.DNI, u.FechaNacimiento, u.Telefono, u.Email
    FROM Usuario u
    INNER JOIN Rol r ON r.IdRol = u.IdRol
    WHERE u.Activo = 1
      AND u.NombreUsuario LIKE '%' + @nombreUsuario + '%'
    ORDER BY u.NombreUsuario;
GO


-- =====================  CLIENTE  (combo CLIENTE de Vender, VerClientes)  =====================
-- Baja lógica (Activo = 0): un cliente con ventas no se puede borrar.

CREATE OR ALTER PROCEDURE AltaCliente
    @nombre varchar(50), @apellido varchar(50), @telefono varchar(20), @email varchar(100), @direccion varchar(150)
AS
    INSERT INTO Cliente (IdCliente, Nombre, Apellido, Telefono, Email, Direccion, Activo)
    VALUES ((SELECT ISNULL(MAX(IdCliente), 0) + 1 FROM Cliente), @nombre, @apellido, @telefono, @email, @direccion, 1);
GO
CREATE OR ALTER PROCEDURE ModificarCliente
    @idCliente int, @nombre varchar(50), @apellido varchar(50), @telefono varchar(20), @email varchar(100),
    @direccion varchar(150)
AS
    UPDATE Cliente SET Nombre = @nombre, Apellido = @apellido, Telefono = @telefono, Email = @email,
                       Direccion = @direccion
    WHERE IdCliente = @idCliente;
GO
CREATE OR ALTER PROCEDURE BajaCliente
    @idCliente int
AS
    UPDATE Cliente SET Activo = 0 WHERE IdCliente = @idCliente;
GO
CREATE OR ALTER PROCEDURE ListarClientes
AS
    SELECT IdCliente, Nombre, Apellido, Telefono, Email, Direccion, Activo
    FROM Cliente
    WHERE Activo = 1
    ORDER BY Apellido, Nombre;
GO
-- Busca por nombre o apellido.
CREATE OR ALTER PROCEDURE BuscarCliente
    @texto varchar(50)
AS
    SELECT IdCliente, Nombre, Apellido, Telefono, Email, Direccion, Activo
    FROM Cliente
    WHERE Activo = 1
      AND (Nombre LIKE '%' + @texto + '%' OR Apellido LIKE '%' + @texto + '%')
    ORDER BY Apellido, Nombre;
GO


-- =====================  Verificación  =====================
SELECT name AS Procedimiento, modify_date
FROM sys.procedures
WHERE name IN ('ListarUbicaciones', 'AltaCorral', 'ModificarCorral', 'ListarCorrales', 'BuscarCorral',
               'AltaProveedor', 'ModificarProveedor', 'ListarProveedores', 'BuscarProveedor',
               'AltaUsuario', 'ModificarUsuario', 'ListarUsuarios', 'BuscarUsuario',
               'AltaCliente', 'ModificarCliente', 'BajaCliente', 'ListarClientes', 'BuscarCliente')
ORDER BY name;
GO
