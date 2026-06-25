CREATE TABLE [dbo].[Tipo_Estado_Ingreso] (
    [teing_id]          INT           IDENTITY (1, 1) NOT NULL,
    [teing_nombre]      NVARCHAR (50) NULL,
    [teing_org_id]      INT           NULL,
    [teing_usu_id_alta] INT           NULL,
    [teing_fec_alta]    DATETIME      NULL,
    [teing_usu_id_mod]  INT           NULL,
    [teing_fec_mod]     DATETIME      NULL,
    [teing_activo]      BIT           NULL,
    [teing_usu_id_baja] INT           NULL,
    [teing_fec_baja]    DATETIME      NULL,
    CONSTRAINT [PK_Tipo_Estado_Ingreso] PRIMARY KEY CLUSTERED ([teing_id] ASC)
);
