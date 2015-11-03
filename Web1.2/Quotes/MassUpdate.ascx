<%@ Control Language="c#" AutoEventWireup="false" Codebehind="MassUpdate.ascx.cs" Inherits="SplendidCRM.Quotes.MassUpdate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="SplendidCRM" %>
<%@ Register TagPrefix="SplendidCRM" Tagname="DatePicker" Src="~/_controls/DatePicker.ascx" %>
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
<div id="divMassUpdate">
	<br>
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title=".LBL_MASS_UPDATE_TITLE" Runat="Server" />
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td style="padding-bottom: 2px;">
				<asp:Button ID="btnUpdate" CommandName="MassUpdate" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_UPDATE") %>' Runat="server" />
				<asp:Button ID="btnDelete" CommandName="MassDelete" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_DELETE") %>' Runat="server" />
			</td>
		</tr>
	</table>
	<script type="text/javascript">
	var ChangeAccount = null;
	var ChangeContact = null;

	function ChangeShippingAccount(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtSHIPPING_ACCOUNT_ID.ClientID   %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtSHIPPING_ACCOUNT_NAME.ClientID %>').value = sPARENT_NAME;
	}
	function ChangeBillingAccount(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtBILLING_ACCOUNT_ID.ClientID   %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtBILLING_ACCOUNT_NAME.ClientID %>').value = sPARENT_NAME;
	}
	function ChangeShippingContact(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtSHIPPING_CONTACT_ID.ClientID   %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtSHIPPING_CONTACT_NAME.ClientID %>').value = sPARENT_NAME;
	}
	function ChangeBillingContact(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtBILLING_CONTACT_ID.ClientID   %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtBILLING_CONTACT_NAME.ClientID %>').value = sPARENT_NAME;
	}
	function AccountPopup()
	{
		return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ContactPopup()
	{
		return window.open('../Contacts/Popup.aspx','ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeUser(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtASSIGNED_USER_ID.ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtASSIGNED_TO.ClientID      %>').value = sPARENT_NAME;
		document.forms[0].submit();
	}
	function UserPopup()
	{
		return window.open('../Users/Popup.aspx?ClearDisabled=1','UserPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	</script>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="tabForm">
		<tr>
			<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></td>
						<td width="35%" class="dataField">
							<asp:TextBox ID="txtASSIGNED_TO" ReadOnly="True" Runat="server" />
							<input ID="txtASSIGNED_USER_ID" type="hidden" runat="server" NAME="Hidden1"/>
							<input ID="btnChangeUser" type="button" class="button" onclick="return UserPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
						<td width="15%" class="dataLabel">&nbsp;</td>
						<td width="35%" class="dataField">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_DATE_VALID_UNTIL") %></td>
						<td class="dataField"><SplendidCRM:DatePicker ID="ctlDATE_QUOTE_EXPECTED_CLOSED" Runat="Server" /></td>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_ORIGINAL_PO_DATE") %></td>
						<td class="dataField"><SplendidCRM:DatePicker ID="ctlORIGINAL_PO_DATE" Runat="Server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_PAYMENT_TERMS") %></td>
						<td class="dataField"><asp:DropDownList ID="lstPAYMENT_TERMS" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_QUOTE_STAGE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstQUOTE_STAGE" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_SHIPPING_ACCOUNT_NAME") %></td>
						<td class="dataField">
							<asp:TextBox ID="txtSHIPPING_ACCOUNT_NAME" ReadOnly="True" Runat="server" />
							<input ID="txtSHIPPING_ACCOUNT_ID" type="hidden" runat="server" />
							<input ID="btnChangeShippingAccount" type="button" class="button" onclick="ChangeAccount=ChangeShippingAccount;return AccountPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_SHIPPING_CONTACT_NAME") %></td>
						<td class="dataField">
							<asp:TextBox ID="txtSHIPPING_CONTACT_NAME" ReadOnly="True" Runat="server" />
							<input ID="txtSHIPPING_CONTACT_ID" type="hidden" runat="server" NAME="Hidden1"/>
							<input ID="btnChangeShippingContact" type="button" class="button" onclick="ChangeContact=ChangeShippingContact;return ContactPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_BILLING_ACCOUNT_NAME") %></td>
						<td class="dataField">
							<asp:TextBox ID="txtBILLING_ACCOUNT_NAME" ReadOnly="True" Runat="server" />
							<input ID="txtBILLING_ACCOUNT_ID" type="hidden" runat="server" NAME="txtBILLING_ACCOUNT_ID"/>
							<input ID="btnChangeBillingAccount" type="button" class="button" onclick="ChangeAccount=ChangeBillingAccount;return AccountPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
						<td class="dataLabel"><%= L10n.Term("Quotes.LBL_BILLING_CONTACT_NAME") %></td>
						<td class="dataField">
							<asp:TextBox ID="txtBILLING_CONTACT_NAME" ReadOnly="True" Runat="server" />
							<input ID="txtBILLING_CONTACT_ID" type="hidden" runat="server" NAME="Hidden1"/>
							<input ID="btnChangeBillingContact" type="button" class="button" onclick="ChangeContact=ChangeBillingContact;return ContactPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
