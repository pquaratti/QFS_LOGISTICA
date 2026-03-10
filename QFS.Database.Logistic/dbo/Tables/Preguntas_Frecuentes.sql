CREATE TABLE [dbo].[Preguntas_Frecuentes] (
    [pgf_id]                 INT            IDENTITY (1, 1) NOT NULL,
    [pgf_titulo]             NVARCHAR (150) NULL,
    [pgf_contenido]          NVARCHAR (MAX) NULL,
    [pgf_pgfc_id]            INT            NULL,
    [pgf_usu_id_creador]     INT            NULL,
    [pgf_usu_id_modificador] INT            NULL,
    [pgf_fec_creador]        DATETIME       NULL,
    [pgf_fec_modificador]    DATETIME       NULL,
    [pgf_usu_id_baja]        INT            NULL,
    [pgf_fec_baja]           DATETIME       NULL,
    [pgf_activo]             BIT            NULL,
    CONSTRAINT [PK_Preguntas_Frecuentes] PRIMARY KEY CLUSTERED ([pgf_id] ASC)
);

