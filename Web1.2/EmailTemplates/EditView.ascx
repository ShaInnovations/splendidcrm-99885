<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.EmailTemplates.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<script type="text/javascript">
// 08/08/2006 Paul.  Fixed access to selected index.  The line was missing the closing square bracket.
function ShowVariable()
{
	document.getElementById('<%= txtVariableText.ClientID %>').value = '$' + document.getElementById('<%= lstVariableName.ClientID %>').options[document.getElementById('<%= lstVariableName.ClientID %>').selectedIndex].value; 
}
function InsertVariable()
{
	var s = document.getElementById('<%= txtVariableText.ClientID %>').value;
	
	var oEditor = FCKeditorAPI.GetInstance('<%= txtBODY.ClientID %>') ;
	oEditor.InsertHtml(s);
	}
</script>

	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="EmailTemplates" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("EmailTemplates.LBL_NAME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="30%" class="dataField"><asp:TextBox ID="txtNAME" TabIndex="1" MaxLength="255" size="30" Runat="server" /></td>
						<td colspan="2">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("EmailTemplates.LBL_DESCRIPTION") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtDESCRIPTION" TabIndex="1" TextMode="MultiLine" Columns="90" Rows="1" style="overflow-y:auto;" Runat="server" /></td>
					</tr>
					<tr>
						<td colspan="4">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("EmailTemplates.LBL_INSERT_VARIABLE") %></td>
						<td class="dataField" colspan="3">
							<asp:DropDownList ID="lstVariableModule" TabIndex="1" OnSelectedIndexChanged="lstVariableModule_Changed" AutoPostBack="true" Runat="server" />
							<asp:DropDownList ID="lstVariableName"   TabIndex="1" onchange="ShowVariable()" Runat="server" />
							<span class="dataLabel">:</span>
							<asp:TextBox      ID="txtVariableText"   size="30" Runat="server" />
							<input type="button" name="btnVariableInsert" class="button" value="<%= L10n.Term("EmailTemplates.LBL_INSERT") %>" onclick="InsertVariable(); return false;" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("EmailTemplates.LBL_SUBJECT") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtSUBJECT" TabIndex="1" TextMode="MultiLine" Columns="90" Rows="1" style="overflow-y:auto;" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("EmailTemplates.LBL_BODY") %></td>
						<td class="dataField" colspan="3">
							<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
							<FCKeditorV2:FCKeditor id="txtBODY" ToolbarSet="SplendidCRM" BasePath="~/FCKeditor/" runat="server" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
