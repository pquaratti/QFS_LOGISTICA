CREATE TABLE [dbo].[SIS_Usuarios] (
    [usu_id]                     INT            IDENTITY (1, 1) NOT NULL,
    [usu_nickname]               NVARCHAR (200) NULL,
    [usu_password]               NVARCHAR (300) NULL,
    [usu_fec_last_logon]         DATETIME       NULL,
    [usu_fec_eliminado]          DATETIME       NULL,
    [usu_fec_pass_changed]       DATETIME       NULL,
    [usu_fec_bloqueado]          DATETIME       NULL,
    [usu_administrador]          BIT            NULL,
    [usu_intentos]               INT            NULL,
    [usu_nombre]                 NVARCHAR (50)  NULL,
    [usu_apellido]               NVARCHAR (50)  NULL,
    [usu_mail]                   NVARCHAR (50)  NULL,
    [usu_documento]              NVARCHAR (50)  NULL,
    [usu_terminos_y_condiciones] DATETIME       NULL,
    [usu_org_id]                 INT            NULL,
    [usu_cat_id]                 INT            NULL,
    [usu_area_id]                INT            NULL,
    [usu_legajo]                 NVARCHAR (50)  NULL,
    CONSTRAINT [PK_SIS_Usuarios] PRIMARY KEY CLUSTERED ([usu_id] ASC)
);

