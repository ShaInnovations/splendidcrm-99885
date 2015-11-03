<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="SplendidCRM.Administration.DynamicLayout.EditViews.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<SplendidCRM:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="DynamicLayout.LBL_EDIT_VIEW_LAYOUT" EnablePrint="false" EnableHelp="true" Runat="Server" />

	<%@ Register TagPrefix="SplendidCRM" Tagname="SearchBasic" Src="../_controls/SearchBasic.ascx" %>
	<SplendidCRM:SearchBasic ID="ctlSearch" ViewTableName="vwEDITVIEWS_Layout" ViewFieldName="EDIT_NAME" Runat="Server" />
	<br>

	<%@ Register TagPrefix="SplendidCRM" Tagname="LayoutButtons" Src="../_controls/LayoutButtons.ascx" %>
	<SplendidCRM:LayoutButtons ID="ctlLayoutButtons" Visible="<%# !PrintView %>" Runat="Server" />

	<table ID="tblMain" width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm" runat="server">
	</table>
</div>
