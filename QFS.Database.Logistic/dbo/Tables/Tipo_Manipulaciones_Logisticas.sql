CREATE TABLE [dbo].[Tipo_Manipulaciones_Logisticas] (
    [tmanilog_id]          INT            IDENTITY (1, 1) NOT NULL,
    [tmanilog_codigo]      NVARCHAR (20)  NULL,
    [tmanilog_org_id]      INT            NULL,
    [tmanilog_nombre]      NVARCHAR (100) NULL,
    [tmanilog_descripcion] NVARCHAR (MAX) NULL,
    [tmanilog_usu_id_alta] INT            NULL,
    [tmanilog_fec_alta]    DATETIME       NULL,
    [tmanilog_usu_id_mod]  INT            NULL,
    [tmanilog_fec_mod]     DATETIME       NULL,
    [tmanilog_activo]      BIT            NULL,
    [tmanilog_usu_id_baja] INT            NULL,
    [tmanilog_fec_baja]    DATETIME       NULL,
    CONSTRAINT [PK_Tipo_Manipulaciones_Logisticas] PRIMARY KEY CLUSTERED ([tmanilog_id] ASC)
);

