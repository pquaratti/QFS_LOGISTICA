CREATE TABLE [dbo].[SIS_Usuarios_Login] (
    [usl_id]        INT            IDENTITY (1, 1) NOT NULL,
    [usl_usu_id]    INT            NULL,
    [usl_token_id]  NVARCHAR (70)  NULL,
    [usl_fec_ini]   DATETIME       NULL,
    [usl_fec_fin]   DATETIME       NULL,
    [usl_ipv4]      NVARCHAR (20)  NULL,
    [usl_navigator] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SIS_Usuarios_Login] PRIMARY KEY CLUSTERED ([usl_id] ASC)
);

