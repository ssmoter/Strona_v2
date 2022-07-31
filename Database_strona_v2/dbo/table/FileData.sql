CREATE TABLE [dbo].[FileModelC]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(100) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [UserId] INT NULL, 
    [Created] DATETIMEOFFSET NULL, 
    [Main] BIT NULL DEFAULT 'False', 
    [StoredFileName] NVARCHAR(MAX) NULL, 
    [Type] NVARCHAR(MAX) NULL, 
    [NSFW] BIT NULL DEFAULT 'False', 
    [Ban] BIT NULL DEFAULT 'False', 
    [NoLike] INT NULL, 
    [UnLike] INT NULL
        
)
