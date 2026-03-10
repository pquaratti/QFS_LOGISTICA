CREATE TABLE [dbo].[Preguntas_Frecuentes_Categorias] (
    [pgfc_id]     INT            NOT NULL,
    [pgfc_nombre] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Preguntas_Frecuentes_Categorias] PRIMARY KEY CLUSTERED ([pgfc_id] ASC)
);

