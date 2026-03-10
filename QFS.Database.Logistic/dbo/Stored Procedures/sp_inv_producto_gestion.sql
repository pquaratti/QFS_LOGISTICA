CREATE PROCEDURE [dbo].[sp_inv_producto_gestion]
    @pro_id INT = 0,
    @pro_org_id INT,
    @pro_rubpro_id INT,
    @pro_catpro_id INT,
    @pro_subcatpro_id INT,
    @pro_unimed_id INT,
    @pro_codigo_interno NVARCHAR(60),
    @pro_codigo_barras NVARCHAR(60) = NULL,
    @pro_descripcion_corta NVARCHAR(180),
    @pro_descripcion_larga NVARCHAR(500) = NULL,
    @pro_presentacion NVARCHAR(120) = NULL,
    @pro_marca NVARCHAR(120) = NULL,
    @pro_modelo NVARCHAR(120) = NULL,
    @pro_tipo_producto NVARCHAR(120) = NULL,
    @pro_requiere_trazabilidad BIT = 0,
    @pro_requiere_lote BIT = 0,
    @pro_requiere_vencimiento BIT = 0,
    @pro_requiere_serie BIT = 0,
    @pro_stock_minimo DECIMAL(18,4) = 0,
    @pro_stock_maximo DECIMAL(18,4) = 0,
    @pro_punto_reposicion DECIMAL(18,4) = 0,
    @pro_estado NVARCHAR(40) = N'ACTIVO',
    @pro_observaciones NVARCHAR(500) = NULL,
    @pro_activo BIT = 1,
    @pro_usu_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @pro_id = 0
    BEGIN
        INSERT INTO [dbo].[Productos]
        (
            [pro_org_id], [pro_rubpro_id], [pro_catpro_id], [pro_subcatpro_id], [pro_unimed_id],
            [pro_codigo_interno], [pro_codigo_barras], [pro_descripcion_corta], [pro_descripcion_larga],
            [pro_presentacion], [pro_marca], [pro_modelo], [pro_tipo_producto],
            [pro_requiere_trazabilidad], [pro_requiere_lote], [pro_requiere_vencimiento], [pro_requiere_serie],
            [pro_stock_minimo], [pro_stock_maximo], [pro_punto_reposicion],
            [pro_estado], [pro_observaciones], [pro_activo], [pro_fec_alta], [pro_usu_id_alta]
        )
        VALUES
        (
            @pro_org_id, @pro_rubpro_id, @pro_catpro_id, @pro_subcatpro_id, @pro_unimed_id,
            @pro_codigo_interno, @pro_codigo_barras, @pro_descripcion_corta, @pro_descripcion_larga,
            @pro_presentacion, @pro_marca, @pro_modelo, @pro_tipo_producto,
            @pro_requiere_trazabilidad, @pro_requiere_lote, @pro_requiere_vencimiento, @pro_requiere_serie,
            @pro_stock_minimo, @pro_stock_maximo, @pro_punto_reposicion,
            @pro_estado, @pro_observaciones, @pro_activo, GETDATE(), @pro_usu_id
        );
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Productos]
        SET
            [pro_rubpro_id] = @pro_rubpro_id,
            [pro_catpro_id] = @pro_catpro_id,
            [pro_subcatpro_id] = @pro_subcatpro_id,
            [pro_unimed_id] = @pro_unimed_id,
            [pro_codigo_interno] = @pro_codigo_interno,
            [pro_codigo_barras] = @pro_codigo_barras,
            [pro_descripcion_corta] = @pro_descripcion_corta,
            [pro_descripcion_larga] = @pro_descripcion_larga,
            [pro_presentacion] = @pro_presentacion,
            [pro_marca] = @pro_marca,
            [pro_modelo] = @pro_modelo,
            [pro_tipo_producto] = @pro_tipo_producto,
            [pro_requiere_trazabilidad] = @pro_requiere_trazabilidad,
            [pro_requiere_lote] = @pro_requiere_lote,
            [pro_requiere_vencimiento] = @pro_requiere_vencimiento,
            [pro_requiere_serie] = @pro_requiere_serie,
            [pro_stock_minimo] = @pro_stock_minimo,
            [pro_stock_maximo] = @pro_stock_maximo,
            [pro_punto_reposicion] = @pro_punto_reposicion,
            [pro_estado] = @pro_estado,
            [pro_observaciones] = @pro_observaciones,
            [pro_activo] = @pro_activo,
            [pro_fec_mod] = GETDATE(),
            [pro_usu_id_mod] = @pro_usu_id
        WHERE [pro_id] = @pro_id;
    END
END
