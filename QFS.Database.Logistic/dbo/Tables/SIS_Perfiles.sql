CREATE TABLE [dbo].[SIS_Perfiles] (
    [prf_id]           INT        IDENTITY (1, 1) NOT NULL,
    [prf_nombre]       NCHAR (80) NULL,
    [prf_mod_id]       INT        NULL,
    [prf_activo]       BIT        NULL,
    [prf_usu_id_alta]  INT        NULL,
    [prf_usu_id_mod]   INT        NULL,
    [prf_usu_id_baja]  INT        NULL,
    [prf_usu_fec_alta] DATETIME   NULL,
    [prf_usu_fec_mod]  DATETIME   NULL,
    [prf_usu_fec_baja] DATETIME   NULL,
    CONSTRAINT [PK_SIS_Perfiles] PRIMARY KEY CLUSTERED ([prf_id] ASC)
);

