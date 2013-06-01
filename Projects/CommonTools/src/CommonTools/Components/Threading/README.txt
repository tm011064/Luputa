Author: Roman Majewski

Description:
Via CommonTools.Components.Threading.Jobs you can easily reference/instance a threadpool defined at
web.config with a few lines of code. Steps needed to implement this process:

	1) create a new job (=thread) [1]
	2) register the job at web.config [2]
	3) Start the thread-execution process in global.asax [3]
	4) Done.

Description:

	[1] Syntax for a job:		
	public class FirstJob : IJob
    {
        #region IJob Members
        public void Execute(NameValueConfigurationCollection options)
        {
            // code that executes when the thread starts

            if (options != null)
            {
                // ... get values defined in web.config's <Options> NameValueElement ...
            }
        }
        #endregion
    }
    
    Description: Everything you need to do to write a job is to implement the IJob interface. This interface
	implements the Execute(NameValueConfigurationCollection) method that gets called when the job executes.
    
	[2] web.config:
	<configSections>
		<section name="Jobs" type="CommonTools.Components.Threading.JobSection, CommonTools.Components" />
	</configSections>

	<Jobs>
		<jobPool minutes="10">
			  <add name="FirstJob"	
				   type="MyJobs.FirstJob, App_Code"	
				   enabled="true" 
				   enableShutDown="false" 
				   firstRunAtInitialization="true">
				<Options>
				  <add name="FirstValue" value="val1" />
				  <add name="SecondValue" value="val2" />
				  <add name="ThirdValue" value="val3" />
				</Options>
			  </add>
			  <add name="SecondJob"	
				   type="MyJobs.SecondJob, App_Code"	
				   enabled="true" 
				   enableShutDown="false" 
				   firstRunAtInitialization="false" />
			  <add name="NoSingleJob"	
				   type="MyJobs.NoSingleJob, App_Code" 
				   enabled="true" 
				   enableShutDown="false" 
				   executeOnOwnThread="true" 
				   seconds="10" />
			 <add name="DailyJob"
				   type="MyJobs.DailyJob, App_Code"
				   enabled="true"
				   enableShutDown="false"
				   executeDaily="true"
				   dailyUTCExecutionTime="11:11:00.000" />
			</jobPool>
		</Jobs>
	
	Description: At the web.config file a new ConfigSection named 'Jobs' must be defined. You can use the 
	web.config file itself to define the jobs or use a custom configuration object which implements the IJobController
	interface (add jobControllerProviderType="myJobControllerType, MyAssembly" to the Jobs tag).
	
	Ad web.config: Every Thread needs to be placed inside the <jobPool> Tag as a NameValueElement. The <jobPool> Tag
	itself must provide a "minutes" attribute. The value of this attribute defines the standard interval for all
	jobs that don't overwrite it (with minutes or seconds) or implement the 'executeDaily' attribute.
	
	There are three modes a job can run on: minutes interval, seconds interval and daily execution at a fixed time.
	Minutes and Seconds are self explaining (the seconds attribute overwrites the minutes attribute), the executedaily
	attribute forces the job to execute at a fixed daytime, defined in the UTC format (Attention! UTC format ignores
	daylight saving times!).
		
	Inside a job NameValueElement additional options can be defined via the <Options> NameValueCollection tag.
	This NameValueCollection can hold values that get passed to the actual job instance as a parameter.

	[3] Global.asax:
	void Application_Start(object sender, EventArgs e) 
	{
		// Code that runs on application startup
		CommonTools.Components.Threading.Jobs.Instance().Start();
	}

	void Application_End(object sender, EventArgs e) 
	{
		//  Code that runs on application shutdown
		CommonTools.Components.Threading.Jobs.Instance().Stop();
	}
	
	Description: In order to instance the threads they need to be started at Global.asax.
	
