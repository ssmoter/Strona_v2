CREATE TABLE [dbo].[TagModel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileId] INT NULL, 
    [UserId] INT NULL, 
    [Value] NVARCHAR(50) NULL
)
