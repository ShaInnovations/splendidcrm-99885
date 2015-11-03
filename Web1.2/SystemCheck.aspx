<%@ Page language="c#" trace="false" Codebehind="SystemCheck.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.SystemCheck" %>
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
 * The Original Code is: SplendidCRM Open Source
 * The Initial Developer of the Original Code is SplendidCRM Software, Inc.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<html>
<head>
	<title>SystemCheck</title>
	<link href="<%= Session["themeURL"] %>style.css" type="text/css" rel="stylesheet">
</head>
<body>
<br>
&nbsp;<a href="Home/">Home</a>
<h1>System</h1>
<table border="1" cellpadding="3" cellspacing="0">
<tr><td>SugarCRM compatibility</td><td><%= Application["CONFIG.sugar_version"] %></td></tr>
<tr><td>SplendidCRM Build</td><td><%= sBuildNumber %></td></tr>
<tr><td>Server Name      </td><td><%= Request.ServerVariables["SERVER_NAME"] %></td></tr>
<tr><td>ApplicationPath  </td><td><%= Request.ApplicationPath                %></td></tr>
<tr><td>rootURL          </td><td><%= Application["rootURL"         ]        %></td></tr>
<tr><td>imageURL         </td><td><%= Application["imageURL"        ]        %></td></tr>
<tr><td>scriptURL        </td><td><%= Application["scriptURL"       ]        %></td></tr>
<tr><td>chartURL         </td><td><%= Application["chartURL"        ]        %></td></tr>
<tr><td>Splendid Provider</td><td><%= Application["SplendidProvider"]        %></td></tr>
<tr><td>Default Theme    </td><td><%= Application["CONFIG.default_theme"   ] %></td></tr>
<tr><td>Default Language </td><td><%= Application["CONFIG.default_language"] %></td></tr>
</table>

<h1>User</h1>
<table border="1" cellpadding="3" cellspacing="0">
<tr><td>AUTH_USER  </td><td><%= Request.ServerVariables["AUTH_USER"] %></td></tr>
<tr><td>USER_ID    </td><td><%= Security.USER_ID                    %></td></tr>
<tr><td>FULL_NAME  </td><td><%= Security.FULL_NAME                  %></td></tr>
<tr><td>USER_NAME  </td><td><%= Security.USER_NAME                  %></td></tr>
<tr><td>IS_ADMIN   </td><td><%= Security.IS_ADMIN    ? "Yes" : "No" %></td></tr>
<tr><td>PORTAL_ONLY</td><td><%= Security.PORTAL_ONLY ? "Yes" : "No" %></td></tr>
<tr><td>themeURL   </td><td><%= Session["themeURL"] %></td></tr>
</table>

<h1>User Preferences</h1>
<table border="1" cellpadding="3" cellspacing="0">
<tr><td>USER_SETTINGS/CULTURE         </td><td><%= Session["USER_SETTINGS/CULTURE"         ] %></td></tr>
<tr><td>USER_SETTINGS/THEME           </td><td><%= Session["USER_SETTINGS/THEME"           ] %></td></tr>
<tr><td>USER_SETTINGS/DATEFORMAT      </td><td><%= Session["USER_SETTINGS/DATEFORMAT"      ] %></td></tr>
<tr><td>USER_SETTINGS/TIMEFORMAT      </td><td><%= Session["USER_SETTINGS/TIMEFORMAT"      ] %></td></tr>
<tr><td>USER_SETTINGS/TIMEZONE        </td><td><%= Session["USER_SETTINGS/TIMEZONE"        ] %></td></tr>
<tr><td>USER_SETTINGS/CURRENCY        </td><td><%= Session["USER_SETTINGS/CURRENCY"        ] %></td></tr>
<tr><td>USER_SETTINGS/CURRENCY_SYMBOL </td><td><%= Session["USER_SETTINGS/CURRENCY_SYMBOL" ] %></td></tr>
<tr><td>USER_SETTINGS/MAIL_FROMNAME   </td><td><%= Session["USER_SETTINGS/MAIL_FROMNAME"   ] %></td></tr>
<tr><td>USER_SETTINGS/MAIL_FROMADDRESS</td><td><%= Session["USER_SETTINGS/MAIL_FROMADDRESS"] %></td></tr>
</table>

<br />
<%@ Register TagPrefix="SplendidCRM" Tagname="AccessView" Src="~/Administration/ACLRoles/AccessView.ascx" %>
<SplendidCRM:AccessView ID="ctlAccessView" EnableACLEditing="false" USER_ID="<%# Security.USER_ID %>" Visible='<%# !Sql.IsEmptyGuid(Security.USER_ID) %>' Runat="Server" />

<h1>System Log</h1>
<asp:DataGrid Width="100%" CssClass="listView"
	CellPadding="3" CellSpacing="0" border="0"
	AllowPaging="false" PageSize="20" AllowSorting="false" AutoGenerateColumns="true" 
	DataSource='<%# Application["SystemErrors"] %>'
	runat="server">
	<ItemStyle            CssClass="oddListRowS1"  VerticalAlign="Top" />
	<AlternatingItemStyle CssClass="evenListRowS1" VerticalAlign="Top" />
	<HeaderStyle          CssClass="listViewThS1"  />
</asp:DataGrid>
</body>
</html>