Example:
	Do the following: create a website and copy/paste the code you see at (1). Next, create a class file and copy/paste (2)
	into it. Create a global.asax (3) and web.config (4) file at your root and open (1) in your browser. Now you should 
	see all current threads (note that the threads actually start after the first interval is completed).

	-------------------------------------------------------------------------------------------------------------------------
	1) Webpage (default.aspx)
	================================================
	<%@ Page Language="C#" AutoEventWireup="true" %>

	<%@ Register TagPrefix="CV" Namespace="CommonTools.Components.Threading" Assembly="CommonTools.Components" %>
	
	<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
	<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
	</head>
		<body>
			<form id="form1" runat="server">
				<div>
					<CV:JobView ID="cv1" runat="server" Theme="Default" Font-Names="Courier New"
						Font-Size="13px" /></div>
			</form>
		</body>
	</html>
	
	-------------------------------------------------------------------------------------------------------------------------
	2) Class file 
	================================================
	using System;
	using CommonTools.Components.Threading;
	using System.Configuration;

	namespace MyJobs
	{
		public class FirstJob : IJob
		{
			public FirstJob() { }

			#region IJob Members

			public void Execute(NameValueConfigurationCollection node)
			{
				string s = string.Empty;
				if (node != null)
				{
					foreach (NameValueConfigurationElement el in node)
					{
						s += el.Value;
					}
				}
				// insert your code...
			}

			#endregion
		}
		public class SecondJob : IJob
		{
			public SecondJob() { }

			#region IJob Members

			public void Execute(NameValueConfigurationCollection node)
			{
				// insert your code...
			}

			#endregion
		}
		public class NoSingleJob : IJob
		{
			public NoSingleJob() { }

			#region IJob Members

			public void Execute(NameValueConfigurationCollection node)
			{
				// insert your code...
			}

			#endregion
		}
		public class DailyJob : IJob
		{
			public DailyJob() { }

			#region IJob Members

			public void Execute(Dictionary<string, string> node)
			{
				// insert your code...
			}

			#endregion
		}
	}

	-------------------------------------------------------------------------------------------------------------------------
	3) Global.asax 
	================================================
	<%@ Application Language="C#" %>

	<script runat="server">

		void Application_Start(object sender, EventArgs e) 
		{
			CommonTools.Components.Threading.Jobs.Instance().Start();
		}	    
		void Application_End(object sender, EventArgs e) 
		{
			CommonTools.Components.Threading.Jobs.Instance().Stop();
		}
	        
		void Application_Error(object sender, EventArgs e) { }
		void Session_Start(object sender, EventArgs e) { }
		void Session_End(object sender, EventArgs e) { }
	       
	</script>

	-------------------------------------------------------------------------------------------------------------------------
	4) web.config
	================================================
	<?xml version="1.0"?>
	<configuration>
		<configSections>
			<section name="Jobs" type="CommonTools.Components.Threading.JobSection, CommonTools.Components" />
		</configSections>

		<Jobs>
			<jobPool minutes="10">
			  <add name="FirstJob"	
				   type="MyJobs.FirstJob, App_Code"	
				   enabled="true" 
				   enableShutDown="false" 
				   firstRunAtInitialization="true">
				<Options>
				  <add name="FirstValue" value="val1" />
				  <add name="SecondValue" value="val2" />
				  <add name="ThirdValue" value="val3" />
				</Options>
			  </add>
			  <add name="SecondJob"	
				   type="MyJobs.SecondJob, App_Code"	
				   enabled="true" 
				   enableShutDown="false" 
				   firstRunAtInitialization="false" />
			  <add name="NoSingleJob"	
				   type="MyJobs.NoSingleJob, App_Code" 
				   enabled="true" 
				   enableShutDown="false" 
				   executeOnOwnThread="true" 
				   seconds="10" />
			 <add name="DailyJob"
				   type="MyJobs.DailyJob, App_Code"
				   enabled="true"
				   enableShutDown="false"
				   executeDaily="true"
				   dailyUTCExecutionTime="11:11:00.000" />
			</jobPool>
		</Jobs>
	</configuration>

	




