CREATE TABLE [dbo].[SIS_Permisos_Especiales] (
    [pee_id]          INT            NOT NULL,
    [pee_nombre]      NVARCHAR (200) NULL,
    [pee_activo]      BIT            NULL,
    [pee_descripcion] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SIS_Permisos_Especiales] PRIMARY KEY CLUSTERED ([pee_id] ASC)
);

