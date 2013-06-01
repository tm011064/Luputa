<%@ Page Language="C#" MasterPageFile="~/Layout/Rack.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="TGC" Namespace="CommonTools.Web.UI" Assembly="CommonTools" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="TestPanel">
        <h1>
            Url Rewriting</h1>
        <a href="UrlRewriteTest/MyTestQuerystringParameter/default.aspx">Url Rewrite test</a></div>
    <div class="TestPanel">
        <h1>
            Cache View</h1>
        <TGC:CacheView ID="cv1" runat="server" Theme="Default" Font-Names="Tahoma" Font-Size="10px"
            Width="700px" />
    </div>
    <div class="TestPanel">
        <h1>
            Exception Log</h1>
        <TGC:ExceptionLogView ID="exlv" runat="server" UseDefaultStyling="true" Width="700px"
            ApplicationLocationEnumType="CommonTools.TestApp.Components.ApplicationLocation, App_Code" />
    </div>
    <div class="TestPanel">
        <h1>
            Event Log</h1>
        <TGC:EventLogView ID="elv" runat="server" UseDefaultStyling="true" Width="700px"
            EventTypesEnumType="CommonTools.TestApp.Components.EventType, App_Code" ApplicationLocationEnumType="CommonTools.TestApp.Components.ApplicationLocation, App_Code" />
    </div>
    <%--<asp:Button ID="btnPostback" runat="server" UseSubmitBehavior="true" Text="Postback"
            OnClick="btnPostback_Click" /><br />
        <br />
        Pagesize:
        <asp:TextBox ID="txtPageSize" runat="server">50</asp:TextBox>, Pageindex:
        <asp:TextBox ID="txtPageIndex" runat="server">0</asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button
            ID="btnLoadGridview" runat="server" Text="Populate grid" OnClick="btnLoadGridview_Click" />
        <br />
        <asp:GridView ID="grvUserPage" runat="server" AutoGenerateColumns="true">
        </asp:GridView>
        <br />
        <asp:TextBox ID="txtAmount" runat="server">100</asp:TextBox>
        <asp:Button ID="btnCreate" runat="server" Text="Create unique users" OnClick="btnCreateUnique_Click" />
        <asp:Button ID="btnIncrement" runat="server" Text="Create incrementing users" OnClick="btnCreateIncrementing_Click" /><br />
        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
        <br />
        <br />
        <asp:Button ID="btnWriteXML" runat="server" Text="WriteXML" OnClick="btnWriteXML_Click" /><br />--%>
</asp:Content>
