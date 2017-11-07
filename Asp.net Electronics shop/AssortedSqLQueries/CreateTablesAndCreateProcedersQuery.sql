CREATE PROC uspCreateUserRole
AS
CREATE TABLE UserRole(
	RoleID int IDENTITY(1,1) PRIMARY KEY,
	RoleName varchar(50) NOT NULL,
)
GO
EXEC uspCreateUserRole
GO
CREATE PROC uspCreateCustomer
AS
CREATE TABLE Customer(
	CustomerID int IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	Email varchar(50) NOT NULL UNIQUE,	
	Phone varchar(50) NOT NULL,
	DateRegistered date NOT NULL,
	IsActivated bit NOT NULL DEFAULT 0,
	LastLogin date NOT NULL,
	UserPass varchar(max) NOT NULL,
	IsLogged bit NOT NULL DEFAULT 0,
	RoleID int NOT NULL,
	FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID)	
)
GO
EXEC uspCreateCustomer
GO
CREATE PROC uspCreateCounty
AS
CREATE TABLE County(
	CountyID int IDENTITY(1,1) PRIMARY KEY,
	CountyName varchar(50)
)
GO
EXEC uspCreateCounty
GO
CREATE PROC uspCreateAddress
AS
CREATE TABLE Address(
	AddressID int IDENTITY(1,1) PRIMARY KEY,
	Street1 varchar(50) NOT NULL,
	Street2 varchar(50) NOT NULL,
	Town varchar(50) NOT NULL,
	CountyID int NOT NULL,
	AddressType varchar(50) NOT NULL,
	CustomerID int NOT NULL
	FOREIGN KEY (CountyID) REFERENCES County(CountyID),
	FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)
GO
EXEC uspCreateAddress
GO
CREATE PROC uspCreateUserCard
AS
CREATE TABLE UserCard(
	CardID int NOT NULL PRIMARY KEY,
	CardProvider varchar(50) NOT NULL,
	ExpiryDate date NOT NULL
)
GO
EXEC uspCreateUserCard
GO
CREATE PROC uspCreateUserAccount
AS
CREATE TABLE UserAccount(
	AccountID int IDENTITY(1,1) PRIMARY KEY,
	CardID int NOT NULL,
	CustomerID int NOT NULL,
	FOREIGN KEY (CardID) REFERENCES UserCard(CardID),
	FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)
GO
EXEC uspCreateUserAccount
GO
CREATE PROC uspCreateOrderStatus
AS
CREATE TABLE OrderStatus(
	OrderStatusID int IDENTITY(1,1) PRIMARY KEY,
	OrderType varchar(50) NOT NULL 
)
GO
EXEC uspCreateOrderStatus
GO
CREATE PROC uspCreateDeliveryStatus
AS
CREATE TABLE DeliveryStatus(
	DeliveryStatusID int IDENTITY(1,1) PRIMARY KEY,
	IsDelivered bit NOT NULL DEFAULT 0 
)
GO
EXEC uspCreateDeliveryStatus
GO
CREATE PROC uspCreateShippingMethod
AS
CREATE TABLE ShippingMethod(
	ShippingID int IDENTITY(1,1) PRIMARY KEY,
	ShipProvider varchar(50) NOT NULL,
	ShipCharges decimal (5,2)
)
GO
EXEC uspCreateShippingMethod
GO
CREATE PROC uspCreateCart
AS
CREATE TABLE Cart(
	CartID int IDENTITY(1,1) PRIMARY KEY,
	CustomerID int NOT NULL,
	TotalCost decimal (5,2) NOT NULL,
	OrderStatusID int NOT NULL,
	OrderDate date NOT NULL,
	DeliveryDate date NOT NULL,
	DeliveryStatusID int NOT NULL,
	ShippingID int NOT NULL,
	Note varchar(100)
	FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
	FOREIGN KEY (OrderStatusID) REFERENCES OrderStatus(OrderStatusID),
	FOREIGN KEY (DeliveryStatusID) REFERENCES DeliveryStatus(DeliveryStatusID),
	FOREIGN KEY (ShippingID) REFERENCES ShippingMethod(ShippingID) 
)
GO
EXEC uspCreateCart
GO
CREATE PROC uspCreateUserTransaction
AS
CREATE TABLE UserTransaction(
	TransactionID varchar(150) NOT NULL PRIMARY KEY,
	CartID int NOT NULL,
	TranCost decimal NOT NULL,
	TranStatus bit NOT NULL DEFAULT 0,
	TranDate date NOT NULL,
	AccountID int NOT NULL,
	FOREIGN KEY (CartID) REFERENCES Cart(CartID),
	FOREIGN KEY (AccountID) REFERENCES UserAccount(AccountID)
)
GO
EXEC uspCreateUserTransaction
GO
CREATE PROC uspCreateCategory
AS
CREATE TABLE Category(
	CategoryID int IDENTITY(1,1) PRIMARY KEY,
	CatName varchar(50) UNIQUE NOT NULL,
	CatDescription varchar(100),
	ImageUrl varchar(50) NOT NULL,
)
GO
EXEC uspCreateCategory
GO
CREATE PROC uspCreateSupplier
AS
CREATE TABLE Supplier(
	SupplierID int IDENTITY(1,1) PRIMARY KEY,
	SupName varchar(50) NOT NULL,
	SupAddress varchar(50) NOT NULL,
	SupPhone varchar(50) NOT NULL, 
	SupEmail varchar(50) NOT NULL 
)
GO
EXEC uspCreateSupplier
GO
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
CREATE PROC uspCreateProductImage
AS
CREATE TABLE ProductImage(
	ImageID int IDENTITY(1,1) PRIMARY KEY,
	ImageUrl varchar(50) NOt NULL,
	ProductID int NOT NULL,
	FOREIGN KEY (ProductID) REFERENCES Product(ProductID))
