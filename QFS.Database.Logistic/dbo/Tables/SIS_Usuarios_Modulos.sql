CREATE TABLE [dbo].[SIS_Usuarios_Modulos] (
    [usi_id]         INT      IDENTITY (1, 1) NOT NULL,
    [usi_usu_id]     INT      NULL,
    [usi_mod_id]     INT      NULL,
    [usi_prf_id]     INT      NULL,
    [usi_fec_acceso] DATETIME NULL,
    CONSTRAINT [PK_SIS_Usuarios_Modulos] PRIMARY KEY CLUSTERED ([usi_id] ASC)
);

