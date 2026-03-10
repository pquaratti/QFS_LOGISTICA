CREATE TABLE [dbo].[Tipo_Consultas_Ayuda] (
    [tipoconsulta_id]     INT            NOT NULL,
    [tipoconsulta_nombre] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Tipo_Consultas_Ayuda] PRIMARY KEY CLUSTERED ([tipoconsulta_id] ASC)
);

