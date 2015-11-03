<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Quotes.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<script type="text/javascript">
var ChangeAccount = null;
var ChangeContact = null;
function ChangeOpportunity(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_NAME").ClientID %>').value = sPARENT_NAME;
}
function OpportunityPopup()
{
	return window.open('../Opportunities/Popup.aspx?ClearDisabled=1','OpportunitiesPopup','width=600,height=400,resizable=1,scrollbars=1');
}

function BillingChangeAccount(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function BillingAccountPopup()
{
	ChangeAccount = BillingChangeAccount;
	return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
}
function ShippingChangeAccount(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function ShippingAccountPopup()
{
	ChangeAccount = ShippingChangeAccount;
	return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
}

function BillingChangeContact(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "BILLING_CONTACT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "BILLING_CONTACT_NAME").ClientID %>').value = sPARENT_NAME;
}
function BillingContactPopup()
{
	ChangeContact = BillingChangeContact;
	var sACCOUNT_NAME = document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_NAME"  ).ClientID %>').value;
	return window.open('../Contacts/Popup.aspx?ACCOUNT_NAME=' + sACCOUNT_NAME,'ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
}
function ShippingChangeContact(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_CONTACT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_CONTACT_NAME").ClientID %>').value = sPARENT_NAME;
}
function ShippingContactPopup()
{
	ChangeContact = ShippingChangeContact;
	var sACCOUNT_NAME = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_NAME"  ).ClientID %>').value;
	return window.open('../Contacts/Popup.aspx?ACCOUNT_NAME=' + sACCOUNT_NAME,'ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divEditView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Quotes" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
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
						<th align="left" class="dataLabel" colspan="2"><h4 class="dataLabel"><%= L10n.Term("Quotes.LBL_BILLING_TITLE" ) %></h4></th>
						<th>&nbsp;</th>
						<th align="left" class="dataLabel" colspan="2"><h4 class="dataLabel"><%= L10n.Term("Quotes.LBL_SHIPPING_TITLE") %></h4></th>
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
				<table ID="tblLineItems" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataLabel" colspan="10"><h4 class="dataLabel"><%= L10n.Term("Quotes.LBL_LINE_ITEMS_TITLE") %></h4></th>
					</tr>
					<tr>
						<td><%= L10n.Term("Quotes.LBL_CURRENCY"        ) %></td><td><asp:DropDownList ID="CURRENCY_ID" DataValueField="ID" DataTextField="NAME_SYMBOL" Runat="server" /></td>
						<td><%= L10n.Term("Quotes.LBL_TAXRATE"         ) %></td><td><asp:DropDownList ID="TAXRATE_ID"  DataValueField="ID" DataTextField="NAME" Runat="server" /></td>
						<td><%= L10n.Term("Quotes.LBL_SHIPPER"         ) %></td><td><asp:DropDownList ID="SHIPPER_ID"  DataValueField="ID" DataTextField="NAME" Runat="server" /></td>
						<td><%= L10n.Term("Quotes.LBL_SHOW_LINE_NUMS"  ) %></td><td><asp:CheckBox     ID="SHOW_LINE_NUMS"   class="checkbox" Runat="server" /></td>
						<td><%= L10n.Term("Quotes.LBL_CALC_GRAND_TOTAL") %></td><td><asp:CheckBox     ID="CALC_GRAND_TOTAL" class="checkbox" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table ID="tblDescription" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataLabel" colspan="2"><h4 class="dataLabel"><%= L10n.Term("Quotes.LBL_DESCRIPTION_TITLE") %></h4></th>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>

	<script type="text/javascript">
	function copyAddressRight()
	{
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	function copyAddressLeft()
	{
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	</script>
</div>
