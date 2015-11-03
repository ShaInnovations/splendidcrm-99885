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

namespace SplendidCRM.Products
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
		protected HtmlTable       tblCost                         ;
		protected HtmlTable       tblManufacturer                 ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			// 06/08/2006 Paul.  Redirect to parent if that is where the note was originated. 
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
				TextBox         txtNAME                = FindControl("NAME") as TextBox;
				HtmlInputHidden txtPRODUCT_TEMPLATE_ID = FindControl("PRODUCT_TEMPLATE_ID") as HtmlInputHidden;
				// 07/05/2006 Paul.  The NAME field can be derived from a product template or manually entered. 
				// In order to fake-out the normal required-field mechanism, assign the empty Guid to the template ID. 
				// The name cannot be null, so if it is, then clear the template ID. 
				if ( Sql.IsEmptyString(txtNAME.Text.Trim()) )
					txtPRODUCT_TEMPLATE_ID.Value = String.Empty;
				else if ( Sql.IsEmptyString(txtPRODUCT_TEMPLATE_ID.Value) )
					txtPRODUCT_TEMPLATE_ID.Value = Guid.Empty.ToString();
				
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditView", this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".CostView", this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".MftView" , this);
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "PRODUCTS";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								SqlProcs.spPRODUCTS_Update
									( ref gID
									, new DynamicControl(this, "PRODUCT_TEMPLATE_ID"           ).ID
									, new DynamicControl(this, "NAME"                          ).Text
									, new DynamicControl(this, "STATUS"                        ).SelectedValue
									, new DynamicControl(this, "ACCOUNT_ID"                    ).ID
									, new DynamicControl(this, "CONTACT_ID"                    ).ID
									, new DynamicControl(this, "QUANTITY"                      ).IntegerValue
									, new DynamicControl(this, "DATE_PURCHASED"                ).DateValue
									, new DynamicControl(this, "DATE_SUPPORT_EXPIRES"          ).DateValue
									, new DynamicControl(this, "DATE_SUPPORT_STARTS"           ).DateValue
									, new DynamicControl(this, "MANUFACTURER_ID"               ).ID
									, new DynamicControl(this, "CATEGORY_ID"                   ).ID
									, new DynamicControl(this, "TYPE_ID"                       ).ID
									, new DynamicControl(this, "WEBSITE"                       ).Text
									, new DynamicControl(this, "MFT_PART_NUM"                  ).Text
									, new DynamicControl(this, "VENDOR_PART_NUM"               ).Text
									, new DynamicControl(this, "SERIAL_NUMBER"                 ).Text
									, new DynamicControl(this, "ASSET_NUMBER"                  ).Text
									, new DynamicControl(this, "TAX_CLASS"                     ).SelectedValue
									, new DynamicControl(this, "WEIGHT"                        ).FloatValue
									, new DynamicControl(this, "CURRENCY_ID"                   ).ID
									, new DynamicControl(this, "COST_PRICE"                    ).DecimalValue
									, new DynamicControl(this, "LIST_PRICE"                    ).DecimalValue
									, new DynamicControl(this, "BOOK_VALUE"                    ).DecimalValue
									, new DynamicControl(this, "BOOK_VALUE_DATE"               ).DateValue
									, new DynamicControl(this, "DISCOUNT_PRICE"                ).DecimalValue
									, new DynamicControl(this, "PRICING_FACTOR"                ).IntegerValue
									, new DynamicControl(this, "PRICING_FORMULA"               ).SelectedValue
									, new DynamicControl(this, "SUPPORT_NAME"                  ).Text
									, new DynamicControl(this, "SUPPORT_CONTACT"               ).Text
									, new DynamicControl(this, "SUPPORT_DESCRIPTION"           ).Text
									, new DynamicControl(this, "SUPPORT_TERM"                  ).SelectedValue
									, new DynamicControl(this, "DESCRIPTION"                   ).Text
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
							// 08/10/2006 Paul.  Need to filter on ID, not 1 = 1.
							sSQL = "select *              " + ControlChars.CrLf
							     + "  from vwPRODUCTS_Edit" + ControlChars.CrLf
							     + " where ID = @ID       " + ControlChars.CrLf;
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
										
										this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain        , rdr);
										this.AppendEditViewFields(m_sMODULE + ".CostView", tblCost        , rdr);
										this.AppendEditViewFields(m_sMODULE + ".MftView" , tblManufacturer, rdr);
										// 07/05/2006 Paul.  The Product Name should be editable. 
										TextBox txtNAME = FindControl("NAME") as TextBox;
										if ( txtNAME != null )
											txtNAME.ReadOnly = false;
									}
								}
							}
						}
					}
					else
					{
						this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain        , null);
						this.AppendEditViewFields(m_sMODULE + ".CostView", tblCost        , null);
						this.AppendEditViewFields(m_sMODULE + ".MftView" , tblManufacturer, null);
						// 07/05/2006 Paul.  The Product Name should be editable. 
						TextBox txtNAME = FindControl("NAME") as TextBox;
						if ( txtNAME != null )
							txtNAME.ReadOnly = false;
						
						// 06/08/2006 Paul.  Prepopulate the Account. 
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
							else if ( !Sql.IsEmptyGuid(gPARENT_ID) && sMODULE == "Contacts" )
							{
								new DynamicControl(this, "CONTACT_ID"  ).ID   = gPARENT_ID;
								new DynamicControl(this, "CONTACT_NAME").Text = sPARENT_NAME;
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
			m_sMODULE = "Products";
			if ( IsPostBack )
			{
				// 12/02/2005 Paul.  Need to add the edit fields in order for events to fire. 
				this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain        , null);
				this.AppendEditViewFields(m_sMODULE + ".CostView", tblCost        , null);
				this.AppendEditViewFields(m_sMODULE + ".MftView" , tblManufacturer, null);
			}
		}
		#endregion
	}
}
