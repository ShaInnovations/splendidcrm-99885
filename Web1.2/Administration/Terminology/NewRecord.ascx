<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Administration.Terminology.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divNewRecord">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="leftColumnModuleHead">
		<tr>
			<th width="5"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_left.gif" %>'  BorderWidth="0" Width="5" Height="21" Runat="server" /></th>
			<th style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/moduleTab_middle.gif);" width="100%"><%= L10n.Term("Terminology.LBL_NEW_FORM_TITLE") %></th>
			<th width="9"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_right.gif" %>' BorderWidth="0" Width="9" Height="21" Runat="server" /></th>
		</tr>
	</table>
	<table border="0" cellpadding="3" cellspacing="0" width="180">
		<tr>
			<td align="left" class="leftColumnModuleS3">
				<p>
				<%= L10n.Term("Terminology.LBL_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br>
				<asp:TextBox ID="txtNAME" Runat="server" /><br>
				<%= L10n.Term("Terminology.LBL_LANG") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br>
				<asp:DropDownList ID="lstLANGUAGE" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /><br>
				<%= L10n.Term("Terminology.LBL_MODULE_NAME") %><br>
				<asp:DropDownList ID="lstMODULE_NAME" DataValueField="MODULE_NAME" DataTextField="MODULE_NAME" Runat="server" /><br>
				<%= L10n.Term("Terminology.LBL_LIST_NAME_LABEL") %><br>
				<asp:DropDownList ID="lstLIST_NAME" DataValueField="LIST_NAME" DataTextField="LIST_NAME" Runat="server" /><br>
				<%= L10n.Term("Terminology.LBL_LIST_ORDER") %><br>
				<asp:TextBox ID="txtLIST_ORDER" Runat="server" /><br>
				<%= L10n.Term("Terminology.LBL_DISPLAY_NAME") %><br>
				<asp:TextBox ID="txtDISPLAY_NAME" Runat="server" /><br>
				</p>
				<p><asp:Button ID="btnSave" CommandName="NewRecord" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /></p>
				<asp:RequiredFieldValidator ID="reqNAME"         ControlToValidate="txtNAME" ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID, btnSave.ClientID) %>
</div>
<script type="text/javascript">
<%
// 01/12/2006 Paul.  NewRecord is having a problem setting the value in the code-behind. 
if ( !IsPostBack )
	Response.Write("SelectOption('" + lstLANGUAGE.ClientID + "','" + L10N.NormalizeCulture(L10n.NAME) + "');");
%>
</script>
