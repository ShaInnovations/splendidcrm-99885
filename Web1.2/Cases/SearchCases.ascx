<%@ Control CodeBehind="SearchCases.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Cases.SearchCases" %>
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
 * The Original Code is: SplendidCRM Open Source
 * The Initial Developer of the Original Code is SplendidCRM Software, Inc.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<div id="divSearchCases">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader ID="ctlListHeader" Title="Cases.LBL_LIST_FORM_TITLE" Runat="Server" />
	
	<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	<SplendidCRM:SplendidGrid id="grdMain" Width="100%" CssClass="listView"
		CellPadding="3" CellSpacing="0" border="0"
		AllowPaging="<%# !PrintView %>" PageSize="20" AllowSorting="true" 
		AutoGenerateColumns="false" 
		EnableViewState="true" runat="server">
		<ItemStyle            CssClass="oddListRowS1"  />
		<AlternatingItemStyle CssClass="evenListRowS1" />
		<HeaderStyle          CssClass="listViewThS1"  />
		<PagerStyle HorizontalAlign="Right" Mode="NextPrev" PageButtonCount="6" Position="Top" CssClass="listViewPaginationTdS1" PrevPageText=".LNK_LIST_PREVIOUS" NextPageText=".LNK_LIST_NEXT" />
		<Columns>
			<asp:BoundColumn     HeaderText="Cases.LBL_LIST_NUMBER"       DataField="CASE_NUMBER"      SortExpression="CASE_NUMBER"  ItemStyle-Width="10%" />
			<asp:HyperLinkColumn HeaderText="Cases.LBL_LIST_SUBJECT"      DataTextField="NAME"         SortExpression="NAME"         ItemStyle-Width="30%" ItemStyle-CssClass="listViewTdLinkS1" DataNavigateUrlField="ID"         DataNavigateUrlFormatString="view.aspx?id={0}" />
			<asp:HyperLinkColumn HeaderText="Cases.LBL_LIST_ACCOUNT_NAME" DataTextField="ACCOUNT_NAME" SortExpression="ACCOUNT_NAME" ItemStyle-Width="20%" ItemStyle-CssClass="listViewTdLinkS1" DataNavigateUrlField="ACCOUNT_ID" DataNavigateUrlFormatString="~/Accounts/view.aspx?id={0}" />
			<asp:BoundColumn     HeaderText="Cases.LBL_LIST_PRIORITY"     DataField="PRIORITY"         SortExpression="PRIORITY"     ItemStyle-Width="10%" />
			<asp:BoundColumn     HeaderText="Cases.LBL_LIST_STATUS"       DataField="STATUS"           SortExpression="STATUS"       ItemStyle-Width="10%" />
			<asp:BoundColumn     HeaderText=".LBL_LIST_ASSIGNED_USER"     DataField="ASSIGNED_TO"      SortExpression="ASSIGNED_TO"  ItemStyle-Width="10%" />
		</Columns>
	</SplendidCRM:SplendidGrid>
</div>
