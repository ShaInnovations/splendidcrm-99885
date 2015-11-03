<%@ Control CodeBehind="FeedSummaryView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Feeds.FeedSummaryView" %>
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
<div id="divDetailView">
	<p>
	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	<table width="100%" cellspacing="0" cellpadding="0" border="0">
		<tr>
			<td align="right">
				<nobr>
				<asp:ImageButton CommandName="MoveUp"   CommandArgument='<%# gID.ToString() %>' OnCommand="Page_Command" ImageUrl='<%# Session["themeURL"] + "images/uparrow.gif"   %>' AlternateText='<%# L10n.Term("Feeds.LBL_MOVE_UP"                ) %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" Runat="server" />
				<asp:ImageButton CommandName="MoveDown" CommandArgument='<%# gID.ToString() %>' OnCommand="Page_Command" ImageUrl='<%# Session["themeURL"] + "images/downarrow.gif" %>' AlternateText='<%# L10n.Term("Feeds.LBL_MOVE_DOWN"              ) %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" Runat="server" />
				<asp:ImageButton CommandName="Delete"   CommandArgument='<%# gID.ToString() %>' OnCommand="Page_Command" ImageUrl='<%# Session["themeURL"] + "images/delete.gif"    %>' AlternateText='<%# L10n.Term("Feeds.LBL_DELETE_FAV_BUTTON_LABEL") %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" CssClass="listViewTdToolsS1" Runat="server" />
				</nobr>
			</td>
		</tr>
	</table>

	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="listView">
		<tr height="20">
			<td scope="col" width="100%" class="listViewThS1">
				<asp:HyperLink Text='<%# sChannelTitle %>' NavigateUrl='<%# "view.aspx?id=" + gID.ToString() %>' CssClass="listViewThLinkS1" Runat="server" />
				- 
				<asp:HyperLink Text='<%# "(" + L10n.Term("Feeds.LBL_VISIT_WEBSITE") + ")" %>' NavigateUrl='<%# sChannelLink %>' CssClass="listViewThLinkS1" Target="_new" Runat="server" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblLastBuildDate" Runat="server" />
			</td>
		</tr>
		<tr>
			<td scope="col" bgcolor="#f1f1f1" class="evenListRowS1" colspan="10">
				<asp:Repeater ID="rpFeed" Runat="server">
					<ItemTemplate>
						<li><asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "title") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "link") %>' class="listViewTdLinkS1" Target="_new" Runat="server" />
						&nbsp;&nbsp;<span class="rssItemDate"><%# DataBinder.Eval(Container.DataItem, "pubDate") %></span>
					</ItemTemplate>
				</asp:Repeater>
			</td>
		</tr>
	</table>
	</p>
</div>
