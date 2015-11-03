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
using System.Web;
using System.Web.UI.WebControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Administration.ProductTemplates
{
	/// <summary>
	///		Summary description for SearchAdvanced.
	/// </summary>
	public class SearchAdvanced : SearchControl
	{
		protected TextBox      txtNAME              ;
		protected TextBox      txtMFT_PART_NUM      ;
		protected TextBox      txtVENDOR_PART_NUM   ;
		protected TextBox      txtSUPPORT_CONTACT   ;
		protected TextBox      txtWEBSITE           ;
		protected TextBox      txtSUPPORT_TERM      ;
		protected DropDownList lstCATEGORY          ;
		protected DropDownList lstTAX_CLASS         ;
		protected DropDownList lstSTATUS            ;
		protected DropDownList lstMANUFACTURER      ;
		protected DropDownList lstTYPE              ;
		protected _controls.DatePicker ctlDATE_COST_PRICE  ;
		protected _controls.DatePicker ctlDATE_AVAILABLE   ;

		public override void ClearForm()
		{
			txtNAME           .Text     = String.Empty;
			txtMFT_PART_NUM   .Text     = String.Empty;
			txtVENDOR_PART_NUM.Text     = String.Empty;
			txtSUPPORT_CONTACT.Text     = String.Empty;
			txtWEBSITE        .Text     = String.Empty;
			txtSUPPORT_TERM   .Text     = String.Empty;
			ctlDATE_COST_PRICE.DateText = String.Empty;
			ctlDATE_AVAILABLE .DateText = String.Empty;
			lstCATEGORY       .SelectedIndex = 0;
			lstTAX_CLASS      .SelectedIndex = 0;
			lstSTATUS         .SelectedIndex = 0;
			lstMANUFACTURER   .SelectedIndex = 0;
			lstTYPE           .SelectedIndex = 0;
		}

		public override void SqlSearchClause(IDbCommand cmd)
		{
			// 07/18/2006 Paul.  SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
			Sql.AppendParameter(cmd, txtNAME           .Text         ,  50, Sql.SqlFilterMode.StartsWith, "NAME"           );
			Sql.AppendParameter(cmd, txtMFT_PART_NUM   .Text         ,  50, Sql.SqlFilterMode.StartsWith, "MFT_PART_NUM"   );
			Sql.AppendParameter(cmd, txtVENDOR_PART_NUM.Text         ,  50, Sql.SqlFilterMode.StartsWith, "VENDOR_PART_NUM");
			Sql.AppendParameter(cmd, txtSUPPORT_CONTACT.Text         ,  50, Sql.SqlFilterMode.StartsWith, "SUPPORT_CONTACT");
			Sql.AppendParameter(cmd, txtWEBSITE        .Text         , 255, Sql.SqlFilterMode.StartsWith, "WEBSITE"        );
			Sql.AppendParameter(cmd, txtSUPPORT_TERM   .Text         ,  25, Sql.SqlFilterMode.StartsWith, "SUPPORT_TERM"   );
			Sql.AppendParameter(cmd, lstTAX_CLASS      .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "TAX_CLASS"      );
			Sql.AppendParameter(cmd, lstSTATUS         .SelectedValue,  25, Sql.SqlFilterMode.Exact     , "STATUS"         );
			if ( !Sql.IsEmptyGuid(lstCATEGORY    .SelectedValue) ) Sql.AppendParameter(cmd, Sql.ToGuid(lstCATEGORY    .SelectedValue), "CATEGORY_ID"    );
			if ( !Sql.IsEmptyGuid(lstMANUFACTURER.SelectedValue) ) Sql.AppendParameter(cmd, Sql.ToGuid(lstMANUFACTURER.SelectedValue), "MANUFACTURER_ID");
			if ( !Sql.IsEmptyGuid(lstTYPE        .SelectedValue) ) Sql.AppendParameter(cmd, Sql.ToGuid(lstTYPE        .SelectedValue), "TYPE_ID"        );
			// 07/09/2006 Paul.  Date is no longer converted in the DatePicker control, so convert it here to server time. 
			Sql.AppendParameter(cmd, T10n.ToServerTime(ctlDATE_COST_PRICE.Value), "DATE_COST_PRICE");
			Sql.AppendParameter(cmd, T10n.ToServerTime(ctlDATE_AVAILABLE .Value), "DATE_AVAILABLE" );
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				lstCATEGORY    .DataSource = SplendidCache.ProductCategories();
				lstCATEGORY    .DataBind();
				lstCATEGORY    .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstSTATUS      .DataSource = SplendidCache.List("product_template_status_dom");
				lstSTATUS      .DataBind();
				lstSTATUS      .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstTAX_CLASS   .DataSource = SplendidCache.List("tax_class_dom");
				lstTAX_CLASS   .DataBind();
				lstTAX_CLASS   .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstMANUFACTURER.DataSource = SplendidCache.Manufacturers();
				lstMANUFACTURER.DataBind();
				lstMANUFACTURER.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				lstTYPE        .DataSource = SplendidCache.ProductTypes();
				lstTYPE        .DataBind();
				lstTYPE        .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
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
