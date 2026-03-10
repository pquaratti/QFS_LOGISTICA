CREATE TABLE [dbo].[Tipo_Prioridad] (
    [tprioridad_id]             INT            NOT NULL,
    [tprioridad_nombre]         NVARCHAR (100) NULL,
    [tprioridad_orden]          INT            NULL,
    [tprioridad_css_text]       NVARCHAR (50)  NULL,
    [tprioridad_css_background] NVARCHAR (50)  NULL,
    [tprioridad_css]            NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Tipo_Prioridad] PRIMARY KEY CLUSTERED ([tprioridad_id] ASC)
);

