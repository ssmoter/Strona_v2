CREATE TABLE [dbo].[UserData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Email] NVARCHAR(50) NULL, 
    [Password] NVARCHAR(50) NULL, 
    [Token] NVARCHAR(MAX) NULL, 
    [Name] NVARCHAR(50) NULL, 
    [ExpiryDate] DATETIMEOFFSET NULL, 
    [LastOnline] DATETIMEOFFSET NULL, 
    [EmailConfirmed] BIT NULL DEFAULT 'False', 
    [Role] NVARCHAR(50) NULL DEFAULT 'User', 
    [DataCreat] DATETIMEOFFSET NULL, 
    [AvatarNameString] NVARCHAR(MAX) NULL, 
    [Ban] BIT NULL DEFAULT 'False', 
    [NoLike] INT NULL, 
    [UnLike] INT NULL, 
)
