﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="CommonTools.Web.UI.NavigationBreadcrumbsSkin" %>
<a class="<%# (this.IsLastNode ? "BreadCrumbActive" : "BreadCrumbLink") %>" href="<%# ResolveUrl(this.MenuItem.UrlRewriteItem.FullVirtualPath) %>"><%# this.MenuItem.BreadcrumbTitle %></a>