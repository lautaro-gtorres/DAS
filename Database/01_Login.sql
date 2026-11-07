-- =============================================================
-- LRChickens - Login
-- Ejecutar sobre la base DBPollos (SSMS o editor de consultas de Azure).
-- Se puede correr más de una vez sin romper nada.
-- =============================================================

-- Devuelve el usuario con su rol y el hash de la contraseña.
-- La contraseña se valida en la BLL (BLL.Encriptador), no en SQL.
CREATE OR ALTER PROCEDURE dbo.sp_Usuario_ObtenerPorNombre
    @NombreUsuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.IdUsuario,
           u.NombreUsuario,
           u.Contrasena,
           u.IdRol,
           r.Descripcion AS Rol,
           u.Activo
    FROM dbo.Usuario u
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.NombreUsuario = @NombreUsuario;
END
GO

-- Usuario administrador inicial: admin / 1122
-- (la contraseña está guardada como hash PBKDF2; cambiarla cuando exista el ABM de usuarios).
-- IdUsuario se calcula a mano porque la columna todavía no es IDENTITY.
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO dbo.Usuario (IdUsuario, NombreUsuario, Contrasena, IdRol, Activo)
    SELECT ISNULL(MAX(IdUsuario), 0) + 1,
           'admin',
           'PBKDF2$10000$bJMH0KNFD4SpW1KqrJ6SBA==$m/wG9OAMnrCVfRwO2NPUxQchsCjAwpstNDUi1vXTOwg=',
           (SELECT IdRol FROM dbo.Rol WHERE Descripcion = 'Administrador'),
           1
    FROM dbo.Usuario;
END
GO
