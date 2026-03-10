CREATE TABLE [dbo].[Categorias_Productos] (
    [catpro_id]          INT            IDENTITY (1, 1) NOT NULL,
    [catpro_org_id]      INT            NOT NULL,
    [catpro_rubpro_id]   INT            NOT NULL,
    [catpro_nombre]      NVARCHAR (120) NOT NULL,
    [catpro_descripcion] NVARCHAR (255) NULL,
    [catpro_activo]      BIT            CONSTRAINT [DF_Categorias_Productos_Activo] DEFAULT ((1)) NOT NULL,
    [catpro_fec_alta]    DATETIME       CONSTRAINT [DF_Categorias_Productos_FecAlta] DEFAULT (getdate()) NOT NULL,
    [catpro_fec_mod]     DATETIME       NULL,
    [catpro_fec_baja]    DATETIME       NULL,
    [catpro_usu_id_alta] INT            NULL,
    [catpro_usu_id_mod]  INT            NULL,
    [catpro_usu_id_baja] INT            NULL,
    CONSTRAINT [PK_Categorias_Productos] PRIMARY KEY CLUSTERED ([catpro_id] ASC),
    CONSTRAINT [FK_Categorias_Productos_Organizacion] FOREIGN KEY ([catpro_org_id]) REFERENCES [dbo].[SIS_Organizaciones] ([org_id]),
    CONSTRAINT [FK_Categorias_Productos_Rubro] FOREIGN KEY ([catpro_rubpro_id]) REFERENCES [dbo].[Rubros_Productos] ([rubpro_id])
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Categorias_Productos_Org_Rubro_Nombre]
    ON [dbo].[Categorias_Productos]([catpro_org_id] ASC, [catpro_rubpro_id] ASC, [catpro_nombre] ASC);
