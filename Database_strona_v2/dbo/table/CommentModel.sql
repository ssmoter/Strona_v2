CREATE TABLE [dbo].[CommentModel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileId] INT NULL, 
    [UserId] INT NULL, 
    [Comment] NVARCHAR(MAX) NULL, 
    [Created] DATETIMEOFFSET NULL, 
    [NoLike] INT NULL DEFAULT 0, 
    [UnLike] INT NULL DEFAULT 0
)
