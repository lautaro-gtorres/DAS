-- =============================================================
-- LRChickens - Login
-- Ejecutar sobre la base DBPollos (SSMS o editor de consultas de Azure).
-- Se puede correr más de una vez sin romper nada.
-- Login simple: usuario y contraseña guardados tal cual en la tabla Usuario.
-- =============================================================

-- Devuelve el usuario (con su rol) si el nombre y la contraseña coinciden y está activo.
CREATE OR ALTER PROCEDURE dbo.SP_ValidarUsuario
    @Usuario  VARCHAR(50),
    @Password VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.IdUsuario,
           u.NombreUsuario,
           u.IdRol,
           r.Descripcion AS Rol,
           u.Activo
    FROM dbo.Usuario u
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.NombreUsuario = @Usuario
      AND u.Contrasena = @Password
      AND u.Activo = 1;
END
GO

-- Usuario administrador inicial: admin / 1122
-- Los Id no son IDENTITY: se calculan a mano con MAX + 1.
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO dbo.Usuario (IdUsuario, NombreUsuario, Contrasena, IdRol, Activo)
    SELECT ISNULL(MAX(IdUsuario), 0) + 1,
           'admin',
           '1122',
           (SELECT IdRol FROM dbo.Rol WHERE Descripcion = 'Administrador'),
           1
    FROM dbo.Usuario;
END
ELSE
    -- Si quedó el admin con la contraseña hasheada de la versión anterior, se pasa a texto plano.
    UPDATE dbo.Usuario SET Contrasena = '1122'
    WHERE NombreUsuario = 'admin' AND Contrasena LIKE 'PBKDF2$%';
GO
