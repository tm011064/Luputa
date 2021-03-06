﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DummyDatabase.SqlProvider
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	public partial class UsersDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertIncrementingUser(IncrementingUser instance);
    partial void UpdateIncrementingUser(IncrementingUser instance);
    partial void DeleteIncrementingUser(IncrementingUser instance);
    partial void InsertUniqueUser(UniqueUser instance);
    partial void UpdateUniqueUser(UniqueUser instance);
    partial void DeleteUniqueUser(UniqueUser instance);
    #endregion
		
		public UsersDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UsersDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UsersDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UsersDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<IncrementingUser> IncrementingUsers
		{
			get
			{
				return this.GetTable<IncrementingUser>();
			}
		}
		
		public System.Data.Linq.Table<UniqueUser> UniqueUsers
		{
			get
			{
				return this.GetTable<UniqueUser>();
			}
		}
		
		[Function(Name="dbo.IncrementingUsers_Delete")]
		public int DeleteIncrementingUser([Parameter(Name="UserID", DbType="Int")] System.Nullable<int> userID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID);
			return ((int)(result.ReturnValue));
		}
		
		[Function(Name="dbo.UniqueUsers_Delete")]
		public int DeleteUniqueUser([Parameter(Name="UserID", DbType="UniqueIdentifier")] System.Nullable<System.Guid> userID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID);
			return ((int)(result.ReturnValue));
		}
		
		[Function(Name="dbo.UniqueUsers_GetUserPageOrderedByDateOfBirth")]
		public ISingleResult<UniqueUser> GetUniqueUserPageOrderedByDateOfBirth([Parameter(Name="PageIndex", DbType="Int")] System.Nullable<int> pageIndex, [Parameter(Name="PageSize", DbType="Int")] System.Nullable<int> pageSize)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pageIndex, pageSize);
			return ((ISingleResult<UniqueUser>)(result.ReturnValue));
		}
		
		[Function(Name="dbo.IncrementingUsers_Insert")]
		public ISingleResult<IncrementingUsers_InsertResult> InsertIncrementingUser([Parameter(Name="AccountStatus", DbType="TinyInt")] System.Nullable<byte> accountStatus, [Parameter(Name="Timezone", DbType="Float")] System.Nullable<double> timezone, [Parameter(Name="Firstname", DbType="NVarChar(256)")] string firstname, [Parameter(Name="Lastname", DbType="NVarChar(256)")] string lastname, [Parameter(Name="DateOfBirth", DbType="DateTime")] System.Nullable<System.DateTime> dateOfBirth, [Parameter(Name="City", DbType="NVarChar(256)")] string city, [Parameter(Name="IsNewletterSubscriber", DbType="Bit")] System.Nullable<bool> isNewletterSubscriber)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), accountStatus, timezone, firstname, lastname, dateOfBirth, city, isNewletterSubscriber);
			return ((ISingleResult<IncrementingUsers_InsertResult>)(result.ReturnValue));
		}
		
		[Function(Name="dbo.UniqueUsers_Insert")]
		public int InsertUniqueUser([Parameter(Name="UserID", DbType="UniqueIdentifier")] System.Nullable<System.Guid> userID, [Parameter(Name="AccountStatus", DbType="TinyInt")] System.Nullable<byte> accountStatus, [Parameter(Name="Timezone", DbType="Float")] System.Nullable<double> timezone, [Parameter(Name="Firstname", DbType="NVarChar(256)")] string firstname, [Parameter(Name="Lastname", DbType="NVarChar(256)")] string lastname, [Parameter(Name="DateOfBirth", DbType="DateTime")] System.Nullable<System.DateTime> dateOfBirth, [Parameter(Name="City", DbType="NVarChar(256)")] string city, [Parameter(Name="IsNewletterSubscriber", DbType="Bit")] System.Nullable<bool> isNewletterSubscriber)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID, accountStatus, timezone, firstname, lastname, dateOfBirth, city, isNewletterSubscriber);
			return ((int)(result.ReturnValue));
		}
		
		[Function(Name="dbo.IncrementingUsers_Update")]
		public int UpdateIncrementingUser([Parameter(Name="UserID", DbType="Int")] System.Nullable<int> userID, [Parameter(Name="AccountStatus", DbType="TinyInt")] System.Nullable<byte> accountStatus, [Parameter(Name="Timezone", DbType="Float")] System.Nullable<double> timezone, [Parameter(Name="Firstname", DbType="NVarChar(256)")] string firstname, [Parameter(Name="Lastname", DbType="NVarChar(256)")] string lastname, [Parameter(Name="DateOfBirth", DbType="DateTime")] System.Nullable<System.DateTime> dateOfBirth, [Parameter(Name="City", DbType="NVarChar(256)")] string city, [Parameter(Name="IsNewletterSubscriber", DbType="Bit")] System.Nullable<bool> isNewletterSubscriber)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID, accountStatus, timezone, firstname, lastname, dateOfBirth, city, isNewletterSubscriber);
			return ((int)(result.ReturnValue));
		}
		
		[Function(Name="dbo.UniqueUsers_Update")]
		public int UpdateUniqueUser([Parameter(Name="UserID", DbType="UniqueIdentifier")] System.Nullable<System.Guid> userID, [Parameter(Name="AccountStatus", DbType="TinyInt")] System.Nullable<byte> accountStatus, [Parameter(Name="Timezone", DbType="Float")] System.Nullable<double> timezone, [Parameter(Name="Firstname", DbType="NVarChar(256)")] string firstname, [Parameter(Name="Lastname", DbType="NVarChar(256)")] string lastname, [Parameter(Name="DateOfBirth", DbType="DateTime")] System.Nullable<System.DateTime> dateOfBirth, [Parameter(Name="City", DbType="NVarChar(256)")] string city, [Parameter(Name="IsNewletterSubscriber", DbType="Bit")] System.Nullable<bool> isNewletterSubscriber)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID, accountStatus, timezone, firstname, lastname, dateOfBirth, city, isNewletterSubscriber);
			return ((int)(result.ReturnValue));
		}
	}
	
	[Table(Name="dbo.IncrementingUsers")]
	public partial class IncrementingUser : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _UserID;
		
		private byte _AccountStatus;
		
		private double _Timezone;
		
		private string _Firstname;
		
		private string _Lastname;
		
		private System.DateTime _DateOfBirth;
		
		private string _City;
		
		private bool _IsNewletterSubscriber;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIDChanging(int value);
    partial void OnUserIDChanged();
    partial void OnAccountStatusChanging(byte value);
    partial void OnAccountStatusChanged();
    partial void OnTimezoneChanging(double value);
    partial void OnTimezoneChanged();
    partial void OnFirstnameChanging(string value);
    partial void OnFirstnameChanged();
    partial void OnLastnameChanging(string value);
    partial void OnLastnameChanged();
    partial void OnDateOfBirthChanging(System.DateTime value);
    partial void OnDateOfBirthChanged();
    partial void OnCityChanging(string value);
    partial void OnCityChanged();
    partial void OnIsNewletterSubscriberChanging(bool value);
    partial void OnIsNewletterSubscriberChanged();
    #endregion
		
		public IncrementingUser()
		{
			OnCreated();
		}
		
		[Column(Storage="_UserID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[Column(Storage="_AccountStatus", DbType="TinyInt NOT NULL")]
		public byte AccountStatus
		{
			get
			{
				return this._AccountStatus;
			}
			set
			{
				if ((this._AccountStatus != value))
				{
					this.OnAccountStatusChanging(value);
					this.SendPropertyChanging();
					this._AccountStatus = value;
					this.SendPropertyChanged("AccountStatus");
					this.OnAccountStatusChanged();
				}
			}
		}
		
		[Column(Storage="_Timezone", DbType="Float NOT NULL")]
		public double Timezone
		{
			get
			{
				return this._Timezone;
			}
			set
			{
				if ((this._Timezone != value))
				{
					this.OnTimezoneChanging(value);
					this.SendPropertyChanging();
					this._Timezone = value;
					this.SendPropertyChanged("Timezone");
					this.OnTimezoneChanged();
				}
			}
		}
		
		[Column(Storage="_Firstname", DbType="NVarChar(256)")]
		public string Firstname
		{
			get
			{
				return this._Firstname;
			}
			set
			{
				if ((this._Firstname != value))
				{
					this.OnFirstnameChanging(value);
					this.SendPropertyChanging();
					this._Firstname = value;
					this.SendPropertyChanged("Firstname");
					this.OnFirstnameChanged();
				}
			}
		}
		
		[Column(Storage="_Lastname", DbType="NVarChar(256)")]
		public string Lastname
		{
			get
			{
				return this._Lastname;
			}
			set
			{
				if ((this._Lastname != value))
				{
					this.OnLastnameChanging(value);
					this.SendPropertyChanging();
					this._Lastname = value;
					this.SendPropertyChanged("Lastname");
					this.OnLastnameChanged();
				}
			}
		}
		
		[Column(Storage="_DateOfBirth", DbType="DateTime NOT NULL")]
		public System.DateTime DateOfBirth
		{
			get
			{
				return this._DateOfBirth;
			}
			set
			{
				if ((this._DateOfBirth != value))
				{
					this.OnDateOfBirthChanging(value);
					this.SendPropertyChanging();
					this._DateOfBirth = value;
					this.SendPropertyChanged("DateOfBirth");
					this.OnDateOfBirthChanged();
				}
			}
		}
		
		[Column(Storage="_City", DbType="NVarChar(256)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this.OnCityChanging(value);
					this.SendPropertyChanging();
					this._City = value;
					this.SendPropertyChanged("City");
					this.OnCityChanged();
				}
			}
		}
		
		[Column(Storage="_IsNewletterSubscriber", DbType="Bit NOT NULL")]
		public bool IsNewletterSubscriber
		{
			get
			{
				return this._IsNewletterSubscriber;
			}
			set
			{
				if ((this._IsNewletterSubscriber != value))
				{
					this.OnIsNewletterSubscriberChanging(value);
					this.SendPropertyChanging();
					this._IsNewletterSubscriber = value;
					this.SendPropertyChanged("IsNewletterSubscriber");
					this.OnIsNewletterSubscriberChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.UniqueUsers")]
	public partial class UniqueUser : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _UserID;
		
		private byte _AccountStatus;
		
		private double _Timezone;
		
		private string _Firstname;
		
		private string _Lastname;
		
		private System.DateTime _DateOfBirth;
		
		private string _City;
		
		private bool _IsNewletterSubscriber;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIDChanging(System.Guid value);
    partial void OnUserIDChanged();
    partial void OnAccountStatusChanging(byte value);
    partial void OnAccountStatusChanged();
    partial void OnTimezoneChanging(double value);
    partial void OnTimezoneChanged();
    partial void OnFirstnameChanging(string value);
    partial void OnFirstnameChanged();
    partial void OnLastnameChanging(string value);
    partial void OnLastnameChanged();
    partial void OnDateOfBirthChanging(System.DateTime value);
    partial void OnDateOfBirthChanged();
    partial void OnCityChanging(string value);
    partial void OnCityChanged();
    partial void OnIsNewletterSubscriberChanging(bool value);
    partial void OnIsNewletterSubscriberChanged();
    #endregion
		
		public UniqueUser()
		{
			OnCreated();
		}
		
		[Column(Storage="_UserID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[Column(Storage="_AccountStatus", DbType="TinyInt NOT NULL")]
		public byte AccountStatus
		{
			get
			{
				return this._AccountStatus;
			}
			set
			{
				if ((this._AccountStatus != value))
				{
					this.OnAccountStatusChanging(value);
					this.SendPropertyChanging();
					this._AccountStatus = value;
					this.SendPropertyChanged("AccountStatus");
					this.OnAccountStatusChanged();
				}
			}
		}
		
		[Column(Storage="_Timezone", DbType="Float NOT NULL")]
		public double Timezone
		{
			get
			{
				return this._Timezone;
			}
			set
			{
				if ((this._Timezone != value))
				{
					this.OnTimezoneChanging(value);
					this.SendPropertyChanging();
					this._Timezone = value;
					this.SendPropertyChanged("Timezone");
					this.OnTimezoneChanged();
				}
			}
		}
		
		[Column(Storage="_Firstname", DbType="NVarChar(256)")]
		public string Firstname
		{
			get
			{
				return this._Firstname;
			}
			set
			{
				if ((this._Firstname != value))
				{
					this.OnFirstnameChanging(value);
					this.SendPropertyChanging();
					this._Firstname = value;
					this.SendPropertyChanged("Firstname");
					this.OnFirstnameChanged();
				}
			}
		}
		
		[Column(Storage="_Lastname", DbType="NVarChar(256)")]
		public string Lastname
		{
			get
			{
				return this._Lastname;
			}
			set
			{
				if ((this._Lastname != value))
				{
					this.OnLastnameChanging(value);
					this.SendPropertyChanging();
					this._Lastname = value;
					this.SendPropertyChanged("Lastname");
					this.OnLastnameChanged();
				}
			}
		}
		
		[Column(Storage="_DateOfBirth", DbType="DateTime NOT NULL")]
		public System.DateTime DateOfBirth
		{
			get
			{
				return this._DateOfBirth;
			}
			set
			{
				if ((this._DateOfBirth != value))
				{
					this.OnDateOfBirthChanging(value);
					this.SendPropertyChanging();
					this._DateOfBirth = value;
					this.SendPropertyChanged("DateOfBirth");
					this.OnDateOfBirthChanged();
				}
			}
		}
		
		[Column(Storage="_City", DbType="NVarChar(256)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this.OnCityChanging(value);
					this.SendPropertyChanging();
					this._City = value;
					this.SendPropertyChanged("City");
					this.OnCityChanged();
				}
			}
		}
		
		[Column(Storage="_IsNewletterSubscriber", DbType="Bit NOT NULL")]
		public bool IsNewletterSubscriber
		{
			get
			{
				return this._IsNewletterSubscriber;
			}
			set
			{
				if ((this._IsNewletterSubscriber != value))
				{
					this.OnIsNewletterSubscriberChanging(value);
					this.SendPropertyChanging();
					this._IsNewletterSubscriber = value;
					this.SendPropertyChanged("IsNewletterSubscriber");
					this.OnIsNewletterSubscriberChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class IncrementingUsers_InsertResult
	{
		
		private int _UserID;
		
		private byte _AccountStatus;
		
		private double _Timezone;
		
		private string _Firstname;
		
		private string _Lastname;
		
		private System.DateTime _DateOfBirth;
		
		private string _City;
		
		private bool _IsNewletterSubscriber;
		
		public IncrementingUsers_InsertResult()
		{
		}
		
		[Column(Storage="_UserID", DbType="Int NOT NULL")]
		public int UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this._UserID = value;
				}
			}
		}
		
		[Column(Storage="_AccountStatus", DbType="TinyInt NOT NULL")]
		public byte AccountStatus
		{
			get
			{
				return this._AccountStatus;
			}
			set
			{
				if ((this._AccountStatus != value))
				{
					this._AccountStatus = value;
				}
			}
		}
		
		[Column(Storage="_Timezone", DbType="Float NOT NULL")]
		public double Timezone
		{
			get
			{
				return this._Timezone;
			}
			set
			{
				if ((this._Timezone != value))
				{
					this._Timezone = value;
				}
			}
		}
		
		[Column(Storage="_Firstname", DbType="NVarChar(256)")]
		public string Firstname
		{
			get
			{
				return this._Firstname;
			}
			set
			{
				if ((this._Firstname != value))
				{
					this._Firstname = value;
				}
			}
		}
		
		[Column(Storage="_Lastname", DbType="NVarChar(256)")]
		public string Lastname
		{
			get
			{
				return this._Lastname;
			}
			set
			{
				if ((this._Lastname != value))
				{
					this._Lastname = value;
				}
			}
		}
		
		[Column(Storage="_DateOfBirth", DbType="DateTime NOT NULL")]
		public System.DateTime DateOfBirth
		{
			get
			{
				return this._DateOfBirth;
			}
			set
			{
				if ((this._DateOfBirth != value))
				{
					this._DateOfBirth = value;
				}
			}
		}
		
		[Column(Storage="_City", DbType="NVarChar(256)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this._City = value;
				}
			}
		}
		
		[Column(Storage="_IsNewletterSubscriber", DbType="Bit NOT NULL")]
		public bool IsNewletterSubscriber
		{
			get
			{
				return this._IsNewletterSubscriber;
			}
			set
			{
				if ((this._IsNewletterSubscriber != value))
				{
					this._IsNewletterSubscriber = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
