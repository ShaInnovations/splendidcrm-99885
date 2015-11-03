<%@ Control CodeBehind="YearGrid.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Calendar.YearGrid" %>
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
<div id="divYear">
	<%@ Register TagPrefix="SplendidCRM" Tagname="CalendarHeader" Src="CalendarHeader.ascx" %>
	<SplendidCRM:CalendarHeader ID="ctlCalendarHeader" ActiveTab="Year" Runat="Server" />
	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />

	<table id="daily_cal_table_outside" width="100%" border="0" cellpadding="0" cellspacing="0" class="monthBox">
		<tr>
			<td>
				<table width="100%" border="0" cellpadding="0" cellspacing="0" class="monthHeader">
					<tr>
						<td align="list" width="1%" class="monthHeaderPrevTd" nowrap>
							<asp:ImageButton CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_previous.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
							<asp:LinkButton  CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' Runat="server" />
						</td>
						<td align="center" width="98%" scope="row">
							<span class="monthHeaderH3"><%= dtCurrentDate.Year %></span>
						</td>
						<td align="right" class="monthHeaderNextTd" width="1%" nowrap>
							<asp:LinkButton  CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' Runat="server" />
							<asp:ImageButton CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_next.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td class="monthCalBody">
				<table id="tblDailyCalTable" border="0" cellpadding="0" cellspacing="1" width="100%" Runat="server" />
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%" cellpadding="0" cellspacing="0" class="monthFooter">
					<tr>
						<td align="left"  width="50%" class="monthFooterPrev" nowrap>
							<asp:ImageButton CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_previous.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
							<asp:LinkButton  CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' Runat="server" />
						</td>
						<td align="right" width="50%" class="monthFooterNext" nowrap>
							<asp:LinkButton  CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' Runat="server" />
							<asp:ImageButton CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' ImageUrl='<%# Session["themeURL"] + "images/calendar_next.gif" %>' BorderWidth="0" Width="6" Height="9" ImageAlign="AbsMiddle" Runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
