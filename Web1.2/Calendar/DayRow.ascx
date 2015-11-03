<%@ Control CodeBehind="DayRow.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Calendar.DayRow" %>
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
				<tr>
					<td width="1%" class="dailyCalBodyTime" scope="row" nowrap>
						<span onclick="toggleDisplay('<%= dtDATE_START.Hour %>_appt'); return false;">
							<a href="#" class="weekCalBodyDayLink"><%= Sql.ToTimeString(dtDATE_START) %></a>
						</span>
					</td>
					<td width="99%" class="dailyCalBodyItems">
						<div style="display:none;" id="<%= dtDATE_START.Hour %>_appt">
							<table cellspacing="1" cellpadding="0" border="0">
								<tr>
									<td colspan="2">
										<asp:RadioButton ID="radScheduleCall"    GroupName="grpAppointment" class="radio" Checked="true" Runat="server" /><span class="dataLabel"><%= L10n.Term("Calendar.LNK_NEW_CALL"   ) %></span>
										&nbsp; &nbsp;
										<asp:RadioButton ID="radScheduleMeeting" GroupName="grpAppointment" class="radio"                Runat="server" /><span class="dataLabel"><%= L10n.Term("Calendar.LNK_NEW_MEETING") %></span>
									</td>
								</tr>
								<tr>
									<td colspan="2"><span class="dataLabel"><%= L10n.Term("Meetings.LBL_SUBJECT") %></span>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
								</tr>
								<tr>
									<td valign="top"><asp:TextBox ID="txtNAME" size="30" MaxLength="255" Runat="server" /></td>
									<td valign="top"><asp:Button ID="btnSave" CommandName="Save" OnCommand="Page_Command" CssClass="button" Text='<%# " " + L10n.Term(".LBL_SAVE_BUTTON_LABEL") + " " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /></td>
								</tr>
							</table>
							<br>
						</div>
						
						<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
						<asp:DataList ID="lstMain" Width="100%"  BorderWidth="0" CellPadding="0" CellSpacing="0" ShowBorder="False"
							RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="0" Runat="server">
							<ItemTemplate>
								<div style="margin-top: 1px;">
								<table cellpadding="0" cellspacing="0" border="0" width="100%" class="monthCalBodyDayItem">
									<tr>
										<td class="monthCalBodyDayIconTd">
											<asp:Image ImageUrl='<%# Session["themeURL"] + "images/" + DataBinder.Eval(Container.DataItem, "ACTIVITY_TYPE") + ".gif" %>' AlternateText='<%# L10n.Term(Sql.ToString(DataBinder.Eval(Container.DataItem, "STATUS"))) + ": " + DataBinder.Eval(Container.DataItem, "NAME") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
										</td>
										<td class="monthCalBodyDayItemTd" width="100%">
											<asp:HyperLink Text='<%# L10n.Term(Sql.ToString(DataBinder.Eval(Container.DataItem, "STATUS"))) + ": " + DataBinder.Eval(Container.DataItem, "NAME") %>' NavigateUrl='<%# "~/" + DataBinder.Eval(Container.DataItem, "ACTIVITY_TYPE") + "/view.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") %>' CssClass="monthCalBodyDayItemLink" Runat="server" />
										</td>
									</tr>
								</table>
								<div>
							</ItemTemplate>
							<SeparatorStyle Height="1px" />
						</asp:DataList>
					</td>
				</tr>
