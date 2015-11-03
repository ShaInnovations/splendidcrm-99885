<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchAdvanced.ascx.cs" Inherits="SplendidCRM.Employees.SearchAdvanced" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Employees.LBL_FIRST_NAME") %></td>
						<td width="30%" class="dataField"><asp:TextBox ID="txtFIRST_NAME" TabIndex="1" Size="25" MaxLength="30" Runat="server" /></td>
						<td width="15%" class="dataLabel"><%= L10n.Term("Employees.LBL_USER_NAME") %></td>
						<td width="30%" class="dataField"><asp:TextBox ID="txtUSER_NAME" TabIndex="2" Size="20" MaxLength="20" Runat="server" /></td>
						<td width="10%" class="dataLabel" rowspan="8" align="right" nowrap>
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
							<asp:Button ID="btnClear"  CommandName="Clear"  OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>' title='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>' AccessKey='<%# L10n.Term(".LBL_CLEAR_BUTTON_KEY" ) %>' Runat="server" />
							<br>
							<asp:ImageButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" ImageUrl='<%# Session["themeURL"] + "images/basic_search.gif" %>' AlternateText='<%# L10n.Term(".LNK_BASIC_SEARCH") %>' BorderWidth="0" Runat="server" />
							&nbsp;
							<asp:LinkButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" Runat="server"><%# L10n.Term(".LNK_BASIC_SEARCH") %></asp:LinkButton>
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_LAST_NAME") %></td>
						<td class="dataField"><asp:TextBox ID="txtLAST_NAME" TabIndex="1" Size="25" MaxLength="30" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_EMPLOYEE_STATUS") %></td>
						<td class="dataField"><asp:DropDownList ID="lstEMPLOYEE_STATUS" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_TITLE") %></td>
						<td class="dataField"><asp:TextBox ID="txtTITLE" TabIndex="1" Size="25" MaxLength="50" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_ANY_PHONE") %></td>
						<td class="dataField"><asp:TextBox ID="txtPHONE" TabIndex="2" Size="20" MaxLength="50" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_DEPARTMENT") %></td>
						<td class="dataField"><asp:TextBox ID="txtDEPARTMENT" TabIndex="1" Size="25" MaxLength="50" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_ANY_EMAIL") %></td>
						<td class="dataField"><asp:TextBox ID="txtEMAIL" TabIndex="2" Size="20" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_ADDRESS") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_STREET" TabIndex="1" Size="25" MaxLength="150" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_CITY") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_CITY" TabIndex="2" Size="15" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_STATE") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_STATE" TabIndex="1" Size="15" MaxLength="100" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_POSTAL_CODE") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_POSTALCODE" TabIndex="2" Size="10" MaxLength="9" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Employees.LBL_COUNTRY") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_COUNTRY" TabIndex="2" Size="10" MaxLength="25" Runat="server" /></td>
						<td class="dataLabel">&nbsp;</td>
						<td class="dataField">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
