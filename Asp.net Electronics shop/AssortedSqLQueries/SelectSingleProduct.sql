CREATE PROCEDURE uspSelectSingleProduct
       @Id INT
AS
SELECT * FROM Product
WHERE ProductID = @Id
GO