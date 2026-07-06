CREATE TABLE [dbo].[Ubicaciones_Productos]
(
	[ubipro_id]            INT             IDENTITY (1, 1) NOT NULL,
    [ubipro_ubilog_id]     INT             NOT NULL,
    [ubipro_pro_id]        INT             NOT NULL,
    [ubipro_cantidad]      DECIMAL (18, 4) CONSTRAINT [DF_Ubicaciones_Productos_Cantidad] DEFAULT ((0)) NOT NULL,
    [ubipro_cantidad_maxima] INT             CONSTRAINT [DF_Ubicaciones_Productos_CantidadMaxima] DEFAULT ((0)) NOT NULL,
    [ubipro_lote]          NVARCHAR (80)   NULL,
    [ubipro_nro_serie]     NVARCHAR (80)   NULL,
    [ubipro_fec_vencimiento] DATETIME      NULL,
    [ubipro_activo]        BIT             CONSTRAINT [DF_Ubicaciones_Productos_Activo] DEFAULT ((1)) NOT NULL,
    [ubipro_fec_alta]      DATETIME        CONSTRAINT [DF_Ubicaciones_Productos_FecAlta] DEFAULT (getdate()) NOT NULL,
    [ubipro_fec_mod]       DATETIME        NULL,
    [ubipro_fec_baja]      DATETIME        NULL,
    [ubipro_usu_id_alta]   INT             NULL,
    [ubipro_usu_id_mod]    INT             NULL,
    [ubipro_usu_id_baja]   INT             NULL
)
