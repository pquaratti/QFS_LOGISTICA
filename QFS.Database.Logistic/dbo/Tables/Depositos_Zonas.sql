CREATE TABLE [dbo].[Depositos_Zonas]
(
    [depzon_id]          INT             IDENTITY (1, 1) NOT NULL,
    [depzon_depo_id]     INT             NULL,
    [depzon_org_id]      INT             NULL,
    [depzon_codigo]      NVARCHAR (30)   NULL,
    [depzon_nombre]      NVARCHAR (100)  NULL,
    [depzon_descripcion] NVARCHAR (MAX)  NULL,
    [depzon_x]           DECIMAL (18, 2) NULL,
    [depzon_y]           DECIMAL (18, 2) NULL,
    [depzon_largo]       DECIMAL (18, 2) NULL,
    [depzon_ancho]       DECIMAL (18, 2) NULL,
    [depzon_usu_id_alta] INT             NULL,
    [depzon_fec_alta]    DATETIME        NULL,
    [depzon_usu_id_mod]  INT             NULL,
    [depzon_fec_mod]     DATETIME        NULL,
    [depzon_fec_baja]    DATETIME        NULL,
    [depzon_usu_id_baja] INT             NULL,
    [depzon_activo]      BIT             NULL,
    CONSTRAINT [PK_Depositos_Zonas] PRIMARY KEY CLUSTERED ([depzon_id] ASC),
    CONSTRAINT [FK_Depositos_Zonas_Depositos] FOREIGN KEY ([depzon_depo_id]) REFERENCES [dbo].[Depositos] ([depo_id])
);
GO

CREATE INDEX [IX_Depositos_Zonas_Deposito] ON [dbo].[Depositos_Zonas] ([depzon_depo_id], [depzon_activo]);