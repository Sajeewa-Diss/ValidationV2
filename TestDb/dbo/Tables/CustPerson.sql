CREATE TABLE [dbo].[CustPerson] (
    [PersonId]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (50)  NULL,
    [FirstName]       NVARCHAR (255) NULL,
    [LastName]        NVARCHAR (255) NULL,
    [DOB]             DATETIME2 (7)  NULL,
    [FavouriteColour] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([PersonId] ASC)
);




