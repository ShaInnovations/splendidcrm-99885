<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.Administration.ProductCategories.Default" %>
<%@ Import Namespace="SplendidCRM" %>
<%
/**********************************************************************************************************************
 * The contents of this file are subject to the SugarCRM Public License Version 1.1.3 ("License"); You may not use this
 * file except in compliance with the License. You may obtain a copy of the License at http://www.sugarcrm.com/SPL
 * Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 * express or implied.  See the License for the specific language governing rights and limitations under the License.
 *
 * All copies of the Covered Code must include on each user interface screen:
 *    (i) the "Powered by SugarCRM" logo and
 *    (ii) the SugarCRM copyright notice
 *    (iii) the SplendidCRM copyright notice
 * in the same form as they appear in the distribution.  See full license for requirements.
 *
 * The Original Code is: SplendidCRM Open Source
 * The Initial Developer of the Original Code is SplendidCRM Software, Inc.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" >
<html>
<head>
	<title><asp:Literal id="litPageTitle" runat="server" /></title>
	<%@ Register TagPrefix="SplendidCRM" Tagname="MetaHeader" Src="~/_controls/MetaHeader.ascx" %>
	<SplendidCRM:MetaHeader ID="ctlMetaHeader" Runat="Server" />
</head>
<body>
<form id="frmMain" method="post" runat="server">
	<%@ Register TagPrefix="SplendidCRM" Tagname="Header" Src="~/_controls/Header.ascx" %>
	<SplendidCRM:Header ID="ctlHeader" Visible="<%# !PrintView %>" ActiveTab="ProductCategories" AdminShortcuts="true" Runat="Server" />
	
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="ProductCategories" Title="ProductCategories.LBL_MODULE_NAME" EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" Runat="Server" />
	
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListView" Src="ListView.ascx" %>
	<SplendidCRM:ListView ID="ctlListView" Visible='<%# SplendidCRM.Security.IS_ADMIN %>' Runat="Server" />
	<asp:Label Text='<%# L10n.Term(".LBL_UNAUTH_ADMIN") %>' Visible='<%# !SplendidCRM.Security.IS_ADMIN %>' Runat="Server" />
	
	<%@ Register TagPrefix="SplendidCRM" Tagname="Footer" Src="~/_controls/Footer.ascx" %>
	<SplendidCRM:Footer ID="ctlFooter" Visible="<%# !PrintView %>" Runat="Server" />
</form>
</body>
</html>
