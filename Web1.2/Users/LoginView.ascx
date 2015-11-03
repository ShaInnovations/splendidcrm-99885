<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LoginView.ascx.cs" Inherits="SplendidCRM.Users.LoginView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomValidators" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
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
<script type="text/javascript">
function set_focus()
{
	var user_name     = document.getElementById('<%= txtUSER_NAME.ClientID %>');
	var user_password = document.getElementById('<%= txtPASSWORD.ClientID  %>');
	if ( user_name != null )
	{
		if ( user_name.value != '' )
		{
			user_password.focus();
			user_password.select();
		}
		else
		{
			user_name.focus();
		}
	}
}

function toggleDisplay(id)
{
	var divMore    = document.getElementById(id);
	var imgOptions = document.getElementById('<%= imgOptions.ClientID %>');
	if ( divMore.style.display == 'none' )
	{
		divMore.style.display = 'inline';
		// 11/19/2005 Paul.  Not sure why imgOptions.src is changed to blank.gif.  Must be a behavior. 
		imgOptions.src = imgOptions.src.replace('options_up.gif', 'options.gif');
	}
	else
	{
		divMore.style.display = 'none';
		imgOptions.src = imgOptions.src.replace('options.gif', 'options_up.gif');
	}
}
</script>

<style type="text/css">
	.buttonLogin {
		border: 1px solid #444444;
		font-size: 11px;
		color: #ffffff;
		background-color: #666666;
		font-weight: bold;
		}
</style>
<div id="divLoginView">
	<br>
	<br>
	<table cellpadding="0" align="center" width="100%" cellspacing="0" border="0">
		<tr>
			<td>
				<table cellpadding="0"  cellspacing="0" border="0" align="center" style="border: 1px solid #888888;">
					<tr>
						<td  align="right" style="padding: 4px; background-color: #ffffff; border-bottom: 1px solid #bbbbbb;">
							SplendidCRM
						</td>
					</tr>
					<tr>
						<td style="background-color: #dddddd; padding: 5px;" align="center">
							<table cellpadding="0" cellspacing="2" border="0" align="center" width="100%">
								<tr>
									<td colspan="2">
										<h1>SplendidCRM Open-Source</h1>
									</td>
								</tr>
								<tr>
									<td colspan="2" style="font-size: 12px; padding-bottom: 5px;">
										<asp:Label ID="lblInstructions" Runat="server"><%# L10n.Term(".NTC_LOGIN_MESSAGE") %></asp:Label>
									</td>
								</tr>
								<tr ID="trError" Visible="false" runat="server">
									<td width="30%" class="dataLabel"><%= L10n.Term("Users.LBL_ERROR") %></td>
									<td width="70%">
										<asp:Label ID="lblError" EnableViewState="false" CssClass="error" Runat="server" />
									</td>
								</tr>
							</table>
							<table id="tblUser" cellpadding="0" cellspacing="2" border="0" align="center" width="100%" Runat="server">
								<tr>
									<td width="30%" class="dataLabel"><%= L10n.Term("Users.LBL_USER_NAME") %></td>
									<td width="70%">
										<asp:TextBox ID="txtUSER_NAME" size="20" style="width: 125px" Runat="server" /> &nbsp;<%= (Sql.IsEmptyString(Application["CONFIG.default_user_name"]) ? String.Empty : "(" + Sql.ToString(Application["CONFIG.default_user_name"]) + ")") %>
									</td>
								</tr>
								<tr>
									<td width="30%" class="dataLabel"><%= L10n.Term("Users.LBL_PASSWORD") %></td>
									<td width="70%">
										<asp:TextBox ID="txtPASSWORD" size="20" style="width: 125px" TextMode="Password" Runat="server" /> &nbsp;<%= (Sql.IsEmptyString(Application["CONFIG.default_password"]) ? String.Empty : "(" + Sql.ToString(Application["CONFIG.default_password"]) + ")") %>
									</td>
								</tr>
							</table>
							<table cellpadding="0" cellspacing="2" border="0" align="center" width="100%">
								<tr>
									<td width="30%">&nbsp;</td>
									<td width="70%">
										<asp:Button ID="btnLogin" CommandName="Login" OnCommand="Page_Command" CssClass="buttonLogin" Text='<%# " "  + L10n.Term("Users.LBL_LOGIN_BUTTON_LABEL") + " "  %>' title='<%# L10n.Term("Users.LBL_LOGIN_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term("Users.LBL_LOGIN_BUTTON_KEY") %>' Runat="server" />
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<div style="cursor: hand;" onclick="toggleDisplay('divMore');">
											<asp:Image ID="imgOptions" ImageUrl='<%# Application["imageURL"] + "options.gif" %>' Width="63" Height="8" BorderWidth="0" AlternateText="Options" Runat="server" />
										</div>
									</td>
								</tr>
							</table>
							<div id="divMore">
								<table cellpadding="0" cellspacing="2" border="0" align="center" width="100%">
									<tr>
										<td width="30%" class="dataLabel"><%= L10n.Term("Users.LBL_THEME") %></td>
										<td width="70%">
											<asp:DropDownList ID="lstTHEME" DataValueField="NAME" DataTextField="NAME" Runat="server" />
										</td>
									</tr>
									<!--  // 01/20/2006 Paul.  The language setting is now in User Settings. -->
									<tr visible="false" runat="server">
										<td width="30%" class="dataLabel"><%= L10n.Term("Users.LBL_LANGUAGE") %></td>
										<td width="70%">
											<asp:DropDownList ID="lstLANGUAGE" DataValueField="NAME" DataTextField="NATIVE_NAME" Runat="server" />
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
	<br>
<%
if ( tblUser.Visible )
{
	Response.Write(Utils.RegisterEnterKeyPress(txtUSER_NAME.ClientID, btnLogin.ClientID));
	Response.Write(Utils.RegisterEnterKeyPress(txtPASSWORD.ClientID , btnLogin.ClientID));
}
%>
</div>
