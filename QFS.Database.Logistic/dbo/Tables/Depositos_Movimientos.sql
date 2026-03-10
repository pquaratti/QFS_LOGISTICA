CREATE TABLE [dbo].[Depositos_Movimientos] (
    [depomov_id]          INT             IDENTITY (1, 1) NOT NULL,
    [depomov_depo_id]     INT             NULL,
    [depomov_item_id]     INT             NULL,
    [depomov_cantidad]    DECIMAL (18, 2) NULL,
    [depomov_usu_id]      INT             NULL,
    [depomov_fecha]       DATETIME        NULL,
    [depomov_descripcion] NVARCHAR (200)  NULL,
    CONSTRAINT [PK_Depositos_Movimientos] PRIMARY KEY CLUSTERED ([depomov_id] ASC)
);

