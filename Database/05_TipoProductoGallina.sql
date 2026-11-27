-- =============================================================
-- LRChickens - El tipo de producto "Pollo" pasa a llamarse "Gallina"
-- ("Pollo" venía de los primeros prototipos). Se venden huevos y gallinas.
-- Ejecutar sobre DBPollos después de 01 a 04. Se puede correr más de una vez.
-- =============================================================

UPDATE TipoProducto SET Descripcion = 'Gallina'
WHERE IdTipoProducto = 2 AND Descripcion = 'Pollo';
GO

-- Verificación: tiene que mostrar 1 = Huevo y 2 = Gallina.
SELECT IdTipoProducto, Descripcion FROM TipoProducto ORDER BY IdTipoProducto;
GO
