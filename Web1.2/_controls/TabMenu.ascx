<%@ Control Language="c#" AutoEventWireup="false" Codebehind="TabMenu.ascx.cs" Inherits="SplendidCRM._controls.TabMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="System.Data" %>
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
						<div id="divTabMenu">
							<table cellspacing="0" cellpadding="0" border="0" width="100%">
								<tr height="20">
									<td style="padding-left:7px; background-image:url(<%= Session["themeURL"] %>images/emptyTabSpace.gif);">&nbsp;</td>
<%
foreach(DataRow row in dtMenu.Rows)
{
	string sMODULE_NAME   = Sql.ToString(row["MODULE_NAME"  ]);
	string sRELATIVE_PATH = Sql.ToString(row["RELATIVE_PATH"]);
	string sDISPLAY_NAME  = L10n.Term(Sql.ToString(row["DISPLAY_NAME"]));
	string sTAB_CLASS     = (sMODULE_NAME == sActiveTab) ? "currentTab" : "otherTab";
	%>
									<td>
										<table cellspacing="0" cellpadding="0" border="0" width="100%">
											<tr class="tabHeight">
												<td style="background-image: url(<%= Sql.ToString(Session["themeURL"]) + "images/" + sTAB_CLASS %>_left.gif  );" class="<%= sTAB_CLASS %>Left"   ><img src="<%= Application["imageURL"] %>blank.gif" class="tabLeft" border="0" alt="<%= sDISPLAY_NAME %>"></td>
												<td style="background-image: url(<%= Sql.ToString(Session["themeURL"]) + "images/" + sTAB_CLASS %>_middle.gif);" class="<%= sTAB_CLASS %>" nowrap><a id="<%= "lnkTabMenu" + Sql.ToString(row["DISPLAY_NAME"]) %>" class="<%= sTAB_CLASS %>Link"  href="<%= sRELATIVE_PATH.Replace("~/", sApplicationPath) %>"><%= sDISPLAY_NAME %></a></td>
												<td style="background-image: url(<%= Sql.ToString(Session["themeURL"]) + "images/" + sTAB_CLASS %>_right.gif );" class="<%= sTAB_CLASS %>Right"  ><img src="<%= Application["imageURL"] %>blank.gif" class="tabRight" border="0" alt="<%= sDISPLAY_NAME %>"></td>
												<td style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/emptyTabSpace.gif);"><img src="<%= Application["imageURL"] %>blank.gif" class="tabSeparator" border="0" alt=""></td>
											</tr>
										</table>
									</td>
	<%
}
%>
									<td width="100%" style="background-image: url(<%= Session["themeURL"] %>images/emptyTabSpace.gif);"><img src="<%= Application["imageURL"] %>blank.gif" width="1" height="1" border="0" alt=""></td>
								</tr>
							</table>
						</div>
