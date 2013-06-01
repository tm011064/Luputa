/******************************************************************************************

	CommonTools.Components.Caching.ClusteredCacheManager SQL installation scripts

 This script will delete all tables and stored procedures used for the ClusteredCacheManager. Table- and stored procedure names can have 
 a custom prefix. To define the prefixes, find and replace the following strings at this document:

  { DatabaseOwner } - replace with your database owner ( e.g.: [dbo] )
  { TablePrefix } - replace with your tableprefix ( e.g.: cc_ ), can be left blank
  { StoredProcedurePrefix } - replace with your stored procedure prefix ( e.g.: proc_cc_ ), can be left blank
 
 *******************************************************************************************/
 

-- ******************************************************************************************
-- Drop Procedures
--
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }CacheKeys]') AND type in (N'U'))
DROP TABLE { DatabaseOwner }.[{ TablePrefix }CacheKeys]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Delete]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_GetLastUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_GetLastUpdate]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_InsertOrUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_InsertOrUpdate]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_DeleteAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_DeleteAll]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }CacheKeys_Get]
GO