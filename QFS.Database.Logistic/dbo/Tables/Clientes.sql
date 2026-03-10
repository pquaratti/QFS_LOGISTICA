CREATE TABLE [dbo].[Clientes] (
    [cli_id]           INT            IDENTITY (1, 1) NOT NULL,
    [cli_org_id]       INT            NULL,
    [cli_razon_social] NVARCHAR (100) NULL,
    [cli_cuit]         NVARCHAR (20)  NULL,
    [cli_mail]         NVARCHAR (100) NULL,
    [cli_usu_id_alta]  INT            NULL,
    [cli_fec_alta]     DATETIME       NULL,
    [cli_usu_id_mod]   INT            NULL,
    [cli_fec_mod]      DATETIME       NULL,
    [cli_usu_id_baja]  INT            NULL,
    [cli_fec_baja]     DATETIME       NULL,
    [cli_activo]       BIT            NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([cli_id] ASC)
);

