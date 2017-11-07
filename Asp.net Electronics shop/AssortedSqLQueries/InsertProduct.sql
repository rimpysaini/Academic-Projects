CREATE PROC uspInsertProduct

	@prodname varchar(50),
	@price decimal (5,2),
	@discount decimal (5,2),
	@description XML,
	@categoryID int,
	@supplierID int
AS
INSERT INTO Product VALUES (@prodname, @price, @discount, @description,@categoryID, @supplierID)