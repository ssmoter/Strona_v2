CREATE TABLE [dbo].[FileData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(100) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Tag] NVARCHAR(MAX) NULL, 
    [UserId] INT NULL, 
    [NoLike] INT NULL, 
    [Spam] INT NULL, 
    [Created] DATETIMEOFFSET NULL, 
    [Main] BIT NULL DEFAULT 'False', 
    [ListUserIdLike] TEXT NULL, 
    [ListUserIdSpam] TEXT NULL,
    [StoredFileName] NVARCHAR(MAX) NULL, 
    [Type] NVARCHAR(MAX) NULL, 
    [NSFW] BIT NULL DEFAULT 'False', 
    [Ban] BIT NULL DEFAULT 'False'
        
)
