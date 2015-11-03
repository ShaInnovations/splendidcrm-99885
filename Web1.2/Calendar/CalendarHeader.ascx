<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CalendarHeader.ascx.cs" Inherits="SplendidCRM.Calendar.CalendarHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<table id="cal_tabs" width="100%" border="0" cellpadding="0" cellspacing="0" Visible="<%# !PrintView %>" runat="server">
	<tr>
		<td style="padding-bottom: 2px;">
			<asp:Button ID="btnDay"    CommandName="Day.Current"    OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term("Calendar.LBL_DAY"   ) + " " %>' title='<%# L10n.Term("Calendar.LBL_DAY"   ) %>' Runat="server" />
			<asp:Button ID="btnWeek"   CommandName="Week.Current"   OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term("Calendar.LBL_WEEK"  ) + " " %>' title='<%# L10n.Term("Calendar.LBL_WEEK"  ) %>' Runat="server" />
			<asp:Button ID="btnMonth"  CommandName="Month.Current"  OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term("Calendar.LBL_MONTH" ) + " " %>' title='<%# L10n.Term("Calendar.LBL_MONTH" ) %>' Runat="server" />
			<asp:Button ID="btnYear"   CommandName="Year.Current"   OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term("Calendar.LBL_YEAR"  ) + " " %>' title='<%# L10n.Term("Calendar.LBL_YEAR"  ) %>' Runat="server" />
			<asp:Button ID="btnShared" CommandName="Shared.Current" OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term("Calendar.LBL_SHARED") + " " %>' title='<%# L10n.Term("Calendar.LBL_SHARED") %>' Runat="server" />
		</td>
	</tr>
</table>
