<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        CommonTools.Components.Logging.LogManager.LogEvent(0, 0, "Application started at " + DateTime.UtcNow.ToString());
        CommonTools.Components.Threading.Jobs.Instance().Start();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        CommonTools.Components.Threading.Jobs.Instance().Stop();
        CommonTools.Components.Logging.LogManager.LogEvent(0, 0, "Application ended at " + DateTime.UtcNow.ToString());
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        CommonTools.Components.Logging.LogManager.LogEvent(0, 0, "An application error occurred at " + DateTime.Now.ToString());    
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
