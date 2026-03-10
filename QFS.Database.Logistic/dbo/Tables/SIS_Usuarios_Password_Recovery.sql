CREATE TABLE [dbo].[SIS_Usuarios_Password_Recovery] (
    [upr_id]             INT            IDENTITY (1, 1) NOT NULL,
    [upr_fec_ini]        DATETIME       NULL,
    [upr_usu_id]         INT            NULL,
    [upr_mail]           NVARCHAR (100) NULL,
    [upr_fec_fin]        DATETIME       NULL,
    [upr_verify_code]    NVARCHAR (6)   NULL,
    [upr_new_password]   NVARCHAR (300) NULL,
    [upr_recovery_token] NVARCHAR (40)  NULL,
    CONSTRAINT [PK_SIS_Usuarios_Password_Recovery] PRIMARY KEY CLUSTERED ([upr_id] ASC)
);

