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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Emails
{
	/// <summary>
	/// Summary description for DetailView.
	/// </summary>
	public class DetailView : SplendidControl
	{
		protected _controls.ModuleHeader  ctlModuleHeader ;
		protected _controls.DetailButtons ctlDetailButtons;

		protected Guid        gID              ;
		protected HtmlTable   tblMain          ;
		protected PlaceHolder plcSubPanel      ;
		protected Repeater    ctlAttachments   ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Edit" )
				{
					Response.Redirect("edit.aspx?ID=" + gID.ToString());
				}
				else if ( e.CommandName == "Duplicate" )
				{
					Response.Redirect("edit.aspx?DuplicateID=" + gID.ToString());
				}
				else if ( e.CommandName == "Delete" )
				{
					SqlProcs.spEMAILS_Delete(gID);
					Response.Redirect("default.aspx");
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				ctlDetailButtons.ErrorText = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "view") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				gID = Sql.ToGuid(Request["ID"]);
				// 11/28/2005 Paul.  We must always populate the table, otherwise it will disappear during event processing. 
				//if ( !IsPostBack )
				{
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *            " + ControlChars.CrLf
							     + "  from vwEMAILS_Edit" + ControlChars.CrLf
							     + " where ID = @ID     " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@ID", gID);
								con.Open();
#if DEBUG
								Page.RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
#endif
								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										ctlModuleHeader.Title = Sql.ToString(rdr["NAME"]);
										Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
										Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
										
										this.AppendDetailViewFields(m_sMODULE + ".DetailView", tblMain, rdr);
										
										// 11/17/2005 Paul.  Archived emails allow editing of the Date & Time Sent. 
										string sEMAIL_TYPE = Sql.ToString(rdr["TYPE"]).ToLower();
										ctlModuleHeader.EnableModuleLabel = false;
										switch ( sEMAIL_TYPE )
										{
											case "archived":
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_ARCHIVED_MODULE_NAME") + ":" + ctlModuleHeader.Title;
												break;
											case "out":
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_LIST_FORM_SENT_TITLE") + ":" + ctlModuleHeader.Title;
												// 01/21/2006 Paul.  Sent emails cannot be edited or duplicated. 
												ctlDetailButtons.ShowEdit      = false;
												ctlDetailButtons.ShowDuplicate = false;
												break;
											default:
												sEMAIL_TYPE = "draft";
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_COMPOSE_MODULE_NAME" ) + ":" + ctlModuleHeader.Title;
												ctlDetailButtons.ShowDuplicate = false;
												// 01/21/2006 Paul.  Draft messages go directly to edit mode. 
												Response.Redirect("edit.aspx?ID=" + gID.ToString());
												break;
										}
										ctlDetailButtons.SetUserAccess(m_sMODULE, Sql.ToGuid(rdr["ASSIGNED_USER_ID"]));
									}
								}
							}
							sSQL = "select *                   " + ControlChars.CrLf
							     + "  from vwEMAILS_Attachments" + ControlChars.CrLf
							     + " where EMAIL_ID = @EMAIL_ID" + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@EMAIL_ID", gID);
#if DEBUG
								Page.RegisterClientScriptBlock("vwEMAILS_Attachments", Sql.ClientScriptBlock(cmd));
#endif
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										ctlAttachments.DataSource = dt.DefaultView;
										ctlAttachments.DataBind();
									}
								}
							}
						}
					}
				}
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				ctlDetailButtons.ErrorText = ex.Message;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
			ctlDetailButtons.Command = new CommandEventHandler(Page_Command);
			m_sMODULE = "Emails";
			this.AppendDetailViewRelationships(m_sMODULE + ".DetailView", plcSubPanel);
		}
		#endregion
	}
}
