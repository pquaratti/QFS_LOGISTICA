SET IDENTITY_INSERT [dbo].[SIS_Perfiles] ON;

MERGE INTO [dbo].[SIS_Perfiles] AS Target
USING (
    VALUES
        (1, N'Administrador Sistema                                                           ', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL),
        (2, N'Administrador                                                                   ', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
) AS Source (NewPrfId, NewPrfNombre, NewPrfModId, NewPrfActivo, NewPrfUsuIdAlta, NewPrfUsuIdMod, NewPrfUsuIdBaja, NewPrfUsuFecAlta, NewPrfUsuFecMod, NewPrfUsuFecBaja)
ON Target.[prf_id] = Source.NewPrfId

WHEN MATCHED THEN
    UPDATE SET
        Target.[prf_nombre] = Source.NewPrfNombre,
        Target.[prf_mod_id] = Source.NewPrfModId,
        Target.[prf_activo] = Source.NewPrfActivo,
        Target.[prf_usu_id_alta] = Source.NewPrfUsuIdAlta,
        Target.[prf_usu_id_mod] = Source.NewPrfUsuIdMod,
        Target.[prf_usu_id_baja] = Source.NewPrfUsuIdBaja,
        Target.[prf_usu_fec_alta] = Source.NewPrfUsuFecAlta,
        Target.[prf_usu_fec_mod] = Source.NewPrfUsuFecMod,
        Target.[prf_usu_fec_baja] = Source.NewPrfUsuFecBaja

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [prf_id],
        [prf_nombre],
        [prf_mod_id],
        [prf_activo],
        [prf_usu_id_alta],
        [prf_usu_id_mod],
        [prf_usu_id_baja],
        [prf_usu_fec_alta],
        [prf_usu_fec_mod],
        [prf_usu_fec_baja]
    )
    VALUES (
        Source.NewPrfId,
        Source.NewPrfNombre,
        Source.NewPrfModId,
        Source.NewPrfActivo,
        Source.NewPrfUsuIdAlta,
        Source.NewPrfUsuIdMod,
        Source.NewPrfUsuIdBaja,
        Source.NewPrfUsuFecAlta,
        Source.NewPrfUsuFecMod,
        Source.NewPrfUsuFecBaja
    );

SET IDENTITY_INSERT [dbo].[SIS_Perfiles] OFF;