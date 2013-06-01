/******************************************************************************************

	CommonTools.Components.Caching.ClusteredCacheManager SQL installation scripts

 This script will create all tables and stored procedures used for the ClusteredCacheManager. Table- and stored procedure names can have 
 a custom prefix. To define the prefixes, find and replace the following strings at this document:

  { Database }				- The name of the database. This is used to call the enable the service broker (only works
							  on SQL Server 2005 Enterprise edition). If you don't want to enable service broker, outcomment
							  the 'Enable service broker' section of this script.
  { DatabaseOwner }			- replace with your database owner ( e.g.: [dbo] )
  { TablePrefix }			- replace with your tableprefix ( e.g.: cc_ ), can be left blank
  { StoredProcedurePrefix } - replace with your stored procedure prefix ( e.g.: proc_cc_ ), can be left blank
 
 *******************************************************************************************/
 
-- ******************************************************************************************
-- Enable service broker
--
ALTER DATABASE { Database } SET ENABLE_BROKER
GO

-- ******************************************************************************************
-- Create TABLES
--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }CacheKeys]') AND type in (N'U'))
BEGIN
CREATE TABLE { DatabaseOwner }.[{ TablePrefix }CacheKeys](
	[ApplicationId] [int] NOT NULL,
	[CacheKey] [nvarchar](256) NOT NULL,
	[LastUpdate] [bigint] NOT NULL,
 CONSTRAINT [PK_{ TablePrefix }CacheKeys] PRIMARY KEY CLUSTERED 
(
	[CacheKey] ASC, ApplicationId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- ******************************************************************************************
-- Create STORED PROCEDURES
--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Get]
(
	@ApplicationId INT
)
AS
BEGIN
	SELECT  [CacheKey], [LastUpdate]
	FROM    { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE	[ApplicationId] = @ApplicationId
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_GetLastUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_GetLastUpdate]
(
	@CacheKey NVARCHAR(256),
	@ApplicationId INT
)
AS
BEGIN
	SELECT  [LastUpdate]
	FROM    { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE   [CacheKey] = @CacheKey
		AND [ApplicationId] = @ApplicationId;
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Delete]
(
	@CacheKey NVARCHAR(256),
	@ApplicationId INT
)
AS
BEGIN
	DELETE FROM { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE	[CacheKey] = @CacheKey
		AND [ApplicationId] = @ApplicationId;
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_DeleteAll]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_DeleteAll]
(
	@ApplicationId INT
)
AS
BEGIN
	DELETE FROM { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE		[ApplicationId] = @ApplicationId
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_InsertOrUpdate]
(
	@CacheKey NVARCHAR(256)
	, @LastUpdate BIGINT
	, @ApplicationId INT
)
AS
BEGIN
	IF ( NOT EXISTS (	SELECT	[CacheKey]
						FROM	{ DatabaseOwner }.[{ TablePrefix }CacheKeys]
						WHERE	CacheKey = @CacheKey	
							AND [ApplicationId] = @ApplicationId) )
	BEGIN
		INSERT INTO	{ DatabaseOwner }.[{ TablePrefix }CacheKeys] (ApplicationId, CacheKey, LastUpdate)
		VALUES  (@ApplicationId, @CacheKey, @LastUpdate);
	END
	ELSE
	BEGIN
		UPDATE	{ DatabaseOwner }.[{ TablePrefix }CacheKeys] WITH(ROWLOCK)
		SET		LastUpdate = @LastUpdate
		WHERE	CacheKey = @CacheKey
			AND [ApplicationId] = @ApplicationId;
	END
END
' 
END
