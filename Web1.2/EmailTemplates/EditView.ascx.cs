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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;
using FredCK.FCKeditorV2;

namespace SplendidCRM.EmailTemplates
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.ModuleHeader ctlModuleHeader;
		protected _controls.EditButtons  ctlEditButtons ;

		protected Guid            gID                          ;
		protected TextBox         txtNAME                      ;
		protected TextBox         txtDESCRIPTION               ;
		protected DropDownList    lstVariableModule            ;
		protected DropDownList    lstVariableName              ;
		protected TextBox         txtVariableText              ;
		protected TextBox         txtSUBJECT                   ;
		protected FCKeditor       txtBODY                      ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "EMAIL_MARKETING";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								SqlProcs.spEMAIL_TEMPLATES_Update
									( ref gID
									, false  // 11/17/2005 Paul.  The PUBLISH flag is no longer used in SugarCRM 3.5.0B
									, txtNAME.Text
									, txtDESCRIPTION.Text
									, txtSUBJECT.Text
									, String.Empty   // BODY
									, txtBODY.Value  // BODY_HTML
									, trn
									);
								SplendidDynamic.UpdateCustomFields(this, trn, gID, sCUSTOM_MODULE, dtCustomFields);
								trn.Commit();
							}
							catch(Exception ex)
							{
								trn.Rollback();
								SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
								ctlEditButtons.ErrorText = ex.Message;
								return;
							}
						}
					}
					Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				if ( Sql.IsEmptyGuid(gID) )
					Response.Redirect("default.aspx");
				else
					Response.Redirect("view.aspx?ID=" + gID.ToString());
			}
		}

		protected void lstVariableModule_Changed(Object sender, EventArgs e)
		{
			lstVariableName.Items.Clear();
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *                       " + ControlChars.CrLf
				     + "  from vwSqlColumns            " + ControlChars.CrLf
				     + " where ObjectName = @ObjectName" + ControlChars.CrLf
				     + " order by colid                " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ObjectName", "vw" + lstVariableModule.SelectedValue.ToUpper());
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sValue  = Sql.ToString(rdr["ColumnName"]);
							string sText   = L10n.Term(lstVariableModule.SelectedValue + ".LBL_" + sValue.ToUpper());
							string sModule = lstVariableModule.SelectedValue;
							if ( sModule == "Contacts" )
								sModule = "contact";
							else if ( sModule == "Accounts" )
								sModule = "account";
							sText = sText.Replace(":", "");
							lstVariableName.Items.Add(new ListItem(sText, sModule + "_" + sValue.ToLower()));
						}
					}
				}
			}
			txtVariableText.Text = "$" + lstVariableName.Items[0].Value;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					lstVariableModule.Items.Add(new ListItem(L10n.Term(".LBL_ACCOUNT"                         ), "Accounts"));
					lstVariableModule.Items.Add(new ListItem(L10n.Term("EmailTemplates.LBL_CONTACT_AND_OTHERS"), "Contacts"));
					lstVariableModule_Changed(null, null);

					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                     " + ControlChars.CrLf
							     + "  from vwEMAIL_TEMPLATES_Edit" + ControlChars.CrLf
							     + " where ID = @ID              " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								if ( !Sql.IsEmptyGuid(gDuplicateID) )
								{
									Sql.AddParameter(cmd, "@ID", gDuplicateID);
									gID = Guid.Empty;
								}
								else
								{
									Sql.AddParameter(cmd, "@ID", gID);
								}
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
										ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

										// 03/04/2006 Paul.  Name was not being set. 
										txtNAME       .Text  = Sql.ToString(rdr["NAME"       ]);
										txtDESCRIPTION.Text  = Sql.ToString(rdr["DESCRIPTION"]);
										txtSUBJECT    .Text  = Sql.ToString(rdr["SUBJECT"    ]);
										// 04/21/2006 Paul.  Change BODY to BODY_HTML. 
										txtBODY       .Value = Sql.ToString(rdr["BODY_HTML"  ]);
									}
								}
							}
						}
					}
				}
				else
				{
					// 12/02/2005 Paul.  When validation fails, the header title does not retain its value.  Update manually. 
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				ctlEditButtons.ErrorText = ex.Message;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This Meeting is required by the ASP.NET Web Form Designer.
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
			ctlEditButtons.Command = new CommandEventHandler(Page_Command);
			m_sMODULE = "EmailTemplates";
		}
		#endregion
	}
}
