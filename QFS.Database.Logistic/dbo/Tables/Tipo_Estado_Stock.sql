CREATE TABLE [dbo].[Tipo_Estado_Stock] (
    [testk_id]          INT           IDENTITY (1, 1) NOT NULL,
    [testk_nombre]      NVARCHAR (50) NULL,
    [testk_org_id]      INT           NULL,
    [testk_usu_id_alta] INT           NULL,
    [testk_fec_alta]    DATETIME      NULL,
    [testk_usu_id_mod]  INT           NULL,
    [testk_fec_mod]     DATETIME      NULL,
    [testk_activo]      BIT           NULL,
    [testk_usu_id_baja] INT           NULL,
    [testk_fec_baja]    DATETIME      NULL,
    CONSTRAINT [PK_Tipo_Estado_Stock] PRIMARY KEY CLUSTERED ([testk_id] ASC)
);
