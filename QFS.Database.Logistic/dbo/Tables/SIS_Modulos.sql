CREATE TABLE [dbo].[SIS_Modulos] (
    [mod_id]          INT            NOT NULL,
    [mod_nombre]      NVARCHAR (100) NULL,
    [mod_descripcion] NVARCHAR (MAX) NULL,
    [mod_activo]      BIT            NULL,
    CONSTRAINT [PK_SIS_Modulos] PRIMARY KEY CLUSTERED ([mod_id] ASC)
);

