-- =============================================
-- Script Template - generate class code
-- =============================================

-- ***************************************************************
-- Declare variables
DECLARE @TableName					NVARCHAR(256)
		, @ClassName				NVARCHAR(256)
		, @DerivesFrom				NVARCHAR(256)
		, @MappedEntitiy			NVARCHAR(256)
		, @EntityObject				NVARCHAR(256)
		, @ColumnPrefixToReplace	NVARCHAR(256)
		, @ColumnPrefixReplaceWith	NVARCHAR(256)
		, @ColumnPrefixReplaceEntityProperties BIT;

-- ***************************************************************
-- Set variables

SET @TableName		= 'cla_TextMessages';
SET @ClassName		= 'TextMessage';
SET @DerivesFrom	= 'IBusinessObject';
SET @MappedEntitiy	= 'cla_TextMessage';
SET @EntityObject	= '_Entity'
SET @ColumnPrefixToReplace = '';
SET @ColumnPrefixReplaceWith = '';
SET @ColumnPrefixReplaceEntityProperties = 0;


-- ***************************************************************
-- Execute script - don't alter anything beneath this line.
DECLARE @ColumnName		NVARCHAR(256)
		, @IsNullable	BIT
		, @DataType		NVARCHAR(128)
		, @MaxLength	NVARCHAR(256)
		, @MaxValue		NVARCHAR(256);

DECLARE @PropertyBuilder		NVARCHAR(MAX)
		, @DataTypeHelper		NVARCHAR(256)
		, @FormattedColumnName	NVARCHAR(256)
		, @LoweredClassName NVARCHAR(256);

SET @LoweredClassName = LOWER(SUBSTRING(@ClassName, 1, 1)) + SUBSTRING(@ClassName, 2, LEN(@ClassName));

IF (@ColumnPrefixReplaceEntityProperties IS NULL)
	SET @ColumnPrefixReplaceEntityProperties = 0;

DECLARE @ClassConstruct		NVARCHAR(MAX);

IF (@DerivesFrom IS NOT NULL AND @DerivesFrom <> '')
	SET @DerivesFrom = ' : ' + @DerivesFrom;

-- ***************************************************************
-- Header
SET @ClassConstruct = 
'
/// <summary>
/// 
/// </summary>
public class ' + @ClassName  + @DerivesFrom + '
{
	#region members
	
	protected ' + @MappedEntitiy + ' ' + @EntityObject + ';
    private bool _IsLoadedFromDatabase;
	
	#endregion

	#region Properties
';

PRINT @ClassConstruct;


-- ***************************************************************
-- Properties
SET		@PropertyBuilder = '';

DECLARE table_cursor CURSOR FOR
SELECT	c.COLUMN_NAME
		, CASE c.IS_NULLABLE 
			WHEN 'NO' THEN 0
			WHEN 'YES' THEN 1
		  END
		, c.DATA_TYPE
		, c.CHARACTER_MAXIMUM_LENGTH
FROM	INFORMATION_SCHEMA.COLUMNS c
WHERE	table_name = @TableName

OPEN table_cursor

FETCH NEXT FROM table_cursor 
INTO @ColumnName, @IsNullable, @DataType, @MaxLength
WHILE @@FETCH_STATUS = 0
BEGIN

	SET @FormattedColumnName = @ColumnName; 
	IF (@ColumnPrefixToReplace IS NOT NULL AND @ColumnPrefixToReplace <> '')
		SET @FormattedColumnName = REPLACE(@ColumnName, @ColumnPrefixToReplace, @ColumnPrefixReplaceWith);
	IF (@ColumnPrefixReplaceEntityProperties = 1)
		SET @ColumnName = @FormattedColumnName; 

	-- first convert the datatype
	SET @DataTypeHelper =	CASE @DataType
								WHEN 'uniqueidentifier' THEN 'Guid'
								WHEN 'decimal' THEN 'decimal'
								WHEN 'float' THEN 'float'
								WHEN 'double' THEN 'double'
								WHEN 'bit' THEN 'bool'
								WHEN 'nvarchar' THEN 'string'
								WHEN 'varchar' THEN 'string'
								WHEN 'text' THEN 'string'
								WHEN 'char' THEN 'char'
								WHEN 'tinyint' THEN 'byte'
								WHEN 'smallint' THEN 'short'
								WHEN 'int' THEN 'int'
								WHEN 'bigint' THEN 'long'
								WHEN 'datetime' THEN 'DateTime'
								WHEN 'smalldatetime' THEN 'DateTime'
								WHEN 'xml' THEN 'XElement'
								ELSE @DataType
							END

