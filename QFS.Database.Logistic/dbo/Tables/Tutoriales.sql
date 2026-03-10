CREATE TABLE [dbo].[Tutoriales] (
    [tut_id]             INT            IDENTITY (1, 1) NOT NULL,
    [tut_titulo]         NVARCHAR (100) NULL,
    [tut_usu_id_creador] INT            NULL,
    [tut_fec_creador]    DATE           NULL,
    [tut_usu_id_mod]     INT            NULL,
    [tut_fec_mod]        DATE           NULL,
    [tut_acc_id]         INT            NULL,
    [tut_usu_id_baja]    INT            NULL,
    [tut_fecha_baja]     DATE           NULL,
    [tut_activo]         BIT            NULL,
    [tut_archivo]        NVARCHAR (100) NULL,
    [tut_descrip]        NVARCHAR (MAX) NULL,
    [tut_icono]          NVARCHAR (80)  NULL,
    CONSTRAINT [PK_Tutoriales] PRIMARY KEY CLUSTERED ([tut_id] ASC)
);

