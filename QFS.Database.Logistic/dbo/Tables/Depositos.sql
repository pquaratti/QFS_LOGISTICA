CREATE TABLE [dbo].[Depositos] (
    [depo_id]          INT            IDENTITY (1, 1) NOT NULL,
    [depo_nombre]      NVARCHAR (50)  NULL,
    [depo_descripcion] NVARCHAR (MAX) NULL,
    [depo_org_id]      INT            NULL,
    [depo_planta_id]   INT            NULL,
    [depo_usu_id_alta] INT            NULL,
    [depo_fec_alta]    DATETIME       NULL,
    [depo_usu_id_mod]  INT            NULL,
    [depo_fec_mod]     DATETIME       NULL,
    [depo_codigo]      NVARCHAR (10)  NULL,
    [depo_fec_baja]    DATETIME       NULL,
    [depo_usu_id_baja] INT            NULL,
    [depo_activo]      BIT            NULL,
    CONSTRAINT [PK_Depositos] PRIMARY KEY CLUSTERED ([depo_id] ASC)
);

