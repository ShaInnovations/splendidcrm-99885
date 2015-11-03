<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchAdvanced.ascx.cs" Inherits="SplendidCRM.Accounts.SearchAdvanced" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
						<td width="20%" class="dataLabel"><%= L10n.Term("Accounts.LBL_ACCOUNT_NAME") %></td>
						<td width="25%" class="dataField"><asp:TextBox ID="txtNAME" TabIndex="1" Size="25" MaxLength="35" Runat="server" /></td>
						<td width="20%" class="dataLabel"><%= L10n.Term("Accounts.LBL_ANY_PHONE") %></td>
						<td width="25%" class="dataField"><asp:TextBox ID="txtPHONE" TabIndex="2" Size="20" MaxLength="25" Runat="server" /></td>
						<td width="10%" class="dataLabel" rowspan="9" align="right" nowrap>
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
							<asp:Button ID="btnClear"  CommandName="Clear"  OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>' title='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>' AccessKey='<%# L10n.Term(".LBL_CLEAR_BUTTON_KEY" ) %>' Runat="server" />
							<br>
							<asp:ImageButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" ImageUrl='<%# Session["themeURL"] + "images/basic_search.gif" %>' AlternateText='<%# L10n.Term(".LNK_BASIC_SEARCH") %>' BorderWidth="0" Runat="server" />
							&nbsp;
							<asp:LinkButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" Runat="server"><%# L10n.Term(".LNK_BASIC_SEARCH") %></asp:LinkButton>
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_WEBSITE") %></td>
						<td class="dataField"><asp:TextBox ID="txtWEBSITE" TabIndex="1" Size="25" MaxLength="255" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_ANY_EMAIL") %></td>
						<td class="dataField"><asp:TextBox ID="txtEMAIL" TabIndex="2" Size="20" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_ANNUAL_REVENUE") %></td>
						<td class="dataField"><asp:TextBox ID="txtANNUAL_REVENUE" TabIndex="1" Size="10" MaxLength="10" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_EMPLOYEES") %></td>
						<td class="dataField"><asp:TextBox ID="txtEMPLOYEES" TabIndex="2" Size="25" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_INDUSTRY") %></td>
						<td class="dataField"><asp:DropDownList ID="lstINDUSTRY" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_OWNERSHIP") %></td>
						<td class="dataField"><asp:TextBox ID="txtOWNERSHIP" TabIndex="2" Size="25" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_TYPE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstACCOUNT_TYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_TICKER_SYMBOL") %></td>
						<td class="dataField"><asp:TextBox ID="txtTICKER_SYMBOL" TabIndex="2" Size="10" MaxLength="10" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_RATING") %></td>
						<td class="dataField"><asp:TextBox ID="txtRATING" TabIndex="1" Size="10" MaxLength="25" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_SIC_CODE") %></td>
						<td class="dataField"><asp:TextBox ID="txtSIC_CODE" TabIndex="2" Size="10" MaxLength="10" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_ANY_ADDRESS") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_STREET" TabIndex="1" Size="25" MaxLength="150" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_CITY") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_CITY" TabIndex="2" Size="15" MaxLength="100" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_STATE") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_STATE" TabIndex="1" Size="15" MaxLength="100" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_POSTAL_CODE") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_POSTALCODE" TabIndex="2" Size="10" MaxLength="20" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></td>
						<td class="dataField"><asp:ListBox ID="lstASSIGNED_USER_ID" DataValueField="ID" DataTextField="USER_NAME" TabIndex="1" Rows="3" SelectionMode="Multiple" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Accounts.LBL_COUNTRY") %></td>
						<td class="dataField"><asp:TextBox ID="txtADDRESS_COUNTRY" TabIndex="2" Size="10" MaxLength="20" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
