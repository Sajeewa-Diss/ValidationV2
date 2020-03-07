CREATE TABLE [dbo].[CustAddress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PersonId] INT NOT NULL,
	[Street] NVarchar(255) NULL,
	[Postcode]	Nvarchar(20) NULL, 
    CONSTRAINT [FK_CustAddress_ToCustPerson] FOREIGN KEY ([PersonId]) REFERENCES [CustPerson]([PersonId])
)
