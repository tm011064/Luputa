IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog_LogLastError]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_SqlErrorLog_LogLastError];
END
GO


CREATE PROCEDURE [dbo].[wm_SqlErrorLog_LogLastError]
(
  @ReturnCode INT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;
  
  IF ( ERROR_NUMBER() IS NULL )
    RETURN;
      
  BEGIN TRY  
  
    INSERT INTO [wm_SqlErrorLog] ([ErrorNumber]
                                 ,[ErrorSeverity]
                                 ,[ErrorState]
                                 ,[ErrorProcedure]
                                 ,[ErrorLine]
                                 ,[ErrorMessage]
                                 ,[SystemUser]
                                 ,[DateCreatedUtc]
                                 ,[ReturnCode])
    VALUES(ERROR_NUMBER()
          ,ERROR_SEVERITY()
          ,ERROR_STATE()
          ,ERROR_PROCEDURE()
          ,ERROR_LINE()
          ,ERROR_MESSAGE()
          ,SYSTEM_USER
          ,GETUTCDATE()
          ,@ReturnCode)
          
  END TRY
  BEGIN CATCH
  
  END CATCH
  
END

GO