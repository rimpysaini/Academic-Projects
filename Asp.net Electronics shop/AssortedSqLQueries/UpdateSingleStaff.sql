CREATE PROCEDURE uspUpdateSingleStaff
       @id INT,
	   @fname varchar(50),
	   @sname varchar(50),
	   @email varchar(100),
	   @role INT,
	   @active INT
  AS
    BEGIN
     UPDATE Staff 
     SET FirstName = @fname, LastName = @sname, Role = @role, Email = @email,IsActive = @active
     WHERE StaffID = @id
    END