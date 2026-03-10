CREATE TABLE [dbo].[Subcategorias_Productos] (
    [subcatpro_id]          INT            IDENTITY (1, 1) NOT NULL,
    [subcatpro_org_id]      INT            NOT NULL,
    [subcatpro_rubpro_id]   INT            NOT NULL,
    [subcatpro_catpro_id]   INT            NOT NULL,
    [subcatpro_nombre]      NVARCHAR (120) NOT NULL,
    [subcatpro_descripcion] NVARCHAR (255) NULL,
    [subcatpro_activo]      BIT            CONSTRAINT [DF_Subcategorias_Productos_Activo] DEFAULT ((1)) NOT NULL,
    [subcatpro_fec_alta]    DATETIME       CONSTRAINT [DF_Subcategorias_Productos_FecAlta] DEFAULT (getdate()) NOT NULL,
    [subcatpro_fec_mod]     DATETIME       NULL,
    [subcatpro_fec_baja]    DATETIME       NULL,
    [subcatpro_usu_id_alta] INT            NULL,
    [subcatpro_usu_id_mod]  INT            NULL,
    [subcatpro_usu_id_baja] INT            NULL,
    CONSTRAINT [PK_Subcategorias_Productos] PRIMARY KEY CLUSTERED ([subcatpro_id] ASC),
    CONSTRAINT [FK_Subcategorias_Productos_Organizacion] FOREIGN KEY ([subcatpro_org_id]) REFERENCES [dbo].[SIS_Organizaciones] ([org_id]),
    CONSTRAINT [FK_Subcategorias_Productos_Rubro] FOREIGN KEY ([subcatpro_rubpro_id]) REFERENCES [dbo].[Rubros_Productos] ([rubpro_id]),
    CONSTRAINT [FK_Subcategorias_Productos_Categoria] FOREIGN KEY ([subcatpro_catpro_id]) REFERENCES [dbo].[Categorias_Productos] ([catpro_id])
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Subcategorias_Productos_Org_Cat_Nombre]
    ON [dbo].[Subcategorias_Productos]([subcatpro_org_id] ASC, [subcatpro_catpro_id] ASC, [subcatpro_nombre] ASC);
