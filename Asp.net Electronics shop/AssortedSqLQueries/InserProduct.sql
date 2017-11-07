CREATE PROC uspInsertProduct

	@prodname varchar(50),
	@price decimal (5,2),
	@discount decimal (5,2),
	@image varchar(50),
	@description xml,
	@categoryID int,
	@supplierID int
AS
INSERT INTO Product VALUES (@prodname, @price, @discount,@image, @description, @categoryID, @supplierID)
GO