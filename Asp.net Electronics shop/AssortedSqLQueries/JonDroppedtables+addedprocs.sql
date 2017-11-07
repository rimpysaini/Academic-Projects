Drop table UserAccount

Drop table UserCard
GO
CREATE PROC uspUpdateCartOrder
@email varchar(50),
@orderdate date
	AS
	UPDATE Cart
	SET OrderStatusID = 2, OrderDate = @orderdate
WHERE CartID = (SELECT CartID From Cart
Inner join Customer on Cart.CustomerID = Customer.CustomerID
WHERE Customer.Email = @email AND Cart.OrderStatusID = 1)
GO