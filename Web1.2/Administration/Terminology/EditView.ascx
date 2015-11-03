<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Administration.Terminology.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divEditView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Terminology" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Terminology.LBL_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="35%" class="dataField">
							<asp:TextBox ID="txtNAME" TabIndex="1" MaxLength="50" size="35" Runat="server" />
							<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
						<td width="15%" class="dataLabel"><%= L10n.Term("Terminology.LBL_LANG") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="35%" class="dataField"><asp:DropDownList ID="lstLANGUAGE" TabIndex="2" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_MODULE_NAME") %></td>
						<td class="dataField"><asp:DropDownList ID="lstMODULE_NAME" TabIndex="3" DataValueField="MODULE_NAME" DataTextField="MODULE_NAME" Runat="server" /></td>
						<td class="dataLabel">&nbsp;</td>
						<td class="dataField">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_LIST_NAME_LABEL") %></td>
						<td class="dataField"><asp:DropDownList ID="lstLIST_NAME" TabIndex="4" DataValueField="LIST_NAME" DataTextField="LIST_NAME" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_LIST_ORDER") %></td>
						<td class="dataField"><asp:TextBox ID="txtLIST_ORDER" TabIndex="5" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Terminology.LBL_DISPLAY_NAME") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtDISPLAY_NAME" TabIndex="6" TextMode="MultiLine" Columns="90" Rows="2" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
