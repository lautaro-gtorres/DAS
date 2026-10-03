/* =====================================================================
   Sistema Bancario - Cambios de esquema + Stored Procedures
   Ejecutar conectado a la base gestion-bancaria (Azure SQL no admite USE).
   Se puede correr varias veces: los cambios de esquema se saltean si ya
   existen y los SP usan CREATE OR ALTER.
   ===================================================================== */

/* ---------------------------------------------------------------------
   1. Cambios de esquema
   Esquema actual: usuario / movimientos / transferencias, sin IDENTITY,
   sin claves foráneas. movimientos ya tiene idUsuario e idTransferencia.
   --------------------------------------------------------------------- */

-- usuario.tipo (Administrador / Cliente) - lo pide el punto 1 del enunciado
IF COL_LENGTH('dbo.usuario', 'tipo') IS NULL
    ALTER TABLE dbo.usuario ADD tipo nvarchar(20) NOT NULL
        CONSTRAINT DF_usuario_tipo DEFAULT 'Cliente';
GO

-- movimientos.usuarioRelacionado (la contraparte, se muestra en el estado de cuenta)
-- Se agrega permitiendo NULL, se completa desde transferencias si ya hay filas
-- y después se vuelve obligatoria.
IF COL_LENGTH('dbo.movimientos', 'usuarioRelacionado') IS NULL
    ALTER TABLE dbo.movimientos ADD usuarioRelacionado int NULL;
GO
UPDATE m
SET usuarioRelacionado = CASE WHEN t.usuarioEmisor = m.idUsuario
                              THEN t.usuarioReceptor
                              ELSE t.usuarioEmisor END
FROM dbo.movimientos m
INNER JOIN dbo.transferencias t ON t.idTransferencia = m.idTransferencia
WHERE m.usuarioRelacionado IS NULL;
GO
IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.movimientos')
             AND name = 'usuarioRelacionado' AND is_nullable = 1)
   AND NOT EXISTS (SELECT 1 FROM dbo.movimientos WHERE usuarioRelacionado IS NULL)
    ALTER TABLE dbo.movimientos ALTER COLUMN usuarioRelacionado int NOT NULL;
GO

-- Usuario (login) único
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_usuario_usuario')
    ALTER TABLE dbo.usuario ADD CONSTRAINT UQ_usuario_usuario UNIQUE (usuario);
GO

-- Claves foráneas (integridad y trazabilidad, punto 5)
IF OBJECT_ID('FK_movimientos_usuario') IS NULL
    ALTER TABLE dbo.movimientos ADD CONSTRAINT FK_movimientos_usuario
        FOREIGN KEY (idUsuario) REFERENCES dbo.usuario (idUsuario);
IF OBJECT_ID('FK_movimientos_usuarioRelacionado') IS NULL
    ALTER TABLE dbo.movimientos ADD CONSTRAINT FK_movimientos_usuarioRelacionado
        FOREIGN KEY (usuarioRelacionado) REFERENCES dbo.usuario (idUsuario);
IF OBJECT_ID('FK_movimientos_transferencias') IS NULL
    ALTER TABLE dbo.movimientos ADD CONSTRAINT FK_movimientos_transferencias
        FOREIGN KEY (idTransferencia) REFERENCES dbo.transferencias (idTransferencia);
IF OBJECT_ID('FK_transferencias_emisor') IS NULL
    ALTER TABLE dbo.transferencias ADD CONSTRAINT FK_transferencias_emisor
        FOREIGN KEY (usuarioEmisor) REFERENCES dbo.usuario (idUsuario);
IF OBJECT_ID('FK_transferencias_receptor') IS NULL
    ALTER TABLE dbo.transferencias ADD CONSTRAINT FK_transferencias_receptor
        FOREIGN KEY (usuarioReceptor) REFERENCES dbo.usuario (idUsuario);
GO

/* ---------------------------------------------------------------------
   2. Usuarios (login + ABM)
   Los IDs se generan con MAX + 1 igual que en el TP1 (ninguna columna
   id es IDENTITY).
   Los SP de escritura no usan SET NOCOUNT ON a propósito, para que
   ExecuteNonQuery devuelva las filas afectadas (el "fa" del DAL).
   --------------------------------------------------------------------- */

