CREATE TABLE [dbo].[Plantas] (
    [planta_id]          INT            IDENTITY (1, 1) NOT NULL,
    [planta_nombre]      NVARCHAR (100) NULL,
    [planta_descripcion] NVARCHAR (MAX) NULL,
    [planta_org_id]      INT            NULL,
    [planta_prv_id]      INT            NULL,
    [planta_loc_id]      INT            NULL,
    [planta_direccion]   NVARCHAR (MAX) NULL,
    [planta_latitud]     NVARCHAR (50)  NULL,
    [planta_longitud]    NVARCHAR (50)  NULL,
    [planta_usu_id_alta] INT            NULL,
    [planta_fec_alta]    DATETIME       NULL,
    [planta_usu_id_mod]  INT            NULL,
    [planta_fec_mod]     DATETIME       NULL,
    [planta_usu_id_baja] INT            NULL,
    [planta_fec_baja]    DATETIME       NULL,
    [planta_activo]      BIT            NULL,
    CONSTRAINT [PK_Plantas] PRIMARY KEY CLUSTERED ([planta_id] ASC)
);

