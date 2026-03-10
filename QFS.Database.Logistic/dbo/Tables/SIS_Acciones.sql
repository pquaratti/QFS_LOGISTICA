CREATE TABLE [dbo].[SIS_Acciones] (
    [acc_id]          INT            IDENTITY (1, 1) NOT NULL,
    [acc_nombre]      NVARCHAR (150) NULL,
    [acc_descripcion] NVARCHAR (MAX) NULL,
    [acc_controller]  NVARCHAR (150) NULL,
    [acc_accion]      NVARCHAR (150) NULL,
    [acc_id_padre]    INT            NULL,
    [acc_icono]       NVARCHAR (100) NULL,
    [acc_orden]       INT            NULL,
    [acc_menu]        BIT            NULL,
    [acc_fec_baja]    DATETIME       NULL,
    [acc_fec_alta]    DATETIME       NULL,
    [acc_fec_mod]     DATETIME       NULL,
    [acc_usu_id_alta] INT            NULL,
    [acc_usu_id_mod]  INT            NULL,
    [acc_usu_id_baja] INT            NULL,
    [acc_activo]      BIT            NULL,
    CONSTRAINT [PK_SIS_Acciones] PRIMARY KEY CLUSTERED ([acc_id] ASC)
);

