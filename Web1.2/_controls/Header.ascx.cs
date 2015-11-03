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
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SplendidCRM._controls
{
	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public class Header : SplendidControl
	{
		protected TabMenu       ctlTabMenu      ;
		protected Shortcuts     ctlShortcuts    ;
		protected PlaceHolder   plcNewRecord    ;
		protected bool          bEnableNewRecord;
		protected string        sNewRecord      ;
		protected HtmlTableCell tdShortcuts     ;
		protected HtmlTableCell tdUnifiedSearch1 ;
		protected HtmlTableCell tdUnifiedSearch2 ;
		protected HtmlTableCell tdUnifiedSearch3 ;
		protected LastViewed    ctlLastViewed   ;
		protected System.Web.UI.WebControls.Image imgCompanyLogo  ;
		
		public string ActiveTab
		{
			get
			{
				return ctlTabMenu.ActiveTab;
			}
			set
			{
				ctlShortcuts.SubMenu = value;
				ctlTabMenu.ActiveTab = value;
			}
		}

		public bool AdminShortcuts
		{
			get
			{
				return ctlShortcuts.AdminShortcuts;
			}
			set
			{
				ctlShortcuts.AdminShortcuts = value;
			}
		}

		public bool EnableShortcuts
		{
			get
			{
				return tdShortcuts.Visible;
			}
			set
			{
				tdShortcuts.Visible = false;
			}
		}

		public bool EnableLastViewed
		{
			get
			{
				return ctlLastViewed.Visible;
			}
			set
			{
				ctlLastViewed.Visible = false;
			}
		}

		public bool EnableNewRecord
		{
			get
			{
				return bEnableNewRecord;
			}
			set
			{
				bEnableNewRecord = value;
				plcNewRecord.Visible = bEnableNewRecord;
				// 01/04/2006 Paul.  Need to rebind in order to hide the CustomFields Add Field control. 
				plcNewRecord.DataBind();
			}
		}

		public string NewRecord
		{
			get
			{
				return sNewRecord;
			}
			set
			{
				sNewRecord = value;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
				// 04/28/2006 Paul.  If the user has not authenticated, then this must be during login.  Disable the search. 
				if ( Sql.IsEmptyGuid(Security.USER_ID) )
				{
					tdUnifiedSearch1.Visible = false;
					tdUnifiedSearch2.Visible = false;
					tdUnifiedSearch3.Visible = false;
				}
				// 04/16/2006 Paul.  Company logo can be customized. 
				if ( !Sql.IsEmptyString(Application["CONFIG.header_logo_image"]) )
				{
					imgCompanyLogo.ImageUrl = Sql.ToString(Application["imageURL"]) + Sql.ToString(Application["CONFIG.header_logo_image"]);
					if ( Sql.ToInteger(Application["CONFIG.header_logo_width"]) > 0 )
						imgCompanyLogo.Width    = Sql.ToInteger(Application["CONFIG.header_logo_width" ]);
					if ( Sql.ToInteger(Application["CONFIG.header_logo_height"]) > 0 )
						imgCompanyLogo.Height   = Sql.ToInteger(Application["CONFIG.header_logo_height"]);
					if ( !Sql.IsEmptyString(Application["CONFIG.header_logo_style"]) )
						imgCompanyLogo.Attributes.Add("style", Sql.ToString(Application["CONFIG.header_logo_style"]));
					imgCompanyLogo.AlternateText = L10n.Term(".COMPANY_LOGO");
				}
				else
				{
					imgCompanyLogo.ImageUrl = Sql.ToString(Application["imageURL"]) + "SplendidCRM_Logo.gif";
					imgCompanyLogo.Width  = 207;
					imgCompanyLogo.Height =  60;
					imgCompanyLogo.Attributes.Add("style", "margin-left: 10px");
					imgCompanyLogo.AlternateText = L10n.Term(".COMPANY_LOGO");
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
			// 01/08/2006 Paul.  Try loading the NewRecord control here.
			// The DynamicLayout manager is having a problem with viewstate when the NewRecord is visible. 
			// Otherwise, the code was working fine for many months when in Page_Load. 
			// 01/08/2006 Paul.  Moving the load hear did not solve the DynamicLayout problem, bit it is still a good idea. 
			// Dynamic controls should be created before Page_Load. 
			if ( !Sql.IsEmptyString(sNewRecord) )
			{
				Control ctl = LoadControl(sNewRecord);
				ctl.ID = "ctlNewRecord";
				plcNewRecord.Controls.Add(ctl);
			}
		}
		#endregion
	}
}
