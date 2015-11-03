<%@ Control Language="c#" AutoEventWireup="false" Codebehind="MassUpdate.ascx.cs" Inherits="SplendidCRM.TestCases.MassUpdate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divMassUpdate">
	<br>
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title=".LBL_MASS_UPDATE_TITLE" Runat="Server" />
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td style="padding-bottom: 2px;">
				<asp:Button ID="btnUpdate" CommandName="MassUpdate" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_UPDATE") %>' Runat="server" />
				<asp:Button ID="btnDelete" CommandName="MassDelete" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_DELETE") %>' Runat="server" />
			</td>
		</tr>
	</table>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="25%" class="dataLabel"><%= L10n.Term("TestCases.LBL_TEST_TYPE") %></td>
						<td width="25%" class="dataField"><asp:DropDownList ID="lstTEST_TYPE"  DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td width="25%" class="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></td>
						<td width="25%" class="dataField"><asp:DropDownList ID="lstASSIGNED_USER_ID" DataValueField="ID" DataTextField="USER_NAME" Runat="server" /></td>
					</tr>
					<tr>
						<td width="25%" class="dataLabel"><%= L10n.Term("TestCases.LBL_TEST_PHASE") %></td>
						<td width="25%" class="dataField"><asp:DropDownList ID="lstTEST_PHASE" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td width="25%" class="dataLabel">&nbsp;</td>
						<td width="25%" class="dataField">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
