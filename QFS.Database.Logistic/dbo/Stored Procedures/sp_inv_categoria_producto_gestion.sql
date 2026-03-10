CREATE PROCEDURE [dbo].[sp_inv_categoria_producto_gestion]
    @catpro_id INT = 0,
    @catpro_org_id INT,
    @catpro_rubpro_id INT,
    @catpro_nombre NVARCHAR(120),
    @catpro_descripcion NVARCHAR(255) = NULL,
    @catpro_activo BIT = 1,
    @catpro_usu_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @catpro_id = 0
    BEGIN
        INSERT INTO [dbo].[Categorias_Productos]
        (
            [catpro_org_id], [catpro_rubpro_id], [catpro_nombre], [catpro_descripcion], [catpro_activo], [catpro_fec_alta], [catpro_usu_id_alta]
        )
        VALUES
        (
            @catpro_org_id, @catpro_rubpro_id, @catpro_nombre, @catpro_descripcion, @catpro_activo, GETDATE(), @catpro_usu_id
        );
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Categorias_Productos]
        SET
            [catpro_rubpro_id] = @catpro_rubpro_id,
            [catpro_nombre] = @catpro_nombre,
            [catpro_descripcion] = @catpro_descripcion,
            [catpro_activo] = @catpro_activo,
            [catpro_fec_mod] = GETDATE(),
            [catpro_usu_id_mod] = @catpro_usu_id
        WHERE [catpro_id] = @catpro_id;
    END
END
