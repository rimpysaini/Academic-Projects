CREATE PROC uspCreateProduct
AS
CREATE TABLE Product(
	ProductID int IDENTITY(1,1) PRIMARY KEY,	
	ProdName varchar(50) NOT NULL,
	Price decimal (5,2),
	Discount decimal (5,2),
	ImageUrl varchar(50) NOT NULL,
	ProductDescription xml,	
	CategoryID int NOT NULL,
	SupplierID int NOT NULL,
	FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
	FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID) 
)
GO
EXEC uspCreateProduct
GO