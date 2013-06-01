Author: Roman Majewski

Description:
The BusinessObjectManager class has two purposes: to automatically validate business object's properties before database
creates/updates/deletes and to provide an easy mechanism for storing/loading business objects at HttpRuntime cache, using
the web.config (or app.config) file to control the cache behaviour for each object. 

The following example illustrates how to use the BusinessObjectManager class. It should only show what can be done, so it won't
compile unless you don't provide a database with an according LINQ to SQL datacontext.

Note how the IBusinessObject UniqueUser property values get validated before Create/Update/Delete database
calls and how caching is implemented via an anonymous delegate call to the common database select:

	return BusinessObjectManager<UniqueUser>.LoadFromCache(typeof(UniqueUserManager), userId.ToString()
		, delegate() { return GetUser(userId); });



Sample Code:

[BusinessObjectCache("UniqueUser")]
public static class UniqueUserManager
{
   public static UniqueUser GetUser(Guid userId)
   {
       UsersDataContext dc = Configuration.GetUsersDataContext();
       var record = dc.UniqueUsers.SingleOrDefault(c => c.UserID == userId);
       if (record != null)
           return new UniqueUser(record);
       return null;
   }
   public static UniqueUser GetUser(Guid userId, bool useCache)
   {
       if (useCache)
       {
           return BusinessObjectManager<UniqueUser>.LoadFromCache(typeof(UniqueUserManager), userId.ToString()
              , delegate() { return GetUser(userId); });
       }
       return GetUser(userId);
   }
   public static UniqueUser Create(UniqueUser user)
   {
       BusinessObjectValidationResult result = BusinessObjectManager<UniqueUser>.ValidateAction(user, ActionType.Create);
       if (result.IsValid)
       {
           UsersDataContext dc = Configuration.GetUsersDataContext();
           int affectedRows = dc.InsertUniqueUser(user.UserID, user.AccountStatus, user.Timezone, user.Firstname, user.Lastname
               , user.DateOfBirth, user.City, user.IsNewletterSubscriber);
           if (affectedRows == 1)
               return user;
       }
       else
       {
           // Log the status, notify someone...
       }
       return null;
   }
   public static UniqueUser Update(UniqueUser user)
   {
       BusinessObjectValidationResult result = BusinessObjectManager<UniqueUser>.ValidateAction(user, ActionType.Update);
       if (result.IsValid)
       {
           UsersDataContext dc = Configuration.GetUsersDataContext();
           if (dc.UpdateUniqueUser(user.UserID, user.AccountStatus, user.Timezone, user.Firstname, user.Lastname
               , user.DateOfBirth, user.City, user.IsNewletterSubscriber) == 1)
           {
               return user;
           }
       }
       else
       {
           // Log the status, notify someone...
       }
       return null;
   }
   public static bool Delete(UniqueUser user)
   {
       BusinessObjectValidationResult result = BusinessObjectManager<UniqueUser>.ValidateAction(user, ActionType.Update);
       if (result.IsValid)
       {
           UsersDataContext dc = Configuration.GetUsersDataContext();
           return (dc.DeleteUniqueUser(user.UserID) == 1);
       }
       else
       {
           // Log the status, notify someone...
       }
       return false;
   }
}

public class UniqueUser : IBusinessObject
{
  	#region globals
  	SqlProvider.UsersDataContext _DataContext;
  	SqlProvider.UniqueUser _Record;
  	#endregion

        #region Properties
  	[BusinessObjectProperty(IsMandatoryForInstance = true)]
  	public Guid UserID
  	{
  	    get { return _Record.UserID; }
  	    internal set { _Record.UserID = value; }
  	}
  	[BusinessObjectProperty(IsMandatoryForInstance = true)]
  	[BusinessObjectValidation(IsRequired = true, MinimumValue = "0"
  	   , MaximumValue = "12", OutOfRangeErrorMessage = "Please provide a value between 0 and 12")]
  	public byte AccountStatus
  	{
  	    get { return _Record.AccountStatus; }
  	    set { _Record.AccountStatus = value; }
  	}
  	[BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
  	[BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
  	public string City
  	{
  	    get { return _Record.City; }
  	    set { _Record.City = value; }
  	}
  	[BusinessObjectProperty(IsMandatoryForInstance = true)]
  	public DateTime DateOfBirth
  	{
  	    get { return _Record.DateOfBirth; }
  	    set { _Record.DateOfBirth = value; }
  	}
  	[BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
  	[BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
  	public string Firstname
  	{
  	    get { return _Record.Firstname; }
  	    set { _Record.Firstname = value; }
  	}
  	[BusinessObjectProperty(IsMandatoryForInstance = true)]
  	public bool IsNewletterSubscriber
  	{
  	    get { return _Record.IsNewletterSubscriber; }
  	    set { _Record.IsNewletterSubscriber = value; }
  	}
  	[BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
  	[BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
  	public string Lastname
  	{
  	    get { return _Record.Lastname; }
  	    set { _Record.Lastname = value; }
  	}
  	[BusinessObjectProperty(IsMandatoryForInstance = true)]
  	[BusinessObjectValidation(IsRequired = true, MinimumValue = "-12"
  	  , MaximumValue = "12", OutOfRangeErrorMessage = "Please enter a value between -12 and 12")]
  	public double Timezone
  	{
  	    get { return _Record.Timezone; }
  	    set { _Record.Timezone = value; }
  	}
  	#endregion

  	#region constructors
  	public UniqueUser()
  	{
  	    _IsLoadedFromDatabase = false;

  	    _DataContext = Configuration.GetUsersDataContext();
  	    _Record = new SqlProvider.UniqueUser() { UserID = Guid.NewGuid(), AccountStatus = 0, IsNewletterSubscriber = false };
  	}

  	internal UniqueUser(SqlProvider.UniqueUser userRecord)
  	{
  	    _IsLoadedFromDatabase = true;

  	    _DataContext = Configuration.GetUsersDataContext();
  	    _Record = userRecord;
  	}
  	#endregion

  	#region IBusinessObject Members

  	public bool IsCreateAble
  	{
  	    get { return !IsLoadedFromDatabase; }
  	}

  	public bool IsDeleteAble
  	{
  	    get { return IsLoadedFromDatabase; }
  	}

  	private bool _IsLoadedFromDatabase;
  	public bool IsLoadedFromDatabase
  	{
  	    get { return _IsLoadedFromDatabase; }
  	}

  	public bool IsUpdateAble
  	{
  	    get { return IsLoadedFromDatabase; }
  	}

  	#endregion
}