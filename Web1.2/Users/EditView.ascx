<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Users.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomValidators" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
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
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Users" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Users.LBL_FIRST_NAME") %></td>
						<td width="35%" class="dataField"><asp:TextBox ID="txtFIRST_NAME" TabIndex="1" MaxLength="30" size="25" Runat="server" /></td>
						<td width="15%" class="dataLabel"><%= L10n.Term("Users.LBL_USER_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="35%" class="dataField">
							<asp:TextBox ID="txtUSER_NAME"  TabIndex="2" MaxLength="30" size="20" Runat="server" />
							<asp:RequiredFieldValidator ID="reqUSER_NAME" ControlToValidate="txtUSER_NAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_LAST_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtLAST_NAME" TabIndex="1" MaxLength="30" size="25" Runat="server" />
							<asp:RequiredFieldValidator ID="reqLAST_NAME" ControlToValidate="txtLAST_NAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
						<td class="dataLabel">&nbsp;</td>
						<td class="dataField">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_STATUS") %></td>
						<td class="dataField"><asp:DropDownList ID="lstSTATUS" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
						<td class="dataLabel">&nbsp;</td>
						<td class="dataField">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
	<p>
	<script type="text/javascript">
	function ChangeUser(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "REPORTS_TO_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "REPORTS_TO_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function UserPopup()
	{
		return window.open('../Users/Popup.aspx','UserPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	</script>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th align="left" class="dataField" colspan="3"><h4 class="dataLabel"><%= L10n.Term("Users.LBL_USER_SETTINGS") %></h4></th>
					</tr>
					<tr>
						<td width="20%" class="dataLabel"><%= L10n.Term("Users.LBL_ADMIN") %></td>
						<td width="15%" class="dataField"><asp:CheckBox ID="chkIS_ADMIN" TabIndex="3" CssClass="checkbox" Runat="server" /></td>
						<td width="65%" class="dataField"><%= L10n.Term("Users.LBL_ADMIN_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_PORTAL_ONLY") %></td>
						<td class="dataField"><asp:CheckBox ID="chkPORTAL_ONLY" TabIndex="3" CssClass="checkbox" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_PORTAL_ONLY_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS") %></td>
						<td class="dataField"><asp:CheckBox ID="chkRECEIVE_NOTIFICATIONS" TabIndex="3" CssClass="checkbox" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_THEME") %></td>
						<td class="dataField"><asp:DropDownList ID="lstTHEME" DataValueField="NAME" DataTextField="NAME" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_THEME_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_LANGUAGE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstLANGUAGE" DataValueField="NAME" DataTextField="NATIVE_NAME" OnSelectedIndexChanged="lstLANGUAGE_Changed" AutoPostBack="true" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_LANGUAGE_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_DATE_FORMAT") %></td>
						<td class="dataField"><asp:DropDownList ID="lstDATE_FORMAT" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_DATE_FORMAT_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_TIME_FORMAT") %></td>
						<td class="dataField"><asp:DropDownList ID="lstTIME_FORMAT" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_TIME_FORMAT_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_TIMEZONE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstTIMEZONE" DataValueField="ID" DataTextField="NAME" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_TIMEZONE_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_GRIDLINE") %></td>
						<td class="dataField"><asp:CheckBox ID="chkGRIDLINE" TabIndex="3" CssClass="checkbox" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_GRIDLINE_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_CURRENCY") %></td>
						<td class="dataField"><asp:DropDownList ID="lstCURRENCY" DataValueField="ID" DataTextField="NAME_SYMBOL" TabIndex="3" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_CURRENCY_TEXT") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_NUMBER_GROUPING_SEP") %></td>
						<td class="dataField"><asp:TextBox ID="txtGROUP_SEPARATOR" TabIndex="3" MaxLength="1" size="1" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_NUMBER_GROUPING_SEP_TEXT")%></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_DECIMAL_SEP")%></td>
						<td class="dataField"><asp:TextBox ID="txtDECIMAL_SEPARATOR" TabIndex="3" MaxLength="1" size="1" Runat="server" /></td>
						<td class="dataField"><%= L10n.Term("Users.LBL_DECIMAL_SEP_TEXT")%></td>
					</tr>
					<!-- 08/05/2006 Paul.  Remove stub of unsupported code. Reminder is not supported at this time. 
					<tr>
						<td class="dataLabel"><%= L10n.Term("Users.LBL_REMINDER") %></td>
						<td class="dataField">
							<asp:CheckBox ID="chkSHOULD_REMIND" TabIndex="3" CssClass="checkbox" Runat="server" />
							<asp:DropDownList ID="lstREMINDER_TIME" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="3" Runat="server" />
						</td>
						<td class="dataField"><%= L10n.Term("Users.LBL_REMINDER_TEXT") %></td>
					</tr>
					-->
				</table>
			</td>
		</tr>
	</table>
	</p>
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataField" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Users.LBL_USER_SETTINGS") %></h4></th>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table ID="tblAddress" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataLabel" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Users.LBL_ADDRESS_INFORMATION") %></h4></th>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table ID="tblMailOptions" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataField" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Users.LBL_MAIL_OPTIONS_TITLE") %></h4></th>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
	<!-- 08/05/2006 Paul.  Remove stub of unsupported code. Calendar Publish Key is not supported at this time. 
	<p>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th align="left" class="dataField" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Users.LBL_CALENDAR_OPTIONS") %></h4></th>
					</tr>
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Users.LBL_PUBLISH_KEY") %></td>
						<td width="20%" class="dataField"><asp:TextBox ID="txtCALENDAR_PUBLISH_KEY" TabIndex="10" MaxLength="25" size="25" Runat="server" /></td>
						<td width="65%" class="dataField"><%= L10n.Term("Users.LBL_CHOOSE_A_KEY") %></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
	-->
</div>
