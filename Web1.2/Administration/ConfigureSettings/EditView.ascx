<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Administration.ConfigureSettings.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<%@ Register TagPrefix="SplendidCRM" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" EnablePrint="false" EnableHelp="true" Runat="Server" />
	<p>
	<%@ Register TagPrefix="SplendidCRM" Tagname="EditButtons" Src="~/_controls/EditButtons.ascx" %>
	<SplendidCRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" Runat="Server" />
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th align="left" class="dataLabel" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Administration.LBL_NOTIFY_TITLE") %></h4></th>
					</tr>
					<tr>
						<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_NOTIFY_FROMNAME") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td width="30%" class="dataField">
							<asp:TextBox ID="txtNOTIFY_FROMNAME" TabIndex="1" size="25" MaxLength="128" Runat="server" />
						</td>
						<td width="15%" class="dataLabel" valign="top"><%= L10n.Term("Administration.LBL_NOTIFY_ON") %></td>
						<td width="40%" class="dataField" valign="top">
							<asp:CheckBox ID="chkNOTIFY_ON" TabIndex="1" CssClass="checkbox" Runat="server" />
							<em><%= L10n.Term("Administration.LBL_NOTIFICATION_ON_DESC") %></em>
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Administration.LBL_NOTIFY_FROMADDRESS") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
						<td class="dataField">
							<asp:TextBox ID="txtNOTIFY_FROMADDRESS" TabIndex="1" size="25" MaxLength="128" Runat="server" />
						</td>
						<td class="dataLabel"><%= L10n.Term("Administration.LBL_NOTIFY_SEND_BY_DEFAULT") %></td>
						<td class="dataField">
							<asp:CheckBox ID="chkNOTIFY_SEND_BY_DEFAULT" TabIndex="1" CssClass="checkbox" Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SENDTYPE") %></td>
						<td class="dataField">
							<asp:DropDownList ID="lstMAIL_SENDTYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" onChange="notify_setrequired(document.ConfigureSettings);" Runat="server" />
						</td>
						<td class="dataLabel"></td>
						<td class="dataField"></td>
					</tr>
					<tr>
						<td colspan="4">
							<div ID="smtp_settings">
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr>
										<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SMTPSERVER") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
										<td width="35%" class="dataField">
											<asp:TextBox ID="txtMAIL_SMTPSERVER" TabIndex="1" size="25" MaxLength="64" Runat="server" />
										</td>
										<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SMTPPORT") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
										<td width="35%" class="dataField">
											<asp:TextBox ID="txtMAIL_SMTPPORT" TabIndex="1" size="5" MaxLength="5" Runat="server" />
										</td>
									</tr>
									<tr>
										<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SMTPAUTH_REQ") %></td>
										<td colspan="3">
											<asp:CheckBox ID="chkMAIL_SMTPAUTH_REQ" TabIndex="1" CssClass="checkbox" onclick="notify_setrequired(document.ConfigureSettings);" Runat="server" />
										</td>
									</tr>
									<tr>
										<td colspan="4">
											<div ID="smtp_auth">
												<table width="100%" cellpadding="0" cellspacing="0">
													<tr>
														<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SMTPUSER") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
														<td width="35%" class="dataField">
															<asp:TextBox ID="txtMAIL_SMTPUSER" TabIndex="1" size="25" MaxLength="64" Runat="server" />
														</td>
														<td width="15%" class="dataLabel"><%= L10n.Term("Administration.LBL_MAIL_SMTPPASS") %> <asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></td>
														<td width="35%" class="dataField">
															<asp:TextBox ID="txtMAIL_SMTPPASS" TextMode="Password" TabIndex="1" size="25" MaxLength="64" Runat="server" />
														</td>
													</tr>
												</table>
											</div>
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<th align="left" class="dataLabel" colspan="4"><h4 class="dataLabel"><%= L10n.Term("Administration.LBL_PORTAL_TITLE") %></h4></th>
					</tr>
					<tr>
						<td width="25%" class="dataLabel" valign="middle"><%= L10n.Term("Administration.LBL_PORTAL_ON") %></td>
						<td width="75%" align="left" class="dataField" valign="middle">
							<asp:CheckBox ID="chkPORTAL_ON" TabIndex="1" CssClass="checkbox" Runat="server" />
							<em><%= L10n.Term("Administration.LBL_PORTAL_ON_DESC") %></em>
						</td>
					</tr>
					<tr>
						<td colspan="4">
							<div id="portal_config">
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr>
										<td width="15%" class="dataLabel">&nbsp;</td>
										<td width="35%" class="dataField">&nbsp;</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</p>
</div>
