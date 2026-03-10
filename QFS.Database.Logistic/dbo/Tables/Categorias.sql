CREATE TABLE [dbo].[Categorias] (
    [cat_id]          INT            IDENTITY (1, 1) NOT NULL,
    [cat_org_id]      INT            NULL,
    [cat_nombre]      NVARCHAR (100) NULL,
    [cat_descripcion] NVARCHAR (200) NULL,
    [cat_activo]      BIT            NULL,
    CONSTRAINT [PK_Categorias_Colaborador] PRIMARY KEY CLUSTERED ([cat_id] ASC)
);

