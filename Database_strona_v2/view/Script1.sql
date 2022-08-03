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

--UPDATE dbo.UserData SET EmailConfirmed=0 WHERE Id=N'4'



--SELECT Id FROM dbo.FileModelC WHERE Created = CAST( AS DATETIMEOFFSET)

--SELECT * FROM dbo.FileModelC
--WHERE Created  =  CONVERT(datetimeoffset(7),'01.08.2022 18:42:55.9658721 +02:00') 
--WHERE Created  =  CONVERT(datetimeoffset(7),'2022-08-01 18:42:55.9658721 +02:00') 

UPDATE dbo.FileModelC set UnLike += -1 WHERE Id=5007
--INSERT INTO dbo.ReactionModel (ObjectId,UserId,ReactionLevel,ReactionType) VAlUES (5007,4062,-1,1)
SELECT NoLike, UnLike FROM dbo.FileModelC WHERE Id=5007