CREATE OR ALTER PROCEDURE dbo.LoginUsuario
    @usuario nvarchar(30),
    @password nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    -- Devuelve también los inactivos: la BLL decide el mensaje ("usuario inactivo").
    SELECT idUsuario, nombre, apellido, dni, usuario, saldo, estado, tipo
    FROM dbo.usuario
    WHERE usuario = @usuario AND password = @password;
END
GO

CREATE OR ALTER PROCEDURE dbo.ListarUsuarios
AS
BEGIN
    SET NOCOUNT ON;
    -- No se devuelve la contraseña para no mostrarla en la grilla.
    SELECT idUsuario, nombre, apellido, dni, usuario, saldo, estado, tipo
    FROM dbo.usuario
    ORDER BY apellido, nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.BuscarUsuario
    @usuario nvarchar(30) = NULL,
    @dni nvarchar(10) = NULL,
    @apellido nvarchar(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idUsuario, nombre, apellido, dni, usuario, saldo, estado, tipo
    FROM dbo.usuario
    WHERE (@usuario IS NULL OR usuario LIKE '%' + @usuario + '%')
      AND (@dni IS NULL OR dni = @dni)
      AND (@apellido IS NULL OR apellido LIKE '%' + @apellido + '%')
    ORDER BY apellido, nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.ObtenerUsuarioPorId
    @idUsuario int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idUsuario, nombre, apellido, dni, usuario, saldo, estado, tipo
    FROM dbo.usuario
    WHERE idUsuario = @idUsuario;
END
GO

-- Para validar el destinatario de una transferencia (existe / activo / distinto)
CREATE OR ALTER PROCEDURE dbo.ObtenerUsuarioPorNombreUsuario
    @usuario nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idUsuario, nombre, apellido, dni, usuario, saldo, estado, tipo
    FROM dbo.usuario
    WHERE usuario = @usuario;
END
GO

CREATE OR ALTER PROCEDURE dbo.AltaUsuario
    @nombre nvarchar(50),
    @apellido nvarchar(50),
    @dni nvarchar(10),
    @usuario nvarchar(30),
    @password nvarchar(30),
    @tipo nvarchar(20),
    @estado bit,
    @saldo float = 0      -- saldo inicial (solo en el alta)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.usuario WHERE usuario = @usuario)
        THROW 50001, 'Ya existe un usuario con ese nombre de usuario.', 1;

    INSERT INTO dbo.usuario (idUsuario, nombre, apellido, dni, usuario, password, saldo, estado, tipo)
    VALUES ((SELECT ISNULL(MAX(idUsuario), 0) + 1 FROM dbo.usuario),
            @nombre, @apellido, @dni, @usuario, @password, @saldo, @estado, @tipo);
END
GO

-- No modifica el saldo (punto 5 del enunciado).
-- Si @password viene vacío o NULL se mantiene la contraseña actual.
CREATE OR ALTER PROCEDURE dbo.ModificarUsuario
    @idUsuario int,
    @nombre nvarchar(50),
    @apellido nvarchar(50),
    @dni nvarchar(10),
    @usuario nvarchar(30),
    @password nvarchar(30) = NULL,
    @tipo nvarchar(20),
    @estado bit
AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.usuario WHERE usuario = @usuario AND idUsuario <> @idUsuario)
        THROW 50002, 'Ya existe otro usuario con ese nombre de usuario.', 1;

    UPDATE dbo.usuario
    SET nombre   = @nombre,
        apellido = @apellido,
        dni      = @dni,
        usuario  = @usuario,
        password = CASE WHEN NULLIF(@password, '') IS NULL THEN password ELSE @password END,
        tipo     = @tipo,
        estado   = @estado
    WHERE idUsuario = @idUsuario;
END
GO

-- Baja lógica: un usuario con movimientos no se puede borrar físicamente (FK).
CREATE OR ALTER PROCEDURE dbo.BajaUsuario
    @idUsuario int
AS
BEGIN
    UPDATE dbo.usuario SET estado = 0 WHERE idUsuario = @idUsuario;
END
GO

/* ---------------------------------------------------------------------
   3. Transferencia - pasos individuales.
   Se llaman desde el DAL en este orden, todos dentro de la MISMA
   SqlTransaction (COMMIT si todo da OK, ROLLBACK si algo falla o si
   DebitarSaldo / AcreditarSaldo devuelven 0 filas afectadas):
     1) AltaTransferencia        -> devuelve @idTransferencia (OUTPUT)
     2) DebitarSaldo  (emisor)
     3) AcreditarSaldo (receptor)
     4) AltaMovimiento (emisor,   tipo 'Egreso')
     5) AltaMovimiento (receptor, tipo 'Ingreso')
   --------------------------------------------------------------------- */

