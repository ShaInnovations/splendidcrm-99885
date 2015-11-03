<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Administration.DynamicLayout.GridViews.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divNewRecord">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="leftColumnModuleHead">
		<tr>
			<th width="5"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_left.gif" %>'  BorderWidth="0" Width="5" Height="21" Runat="server" /></th>
			<th style="background-image: url(<%= Sql.ToString(Session["themeURL"]) %>images/moduleTab_middle.gif);" width="100%"><%= L10n.Term("DynamicLayout.LBL_NEW_FORM_TITLE") %></th>
			<th width="9"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_right.gif" %>' BorderWidth="0" Width="9" Height="21" Runat="server" /></th>
		</tr>
	</table>
	<table border="0" cellpadding="3" cellspacing="0" width="100%">
		<tr>
			<td align="left" class="leftColumnModuleS3">
				<p>
				<input type="hidden" ID="txtFIELD_ID"    runat="server" />
				<%= L10n.Term("DynamicLayout.LBL_FIELD_TYPE") %><asp:Label ID="txtFIELD_INDEX" runat="server" /><br>
				<%= L10n.Term("DynamicLayout.LBL_FIELD_TYPE") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br>
				<asp:DropDownList ID="lstFIELD_TYPE" OnSelectedIndexChanged="lstFIELD_TYPE_Changed" AutoPostBack="true" Runat="server">
					<asp:ListItem Value="BoundColumn"    >BoundColumn</asp:ListItem>
					<asp:ListItem Value="TemplateColumn" >TemplateColumn</asp:ListItem>
					<asp:ListItem Value="HyperLinkColumn">HyperLinkColumn</asp:ListItem>
				</asp:DropDownList><br>
				<%= L10n.Term("DynamicLayout.LBL_DATA_FORMAT") %><br>
				<asp:DropDownList ID="lstDATA_FORMAT" OnSelectedIndexChanged="lstFIELD_TYPE_Changed" AutoPostBack="true" Runat="server">
					<asp:ListItem Value=""         ></asp:ListItem>
					<asp:ListItem Value="HyperLink">HyperLink</asp:ListItem>
					<asp:ListItem Value="Date"     >Date</asp:ListItem>
					<asp:ListItem Value="DateTime" >DateTime</asp:ListItem>
					<asp:ListItem Value="Currency" >Currency</asp:ListItem>
					<asp:ListItem Value="Image"    >Image</asp:ListItem>
				</asp:DropDownList><br>
					<%= L10n.Term("DynamicLayout.LBL_DATA_LABEL"        ) %>(<asp:CheckBox ID="chkFREE_FORM_LABEL" OnCheckedChanged="chkFREE_FORM_LABEL_CheckedChanged" CssClass="checkbox" AutoPostBack="True" Runat="server" /> <%= L10n.Term("DynamicLayout.LBL_FREE_FORM_DATA" ) %>)<br>
					<asp:TextBox ID="txtDATA_LABEL"  size="35" Visible="False" Runat="server" /><asp:DropDownList ID="lstDATA_LABEL" DataTextField="DISPLAY_NAME" DataValueField="NAME" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_DATA_FIELD"        ) %>(<asp:CheckBox ID="chkFREE_FORM_DATA" OnCheckedChanged="chkFREE_FORM_DATA_CheckedChanged" CssClass="checkbox" AutoPostBack="True" Runat="server" /> <%= L10n.Term("DynamicLayout.LBL_FREE_FORM_DATA" ) %>)<br>
					<asp:TextBox ID="txtDATA_FIELD"  size="35" Visible="False" Runat="server" /><asp:DropDownList ID="lstDATA_FIELD" DataTextField="ColumnName" DataValueField="ColumnName" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_SORT_EXPRESSION"   ) %><br><asp:DropDownList ID="lstSORT_EXPRESSION" DataTextField="ColumnName" DataValueField="ColumnName" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_ITEMSTYLE_WIDTH"   ) %><br><asp:TextBox ID="txtITEMSTYLE_WIDTH"    size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_ITEMSTYLE_CSSCLASS") %><br><asp:TextBox ID="txtITEMSTYLE_CSSCLASS" size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_ITEMSTYLE_HORIZONTAL_ALIGN") %><br>
					<asp:DropDownList ID="lstITEMSTYLE_HORIZONTAL_ALIGN" Runat="server">
						<asp:ListItem Value=""       ></asp:ListItem>
						<asp:ListItem Value="Left"   >Left</asp:ListItem>
						<asp:ListItem Value="Center" >Center</asp:ListItem>
						<asp:ListItem Value="Right"  >Right</asp:ListItem>
						<asp:ListItem Value="Justify">Justify</asp:ListItem>
					</asp:DropDownList><br>
					<%= L10n.Term("DynamicLayout.LBL_ITEMSTYLE_VERTICAL_ALIGN") %><br>
					<asp:DropDownList ID="lstITEMSTYLE_VERTICAL_ALIGN" Runat="server">
						<asp:ListItem Value=""      ></asp:ListItem>
						<asp:ListItem Value="Bottom">Bottom</asp:ListItem>
						<asp:ListItem Value="Middle">Middle</asp:ListItem>
						<asp:ListItem Value="Top"   >Top</asp:ListItem>
					</asp:DropDownList><br>
					<%= L10n.Term("DynamicLayout.LBL_ITEMSTYLE_WRAP"    ) %><asp:CheckBox ID="chkITEMSTYLE_WRAP" class="checkbox" Runat="server" /><br>
				<span id="spnURL" runat="server">
					<%= L10n.Term("DynamicLayout.LBL_URL_FIELD"         ) %><br><asp:TextBox ID="txtURL_FIELD"          size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_URL_FORMAT"        ) %><br><asp:TextBox ID="txtURL_FORMAT"         size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_URL_TARGET"        ) %><br><asp:TextBox ID="txtURL_TARGET"         size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_URL_MODULE"        ) %><br><asp:TextBox ID="txtURL_MODULE"         size="35" Runat="server" /><br>
					<%= L10n.Term("DynamicLayout.LBL_URL_ASSIGNED_FIELD") %><br><asp:TextBox ID="txtURL_ASSIGNED_FIELD" size="35" Runat="server" /><br>
				</span>
					<%= L10n.Term("DynamicLayout.LBL_LIST_NAME"         ) %><br><asp:DropDownList ID="lstLIST_NAME" DataTextField="LIST_NAME" DataValueField="LIST_NAME" Runat="server" /><br>
				</p>
				<p>
				<asp:Button ID="btnSave"   CommandName="NewRecord.Save"   OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE"  ) %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY"  ) %>' Runat="server" />
				<asp:Button ID="btnCancel" CommandName="NewRecord.Cancel" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_CANCEL_BUTTON_LABEL") + "  " %>' title='<%# L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_CANCEL_BUTTON_KEY") %>' Runat="server" />
				</p>
				<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtDATA_FIELD" ErrorMessage="(required)" CssClass="required" Enabled="false" EnableClientScript="false" EnableViewState="false" Runat="server" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
</div>
