CREATE TABLE [dbo].[Tipo_Estado_Ubicacion_Logistica] (
    [teubilog_id]          INT           IDENTITY (1, 1) NOT NULL,
    [teubilog_nombre]      NVARCHAR (50) NULL,
    [teubilog_org_id]      INT           NULL,
    [teubilog_usu_id_alta] INT           NULL,
    [teubilog_fec_alta]    DATETIME      NULL,
    [teubilog_usu_id_mod]  INT           NULL,
    [teubilog_fec_mod]     DATETIME      NULL,
    [teubilog_activo]      BIT           NULL,
    [teubilog_usu_id_baja] INT           NULL,
    [teubilog_fec_baja]    DATETIME      NULL,
    CONSTRAINT [PK_Tipo_Estado_Ubicacion_Logistica] PRIMARY KEY CLUSTERED ([teubilog_id] ASC)
);

