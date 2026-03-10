CREATE TABLE [dbo].[Zonas_Logisticas] (
    [zonlog_id]          INT            IDENTITY (1, 1) NOT NULL,
    [zonlog_org_id]      INT            NULL,
    [zonlog_nombre]      NVARCHAR (50)  NULL,
    [zonlog_descripcion] NVARCHAR (MAX) NULL,
    [zonlog_codigo]      NVARCHAR (20)  NULL,
    [zonlog_usu_id_alta] INT            NULL,
    [zonlog_fec_alta]    DATETIME       NULL,
    [zonlog_usu_id_mod]  INT            NULL,
    [zonlog_fec_mod]     DATETIME       NULL,
    [zonlog_activo]      BIT            NULL,
    [zonlog_usu_id_baja] INT            NULL,
    [zonlog_fec_baja]    DATETIME       NULL,
    CONSTRAINT [PK_Zonas_Logisticas] PRIMARY KEY CLUSTERED ([zonlog_id] ASC)
);

