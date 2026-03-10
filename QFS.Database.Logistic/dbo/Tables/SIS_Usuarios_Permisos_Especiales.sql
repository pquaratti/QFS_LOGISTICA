CREATE TABLE [dbo].[SIS_Usuarios_Permisos_Especiales] (
    [upe_id]     INT IDENTITY (1, 1) NOT NULL,
    [upe_usu_id] INT NULL,
    [upe_pee_id] INT NULL,
    CONSTRAINT [PK_SIS_Usuarios_Permisos_Especiales] PRIMARY KEY CLUSTERED ([upe_id] ASC)
);

