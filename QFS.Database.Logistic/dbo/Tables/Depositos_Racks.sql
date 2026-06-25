CREATE TABLE [dbo].[Depositos_Racks]
(
    [deprack_id]                 INT             IDENTITY (1, 1) NOT NULL,
    [deprack_depo_id]            INT             NULL,
    [deprack_zonlog_id]          INT             NULL,
    [deprack_pasillo_id]         INT             NULL,
    [deprack_org_id]             INT             NULL,
    [deprack_codigo]             NVARCHAR (30)   NULL,
    [deprack_nombre]             NVARCHAR (100)  NULL,
    [deprack_descripcion]        NVARCHAR (MAX)  NULL,
    [deprack_x]                  DECIMAL (18, 2) NULL,
    [deprack_y]                  DECIMAL (18, 2) NULL,
    [deprack_largo]              DECIMAL (18, 2) NULL,
    [deprack_ancho]              DECIMAL (18, 2) NULL,
    [deprack_orientacion]        NVARCHAR (1)    NULL,
    [deprack_cantidad_columnas]  INT             NULL,
    [deprack_cantidad_niveles]   INT             NULL,
    [deprack_altura_nivel]       DECIMAL (18, 2) NULL,
    [deprack_peso_maximo]        DECIMAL (18, 2) NULL,
    [deprack_color]              NVARCHAR (20)   NULL,
    [deprack_usu_id_alta]        INT             NULL,
    [deprack_fec_alta]           DATETIME        NULL,
    [deprack_usu_id_mod]         INT             NULL,
    [deprack_fec_mod]            DATETIME        NULL,
    [deprack_fec_baja]           DATETIME        NULL,
    [deprack_usu_id_baja]        INT             NULL,
    [deprack_activo]             BIT             NULL,
    CONSTRAINT [PK_Depositos_Racks] PRIMARY KEY CLUSTERED ([deprack_id] ASC),
    CONSTRAINT [FK_Depositos_Racks_Depositos] FOREIGN KEY ([deprack_depo_id]) REFERENCES [dbo].[Depositos] ([depo_id])
);
GO

CREATE INDEX [IX_Depositos_Racks_Deposito] ON [dbo].[Depositos_Racks] ([deprack_depo_id], [deprack_activo]);
