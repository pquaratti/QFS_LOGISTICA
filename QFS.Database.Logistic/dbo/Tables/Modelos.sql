CREATE TABLE [dbo].[Modelos] (
    [modelo_id]          INT            IDENTITY (1, 1) NOT NULL,
    [modelo_nombre]      NVARCHAR (150) NULL,
    [modelo_marca_id]    INT            NULL,
    [modelo_activo]      BIT            NULL,
    [modelo_usu_id_alta] INT            NULL,
    [modelo_fec_alta]    DATETIME       NULL,
    [model_usu_id_mod]   INT            NULL,
    [modelo_fec_mod]     DATETIME       NULL,
    [modelo_usu_id_baja] INT            NULL,
    [modelo_fec_baja]    DATETIME       NULL,
    CONSTRAINT [PK_Modelos] PRIMARY KEY CLUSTERED ([modelo_id] ASC)
);

