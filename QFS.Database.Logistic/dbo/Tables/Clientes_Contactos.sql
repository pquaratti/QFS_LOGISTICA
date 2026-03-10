CREATE TABLE [dbo].[Clientes_Contactos] (
    [clicont_id]            INT            IDENTITY (1, 1) NOT NULL,
    [clicont_cli_id]        INT            NULL,
    [clicont_tipcontcli_id] INT            NULL,
    [clicont_contenido]     NVARCHAR (200) NULL,
    [clicont_detalle]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Clientes_Contactos] PRIMARY KEY CLUSTERED ([clicont_id] ASC)
);

