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

namespace SplendidCRM.Accounts
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtNAME              ;
		protected TextBox      txtPHONE             ;
		protected TextBox      txtWEBSITE           ;
		protected TextBox      txtEMAIL             ;
		protected TextBox      txtANNUAL_REVENUE    ;
		protected TextBox      txtEMPLOYEES         ;
		protected TextBox      txtOWNERSHIP         ;
		protected TextBox      txtTICKER_SYMBOL     ;
		protected TextBox      txtRATING            ;
		protected TextBox      txtSIC_CODE          ;
		protected TextBox      txtADDRESS_STREET    ;
		protected TextBox      txtADDRESS_CITY      ;
		protected TextBox      txtADDRESS_STATE     ;
		protected TextBox      txtADDRESS_POSTALCODE;
		protected TextBox      txtADDRESS_COUNTRY   ;
		protected DropDownList lstINDUSTRY          ;
		protected DropDownList lstACCOUNT_TYPE      ;
		protected ListBox      lstASSIGNED_USER_ID  ;

		public override void ClearForm()
		{
			txtNAME              .Text    = String.Empty;
			txtPHONE             .Text    = String.Empty;
			txtWEBSITE           .Text    = String.Empty;
			txtEMAIL             .Text    = String.Empty;
			txtANNUAL_REVENUE    .Text    = String.Empty;
			txtEMPLOYEES         .Text    = String.Empty;
			txtOWNERSHIP         .Text    = String.Empty;
			txtTICKER_SYMBOL     .Text    = String.Empty;
			txtRATING            .Text    = String.Empty;
			txtSIC_CODE          .Text    = String.Empty;
			txtADDRESS_STREET    .Text    = String.Empty;
			txtADDRESS_CITY      .Text    = String.Empty;
			txtADDRESS_STATE     .Text    = String.Empty;
			txtADDRESS_POSTALCODE.Text    = String.Empty;
			txtADDRESS_COUNTRY   .Text    = String.Empty;
			lstINDUSTRY        .SelectedIndex = 0;
			lstACCOUNT_TYPE    .SelectedIndex = 0;
			lstASSIGNED_USER_ID.SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, txtNAME              .Text         , 150, Sql.SqlFilterMode.StartsWith, "NAME"          );
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtPHONE             .Text         ,  25, Sql.SqlFilterMode.StartsWith, new string[] {"PHONE_OFFICE"  , "PHONE_FAX", "PHONE_ALTERNATE"} );
			Sql.AppendParameter(cmd, txtWEBSITE           .Text         , 255, Sql.SqlFilterMode.StartsWith, "WEBSITE"       );
			Sql.AppendParameter(cmd, txtEMAIL             .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"EMAIL1", "EMAIL2"} );
			Sql.AppendParameter(cmd, txtANNUAL_REVENUE    .Text         ,  25, Sql.SqlFilterMode.StartsWith, "ANNUAL_REVENUE");
			Sql.AppendParameter(cmd, txtEMPLOYEES         .Text         ,  10, Sql.SqlFilterMode.StartsWith, "EMPLOYEES"     );
			Sql.AppendParameter(cmd, txtOWNERSHIP         .Text         , 100, Sql.SqlFilterMode.StartsWith, "OWNERSHIP"     );
			Sql.AppendParameter(cmd, txtTICKER_SYMBOL     .Text         ,  10, Sql.SqlFilterMode.StartsWith, "TICKER_SYMBOL" );
			Sql.AppendParameter(cmd, txtRATING            .Text         ,  25, Sql.SqlFilterMode.StartsWith, "RATING"        );
			Sql.AppendParameter(cmd, txtSIC_CODE          .Text         ,  10, Sql.SqlFilterMode.StartsWith, "SIC_CODE"      );
			Sql.AppendParameter(cmd, txtADDRESS_STREET    .Text         , 150, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ADDRESS_STREET"    , "BILLING_ADDRESS_STREET"    } );
			Sql.AppendParameter(cmd, txtADDRESS_CITY      .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ADDRESS_CITY"      , "BILLING_ADDRESS_CITY"      } );
			Sql.AppendParameter(cmd, txtADDRESS_STATE     .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ADDRESS_STATE"     , "BILLING_ADDRESS_STATE"     } );
			Sql.AppendParameter(cmd, txtADDRESS_POSTALCODE.Text         ,  20, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ADDRESS_POSTALCODE", "BILLING_ADDRESS_POSTALCODE"} );
			Sql.AppendParameter(cmd, txtADDRESS_COUNTRY   .Text         , 100, Sql.SqlFilterMode.StartsWith, new string[] {"SHIPPING_ADDRESS_COUNTRY"   , "BILLING_ADDRESS_COUNTRY"   } );
			Sql.AppendParameter(cmd, lstINDUSTRY          .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "INDUSTRY"      );
			Sql.AppendParameter(cmd, lstACCOUNT_TYPE      .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "ACCOUNT_TYPE"  );
			Sql.AppendGuids    (cmd, lstASSIGNED_USER_ID                , "ASSIGNED_USER_ID");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstINDUSTRY        .DataSource = SplendidCache.List("industry_dom");
				lstINDUSTRY        .DataBind();
				lstINDUSTRY        .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstACCOUNT_TYPE    .DataSource = SplendidCache.List("account_type_dom");
				lstACCOUNT_TYPE    .DataBind();
				lstACCOUNT_TYPE    .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
