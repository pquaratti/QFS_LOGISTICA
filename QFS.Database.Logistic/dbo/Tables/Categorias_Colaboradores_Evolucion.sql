CREATE TABLE [dbo].[Categorias_Colaboradores_Evolucion] (
    [cce_id]         INT            IDENTITY (1, 1) NOT NULL,
    [cce_usu_id]     INT            NULL,
    [cce_cat_id]     INT            NULL,
    [cce_usu_id_mod] INT            NULL,
    [cce_fecha]      DATETIME       NULL,
    [cce_doc]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Categorias_Colaboradores_Evolucion] PRIMARY KEY CLUSTERED ([cce_id] ASC)
);

