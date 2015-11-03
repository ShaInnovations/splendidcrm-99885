<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchBasic.ascx.cs" Inherits="SplendidCRM.Administration.Terminology.SearchBasic" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divSearch">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="20%" class="dataLabel"><%= L10n.Term("Terminology.LBL_NAME"        ) %></td>
						<td width="25%" class="dataField"><asp:TextBox ID="txtNAME"         TabIndex="1" Size="25" MaxLength="50" Runat="server" /></td>
						<td width="20%" class="dataLabel"><%= L10n.Term("Terminology.LBL_LANG"       ) %></td>
						<td width="25%" class="dataField"><asp:DropDownList ID="lstLANGUAGE" TabIndex="2" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td width="10%" class="dataLabel" rowspan="3" align="right" nowrap>
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_MODULE_NAME") %></td>
						<td class="dataField"><asp:DropDownList ID="lstMODULE_NAME" TabIndex="4" DataValueField="MODULE_NAME" DataTextField="MODULE_NAME" Runat="server" /></td>
						<td class="dataLabel"><asp:CheckBox ID="chkGLOBAL_TERMS" TabIndex="5" OnCheckedChanged="chkGLOBAL_TERMS_CheckedChanged" AutoPostBack="True" CssClass="checkbox" Runat="server" />&nbsp;<%= L10n.Term("Terminology.LBL_GLOBAL_TERMS") %></td>
						<td class="dataField">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_LIST_NAME_LABEL") %></td>
						<td class="dataField"><asp:DropDownList ID="lstLIST_NAME" TabIndex="6" DataValueField="LIST_NAME" DataTextField="LIST_NAME" Runat="server" /></td>
						<td class="dataLabel"><asp:CheckBox ID="chkINCLUDE_LISTS" TabIndex="7" OnCheckedChanged="chkINCLUDE_LISTS_CheckedChanged" AutoPostBack="True" CssClass="checkbox" Runat="server" />&nbsp;<%= L10n.Term("Terminology.LBL_INCLUDE_LISTS") %></td>
						<td class="dataField">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_DISPLAY_NAME") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtDISPLAY_NAME" TabIndex="3" TextMode="MultiLine" Columns="90" Rows="2" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID        , btnSearch.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtDISPLAY_NAME.ClientID, btnSearch.ClientID) %>
</div>
