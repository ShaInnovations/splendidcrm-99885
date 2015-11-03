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

namespace SplendidCRM.Cases
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtNAME            ;
		protected TextBox      txtCASE_NUMBER     ;
		protected TextBox      txtACCOUNT_NAME    ;
		protected DropDownList lstSTATUS          ;
		protected DropDownList lstPRIORITY        ;
		protected ListBox      lstASSIGNED_USER_ID;

		public override void ClearForm()
		{
			txtNAME            .Text    = String.Empty;
			txtCASE_NUMBER     .Text    = String.Empty;
			txtACCOUNT_NAME    .Text    = String.Empty;
			lstSTATUS          .SelectedIndex = 0;
			lstPRIORITY        .SelectedIndex = 0;
			lstASSIGNED_USER_ID.SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, Sql.ToInteger(txtCASE_NUMBER.Text), "CASE_NUMBER", Sql.IsEmptyString(txtCASE_NUMBER.Text));
			Sql.AppendParameter(cmd, txtNAME        .Text         , 255, Sql.SqlFilterMode.StartsWith, "NAME"        );
			Sql.AppendParameter(cmd, txtACCOUNT_NAME.Text         , 100, Sql.SqlFilterMode.StartsWith, "ACCOUNT_NAME");
			Sql.AppendParameter(cmd, lstSTATUS      .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "STATUS"      );
			Sql.AppendParameter(cmd, lstPRIORITY    .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "PRIORITY"    );
			Sql.AppendGuids    (cmd, lstASSIGNED_USER_ID, "ASSIGNED_USER_ID");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstSTATUS          .DataSource = SplendidCache.List("case_status_dom");
				lstSTATUS          .DataBind();
				lstSTATUS          .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstPRIORITY        .DataSource = SplendidCache.List("case_priority_dom");
				lstPRIORITY        .DataBind();
				lstPRIORITY        .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
