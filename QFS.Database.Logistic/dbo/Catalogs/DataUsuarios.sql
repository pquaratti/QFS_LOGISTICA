SET IDENTITY_INSERT [dbo].[SIS_Usuarios] ON;

MERGE INTO [dbo].[SIS_Usuarios] AS Target
USING (
    VALUES
        (1, N'administrador', N'JdA1dk/YQDSPzY2Crd0d5A==', CAST(N'2024-09-20T13:16:51.267' AS DateTime), NULL, NULL, NULL, 1, 0, N'usuario', N'administrador', N'pquaratti@gmail.com', N'0', CAST(N'2021-07-06T21:08:51.217' AS DateTime), 1, 4, 0, N'92914')
) AS Source (NewUsuId, NewUsuNickname, NewUsuPassword, NewUsuFecLastLogon, NewUsuFecEliminado, NewUsuFecPassChanged, NewUsuFecBloqueado, NewUsuAdministrador, NewUsuIntentos, NewUsuNombre, NewUsuApellido, NewUsuMail, NewUsuDocumento, NewUsuTerminosYCondiciones, NewUsuOrgId, NewUsuCatId, NewUsuAreaId, NewUsuLegajo)
ON Target.[usu_id] = Source.NewUsuId

WHEN MATCHED THEN
    UPDATE SET
        Target.[usu_nickname] = Source.NewUsuNickname,
        Target.[usu_password] = Source.NewUsuPassword,
        Target.[usu_fec_last_logon] = Source.NewUsuFecLastLogon,
        Target.[usu_fec_eliminado] = Source.NewUsuFecEliminado,
        Target.[usu_fec_pass_changed] = Source.NewUsuFecPassChanged,
        Target.[usu_fec_bloqueado] = Source.NewUsuFecBloqueado,
        Target.[usu_administrador] = Source.NewUsuAdministrador,
        Target.[usu_intentos] = Source.NewUsuIntentos,
        Target.[usu_nombre] = Source.NewUsuNombre,
        Target.[usu_apellido] = Source.NewUsuApellido,
        Target.[usu_mail] = Source.NewUsuMail,
        Target.[usu_documento] = Source.NewUsuDocumento,
        Target.[usu_terminos_y_condiciones] = Source.NewUsuTerminosYCondiciones,
        Target.[usu_org_id] = Source.NewUsuOrgId,
        Target.[usu_cat_id] = Source.NewUsuCatId,
        Target.[usu_area_id] = Source.NewUsuAreaId,
        Target.[usu_legajo] = Source.NewUsuLegajo

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [usu_id],
        [usu_nickname],
        [usu_password],
        [usu_fec_last_logon],
        [usu_fec_eliminado],
        [usu_fec_pass_changed],
        [usu_fec_bloqueado],
        [usu_administrador],
        [usu_intentos],
        [usu_nombre],
        [usu_apellido],
        [usu_mail],
        [usu_documento],
        [usu_terminos_y_condiciones],
        [usu_org_id],
        [usu_cat_id],
        [usu_area_id],
        [usu_legajo]
    )
    VALUES (
        Source.NewUsuId,
        Source.NewUsuNickname,
        Source.NewUsuPassword,
        Source.NewUsuFecLastLogon,
        Source.NewUsuFecEliminado,
        Source.NewUsuFecPassChanged,
        Source.NewUsuFecBloqueado,
        Source.NewUsuAdministrador,
        Source.NewUsuIntentos,
        Source.NewUsuNombre,
        Source.NewUsuApellido,
        Source.NewUsuMail,
        Source.NewUsuDocumento,
        Source.NewUsuTerminosYCondiciones,
        Source.NewUsuOrgId,
        Source.NewUsuCatId,
        Source.NewUsuAreaId,
        Source.NewUsuLegajo
    );

SET IDENTITY_INSERT [dbo].[SIS_Usuarios] OFF;