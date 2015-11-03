<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DetailView.ascx.cs" Inherits="SplendidCRM.Users.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divMain">
	<script type="text/javascript">
	function PasswordPopup()
	{
		return window.open('Password.aspx','ProjectPopup','width=320,height=230,resizable=1,scrollbars=1');
	}

	function ChangePassword(sOLD_PASSWORD, sNEW_PASSWORD, sCONFIRM_PASSWORD)
	{
		document.getElementById('<%= txtOLD_PASSWORD.ClientID     %>').value = sOLD_PASSWORD    ;
		document.getElementById('<%= txtNEW_PASSWORD.ClientID     %>').value = sNEW_PASSWORD    ;
		document.getElementById('<%= txtCONFIRM_PASSWORD.ClientID %>').value = sCONFIRM_PASSWORD;
		document.forms[0].submit();
	}
	</script>
	<input ID="txtOLD_PASSWORD"     type="hidden" Runat="server" />
	<input ID="txtNEW_PASSWORD"     type="hidden" Runat="server" />
	<input ID="txtCONFIRM_PASSWORD" type="hidden" Runat="server" />

	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Users" EnablePrint="true" EnableHelp="true" Runat="Server" />

	<table width="100%" border="0" cellpadding="0" cellspacing="0" Visible="<%# !PrintView %>" runat="server">
		<tr>
			<td style="padding-bottom: 2px;">
				<asp:Button ID="btnEdit"           CommandName="Edit"           OnCommand="Page_Command"          CssClass="button" Text='<%# "  " + L10n.Term(".LBL_EDIT_BUTTON_LABEL"     ) + "  " %>' title='<%# L10n.Term(".LBL_EDIT_BUTTON_TITLE"                ) %>' AccessKey='<%# L10n.Term(".LBL_EDIT_BUTTON_KEY"                ) %>' Runat="server" />
				<input      ID="btnChangePassword" type="button"                onclick="return PasswordPopup();" class="button"   value='<%# L10n.Term("Users.LBL_CHANGE_PASSWORD_BUTTON_LABEL")    %>' title='<%# L10n.Term("Users.LBL_CHANGE_PASSWORD_BUTTON_TITLE") %>' accessKey='<%# L10n.Term("Users.LBL_CHANGE_PASSWORD_BUTTON_KEY") %>' Runat="server" />
				<asp:Button ID="btnDuplicate"      CommandName="Duplicate"      OnCommand="Page_Command"          CssClass="button" Text='<%# " "  + L10n.Term(".LBL_DUPLICATE_BUTTON_LABEL") + " "  %>' title='<%# L10n.Term(".LBL_DUPLICATE_BUTTON_TITLE"           ) %>' AccessKey='<%# L10n.Term(".LBL_DUPLICATE_BUTTON_KEY"           ) %>' Runat="server" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
			<td align="right">
				<asp:LinkButton ID="btnReset"      CommandName="Users.ResetDefaults" OnCommand="Page_Command" CssClass="listViewTdToolsS1" Runat="server"><%# L10n.Term("Users.LBL_RESET_PREFERENCES") %></asp:LinkButton>
			</td>
		</tr>
	</table>

	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
		<tr>
			<td width="15%" valign="top" class="tabDetailViewDL"><%= L10n.Term("Users.LBL_NAME") %></td>
			<td width="35%" valign="top" class="tabDetailViewDF"><asp:Label ID="txtNAME" Runat="server" /></td>
			<td width="15%" valign="top" class="tabDetailViewDL"><%= L10n.Term("Users.LBL_USER_NAME") %></td>
			<td width="35%" valign="top" class="tabDetailViewDF"><asp:Label ID="txtUSER_NAME" Runat="server" /></td>
		</tr>
		<tr>
			<td valign="top" class="tabDetailViewDL"><%= L10n.Term("Users.LBL_STATUS") %></td>
			<td valign="top" class="tabDetailViewDF"><asp:Label ID="txtSTATUS" Runat="server" /></td>
			<td valign="top" class="tabDetailViewDL">&nbsp;</td>
			<td valign="top" class="tabDetailViewDF">&nbsp;</td>
		</tr>
	</table>
	</p>
</div>

