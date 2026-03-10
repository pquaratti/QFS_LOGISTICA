CREATE TABLE [dbo].[Rubros_Productos] (
    [rubpro_id]          INT            IDENTITY (1, 1) NOT NULL,
    [rubpro_org_id]      INT            NOT NULL,
    [rubpro_nombre]      NVARCHAR (120) NOT NULL,
    [rubpro_descripcion] NVARCHAR (255) NULL,
    [rubpro_activo]      BIT            CONSTRAINT [DF_Rubros_Productos_Activo] DEFAULT ((1)) NOT NULL,
    [rubpro_fec_alta]    DATETIME       CONSTRAINT [DF_Rubros_Productos_FecAlta] DEFAULT (getdate()) NOT NULL,
    [rubpro_fec_mod]     DATETIME       NULL,
    [rubpro_fec_baja]    DATETIME       NULL,
    [rubpro_usu_id_alta] INT            NULL,
    [rubpro_usu_id_mod]  INT            NULL,
    [rubpro_usu_id_baja] INT            NULL,
    CONSTRAINT [PK_Rubros_Productos] PRIMARY KEY CLUSTERED ([rubpro_id] ASC),
    CONSTRAINT [FK_Rubros_Productos_Organizacion] FOREIGN KEY ([rubpro_org_id]) REFERENCES [dbo].[SIS_Organizaciones] ([org_id])
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Rubros_Productos_Org_Nombre]
    ON [dbo].[Rubros_Productos]([rubpro_org_id] ASC, [rubpro_nombre] ASC);
