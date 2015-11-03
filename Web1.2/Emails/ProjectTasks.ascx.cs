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
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Emails
{
	/// <summary>
	///		Summary description for ProjectTasks.
	/// </summary>
	public class ProjectTasks : SplendidControl
	{
		protected Guid            gID            ;
		protected DataView        vwMain         ;
		protected SplendidGrid    grdMain        ;
		protected Label           lblError       ;
		protected HtmlInputHidden txtPROJECT_TASK_ID  ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				switch ( e.CommandName )
				{
					case "ProjectTasks.Create":
						Response.Redirect("~/ProjectTasks/edit.aspx?PARENT_ID=" + gID.ToString());
						break;
					case "ProjectTasks.Edit":
					{
						Guid gPROJECT_TASK_ID = Sql.ToGuid(e.CommandArgument);
						Response.Redirect("~/ProjectTasks/edit.aspx?ID=" + gPROJECT_TASK_ID.ToString());
						break;
					}
					case "ProjectTasks.Remove":
					{
						Guid gPROJECT_TASK_ID = Sql.ToGuid(e.CommandArgument);
						SqlProcs.spEMAILS_PROJECT_TASKS_Delete(gID, gPROJECT_TASK_ID);
						Response.Redirect("view.aspx?ID=" + gID.ToString());
						break;
					}
					default:
						throw(new Exception("Unknown command: " + e.CommandName));
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			gID = Sql.ToGuid(Request["ID"]);
			Guid gPROJECT_TASK_ID = Sql.ToGuid(txtPROJECT_TASK_ID.Value);
			if ( !Sql.IsEmptyGuid(gPROJECT_TASK_ID) )
			{
				try
				{
					SqlProcs.spEMAILS_PROJECT_TASKS_Update(gID, gPROJECT_TASK_ID);
					Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					lblError.Text = ex.Message;
				}
			}

			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL;
				sSQL = "select *                     " + ControlChars.CrLf
				     + "  from vwEMAILS_PROJECT_TASKS" + ControlChars.CrLf
				     + " where EMAIL_ID = @EMAIL_ID  " + ControlChars.CrLf
				     + " order by DATE_ENTERED asc   " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@EMAIL_ID", gID);
#if DEBUG
					Page.RegisterClientScriptBlock("vwEMAILS_PROJECT_TASKS", Sql.ClientScriptBlock(cmd));
#endif
					try
					{
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
								// 09/05/2005 Paul. LinkButton controls will not fire an event unless the the grid is bound. 
								//if ( !IsPostBack )
								{
									grdMain.DataBind();
								}
							}
						}
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
						lblError.Text = ex.Message;
					}
				}
			}
			if ( !IsPostBack )
			{
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
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
			// 11/26/2005 Paul.  Add fields early so that sort events will get called. 
			grdMain.DynamicColumns("Emails.ProjectTasks");
		}
		#endregion
	}
}
