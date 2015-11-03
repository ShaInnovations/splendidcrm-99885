<%@ Control CodeBehind="ImportView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Import.ImportView" %>
<%@ Register TagPrefix="SplendidCRM" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
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
<div id="divImportView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Import" Title="Import.LBL_STEP_1_TITLE" EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" Runat="Server" />

	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	<div id="divImportStep1" runat="server">
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
			<tr>
				<td>
					<h4><%= L10n.Term("Import.LBL_WHAT_IS") %></h4>
					<asp:RadioButton ID="radEXCEL"           GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_EXCEL"          ) %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radXML_SPREADSHEET" GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_XML_SPREADSHEET") %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radXML"             GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_XML"            ) %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radSALESFORCE"      GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_SALESFORCE"     ) %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radACT_2005"        GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_ACT_2005"       ) %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radCUSTOM_CSV"      GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_CUSTOM_CSV"     ) %>' CssClass="checkbox" runat="server" /><br />
					<asp:RadioButton ID="radCUSTOM_TAB"      GroupName="radSOURCE" Text='<%# L10n.Term("Import.LBL_CUSTOM_TAB"     ) %>' CssClass="checkbox" runat="server" /><br />
				</td>
			</tr>
		</table>
	</div>
	<div id="divImportStep2" runat="server">
		<table id="tblInstructionsExcel" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_EXCEL_TITLE") %></p>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsXmlSpreadsheet" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_XML_SPREADSHEET_TITLE") %></p>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsXML" width="100%" cellpadding="0" cellspacing="0" border="0" visible="true" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_XML_TITLE") %></p>
					<pre>
&lt;xml&gt;
   &lt;<%= sImportModule.ToLower() %>&gt;
      &lt;id&gt;&lt;/id&gt;
      &lt;name&gt;&lt;/name&gt;
   &lt;/<%= sImportModule.ToLower() %>&gt;
