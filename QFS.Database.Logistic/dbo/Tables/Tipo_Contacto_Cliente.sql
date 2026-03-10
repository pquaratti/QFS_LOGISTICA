CREATE TABLE [dbo].[Tipo_Contacto_Cliente] (
    [tipcontcli_id]     INT           NOT NULL,
    [tipcontcli_nombre] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Tipo_Contacto_Cliente] PRIMARY KEY CLUSTERED ([tipcontcli_id] ASC)
);

