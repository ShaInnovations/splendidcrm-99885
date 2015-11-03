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
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SplendidCRM._controls
{
	/// <summary>
	///		Summary description for Footer.
	/// </summary>
	public class Footer : SplendidControl
	{
		protected Image        imgFooterSugarCRM;
		protected PlaceHolder  phFooterMenu     ;
		protected DropDownList lstTHEME         ;
		protected DropDownList lstLANGUAGE      ;
		protected HtmlTableRow trFooterMenu     ;
		protected HtmlTable    tblTheme         ;

		protected void lstTHEME_Changed(Object sender, EventArgs e)
		{
			SplendidInit.ChangeTheme(lstTHEME.SelectedValue, lstLANGUAGE.SelectedValue);
			Response.Redirect(Request.RawUrl);
		}

		protected void lstLANGUAGE_Changed(Object sender, EventArgs e)
		{
			SplendidInit.ChangeTheme(lstTHEME.SelectedValue, lstLANGUAGE.SelectedValue);
			Response.Redirect(Request.RawUrl);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			imgFooterSugarCRM.DataBind();

			string sSeparator = "  ";
			DataTable dt = SplendidCache.TabMenu();
			// 04/28/2006 Paul.  Hide the footer menu if there is no menu to display. 
			if ( dt.Rows.Count == 0 )
			{
				trFooterMenu.Visible = false;
				tblTheme    .Visible = false;
			}
			foreach(DataRow row in dt.Rows)
			{
				Literal litSeparator = new Literal();
				litSeparator.Text = sSeparator;
				phFooterMenu.Controls.Add(litSeparator);
				
				HyperLink lnk = new HyperLink();
				lnk.ID          = "lnkFooter" + Sql.ToString(row["DISPLAY_NAME"]) ;
				lnk.NavigateUrl = Sql.ToString(row["RELATIVE_PATH"]);
				lnk.Text        = L10n.Term(Sql.ToString(row["DISPLAY_NAME"]));
				lnk.CssClass    = "footerLink";
				phFooterMenu.Controls.Add(lnk);
				
				sSeparator = "\r\n| ";
			}
			// 04/28/2006 Paul.  No need to populate the lists if they are not going to be displayed. 
			if ( !IsPostBack && dt.Rows.Count > 0 )
			{
				lstLANGUAGE.DataSource = SplendidCache.Languages();
				lstLANGUAGE.DataBind();
				lstTHEME.DataSource = SplendidCache.Themes();
				lstTHEME.DataBind();

				try
				{
					lstTHEME.SelectedValue = Sql.ToString(HttpContext.Current.Session["USER_SETTINGS/THEME"]);
				}
				catch
				{
				}
				try
				{
					lstLANGUAGE.SelectedValue = Sql.ToString(HttpContext.Current.Session["USER_SETTINGS/CULTURE"]);
				}
				catch
				{
				}

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
