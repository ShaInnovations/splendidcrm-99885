<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LeadDetailButtons.ascx.cs" Inherits="SplendidCRM.Leads._controls.LeadDetailButtons" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<table width="100%" border="0" cellpadding="0" cellspacing="0" Visible="<%# !PrintView %>" runat="server">
	<tr>
		<td style="padding-bottom: 2px;">
			<asp:Button ID="btnEdit"      CommandName="Edit"      OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_EDIT_BUTTON_LABEL"     ) + "  " %>' title='<%# L10n.Term(".LBL_EDIT_BUTTON_TITLE"     ) %>' AccessKey='<%# L10n.Term(".LBL_EDIT_BUTTON_KEY"     ) %>' Runat="server" />
			<asp:Button ID="btnDuplicate" CommandName="Duplicate" OnCommand="Page_Command" CssClass="button" Text='<%# " "  + L10n.Term(".LBL_DUPLICATE_BUTTON_LABEL") + " "  %>' title='<%# L10n.Term(".LBL_DUPLICATE_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_DUPLICATE_BUTTON_KEY") %>' Runat="server" />
			<span onclick="return confirm('<%= L10n.TermJavaScript(".NTC_DELETE_CONFIRMATION") %>')">
			<asp:Button ID="btnDelete"    CommandName="Delete"    OnCommand="Page_Command" CssClass="button" Text='<%# " "  + L10n.Term(".LBL_DELETE_BUTTON_LABEL"   ) + " "  %>' title='<%# L10n.Term(".LBL_DELETE_BUTTON_TITLE"   ) %>' AccessKey='<%# L10n.Term(".LBL_DELETE_BUTTON_KEY"   ) %>' Runat="server" />
			</span>
			<asp:Button ID="btnConvert"   CommandName="Convert"   OnCommand="Page_Command" CssClass="button" Text='<%# " "  + L10n.Term("Leads.LBL_CONVERTLEAD"      ) + " "  %>' title='<%# L10n.Term("Leads.LBL_CONVERTLEAD_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_CONVERTLEAD_BUTTON_KEY") %>' Runat="server" />
			<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
		</td>
	</tr>
</table>
