IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitStrings];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION dbo.wm_SplitStrings
(
	@List NVARCHAR(MAX)
)
RETURNS @ParsedList TABLE (	Value NVARCHAR(MAX) )
AS
BEGIN

  IF (@List IS NULL)
    RETURN;

	DECLARE @String NVARCHAR(MAX)
	        , @Pos INT;

	SET @List = '''''','' + LTRIM(RTRIM(@List)) + '','''''';	
	SET @Pos = CHARINDEX('''''','''''', @List, 1);

	IF (REPLACE(@List, '''''','''''', '''') <> '''')
	BEGIN
		WHILE (@Pos > 0)
		BEGIN
		
			SET @String = LTRIM(RTRIM(LEFT(@List, @Pos - 1)))
			IF (@String <> '''')
			BEGIN
				INSERT INTO @ParsedList (Value) 
				VALUES (CAST(@String AS NVARCHAR(MAX))) --Use Appropriate conversion
			END
			SET @List = RIGHT(@List, LEN(@List) - @Pos - 2)
			SET @Pos = CHARINDEX('''''','''''', @List, 1)

		END
	END	
	RETURN
END
' 
END

GO