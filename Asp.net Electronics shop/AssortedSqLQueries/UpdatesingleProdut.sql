CREATE PROCEDURE uspUpdateSingleProduct
       @Id INT,
	   @prodName varchar(50),
	   @price decimal(8,2),
	   @discount decimal(5,2),
	   @imageUrl varchar(120),
	   @catId int,
	   @supId int
  AS
    BEGIN
     UPDATE Product 
     SET ProdName = @prodName,Price = @price,Discount = @discount,ImageUrl = @imageUrl,CategoryID = @catId,SupplierID = @supId
     WHERE ProductID = @Id
    END