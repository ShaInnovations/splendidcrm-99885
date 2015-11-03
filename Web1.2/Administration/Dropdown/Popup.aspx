<%@ Page language="c#" Codebehind="Popup.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.Administration.Dropdown.Popup" %>
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
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" >
<html>
<head>
	<title><asp:Literal id="litPageTitle" runat="server" /></title>
	<%@ Register TagPrefix="SplendidCRM" Tagname="MetaHeader" Src="~/_controls/MetaHeader.ascx" %>
	<SplendidCRM:MetaHeader ID="ctlMetaHeader" Runat="Server" />
</head>
<script type="text/javascript">
function ChangeItem()
{
	if ( window.opener != null && window.opener.ChangeItem != null )
	{
		window.opener.ChangeItem(document.getElementById('<%= txtKEY.ClientID %>').value, document.getElementById('<%= txtVALUE.ClientID %>').value, <%= nINDEX %>);
		window.close();
	}
}
// 08/30/2006 Paul.  Fix onload to support Firefox. 
window.onload = function()
{
	document.getElementById('<%= txtKEY.ClientID %>').focus();
}
</script>

<body style="margin: 10px">
<form id="frmMain" method="post" runat="server">
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="tabForm">
	<tr>
		<td>
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
				<tr>
					<td class="dataLabel" noWrap><%= L10n.Term("Dropdown.LBL_KEY"  ) %>&nbsp;&nbsp;<asp:TextBox ID="txtKEY"   CssClass="dataField" Runat="server" /></td>
					<td class="dataLabel" noWrap><%= L10n.Term("Dropdown.LBL_VALUE") %>&nbsp;&nbsp;<asp:TextBox ID="txtVALUE" CssClass="dataField" Runat="server" /></td>
					<td align="right">
						<input type="button" class="button" value="<%= L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) %>" title="<%= L10n.Term(".LBL_SAVE_BUTTON_TITLE"  ) %>" accesskey="<%= L10n.Term(".LBL_SAVE_BUTTON_KEY"  ) %>" onclick="ChangeItem();" />
						<input type="button" class="button" value="<%= L10n.Term(".LBL_CANCEL_BUTTON_LABEL") %>" title="<%= L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>" accesskey="<%= L10n.Term(".LBL_CANCEL_BUTTON_KEY") %>" onclick="window.close();" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
</form>
</body>
</html>
