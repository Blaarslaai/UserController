USE [db_UserController]
GO

IF OBJECT_ID('users', 'U') IS NOT NULL 
	DROP TABLE [users];

GO

CREATE TABLE [users]
(
	[id] INT IDENTITY(1,1) PRIMARY KEY,
	[UserId] VARCHAR(50),
	[UserName] VARCHAR(50),
	[UserToken] VARCHAR(MAX),
	[Epoc] VARCHAR(50),
	[DateTime] DATETIME DEFAULT GETDATE()
)
