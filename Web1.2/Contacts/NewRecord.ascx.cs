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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.Contacts
{
	/// <summary>
	///		Summary description for New.
	/// </summary>
	public class NewRecord : SplendidControl
	{
		protected Label                      lblError     ;
		protected TextBox                    txtFIRST_NAME;
		protected TextBox                    txtLAST_NAME ;
		protected TextBox                    txtPHONE_WORK;
		protected TextBox                    txtEMAIL1    ;
		protected RequiredFieldValidator     reqLAST_NAME ;
		protected RegularExpressionValidator reqPHONE_WORK;
		protected RegularExpressionValidator reqEMAIL1    ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "NewRecord" )
			{
				reqLAST_NAME .Enabled = true;
				//reqPHONE_WORK.Enabled = true;  // 07/16/2005 Paul.  Phone is not currently validated. 
				reqEMAIL1    .Enabled = true;
				reqLAST_NAME .Validate();
				reqPHONE_WORK.Validate();
				reqEMAIL1    .Validate();
				if ( Page.IsValid )
				{
					Guid gID = Guid.Empty;
					try
					{
						SqlProcs.spCONTACTS_New(ref gID, txtFIRST_NAME.Text, txtLAST_NAME.Text, txtPHONE_WORK.Text, txtEMAIL1.Text);
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
						lblError.Text = ex.Message;
					}
					if ( !Sql.IsEmptyGuid(gID) )
						Response.Redirect("~/Contacts/view.aspx?ID=" + gID.ToString());
				}
				else
				{
					reqEMAIL1.ErrorMessage = L10n.Term("Contacts.LBL_INVALID_EMAIL") + " " + txtEMAIL1.Text + "<br>";
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 06/04/2006 Paul.  NewRecord should not be displayed if the user does not have edit rights. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//this.DataBind();  // Need to bind so that Text of the Button gets updated. 
			reqLAST_NAME.ErrorMessage = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Contacts.LBL_LIST_LAST_NAME") + "<br>";
			reqEMAIL1.ErrorMessage = L10n.Term("Contacts.LBL_INVALID_EMAIL") + "<br>";
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
			m_sMODULE = "Contacts";
		}
		#endregion
	}
}
