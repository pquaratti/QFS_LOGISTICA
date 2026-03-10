CREATE TABLE [dbo].[Mesa_Ayuda_Interaccion] (
    [mesainteraccion_id]      INT            IDENTITY (1, 1) NOT NULL,
    [mesainteraccion_mesa_id] INT            NULL,
    [mesainteraccion_usu_id]  INT            NULL,
    [mesainteraccion_fecha]   DATETIME       NULL,
    [mesainteraccion_mensaje] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Mesa_Ayuda_Interaccion] PRIMARY KEY CLUSTERED ([mesainteraccion_id] ASC)
);

