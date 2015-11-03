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

namespace SplendidCRM.Leads
{
	/// <summary>
	///		Summary description for SearchLeads.
	/// </summary>
	public class SearchLeads : SplendidControl
	{
		protected SplendidCRM._controls.ListHeader ctlListHeader;

		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;

		public static string UnifiedSearch(string sUnifiedSearch, IDbCommand cmd)
		{
			string sSQL = String.Empty;
			SearchBuilder sb = new SearchBuilder(sUnifiedSearch, cmd);
			sSQL += sb.BuildQuery("   and ", "NAME"        );
			//sSQL += sb.BuildQuery("    or ", "LAST_NAME"   );
			//sSQL += sb.BuildQuery("    or ", "FIRST_NAME"  );
			sSQL += sb.BuildQuery("    or ", "ACCOUNT_NAME");
			sSQL += sb.BuildQuery("    or ", "EMAIL1"      );
			sSQL += sb.BuildQuery("    or ", "EMAIL2"      );
			if ( Information.IsNumeric(sUnifiedSearch) )
			{
				sSQL += sb.BuildQuery("    or ", "PHONE_HOME"  );
				sSQL += sb.BuildQuery("    or ", "PHONE_MOBILE");
				sSQL += sb.BuildQuery("    or ", "PHONE_WORK"  );
				sSQL += sb.BuildQuery("    or ", "PHONE_OTHER" );
				sSQL += sb.BuildQuery("    or ", "PHONE_FAX"   );
			}
			return sSQL;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//Page.DataBind();
			string sUnifiedSearch = Sql.ToString(Request["txtUnifiedSearch"]);
			if ( !Sql.IsEmptyString(sUnifiedSearch.Trim()) )
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					sSQL = "select *           " + ControlChars.CrLf
					     + "  from vwLEADS_List" + ControlChars.CrLf
					     + " where 1 = 1       " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL + UnifiedSearch(sUnifiedSearch, cmd);
#if DEBUG
						Page.RegisterClientScriptBlock("vwLEADS_List", Sql.ClientScriptBlock(cmd));
#endif
						try
						{
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									// 08/01/2005 Paul.  Convert the term here so that sorting will apply. 
									foreach(DataRow row in dt.Rows)
									{
										// 08/17/2005 Paul.  Don't convert if NULL.
										row["STATUS"] = L10n.Term(".lead_status_dom.", row["STATUS"]);
									}
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
						catch(Exception ex)
						{
							SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
							lblError.Text = ex.Message;
						}
					}
				}
				ctlListHeader.Visible = true;
			}
			else
			{
				ctlListHeader.Visible = false;
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
