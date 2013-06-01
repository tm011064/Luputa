<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="customtests_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script type="text/javascript" src="swfobject.js"></script>



</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
swfobject.embedSWF("open-flash-chart.swf"
                   , "my_chart"
                   , "550"
                   , "500"
                   , "9.0.0"
                   , "expressInstall.swf"
                   , { "data-file" : "<%= (string.IsNullOrEmpty(Request.QueryString["f"]) ? "OpenFlashChartJSON.ashx" : Request.QueryString["f"])     + "?lala=" + Guid.NewGuid().ToString() %>" } );
    </script>
    <div id="my_chart"></div>
    </form>
</body>
</html>
