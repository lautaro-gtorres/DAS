-- =============================================================
-- LRChickens - Reportes (formulario Reportes)
-- Ejecutar sobre DBPollos después de 01 a 08. Se puede correr más de una vez.
--
-- Toda la lógica de los reportes está acá: período, agrupación, totales y promedios.
-- @ultimoAnio = 0: último mes (desde hoy - 1 mes); 1: último año (desde hoy - 1 año).
-- Todos devuelven las mismas columnas: Concepto, Registros, Cantidad, Valor.
-- GROUP BY ROLLUP agrega sola la última fila con el TOTAL (sin UNION).
-- Si en el período no hay datos, no devuelven filas.
-- =============================================================


-- Ventas por producto: cantidad de ventas, unidades vendidas y total en pesos.
-- TipoProducto: 1 = Huevo, 2 = Gallina (igual que en AltaVenta), así no hace falta JOIN.
CREATE OR ALTER PROCEDURE ReporteVentas
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT ISNULL(CASE IdTipoProducto WHEN 1 THEN 'Huevo' WHEN 2 THEN 'Gallina' END, 'TOTAL') AS Concepto,
           COUNT(*) AS Registros,
           CAST(SUM(Cantidad) AS decimal(14,2)) AS Cantidad,
           CAST(SUM(Total) AS decimal(14,2)) AS Valor
    FROM Venta
    WHERE Fecha BETWEEN @desde AND @hoy
    GROUP BY ROLLUP(IdTipoProducto)
    ORDER BY GROUPING(IdTipoProducto), Concepto;
END
GO


-- Compras de alimento por alimento: cantidad de compras, kilos y total en pesos.
CREATE OR ALTER PROCEDURE ReporteCompras
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT ISNULL(a.NombreAlimento, 'TOTAL') AS Concepto,
           COUNT(*) AS Registros,
           CAST(SUM(c.Kilogramos) AS decimal(14,2)) AS Cantidad,
           CAST(SUM(c.Total) AS decimal(14,2)) AS Valor
    FROM Compra c
    INNER JOIN Alimento a ON a.IdAlimento = c.IdAlimento
    WHERE c.Fecha BETWEEN @desde AND @hoy
    GROUP BY ROLLUP(a.NombreAlimento)
    ORDER BY GROUPING(a.NombreAlimento), Concepto;
END
GO


-- Producción de huevos por corral: días con registro, huevos y promedio por día.
-- En la fila TOTAL, COUNT(DISTINCT Fecha) cuenta los días distintos de toda la granja,
-- así el promedio es el de la granja entera.
CREATE OR ALTER PROCEDURE ReporteProduccion
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT ISNULL('Corral ' + co.NroCorral, 'TOTAL') AS Concepto,
           COUNT(DISTINCT p.Fecha) AS Registros,
           CAST(SUM(p.CantidadHuevos) AS decimal(14,2)) AS Cantidad,
           CAST(SUM(p.CantidadHuevos) * 1.0 / COUNT(DISTINCT p.Fecha) AS decimal(14,1)) AS Valor
    FROM Produccion p
    INNER JOIN Corral co ON co.IdCorral = p.IdCorral
    WHERE p.Fecha BETWEEN @desde AND @hoy
    GROUP BY ROLLUP(co.NroCorral)
    ORDER BY GROUPING(co.NroCorral), Concepto;
END
GO


-- =====================  Verificación  =====================
SELECT name AS Procedimiento, modify_date
FROM sys.procedures
WHERE name IN ('ReporteVentas', 'ReporteCompras', 'ReporteProduccion')
ORDER BY name;
GO
