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
	///		Summary description for SearchBasic.
	/// </summary>
	public class SearchBasic : SearchControl
	{
		protected TextBox  txtNAME             ;
		protected TextBox  txtCITY             ;
		protected TextBox  txtWEBSITE          ;
		protected TextBox  txtPHONE            ;
		protected CheckBox chkCURRENT_USER_ONLY;

		public override void ClearForm()
		{
			txtNAME             .Text    = String.Empty;
			txtCITY             .Text    = String.Empty;
			txtWEBSITE          .Text    = String.Empty;
			txtPHONE            .Text    = String.Empty;
			chkCURRENT_USER_ONLY.Checked = false;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, txtNAME   .Text, 150, Sql.SqlFilterMode.StartsWith, "NAME"   );
			Sql.AppendParameter(cmd, txtCITY   .Text, 100, Sql.SqlFilterMode.StartsWith, "CITY"   );
			Sql.AppendParameter(cmd, txtWEBSITE.Text, 255, Sql.SqlFilterMode.StartsWith, "WEBSITE");
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtPHONE  .Text,  25, Sql.SqlFilterMode.StartsWith, "PHONE"  );
			if ( chkCURRENT_USER_ONLY.Checked )
			{
				Sql.AppendParameter(cmd, Security.USER_ID, "ASSIGNED_USER_ID", false);
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
