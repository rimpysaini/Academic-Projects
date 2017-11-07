CREATE PROC uspCheckLoginAdmin
@email varchar(50),
@pass varchar(MAX)
AS
SELECT Staff.Role From Staff WHERE Email=@email AND Pass=@pass AND IsActive=2