<%@ Control Language="c#" AutoEventWireup="false" Codebehind="VerifyProcedures.ascx.cs" Inherits="SplendidCRM.Administration.SyncSchema.VerifyProcedures" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="System.Xml" %>
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
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="WizardButtons" Src="WizardButtons.ascx" %>
	<SplendidCRM:WizardButtons ID="ctlWizardButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel" valign="top"><%= L10n.Term("Administration.LBL_SOURCE_PROVIDER") %></td>
						<td width="35%" class="dataField" valign="top"><asp:Label ID="lblSOURCE_PROVIDER" Runat="server" /></td>
						<td width="15%" class="dataLabel" valign="top"><%= L10n.Term("Administration.LBL_DESTINATION_PROVIDER") %></td>
						<td width="35%" class="dataField" valign="top"><asp:Label ID="lblDESTINATION_PROVIDER" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel" valign="top"><%= L10n.Term("Administration.LBL_SOURCE_CONNECTION") %></td>
						<td class="dataField" valign="top"><asp:Label ID="lblSOURCE_CONNECTION" Runat="server" /></td>
						<td class="dataLabel" valign="top"><%= L10n.Term("Administration.LBL_DESTINATION_CONNECTION") %></td>
						<td class="dataField" valign="top"><asp:Label ID="lblDESTINATION_CONNECTION" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel" colspan="2"><asp:Label ID="lblSourceError"      ForeColor="Red" EnableViewState="False" Runat="server" /></td>
						<td class="dataLabel" colspan="2"><asp:Label ID="lblDestinationError" ForeColor="Red" EnableViewState="False" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th class="dataLabel"><%= L10n.Term("Administration.LBL_UNIQUE_PROCEDURES") %></th>
						<th class="dataLabel"><%= L10n.Term("Administration.LBL_UNIQUE_PROCEDURES") %></th>
					</tr>
					<tr>
						<td class="dataLabel"><asp:Literal ID="litSOURCE_UNIQUE"      Runat="server" /></td>
						<td class="dataLabel"><asp:Literal ID="litDESTINATION_UNIQUE" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th class="dataLabel"><%= L10n.Term("Administration.LBL_ALL_PROCEDURES") %></th>
						<th class="dataLabel"><%= L10n.Term("Administration.LBL_ALL_PROCEDURES") %></th>
					</tr>
					<tr>
						<td class="dataLabel"><asp:Literal ID="litSOURCE_LIST"      Runat="server" /></td>
						<td class="dataLabel"><asp:Literal ID="litDESTINATION_LIST" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
<%
#if DEBUG
	XmlUtil.Dump(GetXml());
#endif
%>
