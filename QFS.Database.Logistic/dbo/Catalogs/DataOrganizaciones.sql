SET IDENTITY_INSERT [dbo].[SIS_Organizaciones] ON;

MERGE INTO [dbo].[SIS_Organizaciones] AS Target
USING (
    VALUES
        (1, N'Organización 01', N'ORG 01', N'mail@mail.com', 1, 1, CAST(N'2022-02-10T13:30:15.603' AS DateTime), 1, CAST(N'2024-09-14T17:20:17.547' AS DateTime), NULL, NULL)
) AS Source (NewOrgId, NewOrgNombre, NewOrgAbreviatura, NewOrgMail, NewOrgActivo, NewOrgUsuIdAlta, NewOrgFecAlta, NewOrgUsuIdMod, NewOrgFecMod, NewOrgFecBaja, NewOrgUsuIdBaja)
ON Target.[org_id] = Source.NewOrgId

WHEN MATCHED THEN
    UPDATE SET
        Target.[org_nombre] = Source.NewOrgNombre,
        Target.[org_abreviatura] = Source.NewOrgAbreviatura,
        Target.[org_mail] = Source.NewOrgMail,
        Target.[org_activo] = Source.NewOrgActivo,
        Target.[org_usu_id_alta] = Source.NewOrgUsuIdAlta,
        Target.[org_fec_alta] = Source.NewOrgFecAlta,
        Target.[org_usu_id_mod] = Source.NewOrgUsuIdMod,
        Target.[org_fec_mod] = Source.NewOrgFecMod,
        Target.[org_fec_baja] = Source.NewOrgFecBaja,
        Target.[org_usu_id_baja] = Source.NewOrgUsuIdBaja

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [org_id],
        [org_nombre],
        [org_abreviatura],
        [org_mail],
        [org_activo],
        [org_usu_id_alta],
        [org_fec_alta],
        [org_usu_id_mod],
        [org_fec_mod],
        [org_fec_baja],
        [org_usu_id_baja]
    )
    VALUES (
        Source.NewOrgId,
        Source.NewOrgNombre,
        Source.NewOrgAbreviatura,
        Source.NewOrgMail,
        Source.NewOrgActivo,
        Source.NewOrgUsuIdAlta,
        Source.NewOrgFecAlta,
        Source.NewOrgUsuIdMod,
        Source.NewOrgFecMod,
        Source.NewOrgFecBaja,
        Source.NewOrgUsuIdBaja
    );

SET IDENTITY_INSERT [dbo].[SIS_Organizaciones] OFF;