<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Contacts.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
			<th style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/moduleTab_middle.gif);" width="100%"><%= L10n.Term("Contacts.LBL_NEW_FORM_TITLE") %></th>
			<th width="9"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_right.gif" %>' BorderWidth="0" Width="9" Height="21" Runat="server" /></th>
		</tr>
	</table>
	<table width="100%" cellpadding="3" cellspacing="0" border="0">
		<tr>
			<td align="left" class="leftColumnModuleS3">
				<p>
				<%= L10n.Term("Contacts.LBL_FIRST_NAME") %><br>
				<asp:TextBox ID="txtFIRST_NAME" MaxLength="25" Runat="server" /><br>
				<%= L10n.Term("Contacts.LBL_LAST_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br>
				<asp:TextBox ID="txtLAST_NAME" MaxLength="25" Runat="server" /><br>
				<%= L10n.Term("Contacts.LBL_PHONE") %><br>
				<asp:TextBox ID="txtPHONE_WORK" MaxLength="25" Runat="server" /><br>
				<%= L10n.Term("Contacts.LBL_EMAIL_ADDRESS") %><br>
				<asp:TextBox ID="txtEMAIL1" MaxLength="100" Runat="server" />
				</p>
				<p><asp:Button ID="btnSave" CommandName="NewRecord" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /></p>
				<asp:RequiredFieldValidator ID="reqLAST_NAME" ControlToValidate="txtLAST_NAME" ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
				<!--
				http://www.regexlib.com/
				Expression :  ^(?:(?:[\+]?(?<CountryCode>[\d]{1,3}(?:[ ]+|[\-.])))?[(]?(?<AreaCode>[\d]{3})[\-/)]?(?:[ ]+)?)?(?<Number>[a-zA-Z2-9][a-zA-Z0-9 \-.]{6,})(?:(?:[ ]+|[xX]|(i:ext[\.]?)){1,2}(?<Ext>[\d]{1,5}))?$
				Description:  This allows the formatting of most phone numbers.
				Matches    :  [1-800-DISCOVER], [(610) 310-5555 x5555], [533-1123]
				Non-Matches:  [1 533-1123], [553334], [66/12343]
				-->
				<asp:RegularExpressionValidator ID="reqPHONE_WORK" ControlToValidate="txtPHONE_WORK" ErrorMessage="(invalid phone)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" 
					ValidationExpression="^(?:(?:[\+]?(?<CountryCode>[\d]{1,3}(?:[ ]+|[\-.])))?[(]?(?<AreaCode>[\d]{3})[\-/)]?(?:[ ]+)?)?(?<Number>[a-zA-Z2-9][a-zA-Z0-9 \-.]{6,})(?:(?:[ ]+|[xX]|(i:ext[\.]?)){1,2}(?<Ext>[\d]{1,5}))?$" />
				<!--
				http://www.regexlib.com/
				Expression :  ^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$
				Description:  The most complete email validation routine I could come up with. It verifies that: - Only letters, numbers and email acceptable symbols (+, _, -, .) are allowed - No two different symbols may follow each other - Cannot begin with a symbol - Ending domain ...
				Matches    :  [g_s+gav@com.com], [gav@gav.com], [jim@jim.c.dc.ca]
				Non-Matches:  [gs_.gs@com.com], [gav@gav.c], [jim@--c.ca]
				-->
				<asp:RegularExpressionValidator ID="reqEMAIL1" ControlToValidate="txtEMAIL1" ErrorMessage="(invalid email)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" 
					ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtFIRST_NAME.ClientID, btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtLAST_NAME.ClientID , btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtPHONE_WORK.ClientID, btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtEMAIL1.ClientID    , btnSave.ClientID) %>
</div>
