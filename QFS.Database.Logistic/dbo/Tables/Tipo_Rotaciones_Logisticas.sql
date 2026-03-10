CREATE TABLE [dbo].[Tipo_Rotaciones_Logisticas] (
    [trolog_id]          INT            IDENTITY (1, 1) NOT NULL,
    [trolog_org_id]      INT            NULL,
    [trolog_codigo]      NVARCHAR (20)  NULL,
    [trolog_nombre]      NVARCHAR (100) NULL,
    [trolog_descripcion] NVARCHAR (MAX) NULL,
    [trolog_usu_id_alta] INT            NULL,
    [trolog_fec_alta]    DATETIME       NULL,
    [trolog_usu_id_mod]  INT            NULL,
    [trolog_fec_mod]     DATETIME       NULL,
    [trolog_activo]      BIT            NULL,
    [trolog_usu_id_baja] INT            NULL,
    [trolog_fec_baja]    DATETIME       NULL,
    CONSTRAINT [PK_Tipo_Rotaciones_Logisticas] PRIMARY KEY CLUSTERED ([trolog_id] ASC)
);

