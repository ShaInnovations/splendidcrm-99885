<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Reports.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="SplendidCRM" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
<%@ Register TagPrefix="SplendidCRM" Tagname="DatePicker" Src="~/_controls/DatePicker.ascx" %>
<%@ Register TagPrefix="CustomValidators" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
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
<script type="text/javascript">
function ChangeUser(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= txtASSIGNED_USER_ID.ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= txtASSIGNED_TO.ClientID      %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function UserPopup()
{
	return window.open('../Users/Popup.aspx?ClearDisabled=1','UserPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divEditView">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Reports" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="ReportButtons" Src="_controls/ReportButtons.ascx" %>
	<SplendidCRM:ReportButtons ID="ctlReportButtons" Visible="<%# !PrintView %>" Runat="Server" />
	</p>

	<script type="text/javascript">
	function SelectWizardTab(key)
	{
		for ( var i = 1; i <= 5; i++ )
		{
			var sListClass = '';
			var sLinkClass = '';
			var sListStyle = 'none';

			if ( key == i )
			{
				sListClass = 'active' ;
				sLinkClass = 'current';
				sListStyle = 'block'  ;
			}
			document.getElementById('liReportWizard'   + i).className     = sListClass;
			document.getElementById('linkReportWizard' + i).className     = sLinkClass;
			document.getElementById('divReportWizard'  + i).style.display = sListStyle;
		}
		document.getElementById('<%= txtACTIVE_TAB.ClientID %>').value = key;
		//document.getElementById('divReportWizard2').style.display = 'block';
		//document.getElementById('divReportWizard4').style.display = 'block';
	}
	</script>

	<input id="txtACTIVE_TAB" type="hidden" runat="server" />
	<ul class="tablist">
		<li id="liReportWizard1" class="active"                ><a id="linkReportWizard1" href="javascript:SelectWizardTab(1);" class="current"                      ><%= L10n.Term("Reports.LBL_WIZARD_STEP1") %></a></li>
		<li id="liReportWizard2" class=""                      ><a id="linkReportWizard2" href="javascript:SelectWizardTab(2);" class=""                             ><%= L10n.Term("Reports.LBL_WIZARD_STEP2") %></a></li>
		<li id="liReportWizard3" class=""                      ><a id="linkReportWizard3" href="javascript:SelectWizardTab(3);" class=""                             ><%= L10n.Term("Reports.LBL_WIZARD_STEP3") %></a></li>
		<li id="liReportWizard4" class=""                      ><a id="linkReportWizard4" href="javascript:SelectWizardTab(4);" class=""                             ><%= L10n.Term("Reports.LBL_WIZARD_STEP4") %></a></li>
		<li id="liReportWizard5" class="" style="DISPLAY: none"><a id="linkReportWizard5" href="javascript:SelectWizardTab(5);" class=""        style="DISPLAY: none"><%= L10n.Term("Reports.LBL_WIZARD_STEP5") %></a></li>
	</ul>
	<div id="divReportWizard1" style="DISPLAY:block">
		<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
			<tr>
				<td>
					<table border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td width="15%" class="dataLabel"><h4 class="dataLabel"><%= L10n.Term("Reports.LBL_MODULE_NAME") %></h4></td>
							<td width="35%" class="dataField">
								<asp:DropDownList ID="lstMODULE" TabIndex="1" DataValueField="MODULE_NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstMODULE_Changed" AutoPostBack="true" Runat="server" />
								<asp:Label ID="lblMODULE" runat="server" />
							</td>
							<td width="15%" class="dataLabel"><h4 class="dataLabel"><%= L10n.Term("Reports.LBL_REPORT_NAME") %></h4></td>
							<td width="35%" class="dataField">
								<asp:TextBox ID="txtNAME" TabIndex="2" size="35" MaxLength="150" Runat="server" />
								<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
							</td>
						</tr>
						<tr>
							<td class="dataLabel"><h4 class="dataLabel"><%= L10n.Term("Reports.LBL_RELATED") %></h4></td>
							<td class="dataField">
								<asp:DropDownList ID="lstRELATED" TabIndex="3" DataValueField="MODULE_NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstRELATED_Changed" AutoPostBack="true" Runat="server" /><br />
								<asp:Label ID="lblRELATED" runat="server" />
							</td>
							<td class="dataLabel"><h4 class="dataLabel"><%= L10n.Term("Reports.LBL_ASSIGNED_USER_ID") %></h4></td>
							<td class="dataField">
								<asp:TextBox ID="txtASSIGNED_TO" ReadOnly="True" Runat="server" />
								<input ID="txtASSIGNED_USER_ID" type="hidden" runat="server" />
								<input ID="btnChangeUser" type="button" class="button" onclick="return UserPopup();" title="<%# L10n.Term(".LBL_CHANGE_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_CHANGE_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_CHANGE_BUTTON_LABEL") %>" />
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<div style="DISPLAY: none">
									<asp:RadioButton ID="radREPORT_TYPE_TABULAR"   GroupName="radREPORT_TYPE" Text='<%# L10n.Term(".dom_report_types.tabular"         ) %>' CssClass="checkbox" AutoPostBack="true" runat="server" /><br />
									<asp:RadioButton ID="radREPORT_TYPE_SUMMATION" GroupName="radREPORT_TYPE" Text='<%# L10n.Term(".dom_report_types.summary"         ) %>' CssClass="checkbox" AutoPostBack="true" runat="server" /><br />
									<asp:RadioButton ID="radREPORT_TYPE_DETAILED"  GroupName="radREPORT_TYPE" Text='<%# L10n.Term(".dom_report_types.detailed_summary") %>' CssClass="checkbox" AutoPostBack="true" Visible="false" runat="server" />
								</div>
							</td>
							<td class="dataLabel">
								<h4 class="dataLabel">
									<%= L10n.Term("Reports.LBL_SHOW_QUERY") %>
								</h4>
							</td>
							<td class="dataField"><asp:CheckBox ID="chkSHOW_QUERY" CssClass="checkbox" runat="server" /></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<div id="divReportWizard2" style="DISPLAY:none">
		<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
			<tr>
				<td>
					<h4 class="dataLabel"><%= L10n.Term("Reports.LBL_FILTERS") %></h4>
					<asp:Button ID="btnAddFilter" CommandName="Filters.Add" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term("Reports.LBL_ADD_FILTER_BUTTON_LABEL") %>' title='<%# L10n.Term("Reports.LBL_ADD_FILTER_BUTTON_LABEL") %>' Runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:DataGrid ID="dgFilters" AutoGenerateColumns="false"
						CellPadding="3" CellSpacing="0" 
						AllowPaging="false" AllowSorting="false" ShowHeader="true"
						EnableViewState="true" runat="server">
						<Columns>
							<asp:BoundColumn HeaderText="Module"   DataField="MODULE_NAME" />
							<asp:BoundColumn HeaderText="Field"    DataField="DATA_FIELD"  />
							<asp:BoundColumn HeaderText="Type"     DataField="DATA_TYPE"   />
							<asp:BoundColumn HeaderText="Operator" DataField="OPERATOR"    />
							<asp:BoundColumn HeaderText="Search"   DataField="SEARCH_TEXT" />
							<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
								<ItemTemplate>
									<asp:Button ID="btnEditFilter" CommandName="Filters.Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_EDIT_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_EDIT_BUTTON_TITLE") %>' Runat="server" />
									&nbsp;
									<asp:Button ID="btnDeleteFilter" CommandName="Filters.Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term("Reports.LBL_REMOVE_BUTTON_LABEL") %>' title='<%# L10n.Term("Reports.LBL_REMOVE_BUTTON_TITLE") %>' Runat="server" />
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>
			<tr>
				<td>
					<input id="txtFILTER_ID" type="hidden" runat="server" />
					<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td valign="top">
								<asp:DropDownList ID="lstFILTER_COLUMN_SOURCE" TabIndex="10" DataValueField="MODULE_NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstFILTER_COLUMN_SOURCE_Changed" AutoPostBack="true" Runat="server" /><br />
								<asp:Label ID="lblFILTER_COLUMN_SOURCE" runat="server" />
							</td>
							<td valign="top">
								<asp:DropDownList ID="lstFILTER_COLUMN" TabIndex="11" DataValueField="NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstFILTER_COLUMN_Changed" AutoPostBack="true" Runat="server" /><br />
								<asp:Label ID="lblFILTER_COLUMN" runat="server" />
							</td>
							<td valign="top">
								<asp:DropDownList ID="lstFILTER_OPERATOR" TabIndex="12" DataValueField="NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstFILTER_OPERATOR_Changed" AutoPostBack="true" Runat="server" /><br />
								<asp:Label ID="lblFILTER_OPERATOR_TYPE" runat="server" />
								<asp:Label ID="lblFILTER_OPERATOR" runat="server" />
							</td>
							<td valign="top" nowrap>
								<table border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td>
											<input type="hidden" id="txtFILTER_SEARCH_ID" runat="server" />
											<input type="hidden" id="txtFILTER_SEARCH_DATA_TYPE" runat="server" />
											
											<asp:TextBox ID="txtFILTER_SEARCH_TEXT"   runat="server" />
											
											<asp:DropDownList ID="lstFILTER_SEARCH_DROPDOWN" DataValueField="NAME" DataTextField="DISPLAY_NAME" runat="server" />
											<asp:ListBox      ID="lstFILTER_SEARCH_LISTBOX"  DataValueField="NAME" DataTextField="DISPLAY_NAME" runat="server" />
											
											<SplendidCRM:DatePicker ID="ctlFILTER_SEARCH_START_DATE" EnableDateFormat="false" Runat="Server" />
										</td>
										<td>
											<asp:Label ID="lblFILTER_AND_SEPARATOR" Text='<%# L10n.Term("Schedulers.LBL_AND") %>' runat="server" />
										</td>
										<td>
											<SplendidCRM:DatePicker ID="ctlFILTER_SEARCH_END_DATE" EnableDateFormat="false" Runat="Server" />
											
											<asp:TextBox ID="txtFILTER_SEARCH_TEXT2"  runat="server" />
											
											<input id="btnFILTER_SEARCH_SELECT" type="button" class="button" onclick="return SearchPopup();" title='<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>' value='<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>' runat="server" />
										</td>
									</tr>
								</table>
								<asp:Label ID="lblFILTER_ID" runat="server" />
							</td>
							<td valign="top">
								<asp:Button CommandName="Filters.Update" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_UPDATE_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_UPDATE_BUTTON_TITLE") %>' Runat="server" />
							</td>
							<td valign="top">
								<asp:Button CommandName="Filters.Cancel" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_CANCEL_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>' Runat="server" />
							</td>
							<td width="80%"></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<div id="divReportWizard3" style="DISPLAY:none">
		<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
			<tr>
				<td>
					Grouping is not supported at this time. 
				</td>
			</tr>
		</table>
	</div>
	<div id="divReportWizard4" style="DISPLAY:none">
		<table width="100%" border="0" cellspacing="0" cellpadding="0"  class="tabForm">
			<tr>
				<td>
					<table border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td class="dataLabel"><h4 class="dataLabel"><%= L10n.Term("Reports.LBL_MODULE_NAME") %></font>&nbsp;</td>
							<td class="dataField">
								<asp:DropDownList ID="lstMODULE_COLUMN_SOURCE" TabIndex="1" DataValueField="MODULE_NAME" DataTextField="DISPLAY_NAME" OnSelectedIndexChanged="lstMODULE_COLUMN_SOURCE_Changed" AutoPostBack="true" Runat="server" /><br />
								<asp:Label ID="lblMODULE_COLUMN_SOURCE" runat="server" />
							</td>
						</tr>
					</table>
					<asp:Label ID="lblDisplayColumnsRequired" CssClass="required" Text='<%# L10n.Term("Reports.LBL_DISPLAY_COLUMNS_REQUIRED") %>' EnableViewState="false" runat="server" />
					<%@ Register TagPrefix="SplendidCRM" Tagname="Chooser" Src="~/_controls/Chooser.ascx" %>
					<SplendidCRM:Chooser ID="ctlDisplayColumnsChooser" ChooserTitle="Reports.LBL_CHOOSE_COLUMNS" LeftTitle="Reports.LBL_DISPLAY_COLUMNS" RightTitle="Reports.LBL_AVAILABLE_COLUMNS" Enabled="true" Runat="Server" />
				</td>
			</tr>
		</table>
	</div>
	<div id="divReportWizard5" style="DISPLAY:none">
	</div>
	<script type="text/javascript">
		SelectWizardTab(<%= txtACTIVE_TAB.Value %>);
	</script>
<%
if ( chkSHOW_QUERY.Checked )
{
	Response.Write("<br /><table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"100%\" bgcolor=\"LightGrey\"><tr><td>");
	Response.Write("<pre><b>" + (bRun ? ctlReportView.ReportSQL : sReportSQL) + "</b></pre>");
	Response.Write("</td></tr></table><br />");
}
%>
	<%@ Register TagPrefix="SplendidCRM" Tagname="ReportView" Src="ReportView.ascx" %>
	<SplendidCRM:ReportView ID="ctlReportView" Runat="Server" />
	
	<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />

<script type="text/javascript">
function MoveLeftToRight(sLeftID, sRightID, bReverse)
{
	var lstLeft  = document.getElementById(sLeftID );
	var lstRight = document.getElementById(sRightID);
	var lstModule = document.getElementById('<%= lstMODULE_COLUMN_SOURCE.ClientID %>');
	var sModuleName = lstModule.options[lstModule.selectedIndex].text;
	for ( i=0; i < lstLeft.options.length ; i++ )
	{
		if ( lstLeft.options[i].selected == true )
		{
			var oOption = document.createElement("OPTION");
			if ( bReverse == 1 )
				oOption.text  = sModuleName + ': ' + lstLeft.options[i].text;
			else if ( lstLeft.options[i].text.indexOf(': ') >= 0 )
				oOption.text  = lstLeft.options[i].text.substring(lstLeft.options[i].text.indexOf(': ')+2);
			else
				oOption.text  = lstLeft.options[i].text;
			oOption.value = lstLeft.options[i].value;
			lstRight.options.add(oOption);
		}
	}
	for ( i=lstLeft.options.length-1; i >= 0  ; i-- )
	{
		if ( lstLeft.options[i].selected == true )
			lstLeft.options.remove(i);
	}
	// 08/05/2005 Paul. Don't use the sLeftID & sRightID values as they can be reversed. 
	CopyToHidden('<%= ctlDisplayColumnsChooser.lstLeft.ClientID  %>', '<%= ctlDisplayColumnsChooser.txtLeft.ClientID  %>');
	CopyToHidden('<%= ctlDisplayColumnsChooser.lstRight.ClientID %>', '<%= ctlDisplayColumnsChooser.txtRight.ClientID %>');
}
</script>
<%
if ( bDebug )
{
	Response.Write("<div style=\"width: 1200; border: 1px solid black; overflow: hidden; \">");
	XmlUtil.Dump(rdl);
	Response.Write("</div>");
}
%>
</div>


