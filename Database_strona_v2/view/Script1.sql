USE Database_strona_v2
--CREATE TABLE [dbo].[test]
--(
--	[test] INT NOT NULL PRIMARY KEY,    
--)

--DECLARE @HashThis NVARCHAR(32);  
--SET @HashThis = CONVERT(NVARCHAR(32),N'1');  

--DROP TABLE dbo.HashThis


--SELECT Id,NoLike,Created FROM dbo.FileData

--SELECT * FROM dbo.CommentModelServer_4002

--delete from dbo.FileData where Id = 5003

UPDATE dbo.UserData SET EmailConfirmed=0 WHERE Id=N'4'