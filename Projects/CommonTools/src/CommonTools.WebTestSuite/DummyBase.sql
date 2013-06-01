SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_BulkInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UniqueUsers_BulkInsert]
(
	@Filename	NVARCHAR(256)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(512);
	SET	@SQL = ''BULK INSERT UniqueUsers FROM '''''' + @Filename + '''''' WITH (FIELDTERMINATOR = '''','''')'';
	EXEC (@SQL)
--	BULK	INSERT	UniqueUsers 
--			FROM	''" +@Filename+ "'' 
--			WITH	(FIELDTERMINATOR = '','');

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncrementingUsers_BulkInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[IncrementingUsers_BulkInsert]
(
	@Filename	NVARCHAR(256)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(512);
	SET	@SQL = ''BULK INSERT IncrementingUsers FROM '''''' + @Filename + '''''' WITH (FIELDTERMINATOR = '''','''')'';
	EXEC (@SQL)

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ct_Exceptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ct_Exceptions](
	[ExceptionID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppLocation] [int] NOT NULL,
	[Exception] [nvarchar](256) NOT NULL,
	[ExceptionMessage] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Method] [nvarchar](128) NOT NULL,
	[IPAddress] [varchar](15) NULL,
	[UserAgent] [nvarchar](64) NULL,
	[HttpReferrer] [nvarchar](256) NULL,
	[HttpVerb] [nvarchar](24) NULL,
	[Url] [nvarchar](1024) NULL,
	[HashCode] [int] NOT NULL,
	[HandlingStatus] [tinyint] NOT NULL,
	[TotalOccurrences] [int] NOT NULL,
	[DateLastOccurred] [datetime] NOT NULL,
 CONSTRAINT [PK_ct_Exceptions] PRIMARY KEY CLUSTERED 
(
	[ExceptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ct_EventLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ct_EventLog](
	[EventID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppLocation] [int] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[EventType] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[MachineName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_ct_EventLog] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UniqueUsers](
	[UserID] [uniqueidentifier] NOT NULL,
	[AccountStatus] [tinyint] NOT NULL,
	[Timezone] [float] NOT NULL,
	[Firstname] [nvarchar](256) NULL,
	[Lastname] [nvarchar](256) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[City] [nvarchar](256) NULL,
	[IsNewletterSubscriber] [bit] NOT NULL,
 CONSTRAINT [PK_UniqueUsers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncrementingUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[IncrementingUsers](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountStatus] [tinyint] NOT NULL,
	[Timezone] [float] NOT NULL,
	[Firstname] [nvarchar](256) NULL,
	[Lastname] [nvarchar](256) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[City] [nvarchar](256) NULL,
	[IsNewletterSubscriber] [bit] NOT NULL,
 CONSTRAINT [PK_IncrementingUsers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_Exceptions_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[proc_ct_Exceptions_Update]
(
	@ExceptionID bigint,	
	@HandlingStatus tinyint
)
AS
	SET NOCOUNT OFF;

UPDATE	[ct_Exceptions] 
SET		[HandlingStatus] = @HandlingStatus
WHERE	[ExceptionID] = @ExceptionID

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_Exceptions_GetPage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[proc_ct_Exceptions_GetPage]
(
	@PageIndex			INT, 
    @PageSize			INT,
	@RowCount			INT OUTPUT,
	@AppLocationFilter	INT,	-- set this to -1 if you want to return all values
	@HandlingStatus		INT		-- set this to -1 if you want to return all values
)
AS
BEGIN

	IF @AppLocationFilter >= 0 AND @HandlingStatus >= 0
	BEGIN
			SELECT  el.[ExceptionID]
				  , el.[AppLocation]
				  , el.[Exception]
				  , el.[ExceptionMessage]
				  , el.[DateCreated]
				  , el.[Method]
				  , el.[IPAddress]
				  , el.[UserAgent]
				  , el.[HttpReferrer]
				  , el.[HttpVerb]
				  , el.[Url]
				  , el.[HashCode]
				  , el.[HandlingStatus]
				  , el.[TotalOccurrences]
				  , el.[DateLastOccurred]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_Exceptions.[ExceptionID] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		ct_Exceptions
					WHERE	ct_Exceptions.AppLocation = @AppLocationFilter
						AND	ct_Exceptions.HandlingStatus = @HandlingStatus
				 ) AS elo, ct_Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	ct_Exceptions
			WHERE	ct_Exceptions.AppLocation = @AppLocationFilter
				AND	ct_Exceptions.HandlingStatus = @HandlingStatus

	END
	ELSE IF @AppLocationFilter >= 0
	BEGIN

			SELECT  el.[ExceptionID]
				  , el.[AppLocation]
				  , el.[Exception]
				  , el.[ExceptionMessage]
				  , el.[DateCreated]
				  , el.[Method]
				  , el.[IPAddress]
				  , el.[UserAgent]
				  , el.[HttpReferrer]
				  , el.[HttpVerb]
				  , el.[Url]
				  , el.[HashCode]
				  , el.[HandlingStatus]
				  , el.[TotalOccurrences]
				  , el.[DateLastOccurred]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_Exceptions.[ExceptionID] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		ct_Exceptions
					WHERE	ct_Exceptions.AppLocation = @AppLocationFilter
				 ) AS elo, ct_Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	ct_Exceptions
			WHERE	ct_Exceptions.AppLocation = @AppLocationFilter

	END
	ELSE IF @HandlingStatus >= 0
	BEGIN

SELECT  el.[ExceptionID]
				  , el.[AppLocation]
				  , el.[Exception]
				  , el.[ExceptionMessage]
				  , el.[DateCreated]
				  , el.[Method]
				  , el.[IPAddress]
				  , el.[UserAgent]
				  , el.[HttpReferrer]
				  , el.[HttpVerb]
				  , el.[Url]
				  , el.[HashCode]
				  , el.[HandlingStatus]
				  , el.[TotalOccurrences]
				  , el.[DateLastOccurred]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_Exceptions.[ExceptionID] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		ct_Exceptions
					WHERE	ct_Exceptions.HandlingStatus = @HandlingStatus
				 ) AS elo, ct_Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	ct_Exceptions
			WHERE	ct_Exceptions.HandlingStatus = @HandlingStatus

	END
	ELSE
	BEGIN

			SELECT  el.[ExceptionID]
				  , el.[AppLocation]
				  , el.[Exception]
				  , el.[ExceptionMessage]
				  , el.[DateCreated]
				  , el.[Method]
				  , el.[IPAddress]
				  , el.[UserAgent]
				  , el.[HttpReferrer]
				  , el.[HttpVerb]
				  , el.[Url]
				  , el.[HashCode]
				  , el.[HandlingStatus]
				  , el.[TotalOccurrences]
				  , el.[DateLastOccurred]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_Exceptions.[ExceptionID] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		ct_Exceptions
				 ) AS elo, ct_Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	ct_Exceptions;
	END

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_Exceptions_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[proc_ct_Exceptions_Get]
AS
	SET NOCOUNT ON;
SELECT     ct_Exceptions.*
FROM         ct_Exceptions' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_Exceptions_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[proc_ct_Exceptions_Insert]
(
	@AppLocation int,
	@Exception nvarchar(256),
	@ExceptionMessage nvarchar(MAX),
	@Method nvarchar(128),
	@IPAddress varchar(15),
	@UserAgent nvarchar(64),
	@HttpReferrer nvarchar(256),
	@HttpVerb nvarchar(24),
	@Url nvarchar(1024),
	@HashCode int,
	@HandlingStatus tinyint
)
AS
BEGIN

	SET NOCOUNT OFF;

	--		[ct_Exceptions_Insert].HandlingStatus values:
	--
	--	0 : Expected 		-> this exception might occure but is expected and handled
	--	1 : HandledInCode   -> this exception should not be thrown but is handled in code
	--	2 : Unhandled       -> this was an unhandled exception, further investigation is necessary
	--	3 : Resolved		-> the error that caused this execption was resolved and should not occure any more
	IF EXISTS (	SELECT	ExceptionID
				FROM	[ct_Exceptions] 
				WHERE	HashCode = @HashCode )
	BEGIN
		UPDATE
			[ct_Exceptions]
		SET
			DateLastOccurred = GetDate(),
			TotalOccurrences = TotalOccurrences + 1
		WHERE
			HashCode = @HashCode
	END
	ELSE
	BEGIN
		INSERT INTO [ct_Exceptions] (
			[AppLocation]
			, [Exception]
			, [ExceptionMessage]
			, [DateCreated]
			, [Method]
			, [IPAddress]
			, [UserAgent]
			, [HttpReferrer]
			, [HttpVerb]
			, [Url]
			, [HashCode]
			, [HandlingStatus]
			, [TotalOccurrences]
			, [DateLastOccurred]) 
		VALUES (
			@AppLocation
			, @Exception
			, @ExceptionMessage
			, GETDATE()
			, @Method
			, @IPAddress
			, @UserAgent
			, @HttpReferrer
			, @HttpVerb
			, @Url
			, @HashCode
			, @HandlingStatus
			, 1
			, GETDATE());
	END
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_Exceptions_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[proc_ct_Exceptions_Delete]
(
	@ExceptionID BIGINT,	-- set this to -1 if you want to delete records based on date
	@OlderThanInDays INT	-- set this to -1 if you want to delete a single event
)
AS
BEGIN

	SET NOCOUNT OFF;

	IF @ExceptionID >= 0
	BEGIN
		DELETE FROM [ct_Exceptions] 
		WHERE [ExceptionID] = @ExceptionID
	END

	IF @OlderThanInDays >= 0
	BEGIN
		DELETE FROM [ct_Exceptions] 
		WHERE [DateCreated] < DATEADD(d, @OlderThanInDays * -1, GETDATE());
	END

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_EventLog_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[proc_ct_EventLog_Insert]
(
	@AppLocation int,
	@EventType int,
	@Message nvarchar(MAX),
	@MachineName nvarchar(256)
)
AS
	SET NOCOUNT OFF;

INSERT INTO [ct_EventLog] ([AppLocation], [EventDate], [EventType], [Message], [MachineName]) 
VALUES (@AppLocation, GETDATE(), @EventType, @Message, @MachineName);
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_EventLog_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[proc_ct_EventLog_Update]
(
	@EventID bigint,
	@AppLocation int,
	@EventDate datetime,
	@EventType int,
	@Message nvarchar(MAX),
	@MachineName nvarchar(256)
)
AS
	SET NOCOUNT OFF;

UPDATE [ct_EventLog] 
SET [AppLocation] = @AppLocation, [EventDate] = @EventDate, [EventType] = @EventType, [Message] = @Message, [MachineName] = @MachineName
WHERE [EventID] = @EventID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_EventLog_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[proc_ct_EventLog_Delete]
(
	@EventID BIGINT,			-- set this to -1 if you want to delete records based on date
	@OlderThanInDays INT	-- set this to -1 if you want to delete a single event
)
AS
BEGIN
	
	SET NOCOUNT OFF;

	IF @EventID >= 0
	BEGIN
		DELETE FROM [ct_EventLog] 
		WHERE [EventID] = @EventID
	END

	IF @OlderThanInDays >= 0
	BEGIN
		DELETE FROM [ct_EventLog] 
		WHERE [EventDate] < DATEADD(d, @OlderThanInDays * -1, GETDATE());
	END

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_EventLog_GetPage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[proc_ct_EventLog_GetPage]
(
	@PageIndex INT, 
    @PageSize INT, 
	@RowCount INT OUTPUT,
	@AppLocationFilter INT,	-- set this to -1 if you want to return all values
	@EventTypeFilter INT	-- set this to -1 if you want to return all values
)
AS
BEGIN

		IF @AppLocationFilter >= 0 AND @EventTypeFilter >= 0
		BEGIN
				
			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		ct_EventLog
					WHERE		ct_EventLog.EventType = @EventTypeFilter
							AND ct_EventLog.AppLocation = @AppLocationFilter
				 ) AS elo, ct_EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	ct_EventLog
				WHERE	ct_EventLog.EventType = @EventTypeFilter
					AND ct_EventLog.AppLocation = @AppLocationFilter;	

		END
		ELSE IF @AppLocationFilter >= 0
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		ct_EventLog
					WHERE	ct_EventLog.AppLocation = @AppLocationFilter
				 ) AS elo, ct_EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	ct_EventLog
				WHERE	ct_EventLog.AppLocation = @AppLocationFilter;

		END
		ELSE IF @EventTypeFilter >= 0
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		ct_EventLog
					WHERE	ct_EventLog.EventType = @EventTypeFilter
				 ) AS elo, ct_EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	ct_EventLog
				WHERE	ct_EventLog.EventType = @EventTypeFilter;

		END
		ELSE
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY ct_EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		ct_EventLog
				 ) AS elo, ct_EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]


				SELECT	@RowCount = COUNT(*) 
				FROM	ct_EventLog;

		END

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_ct_EventLog_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[proc_ct_EventLog_Get]
AS
	SET NOCOUNT ON;
