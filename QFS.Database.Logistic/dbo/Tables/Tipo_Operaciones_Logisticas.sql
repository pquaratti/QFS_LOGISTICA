CREATE TABLE [dbo].[Tipo_Operaciones_Logisticas] (
    [topelog_id]            INT            IDENTITY (1, 1) NOT NULL,
    [topelog_nombre]        NVARCHAR (100) NULL,
    [topelog_observaciones] NVARCHAR (MAX) NULL,
    [topelog_org_id]        INT            NULL,
    [topelog_usu_id_alta]   INT            NULL,
    [topelog_fec_alta]      DATETIME       NULL,
    [topelog_activo]        BIT            NULL,
    [topelog_fec_mod]       DATETIME       NULL,
    [topelog_usu_id_mod]    INT            NULL,
    [topelog_usu_id_baja]   INT            NULL,
    [topelog_fec_baja]      DATETIME       NULL,
    CONSTRAINT [PK_Tipo_Operaciones_Logisticas] PRIMARY KEY CLUSTERED ([topelog_id] ASC)
);

