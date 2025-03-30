-- Insert a new user
CREATE PROCEDURE [dbo].[InsertUser]
	@UserId VARCHAR(50),
	@UserName VARCHAR(50),
	@UserToken VARCHAR(MAX),
	@Epoc VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[users] (UserId, UserName, UserToken, Epoc, DateTime)
	VALUES (@UserId, @UserName, @UserToken, @Epoc, GETDATE());
END
GO

-- Select all users
CREATE PROCEDURE [dbo].[GetAllUsers]
	@Offset INT,
	@FetchRows INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[users]
	ORDER BY id ASC
	OFFSET @Offset ROWS
	FETCH NEXT @FetchRows ROWS ONLY;
END
GO

-- Select a single user by username
CREATE PROCEDURE [dbo].[GetUserByUserName]
	@UserName VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[users] WHERE UserName = @UserName;
END
GO

-- Select a single user by userid
CREATE PROCEDURE [dbo].[GetUserByUserId]
	@UserId VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[users] WHERE UserId = @UserId;
END
GO

-- Update a user based on username
CREATE PROCEDURE UpdateUserByUserId
	@UserId VARCHAR(50),
	@UserName VARCHAR(50),
	@UserToken VARCHAR(MAX),
	@Epoc VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[users]
	SET UserName = @UserName, UserToken = @UserToken, Epoc = @Epoc, DateTime = GETDATE()
	WHERE UserId = @UserId;
END
GO

-- Delete all users
CREATE PROCEDURE DeleteAllUsers
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[users];
END
GO

-- Delete a single user based on username
CREATE PROCEDURE [dbo].[DeleteUserByUsername]
	@UserId VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[users] WHERE UserId = @UserId;
END
GO
