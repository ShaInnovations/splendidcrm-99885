<%@ Control CodeBehind="WeekRow.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Calendar.WeekRow" %>
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
						<asp:HyperLink Text='<%# dtDATE_START.ToString("ddd d") %>' NavigateUrl='<%# "default.aspx?day=" + dtDATE_START.Day + "&month=" + dtDATE_START.Month + "&year=" + dtDATE_START.Year %>' CssClass="weekCalBodyDayLink" Runat="server" />
					</td>
					<td width="99%" class="dailyCalBodyItems">
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
