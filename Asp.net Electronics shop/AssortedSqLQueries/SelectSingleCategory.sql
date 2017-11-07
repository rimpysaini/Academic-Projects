CREATE PROCEDURE uspSelectSigleCategory 
@id int
AS
SELECT        CategoryID,CatName,CatDescription,ImageUrl
FROM          Category
WHERE		  CategoryID = @id
GO