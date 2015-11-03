<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ListHeader.ascx.cs" Inherits="SplendidCRM._controls.ListHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
	<table border="0" cellspacing="0" cellpadding="0" width="100%">
		<tbody>
			<tr>
				<td nowrap>
					<h3><asp:Image ImageUrl='<%# Session["themeURL"] + "images/h3Arrow.gif" %>' Height="11" Width="11" BorderWidth="0" Runat="server" />&nbsp;<%= L10n.Term(sTitle) %></h3>
				</td>
				<td width="100%"><asp:Image Height="1" Width="1" AlternateText="" ImageUrl="~/Include/images/blank.gif"></td>
			</tr>
		</tbody>
	</table>
