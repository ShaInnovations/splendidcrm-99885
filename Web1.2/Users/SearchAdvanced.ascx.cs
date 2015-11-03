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

namespace SplendidCRM.Users
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtFIRST_NAME        ;
		protected TextBox      txtLAST_NAME         ;
		protected TextBox      txtUSER_NAME         ;
		protected TextBox      txtPHONE             ;
		protected TextBox      txtEMAIL             ;
		protected TextBox      txtTITLE             ;
		protected TextBox      txtDEPARTMENT        ;
		protected TextBox      txtADDRESS_STREET    ;
		protected TextBox      txtADDRESS_CITY      ;
		protected TextBox      txtADDRESS_STATE     ;
		protected TextBox      txtADDRESS_POSTALCODE;
		protected TextBox      txtADDRESS_COUNTRY   ;
		protected CheckBox     chkIS_ADMIN          ;
		protected DropDownList lstSTATUS            ;

		public override void ClearForm()
		{
			txtFIRST_NAME        .Text    = String.Empty;
			txtLAST_NAME         .Text    = String.Empty;
			txtUSER_NAME         .Text    = String.Empty;
			txtPHONE             .Text    = String.Empty;
			txtEMAIL             .Text    = String.Empty;
			txtTITLE             .Text    = String.Empty;
			txtDEPARTMENT        .Text    = String.Empty;
			txtADDRESS_STREET    .Text    = String.Empty;
			txtADDRESS_CITY      .Text    = String.Empty;
			txtADDRESS_STATE     .Text    = String.Empty;
			txtADDRESS_POSTALCODE.Text    = String.Empty;
			txtADDRESS_COUNTRY   .Text    = String.Empty;
			chkIS_ADMIN.Checked = false;
			lstSTATUS  .SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, txtFIRST_NAME        .Text         ,  30, Sql.SqlFilterMode.StartsWith, "FIRST_NAME"        );
			Sql.AppendParameter(cmd, txtLAST_NAME         .Text         ,  30, Sql.SqlFilterMode.StartsWith, "LAST_NAME"         );
			Sql.AppendParameter(cmd, txtUSER_NAME         .Text         ,  20, Sql.SqlFilterMode.StartsWith, "USER_NAME"         );
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtPHONE             .Text         ,  50, Sql.SqlFilterMode.StartsWith, new string[] {"PHONE_HOME", "PHONE_MOBILE", "PHONE_WORK", "PHONE_OTHER", "PHONE_FAX"} );
			Sql.AppendParameter(cmd, txtEMAIL             .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"EMAIL1", "EMAIL2"} );
			Sql.AppendParameter(cmd, txtTITLE             .Text         ,  50, Sql.SqlFilterMode.StartsWith, "TITLE"             );
			Sql.AppendParameter(cmd, txtDEPARTMENT        .Text         ,  75, Sql.SqlFilterMode.StartsWith, "DEPARTMENT"        );
			Sql.AppendParameter(cmd, txtADDRESS_STREET    .Text         , 150, Sql.SqlFilterMode.StartsWith, "ADDRESS_STREET"    );
			Sql.AppendParameter(cmd, txtADDRESS_CITY      .Text         , 100, Sql.SqlFilterMode.StartsWith, "ADDRESS_CITY"      );
			Sql.AppendParameter(cmd, txtADDRESS_STATE     .Text         , 100, Sql.SqlFilterMode.StartsWith, "ADDRESS_STATE"     );
			Sql.AppendParameter(cmd, txtADDRESS_POSTALCODE.Text         ,  20, Sql.SqlFilterMode.StartsWith, "ADDRESS_POSTALCODE");
			Sql.AppendParameter(cmd, txtADDRESS_COUNTRY   .Text         , 100, Sql.SqlFilterMode.StartsWith, "ADDRESS_COUNTRY"   );
			Sql.AppendParameter(cmd, lstSTATUS            .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "STATUS"            );
			Sql.AppendParameter(cmd, chkIS_ADMIN          .Checked      , "IS_ADMIN"  );
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstSTATUS.DataSource = SplendidCache.List("user_status_dom");
				lstSTATUS.DataBind();
				lstSTATUS.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