&lt;/xml&gt;</pre>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsSalesForce" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_SF_TITLE") %></p>
				</td>
			</tr>
			<tr>
				<td class="body">
					<table>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_1") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_1") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_2") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_2") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_3") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_3") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_4") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_4") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_5") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_5") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_6") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_6") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_7") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_7") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_8") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_8") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_9") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_9") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_10") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_10") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_11") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_SF_NUM_11") %></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsAct" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_ACT_TITLE") %></p>
				</td>
			</tr>
			<tr>
				<td class="body">
					<table>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_1") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_1") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_2") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_2") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_3") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_3") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_4") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_4") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_5") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_5") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_6") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_6") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_7") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_7") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_8") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_8") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_9") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_9") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_10") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_ACT_NUM_10") %></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsCommaDelimited" width="100%" cellpadding="0" cellspacing="0" border="0" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_CUSTOM_TITLE") %></p>
				</td>
			</tr>
			<tr>
				<td class="body">
					<table>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_1") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_CUSTOM_NUM_1") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_2") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_CUSTOM_NUM_2") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_3") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_CUSTOM_NUM_3") %></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table id="tblInstructionsTabDelimited" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<p><%= L10n.Term("Import.LBL_IMPORT_TAB_TITLE") %></p>
				</td>
			</tr>
			<tr>
				<td class="body">
					<table>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_1") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_TAB_NUM_1") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_2") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_TAB_NUM_2") %></td>
						</tr>
						<tr>
							<td valign="top" class="body"><b><%= L10n.Term("Import.LBL_NUM_3") %></b></td>
							<td class="body"><%= L10n.Term("Import.LBL_TAB_NUM_3") %></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br>
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
			<tr>
				<td>
					<table border="0" cellspacing="0" cellpadding="0" width="100%">
						<tr>
							<th align="left" class="dataLabel" colspan="4"><%= L10n.Term("Import.LBL_SELECT_FILE") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></th>
						</tr>
						<tr>
							<td class="dataLabel">
								<input id="fileIMPORT" type="file" size="60" MaxLength="255" runat="server" />
								<asp:RequiredFieldValidator ID="reqFILENAME" ControlToValidate="fileIMPORT" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
							</td>
						</tr>
						<tr>
							<td class="dataField">
								<%= L10n.Term("Import.LBL_HAS_HEADER") %>&nbsp;
								<asp:CheckBox ID="chkHasHeader" CssClass="checkbox" Runat="server" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<div id="divImportStep3" runat="server">
		<table width="100%" cellpadding="0" cellspacing="0" border="0" visible="true">
			<tr>
				<td align="right" nowrap>
					<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /> <%= L10n.Term(".NTC_REQUIRED") %>
				</td>
			</tr>
			<tr>
				<td class="body">
					<%= L10n.Term("Import.LBL_SELECT_FIELDS_TO_MAP") %>
				</td>
			</tr>
		</table>
		<table id="tblImportMappings" width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView" runat="server" />
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
			<tr>
				<td class="tabDetailViewDF">
					<asp:CheckBox ID="chkSaveMap" CssClass="checkbox" runat="server" /> <%= L10n.Term("Import.LBL_SAVE_AS_CUSTOM") %> <asp:TextBox ID="txtSaveMap" runat="server" />
				</td>
			</tr>
		</table>
		<table id="tblNotesAccounts" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<br />
					<b><%= L10n.Term("Import.LBL_NOTES") %></b>
					<ul>
						<li><%= L10n.Term("Import.LBL_ACCOUNTS_NOTE_1") %></li>
						<li><%= L10n.Term("Import.LBL_ACCOUNTS_NOTE_2") %></li>
					</ul>
				</td>
			</tr>
		</table>
		<table id="tblNotesContacts" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<br />
					<b><%= L10n.Term("Import.LBL_NOTES") %></b>
					<ul>
						<li><%= L10n.Term("Import.LBL_CONTACTS_NOTE_1") %></li>
						<li><%= L10n.Term("Import.LBL_CONTACTS_NOTE_2") %></li>
						<li><%= L10n.Term("Import.LBL_CONTACTS_NOTE_3") %></li>
						<li><%= L10n.Term("Import.LBL_CONTACTS_NOTE_4") %></li>
					</ul>
				</td>
			</tr>
		</table>
		<table id="tblNotesOpportunities" width="100%" cellpadding="0" cellspacing="0" border="0" visible="false" runat="server">
			<tr>
				<td class="body">
					<br />
					<b><%= L10n.Term("Import.LBL_NOTES") %></b>
					<ul>
						<li><%= L10n.Term("Import.LBL_OPPORTUNITIES_NOTE_1") %></li>
					</ul>
				</td>
			</tr>
		</table>
	</div>
	<div id="divImportStep4" runat="server">
		<asp:Label ID="lblStatus"       runat="server" /><br />
		<asp:Label ID="lblSuccessCount" runat="server" /><br />
		<asp:Label ID="lblFailedCount"  runat="server" /><br />
	</div>
	<br>
	<table width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td align="left">
				<asp:Button ID="btnBack"   CommandName="Back"   OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_BACK_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_BACK_BUTTON_TITLE") %>' Runat="server" />
			</td>
			<td align="right">
				<asp:Button ID="btnNext"   CommandName="Next"       OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_NEXT_BUTTON_LABEL") + "  " %>' Runat="server" />
				<asp:Button ID="btnImport" CommandName="ImportMore" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term("Import.LBL_IMPORT_MORE") + "  " %>' title='<%# L10n.Term("Import.LBL_IMPORT_MORE") %>' Visible="false" Runat="server" />
				<asp:Button ID="btnFinish" CommandName="Finish"     OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term("Import.LBL_FINISHED"   ) + "  " %>' title='<%# L10n.Term("Import.LBL_FINISHED"   ) %>' Visible="false" Runat="server" />
			</td>
			</td>
		</tr>
	</table>
	</p>
	<div id="divLastImported" runat="server">
		<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
		<SplendidCRM:ListHeader ID="ctlListHeader" Visible="<%# !PrintView %>" Title="Import.LBL_LAST_IMPORTED" Runat="Server" />
		<SplendidCRM:SplendidGrid id="grdMain" Width="100%" CssClass="listView"
			CellPadding="3" CellSpacing="0" border="0"
			AllowPaging="false" PageSize="20" AllowSorting="false" 
			AutoGenerateColumns="false" 
			EnableViewState="true" runat="server">
			<ItemStyle            CssClass="oddListRowS1"  />
			<AlternatingItemStyle CssClass="evenListRowS1" />
			<HeaderStyle          CssClass="listViewThS1"  />
			<PagerStyle HorizontalAlign="Right" Mode="NextPrev" PageButtonCount="6" Position="Top" CssClass="listViewPaginationTdS1" PrevPageText=".LNK_LIST_PREVIOUS" NextPageText=".LNK_LIST_NEXT" />
		</SplendidCRM:SplendidGrid>
	</div>
	<div style="DISPLAY: none">
		<pre><%= sbImport.ToString() %></pre>
	</div>
<%
if ( bDebug )
{
	//Response.Write("<div style=\"width: 1200; border: 1px solid black; overflow: hidden; \">");
	XmlUtil.Dump(xml);
	//Response.Write("</div>");
}
%>
</div>
