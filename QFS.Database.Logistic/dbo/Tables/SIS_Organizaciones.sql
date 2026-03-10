CREATE TABLE [dbo].[SIS_Organizaciones] (
    [org_id]          INT            IDENTITY (1, 1) NOT NULL,
    [org_nombre]      NVARCHAR (150) NULL,
    [org_abreviatura] NVARCHAR (100) NULL,
    [org_mail]        NVARCHAR (200) NULL,
    [org_activo]      BIT            NULL,
    [org_usu_id_alta] INT            NULL,
    [org_fec_alta]    DATETIME       NULL,
    [org_usu_id_mod]  INT            NULL,
    [org_fec_mod]     DATETIME       NULL,
    [org_fec_baja]    DATETIME       NULL,
    [org_usu_id_baja] INT            NULL,
    CONSTRAINT [PK_Organizaciones] PRIMARY KEY CLUSTERED ([org_id] ASC)
);

