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

namespace SplendidCRM.Opportunities
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtNAME              ;
		protected TextBox      txtAMOUNT            ;
		protected TextBox      txtACCOUNT_NAME      ;
		protected TextBox      txtDATE_CLOSED       ;
		protected TextBox      txtNEXT_STEP         ;
		protected TextBox      txtPROBABILITY       ;
		protected DropDownList lstOPPORTUNITY_TYPE  ;
		protected DropDownList lstLEAD_SOURCE       ;
		protected DropDownList lstSALES_STAGE       ;
		protected ListBox      lstASSIGNED_USER_ID  ;

		public override void ClearForm()
		{
			txtNAME              .Text    = String.Empty;
			txtAMOUNT            .Text    = String.Empty;
			txtACCOUNT_NAME      .Text    = String.Empty;
			txtDATE_CLOSED       .Text    = String.Empty;
			txtNEXT_STEP         .Text    = String.Empty;
			txtPROBABILITY       .Text    = String.Empty;
			lstOPPORTUNITY_TYPE  .SelectedIndex = 0;
			lstLEAD_SOURCE       .SelectedIndex = 0;
			lstSALES_STAGE       .SelectedIndex = 0;
			lstASSIGNED_USER_ID  .SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			// 09/13/2006 Paul.  Change FIRST_NAME to NAME. 
			Sql.AppendParameter(cmd, txtNAME            .Text         ,  25, Sql.SqlFilterMode.StartsWith, "NAME"      );
			Sql.AppendParameter(cmd, Sql.ToDecimal (txtAMOUNT     .Text), "AMOUNT"     , Sql.IsEmptyString(txtAMOUNT.Text));
			Sql.AppendParameter(cmd, T10n.ToServerTime(Sql.ToDateTime(txtDATE_CLOSED.Text)), "DATE_CLOSED");
			Sql.AppendParameter(cmd, txtNEXT_STEP       .Text         ,  25, Sql.SqlFilterMode.StartsWith, "NEXT_STEP"       );
			Sql.AppendParameter(cmd, txtACCOUNT_NAME    .Text         , 150, Sql.SqlFilterMode.StartsWith, "ACCOUNT_NAME"    );
			// 09/01/2006 Paul.  Add PROBABILITY. 
			Sql.AppendParameter(cmd, Sql.ToFloat   (txtPROBABILITY.Text), "PROBABILITY", Sql.IsEmptyString(txtPROBABILITY.Text));
			Sql.AppendParameter(cmd, lstOPPORTUNITY_TYPE.SelectedValue,  25, Sql.SqlFilterMode.Exact     , "OPPORTUNITY_TYPE");
			Sql.AppendParameter(cmd, lstLEAD_SOURCE     .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "LEAD_SOURCE"     );
			// 09/01/2006 Paul.  Change STATUS to SALES_STAGE. 
			Sql.AppendParameter(cmd, lstSALES_STAGE     .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "SALES_STAGE"     );
			Sql.AppendGuids    (cmd, lstASSIGNED_USER_ID              , "ASSIGNED_USER_ID");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstOPPORTUNITY_TYPE.DataSource = SplendidCache.List("opportunity_type_dom");
				lstOPPORTUNITY_TYPE.DataBind();
				lstOPPORTUNITY_TYPE.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstLEAD_SOURCE     .DataSource = SplendidCache.List("lead_source_dom");
				lstLEAD_SOURCE     .DataBind();
				lstLEAD_SOURCE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstSALES_STAGE     .DataSource = SplendidCache.List("sales_stage_dom");
				lstSALES_STAGE     .DataBind();
				lstSALES_STAGE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
