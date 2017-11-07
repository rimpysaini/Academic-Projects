CREATE PROCEDURE uspSelectSingleStaff 
@id int
AS
SELECT        *
FROM          Staff
WHERE		  StaffID = @id
GO