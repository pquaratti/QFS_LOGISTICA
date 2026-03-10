CREATE TABLE [dbo].[SIS_Perfiles_Acciones] (
    [pac_id]     INT IDENTITY (1, 1) NOT NULL,
    [pac_acc_id] INT NULL,
    [pac_prf_id] INT NULL,
    CONSTRAINT [PK_SIS_Perfiles_Acciones] PRIMARY KEY CLUSTERED ([pac_id] ASC)
);

