CREATE TABLE [dbo].[Tipo_Estado_Pedido_Salida] (
    [tepsa_id]          INT           IDENTITY (1, 1) NOT NULL,
    [tepsa_nombre]      NVARCHAR (50) NULL,
    [tepsa_org_id]      INT           NULL,
    [tepsa_usu_id_alta] INT           NULL,
    [tepsa_fec_alta]    DATETIME      NULL,
    [tepsa_usu_id_mod]  INT           NULL,
    [tepsa_fec_mod]     DATETIME      NULL,
    [tepsa_activo]      BIT           NULL,
    [tepsa_usu_id_baja] INT           NULL,
    [tepsa_fec_baja]    DATETIME      NULL,
    CONSTRAINT [PK_Tipo_Estado_Pedido_Salida] PRIMARY KEY CLUSTERED ([tepsa_id] ASC)
);
