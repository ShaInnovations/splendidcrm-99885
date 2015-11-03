<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Emails.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
function AddFile()
{
	for(var i=0;i<10;i++)
	{
		var elem = document.getElementById('file'+i);
		if(elem.style.display == 'none')
		{
			elem.style.display='block';
			break;
		}
	}
}
function DeleteFile(index)
{
	var elem = document.getElementById('file'+index);
	elem.style.display='none';
}
</script>

	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Emails" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="EmailButtons" Src="_controls/EmailButtons.ascx" %>
	<SplendidCRM:EmailButtons ID="ctlEmailButtons" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="SplendidCRM" Tagname="ParentPopupScripts" Src="~/_controls/ParentPopupScripts.ascx" %>
	<SplendidCRM:ParentPopupScripts ID="ctlParentPopupScripts" ListField="lstPARENT_TYPE" NameField="txtPARENT_NAME" HiddenField="txtPARENT_ID" Runat="Server" />
	<script type="text/javascript">
	var ChangeContactEmail = null;

	function ChangeEmailFields(sCONTACT_ID, sCONTACT_NAME, sCONTACT_EMAIL, txtADDRS, txtADDRS_IDS, txtADDRS_NAMES, txtADDRS_EMAILS)
	{
		if ( sCONTACT_ID == '' )
		{
			txtADDRS       .value = '';
			txtADDRS_IDS   .value = '';
			txtADDRS_NAMES .value = '';
			txtADDRS_EMAILS.value = '';
		}
		else
		{
			if ( txtADDRS       .value.length > 0 ) txtADDRS       .value += ';';
			if ( txtADDRS_IDS   .value.length > 0 ) txtADDRS_IDS   .value += ';';
			if ( txtADDRS_NAMES .value.length > 0 ) txtADDRS_NAMES .value += ';';
			if ( txtADDRS_EMAILS.value.length > 0 ) txtADDRS_EMAILS.value += ';';
			txtADDRS       .value += sCONTACT_NAME + ' <' + sCONTACT_EMAIL + '>';
			txtADDRS_IDS   .value += sCONTACT_ID   ;
			txtADDRS_NAMES .value += sCONTACT_NAME ;
			txtADDRS_EMAILS.value += sCONTACT_EMAIL;
		}
	}

	function ChangeToEmail(sCONTACT_ID, sCONTACT_NAME, sCONTACT_EMAIL)
	{
		ChangeEmailFields
			( sCONTACT_ID
			, sCONTACT_NAME
			, sCONTACT_EMAIL
			, document.getElementById('<%= txtTO_ADDRS       .ClientID %>')
			, document.getElementById('<%= txtTO_ADDRS_IDS   .ClientID %>')
			, document.getElementById('<%= txtTO_ADDRS_NAMES .ClientID %>')
			, document.getElementById('<%= txtTO_ADDRS_EMAILS.ClientID %>')
			);
	}
	function ChangeCcEmail(sCONTACT_ID, sCONTACT_NAME, sCONTACT_EMAIL)
	{
		ChangeEmailFields
			( sCONTACT_ID
			, sCONTACT_NAME
			, sCONTACT_EMAIL
			, document.getElementById('<%= txtCC_ADDRS       .ClientID %>')
			, document.getElementById('<%= txtCC_ADDRS_IDS   .ClientID %>')
			, document.getElementById('<%= txtCC_ADDRS_NAMES .ClientID %>')
			, document.getElementById('<%= txtCC_ADDRS_EMAILS.ClientID %>')
			);
	}
	function ChangeBccEmail(sCONTACT_ID, sCONTACT_NAME, sCONTACT_EMAIL)
	{
		ChangeEmailFields
			( sCONTACT_ID
			, sCONTACT_NAME
			, sCONTACT_EMAIL
			, document.getElementById('<%= txtBCC_ADDRS       .ClientID %>')
			, document.getElementById('<%= txtBCC_ADDRS_IDS   .ClientID %>')
			, document.getElementById('<%= txtBCC_ADDRS_NAMES .ClientID %>')
			, document.getElementById('<%= txtBCC_ADDRS_EMAILS.ClientID %>')
			);
	}
	function ContactEmailPopup()
	{
		return window.open('../Contacts/PopupEmail.aspx','ContactEmailPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	</script>
	<input ID="txtFROM_NAME"        type="hidden" runat="server" />
	<input ID="txtTO_ADDRS_IDS"     type="hidden" runat="server" />
	<input ID="txtTO_ADDRS_NAMES"   type="hidden" runat="server" />
	<input ID="txtTO_ADDRS_EMAILS"  type="hidden" runat="server" />
	<input ID="txtCC_ADDRS_IDS"     type="hidden" runat="server" />
	<input ID="txtCC_ADDRS_NAMES"   type="hidden" runat="server" />
	<input ID="txtCC_ADDRS_EMAILS"  type="hidden" runat="server" />
	<input ID="txtBCC_ADDRS_IDS"    type="hidden" runat="server" />
	<input ID="txtBCC_ADDRS_NAMES"  type="hidden" runat="server" />
	<input ID="txtBCC_ADDRS_EMAILS" type="hidden" runat="server" />

	<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="20%" class="dataLabel" valign="top">
							<span ID="spnDATE_START" runat="server">
							<%= L10n.Term("Emails.LBL_DATE_AND_TIME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />
							</span>
						</td>
						<td width="35%" class="dataField">
							<%@ Register TagPrefix="SplendidCRM" Tagname="DateTimeEdit" Src="~/_controls/DateTimeEdit.ascx" %>
							<SplendidCRM:DateTimeEdit ID="ctlDATE_START" EnableNone="false" Runat="Server" />
						</td>
						<td width="15%" class="dataLabel"><asp:DropDownList ID="lstPARENT_TYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="3" onChange="ChangeParentType();" Runat="server" /></td>
						<td width="25%" class="dataField" nowrap>
							<asp:TextBox ID="txtPARENT_NAME" ReadOnly="True" Runat="server" />
							<input id="txtPARENT_ID" type="hidden" runat="server" />
							<input ID="btnChangeParent" type="button" class="button" onclick="return ParentPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></td>
						<td class="dataField"><asp:DropDownList ID="lstASSIGNED_USER_ID" DataValueField="ID" DataTextField="USER_NAME" TabIndex="2" Runat="server" /></td>
						<td class="dataLabel">
							<span ID="spnTEMPLATE_LABEL" runat="server">
							<%= L10n.Term("Emails.LBL_USE_TEMPLATE") %>
							</span>
						</td>
						<td class="dataField" colspan="3"><asp:DropDownList ID="lstEMAIL_TEMPLATE" DataValueField="ID" DataTextField="NAME" TabIndex="0" OnSelectedIndexChanged="lstEMAIL_TEMPLATE_Changed" AutoPostBack="true" Runat="server" /></td>
					</tr>
					<tr>
						<td colspan="4">&nbsp;</td>
					</tr>
					<tr id="trFROM" runat="server">
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_FROM") %></td>
						<td class="dataField" colspan="3"><asp:TextBox ID="txtFROM_ADDR" TabIndex="0" MaxLength="255" size="40" Runat="server" /></td>
					</tr>
					<tr id="trNOTE_SEMICOLON" runat="server">
						<td class="dataLabel">&nbsp;</td>
						<td class="dataField" colspan="3"><%= L10n.Term("Emails.LBL_NOTE_SEMICOLON") %></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_TO") %></td>
						<td class="dataField" colspan="3">
							<asp:TextBox ID="txtTO_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />
							<input ID="btnChangeTO" type="button" class="button" onclick="ChangeContactEmail=ChangeToEmail;ContactEmailPopup();return false;" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_CC") %></td>
						<td class="dataField" colspan="3">
							<asp:TextBox ID="txtCC_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />
							<input ID="btnChangeCC" type="button" class="button" onclick="ChangeContactEmail=ChangeCcEmail;ContactEmailPopup();return false;" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_BCC") %></td>
						<td class="dataField" colspan="3">
							<asp:TextBox ID="txtBCC_ADDRS" TabIndex="0" TextMode="MultiLine" Columns="80" Rows="1" style="overflow-y:auto;" Runat="server" />
							<input ID="btnChangeBCC" type="button" class="button" onclick="ChangeContactEmail=ChangeBccEmail;ContactEmailPopup();return false;" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
						</td>
					</tr>
					<tr>
						<td colspan="4">&nbsp;</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_SUBJECT") %></td>
						<td class="dataField" colspan="3">
							<asp:TextBox ID="txtNAME" TabIndex="0" TextMode="MultiLine" Columns="100" Rows="1" style="overflow-y:auto;" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_BODY") %></td>
						<td class="dataField" colspan="3">
							<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
							<FCKeditorV2:FCKeditor id="txtDESCRIPTION" ToolbarSet="SplendidCRM" BasePath="~/FCKeditor/" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Emails.LBL_ATTACHMENTS") %></td>
						<td class="dataField" colspan="3">
							<asp:Repeater id="ctlAttachments" runat="server">
								<HeaderTemplate />
								<ItemTemplate>
										<asp:HyperLink NavigateUrl='<%# "~/Notes/Attachment.aspx?ID=" + DataBinder.Eval(Container.DataItem, "NOTE_ATTACHMENT_ID") %>' Target="_blank" Runat="server">
										<%# DataBinder.Eval(Container.DataItem, "FILENAME") %>
										</asp:HyperLink><br>
								</ItemTemplate>
								<FooterTemplate />
							</asp:Repeater>
							<div id="uploads_div">
							<div style="display: none" id="file0"><input id="email_attachment0" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('0');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file1"><input id="email_attachment1" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('1');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file2"><input id="email_attachment2" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('2');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file3"><input id="email_attachment3" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('3');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file4"><input id="email_attachment4" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('4');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file5"><input id="email_attachment5" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('5');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file6"><input id="email_attachment6" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('6');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file7"><input id="email_attachment7" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('7');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file8"><input id="email_attachment8" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('8');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							<div style="display: none" id="file9"><input id="email_attachment9" type="file" tabindex="0" size="40" runat="server" />&nbsp;<input type="button" onclick="DeleteFile('9');" class="button" value="<%= L10n.Term(".LBL_REMOVE") %>"/></div>
							</div>
							<input type="button" class="button" onclick="AddFile();" value="<%= L10n.Term("Emails.LBL_ADD_FILE") %>" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
