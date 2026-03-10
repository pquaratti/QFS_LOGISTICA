CREATE PROCEDURE [dbo].[sp_inv_subcategoria_producto_gestion]
    @subcatpro_id INT = 0,
    @subcatpro_org_id INT,
    @subcatpro_rubpro_id INT,
    @subcatpro_catpro_id INT,
    @subcatpro_nombre NVARCHAR(120),
    @subcatpro_descripcion NVARCHAR(255) = NULL,
    @subcatpro_activo BIT = 1,
    @subcatpro_usu_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @subcatpro_id = 0
    BEGIN
        INSERT INTO [dbo].[Subcategorias_Productos]
        (
            [subcatpro_org_id], [subcatpro_rubpro_id], [subcatpro_catpro_id], [subcatpro_nombre], [subcatpro_descripcion], [subcatpro_activo], [subcatpro_fec_alta], [subcatpro_usu_id_alta]
        )
        VALUES
        (
            @subcatpro_org_id, @subcatpro_rubpro_id, @subcatpro_catpro_id, @subcatpro_nombre, @subcatpro_descripcion, @subcatpro_activo, GETDATE(), @subcatpro_usu_id
        );
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Subcategorias_Productos]
        SET
            [subcatpro_rubpro_id] = @subcatpro_rubpro_id,
            [subcatpro_catpro_id] = @subcatpro_catpro_id,
            [subcatpro_nombre] = @subcatpro_nombre,
            [subcatpro_descripcion] = @subcatpro_descripcion,
            [subcatpro_activo] = @subcatpro_activo,
            [subcatpro_fec_mod] = GETDATE(),
            [subcatpro_usu_id_mod] = @subcatpro_usu_id
        WHERE [subcatpro_id] = @subcatpro_id;
    END
END
