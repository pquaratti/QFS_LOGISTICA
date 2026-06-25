CREATE TABLE [dbo].[Depositos_Pasillos]
(
	[depopas_id]                  INT             IDENTITY (1, 1) NOT NULL,
    [depopas_depo_id]             INT             NULL,
    [depopas_org_id]              INT             NULL,
    [depopas_codigo]              NVARCHAR (30)   NULL,
    [depopas_nombre]              NVARCHAR (100)  NULL,
    [depopas_descripcion]         NVARCHAR (MAX)  NULL,
    [depopas_x]                   DECIMAL (18, 2) NULL,
    [depopas_y]                   DECIMAL (18, 2) NULL,
    [depopas_largo]               DECIMAL (18, 2) NULL,
    [depopas_ancho]               DECIMAL (18, 2) NULL,
    [depopas_orientacion]         NVARCHAR (1)    NULL,
    [depopas_cantidad_posiciones] INT             NULL,
    [depopas_cantidad_alturas]    INT             NULL,
    [depopas_altura_nivel]        DECIMAL (18, 2) NULL,
    [depopas_peso_maximo]         DECIMAL (18, 2) NULL,
    [depopas_usu_id_alta]         INT             NULL,
    [depopas_fec_alta]            DATETIME        NULL,
    [depopas_usu_id_mod]          INT             NULL,
    [depopas_fec_mod]             DATETIME        NULL,
    [depopas_fec_baja]            DATETIME        NULL,
    [depopas_usu_id_baja]         INT             NULL,
    [depopas_activo]              BIT             NULL,
    CONSTRAINT [PK_Depositos_Pasillos] PRIMARY KEY CLUSTERED ([depopas_id] ASC),
    CONSTRAINT [FK_Depositos_Pasillos_Depositos] FOREIGN KEY ([depopas_depo_id]) REFERENCES [dbo].[Depositos] ([depo_id])
);
GO

CREATE INDEX [IX_Depositos_Pasillos_Deposito] ON [dbo].[Depositos_Pasillos] ([depopas_depo_id], [depopas_activo]);
