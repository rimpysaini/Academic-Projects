ALTER TABLE Customer
ADD Street1 varchar(50),
Street2 varchar(50),
Town varchar(50),
County varchar(50);

DROP Table County;

DROP Table Address;

DROP table ProductImage;

GO

ALTER PROC uspInsertCustomerandCart
	@firstname varchar(50),
	@lastname varchar(50),
	@email varchar(50),
	@phone varchar(50),
	@dateregistered date,
	@isactivated bit,
	@lastlogin date,
	@userpass varchar(max),
	@securityquestion varchar(max),
	@securityanswer varchar(max),
	@address1 varchar(50),
	@address2 varchar(50),
	@town varchar(50),
	@county varchar(50),
	@islogged bit,
	@roleid int,
	@totalcost decimal (5,2),
	@orderstatusid int,
	@deliverystatusid int,
	@shippingid int
	
AS
INSERT INTO Customer VALUES (@firstname, @lastname, @email, @phone, @dateregistered, @isactivated,@lastlogin, @userpass, @islogged, @roleid, @securityquestion, @securityanswer, @address1, @address2, @town, @county)
INSERT INTO Cart (CustomerID, TotalCost, OrderStatusID, DeliveryStatusID, ShippingID) VALUES ((SELECT CustomerID FROM Customer WHERE Email=@email), @totalcost, @orderstatusid, @deliverystatusid, @shippingid)

GO
CREATE PROCEDURE uspSelectItemsFromCart
@email varchar(50)
AS
SELECT Product.ProductID, Items.Quantity, Product.ProdName, Product.Price, Product.CategoryID, Product.Discount, Product.SupplierID, Product.ProductDescription, Product.ImageUrl, Supplier.SupName
FROM Product INNER JOIN Supplier ON Product.SupplierID = Supplier.SupplierID INNER JOIN Items ON Product.ProductID = Items.ProductID INNER JOIN Cart on Items.CartID = Cart.CartID Inner join Customer on Cart.CustomerID = Customer.CustomerID
WHERE Customer.Email = @email AND Cart.OrderStatusID = 1