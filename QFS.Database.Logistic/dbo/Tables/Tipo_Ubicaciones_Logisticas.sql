CREATE TABLE [dbo].[Tipo_Ubicaciones_Logisticas] (
    [tubilog_id]          INT            IDENTITY (1, 1) NOT NULL,
    [tubilog_org_id]      INT            NULL,
    [tubilog_nombre]      NVARCHAR (100) NULL,
    [tubilog_descripcion] NVARCHAR (MAX) NULL,
    [tubilog_usu_id_alta] INT            NULL,
    [tubilog_fec_alta]    DATETIME       NULL,
    [tubilog_usu_id_mod]  INT            NULL,
    [tubilog_fec_mod]     DATETIME       NULL,
    [tubilog_activo]      BIT            NULL,
    [tubilog_usu_id_baja] INT            NULL,
    [tubilog_fec_baja]    DATETIME       NULL,
    CONSTRAINT [PK_Tipo_Ubicaciones_Logisticas] PRIMARY KEY CLUSTERED ([tubilog_id] ASC)
);