	-- check/set the max length
	IF (@MaxLength IS NULL)
	BEGIN
		SET @MaxLength = 	CASE @DataType
								WHEN 'nvarchar' THEN '16384'
								WHEN 'varchar' THEN '16384'
								WHEN 'text' THEN '16384'
								WHEN 'char' THEN '1'								
								ELSE NULL
							END
		SET @MaxValue =		CASE @DataType
								WHEN 'decimal' THEN '79228162514264337593543950335'
								WHEN 'float' THEN '-1'
								WHEN 'double' THEN '-1'
								WHEN 'tinyint' THEN '255'
								WHEN 'smallint' THEN '32767'
								WHEN 'int' THEN '2147483647'
								WHEN 'bigint' THEN '9223372036854775807'
								ELSE NULL
							END
	END

	-- append the nullable type identifier
	IF (@IsNullable = 1 AND @DataTypeHelper <> 'string')
		SET @DataTypeHelper = @DataTypeHelper + '?';

	-- create attributes
	SET @PropertyBuilder = 
'	
	[BusinessObjectProperty(IsMandatoryForInstance = ' +	CASE @IsNullable 
																WHEN 0 THEN 'true' 
																WHEN 1 THEN 'false' 
															END + ')]
	[BusinessObjectValidation(
		IsRequired = ' +	CASE @IsNullable 
								WHEN 0 THEN 'true' 
								WHEN 1 THEN 'false' 
							END + ', IsRequiredMessage = "Please enter a ' + @FormattedColumnName + '", IsRequiredMessageResourceKey = null';

	IF (@MaxLength IS NOT NULL)
	BEGIN
		SET @PropertyBuilder = @PropertyBuilder +
'
        , MinLength = 0, MaxLength = ' + @MaxLength + ', OutOfRangeErrorMessage = "' +	
			CASE @DataTypeHelper
				WHEN 'string' THEN 'The ' + @FormattedColumnName + ' must consist of at least 0 and a maximum of ' + @MaxLength + ' characters.' 
				ELSE 'Please enter a value between 0 and ' + @MaxLength + '.'
			END +'", OutOfRangeErrorMessageResourceKey = null';

	END
	ELSE IF (@MaxValue IS NOT NULL)
	BEGIN
		SET @PropertyBuilder = @PropertyBuilder +
'
        , MinimumValue = "0", MaximumValue = "' + @MaxValue + '", OutOfRangeErrorMessage = "' +	
			CASE @DataTypeHelper
				WHEN 'string' THEN 'The ' + @FormattedColumnName + ' must consist of at least 0 and a maximum of ' + @MaxValue + ' characters.' 
				ELSE 'Please enter a value between 0 and ' + @MaxValue + '.'
			END +'", OutOfRangeErrorMessageResourceKey = null';

	END

	SET @PropertyBuilder = @PropertyBuilder + ')]';

	IF (@DataTypeHelper = 'string')
	BEGIN
		SET @PropertyBuilder = @PropertyBuilder +
'
	[BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]';
	END

	-- finally, create property
	SET @PropertyBuilder = @PropertyBuilder +
'
	public ' + @DataTypeHelper + ' ' + @FormattedColumnName + '
	{
		get { return ' + @EntityObject + '.' + @ColumnName + '; }
		set { ' + @EntityObject + '.' + @ColumnName + ' = value; }
	}
';

	PRINT @PropertyBuilder;

    FETCH NEXT FROM table_cursor 
    INTO @ColumnName, @IsNullable, @DataType, @MaxLength
END 
CLOSE table_cursor
DEALLOCATE table_cursor

-- ***************************************************************
-- IBusinessObject interface
IF (@DerivesFrom IS NOT NULL AND PATINDEX('%IBusinessObject%', @DerivesFrom) >= 0)
BEGIN 
SET @ClassConstruct = 
'	
    #region IBusinessObject Members

    /// <summary>
    /// Determines whether this object can be inserted 
    /// </summary>
    public bool IsCreateAble
    {
        get { return !IsLoadedFromDatabase; }
    }

    /// <summary>
    /// Determines whether this object can be deleted
    /// </summary>
    public bool IsDeleteAble
    {
        get { return IsLoadedFromDatabase; }
    }

    /// <summary>
    /// Determines whether this object was loaded from a database
    /// </summary>
    public bool IsLoadedFromDatabase
    {
        get { return _IsLoadedFromDatabase; }
        internal set { _IsLoadedFromDatabase = value; }
    }

    /// <summary>
    /// Determines whether this object can be updated
    /// </summary>
    public bool IsUpdateAble
    {
        get { return IsLoadedFromDatabase; }
    }

    #endregion
';
	PRINT @ClassConstruct;
END

-- ***************************************************************
-- Constructor
SET @ClassConstruct = 
'
	#endregion
		
	#region constructors

	public ' + @ClassName  + '()
	{
		_IsLoadedFromDatabase = false;
		' + @EntityObject + ' = new ' +  @MappedEntitiy + '();
	}

	internal ' + @ClassName  + '(' + @MappedEntitiy + ' ' + @LoweredClassName + ')
	{
		_IsLoadedFromDatabase = true;
		' + @EntityObject + ' = '+@LoweredClassName+';
	}

	#endregion
} 
';
PRINT @ClassConstruct;