CREATE OR ALTER PROCEDURE dbo.AltaTransferencia
    @usuarioEmisor int,
    @usuarioReceptor int,
    @importe float,
    @idTransferencia int OUTPUT
AS
BEGIN
    SET @idTransferencia = (SELECT ISNULL(MAX(idTransferencia), 0) + 1
                            FROM dbo.transferencias WITH (UPDLOCK, HOLDLOCK));

    INSERT INTO dbo.transferencias (idTransferencia, usuarioEmisor, usuarioReceptor, importe, fechaHora)
    VALUES (@idTransferencia, @usuarioEmisor, @usuarioReceptor, @importe, GETDATE());
END
GO

-- Devuelve 0 filas afectadas si no hay saldo suficiente o el usuario está inactivo
-- (segunda barrera además de la validación de la BLL).
CREATE OR ALTER PROCEDURE dbo.DebitarSaldo
    @idUsuario int,
    @importe float
AS
BEGIN
    UPDATE dbo.usuario
    SET saldo = saldo - @importe
    WHERE idUsuario = @idUsuario
      AND estado = 1
      AND saldo >= @importe;
END
GO

-- Devuelve 0 filas afectadas si el receptor no existe o está inactivo.
CREATE OR ALTER PROCEDURE dbo.AcreditarSaldo
    @idUsuario int,
    @importe float
AS
BEGIN
    UPDATE dbo.usuario
    SET saldo = saldo + @importe
    WHERE idUsuario = @idUsuario
      AND estado = 1;
END
GO

-- La fecha del movimiento se toma de la transferencia para que ambos lados coincidan.
CREATE OR ALTER PROCEDURE dbo.AltaMovimiento
    @idUsuario int,
    @tipo nvarchar(20),
    @importe float,
    @usuarioRelacionado int,
    @concepto nvarchar(50),
    @idTransferencia int
AS
BEGIN
    DECLARE @fechaHora datetime =
        ISNULL((SELECT fechaHora FROM dbo.transferencias WHERE idTransferencia = @idTransferencia), GETDATE());

    INSERT INTO dbo.movimientos (idMovimiento, fechaHora, tipo, importe, idUsuario, usuarioRelacionado, concepto, idTransferencia)
    VALUES ((SELECT ISNULL(MAX(idMovimiento), 0) + 1 FROM dbo.movimientos WITH (UPDLOCK, HOLDLOCK)),
            @fechaHora, @tipo, @importe, @idUsuario, @usuarioRelacionado, ISNULL(@concepto, ''), @idTransferencia);
END
GO

/* ---------------------------------------------------------------------
   4. Consultas
   --------------------------------------------------------------------- */

-- Estado de cuenta: solo los movimientos del usuario logueado (punto 4).
CREATE OR ALTER PROCEDURE dbo.ListarMovimientosPorUsuario
    @idUsuario int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.idMovimiento,
           m.fechaHora,
           m.tipo,
           m.importe,
           m.usuarioRelacionado,
           r.usuario AS usuarioRelacionadoNombre,
           m.concepto,
           m.idTransferencia
    FROM dbo.movimientos m
    INNER JOIN dbo.usuario r ON r.idUsuario = m.usuarioRelacionado
    WHERE m.idUsuario = @idUsuario
    ORDER BY m.fechaHora DESC, m.idMovimiento DESC;
END
GO

-- Para el administrador (trazabilidad de todas las operaciones).
CREATE OR ALTER PROCEDURE dbo.ListarTransferencias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.idTransferencia,
           t.fechaHora,
           t.usuarioEmisor,
           e.usuario AS usuarioEmisorNombre,
           t.usuarioReceptor,
           r.usuario AS usuarioReceptorNombre,
           t.importe
    FROM dbo.transferencias t
    INNER JOIN dbo.usuario e ON e.idUsuario = t.usuarioEmisor
    INNER JOIN dbo.usuario r ON r.idUsuario = t.usuarioReceptor
    ORDER BY t.fechaHora DESC;
END
GO
