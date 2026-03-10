CREATE TABLE [dbo].[Marcas] (
    [marca_id]          INT           IDENTITY (1, 1) NOT NULL,
    [marca_nombre]      NVARCHAR (50) NULL,
    [marca_activo]      BIT           NULL,
    [marca_usu_id_alta] INT           NULL,
    [marca_fec_alta]    DATETIME      NULL,
    [marca_usu_id_mod]  INT           NULL,
    [marca_fec_mod]     DATETIME      NULL,
    [marca_fec_baja]    DATETIME      NULL,
    [marca_usu_id_baja] INT           NULL,
    CONSTRAINT [PK_Marcas] PRIMARY KEY CLUSTERED ([marca_id] ASC)
);

