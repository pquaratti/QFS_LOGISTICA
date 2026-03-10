CREATE TABLE [dbo].[SIS_Localidades] (
    [loc_id]     INT            IDENTITY (1, 1) NOT NULL,
    [loc_prv_id] INT            NULL,
    [loc_nombre] NVARCHAR (100) NULL,
    CONSTRAINT [PK_SIS_Localidades] PRIMARY KEY CLUSTERED ([loc_id] ASC)
);

