MERGE INTO [dbo].[SIS_Modulos] AS Target
USING (
    VALUES
        (1, N'SISTEMA', N'SISTEMA', 1),
        (2, N'GESTION', N'Módulo de Gestión', 1)
) AS Source (NewModId, NewModNombre, NewModDescripcion, NewModActivo)
ON Target.[mod_id] = Source.NewModId

WHEN MATCHED THEN
    UPDATE SET
        Target.[mod_nombre] = Source.NewModNombre,
        Target.[mod_descripcion] = Source.NewModDescripcion,
        Target.[mod_activo] = Source.NewModActivo

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [mod_id],
        [mod_nombre],
        [mod_descripcion],
        [mod_activo]
    )
    VALUES (
        Source.NewModId,
        Source.NewModNombre,
        Source.NewModDescripcion,
        Source.NewModActivo
    );