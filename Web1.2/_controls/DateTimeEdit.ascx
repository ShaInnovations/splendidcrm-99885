<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DateTimeEdit.ascx.cs" Inherits="SplendidCRM._controls.DateTimeEdit" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
function ChangeDate<%= txtDATE.ClientID.Replace(":", "_") %>(sDATE)
{
	document.getElementById('<%= txtDATE.ClientID %>').value = sDATE;
}
</script>
<table border="0" cellpadding="0" cellspacing="0">
	<tr >
		<td nowrap>
			<asp:TextBox ID="txtDATE" TabIndex="1" size="11" MaxLength="40" Runat="server" />
			<span onclick="ChangeDate=ChangeDate<%= txtDATE.ClientID.Replace(":", "_") %>;CalendarPopup(document.getElementById('<%= txtDATE.ClientID %>'), event.clientX, event.clientY);">
				<asp:Image ID="imgCalendar" ImageUrl='<%# Session["themeURL"] + "images/Calendar.gif" %>' AlternateText='<%# L10n.Term(".LBL_ENTER_DATE") %>' BorderWidth="0" Width="16" Height="16" ImageAlign="AbsMiddle" Runat="server" />
			</span>
			&nbsp;
		</td>
		<td nowrap>
			<asp:TextBox ID="txtTIME" TabIndex="1" size="7" MaxLength="12" Runat="server" />
		</td>
		<td nowrap>
			<div style="DISPLAY: <%= bEnableNone ? "INLINE" : "NONE" %>">
			<asp:CheckBox ID="chkNONE" TabIndex="1" CssClass="checkbox" Runat="server" />
			<%= L10n.Term("Tasks.LBL_NONE") %>
			</div>
		</td>
		<td>
			<!-- 08/31/2006 Paul.  We cannot use a regular expression validator because there are just too many date formats. -->
			<CustomValidators:DateValidator ID="valDATE" ControlToValidate="txtDATE" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
			<CustomValidators:TimeValidator ID="valTIME" ControlToValidate="txtTIME" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
			<asp:RequiredFieldValidator     ID="reqDATE" ControlToValidate="txtDATE" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
		</td>
	</tr>
	<tr>
		<td nowrap><span class="dateFormat"><asp:Label ID="lblDATEFORMAT" Runat="server" /></span></td>
		<td nowrap><span class="dateFormat"><asp:Label ID="lblTIMEFORMAT" Runat="server" /></span></td>
		<td nowrap>&nbsp;</td>
		<td nowrap>&nbsp;</td>
	</tr>
</table>
