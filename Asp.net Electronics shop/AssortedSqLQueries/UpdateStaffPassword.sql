CREATE PROCEDURE uspUpdateStaffPassword
       @id INT,
	   @pass varchar(MAX)
  AS
    BEGIN
     UPDATE Staff 
     SET Pass = @pass
     WHERE StaffId = @id
    END