SELECT     ct_EventLog.*
FROM         ct_EventLog' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users_GetExecutionStats]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[Users_GetExecutionStats]
AS
BEGIN

DECLARE @ExecutionStart DATETIME;

-- **********************************************************
-- //////// USER COUNT //////////////////////////////////////
-- **********************************************************

	DECLARE	@IUCount INT;
	DECLARE	@UUCount INT;
	DECLARE	@IUPKCount INT;
	DECLARE	@UUPKCount INT;
	DECLARE	@IUAccountCount INT;
	DECLARE	@UUAccountCount INT;

	SET @ExecutionStart = GETDATE();
	SET @IUCount =	(	SELECT	 COUNT(*)
						FROM	IncrementingUsers AS iu)
	PRINT ''Execution time on IncrementingUsers count: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

	SET @ExecutionStart = GETDATE();
	SET @UUCount =	(	SELECT	 COUNT(*)
						FROM	UniqueUsers AS uu)
	PRINT ''Execution time on UniqueUsers count: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';


	SET @ExecutionStart = GETDATE();
	SET @UUAccountCount =	(	SELECT	 COUNT(*)
								FROM	UniqueUsers 
								WHERE	AccountStatus = 1)
	PRINT ''Execution time on UniqueUsers count (non indexed constraint): '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

	SET @ExecutionStart = GETDATE();
	SET @IUAccountCount =	(	SELECT	 COUNT(*)
								FROM	IncrementingUsers 
								WHERE	AccountStatus = 1)
	PRINT ''Execution time on IncrementingUsers count (indexed constraint): '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

	SET @ExecutionStart = GETDATE();
	SET @IUPKCount =	(	SELECT	 COUNT(UserID)
							FROM	IncrementingUsers AS iu)
	PRINT ''Execution time on PK IncrementingUsers count: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

	SET @ExecutionStart = GETDATE();
	SET @UUPKCount =	(	SELECT	 COUNT(UserID)
							FROM	UniqueUsers AS uu)
	PRINT ''Execution time on PK UniqueUsers count: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

