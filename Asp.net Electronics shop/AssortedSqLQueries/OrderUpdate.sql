select ct.CartID,ct.OrderStatusID,ut.TranDate,ct.DeliveryStatusID,c.FirstName,c.LastName,c.Email,c.Phone,c.Street1,c.Street2,c.Town,ut.TranCost

from Cart ct
inner join  Customer c on ct.CustomerID = c.CustomerID
inner join  UserTransaction ut on ut.CartID = ct.CartID

