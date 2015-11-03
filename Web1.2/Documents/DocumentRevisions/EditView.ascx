<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.DocumentRevisions.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Documents" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Documents.LBL_DOC_NAME"           ) %></td>
						<td width="35%" class="dataField"><asp:HyperLink ID="lnkFILENAME" Target="_blank" Runat="server" /></td>
						<td width="15%" class="dataLabel"><%= L10n.Term("Documents.LBL_CURRENT_DOC_VERSION") %></td>
						<td width="35%" class="dataField"><asp:Label ID="txtCURRENT_REVISION" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Documents.LBL_FILENAME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<input id="fileCONTENT" type="file" TabIndex="2" size="60" MaxLength="255" runat="server" />
							<asp:RequiredFieldValidator ID="reqFILENAME" ControlToValidate="fileCONTENT" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
						<td class="dataLabel"><%= L10n.Term("Documents.LBL_DOC_VERSION") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtREVISION" TabIndex="3" Runat="server" />
							<asp:RequiredFieldValidator ID="reqREVISION" ControlToValidate="txtREVISION" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Documents.LBL_CHANGE_LOG") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtCHANGE_LOG" TabIndex="4" size="126" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
