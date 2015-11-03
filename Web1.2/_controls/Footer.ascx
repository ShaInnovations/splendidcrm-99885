<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Footer.ascx.cs" Inherits="SplendidCRM._controls.Footer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
		</td>
	</tr>
	<tr ID="trFooterMenu" runat="server">
		<td colspan="2" align="center" class="footer">
			<div id="divFooterMenu">
				<hr width="80%" size="1" class="footerHR">
<asp:PlaceHolder ID="phFooterMenu" Runat="server" />
				<hr width="80%" size="1" class="footerHR">
			</div>
		</td>
	</tr>
</table>

<div id="divTheme">
	<table ID="tblTheme" cellpadding="0" cellspacing="2" border="0" align="center" runat="server">
		<tr>
			<td align="right"><%= L10n.Term("Users.LBL_THEME") %></td>
			<td>
				<asp:DropDownList ID="lstTHEME" DataValueField="NAME" DataTextField="NAME" OnSelectedIndexChanged="lstTHEME_Changed" AutoPostBack="true" Runat="server" />
			</td>
		</tr>
		<tr>
			<td align="right"><%= L10n.Term("Users.LBL_LANGUAGE") %></td>
			<td>
				<asp:DropDownList ID="lstLANGUAGE" DataValueField="NAME" DataTextField="NATIVE_NAME" OnSelectedIndexChanged="lstLANGUAGE_Changed" AutoPostBack="true" Runat="server" />
			</td>
		</tr>
	</table>
	<br>
</div>

<div id="divFooterCopyright">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td align="center" class="copyRight">
				Portions &copy; 2005-2006 <a href="http://www.splendidcrm.com" target="_blank"  class="copyRightLink">SplendidCRM Software, Inc.</a> All Rights Reserved.<br>
				<!--
				// Under the Sugar Public License referenced above, you are required to leave in all copyright statements in both
				// the code and end-user application.
				-->
				&copy; 2005 <a href="http://www.sugarcrm.com" target="_blank"  class="copyRightLink">SugarCRM Inc.</a> All Rights Reserved.<br>
				<!--
				// Under the Sugar Public License referenced above, you are required to leave in all copyright statements in both
				// the code and end-user application as well as the the powered by image. You can not change the url or the image below  .
				-->
				<asp:HyperLink ID="lnkFooterSugarCRM" NavigateUrl="http://www.sugarcrm.com" Target="_blank" Runat="server">
					<asp:Image ID="imgFooterSugarCRM" ImageUrl='<%# Application["imageURL"] + "powered_by_sugarcrm.gif" %>' AlternateText="Powered By SugarCRM" BorderWidth="0" Width="134" Height="26"  style="margin-top: 2px" Runat="server" />
				</asp:HyperLink>
			</td>
		</tr>
	</table>
</div>
