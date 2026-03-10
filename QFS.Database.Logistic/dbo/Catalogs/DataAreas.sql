SET IDENTITY_INSERT [dbo].[SIS_Areas] ON;

MERGE INTO [dbo].[SIS_Areas] AS Target
USING (
    VALUES
        (1, N'Area 1 ', N'A1', 0, 1, 1, NULL, NULL, CAST(N'2024-09-14T17:21:14.837' AS DateTime), NULL, NULL, 0),
        (2, N'Area 2', N'A2', 0, 1, 1, NULL, NULL, CAST(N'2024-09-14T17:21:22.670' AS DateTime), NULL, NULL, 0)
) AS Source (NewAreaId, NewAreaNombre, NewAreaAbreviatura, NewAreaAreaId, NewAreaOrgId, NewAreaUsuIdAlta, NewAreaUsuIdMod, NewAreaUsuIdBaja, NewAreaFecAlta, NewAreaFecBaja, NewAreaFecMod, NewAreaActivo)
ON Target.[area_id] = Source.NewAreaId

WHEN MATCHED THEN
    UPDATE SET
        Target.[area_nombre] = Source.NewAreaNombre,
        Target.[area_abreviatura] = Source.NewAreaAbreviatura,
        Target.[area_area_id] = Source.NewAreaAreaId,
        Target.[area_org_id] = Source.NewAreaOrgId,
        Target.[area_usu_id_alta] = Source.NewAreaUsuIdAlta,
        Target.[area_usu_id_mod] = Source.NewAreaUsuIdMod,
        Target.[area_usu_id_baja] = Source.NewAreaUsuIdBaja,
        Target.[area_fec_alta] = Source.NewAreaFecAlta,
        Target.[area_fec_baja] = Source.NewAreaFecBaja,
        Target.[area_fec_mod] = Source.NewAreaFecMod,
        Target.[area_activo] = Source.NewAreaActivo

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [area_id],
        [area_nombre],
        [area_abreviatura],
        [area_area_id],
        [area_org_id],
        [area_usu_id_alta],
        [area_usu_id_mod],
        [area_usu_id_baja],
        [area_fec_alta],
        [area_fec_baja],
        [area_fec_mod],
        [area_activo]
    )
    VALUES (
        Source.NewAreaId,
        Source.NewAreaNombre,
        Source.NewAreaAbreviatura,
        Source.NewAreaAreaId,
        Source.NewAreaOrgId,
        Source.NewAreaUsuIdAlta,
        Source.NewAreaUsuIdMod,
        Source.NewAreaUsuIdBaja,
        Source.NewAreaFecAlta,
        Source.NewAreaFecBaja,
        Source.NewAreaFecMod,
        Source.NewAreaActivo
    );

SET IDENTITY_INSERT [dbo].[SIS_Areas] OFF;