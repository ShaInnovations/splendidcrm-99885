<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Administration.FieldLayout.ListView" %>
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
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="Administration.LBL_MANAGE_LAYOUT" EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>

	<p>
	<SplendidCRM:ListHeader Title="Administration.LBL_MANAGE_LAYOUT" Runat="Server" />
	<table width="100%" cellpadding="0" cellspacing="1" border="0" class="tabDetailView2">
		<tr>
			<td width="35%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Administration.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_LAYOUT_DETAILVIEW_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_LAYOUT_DETAILVIEW_TITLE") %>' NavigateUrl="~/Administration/DynamicLayout/DetailViews/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_LAYOUT_DETAILVIEW") %></td>
		</tr>
		<tr>
			<td width="35%" class="tabDetailViewDL2" nowrap>
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Administration.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_LAYOUT_EDITVIEW_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_LAYOUT_EDITVIEW_TITLE") %>' NavigateUrl="~/Administration/DynamicLayout/EditViews/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_LAYOUT_EDITVIEW") %></td>
		</tr>
		<tr>
			<td  width="35%" class="tabDetailViewDL2">
				<asp:Image ImageUrl='<%# Session["themeURL"] + "images/Administration.gif" %>' AlternateText='<%# L10n.Term("Administration.LBL_LAYOUT_GRIDVIEW_TITLE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_LAYOUT_GRIDVIEW_TITLE") %>' NavigateUrl="~/Administration/DynamicLayout/GridViews/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</td>
			<td class="tabDetailViewDF2"><%= L10n.Term("Administration.LBL_LAYOUT_GRIDVIEW") %></td>
		</tr>
	</table>
	</p>
</div>
