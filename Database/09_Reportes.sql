-- =============================================================
-- LRChickens - Reportes (formulario Reportes)
-- Ejecutar sobre DBPollos después de 01 a 08. Se puede correr más de una vez.
--
-- Toda la lógica de los reportes está acá: período, agrupación, totales y promedios.
-- @ultimoAnio = 0: último mes (desde hoy - 1 mes); 1: último año (desde hoy - 1 año).
-- Todos devuelven las mismas columnas: Concepto, Registros, Cantidad, Valor,
-- con una última fila 'TOTAL'.
-- =============================================================


-- Ventas por producto: cantidad de ventas, unidades vendidas y total en pesos.
CREATE OR ALTER PROCEDURE ReporteVentas
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT Concepto, Registros, Cantidad, Valor
    FROM
    (
        SELECT 0 AS Orden, tp.Descripcion AS Concepto, COUNT(*) AS Registros,
               CAST(SUM(v.Cantidad) AS decimal(14,2)) AS Cantidad,
               CAST(SUM(v.Total) AS decimal(14,2)) AS Valor
        FROM Venta v
        INNER JOIN TipoProducto tp ON tp.IdTipoProducto = v.IdTipoProducto
        WHERE v.Fecha BETWEEN @desde AND @hoy
        GROUP BY tp.Descripcion

        UNION ALL

        SELECT 1, 'TOTAL', COUNT(*),
               CAST(ISNULL(SUM(v.Cantidad), 0) AS decimal(14,2)),
               CAST(ISNULL(SUM(v.Total), 0) AS decimal(14,2))
        FROM Venta v
        WHERE v.Fecha BETWEEN @desde AND @hoy
    ) r
    ORDER BY Orden, Concepto;
END
GO


-- Compras de alimento por alimento: cantidad de compras, kilos y total en pesos.
CREATE OR ALTER PROCEDURE ReporteCompras
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT Concepto, Registros, Cantidad, Valor
    FROM
    (
        SELECT 0 AS Orden, a.NombreAlimento AS Concepto, COUNT(*) AS Registros,
               CAST(SUM(c.Kilogramos) AS decimal(14,2)) AS Cantidad,
               CAST(SUM(c.Total) AS decimal(14,2)) AS Valor
        FROM Compra c
        INNER JOIN Alimento a ON a.IdAlimento = c.IdAlimento
        WHERE c.Fecha BETWEEN @desde AND @hoy
        GROUP BY a.NombreAlimento

        UNION ALL

        SELECT 1, 'TOTAL', COUNT(*),
               CAST(ISNULL(SUM(c.Kilogramos), 0) AS decimal(14,2)),
               CAST(ISNULL(SUM(c.Total), 0) AS decimal(14,2))
        FROM Compra c
        WHERE c.Fecha BETWEEN @desde AND @hoy
    ) r
    ORDER BY Orden, Concepto;
END
GO


-- Producción de huevos por corral: días con registro, huevos y promedio por día.
-- La fila TOTAL es la granja entera: huevos de todos los corrales / días distintos con producción.
CREATE OR ALTER PROCEDURE ReporteProduccion
    @ultimoAnio bit
AS
BEGIN
    DECLARE @hoy date = CAST(GETDATE() AS date);
    DECLARE @desde date = CASE WHEN @ultimoAnio = 1 THEN DATEADD(YEAR, -1, @hoy) ELSE DATEADD(MONTH, -1, @hoy) END;

    SELECT Concepto, Registros, Cantidad, Valor
    FROM
    (
        SELECT 0 AS Orden, 'Corral ' + co.NroCorral AS Concepto, COUNT(*) AS Registros,
               CAST(SUM(p.CantidadHuevos) AS decimal(14,2)) AS Cantidad,
               CAST(SUM(p.CantidadHuevos) * 1.0 / COUNT(*) AS decimal(14,1)) AS Valor
        FROM Produccion p
        INNER JOIN Corral co ON co.IdCorral = p.IdCorral
        WHERE p.Fecha BETWEEN @desde AND @hoy
        GROUP BY co.NroCorral

        UNION ALL

        SELECT 1, 'TOTAL', COUNT(DISTINCT p.Fecha),
               CAST(ISNULL(SUM(p.CantidadHuevos), 0) AS decimal(14,2)),
               CAST(ISNULL(SUM(p.CantidadHuevos) * 1.0 / NULLIF(COUNT(DISTINCT p.Fecha), 0), 0) AS decimal(14,1))
        FROM Produccion p
        WHERE p.Fecha BETWEEN @desde AND @hoy
    ) r
    ORDER BY Orden, Concepto;
END
GO


-- =====================  Verificación  =====================
SELECT name AS Procedimiento, modify_date
FROM sys.procedures
WHERE name IN ('ReporteVentas', 'ReporteCompras', 'ReporteProduccion')
ORDER BY name;
GO
