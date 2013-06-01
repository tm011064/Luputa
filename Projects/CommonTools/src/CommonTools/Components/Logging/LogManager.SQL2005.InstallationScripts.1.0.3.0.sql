/******************************************************************************************

	CommonTools.Components.Logging.LogManager SQL installation scripts

 This script will create all tables and stored procedures used for the LogManager. Table- and stored procedure names must have 
 a custom prefix. To define the prefixes, find and replace the following strings at this document:

  { DatabaseOwner } - replace with your database owner ( e.g.: [dbo] )
  { TablePrefix } - replace with your tableprefix ( e.g.: el_ ), can be left blank
  { StoredProcedurePrefix } - replace with your stored procedure prefix ( e.g.: proc_el_ ), can be left blank
 
 *******************************************************************************************/
 

-- ******************************************************************************************
-- Create TABLES
--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }Exceptions]') AND type in (N'U'))
BEGIN
	CREATE TABLE { DatabaseOwner }.[{ TablePrefix }Exceptions](
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
		[MachineName] [nvarchar](256) NULL,
		[AuthenticatedUserId] [nvarchar](256) NULL,
	 CONSTRAINT [PK_{ TablePrefix }Exceptions] PRIMARY KEY CLUSTERED 
	(
		[ExceptionID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
ELSE
BEGIN
	IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns c WHERE	table_name = N'{ TablePrefix }Exceptions' AND LOWER(c.COLUMN_NAME) = 'machinename')
		ALTER TABLE { DatabaseOwner }.[{ TablePrefix }Exceptions]	ADD [MachineName] [nvarchar](256) NULL;
	IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns c WHERE	table_name = N'{ TablePrefix }Exceptions' AND LOWER(c.COLUMN_NAME) = 'authenticateduserId')
		ALTER TABLE { DatabaseOwner }.[{ TablePrefix }Exceptions]	ADD [AuthenticatedUserId] [nvarchar](256) NULL;
END

GO 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }EventLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE { DatabaseOwner }.[{ TablePrefix }EventLog](
		[EventID] [bigint] IDENTITY(1,1) NOT NULL,
		[AppLocation] [int] NOT NULL,
		[EventDate] [datetime] NOT NULL,
		[EventType] [int] NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[MachineName] [nvarchar](256) NOT NULL,
		[AuthenticatedUserId] [nvarchar](256) NULL,
	 CONSTRAINT [PK_{ TablePrefix }EventLog] PRIMARY KEY CLUSTERED 
	(
		[EventID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
ELSE
BEGIN
	IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns c WHERE	table_name = N'{ TablePrefix }EventLog' AND LOWER(c.COLUMN_NAME) = 'authenticateduserId')
		ALTER TABLE { DatabaseOwner }.[{ TablePrefix }EventLog]	ADD [MachineName] [nvarchar](256) NULL;
END
GO

-- ******************************************************************************************
-- Create STORED PROCEDURES
--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]
AS
BEGIN
	SET NOCOUNT ON;
SELECT     { TablePrefix }EventLog.*
FROM         { TablePrefix }EventLog
END'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Get]
AS
BEGIN
	SET NOCOUNT ON;
SELECT     { TablePrefix }Exceptions.*
FROM         { TablePrefix }Exceptions
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('   PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]
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
	@HandlingStatus tinyint,
	@MachineName nvarchar(256),
	@AuthenticatedUserId nvarchar(256)
)
AS
BEGIN

	SET NOCOUNT OFF;

	--		[{ TablePrefix }Exceptions_Insert].HandlingStatus values:
	--
	--	0 : Expected 		-> this exception might occure but is expected and handled
	--	1 : HandledInCode   -> this exception should not be thrown but is handled in code
	--	2 : Unhandled       -> this was an unhandled exception, further investigation is necessary
	--	3 : Resolved		-> the error that caused this execption was resolved and should not occure any more
	IF EXISTS (	SELECT	ExceptionID
				FROM	[{ TablePrefix }Exceptions] 
				WHERE	HashCode = @HashCode )
	BEGIN
		UPDATE
			[{ TablePrefix }Exceptions]
		SET
			DateLastOccurred = GetDate(),
			TotalOccurrences = TotalOccurrences + 1
		WHERE
			HashCode = @HashCode
	END
	ELSE
	BEGIN
		INSERT INTO [{ TablePrefix }Exceptions] (
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
			, [DateLastOccurred]
			, [MachineName]
			, [AuthenticatedUserId]) 
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
			, GETDATE()
			, @MachineName
			, @AuthenticatedUserId);
	END
END



' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('   PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]
(
	@ExceptionID BIGINT,	-- set this to -1 if you want to delete records based on date
	@OlderThanInDays INT	-- set this to -1 if you want to delete a single event
)
AS
BEGIN

	SET NOCOUNT OFF;

	IF @ExceptionID >= 0
	BEGIN
		DELETE FROM [{ TablePrefix }Exceptions] 
		WHERE [ExceptionID] = @ExceptionID
	END

	IF @OlderThanInDays >= 0
	BEGIN
		DELETE FROM [{ TablePrefix }Exceptions] 
		WHERE [DateCreated] < DATEADD(d, @OlderThanInDays * -1, GETDATE());
	END

