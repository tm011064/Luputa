Author: Roman Majewski

Description:

Update: Version 1.0.3.0
From this build number on, asynchronous logging is possible and there are additional table columns at the database which enable to store the 
authenticated userid (if it exists) and the machinename for exceptions. The LogManager.SQL2005.InstallationScripts.1.0.3.0.sql sql script must
be run against existing projects so the stored procedures can handle this change.

CommonTools.Components.Logging.LogManager implements two database table to log exceptions and events respectively. Everything you need to
do to use this class is to provide a connectionstrin name (defined at app.config or web.config).

web.config example: 

<configuration>

	<configSections>
		<section name="Logging" type="CommonTools.Components.Logging.LogSection, CommonTools" />    
	</configSections>
  
	<Logging connectionStringName="DummyBaseConnectionString" storedProcedurePrefix="proc_ct_" />
  
	<connectionStrings>
		<add name="DummyBaseConnectionString" connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=DummyBase;Integrated Security=True" providerName="System.Data.SqlClient"/>
	</connectionStrings>
	
</configuration>



SQL installation: run the script found at LogManager.SQL.InstallationScripts.sql against your database. Follow the instructions on top of the 
document to adapt your custom SQL naming standards to the LogManager tables and stored procedures.



Additionally, the LogManager comes with two webcontrols for easily viewing/paging through/deleting event- and exception records. To use these
controls, put the following webcontrols on your page:

<%@ Register TagPrefix="TGS" Namespace="CommonTools.Components.Logging" Assembly="CommonTools.Components" %>

<TGS:ExceptionLogView ID="lvLogs" runat="server" UseDefaultStyling="true" MaxPagerItemsPerRow="7"
    ApplicationLocationEnumType="MSNWrapper.Configuration.ApplicationLocation, MSNWrapper.Configuration" />

<TGS:EventLogView ID="lvLogs" runat="server" UseDefaultStyling="true" MaxPagerItemsPerRow="7" 
	EventTypesEnumType="MSNWrapper.Configuration.EventLogType, MSNWrapper.Configuration"
    ApplicationLocationEnumType="MSNWrapper.Configuration.ApplicationLocation, MSNWrapper.Configuration" />
    
These webcontrols rely on Enum (type = System.Int32 or smaller) for AppLocations and EventTypes.