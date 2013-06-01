IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitIntegers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitIntegers];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitIntegers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION dbo.wm_SplitIntegers
(
	@List NVARCHAR(MAX)
)
RETURNS @ParsedList TABLE (	Number INT )
AS
BEGIN

	DECLARE @Number NVARCHAR(32)
	        , @Pos INT;

	SET @List = LTRIM(RTRIM(@List))+ '','';
	SET @Pos = CHARINDEX('','', @List, 1);

	IF (REPLACE(@List, '','', '''') <> '''')
	BEGIN
		WHILE (@Pos > 0)
		BEGIN
		
			SET @Number = LTRIM(RTRIM(LEFT(@List, @Pos - 1)))
			IF (@Number <> '''')
			BEGIN
				INSERT INTO @ParsedList (Number) 
				VALUES (CAST(@Number AS int)) --Use Appropriate conversion
			END
			SET @List = RIGHT(@List, LEN(@List) - @Pos)
			SET @Pos = CHARINDEX('','', @List, 1)

		END
	END	
	RETURN
END
' 
END

GO