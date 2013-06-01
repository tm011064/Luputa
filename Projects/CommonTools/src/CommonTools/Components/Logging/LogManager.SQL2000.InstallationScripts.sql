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
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }EventLog]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE { DatabaseOwner }.[{ TablePrefix }EventLog](
	[EventID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppLocation] [int] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[EventType] [int] NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[MachineName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }Exceptions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE { DatabaseOwner }.[{ TablePrefix }Exceptions](
	[ExceptionID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppLocation] [int] NOT NULL,
	[Exception] [nvarchar](256) NOT NULL,
	[ExceptionMessage] [nvarchar](4000) NOT NULL,
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
 CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED 
(
	[ExceptionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END

-- ******************************************************************************************
-- Create STORED PROCEDURES
--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]
AS
	SET NOCOUNT ON;
SELECT     { TablePrefix }EventLog.*
FROM       { TablePrefix }EventLog' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
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
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]
(
	@PageIndex INT, 
    @PageSize INT, 
	@RowCount INT OUTPUT,
	@AppLocationFilter INT,	-- set this to -1 if you want to return all values
	@EventTypeFilter INT	-- set this to -1 if you want to return all values
)
AS
BEGIN

		-- create temp table for paging
		CREATE TABLE #tEventRecords
		(
			RowNumber INT IDENTITY(1,1)
			, EventID BIGINT
		)

		IF @AppLocationFilter >= 0 AND @EventTypeFilter >= 0
		BEGIN

			INSERT	INTO	#tEventRecords	(EventID)
			SELECT			el.EventID
			FROM			{ TablePrefix }EventLog AS el
			WHERE			el.EventType = @EventTypeFilter
						AND el.AppLocation = @AppLocationFilter
			ORDER BY		el.[EventID] DESC
			
			-- now get results
			SELECT    el.*
			FROM	  { TablePrefix }EventLog AS el, #tEventRecords AS elTemp
			WHERE     (elTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (elTemp.EventID = el.EventID)

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }EventLog AS el
			WHERE	el.EventType = @EventTypeFilter
				AND el.AppLocation = @AppLocationFilter;

		END
		ELSE IF @AppLocationFilter >= 0
		BEGIN

			INSERT	INTO	#tEventRecords	(EventID)
			SELECT			el.EventID
			FROM			{ TablePrefix }EventLog AS el
			WHERE			el.AppLocation = @AppLocationFilter
			ORDER BY		el.[EventID] DESC
			
			-- now get results
			SELECT    el.*
			FROM	  { TablePrefix }EventLog AS el, #tEventRecords AS elTemp
			WHERE     (elTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (elTemp.EventID = el.EventID)

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }EventLog AS el
			WHERE	el.AppLocation = @AppLocationFilter;

		END
		ELSE IF @EventTypeFilter >= 0
		BEGIN

			INSERT	INTO	#tEventRecords	(EventID)
			SELECT			el.EventID
			FROM			{ TablePrefix }EventLog AS el
			WHERE			el.EventType = @EventTypeFilter
			ORDER BY		el.[EventID] DESC
			
			-- now get results
			SELECT    el.*
			FROM	  { TablePrefix }EventLog AS el, #tEventRecords AS elTemp
			WHERE     (elTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (elTemp.EventID = el.EventID)

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }EventLog AS el
			WHERE	el.EventType = @EventTypeFilter;

		END
		ELSE
		BEGIN

			INSERT	INTO	#tEventRecords	(EventID)
			SELECT			el.EventID
			FROM			{ TablePrefix }EventLog AS el
			ORDER BY		el.[EventID] DESC
			
			-- now get results
			SELECT    el.*
			FROM	  { TablePrefix }EventLog AS el, #tEventRecords AS elTemp
			WHERE     (elTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (elTemp.EventID = el.EventID)

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }EventLog;

		END

	-- finally drop table
	DROP TABLE #tEventRecords

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]
(
	@AppLocation int,
	@EventType int,
	@Message nvarchar(4000),
	@MachineName nvarchar(256)
)
AS
	SET NOCOUNT OFF;

INSERT INTO [{ TablePrefix }EventLog] ([AppLocation], [EventDate], [EventType], [Message], [MachineName]) 
VALUES (@AppLocation, GETDATE(), @EventType, @Message, @MachineName);
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]
(
	@EventID bigint,
	@AppLocation int,
	@EventDate datetime,
	@EventType int,
	@Message nvarchar(4000),
	@MachineName nvarchar(256)
)
AS
	SET NOCOUNT OFF;

