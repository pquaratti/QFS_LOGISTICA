CREATE TABLE [dbo].[SIS_Provincias] (
    [prv_id]     INT            IDENTITY (1, 1) NOT NULL,
    [prv_nombre] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Provincias] PRIMARY KEY CLUSTERED ([prv_id] ASC)
);

