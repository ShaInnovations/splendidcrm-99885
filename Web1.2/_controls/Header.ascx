<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Header.ascx.cs" Inherits="SplendidCRM._controls.Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
 * The Original Code is: SugarCRM Open Source
 * The Initial Developer of the Original Code is SugarCRM, Inc.
 * Portions created by SugarCRM are Copyright (C) 2004-2005 SugarCRM, Inc. All Rights Reserved.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<div id="divHeader">
	<div style="<%= Sql.IsEmptyString(Application["CONFIG.header_background"]) ? String.Empty : "background-image: url(" + Sql.ToString(Session["themeURL"]) + Sql.ToString(Application["CONFIG.header_background"]) + ");" %>">
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr height="60">
				<td><asp:Image ID="imgCompanyLogo" BorderWidth="0" Runat="server" /></td>
				<td align="right" valign="top" nowrap class="myArea" style="padding-right: 10px;">
					<asp:HyperLink ID="lnkMyAccount" Text='<%# L10n.Term(".LBL_MY_ACCOUNT") %>' NavigateUrl="~/Users/MyAccount.aspx" CssClass="myAreaLink" Runat="server" />
					&nbsp;|&nbsp;
					<asp:HyperLink ID="lnkEmployees" Text='<%# L10n.Term(".LBL_EMPLOYEES") %>' NavigateUrl="~/Employees/default.aspx" CssClass="myAreaLink" Runat="server" />
					&nbsp;|&nbsp;
<%
if ( Security.IS_ADMIN )
	{
	%>
					<asp:HyperLink ID="lnkAdmin" Text='<%# L10n.Term(".LBL_ADMIN") %>' NavigateUrl="~/Administration/default.aspx" CssClass="myAreaLink" Runat="server" />
					&nbsp;|&nbsp;
	<%
	}
if ( !Security.IsWindowsAuthentication() )
	{
	%>
					<asp:HyperLink ID="lnkLogout" Text='<%# L10n.Term(".LBL_LOGOUT") %>' NavigateUrl="~/Users/Logout.aspx" CssClass="myAreaLink" Runat="server" />
					&nbsp;|&nbsp;
	<%
	}
%>
					<asp:HyperLink ID="lnkAbout" Text='<%# L10n.Term(".LNK_ABOUT") %>' NavigateUrl="~/Home/About.aspx" CssClass="myAreaLink" Runat="server" />
					<br>
<%= Application["CONFIG.platform_title"] %>
				</td>
			</tr>
		</table>
		<%@ Register TagPrefix="SplendidCRM" Tagname="TabMenu" Src="~/_controls/TabMenu.ascx" %>
		<SplendidCRM:TabMenu ID="ctlTabMenu" Runat="Server" />
	</div>
	<table width="100%" cellspacing="0" cellpadding="0" border="0">
		<tr height="20">
			<td class="subTabBar" colspan="2">
				<table width="100%" cellspacing="0" cellpadding="0" border="0" height="20">
					<tr>
						<td class="welcome" width="100%"><%= L10n.Term(".NTC_WELCOME") + " " + SplendidCRM.Security.USER_NAME %></td>
						<td ID="tdUnifiedSearch1" class="search" style="padding: 0px" align="right" width="11" runat="server"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/searchSeparator.gif" %>' Width="12" Height="20" BorderWidth="0" AlternateText='<%# L10n.Term(".LBL_SEARCH") %>' Runat="server" /></td>
						<td ID="tdUnifiedSearch2" class="search" style="padding: 0px" align="right" runat="server">&nbsp;<b><%= L10n.Term(".LBL_SEARCH") %></b></td>
						<td ID="tdUnifiedSearch3" class="search" nowrap runat="server">
							<div id="divUnifiedSearch">
								<script type="text/javascript">
								function UnifiedSearch()
								{
									var frm = document.forms[0];
									var sUrl = '<%= Application["rootURL"] %>Home/UnifiedSearch.aspx?txtUnifiedSearch=' + escape(frm['txtUnifiedSearch'].value);
									window.location.href = sUrl;
									return false;
								}
								</script>
								&nbsp;<input ID="txtUnifiedSearch" type="text" class="searchField" name="txtUnifiedSearch" size="14" value="<%= Server.HtmlEncode(Request["txtUnifiedSearch"]) %>" />
								&nbsp;<input ID="btnUnifiedSearch" type="image" onclick="return UnifiedSearch();" class="searchButton" width="30" height="17" src="<%= Session["themeURL"] %>images/searchButton.gif" value="<%= L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>" align="absmiddle" alt="<%= L10n.Term(".LBL_SEARCH") %>" />
<%= Utils.RegisterEnterKeyPress("txtUnifiedSearch", "btnUnifiedSearch") %>
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<table width="100%" cellspacing="0" cellpadding="0" border="0">
		<tr height="20">
			<td class="lastView">
				<%@ Register TagPrefix="SplendidCRM" Tagname="LastViewed" Src="~/_controls/LastViewed.ascx" %>
				<SplendidCRM:LastViewed ID="ctlLastViewed" Runat="Server" />
			</td>
		</tr>
		<tr height="10">
			<td></td>
		</tr>
	</table>
</div>
<table width="100%" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td ID="tdShortcuts" style="padding-left: 10px; padding-right: 10px; vertical-align: top;" width="10%" Runat="server">
				<%@ Register TagPrefix="SplendidCRM" Tagname="Shortcuts" Src="~/_controls/Shortcuts.ascx" %>
				<SplendidCRM:Shortcuts ID="ctlShortcuts" Runat="Server" />
				<asp:PlaceHolder ID="plcNewRecord" Runat="server" />
		</td>
		<td style="padding-right: 10px; vertical-align: top;">
