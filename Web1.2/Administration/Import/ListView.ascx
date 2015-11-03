<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Administration.Import.ListView" %>
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
<div id="divListView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="Administration.LBL_MODULE_TITLE" EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title="Administration.LBL_IMPORT_DATABASE_TITLE" Runat="Server" />
	
	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	<table width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td>
				<%= L10n.Term("Administration.LBL_IMPORT_DATABASE_INSTRUCTIONS") %>
			</td>
		</tr>
	</table>
	<br>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table border="0" cellspacing="0" cellpadding="0" width="100%">
					<tr>
						<th align="left" class="dataLabel" colspan="4"><%= L10n.Term("Import.LBL_SELECT_FILE") %>&nbsp;<asp:Label ID="Label1" CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></th>
					</tr>
					<tr>
						<td class="dataLabel">
							<input id="fileIMPORT" type="file" size="60" MaxLength="255" runat="server" />
							<asp:RequiredFieldValidator ID="reqFILENAME" ControlToValidate="fileIMPORT" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataField">
							<%= L10n.Term("Administration.LBL_IMPORT_DATABASE_TRUNCATE") %>&nbsp;
							<asp:CheckBox ID="chkTruncate" CssClass="checkbox" Runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
	<table width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td align="left" ><asp:Button ID="btnBack" CommandName="Back" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_BACK_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_BACK_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_BACK_BUTTON_KEY") %>' Enabled="False" Runat="server" /></td>
			<td align="right"><asp:Button ID="btnNext" CommandName="Next" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_NEXT_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_NEXT_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_NEXT_BUTTON_KEY") %>' Runat="server" /></td>
			</td>
		</tr>
	</table>
	</p>
	<table width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td><asp:Literal ID="lblImportErrors" Runat="server" /></td>
		</tr>
	</table>
</div>
