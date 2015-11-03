<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Administration.ListView" %>
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
<div id="divListView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="Administration.LBL_MODULE_TITLE" EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_ADMINISTRATION_HOME_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td width="20%" class="tabDetailViewDL2" nowrap>
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Administration.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' NavigateUrl="~/Administration/Config/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_CONFIGURE_SETTINGS") %></td>
			<td width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Upgrade.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_UPGRADE_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SYSTEM_CHECK_TITLE") %>' NavigateUrl="~/SystemCheck.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp; ( <asp:HyperLink Text='Reload' NavigateUrl="~/SystemCheck.aspx?Reload=1" CssClass="tabDetailViewDL2Link" Runat="server" /> )
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_SYSTEM_CHECK") %></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Currencies.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_CURRENCIES") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_CURRENCIES") %>' NavigateUrl="~/Administration/Currencies/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_CURRENCY") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Schema.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_SYNC_SCHEMA_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SYNC_SCHEMA_TITLE") %>' NavigateUrl="~/Administration/SyncSchema/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_SYNC_SCHEMA") %></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Import.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_IMPORT_DATABASE_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IMPORT_DATABASE_TITLE") %>' NavigateUrl="~/Administration/Import/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_IMPORT_DATABASE") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Export.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_EXPORT_DATABASE_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Enabled="false" Text='<%# L10n.Term("Administration.LBL_EXPORT_DATABASE_TITLE") %>' NavigateUrl="~/Administration/Export/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_EXPORT_DATABASE") %></td>
		</tr>
	</table>
	</p>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_USERS_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Users.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_USERS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_USERS_TITLE") %>' NavigateUrl="~/Users/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_MANAGE_USERS") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Roles.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_ROLES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_ROLES_TITLE") %>' NavigateUrl="~/Administration/ACLRoles/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_MANAGE_ROLES") %></td>
		</tr>
	</table>
	</p>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_STUDIO_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td width="20%" class="tabDetailViewDL2" nowrap>
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Layout.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_LAYOUT") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_LAYOUT") %>' NavigateUrl="~/Administration/DynamicLayout/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_LAYOUT") %></td>
			<td width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Dropdown.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_DROPDOWN_EDITOR") %>' NavigateUrl="~/Administration/Dropdown/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.DESC_DROPDOWN_EDITOR") %></td>
		</tr>
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/FieldLabels.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_EDIT_CUSTOM_FIELDS") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_EDIT_CUSTOM_FIELDS") %>' NavigateUrl="~/Administration/EditCustomFields/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp; ( <asp:HyperLink Text='Recompile' NavigateUrl="~/SystemCheck.aspx?Recompile=1" CssClass="tabDetailViewDL2Link" Runat="server" /> )
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.DESC_EDIT_CUSTOM_FIELDS") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/ConfigureTabs.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' NavigateUrl="~/Administration/ConfigureTabs/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_CHOOSE_WHICH") %></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/RenameTabs.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_RENAME_TABS") %>' NavigateUrl="~/Administration/RenameTabs/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_CHANGE_NAME_TABS") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/iFrames.gif" %>' AlternateText='<%# L10n.Term("Administration.DESC_IFRAME") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IFRAME") %>' NavigateUrl="~/iFrames/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.DESC_IFRAME") %></td>
		</tr>
		<tr>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Terminology.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY_TITLE") %>' NavigateUrl="~/Administration/Terminology/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY") %></td>
			<td class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Terminology.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY_TITLE") %>' NavigateUrl="~/Administration/Terminology/Import/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY") %></td>
		</tr>
	</table>
	</p>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_PRODUCTS_QUOTES_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/ProductTemplates.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_PRODUCT_TEMPLATES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_PRODUCT_TEMPLATES_TITLE") %>' NavigateUrl="~/Administration/ProductTemplates/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_PRODUCT_TEMPLATES_DESC") %></td>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Manufacturers.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANUFACTURERS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANUFACTURERS_TITLE") %>' NavigateUrl="~/Administration/Manufacturers/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_MANUFACTURERS_DESC") %></td>
		</tr>
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/ProductCategories.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_PRODUCT_CATEGORIES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_PRODUCT_CATEGORIES_TITLE") %>' NavigateUrl="~/Administration/ProductCategories/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_PRODUCT_CATEGORIES_DESC") %></td>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Shippers.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_SHIPPERS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SHIPPERS_TITLE") %>' NavigateUrl="~/Administration/Shippers/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_SHIPPERS_DESC") %></td>
		</tr>
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/ProductTypes.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_PRODUCT_TYPES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_PRODUCT_TYPES_TITLE") %>' NavigateUrl="~/Administration/ProductTypes/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_PRODUCT_TYPES_DESC") %></td>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/TaxRates.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_TAX_RATES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_TAX_RATES_TITLE") %>' NavigateUrl="~/Administration/TaxRates/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_TAX_RATES_DESC") %></td>
		</tr>
	</table>
	</p>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_BUG_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Releases.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_RELEASES") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_RELEASES") %>' NavigateUrl="~/Administration/Releases/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_RELEASE") %></td>
			<td width="20%" class="tabDetailViewDL2">&nbsp;</td>
			<td width="30%" class="tabDetailViewDF2">&nbsp;</td>
		</tr>
	</table>
	</p>

	<!--
	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_FORECASTS_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/TimePeriods.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_TIME_PERIODS_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_TIME_PERIODS_TITLE") %>' NavigateUrl="~/Administration/TimePeriods/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_TIME_PERIODS_DESC") %></td>
			<td width="20%" class="tabDetailViewDL2">&nbsp;</td>
			<td width="30%" class="tabDetailViewDF2">&nbsp;</td>
		</tr>
	</table>
	</p>
	-->

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_CONTRACTS_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td  width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Contracts.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_CONTRACT_TYPES_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONTRACT_TYPES_TITLE") %>' NavigateUrl="~/Administration/ContractTypes/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td width="30%" class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_CONTRACT_TYPES_DESC") %></td>
			<td width="20%" class="tabDetailViewDL2">&nbsp;</td>
			<td width="30%" class="tabDetailViewDF2">&nbsp;</td>
		</tr>
	</table>
	</p>

	<!--
	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_MASS_EMAIL_MANAGER_TITLE" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td width="20%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/EmailMan.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_MASS_EMAIL_MANAGER_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Enabled="false" Text='<%# L10n.Term("Administration.LBL_MASS_EMAIL_MANAGER_TITLE") %>' NavigateUrl="~/Administration/EmailMan/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_MASS_EMAIL_MANAGER_DESC") %></td>
			<td width="20%" class="tabDetailViewDL2">&nbsp;</td>
			<td width="30%" class="tabDetailViewDF2">&nbsp;</td>
		</tr>
	</table>
	</p>
	-->
</div>
