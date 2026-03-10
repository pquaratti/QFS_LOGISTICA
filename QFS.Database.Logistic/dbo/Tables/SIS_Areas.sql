CREATE TABLE [dbo].[SIS_Areas] (
    [area_id]          INT            IDENTITY (1, 1) NOT NULL,
    [area_nombre]      NVARCHAR (200) NULL,
    [area_abreviatura] NVARCHAR (50)  NULL,
    [area_area_id]     INT            NULL,
    [area_org_id]      INT            NULL,
    [area_usu_id_alta] INT            NULL,
    [area_usu_id_mod]  INT            NULL,
    [area_usu_id_baja] INT            NULL,
    [area_fec_alta]    DATETIME       NULL,
    [area_fec_baja]    DATETIME       NULL,
    [area_fec_mod]     DATETIME       NULL,
    [area_activo]      BIT            NULL,
    CONSTRAINT [PK_SIS_Areas] PRIMARY KEY CLUSTERED ([area_id] ASC)
);

