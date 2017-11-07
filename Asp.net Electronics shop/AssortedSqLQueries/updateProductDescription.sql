CREATE PROCEDURE uspUpdateSingleProductDescription
       @id INT,
	   @description XML
  AS
    BEGIN
     UPDATE Product 
     SET ProductDescription = @description
     WHERE ProductID = @id
    END