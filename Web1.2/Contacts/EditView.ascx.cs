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

namespace SplendidCRM.Contacts
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.ModuleHeader ctlModuleHeader;
		protected _controls.EditButtons  ctlEditButtons ;

		protected Guid            gID                             ;
		protected HtmlTable       tblMain                         ;
		protected HtmlTable       tblAddress                      ;
		protected HtmlTable       tblDescription                  ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			// 08/21/2005 Paul.  Redirect to parent if that is where the note was originated. 
			Guid   gPARENT_ID   = Sql.ToGuid(Request["PARENT_ID"]);
			string sMODULE      = String.Empty;
			string sPARENT_TYPE = String.Empty;
			string sPARENT_NAME = String.Empty;
			try
			{
				SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				// The only possible error is a connection failure, so just ignore all errors. 
				gPARENT_ID = Guid.Empty;
			}
			if ( e.CommandName == "Save" )
			{
				// 01/16/2006 Paul.  Enable validator before validating page. 
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditView"       , this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditAddress"    , this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditDescription", this);
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "CONTACTS";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								SqlProcs.spCONTACTS_Update
									( ref gID
									, new DynamicControl(this, "ASSIGNED_USER_ID"          ).ID
									, new DynamicControl(this, "SALUTATION"                ).SelectedValue
									, new DynamicControl(this, "FIRST_NAME"                ).Text
									, new DynamicControl(this, "LAST_NAME"                 ).Text
									, new DynamicControl(this, "ACCOUNT_ID"                ).ID
									, new DynamicControl(this, "LEAD_SOURCE"               ).SelectedValue
									, new DynamicControl(this, "TITLE"                     ).Text
									, new DynamicControl(this, "DEPARTMENT"                ).Text
									, new DynamicControl(this, "REPORTS_TO_ID"             ).ID
									, new DynamicControl(this, "BIRTHDATE"                 ).DateValue
									, new DynamicControl(this, "DO_NOT_CALL"               ).Checked
									, new DynamicControl(this, "PHONE_HOME"                ).Text
									, new DynamicControl(this, "PHONE_MOBILE"              ).Text
									, new DynamicControl(this, "PHONE_WORK"                ).Text
									, new DynamicControl(this, "PHONE_OTHER"               ).Text
									, new DynamicControl(this, "PHONE_FAX"                 ).Text
									, new DynamicControl(this, "EMAIL1"                    ).Text
									, new DynamicControl(this, "EMAIL2"                    ).Text
									, new DynamicControl(this, "ASSISTANT"                 ).Text
									, new DynamicControl(this, "ASSISTANT_PHONE"           ).Text
									, new DynamicControl(this, "EMAIL_OPT_OUT"             ).Checked
									, new DynamicControl(this, "INVALID_EMAIL"             ).Checked
									, new DynamicControl(this, "PRIMARY_ADDRESS_STREET"    ).Text
									, new DynamicControl(this, "PRIMARY_ADDRESS_CITY"      ).Text
									, new DynamicControl(this, "PRIMARY_ADDRESS_STATE"     ).Text
									, new DynamicControl(this, "PRIMARY_ADDRESS_POSTALCODE").Text
									, new DynamicControl(this, "PRIMARY_ADDRESS_COUNTRY"   ).Text
									, new DynamicControl(this, "ALT_ADDRESS_STREET"        ).Text
									, new DynamicControl(this, "ALT_ADDRESS_CITY"          ).Text
									, new DynamicControl(this, "ALT_ADDRESS_STATE"         ).Text
									, new DynamicControl(this, "ALT_ADDRESS_POSTALCODE"    ).Text
									, new DynamicControl(this, "ALT_ADDRESS_COUNTRY"       ).Text
									, new DynamicControl(this, "DESCRIPTION"               ).Text
									, sMODULE
									, gPARENT_ID
									, new DynamicControl(this, "SYNC_CONTACT"              ).Checked
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
					if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
					else
						Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				if ( !Sql.IsEmptyGuid(gPARENT_ID) )
					Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
				else if ( Sql.IsEmptyGuid(gID) )
					Response.Redirect("default.aspx");
				else
					Response.Redirect("view.aspx?ID=" + gID.ToString());
			}
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
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							// 02/09/2006 Paul.  SugarCRM uses the CONTACTS_USERS table to allow each user to 
							// choose the contacts they want sync'd with Outlook. 
							// 02/09/2006 Paul.  Need to allow SYNC_USER_ID to be NULL, 
							// otherwise we will not get any results if the contact is not sync'd. 
							// 03/06/2006 Paul.  The join to CONTACTS_USERS must occur external to the view. 
							// This is the only way to ensure that the record is always returned, with the sync flag set. 
							// 04/23/2006 Paul.  Bug fix.  vwCONTACTS_USERS does not have an ID, use CONTACT_ID instead. 
							sSQL = "select vwCONTACTS_Edit.*                                                        " + ControlChars.CrLf
							     + "     , (case when vwCONTACTS_USERS.CONTACT_ID is null then 0 else 1 end) as SYNC_CONTACT" + ControlChars.CrLf
							     + "  from            vwCONTACTS_Edit                                               " + ControlChars.CrLf
							     + "  left outer join vwCONTACTS_USERS                                              " + ControlChars.CrLf
							     + "               on vwCONTACTS_USERS.CONTACT_ID = vwCONTACTS_Edit.ID              " + ControlChars.CrLf
							     + "              and vwCONTACTS_USERS.USER_ID    = @SYNC_USER_ID                   " + ControlChars.CrLf
							     + " where vwCONTACTS_Edit.ID = @ID                                                 " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@SYNC_USER_ID", Security.USER_ID);
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
										ctlModuleHeader.Title = Sql.ToString(rdr["SALUTATION"]) + " " + Sql.ToString(rdr["FIRST_NAME"]) + " " + Sql.ToString(rdr["LAST_NAME"]);
										Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
										Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
										ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;
										
										this.AppendEditViewFields(m_sMODULE + ".EditView"       , tblMain       , rdr);
										this.AppendEditViewFields(m_sMODULE + ".EditAddress"    , tblAddress    , rdr);
										this.AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, rdr);
									}
								}
							}
						}
					}
					else
					{
						this.AppendEditViewFields(m_sMODULE + ".EditView"       , tblMain       , null);
						this.AppendEditViewFields(m_sMODULE + ".EditAddress"    , tblAddress    , null);
						this.AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, null);
						// 03/27/2006 Paul.  Prepopulate the Account. 
						Guid gPARENT_ID = Sql.ToGuid(Request["PARENT_ID"]);
						if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						{
							string sMODULE      = String.Empty;
							string sPARENT_TYPE = String.Empty;
							string sPARENT_NAME = String.Empty;
							SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
							if ( !Sql.IsEmptyGuid(gPARENT_ID) && sMODULE == "Accounts" )
							{
								new DynamicControl(this, "ACCOUNT_ID"  ).ID   = gPARENT_ID;
								new DynamicControl(this, "ACCOUNT_NAME").Text = sPARENT_NAME;
							}
							// 06/09/2006 Paul.  If the parent is a contact, then that becomes the Reports To. 
							// This is a bug that exists in SugarCRM 4.2. 
							else if ( !Sql.IsEmptyGuid(gPARENT_ID) && sMODULE == "Contacts" )
							{
								new DynamicControl(this, "REPORTS_TO_ID"  ).ID   = gPARENT_ID;
								new DynamicControl(this, "REPORTS_TO_NAME").Text = sPARENT_NAME;
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
			ctlEditButtons.Command = new CommandEventHandler(Page_Command);
			m_sMODULE = "Contacts";
			if ( IsPostBack )
			{
				// 12/02/2005 Paul.  Need to add the edit fields in order for events to fire. 
				this.AppendEditViewFields(m_sMODULE + ".EditView"       , tblMain       , null);
				this.AppendEditViewFields(m_sMODULE + ".EditAddress"    , tblAddress    , null);
				this.AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, null);
			}
		}
		#endregion
	}
}
