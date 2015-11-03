<%@ Control CodeBehind="SharedGrid.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Calendar.SharedGrid" %>
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
<div id="SharedGrid">
	<%@ Register TagPrefix="SplendidCRM" Tagname="CalendarHeader" Src="CalendarHeader.ascx" %>
	<SplendidCRM:CalendarHeader ID="ctlCalendarHeader" ActiveTab="Shared" Runat="Server" />

	<p>
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td nowrap>
				<h3><asp:Image ImageUrl='<%# Session["themeURL"] + "images/h3Arrow.gif" %>' Height="11" Width="11" BorderWidth="0" Runat="server" />&nbsp;<%= L10n.Term("Calendar.LBL_SHARED_CAL_TITLE") %></h3>
			</td>
			<td align="right" nowrap>
				<span onclick="toggleDisplay('shared_cal_edit'); return false;">
					<asp:ImageButton CommandName="Edit" OnCommand="Page_Command" CssClass="chartToolsLink" AlternateText='<%# L10n.Term("Calendar.LBL_EDIT") %>' ImageUrl='<%# Session["themeURL"] + "images/edit.gif" %>' BorderWidth="0" Width="13" Height="13" ImageAlign="AbsMiddle" Runat="server" />
					<asp:LinkButton  CommandName="Edit" OnCommand="Page_Command" CssClass="chartToolsLink" Text='<%# L10n.Term("Calendar.LBL_EDIT") %>' Runat="server" />
				</span>
			</td>
		</tr>
	</table>
	</p>

	<div ID="shared_cal_edit" style="DISPLAY: none">
		<table cellpadding="0" cellspacing="3" border="0" align="center">
			<tr>
				<th valign="top"  align="center" colspan="2"><%= L10n.Term("Calendar.LBL_SELECT_USERS") %></th>
			</tr>
			<tr>
				<td>
					<table cellpadding="1" cellspacing="1" border="0" class="chartForm" align="center">
						<tr>
							<td valign="top" nowrap><b><%= L10n.Term("Calendar.LBL_USERS") %></b></td>
							<td valign="top">
								<asp:ListBox ID="lstUSERS" DataValueField="ID" DataTextField="USER_NAME" SelectionMode="Multiple" Rows="3" Runat="server" />
							</td>
							<td valign="top">
								<a onclick="javascript:MoveUp('<%= lstUSERS.ClientID %>');">
									<asp:Image ImageUrl='<%# Session["themeURL"] + "images/uparrow_big.gif" %>' AlternateText='<%# L10n.Term(".LNK_SORT") %>' style="margin-bottom: 1px;" BorderWidth="0" Width="16" Height="16" Runat="server" />
								</a><br>
								<a onclick="javascript:MoveDown('<%= lstUSERS.ClientID %>');">
									<asp:Image ImageUrl='<%# Session["themeURL"] + "images/downarrow_big.gif" %>' AlternateText='<%# L10n.Term(".LNK_SORT") %>' style="margin-top: 1px;" BorderWidth="0" Width="16" Height="16" Runat="server" />
								</a>
							</td>
						</tr>
						<tr>
							<td align="right" colspan="2">
								<asp:Button ID="btnSubmit" CommandName="Submit" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SELECT_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>' Runat="server" />
								<input type="button" class="button" onclick="toggleDisplay('shared_cal_edit');" value='<%= "  " + L10n.Term(".LBL_CANCEL_BUTTON_LABEL") + "  " %>' title='<%= L10n.Term(".LBL_DONE_CANCEL_TITLE") %>' AccessKey='<%= L10n.Term(".LBL_CANCEL_BUTTON_KEY") %>' />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>


	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	<table id="daily_cal_table_outside" width="100%" border="0" cellpadding="0" cellspacing="0" class="monthBox">
		<tr>
			<td>
				<table width="100%" border="0" cellpadding="0" cellspacing="0" class="monthHeader">
					<tr>
						<td align="list" width="1%" class="monthHeaderPrevTd" nowrap>
							<asp:ImageButton CommandName="Shared.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term(".LNK_LIST_PREVIOUS") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_previous.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
							<asp:LinkButton  CommandName="Shared.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term(".LNK_LIST_PREVIOUS") %>' Runat="server" />
						</td>
						<td align="center" width="98%" scope="row">
							<span class="monthHeaderH3"><%= dtCurrentWeek.ToLongDateString() + " - " + dtCurrentWeek.AddDays(6).ToLongDateString() %></span>
						</td>
						<td align="right" class="monthHeaderNextTd" width="1%" nowrap>
							<asp:LinkButton  CommandName="Shared.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term(".LBL_NEXT_BUTTON_LABEL") %>' Runat="server" />
							<asp:ImageButton CommandName="Shared.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term(".LBL_NEXT_BUTTON_LABEL") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_next.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td class="monthCalBody">
				<asp:PlaceHolder ID="plcWeekRows" Runat="server" />
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%" cellpadding="0" cellspacing="0" class="monthFooter">
					<tr>
						<td align="left"  width="50%" class="monthFooterPrev" nowrap>
							<asp:ImageButton CommandName="Shared.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term(".LNK_LIST_PREVIOUS") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_previous.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
							<asp:LinkButton  CommandName="Shared.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term(".LNK_LIST_PREVIOUS") %>' Runat="server" />
						</td>
						<td align="right" width="50%" class="monthFooterNext" nowrap>
							<asp:LinkButton  CommandName="Shared.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term(".LBL_NEXT_BUTTON_LABEL") %>' Runat="server" />
							<asp:ImageButton CommandName="Shared.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term(".LBL_NEXT_BUTTON_LABEL") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_next.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table> 
	<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>
