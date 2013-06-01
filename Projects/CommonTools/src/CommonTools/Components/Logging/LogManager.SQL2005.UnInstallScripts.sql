/******************************************************************************************

	CommonTools.Components.Logging.LogManager SQL uninstall scripts

 This script will create delete all tables and stored procedures used for the LogManager. Table- and stored procedure names must have 
 a custom prefix. To define the prefixes, find and replace the following strings at this document:

  { DatabaseOwner } - replace with your database owner ( e.g.: [dbo] )
  { TablePrefix } - replace with your tableprefix ( e.g.: el_ ), can be left blank
  { StoredProcedurePrefix } - replace with your stored procedure prefix ( e.g.: proc_el_ ), can be left blank
 
 *******************************************************************************************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }Exceptions]') AND type in (N'U'))
DROP TABLE { DatabaseOwner }.[{ TablePrefix }Exceptions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ TablePrefix }EventLog]') AND type in (N'U'))
DROP TABLE { DatabaseOwner }.[{ TablePrefix }EventLog]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Delete]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Get]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_GetPage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Insert]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }EventLog_Update]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Delete]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Get]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_GetPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_GetPage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Insert]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }ExceptionLog_Update]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Delete]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_GetPage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Insert]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{ DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE { DatabaseOwner }.[{ StoredProcedurePrefix }Exceptions_Update]
GO