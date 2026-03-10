CREATE TABLE [dbo].[Mesa_Ayuda] (
    [mesa_id]                 INT            IDENTITY (1, 1) NOT NULL,
    [mesa_fecha]              DATETIME       NULL,
    [mesa_problema]           NVARCHAR (MAX) NULL,
    [mesa_solucion]           NVARCHAR (MAX) NULL,
    [mesa_tipoconsulta_id]    INT            NULL,
    [mesa_usu_id_alta]        INT            NULL,
    [mesa_fec_alta]           DATETIME       NULL,
    [mesa_usu_id_mod]         INT            NULL,
    [mesa_fec_mod]            DATETIME       NULL,
    [mesa_fec_cerrada]        DATETIME       NULL,
    [mesa_usu_id_solicita]    INT            NULL,
    [mesa_usu_id_responsable] INT            NULL,
    CONSTRAINT [PK_Mesa_Ayuda] PRIMARY KEY CLUSTERED ([mesa_id] ASC)
);

