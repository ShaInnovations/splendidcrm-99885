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

namespace SplendidCRM.Contacts
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtFIRST_NAME        ;
		protected TextBox      txtPHONE             ;
		protected TextBox      txtLAST_NAME         ;
		protected TextBox      txtEMAIL             ;
		protected TextBox      txtACCOUNT_NAME      ;
		protected TextBox      txtASSISTANT         ;
		protected TextBox      txtADDRESS_STREET    ;
		protected TextBox      txtADDRESS_CITY      ;
		protected TextBox      txtADDRESS_STATE     ;
		protected TextBox      txtADDRESS_POSTALCODE;
		protected TextBox      txtADDRESS_COUNTRY   ;
		protected CheckBox     chkDO_NOT_CALL       ;
		protected CheckBox     chkEMAIL_OPT_OUT     ;
		protected DropDownList lstLEAD_SOURCE       ;
		protected ListBox      lstASSIGNED_USER_ID  ;

		public override void ClearForm()
		{
			txtFIRST_NAME        .Text    = String.Empty;
			txtPHONE             .Text    = String.Empty;
			txtLAST_NAME         .Text    = String.Empty;
			txtEMAIL             .Text    = String.Empty;
			txtACCOUNT_NAME      .Text    = String.Empty;
			txtASSISTANT         .Text    = String.Empty;
			txtADDRESS_STREET    .Text    = String.Empty;
			txtADDRESS_CITY      .Text    = String.Empty;
			txtADDRESS_STATE     .Text    = String.Empty;
			txtADDRESS_POSTALCODE.Text    = String.Empty;
			txtADDRESS_COUNTRY   .Text    = String.Empty;
			chkDO_NOT_CALL       .Checked = false;
			chkEMAIL_OPT_OUT     .Checked = false;
			lstLEAD_SOURCE       .SelectedIndex = 0;
			lstASSIGNED_USER_ID  .SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, txtFIRST_NAME        .Text         ,  25, Sql.SqlFilterMode.StartsWith, "FIRST_NAME"    );
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtPHONE             .Text         ,  25, Sql.SqlFilterMode.StartsWith, new string[] {"PHONE_HOME", "PHONE_MOBILE", "PHONE_WORK", "PHONE_OTHER", "PHONE_FAX", "ASSISTANT_PHONE"} );
			Sql.AppendParameter(cmd, txtLAST_NAME         .Text         ,  25, Sql.SqlFilterMode.StartsWith, "LAST_NAME"     );
			Sql.AppendParameter(cmd, txtEMAIL             .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"EMAIL1", "EMAIL2"} );
			Sql.AppendParameter(cmd, txtACCOUNT_NAME      .Text         , 150, Sql.SqlFilterMode.StartsWith, "ACCOUNT_NAME"  );
			Sql.AppendParameter(cmd, txtASSISTANT         .Text         ,  75, Sql.SqlFilterMode.StartsWith, "ASSISTANT"     );
			Sql.AppendParameter(cmd, txtADDRESS_STREET    .Text         , 150, Sql.SqlFilterMode.StartsWith, new string[] {"PRIMARY_ADDRESS_STREET"    , "ALT_ADDRESS_STREET"    } );
			Sql.AppendParameter(cmd, txtADDRESS_CITY      .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"PRIMARY_ADDRESS_CITY"      , "ALT_ADDRESS_CITY"      } );
			Sql.AppendParameter(cmd, txtADDRESS_STATE     .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"PRIMARY_ADDRESS_STATE"     , "ALT_ADDRESS_STATE"     } );
			Sql.AppendParameter(cmd, txtADDRESS_POSTALCODE.Text         ,  20, Sql.SqlFilterMode.StartsWith, new string[] {"PRIMARY_ADDRESS_POSTALCODE", "ALT_ADDRESS_POSTALCODE"} );
			Sql.AppendParameter(cmd, txtADDRESS_COUNTRY   .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"PRIMARY_ADDRESS_COUNTRY"   , "ALT_ADDRESS_COUNTRY"   } );
			Sql.AppendParameter(cmd, chkDO_NOT_CALL       .Checked      , "DO_NOT_CALL"  );
			Sql.AppendParameter(cmd, chkEMAIL_OPT_OUT     .Checked      , "EMAIL_OPT_OUT");
			Sql.AppendParameter(cmd, lstLEAD_SOURCE       .SelectedValue, 100, Sql.SqlFilterMode.Exact     , "LEAD_SOURCE"  );
			Sql.AppendGuids    (cmd, lstASSIGNED_USER_ID                , "ASSIGNED_USER_ID");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstLEAD_SOURCE     .DataSource = SplendidCache.List("lead_source_dom");
				lstLEAD_SOURCE     .DataBind();
				lstLEAD_SOURCE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