GO
EXEC uspCreateProductImage
GO
CREATE PROC uspCreateItems
AS
CREATE TABLE Items(
	CartID int NOT NULL,
	ProductID int NOT NULL,
	Quantity int,	
	FOREIGN KEY (CartID) REFERENCES Cart(CartID),
	FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
	PRIMARY KEY (CartID, ProductID)
)
GO
EXEC uspCreateItems
GO
CREATE PROC uspCreateError
AS
CREATE TABLE Error(
	ErrorID int IDENTITY(1,1) PRIMARY KEY,
	ErrorDate date NOT NULL,
	ErrorMessageType varchar(50),
	ErrorMessage varchar(300)
)
GO
EXEC uspCreateError
GO

CREATE PROC uspInsertCounty
	@countyname varchar(50)
AS
INSERT INTO County VALUES (@countyname)
GO
CREATE PROC uspInsertAddress
	@street1 varchar(50),
	@street2 varchar(50),
	@town varchar(50),
	@countyid int,
	@addresstype varchar(50),
	@customerID int
AS
INSERT INTO Address VALUES (@street1, @street2, @town, @countyID, @addresstype, @customerID)
GO
CREATE PROC uspInsertUserRole
	@rolename varchar(50)
AS
INSERT INTO UserRole VALUES (@rolename)
GO
CREATE PROC uspInsertCustomer
	@firstname varchar(50),
	@lastname varchar(50),
	@email varchar(50),
	@phone varchar(50),
	@dateregistered date,
	@isactivated bit,
	@lastlogin date,
	@userpass varchar(50),
	@islogged bit,
	@roleid int
AS
INSERT INTO Customer VALUES (@firstname, @lastname, @email, @phone, @dateregistered, @isactivated,@lastlogin, @userpass, @islogged, @roleid)
GO
CREATE PROC uspInsertUserCard
	@cardid int,
	@cardprovider varchar(50),
	@expirydate date
AS
INSERT INTO UserCard VALUES (@cardid, @cardprovider, @expirydate)
GO
CREATE PROC uspInsertUserAccount
	@cardid int,
	@customerid int
AS
INSERT INTO UserAccount VALUES (@cardid, @customerid)
GO
CREATE PROC uspInsertOrderStatus

	@ordertype varchar(50) 
AS
INSERT INTO OrderStatus VALUES (@ordertype)
GO
CREATE PROC uspInsertDeliveryStatus

	@isdelivered bit

AS
INSERT INTO DeliveryStatus VALUES (@isdelivered)
GO
CREATE PROC uspInsertShippingMethod

	@shipprovider varchar(50),
	@shipcharges decimal (5,2)
