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

namespace SplendidCRM.Calls
{
	/// <summary>
	///		Summary description for ListView.
	/// </summary>
	public class ListView : SplendidControl
	{
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;
		protected PlaceHolder   plcSearch      ;
		protected SearchControl ctlSearch      ;
		protected int           nAdvanced      ;
		protected MassUpdate    ctlMassUpdate  ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Clear" )
				{
					ctlSearch.ClearForm();
					Server.Transfer("default.aspx?Advanced=" + nAdvanced.ToString());
				}
				else if ( e.CommandName == "Search" )
				{
					// 10/13/2005 Paul.  Make sure to clear the page index prior to applying search. 
					grdMain.CurrentPageIndex = 0;
					grdMain.ApplySort();
					grdMain.DataBind();
				}
				else if ( e.CommandName == "MassUpdate" )
				{
					string[] arrID = Request.Form.GetValues("chkMain");
					if ( arrID != null )
					{
						string sIDs = Utils.ValidateIDs(arrID);
						sIDs = Utils.FilterByACL(m_sMODULE, "edit", arrID, "CALLS");
						if ( !Sql.IsEmptyString(sIDs) )
						{
							// 07/09/2006 Paul.  The date conversion was moved out of the MassUpdate control. 
							SqlProcs.spCALLS_MassUpdate(sIDs, ctlMassUpdate.ASSIGNED_USER_ID, T10n.ToServerTime(ctlMassUpdate.DATE_START), ctlMassUpdate.STATUS, ctlMassUpdate.DIRECTION);
							Response.Redirect("default.aspx");
						}
					}
				}
				else if ( e.CommandName == "MassDelete" )
				{
					string[] arrID = Request.Form.GetValues("chkMain");
					if ( arrID != null )
					{
						string sIDs = Utils.ValidateIDs(arrID);
						sIDs = Utils.FilterByACL(m_sMODULE, "delete", arrID, "CALLS");
						if ( !Sql.IsEmptyString(sIDs) )
						{
							SqlProcs.spCALLS_MassDelete(sIDs);
							Response.Redirect("default.aspx");
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

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term(m_sMODULE + ".LBL_LIST_FORM_TITLE"));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "list") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					sSQL = "select *           " + ControlChars.CrLf
					     + "  from vwCALLS_List" + ControlChars.CrLf
					     + " where 1 = 1       " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						int nACLACCESS = Security.GetUserAccess(m_sMODULE, "list");
						if ( nACLACCESS == ACL_ACCESS.OWNER )
							Sql.AppendParameter(cmd, Security.USER_ID, "ASSIGNED_USER_ID", false);
						ctlSearch.SqlSearchClause(cmd);
#if DEBUG
						Page.RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
#endif
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								// 08/01/2005 Paul.  Convert the term here so that sorting will apply. 
								// 11/26/2005 Paul.  List conversion now occurs in the dynamic column of the grid. 
								/*
								foreach(DataRow row in dt.Rows)
								{
									// 08/17/2005 Paul.  Don't convert if NULL.
									row["STATUS"   ] = L10n.Term(".call_status_dom."   , row["STATUS"   ]);
									row["DIRECTION"] = L10n.Term(".call_direction_dom.", row["DIRECTION"]);
								}
								*/
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
								if ( !IsPostBack )
								{
									grdMain.SortColumn = "NAME";
									grdMain.SortOrder  = "asc" ;
									grdMain.ApplySort();
									grdMain.DataBind();
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
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
			m_sMODULE = "Calls";
			grdMain.DynamicColumns(m_sMODULE + ".ListView");
			// We have to load the control in here, otherwise the control will not initialized before the Page_Load above. 
			ctlSearch = (SearchControl) LoadControl("SearchBasic.ascx");
			plcSearch.Controls.Add(ctlSearch);
			ctlSearch.Command = new CommandEventHandler(Page_Command);
			ctlMassUpdate.Command = new CommandEventHandler(Page_Command);
			// 05/02/2006 Paul.  Hide the MassUpdate control if the user cannot make changes. 
			if ( Security.GetUserAccess(m_sMODULE, "delete") < 0 && Security.GetUserAccess(m_sMODULE, "edit") < 0 )
				ctlMassUpdate.Visible = false;

		}
		#endregion
	}
}
