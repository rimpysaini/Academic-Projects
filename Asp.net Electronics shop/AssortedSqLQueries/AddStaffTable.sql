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
