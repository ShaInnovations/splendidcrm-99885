<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Shortcuts.ascx.cs" Inherits="SplendidCRM._controls.Shortcuts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
<div id="divShortcuts">
<p>
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="leftColumnModuleHead" onMouseOver="hiliteItem(this,'no');">
	<tr>
		<th width="5"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_left.gif" %>'  BorderWidth="0" Width="5" Height="21" Runat="server" /></th>
		<th style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/moduleTab_middle.gif);" width="100%"><%= L10n.Term("Shortcuts") %></th>
		<th width="9"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_right.gif" %>' BorderWidth="0" Width="9" Height="21" Runat="server" /></th>
	</tr>
</table>
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="subMenu" onMouseOver="hiliteItem(this,'no');" >
<%
string sApplicationPath = Request.ApplicationPath;
if ( !sApplicationPath.EndsWith("/") )
	sApplicationPath += "/";
DataTable dt = SplendidCache.Shortcuts(sSubMenu);
if ( Security.IS_ADMIN || !bAdminShortcuts )
	{
	foreach(DataRow row in dt.Rows)
	{
		string sRELATIVE_PATH = Sql.ToString(row["RELATIVE_PATH"]);
		string sIMAGE_NAME    = Sql.ToString(row["IMAGE_NAME"   ]);
		string sID            = Sql.ToString(row["DISPLAY_NAME" ]).Replace(" ", "_");
		string sDISPLAY_NAME  = L10n.Term(Sql.ToString(row["DISPLAY_NAME"]));
		if ( sRELATIVE_PATH.StartsWith("~/") )
			sRELATIVE_PATH = sRELATIVE_PATH.Replace("~/", sApplicationPath);
		%>
		<tr>
			<td class="subMenuTDIcon" width="16" style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/createBg.gif); background-repeat : repeat-y;">
				<a onMouseOver="document.getElementById('<%= sID %>').className='subMenuTDMouseOver';" onMouseOut="document.getElementById('<%= sID %>').className='subMenuTD';" class="subMenuLink" href="<%= sRELATIVE_PATH %>">
				<img src="<%= Sql.ToString(Session["themeURL"]) + "images/" + sIMAGE_NAME %>" alt="<%= L10n.Term(sDISPLAY_NAME) %>"  border="0" height="16" width="16" align="absmiddle"></a>
			</td>
			<td nowrap id="<%= sID %>" class="subMenuTD" onMouseOver="this.className='subMenuTDMouseOver';this.style.cursor='hand';" onMouseOut="this.className='subMenuTD';this.style.cursor='auto';" onclick="location.href='<%= sRELATIVE_PATH %>'">&nbsp;
				<a class="subMenuLink" href="<%= sRELATIVE_PATH %>"><%= L10n.Term(sDISPLAY_NAME) %></a>
			</td>
		</tr>
		<%
	}
}
%>
</table>
</p>
<img src="<%= Sql.ToString(Session["themeURL"]) %>images/spacer.gif" border="0" height="1" width="180"><br>
</div>
