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
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Quotes
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
		protected DropDownList    CURRENCY_ID                     ;
		protected DropDownList    TAXRATE_ID                      ;
		protected DropDownList    SHIPPER_ID                      ;
		protected CheckBox        SHOW_LINE_NUMS                  ;
		protected CheckBox        CALC_GRAND_TOTAL                ;


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
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditView"       , this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditAddress"    , this);
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditDescription", this);
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "QUOTES";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								SqlProcs.spQUOTES_Update
									( ref gID
									, new DynamicControl(this, "ASSIGNED_USER_ID"              ).ID
									, new DynamicControl(this, "NAME"                          ).Text
									, new DynamicControl(this, "OPPORTUNITY_ID"                ).ID
									, new DynamicControl(this, "QUOTE_TYPE"                    ).SelectedValue
									, new DynamicControl(this, "PAYMENT_TERMS"                 ).SelectedValue
									, new DynamicControl(this, "ORDER_STAGE"                   ).SelectedValue
									, new DynamicControl(this, "QUOTE_STAGE"                   ).SelectedValue
									, new DynamicControl(this, "PURCHASE_ORDER_NUM"            ).Text
									, new DynamicControl(this, "ORIGINAL_PO_DATE"              ).DateValue
									, new DynamicControl(this, "DATE_QUOTE_CLOSED"             ).DateValue
									, new DynamicControl(this, "DATE_QUOTE_EXPECTED_CLOSED"    ).DateValue
									, new DynamicControl(this, "DATE_ORDER_SHIPPED"            ).DateValue
									, new DynamicControl(this, "SHOW_LINE_NUMS"                ).Checked
									, new DynamicControl(this, "CALC_GRAND_TOTAL"              ).Checked
									, new DynamicControl(this, "CURRENCY_ID"                   ).ID
									, new DynamicControl(this, "TAXRATE_ID"                    ).ID
									, new DynamicControl(this, "SHIPPER_ID"                    ).ID
									, new DynamicControl(this, "SUBTOTAL"                      ).DecimalValue
									, new DynamicControl(this, "SHIPPING"                      ).DecimalValue
									, new DynamicControl(this, "TAX"                           ).DecimalValue
									, new DynamicControl(this, "TOTAL"                         ).DecimalValue
									, new DynamicControl(this, "BILLING_ACCOUNT_ID"            ).ID
									, new DynamicControl(this, "BILLING_CONTACT_ID"            ).ID
									, new DynamicControl(this, "BILLING_ADDRESS_STREET"        ).Text
									, new DynamicControl(this, "BILLING_ADDRESS_CITY"          ).Text
									, new DynamicControl(this, "BILLING_ADDRESS_STATE"         ).Text
									, new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE"    ).Text
									, new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"       ).Text
									, new DynamicControl(this, "SHIPPING_ACCOUNT_ID"           ).ID
									, new DynamicControl(this, "SHIPPING_CONTACT_ID"           ).ID
									, new DynamicControl(this, "SHIPPING_ADDRESS_STREET"       ).Text
									, new DynamicControl(this, "SHIPPING_ADDRESS_CITY"         ).Text
									, new DynamicControl(this, "SHIPPING_ADDRESS_STATE"        ).Text
									, new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE"   ).Text
									, new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"      ).Text
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

		private void UpdateAccount(Guid gACCOUNT_ID, bool bUpdateBilling, bool bUpdateShipping)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *         " + ControlChars.CrLf
				     + "  from vwACCOUNTS" + ControlChars.CrLf
				     + " where ID = @ID  " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ID", gACCOUNT_ID);
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							if ( bUpdateBilling )
							{
								new DynamicControl(this, "BILLING_ACCOUNT_ID"         ).ID   = Sql.ToGuid  (rdr["ID"                         ]);
								new DynamicControl(this, "BILLING_ACCOUNT_NAME"       ).Text = Sql.ToString(rdr["NAME"                       ]);
								new DynamicControl(this, "BILLING_ADDRESS_STREET"     ).Text = Sql.ToString(rdr["BILLING_ADDRESS_STREET"     ]);
								new DynamicControl(this, "BILLING_ADDRESS_CITY"       ).Text = Sql.ToString(rdr["BILLING_ADDRESS_CITY"       ]);
								new DynamicControl(this, "BILLING_ADDRESS_STATE"      ).Text = Sql.ToString(rdr["BILLING_ADDRESS_STATE"      ]);
								new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE" ).Text = Sql.ToString(rdr["BILLING_ADDRESS_POSTALCODE" ]);
								new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"    ).Text = Sql.ToString(rdr["BILLING_ADDRESS_COUNTRY"    ]);
							}
							if ( bUpdateShipping )
							{
								new DynamicControl(this, "SHIPPING_ACCOUNT_ID"        ).ID   = Sql.ToGuid  (rdr["ID"                         ]);
								new DynamicControl(this, "SHIPPING_ACCOUNT_NAME"      ).Text = Sql.ToString(rdr["NAME"                       ]);
								new DynamicControl(this, "SHIPPING_ADDRESS_STREET"    ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_STREET"    ]);
								new DynamicControl(this, "SHIPPING_ADDRESS_CITY"      ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_CITY"      ]);
								new DynamicControl(this, "SHIPPING_ADDRESS_STATE"     ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_STATE"     ]);
								new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").Text = Sql.ToString(rdr["SHIPPING_ADDRESS_POSTALCODE"]);
								new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"   ).Text = Sql.ToString(rdr["SHIPPING_ADDRESS_COUNTRY"   ]);
							}
						}
					}
				}
			}
		}

		private void UpdateContact(Guid gCONTACT_ID, bool bUpdateBilling, bool bUpdateShipping)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *         " + ControlChars.CrLf
				     + "  from vwCONTACTS" + ControlChars.CrLf
				     + " where ID = @ID  " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ID", gCONTACT_ID);
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							if ( bUpdateBilling )
							{
								new DynamicControl(this, "BILLING_CONTACT_ID"   ).ID   = Sql.ToGuid  (rdr["ID"  ]);
								new DynamicControl(this, "BILLING_CONTACT_NAME" ).Text = Sql.ToString(rdr["NAME"]);
							}
							if ( bUpdateShipping )
							{
								new DynamicControl(this, "SHIPPING_CONTACT_ID"  ).ID   = Sql.ToGuid  (rdr["ID"  ]);
								new DynamicControl(this, "SHIPPING_CONTACT_NAME").Text = Sql.ToString(rdr["NAME"]);
							}
							Guid gACCOUNT_ID = Sql.ToGuid("ACCOUNT_ID");
							if ( !Sql.IsEmptyGuid(gACCOUNT_ID) )
							{
								UpdateAccount(gACCOUNT_ID, bUpdateBilling, bUpdateShipping);
							}
						}
					}
				}
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
					CURRENCY_ID.DataSource = SplendidCache.Currencies();
					CURRENCY_ID.DataBind();
					TAXRATE_ID .DataSource = SplendidCache.TaxRates();
					TAXRATE_ID .DataBind();
					TAXRATE_ID .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
					SHIPPER_ID .DataSource = SplendidCache.Shippers();
					SHIPPER_ID .DataBind();
					SHIPPER_ID.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
					
					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *            " + ControlChars.CrLf
							     + "  from vwQUOTES_Edit" + ControlChars.CrLf
							     + " where ID = @ID     " + ControlChars.CrLf;
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
										ViewState["BILLING_ACCOUNT_ID" ] = Sql.ToGuid(rdr["BILLING_ACCOUNT_ID" ]);
										ViewState["SHIPPING_ACCOUNT_ID"] = Sql.ToGuid(rdr["SHIPPING_ACCOUNT_ID"]);
										
										this.AppendEditViewFields(m_sMODULE + ".EditView"       , tblMain       , rdr);
										this.AppendEditViewFields(m_sMODULE + ".EditAddress"    , tblAddress    , rdr);
										this.AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, rdr);
										
										new DynamicControl(this, "SHOW_LINE_NUMS"  ).Checked = Sql.ToBoolean(rdr["SHOW_LINE_NUMS"  ]);
										new DynamicControl(this, "CALC_GRAND_TOTAL").Checked = Sql.ToBoolean(rdr["CALC_GRAND_TOTAL"]);
										try
										{
											new DynamicControl(this, "CURRENCY_ID").SelectedValue = Sql.ToString(rdr["CURRENCY_ID"]);
										}
										catch
										{
										}
										try
										{
											new DynamicControl(this, "TAXRATE_ID").SelectedValue = Sql.ToString(rdr["TAXRATE_ID"]);
										}
										catch
										{
										}
										try
										{
											new DynamicControl(this, "SHIPPER_ID" ).SelectedValue = Sql.ToString(rdr["SHIPPER_ID"]);
										}
										catch
										{
										}
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
								UpdateAccount(gPARENT_ID, true, true);
							}
							if ( !Sql.IsEmptyGuid(gPARENT_ID) && sMODULE == "Contacts" )
							{
								UpdateContact(gPARENT_ID, true, true);
							}
							else if ( !Sql.IsEmptyGuid(gPARENT_ID) && sMODULE == "Opportunities" )
							{
								new DynamicControl(this, "OPPORTUNITY_ID"   ).ID   = gPARENT_ID;
								new DynamicControl(this, "OPPORTUNITY_NAME" ).Text = sPARENT_NAME;
							}
						}
					}
				}
				else
				{
					// 12/02/2005 Paul.  When validation fails, the header title does not retain its value.  Update manually. 
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);

					DynamicControl ctlBILLING_ACCOUNT_ID  = new DynamicControl(this, "BILLING_ACCOUNT_ID" );
					DynamicControl ctlSHIPPING_ACCOUNT_ID = new DynamicControl(this, "SHIPPING_ACCOUNT_ID");
					if ( Sql.ToGuid(ViewState["BILLING_ACCOUNT_ID" ]) != ctlBILLING_ACCOUNT_ID.ID )
					{
						UpdateAccount(ctlBILLING_ACCOUNT_ID.ID, true, true);
						ViewState["BILLING_ACCOUNT_ID" ] = ctlBILLING_ACCOUNT_ID.ID;
						ViewState["SHIPPING_ACCOUNT_ID"] = ctlBILLING_ACCOUNT_ID.ID;
					}
					if ( Sql.ToGuid(ViewState["SHIPPING_ACCOUNT_ID"]) != ctlSHIPPING_ACCOUNT_ID.ID )
					{
						UpdateAccount(ctlSHIPPING_ACCOUNT_ID.ID, false, true);
						ViewState["SHIPPING_ACCOUNT_ID"] = ctlSHIPPING_ACCOUNT_ID.ID;
					}

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
			m_sMODULE = "Quotes";
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