-- **********************************************************
-- //////// USER SELECTION //////////////////////////////////
-- **********************************************************

	DECLARE	@IUSelectID INT;
	DECLARE	@UUSelectID UNIQUEIDENTIFIER;

	SET @ExecutionStart = GETDATE();
	SET @UUSelectID = (	SELECT	UserID
						FROM	UniqueUsers
						WHERE	UserID = ''19C69577-68A8-45FC-B581-000022E0ED3C'')
	PRINT ''Execution time on UniqueUsers select: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

	SET @ExecutionStart = GETDATE();
	SET @IUSelectID = (	SELECT	UserID
						FROM	IncrementingUsers
						WHERE	UserID = 1433299 )
	PRINT ''Execution time on IncrementingUsers select: '' + CAST((DATEDIFF(ms, @ExecutionStart, GETDATE())) AS NVARCHAR(32)) + '' ms'';

END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users_GetCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Users_GetCount]
AS
BEGIN

	DECLARE @IUCount INT;
	DECLARE @UUCount INT;

	SELECT	 @IUCount = COUNT(iu.UserID)
	FROM	IncrementingUsers AS iu

	SELECT	 @UUCount =  COUNT(uu.UserID)
	FROM	UniqueUsers AS uu

	SELECT	@IUCount AS IncrementingUserCount
			,@UUCount AS UniqueUserCount

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_GetByUserID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[UniqueUsers_GetByUserID]
(
	@UserID UNIQUEIDENTIFIER
)
AS
BEGIN

	SELECT	*
	FROM	UniqueUsers
	WHERE	UserID = @UserID

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_GetUserPageOrderedByDateOfBirth]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[UniqueUsers_GetUserPageOrderedByDateOfBirth]
(
	@PageIndex	INT,
	@PageSize	INT
)
AS
BEGIN

	SELECT  uu.[UserID]
          , uu.[AccountStatus]
          , uu.[Timezone]
          , uu.[Firstname]
          , uu.[Lastname]
          , uu.[DateOfBirth]
          , uu.[City]
          , uu.[IsNewletterSubscriber]
    FROM (  SELECT	  ROW_NUMBER() OVER (ORDER BY [DateOfBirth] DESC) AS [ROW_NUMBER]
					, [UserID] AS UserID
            FROM	[UniqueUsers]
		 ) AS uuo, UniqueUsers AS uu 
    WHERE 
			(uuo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
		AND	(uuo.UserID = uu.UserID)
    ORDER BY uuo.[ROW_NUMBER]

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[UniqueUsers_Insert]
(
	@UserID uniqueidentifier
   ,@AccountStatus tinyint
   ,@Timezone float
   ,@Firstname nvarchar(256)
   ,@Lastname nvarchar(256)
   ,@DateOfBirth datetime
   ,@City nvarchar(256)
   ,@IsNewletterSubscriber bit
)
AS
BEGIN

	INSERT INTO [UniqueUsers]
           ([UserID]
           ,[AccountStatus]
           ,[Timezone]
           ,[Firstname]
           ,[Lastname]
           ,[DateOfBirth]
           ,[City]
           ,[IsNewletterSubscriber])
     VALUES
           (@UserID
		   ,@AccountStatus
		   ,@Timezone
		   ,@Firstname
		   ,@Lastname
		   ,@DateOfBirth 
		   ,@City
		   ,@IsNewletterSubscriber);
	
	RETURN @@ROWCOUNT;

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[UniqueUsers_Update]
(
	@UserID uniqueidentifier
   ,@AccountStatus tinyint
   ,@Timezone float
   ,@Firstname nvarchar(256)
   ,@Lastname nvarchar(256)
   ,@DateOfBirth datetime
   ,@City nvarchar(256)
   ,@IsNewletterSubscriber bit
)
AS
BEGIN

	UPDATE [UniqueUsers]
	SET		[AccountStatus] =@AccountStatus
           ,[Timezone] =@Timezone
           ,[Firstname]=@Firstname
           ,[Lastname]=@Lastname
           ,[DateOfBirth]=@DateOfBirth
           ,[City]=@City
           ,[IsNewletterSubscriber]=@IsNewletterSubscriber
    WHERE  [UserID] = @UserID;

	RETURN @@ROWCOUNT;

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniqueUsers_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UniqueUsers_Delete]
(
	@UserID uniqueidentifier
)
AS
BEGIN

	DELETE FROM	UniqueUsers
	WHERE	UserID = @UserID; 

	RETURN @@ROWCOUNT;

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncrementingUsers_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[IncrementingUsers_Insert]
(
    @AccountStatus tinyint
   ,@Timezone float
   ,@Firstname nvarchar(256)
   ,@Lastname nvarchar(256)
   ,@DateOfBirth datetime
   ,@City nvarchar(256)
   ,@IsNewletterSubscriber bit
)
AS
BEGIN

	INSERT INTO IncrementingUsers
           ([AccountStatus]
           ,[Timezone]
           ,[Firstname]
           ,[Lastname]
           ,[DateOfBirth]
           ,[City]
           ,[IsNewletterSubscriber])
     VALUES
           (@AccountStatus
		   ,@Timezone
		   ,@Firstname
		   ,@Lastname
		   ,@DateOfBirth 
		   ,@City
		   ,@IsNewletterSubscriber);

	SELECT	*
	FROM	IncrementingUsers
	WHERE	UserID = SCOPE_IDENTITY(); 

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncrementingUsers_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[IncrementingUsers_Update]
(
	@UserID int
   ,@AccountStatus tinyint
   ,@Timezone float
   ,@Firstname nvarchar(256)
   ,@Lastname nvarchar(256)
   ,@DateOfBirth datetime
   ,@City nvarchar(256)
   ,@IsNewletterSubscriber bit
)
AS
BEGIN

	UPDATE IncrementingUsers
	SET		[AccountStatus] =@AccountStatus
           ,[Timezone] =@Timezone
           ,[Firstname]=@Firstname
           ,[Lastname]=@Lastname
           ,[DateOfBirth]=@DateOfBirth
           ,[City]=@City
           ,[IsNewletterSubscriber]=@IsNewletterSubscriber
    WHERE  [UserID] = @UserID;

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncrementingUsers_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[IncrementingUsers_Delete]
(
	@UserID int
)
AS
BEGIN

	DELETE FROM	IncrementingUsers
	WHERE	UserID = @UserID; 

END
' 
END