<div id="divUserSettings">
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
		<tr>
			<th valign="top" align="left" colspan="3" class="tabDetailViewDL"><h4 class="tabDetailViewDL"><%= L10n.Term("Users.LBL_USER_SETTINGS") %></h4></th>
		</tr>
		<tr>
			<td width="20%" class="tabDetailViewDL"><%= L10n.Term("Users.LBL_ADMIN") %>&nbsp;</td>
			<td width="15%" class="tabDetailViewDF"><asp:CheckBox ID="chkIS_ADMIN" Enabled="false" CssClass="checkbox" Runat="server" />&nbsp;</td>
			<td width="65%" class="tabDetailViewDF"><%= L10n.Term("Users.LBL_ADMIN_TEXT") %>&nbsp;</td>
		</tr>
		<tr></tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_PORTAL_ONLY") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:CheckBox ID="chkPORTAL_ONLY" Enabled="false" CssClass="checkbox" Runat="server" />&nbsp;</td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_PORTAL_ONLY_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:CheckBox ID="chkRECEIVE_NOTIFICATIONS" Enabled="false" CssClass="checkbox" Runat="server" />&nbsp;</td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_LANGUAGE") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:Label ID="txtLANGUAGE" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_LANGUAGE_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_DATE_FORMAT") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:Label ID="txtDATEFORMAT" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_DATE_FORMAT_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_TIME_FORMAT") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:Label ID="txtTIMEFORMAT" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_TIME_FORMAT_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_TIMEZONE") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:Label ID="txtTIMEZONE" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_ZONE_TEXT") %>&nbsp;</td>
		</tr>
		<!-- 08/05/2006 Paul.  Remove stub of unsupported code. Show Gridline is not supported at this time. 
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_GRIDLINE") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:CheckBox ID="chkGRIDLINE" Enabled="false" CssClass="checkbox" Runat="server" />&nbsp;</td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_GRIDLINE_TEXT") %>&nbsp;</td>
		</tr>
		-->
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_CURRENCY") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:Label ID="txtCURRENCY" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_CURRENCY_TEXT") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_NUMBER_GROUPING_SEP") %></td>
			<td class="tabDetailViewDF"><asp:Label ID="txtGROUP_SEPARATOR" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_NUMBER_GROUPING_SEP_TEXT")%></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_DECIMAL_SEP")%></td>
			<td class="tabDetailViewDF"><asp:Label ID="txtDECIMAL_SEPARATOR" Runat="server" /></td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_DECIMAL_SEP_TEXT")%></td>
		</tr>
		<!-- 08/05/2006 Paul.  Remove stub of unsupported code. Reminder is not supported at this time. 
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_REMINDER") %>&nbsp;</td>
			<td class="tabDetailViewDF"><asp:CheckBox ID="chkREMINDER" Enabled="false" CssClass="checkbox" Runat="server" />&nbsp;</td>
			<td class="tabDetailViewDF"><%= L10n.Term("Users.LBL_REMINDER_TEXT") %>&nbsp;</td>
		</tr>
		-->
	</table>
	</p>
</div>

<div id="divDetailView">
	<p>
	<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView" runat="server">
		<tr>
			<th valign="top" align="left" colspan="4" class="tabDetailViewDL"><h4 class="tabDetailViewDL"><%= L10n.Term("Users.LBL_USER_INFORMATION") %></h4></th>
		</tr>
	</table>
	</p>
</div>

<div id="divMailOptions">
	<p>
	<table ID="tblMailOptions" width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView" runat="server">
		<tr>
			<th valign="top" align="left" colspan="4" class="tabDetailViewDL"><h4 class="tabDetailViewDL"><%= L10n.Term("Users.LBL_MAIL_OPTIONS_TITLE") %></h4></th>
		</tr>
	</table>
	</p>
</div>

<!-- 08/05/2006 Paul.  Remove stub of unsupported code. Calendar Publish Key is not supported at this time. 
<div id="divCalendarOptions">
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
		<tr>
			<th valign="top" align="left" colspan="3" class="tabDetailViewDL"><h4 class="tabDetailViewDL"><%= L10n.Term("Users.LBL_CALENDAR_OPTIONS") %></h4></th>
		</tr>
		<tr>
			<td width="20%" class="tabDetailViewDL"><%= L10n.Term("Users.LBL_PUBLISH_KEY") %>&nbsp;</td>
			<td width="15%" class="tabDetailViewDF"><asp:Label ID="txtCALENDAR_PUBLISH_KEY" Runat="server" /></td>
			<td width="65%" class="tabDetailViewDF"><%= L10n.Term("Users.LBL_CHOOSE_A_KEY") %>&nbsp;</td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_YOUR_PUBLISH_URL") %>&nbsp;</td>
			<td colspan="2" class="tabDetailViewDF"><asp:Label ID="txtCALENDAR_PUBLISH_URL" Runat="server" /></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL"><%= L10n.Term("Users.LBL_SEARCH_URL") %>&nbsp;</td>
			<td colspan="2" class="tabDetailViewDF"><asp:Label ID="txtCALENDAR_SEARCH_URL" Runat="server" /></td>
		</tr>
	</table>
	</p>
</div>
-->

<div id="divAccessRights">
	<%@ Register TagPrefix="SplendidCRM" Tagname="AccessView" Src="~/Administration/ACLRoles/AccessView.ascx" %>
	<SplendidCRM:AccessView ID="ctlAccessView" EnableACLEditing="false" Runat="Server" />
</div>

<div id="divDetailSubPanel">
	<%@ Register TagPrefix="SplendidCRM" Tagname="Roles" Src="Roles.ascx" %>
	<SplendidCRM:Roles ID="ctlRoles" Runat="Server" />
</div>

<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
