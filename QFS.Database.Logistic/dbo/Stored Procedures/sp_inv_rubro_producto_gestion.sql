CREATE PROCEDURE [dbo].[sp_inv_rubro_producto_gestion]
    @rubpro_id INT = 0,
    @rubpro_org_id INT,
    @rubpro_nombre NVARCHAR(120),
    @rubpro_descripcion NVARCHAR(255) = NULL,
    @rubpro_activo BIT = 1,
    @rubpro_usu_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @rubpro_id = 0
    BEGIN
        INSERT INTO [dbo].[Rubros_Productos]
        (
            [rubpro_org_id], [rubpro_nombre], [rubpro_descripcion], [rubpro_activo], [rubpro_fec_alta], [rubpro_usu_id_alta]
        )
        VALUES
        (
            @rubpro_org_id, @rubpro_nombre, @rubpro_descripcion, @rubpro_activo, GETDATE(), @rubpro_usu_id
        );
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Rubros_Productos]
        SET
            [rubpro_nombre] = @rubpro_nombre,
            [rubpro_descripcion] = @rubpro_descripcion,
            [rubpro_activo] = @rubpro_activo,
            [rubpro_fec_mod] = GETDATE(),
            [rubpro_usu_id_mod] = @rubpro_usu_id
        WHERE [rubpro_id] = @rubpro_id;
    END
END
