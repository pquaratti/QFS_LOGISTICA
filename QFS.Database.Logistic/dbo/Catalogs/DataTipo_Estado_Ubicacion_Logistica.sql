SET IDENTITY_INSERT [dbo].[Tipo_Estado_Ubicacion_Logistica] ON;

MERGE INTO [dbo].[Tipo_Estado_Ubicacion_Logistica] AS Target
USING (
    VALUES
        (1, N'Libre', 1, 1, CAST(N'2026-07-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL, NULL),
        (2, N'Ocupada', 1, 1, CAST(N'2026-07-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL, NULL),
        (3, N'Parcial', 1, 1, CAST(N'2026-07-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL, NULL),
        (4, N'Reservada', 1, 1, CAST(N'2026-07-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL, NULL),
        (5, N'Bloqueada', 1, 1, CAST(N'2026-07-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL, NULL)
) AS Source (NewTeubilogId, NewTeubilogNombre, NewTeubilogOrgId, NewTeubilogUsuIdAlta, NewTeubilogFecAlta, NewTeubilogUsuIdMod, NewTeubilogFecMod, NewTeubilogActivo, NewTeubilogUsuIdBaja, NewTeubilogFecBaja)
ON Target.[teubilog_id] = Source.NewTeubilogId

WHEN MATCHED THEN
    UPDATE SET
        Target.[teubilog_nombre] = Source.NewTeubilogNombre,
        Target.[teubilog_org_id] = Source.NewTeubilogOrgId,
        Target.[teubilog_usu_id_alta] = Source.NewTeubilogUsuIdAlta,
        Target.[teubilog_fec_alta] = Source.NewTeubilogFecAlta,
        Target.[teubilog_usu_id_mod] = Source.NewTeubilogUsuIdMod,
        Target.[teubilog_fec_mod] = Source.NewTeubilogFecMod,
        Target.[teubilog_activo] = Source.NewTeubilogActivo,
        Target.[teubilog_usu_id_baja] = Source.NewTeubilogUsuIdBaja,
        Target.[teubilog_fec_baja] = Source.NewTeubilogFecBaja

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [teubilog_id],
        [teubilog_nombre],
        [teubilog_org_id],
        [teubilog_usu_id_alta],
        [teubilog_fec_alta],
        [teubilog_usu_id_mod],
        [teubilog_fec_mod],
        [teubilog_activo],
        [teubilog_usu_id_baja],
        [teubilog_fec_baja]
    )
    VALUES (
        Source.NewTeubilogId,
        Source.NewTeubilogNombre,
        Source.NewTeubilogOrgId,
        Source.NewTeubilogUsuIdAlta,
        Source.NewTeubilogFecAlta,
        Source.NewTeubilogUsuIdMod,
        Source.NewTeubilogFecMod,
        Source.NewTeubilogActivo,
        Source.NewTeubilogUsuIdBaja,
        Source.NewTeubilogFecBaja
    );

SET IDENTITY_INSERT [dbo].[Tipo_Estado_Ubicacion_Logistica] OFF;