<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ConvertView.ascx.cs" Inherits="SplendidCRM.Prospects.ConvertView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Leads" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<script type="text/javascript">
	function ChangeCampaign(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "CAMPAIGN_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "CAMPAIGN_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function CampaignPopup()
	{
		return window.open('../Campaigns/Popup.aspx','CampaignPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	</script>
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataField" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Leads.LBL_CONTACT_INFORMATION") %></h4></th>
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
						<th align="left" class="dataLabel" colspan="5"><h4 class="dataLabel"><%= L10n.Term("Leads.LBL_ADDRESS_INFORMATION") %></h4></th>
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
				<table ID="tblDescription" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<th align="left" class="dataLabel" colspan="2"><h4 class="dataLabel"><%= L10n.Term("Leads.LBL_DESCRIPTION_INFORMATION") %></h4></th>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>

	<script type="text/javascript">
	function copyAddressRight()
	{
		document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	function copyAddressLeft()
	{
		document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "PRIMARY_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "ALT_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	</script>
</div>
