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
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI.WebControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Quotes
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtNAME              ;
		protected TextBox      txtQUOTE_NUM         ;
		protected TextBox      txtACCOUNT_NAME      ;
		protected TextBox      txtTOTAL             ;
		protected TextBox      txtNEXT_STEP         ;
		protected DropDownList lstQUOTE_TYPE        ;
		protected DropDownList lstLEAD_SOURCE       ;
		protected DropDownList lstQUOTE_STAGE       ;
		protected ListBox      lstASSIGNED_USER_ID  ;
		protected _controls.DatePicker ctlDATE_QUOTE_EXPECTED_CLOSED;

		public override void ClearForm()
		{
			txtNAME            .Text    = String.Empty;
			txtQUOTE_NUM       .Text    = String.Empty;
			txtACCOUNT_NAME    .Text    = String.Empty;
			txtTOTAL           .Text    = String.Empty;
			txtNEXT_STEP       .Text    = String.Empty;
			lstQUOTE_TYPE      .SelectedIndex = 0;
			lstLEAD_SOURCE     .SelectedIndex = 0;
			lstQUOTE_STAGE     .SelectedIndex = 0;
			lstASSIGNED_USER_ID.SelectedIndex = 0;
			ctlDATE_QUOTE_EXPECTED_CLOSED.DateText = String.Empty;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, Sql.ToInteger(txtQUOTE_NUM.Text), "QUOTE_NUM", Sql.IsEmptyString(txtQUOTE_NUM.Text));
			Sql.AppendParameter(cmd, Sql.ToDecimal(txtTOTAL    .Text), "TOTAL"    , Sql.IsEmptyString(txtTOTAL    .Text));
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtNAME            .Text         ,  50, Sql.SqlFilterMode.StartsWith, "NAME"        );
			Sql.AppendParameter(cmd, txtACCOUNT_NAME    .Text         , 150, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ACCOUNT_NAME", "BILLING_ACCOUNT_NAME"} );
			Sql.AppendParameter(cmd, txtNEXT_STEP       .Text         , 255, Sql.SqlFilterMode.StartsWith, "NEXT_STEP"   );
			Sql.AppendParameter(cmd, lstQUOTE_TYPE      .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "QUOTE_TYPE"  );
			Sql.AppendParameter(cmd, lstLEAD_SOURCE     .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "LEAD_SOURCE" );
			Sql.AppendParameter(cmd, lstQUOTE_STAGE     .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "QUOTE_STAGE" );
			// 07/09/2006 Paul.  Date is no longer converted in the DatePicker control, so convert it here to server time. 
			Sql.AppendParameter(cmd, T10n.ToServerTime(ctlDATE_QUOTE_EXPECTED_CLOSED.Value), "DATE_QUOTE_EXPECTED_CLOSED");
			Sql.AppendGuids    (cmd, lstASSIGNED_USER_ID, "ASSIGNED_USER_ID");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstQUOTE_TYPE     .DataSource = SplendidCache.List("quote_type_dom");
				lstQUOTE_TYPE     .DataBind();
				lstQUOTE_TYPE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstQUOTE_STAGE    .DataSource = SplendidCache.List("quote_stage_dom");
				lstQUOTE_STAGE    .DataBind();
				lstQUOTE_STAGE    .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstLEAD_SOURCE    .DataSource = SplendidCache.List("lead_source_dom");
				lstLEAD_SOURCE    .DataBind();
				lstLEAD_SOURCE    .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstASSIGNED_USER_ID.DataSource = SplendidCache.AssignedUser();
				lstASSIGNED_USER_ID.DataBind();
				// 06/03/2004 Paul.  A Multiple-line ListBox does not need a NULL entry. 
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
