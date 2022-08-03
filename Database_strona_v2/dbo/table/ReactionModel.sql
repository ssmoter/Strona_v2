CREATE TABLE [dbo].[ReactionModel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ObjectId] INT NULL, 
    [UserId] INT NULL, 
    [ReactionLevel] INT NULL, 
    [ReactionType] INT NULL
)
