CREATE TABLE Product(
	ProductID int IDENTITY(1,1) PRIMARY KEY,	
	ProdName varchar(50) NOT NULL,
	Price decimal (5,2),
	Discount decimal (5,2),
	ImageUrl varchar(50) NOt NULL,
	Description xml,	
	CategoryID int NOT NULL,
	SupplierID int NOT NULL,
	ProductImage varchar(120),	
	FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
	FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID) 
)