END
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('    PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]
(
	@AppLocation int,
	@EventType int,
	@Message nvarchar(MAX),
	@MachineName nvarchar(256),
	@AuthenticatedUserId nvarchar(256)
)
AS
	SET NOCOUNT OFF;

INSERT INTO [{ TablePrefix }EventLog] ([AppLocation], [EventDate], [EventType], [Message], [MachineName], [AuthenticatedUserId]) 
VALUES (@AppLocation, GETDATE(), @EventType, @Message, @MachineName, @AuthenticatedUserId);
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('    PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]
(
	@EventID bigint,
	@AppLocation int,
	@EventDate datetime,
	@EventType int,
	@Message nvarchar(MAX),
	@MachineName nvarchar(256),
	@AuthenticatedUserId nvarchar(256)
)
AS
	SET NOCOUNT OFF;

UPDATE [{ TablePrefix }EventLog] 
SET [AppLocation] = @AppLocation, [EventDate] = @EventDate, [EventType] = @EventType, [Message] = @Message, [MachineName] = @MachineName, [AuthenticatedUserId] = @AuthenticatedUserId
WHERE [EventID] = @EventID
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Delete]
(
	@EventID BIGINT,			-- set this to -1 if you want to delete records based on date
	@OlderThanInDays INT	-- set this to -1 if you want to delete a single event
)
AS
BEGIN
	
	SET NOCOUNT OFF;

	IF @EventID >= 0
	BEGIN
		DELETE FROM [{ TablePrefix }EventLog] 
		WHERE [EventID] = @EventID
	END

	IF @OlderThanInDays >= 0
	BEGIN
		DELETE FROM [{ TablePrefix }EventLog] 
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


DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('    PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]
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
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		{ TablePrefix }EventLog
					WHERE		{ TablePrefix }EventLog.EventType = @EventTypeFilter
							AND { TablePrefix }EventLog.AppLocation = @AppLocationFilter
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter
					AND { TablePrefix }EventLog.AppLocation = @AppLocationFilter;	

		END
		ELSE IF @AppLocationFilter >= 0
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		{ TablePrefix }EventLog
					WHERE	{ TablePrefix }EventLog.AppLocation = @AppLocationFilter
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.AppLocation = @AppLocationFilter;

		END
		ELSE IF @EventTypeFilter >= 0
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		{ TablePrefix }EventLog
					WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter;

		END
		ELSE
		BEGIN

			SELECT  el.[EventID]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventID] DESC) AS [ROW_NUMBER]
							, [EventID] AS EventID
					FROM		{ TablePrefix }EventLog
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventID] = el.[EventID])
			ORDER BY elo.[ROW_NUMBER]


				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog;

		END

END
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('   PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]
(
	@ExceptionID bigint,	
	@HandlingStatus tinyint
)
AS
	SET NOCOUNT OFF;

UPDATE	[{ TablePrefix }Exceptions] 
SET		[HandlingStatus] = @HandlingStatus
WHERE	[ExceptionID] = @ExceptionID

' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('     PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]
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
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }Exceptions.[DateLastOccurred] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		{ TablePrefix }Exceptions
					WHERE	{ TablePrefix }Exceptions.AppLocation = @AppLocationFilter
						AND	{ TablePrefix }Exceptions.HandlingStatus = @HandlingStatus
				 ) AS elo, { TablePrefix }Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions
			WHERE	{ TablePrefix }Exceptions.AppLocation = @AppLocationFilter
				AND	{ TablePrefix }Exceptions.HandlingStatus = @HandlingStatus

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
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }Exceptions.[DateLastOccurred] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		{ TablePrefix }Exceptions
					WHERE	{ TablePrefix }Exceptions.AppLocation = @AppLocationFilter
				 ) AS elo, { TablePrefix }Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions
			WHERE	{ TablePrefix }Exceptions.AppLocation = @AppLocationFilter

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
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }Exceptions.[DateLastOccurred] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		{ TablePrefix }Exceptions
					WHERE	{ TablePrefix }Exceptions.HandlingStatus = @HandlingStatus
				 ) AS elo, { TablePrefix }Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions
			WHERE	{ TablePrefix }Exceptions.HandlingStatus = @HandlingStatus

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
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }Exceptions.[DateLastOccurred] DESC) AS [ROW_NUMBER]
							, [ExceptionID] AS ExceptionID
					FROM		{ TablePrefix }Exceptions
				 ) AS elo, { TablePrefix }Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[ExceptionID] = el.[ExceptionID])
			ORDER BY elo.[ROW_NUMBER]

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions;
	END

END
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_InsertBatch]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('    PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_InsertBatch]
(
	@EventXml	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @EventDocument INT;
	EXEC sp_xml_preparedocument @EventDocument OUTPUT, @EventXml
	
	INSERT INTO [{ TablePrefix }EventLog] ([AppLocation], [EventDate], [EventType], [Message], [MachineName], [AuthenticatedUserId]) 
	SELECT	x.AppLocation
			, x.EventDate
			, x.EventType
			, x.Message
			, x.MachineName
			, x.AuthenticatedUserId
	FROM	OPENXML (@EventDocument, ''insert/e'')
	WITH	(	AppLocation int ''@a''
				, EventDate datetime ''@d''
				, EventType int ''@e''
				, Message nvarchar(MAX) ''@m''
				, MachineName nvarchar(256) ''@n''
				, AuthenticatedUserId nvarchar(256) ''@u'') x;
				
	EXEC sp_xml_removedocument @EventDocument;
	
END
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO