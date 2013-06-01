/******************************************************************************************

	CommonTools.Components.Caching.ClusteredCacheManager SQL installation scripts

 This script will create all tables and stored procedures used for the ClusteredCacheManager. Table- and stored procedure names can have 
 a custom prefix. To define the prefixes, find and replace the following strings at this document:

  { DatabaseOwner }			- replace with your database owner ( e.g.: [dbo] )
  { TablePrefix }			- replace with your tableprefix ( e.g.: cc_ ), can be left blank
  { StoredProcedurePrefix } - replace with your stored procedure prefix ( e.g.: proc_cc_ ), can be left blank
 
 *******************************************************************************************/

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
	[CacheKey] [nvarchar](256) NOT NULL,
	[LastUpdate] [bigint] NOT NULL,
 CONSTRAINT [PK_{ TablePrefix }CacheKeys] PRIMARY KEY CLUSTERED 
(
	[CacheKey] ASC
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
AS
BEGIN
	SELECT  [CacheKey], [LastUpdate]
	FROM    { DatabaseOwner }.[{ TablePrefix }CacheKeys]
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
	@CacheKey NVARCHAR(256) 
)
AS
BEGIN
	SELECT  [LastUpdate]
	FROM    { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE   CacheKey = @CacheKey;
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
	@CacheKey NVARCHAR(256) 
)
AS
BEGIN
	DELETE FROM { DatabaseOwner }.[{ TablePrefix }CacheKeys]
	WHERE	CacheKey = @CacheKey;
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
AS
BEGIN
	DELETE FROM { DatabaseOwner }.[{ TablePrefix }CacheKeys]
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
)
AS
BEGIN
	IF ((	SELECT CacheKey 
			FROM { DatabaseOwner }.[{ TablePrefix }CacheKeys]
			WHERE CacheKey = @CacheKey	) IS NULL)
	BEGIN
		INSERT INTO	{ DatabaseOwner }.[{ TablePrefix }CacheKeys] (CacheKey, LastUpdate)
		VALUES  (@CacheKey, @LastUpdate);
	END
	ELSE
	BEGIN
		UPDATE	{ DatabaseOwner }.[{ TablePrefix }CacheKeys]
		SET		LastUpdate = @LastUpdate
		WHERE	CacheKey = @CacheKey;
	END
END
' 
END
