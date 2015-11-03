<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Administration.Currencies.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divEditView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader ID="ctlListHeader" Title="Currencies.LBL_CURRENCY" Runat="Server" />
	<p>
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td align="left" style="padding-bottom: 2px;">
				<asp:Button ID="btnSave"  CommandName="Save"  OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL" ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE" ) %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY" ) %>' Runat="server" />
				<asp:Button ID="btnClear" CommandName="Clear" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_CLEAR_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_CLEAR_BUTTON_KEY") %>' Runat="server" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
			<td align="right" nowrap><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /> <%= L10n.Term(".NTC_REQUIRED") %></td>
		</tr>
	</table>
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Currencies.LBL_LIST_NAME") + ":" %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="35%" class="dataField">
							<asp:TextBox ID="txtNAME" TabIndex="1" size="30" MaxLength="36" Runat="server" />
							<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
						<td width="15%" class="dataLabel"><%= L10n.Term("Currencies.LBL_LIST_ISO4217") + ":" %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="35%" class="dataField">
							<asp:TextBox ID="txtISO4217" TabIndex="1" size="3" MaxLength="3" Runat="server" />
							<asp:RequiredFieldValidator ID="reqISO4217" ControlToValidate="txtISO4217" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Currencies.LBL_LIST_RATE") + ":" %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtCONVERSION_RATE" TabIndex="1" size="30" MaxLength="20" Runat="server" />
							<asp:RequiredFieldValidator ID="reqCONVERSION_RATE" ControlToValidate="txtCONVERSION_RATE" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
							<!--
							http://www.regexlib.com/
							Expression :  (^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)
							Description:  This matches all positive decimal values. There was one here already which claimed to but would fail on value 0.00 which is positive AFAIK...  
							Matches    :  [0.00], [1.23], [4.56]
							Non-Matches:  [-1.03], [-0.01], [-0.00]
							-->
							<asp:RegularExpressionValidator ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)" ControlToValidate="txtCONVERSION_RATE" ErrorMessage='<%# L10n.Term(".bug_resolution_dom.Invalid") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
						<td class="dataLabel"><%= L10n.Term("Currencies.LBL_LIST_SYMBOL") + ":" %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtSYMBOL" TabIndex="1" size="3" MaxLength="36" Runat="server" />
							<asp:RequiredFieldValidator ID="reqSYMBOL" ControlToValidate="txtSYMBOL" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Currencies.LBL_LIST_STATUS") + ":" %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:DropDownList ID="lstSTATUS" TabIndex="1" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" />
						</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
