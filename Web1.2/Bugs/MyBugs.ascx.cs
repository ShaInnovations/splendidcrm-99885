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

namespace SplendidCRM.Bugs
{
	/// <summary>
	///		Summary description for MyBugs.
	/// </summary>
	public class MyBugs : SplendidControl
	{
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL;
				sSQL = "select *                                   " + ControlChars.CrLf
				     + "  from vwBUGS_MyList                       " + ControlChars.CrLf
				     + " where ASSIGNED_USER_ID = @ASSIGNED_USER_ID" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);
#if DEBUG
					Page.RegisterClientScriptBlock("vwBUGS_List", Sql.ClientScriptBlock(cmd));
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
								// 11/26/2005 Paul.  List conversion now occurs in the dynamic column of the grid. 
								/*
								foreach(DataRow row in dt.Rows)
								{
									// 08/17/2005 Paul.  Don't convert if NULL.
									row["STATUS"  ] = L10n.Term(".bug_status_dom."  , row["STATUS"  ]);
									row["TYPE"    ] = L10n.Term(".bug_type_dom."    , row["TYPE"    ]);
									row["PRIORITY"] = L10n.Term(".bug_priority_dom.", row["PRIORITY"]);
								}
								*/
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
								if ( !IsPostBack )
								{
									grdMain.SortColumn = "DATE_ENTERED";
									grdMain.SortOrder  = "desc" ;
									grdMain.ApplySort();
								}
								// 09/15/2005 Paul. We must always bind, otherwise a Dashboard refresh will display the grid with empty rows. 
								grdMain.DataBind();
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
			// 12/05/2005 Paul.  Add fields early so that sort events will get called. 
			grdMain.DynamicColumns("Bugs.MyBugs");
		}
		#endregion
	}
}
