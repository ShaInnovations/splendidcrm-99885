<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DetailView.ascx.cs" Inherits="SplendidCRM.Administration.Config.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
 * The Original Code is: SplendidCRM Open Source
 * The Initial Developer of the Original Code is SplendidCRM Software, Inc.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<div id="divDetailView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" EnablePrint="true" EnableHelp="true" Runat="Server" />

	<%@ Register TagPrefix="SplendidCRM" Tagname="DetailButtons" Src="~/_controls/DetailButtons.ascx" %>
	<SplendidCRM:DetailButtons ID="ctlDetailButtons" Visible="<%# !PrintView %>" ShowDuplicate="false" Runat="Server" />

	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
		<tr>
			<td width="15%"  valign="top" class="tabDetailViewDL"><%= L10n.Term("Config.LBL_NAME"    ) %></td>
			<td width="35%"  valign="top" class="tabDetailViewDF"><asp:Label ID="txtNAME"     Runat="server" /></td>
			<td width="15%"  valign="top" class="tabDetailViewDL"><%= L10n.Term("Config.LBL_CATEGORY") %></td>
			<td width="35%"  valign="top" class="tabDetailViewDF"><asp:Label ID="txtCATEGORY" Runat="server" /></td>
		</tr>
		<tr>
			<td valign="top" class="tabDetailViewDL"><%= L10n.Term("Config.LBL_VALUE") %></td>
			<td colspan="3" class="tabDetailViewDF"><asp:Label ID="txtVALUE" Runat="server" /></td>
		</tr>
	</table>
</div>

<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
