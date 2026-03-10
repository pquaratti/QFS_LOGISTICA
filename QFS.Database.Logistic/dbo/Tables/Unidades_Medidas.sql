CREATE TABLE [dbo].[Unidades_Medidas] (
    [unimed_id]          INT           IDENTITY (1, 1) NOT NULL,
    [unimed_org_id]      INT           NOT NULL,
    [unimed_nombre]      NVARCHAR (80) NOT NULL,
    [unimed_codigo]      NVARCHAR (20) NOT NULL,
    [unimed_activo]      BIT           CONSTRAINT [DF_Unidades_Medidas_Activo] DEFAULT ((1)) NOT NULL,
    [unimed_fec_alta]    DATETIME      CONSTRAINT [DF_Unidades_Medidas_FecAlta] DEFAULT (getdate()) NOT NULL,
    [unimed_fec_mod]     DATETIME      NULL,
    [unimed_fec_baja]    DATETIME      NULL,
    [unimed_usu_id_alta] INT           NULL,
    [unimed_usu_id_mod]  INT           NULL,
    [unimed_usu_id_baja] INT           NULL,
    CONSTRAINT [PK_Unidades_Medidas] PRIMARY KEY CLUSTERED ([unimed_id] ASC),
    CONSTRAINT [FK_Unidades_Medidas_Organizacion] FOREIGN KEY ([unimed_org_id]) REFERENCES [dbo].[SIS_Organizaciones] ([org_id])
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Unidades_Medidas_Org_Codigo]
    ON [dbo].[Unidades_Medidas]([unimed_org_id] ASC, [unimed_codigo] ASC);