UPDATE [{ TablePrefix }EventLog] 
SET [AppLocation] = @AppLocation, [EventDate] = @EventDate, [EventType] = @EventType, [Message] = @Message, [MachineName] = @MachineName
WHERE [EventID] = @EventID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]
(
	@ExceptionID bigint,	
	@HandlingStatus tinyint
)
AS
	SET NOCOUNT OFF;

UPDATE	[{ TablePrefix }Exceptions] 
SET		[HandlingStatus] = @HandlingStatus
WHERE	[ExceptionID] = @ExceptionID

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]
(
	@PageIndex			INT, 
    @PageSize			INT,
	@RowCount			INT OUTPUT,
	@AppLocationFilter	INT,	-- set this to -1 if you want to return all values
	@HandlingStatus		INT		-- set this to -1 if you want to return all values
)
AS
BEGIN

	-- create temp table for paging
	CREATE TABLE #tExceptionRecords
	(
		RowNumber INT IDENTITY(1,1)
		, ExceptionID BIGINT
	)

	IF @AppLocationFilter >= 0 AND @HandlingStatus >= 0
	BEGIN
			
			INSERT	INTO	#tExceptionRecords	(ExceptionID)
			SELECT			ex.ExceptionID
			FROM			{ TablePrefix }Exceptions AS ex
			WHERE			ex.HandlingStatus = @HandlingStatus
						AND ex.AppLocation = @AppLocationFilter
			ORDER BY		ex.[DateLastOccurred] DESC
			
			-- now get results
			SELECT    ex.*
			FROM	  { TablePrefix }Exceptions AS ex, #tExceptionRecords AS exTemp
			WHERE     (exTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (exTemp.[ExceptionID] = ex.[ExceptionID])

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions AS ex
			WHERE	ex.AppLocation = @AppLocationFilter
				AND	ex.HandlingStatus = @HandlingStatus;

	END
	ELSE IF @AppLocationFilter >= 0
	BEGIN

			INSERT	INTO	#tExceptionRecords	(ExceptionID)
			SELECT			ex.ExceptionID
			FROM			{ TablePrefix }Exceptions AS ex
			WHERE			ex.AppLocation = @AppLocationFilter
			ORDER BY		ex.[DateLastOccurred] DESC
			
			-- now get results
			SELECT    ex.*
			FROM	  { TablePrefix }Exceptions AS ex, #tExceptionRecords AS exTemp
			WHERE     (exTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (exTemp.[ExceptionID] = ex.[ExceptionID])

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions AS ex
			WHERE	ex.AppLocation = @AppLocationFilter;

	END
	ELSE IF @HandlingStatus >= 0
	BEGIN
			INSERT	INTO	#tExceptionRecords	(ExceptionID)
			SELECT			ex.ExceptionID
			FROM			{ TablePrefix }Exceptions AS ex
			WHERE			ex.HandlingStatus = @HandlingStatus
			ORDER BY		ex.ex.[DateLastOccurred] DESC
			
			-- now get results
			SELECT    ex.*
			FROM	  { TablePrefix }Exceptions AS ex, #tExceptionRecords AS exTemp
			WHERE     (exTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (exTemp.[ExceptionID] = ex.[ExceptionID])

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions AS ex
			WHERE	ex.HandlingStatus = @HandlingStatus;

	END
	ELSE
	BEGIN

			INSERT	INTO	#tExceptionRecords	(ExceptionID)
			SELECT			ex.ExceptionID
			FROM			{ TablePrefix }Exceptions AS ex
			ORDER BY		ex.ex.[DateLastOccurred] DESC
			
			-- now get results
			SELECT    ex.*
			FROM	  { TablePrefix }Exceptions AS ex, #tExceptionRecords AS exTemp
			WHERE     (exTemp.RowNumber BETWEEN (@PageIndex) * @PageSize + 1 AND (@PageIndex + 1) * @PageSize)
				AND	  (exTemp.[ExceptionID] = ex.[ExceptionID])

			SELECT	@RowCount = COUNT(*) 
			FROM	{ TablePrefix }Exceptions AS ex;
	END

	-- finally drop table
	DROP TABLE #tExceptionRecords

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]
(
	@AppLocation int,
	@Exception nvarchar(256),
	@ExceptionMessage nvarchar(4000),
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

	--		[{ StoredProcedurePrefix }Exceptions_Insert].HandlingStatus values:
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
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Get]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Get]
AS
	SET NOCOUNT ON;
SELECT     { TablePrefix }Exceptions.*
FROM         { TablePrefix }Exceptions' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJE{ StoredProcedurePrefix }ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]
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
' 
END
