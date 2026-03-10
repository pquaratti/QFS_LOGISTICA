/* SPs de reportes inventario */
GO
CREATE OR ALTER PROCEDURE SP_Inv_StockPorDeposito
    @org_id INT,
    @depo_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.depo_nombre, p.pro_codigo_interno, p.pro_descripcion_corta,
           s.stock_actual, s.stock_reservado, s.stock_disponible
    FROM Stocks s
    INNER JOIN Depositos d ON d.depo_id = s.stock_depo_id
    INNER JOIN Productos p ON p.pro_id = s.stock_pro_id
    WHERE s.stock_org_id = @org_id
      AND (@depo_id IS NULL OR s.stock_depo_id = @depo_id)
    ORDER BY d.depo_nombre, p.pro_descripcion_corta;
END
GO

CREATE OR ALTER PROCEDURE SP_Inv_MovimientosPeriodo
    @org_id INT,
    @fecha_desde DATETIME,
    @fecha_hasta DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.movinv_fecha, t.timi_nombre, p.pro_codigo_interno,
           p.pro_descripcion_corta, d.depo_nombre, m.movinv_cantidad,
           m.movinv_lote, m.movinv_vencimiento, m.movinv_serie,
           m.movinv_estado
    FROM Movimientos_Inventario m
    INNER JOIN Tipos_Movimientos_Inventario t ON t.timi_id = m.movinv_timi_id
    INNER JOIN Productos p ON p.pro_id = m.movinv_pro_id
    INNER JOIN Depositos d ON d.depo_id = m.movinv_depo_origen_id
    WHERE m.movinv_org_id = @org_id
      AND m.movinv_fecha BETWEEN @fecha_desde AND @fecha_hasta
    ORDER BY m.movinv_fecha DESC;
END
GO