AS
INSERT INTO ShippingMethod VALUES (@shipprovider, @shipcharges)
GO
CREATE PROC uspInsertCart

	@customerid int,
	@totalcost decimal (5,2),
	@orderstatusid int,
	@orderdate date,
	@deliverydate date,
	@deliverystatusid int,
	@shippingid int,
	@note varchar(100)	
AS
INSERT INTO Cart VALUES (@customerid, @totalcost, @orderstatusid, @orderdate, @deliverydate, @deliverystatusid, @shippingid, @note)
GO
CREATE PROC uspInsertUserTransaction
	@transactionid varchar(150),
	@cartid int,
	@trancost decimal,
	@transtatus bit,
	@trandate date,
	@accountid int
AS
INSERT INTO UserTransaction VALUES (@transactionid, @cartid, @trancost, @transtatus, @trandate, @accountid)
GO
CREATE PROC uspInsertProductImage
	@imageurl varchar(50),
	@productID int
AS
INSERT INTO ProductImage VALUES (@imageurl, @productID)
GO

CREATE PROC uspInsertSupplier
	@supname varchar(50),
	@supaddress varchar(50),
	@supphone varchar(50),
	@supemail varchar(50)
AS
INSERT INTO Supplier VALUES (@supname, @supaddress, @supphone, @supemail)
GO

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

CREATE PROC uspInsertItems

	@cartid int,
	@productID int,
	@quantity int
AS
INSERT INTO Items VALUES (@cartid, @productID, @quantity)
GO

CREATE PROC uspAllProducts
AS
SELECT * FROM Product
GO


CREATE PROC uspInsertProductImage
	@imageurl varchar(50),
	@productID int
AS
INSERT INTO ProductImage VALUES (@imageurl, @productID)
GO

CREATE PROC uspInsertSupplier
	@supname varchar(50),
	@supaddress varchar(50),
	@supphone varchar(50)
AS
INSERT INTO Supplier VALUES (@supname, @supaddress, @supphone)
GO

CREATE PROC uspInsertCategory
	@catname varchar(50),
	@catdescription varchar(100),
	@imageURL varchar(50)
AS
INSERT INTO Category VALUES (@catname, @catdescription, @imageURL)
GO

CREATE PROC uspInsertError
	@errdate date,
	@errtype varchar(50),
	@errmsg varchar(300),
AS
INSERT INTO Error VALUES (@errdate, @errtype, @errmsg)
GO

CREATE PROCEDURE uspSelectSingleCategory 
@id int
AS
SELECT        CategoryID,CatName,CatDescription,ImageUrl
FROM          Category
WHERE		  CategoryID = @id
GO

CREATE PROCEDURE uspSelectSingleSupplier 
@id int
AS
SELECT        SupplierID,SupName,SupAddress,SupPhone,SupEmail
FROM          Supplier
WHERE		  SupplierID = @id
GO



CREATE PROC uspCreateStaff
AS
CREATE TABLE [dbo].[Staff] (
    [StaffID]   INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (50)  NULL,
    [LastName]  VARCHAR (50)  NOT NULL,
    [Role]      VARCHAR (50)  NOT NULL,
    [IsActive]  BIT           DEFAULT ((0)) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Pass]      VARCHAR (MAX) NOT NULL,
    UNIQUE NONCLUSTERED ([Email] ASC)
);

GO
EXEC uspCreateStaff
GO


CREATE PROC uspInsertStaff
	@fname varchar(50),
	@sname varchar(50),
	@role varchar(20),
	@active int,
	@email varchar(100),
	@pass varchar(MAX)
AS
INSERT INTO Staff VALUES (@fname, @sname, @role,@active,@email,@pass)
GO


CREATE PROCEDURE uspSelectSingleStaff 
@id int
AS
SELECT        *
FROM          Staff
WHERE		  StaffID = @id
GO


CREATE PROCEDURE uspUpdateSingleStaff
       @staffId INT,
	   @staffFirstName varchar(50),
	   @staffLastName varchar(50),
	   @staffRole varchar(50),
	   @staffActive int,
	   @staffPass varchar(50),
	   @staffEmail varchar(50)
  AS
    BEGIN
     UPDATE Staff 
     SET FirstName = @staffFirstName, LastName = @staffLastName, Role = @staffRole, Email = @staffEmail, @staffActive, Pass = @staffPass
     WHERE StaffID = @staffId
    END