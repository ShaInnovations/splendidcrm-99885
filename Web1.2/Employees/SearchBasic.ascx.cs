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

namespace SplendidCRM.Employees
{
	/// <summary>
	///		Summary description for SearchBasic.
	/// </summary>
	public class SearchBasic : SearchControl
	{
		protected TextBox      txtFIRST_NAME       ;
		protected TextBox      txtLAST_NAME        ;
		protected TextBox      txtDEPARTMENT       ;
		protected DropDownList lstEMPLOYEE_STATUS  ;

		public override void ClearForm()
		{
			txtFIRST_NAME       .Text    = String.Empty;
			txtLAST_NAME        .Text    = String.Empty;
			txtDEPARTMENT       .Text    = String.Empty;
			lstEMPLOYEE_STATUS.SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			Sql.AppendParameter(cmd, txtFIRST_NAME     .Text         ,  25, Sql.SqlFilterMode.StartsWith, "FIRST_NAME"     );
			Sql.AppendParameter(cmd, txtLAST_NAME      .Text         ,  25, Sql.SqlFilterMode.StartsWith, "LAST_NAME"      );
			Sql.AppendParameter(cmd, txtDEPARTMENT     .Text         ,  50, Sql.SqlFilterMode.StartsWith, "DEPARTMENT"     );
			Sql.AppendParameter(cmd, lstEMPLOYEE_STATUS.SelectedValue,  25, Sql.SqlFilterMode.Exact     , "EMPLOYEE_STATUS");
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstEMPLOYEE_STATUS.DataSource = SplendidCache.List("employee_status_dom");
				lstEMPLOYEE_STATUS.DataBind();
				lstEMPLOYEE_STATUS.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
