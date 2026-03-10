SET IDENTITY_INSERT [dbo].[SIS_Provincias] ON;

MERGE INTO [dbo].[SIS_Provincias] AS Target
USING (
    VALUES
        (2, N'Ciudad de Buenos Aires'),
        (6, N'Buenos Aires'),
        (10, N'Catamarca'),
        (14, N'Córdoba'),
        (18, N'Corrientes'),
        (22, N'Chaco'),
        (26, N'Chubut'),
        (30, N'Entre Ríos'),
        (34, N'Formosa'),
        (38, N'Jujuy'),
        (42, N'La Pampa'),
        (46, N'La Rioja'),
        (50, N'Mendoza'),
        (54, N'Misiones'),
        (58, N'Neuquén'),
        (62, N'Río Negro'),
        (66, N'Salta'),
        (70, N'San Juan'),
        (74, N'San Luis'),
        (78, N'Santa Cruz'),
        (82, N'Santa Fe'),
        (86, N'Santiago del Estero'),
        (90, N'Tucumán'),
        (94, N'Tierra del Fuego')
) AS Source (NewPrvId, NewPrvNombre)
ON Target.[prv_id] = Source.NewPrvId

WHEN MATCHED THEN
    UPDATE SET
        Target.[prv_nombre] = Source.NewPrvNombre

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [prv_id],
        [prv_nombre]
    )
    VALUES (
        Source.NewPrvId,
        Source.NewPrvNombre
    );

SET IDENTITY_INSERT [dbo].[SIS_Provincias] OFF;