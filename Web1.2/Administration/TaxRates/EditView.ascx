<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Administration.TaxRates.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<SplendidCRM:ListHeader ID="ctlListHeader" Title="TaxRates.LBL_NAME" Runat="Server" />
	<p>
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td align="left" style="padding-bottom: 2px;">
				<asp:Button ID="btnSave"    CommandName="Save"    OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"    ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE"    ) %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY"    ) %>' Runat="server" />
				<asp:Button ID="btnSaveNew" CommandName="SaveNew" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_NEW_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_SAVE_NEW_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SAVE_NEW_BUTTON_KEY") %>' Runat="server" />
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
						<td width="15%" class="dataLabel" valign="top" nowrap><%= L10n.Term("TaxRates.LBL_NAME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="85%" class="dataField">
							<asp:TextBox ID="txtNAME" TabIndex="1" size="60" MaxLength="50" Runat="server" />
							<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel" valign="top"><%= L10n.Term("TaxRates.LBL_VALUE") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtVALUE" TabIndex="2" size="10" MaxLength="10" Runat="server" />
							<asp:RequiredFieldValidator ID="reqVALUE" ControlToValidate="txtVALUE" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Display="Dynamic" Runat="server" />&nbsp;
						</td>
					</tr>
					<tr>
						<td class="dataLabel" valign="top"><%= L10n.Term("TaxRates.LBL_STATUS") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:DropDownList ID="lstSTATUS" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" />
							<em><%= L10n.Term("TaxRates.LBL_STATUS_DESC") %></em>
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("TaxRates.LBL_ORDER") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtLIST_ORDER" TabIndex="3" size="5" MaxLength="10" Runat="server" />
							<asp:RequiredFieldValidator ID="reqLIST_ORDER" ControlToValidate="txtLIST_ORDER" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Display="Dynamic" Runat="server" />&nbsp;
							<em><%= L10n.Term("TaxRates.LBL_ORDER_DESC") %></em>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
