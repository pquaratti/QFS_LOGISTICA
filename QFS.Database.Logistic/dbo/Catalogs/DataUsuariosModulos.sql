SET IDENTITY_INSERT [dbo].[SIS_Usuarios_Modulos] ON;

MERGE INTO [dbo].[SIS_Usuarios_Modulos] AS Target
USING (
    VALUES
        (1, 1, 1, 1, NULL),
        (2, 1, 2, 2, NULL),
        (3, 2, 2, 2, NULL)
) AS Source (
    NewUsiId,
    NewUsiUsuId,
    NewUsiModId,
    NewUsiPrfId,
    NewUsiFecAcceso
)
ON Target.[usi_id] = Source.NewUsiId

WHEN MATCHED THEN
    UPDATE SET
        Target.[usi_usu_id]     = Source.NewUsiUsuId,
        Target.[usi_mod_id]     = Source.NewUsiModId,
        Target.[usi_prf_id]     = Source.NewUsiPrfId,
        Target.[usi_fec_acceso] = Source.NewUsiFecAcceso

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [usi_id],
        [usi_usu_id],
        [usi_mod_id],
        [usi_prf_id],
        [usi_fec_acceso]
    )
    VALUES (
        Source.NewUsiId,
        Source.NewUsiUsuId,
        Source.NewUsiModId,
        Source.NewUsiPrfId,
        Source.NewUsiFecAcceso
    );

SET IDENTITY_INSERT [dbo].[SIS_Usuarios_Modulos] OFF;