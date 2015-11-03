<%@ Page language="c#" Codebehind="Password.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.Administration.Users.Password" %>
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
function ChangePassword()
{
	var txtIS_ADMIN         = document.getElementById('<%= txtIS_ADMIN.ClientID         %>');
	var txtOLD_PASSWORD     = document.getElementById('<%= txtOLD_PASSWORD.ClientID     %>');
	var txtNEW_PASSWORD     = document.getElementById('<%= txtNEW_PASSWORD.ClientID     %>');
	var txtCONFIRM_PASSWORD = document.getElementById('<%= txtCONFIRM_PASSWORD.ClientID %>');
	if ( txtIS_ADMIN.value == 0 && txtOLD_PASSWORD.value == '' )
	{
		alert('<%= L10n.TermJavaScript("Users.ERR_ENTER_OLD_PASSWORD") %>');
		return false;
	}
	if ( txtNEW_PASSWORD.value == '' )
	{
		alert('<%= L10n.TermJavaScript("Users.ERR_ENTER_NEW_PASSWORD") %>');
		return false;
	}
	if ( txtCONFIRM_PASSWORD.value == '' )
	{
		alert('<%= L10n.TermJavaScript("Users.ERR_ENTER_CONFIRMATION_PASSWORD") %>');
		return false;
	}

	if ( txtNEW_PASSWORD.value == txtCONFIRM_PASSWORD.value )
	{
		if ( window.opener != null && window.opener.ChangePassword != null )
		{
			window.opener.ChangePassword(txtOLD_PASSWORD.value, txtNEW_PASSWORD.value, txtCONFIRM_PASSWORD.value);
			window.close();
		}
		return true;
	}
	else
	{
		alert('<%= L10n.TermJavaScript("Users.ERR_REENTER_PASSWORDS") %>');
		return false;
	}
	return false;
}

// 08/30/2006 Paul.  Fix onload to support Firefox. 
window.onload = function()
{
	var txtIS_ADMIN         = document.getElementById('<%= txtIS_ADMIN.ClientID         %>');
	var txtOLD_PASSWORD     = document.getElementById('<%= txtOLD_PASSWORD.ClientID     %>');
	var txtNEW_PASSWORD     = document.getElementById('<%= txtNEW_PASSWORD.ClientID     %>');
	if ( txtIS_ADMIN.value == 0 )
		txtOLD_PASSWORD.focus();
	else
		txtNEW_PASSWORD.focus();
}
</script>

<body style="margin: 10px">
<form id="frmMain" method="post" runat="server">
<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
<SplendidCRM:ListHeader Title="Users.LBL_CHANGE_PASSWORD" Runat="Server" />
<br>
<input ID="txtIS_ADMIN" type="hidden" value='<%# SplendidCRM.Security.IS_ADMIN ? "1" : "0" %>' runat="server" />
<div style="DISPLAY: <%= Security.IS_ADMIN ? "none" : "inline" %>">
	<table width="100%" cellspacing="0" cellpadding="1" border="0">
		<tr>
			<td width="40%" class="dataLabel"><%= L10n.Term("Users.LBL_OLD_PASSWORD") %></td>
			<td width="60%" class="dataField"><asp:TextBox ID="txtOLD_PASSWORD"     TextMode="Password" TabIndex="1" size="15" MaxLength="15" CssClass="dataField" Runat="server" /></td>
		</tr>
	</table>
</div>
<table width="100%" cellspacing="0" cellpadding="1" border="0">
	<tr>
		<td width="40%" class="dataLabel"nowrap><%= L10n.Term("Users.LBL_NEW_PASSWORD") %></td>
		<td width="60%" class="dataField"><asp:TextBox ID="txtNEW_PASSWORD"     TextMode="Password" TabIndex="1" size="15" MaxLength="15" CssClass="dataField" Runat="server" /></td>
	</tr>
	<tr>
		<td width="40%" class="dataLabel" nowrap><%= L10n.Term("Users.LBL_CONFIRM_PASSWORD") %></td>
		<td width="60%" class="dataField"><asp:TextBox ID="txtCONFIRM_PASSWORD" TextMode="Password" TabIndex="1" size="15" MaxLength="15" CssClass="dataField" Runat="server" /></td>
	</tr>
	<tr>
		<td width="40%" class="dataLabel"></td>
		<td width="60%" class="dataField"></td>
	</tr>
</table>
<br>
<table width="100%" cellspacing="0" cellpadding="1" border="0">
	<tr>
		<td align="right">
			<input type="button" class="button" value="<%= L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) %>" title="<%= L10n.Term(".LBL_SAVE_BUTTON_TITLE"  ) %>" accesskey="<%= L10n.Term(".LBL_SAVE_BUTTON_KEY"  ) %>" onclick="return ChangePassword();" />
		</td>
		<td align="left">
			<input type="button" class="button" value="<%= L10n.Term(".LBL_CANCEL_BUTTON_LABEL") %>" title="<%= L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>" accesskey="<%= L10n.Term(".LBL_CANCEL_BUTTON_KEY") %>" onclick="window.close();"   />
		</td>
	</tr>
</table>
</form>
</body>
</html>



