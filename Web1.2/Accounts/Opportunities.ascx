<%@ Control CodeBehind="Opportunities.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Accounts.Opportunities" %>
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
<script type="text/javascript">
function ChangeOpportunity(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= txtOPPORTUNITY_ID.ClientID %>').value = sPARENT_ID  ;
	document.forms[0].submit();
}
function OpportunityPopup()
{
	return window.open('../Opportunities/Popup.aspx?ClearDisabled=1','OpportunitiesPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divAccountsOpportunities">
	<input ID="txtOPPORTUNITY_ID" type="hidden" Runat="server" />
	<br>
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title="Opportunities.LBL_MODULE_NAME" Runat="Server" />
	
	<table width="100%" border="0" cellpadding="0" cellspacing="0" Visible="<%# !PrintView %>" runat="server">
		<tr>
			<td style="padding-bottom: 2px;">
				<asp:Button CommandName="Opportunities.Create" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_NEW_BUTTON_LABEL"   ) + "  " %>' title='<%# L10n.Term(".LBL_NEW_BUTTON_TITLE"   ) %>' AccessKey='<%# L10n.Term(".LBL_NEW_BUTTON_KEY"   ) %>' Runat="server" />
				<input type="button" class="button" onclick="return OpportunityPopup();" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accessKey="<%# L10n.Term(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>

	<SplendidCRM:SplendidGrid id="grdMain" Width="100%" CssClass="listView"
		CellPadding="3" CellSpacing="0" border="0"
		AllowPaging="false" PageSize="20" AllowSorting="false" 
		AutoGenerateColumns="false" 
		EnableViewState="true" runat="server">
		<ItemStyle            CssClass="oddListRowS1"  />
		<AlternatingItemStyle CssClass="evenListRowS1" />
		<HeaderStyle          CssClass="listViewThS1"  />
		<PagerStyle HorizontalAlign="Right" Mode="NextPrev" PageButtonCount="6" Position="Top" CssClass="listViewPaginationTdS1" PrevPageText=".LNK_LIST_PREVIOUS" NextPageText=".LNK_LIST_NEXT" />
		<Columns>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton CommandName="Opportunities.Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OPPORTUNITY_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_EDIT") %>' ImageUrl='<%# Session["themeURL"] + "images/edit_inline.gif" %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" Runat="server" />
					<asp:LinkButton  CommandName="Opportunities.Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OPPORTUNITY_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_EDIT") %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</SplendidCRM:SplendidGrid>
</div>
