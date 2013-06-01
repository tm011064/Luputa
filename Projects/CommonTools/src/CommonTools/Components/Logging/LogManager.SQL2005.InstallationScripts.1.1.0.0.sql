﻿/******************************************************************************************

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
		[ApplicationId] [int] NOT NULL,
		[ExceptionId] [bigint] IDENTITY(1,1) NOT NULL,
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
		[ExceptionId] ASC
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
		[ApplicationId] [int] NOT NULL,
		[EventId] [bigint] IDENTITY(1,1) NOT NULL,
		[AppLocation] [int] NOT NULL,
		[EventDate] [datetime] NOT NULL,
		[EventType] [int] NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[MachineName] [nvarchar](256) NOT NULL,
		[AuthenticatedUserId] [nvarchar](256) NULL,
	 CONSTRAINT [PK_{ TablePrefix }EventLog] PRIMARY KEY CLUSTERED 
	(
		[EventId] ASC
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
(
	@ApplicationId int
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT     { TablePrefix }EventLog.*
	FROM       { TablePrefix }EventLog
	WHERE	   { TablePrefix }EventLog.ApplicationId = @ApplicationId;
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
(
	@ApplicationId int
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT     { TablePrefix }Exceptions.*
	FROM       { TablePrefix }Exceptions
	WHERE	   { TablePrefix }Exceptions.ApplicationId = @ApplicationId;
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
	@ApplicationId int,
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
	IF EXISTS (	SELECT	ExceptionId
				FROM	[{ TablePrefix }Exceptions] 
				WHERE	HashCode = @HashCode )
	BEGIN
		UPDATE
			[{ TablePrefix }Exceptions]
		SET
			DateLastOccurred = GETUTCDATE()
			TotalOccurrences = TotalOccurrences + 1
		WHERE
			HashCode = @HashCode	AND
			ApplicationId = @ApplicationId
			
	END
	ELSE
	BEGIN
		INSERT INTO [{ TablePrefix }Exceptions] (
			[ApplicationId]
			, [AppLocation]
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
			@ApplicationId
			, @AppLocation
			, @Exception
			, @ExceptionMessage
			, GETUTCDATE()
			, @Method
			, @IPAddress
			, @UserAgent
			, @HttpReferrer
			, @HttpVerb
			, @Url
			, @HashCode
			, @HandlingStatus
			, 1
			, GETUTCDATE()
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
	@ApplicationId int = null,
	@ExceptionId bigint = -1,	-- set this to -1 if you want to delete records based on date
	@OlderThanInDays int = -1	-- set this to -1 if you want to delete a single event
)
AS
BEGIN

	SET NOCOUNT OFF;

	IF ((@ExceptionId IS NOT NULL) AND (@ExceptionId >= 0))
	BEGIN
		DELETE FROM [{ TablePrefix }Exceptions] 
		WHERE [ExceptionId] = @ExceptionId
	END

	IF ((@ApplicationId IS NOT NULL) AND (@OlderThanInDays >= 0) AND (@OlderThanInDays >= 0))
	BEGIN
		DELETE FROM [{ TablePrefix }Exceptions] 
		WHERE [DateCreated] < DATEADD(d, @OlderThanInDays * -1, GETUTCDATE())
			AND ApplicationId = @ApplicationId;
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
	@ApplicationId int,
	@AppLocation int,
	@EventType int,
	@Message nvarchar(MAX),
	@MachineName nvarchar(256),
	@AuthenticatedUserId nvarchar(256)
)
AS
	SET NOCOUNT OFF;

INSERT INTO [{ TablePrefix }EventLog] ([ApplicationId], [AppLocation], [EventDate], [EventType], [Message], [MachineName], [AuthenticatedUserId]) 
VALUES (@ApplicationId, @AppLocation, GETUTCDATE(), @EventType, @Message, @MachineName, @AuthenticatedUserId);
' AS NVARCHAR(MAX));
EXEC dbo.sp_executesql @statement = @AlterCreate; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



DECLARE @AlterCreate NVARCHAR(MAX);
SET @AlterCreate = 'ALTER ';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]') AND type in (N'P', N'PC'))
	SET @AlterCreate = 'CREATE ';
SET @AlterCreate =  @AlterCreate + CAST('    PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]
(
	@EventId bigint,
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
WHERE [EventId] = @EventId
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
	@ApplicationId int = null,
	@EventId bigint = -1,			-- set this to -1 if you want to delete records based on date
	@OlderThanInDays int = -1,		-- set this to -1 if you want to delete a single event
	@EventType int = null
)
AS
BEGIN
	
	SET NOCOUNT OFF;

	IF ((@EventId IS NOT NULL) AND (@EventId >= 0))
	BEGIN
		DELETE FROM [{ TablePrefix }EventLog] 
		WHERE [EventId] = @EventId
	END

	IF ((@ApplicationId IS NOT NULL) AND (@OlderThanInDays >= 0) AND (@OlderThanInDays >= 0))	
	BEGIN
	
		IF (@EventType IS NOT NULL)
		BEGIN
			DELETE FROM [{ TablePrefix }EventLog] 
			WHERE [EventDate] < DATEADD(d, @OlderThanInDays * -1, GETUTCDATE())
				AND ApplicationId = @ApplicationId
				AND EventType = @EventType;
		END
		ELSE
		BEGIN
			DELETE FROM [{ TablePrefix }EventLog] 
			WHERE [EventDate] < DATEADD(d, @OlderThanInDays * -1, GETUTCDATE())
				AND ApplicationId = @ApplicationId;
		END
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
	@ApplicationId INT,
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
				
			SELECT  el.[EventId]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventId] DESC) AS [ROW_NUMBER]
							, [EventId] AS EventId
					FROM		{ TablePrefix }EventLog
					WHERE		{ TablePrefix }EventLog.EventType = @EventTypeFilter
							AND { TablePrefix }EventLog.AppLocation = @AppLocationFilter
							AND { TablePrefix }EventLog.ApplicationId = @ApplicationId
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventId] = el.[EventId])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter
					AND { TablePrefix }EventLog.AppLocation = @AppLocationFilter;	

		END
		ELSE IF @AppLocationFilter >= 0
		BEGIN

			SELECT  el.[EventId]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventId] DESC) AS [ROW_NUMBER]
							, [EventId] AS EventId
					FROM		{ TablePrefix }EventLog
					WHERE	{ TablePrefix }EventLog.AppLocation = @AppLocationFilter
						AND { TablePrefix }EventLog.ApplicationId = @ApplicationId
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventId] = el.[EventId])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.AppLocation = @AppLocationFilter;

		END
		ELSE IF @EventTypeFilter >= 0
		BEGIN

			SELECT  el.[EventId]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventId] DESC) AS [ROW_NUMBER]
							, [EventId] AS EventId
					FROM		{ TablePrefix }EventLog
					WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter
						AND { TablePrefix }EventLog.ApplicationId = @ApplicationId
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventId] = el.[EventId])
			ORDER BY elo.[ROW_NUMBER]

				SELECT	@RowCount = COUNT(*) 
				FROM	{ TablePrefix }EventLog
				WHERE	{ TablePrefix }EventLog.EventType = @EventTypeFilter;

		END
		ELSE
		BEGIN

			SELECT  el.[EventId]
				  , el.[AppLocation]
				  , el.[EventDate]
				  , el.[EventType]
				  , el.[Message]
				  , el.[MachineName]
				  , el.[AuthenticatedUserId]
			FROM (  SELECT	  TOP ((@PageIndex + 1) * @PageSize) ROW_NUMBER() OVER (ORDER BY { TablePrefix }EventLog.[EventId] DESC) AS [ROW_NUMBER]
							, [EventId] AS EventId
					FROM		{ TablePrefix }EventLog
					WHERE	{ TablePrefix }EventLog.ApplicationId = @ApplicationId
				 ) AS elo, { TablePrefix }EventLog AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	(elo.[EventId] = el.[EventId])
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
	@ExceptionId bigint,	
	@HandlingStatus tinyint
)
AS
	SET NOCOUNT OFF;

UPDATE	[{ TablePrefix }Exceptions] 
SET		[HandlingStatus] = @HandlingStatus
WHERE	[ExceptionId] = @ExceptionId

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
SET @AlterCreate =  @AlterCreate + CAST(' PROCEDURE [dbo].[{ StoredProcedurePrefix }Exceptions_GetPage]
(
	@ApplicationId		INT,
	@PageIndex			INT, 
    @PageSize			INT,
	@RowCount			INT OUTPUT,
	@AppLocationFilter	INT,	-- set this to -1 if you want to return all values
	@HandlingStatus		INT,	-- set this to -1 if you want to return all values
	@OrderBy			INT = 0 	/*
										0	-> DateLastOccurred DESC 		
										1	-> DateCreated DESC
										2	-> TotalOccurrences DESC
										10	-> DateLastOccurred ASC 		
										11	-> DateCreated ASC
										12	-> TotalOccurrences ASC
									*/
)
AS
BEGIN

	DECLARE @WhereClause NVARCHAR(MAX);
	DECLARE @Command NVARCHAR(MAX);
	DECLARE @OrderByCommand NVARCHAR(32);

	SET @OrderByCommand =	CASE @OrderBy
								WHEN 0 THEN ''[DateLastOccurred] DESC ''
								WHEN 1 THEN ''[DateCreated] DESC ''
								WHEN 2 THEN ''[TotalOccurrences] DESC ''
								WHEN 10 THEN ''[DateLastOccurred] ASC ''
								WHEN 11 THEN ''[DateCreated] ASC ''
								WHEN 12 THEN ''[TotalOccurrences] ASC ''
								ELSE ''[DateLastOccurred] DESC ''
							END;

	IF @AppLocationFilter >= 0 AND @HandlingStatus >= 0
	BEGIN
		SET @WhereClause = '' WHERE	{ TablePrefix }Exceptions.AppLocation = '' + CAST(@AppLocationFilter AS NVARCHAR(32)) + '' AND	{ TablePrefix }Exceptions.HandlingStatus = '' + CAST(@HandlingStatus AS NVARCHAR(32)) + '' AND { TablePrefix }Exceptions.ApplicationId = '' + CAST(@ApplicationId AS NVARCHAR(32));
	END
	ELSE IF @AppLocationFilter >= 0
	BEGIN
		SET @WhereClause = '' WHERE	{ TablePrefix }Exceptions.AppLocation = '' + CAST(@AppLocationFilter AS NVARCHAR(32)) + '' AND { TablePrefix }Exceptions.ApplicationId = '' + CAST(@ApplicationId AS NVARCHAR(32));	
	END
	ELSE IF @HandlingStatus >= 0
	BEGIN
		SET @WhereClause = '' WHERE	{ TablePrefix }Exceptions.HandlingStatus = '' + CAST(@HandlingStatus AS NVARCHAR(32)) + '' AND { TablePrefix }Exceptions.ApplicationId = '' + CAST(@ApplicationId AS NVARCHAR(32));
	END
	ELSE
	BEGIN
		SET @WhereClause =  '' WHERE { TablePrefix }Exceptions.ApplicationId = '' + CAST(@ApplicationId AS NVARCHAR(32));
	END

	SET @Command = ''SELECT  el.[ExceptionId]
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
			FROM (  SELECT	  TOP (('' + CAST(@PageIndex AS NVARCHAR(32)) + '' + 1) * '' + CAST(@PageSize AS NVARCHAR(32)) + '') ROW_NUMBER() OVER (ORDER BY { TablePrefix }Exceptions.''+@OrderByCommand+'') AS [ROW_NUMBER]
							, [ExceptionId] AS ExceptionId
					FROM		{ TablePrefix }Exceptions '' + @WhereClause + ''
				 ) AS elo, { TablePrefix }Exceptions AS el 
			WHERE 
					(elo.[ROW_NUMBER] BETWEEN ('' + CAST(@PageIndex AS NVARCHAR(32)) + '') * '' + CAST(@PageSize AS NVARCHAR(32)) + '' + 1 AND ('' + CAST(@PageIndex AS NVARCHAR(32)) + '' + 1) * '' + CAST(@PageSize AS NVARCHAR(32)) + '')
				AND	(elo.[ExceptionId] = el.[ExceptionId])
			ORDER BY elo.[ROW_NUMBER]

			INSERT INTO #tmpRowCount (TotalRows)
			SELECT	COUNT(*) 
			FROM	{ TablePrefix }Exceptions  '' + @WhereClause + '';'';

	CREATE TABLE #tmpRowCount (TotalRows INT);
	EXECUTE (@Command);
	SET @RowCount = (	SELECT	TOP(1) TotalRows
						FROM	#tmpRowCount	);
	DROP TABLE #tmpRowCount;

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
	
	INSERT INTO [{ TablePrefix }EventLog] ([ApplicationId], [AppLocation], [EventDate], [EventType], [Message], [MachineName], [AuthenticatedUserId]) 
	SELECT	x.ApplicationId
			, x.AppLocation
			, x.EventDate
			, x.EventType
			, x.Message
			, x.MachineName
			, x.AuthenticatedUserId
	FROM	OPENXML (@EventDocument, ''insert/e'')
	WITH	(	ApplicationId int ''@i''
				, AppLocation int ''@a''
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