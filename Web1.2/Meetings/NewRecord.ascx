<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="SplendidCRM.Meetings.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divNewRecord">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="leftColumnModuleHead">
		<tr>
			<th width="5"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_left.gif" %>'  BorderWidth="0" Width="5" Height="21" Runat="server" /></th>
			<th style="background-image : url(<%= Sql.ToString(Session["themeURL"]) %>images/moduleTab_middle.gif);" width="100%"><%= L10n.Term("Meetings.LBL_NEW_FORM_TITLE") %></th>
			<th width="9"><asp:Image ImageUrl='<%# Session["themeURL"] + "images/moduleTab_right.gif" %>' BorderWidth="0" Width="9" Height="21" Runat="server" /></th>
		</tr>
	</table>
	<table width="100%" cellpadding="3" cellspacing="0" border="0">
		<tr>
			<td align="left" class="leftColumnModuleS3">
				<asp:RadioButton ID="radScheduleCall"    GroupName="grpSchedule" class="radio"                Runat="server" /><span class="dataLabel"><%= L10n.Term("Calls.LNK_NEW_CALL"   ) %></span><br>
				<asp:RadioButton ID="radScheduleMeeting" GroupName="grpSchedule" class="radio" Checked="true" Runat="server" /><span class="dataLabel"><%= L10n.Term("Calls.LNK_NEW_MEETING") %></span><br>
				<p>
				<%= L10n.Term("Meetings.LBL_SUBJECT") %><asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /><br>
				<asp:TextBox ID="txtNAME" size="25" MaxLength="50" Runat="server" /><br>
				<%= L10n.Term("Meetings.LBL_DATE") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />&nbsp;<span class="dateFormat"><asp:Label ID="lblDATEFORMAT" Runat="server" /></span><br>
				<%@ Register TagPrefix="SplendidCRM" Tagname="DatePicker" Src="~/_controls/DatePicker.ascx" %>
				<SplendidCRM:DatePicker ID="ctlDATE_START" Runat="Server" />
				<%= L10n.Term("Meetings.LBL_TIME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" />&nbsp;<span class="dateFormat"><asp:Label ID="lblTIMEFORMAT" Runat="server" /></span><br>
				<asp:TextBox ID="txtTIME_START" size="15" MaxLength="10" Runat="server" /><br>
				</p>
				<p><asp:Button ID="btnSave" CommandName="NewRecord" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_SAVE_BUTTON_LABEL"  ) + "  " %>' title='<%# L10n.Term(".LBL_SAVE_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SAVE_BUTTON_KEY") %>' Runat="server" /></p>
				<asp:RequiredFieldValidator                           ID="reqNAME"       ControlToValidate="txtNAME"       CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
				<CustomValidators:RequiredFieldValidatorForDatePicker ID="reqDATE_START" ControlToValidate="ctlDATE_START" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
				<asp:RequiredFieldValidator                           ID="reqTIME_START" ControlToValidate="txtTIME_START" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
				<CustomValidators:DatePickerValidator                 ID="valDATE_START" ControlToValidate="ctlDATE_START" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
				<CustomValidators:TimeValidator                       ID="valTIME_START" ControlToValidate="txtTIME_START" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID          , btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(ctlDATE_START.DateClientID, btnSave.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtTIME_START.ClientID    , btnSave.ClientID) %>
</div>
