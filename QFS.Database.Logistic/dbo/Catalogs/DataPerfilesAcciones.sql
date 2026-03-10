SET IDENTITY_INSERT [dbo].[SIS_Perfiles_Acciones] ON;

MERGE INTO [dbo].[SIS_Perfiles_Acciones] AS Target
USING (
    VALUES
        (1, 1, 1),
        (2, 2, 1),
        (3, 3, 1),
        (4, 4, 1),
        (5, 5, 1),
        (6, 1, 2),
        (7, 10, 2),
        (8, 11, 2),
        (9, 17, 1),
        (10, 16, 2),
        (11, 18, 2),
        (12, 19, 2),
        (13, 20, 2),
        (14, 6, 2),
        (15, 21, 2),
        (16, 22, 2),
        (17, 23, 2),
        (18, 24, 2),
        (19, 25, 2),
        (20, 26, 2),
        (21, 27, 2),
        (22, 28, 2),
        (23, 29, 2),
        (24, 30, 2),
        (29, 33, 2),
        (30, 34, 2),
        (31, 35, 2),
        (32, 36, 2),
        (33, 37, 2)
) AS Source (NewPacId, NewPacAccId, NewPacPrfId)
ON Target.[pac_id] = Source.NewPacId

WHEN MATCHED THEN
    UPDATE SET
        Target.[pac_acc_id] = Source.NewPacAccId,
        Target.[pac_prf_id] = Source.NewPacPrfId

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [pac_id],
        [pac_acc_id],
        [pac_prf_id]
    )
    VALUES (
        Source.NewPacId,
        Source.NewPacAccId,
        Source.NewPacPrfId
    );

SET IDENTITY_INSERT [dbo].[SIS_Perfiles_Acciones] OFF;