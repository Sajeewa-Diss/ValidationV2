CREATE PROCEDURE [dbo].[proc_CustByLastName]
	@lastname nvarchar(255)
AS
	SELECT [Id], [Title], [FirstName], [LastName], [AddressId], [PersonId], [Street], [Postcode] 
	FROM dbo.FullCust WHERE [Lastname] = @lastname;

