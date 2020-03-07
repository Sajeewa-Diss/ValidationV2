CREATE VIEW [dbo].[FullCust]
	AS SELECT [p].[PersonId] AS Id, [p].[Title], [p].[FirstName], [p].[LastName],
	[a].[Id] AS AddressId, [a].[PersonId], [a].[Street], [a].[Postcode] FROM [CustPerson] p LEFT JOIN [CustAddress] a
	ON p.[PersonId] = a.PersonId
