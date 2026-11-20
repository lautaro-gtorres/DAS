/* =====================================================================
   2do Parcial - Radio Taxi
   Ejecutar en la base Prueba (la del Acceso).
   Tablas del enunciado + Vehiculo.Disponible (para no asignar un vehículo
   ocupado hasta que se libere).
   ===================================================================== */

CREATE TABLE Vehiculo (
    IdVehiculo int PRIMARY KEY,
    Marca      varchar(50) NOT NULL,
    Modelo     varchar(50) NOT NULL,
    Patente    varchar(10) NOT NULL,
    Disponible bit NOT NULL DEFAULT 1
);

CREATE TABLE Pasajero (
    IdPasajero int PRIMARY KEY,
    Nombre     varchar(50)  NOT NULL,
    Apellido   varchar(50)  NOT NULL,
    Direccion  varchar(100) NOT NULL
);

CREATE TABLE Viaje (
    IdViaje    int PRIMARY KEY,
    IdVehiculo int NOT NULL REFERENCES Vehiculo (IdVehiculo),
    IdPasajero int NOT NULL REFERENCES Pasajero (IdPasajero),
    Fecha      datetime      NOT NULL,
    Destino    varchar(100)  NOT NULL,
    Kilometro  decimal(10,2) NOT NULL,
    Importe    decimal(12,2) NOT NULL
);
GO

-- Punto 2: los vehículos se cargan desde la base
INSERT INTO Vehiculo (IdVehiculo, Marca, Modelo, Patente) VALUES
    (1, 'Toyota', 'Etios', 'AB123CD'),
    (2, 'Chevrolet', 'Prisma', 'AC456EF'),
    (3, 'Fiat', 'Cronos', 'AD789GH'),
    (4, 'Renault', 'Logan', 'AE012IJ');
GO

/* ---------------- Pasajeros (ABM) ---------------- */

CREATE PROCEDURE AltaPasajero
    @nombre varchar(50), @apellido varchar(50), @direccion varchar(100)
AS
    -- Sin IDENTITY: el Id es el máximo + 1
    INSERT INTO Pasajero (IdPasajero, Nombre, Apellido, Direccion)
    VALUES ((SELECT ISNULL(MAX(IdPasajero), 0) + 1 FROM Pasajero), @nombre, @apellido, @direccion);
GO

CREATE PROCEDURE ModificarPasajero
    @idPasajero int, @nombre varchar(50), @apellido varchar(50), @direccion varchar(100)
AS
    UPDATE Pasajero SET Nombre = @nombre, Apellido = @apellido, Direccion = @direccion
    WHERE IdPasajero = @idPasajero;
GO

CREATE PROCEDURE BajaPasajero
    @idPasajero int
AS
    DELETE FROM Pasajero WHERE IdPasajero = @idPasajero;
GO

CREATE PROCEDURE ListarPasajeros
AS
    SELECT IdPasajero, Nombre, Apellido, Direccion FROM Pasajero ORDER BY Apellido, Nombre;
GO

CREATE PROCEDURE BuscarPasajero
    @apellido varchar(50)
AS
    SELECT IdPasajero, Nombre, Apellido, Direccion FROM Pasajero
    WHERE Apellido LIKE '%' + @apellido + '%'
    ORDER BY Apellido, Nombre;
GO

/* ---------------- Vehículos ---------------- */

CREATE PROCEDURE ListarVehiculos
AS
    SELECT IdVehiculo, Marca, Modelo, Patente, Disponible FROM Vehiculo ORDER BY Patente;
GO

CREATE PROCEDURE LiberarVehiculo
    @idVehiculo int
AS
    UPDATE Vehiculo SET Disponible = 1 WHERE IdVehiculo = @idVehiculo;
GO

/* ---------------- Viajes ---------------- */

-- Registra el viaje y deja el vehículo ocupado
CREATE PROCEDURE AltaViaje
    @idVehiculo int, @idPasajero int, @fecha datetime, @destino varchar(100),
    @kilometro decimal(10,2), @importe decimal(12,2)
AS
BEGIN
    IF (SELECT Disponible FROM Vehiculo WHERE IdVehiculo = @idVehiculo) = 0
        THROW 50001, 'El vehículo ya está asignado a otro viaje.', 1;

    BEGIN TRAN;
        INSERT INTO Viaje (IdViaje, IdVehiculo, IdPasajero, Fecha, Destino, Kilometro, Importe)
        VALUES ((SELECT ISNULL(MAX(IdViaje), 0) + 1 FROM Viaje),
                @idVehiculo, @idPasajero, @fecha, @destino, @kilometro, @importe);
        UPDATE Vehiculo SET Disponible = 0 WHERE IdVehiculo = @idVehiculo;
    COMMIT;
END
GO

CREATE PROCEDURE ListarViajes
AS
    SELECT IdViaje, IdVehiculo, IdPasajero, Fecha, Destino, Kilometro, Importe
    FROM Viaje ORDER BY Fecha DESC;
GO

/* ---------------- XML ---------------- */

-- Punto 6
CREATE PROCEDURE ListarViajesMayores25000
AS
    SELECT IdViaje, IdVehiculo, IdPasajero, Fecha, Destino, Kilometro, Importe
    FROM Viaje WHERE Importe > 25000;
GO

-- Punto 7
CREATE PROCEDURE ListarViajesHasta20Km
AS
    SELECT IdViaje, IdVehiculo, IdPasajero, Fecha, Destino, Kilometro, Importe
    FROM Viaje WHERE Kilometro <= 20;
GO
