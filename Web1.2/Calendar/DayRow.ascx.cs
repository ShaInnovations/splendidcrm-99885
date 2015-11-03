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
using System.Globalization;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Calendar
{
	/// <summary>
	///		Summary description for DayRow.
	/// </summary>
	public class DayRow : SplendidControl
	{
		protected DateTime      dtDATE_START = DateTime.MinValue;
		//protected DateTime      dtDATE_END   = DateTime.MaxValue;
		protected DataView      vwMain             ;
		protected DataList      lstMain            ;
		protected Label         lblError           ;

		protected RadioButton   radScheduleCall    ;
		protected RadioButton   radScheduleMeeting ;
		protected TextBox       txtNAME            ;
		// 06/14/2006 Paul.  The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
		protected Button        btnSave            ;
		protected CultureInfo   ciEnglish          ;

		public CommandEventHandler Command;

		public DateTime DATE_START
		{
			get
			{
				return dtDATE_START;
			}
			set
			{
				dtDATE_START = value;
				// 06/15/2006 Paul.  Instead of binding the command argument at render time, we must bind 
				// here to ensure that ciEnglish has been created.  This is normally not a problem, but it is with DayRow
				// because we are manually loading the control during the rendering of DayGrid. 
				if ( ciEnglish == null )
					ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
				btnSave.CommandArgument = dtDATE_START.ToString(CalendarControl.SqlDateTimeFormat);
			}
		}
		/*
		public DateTime DATE_END
		{
			get
			{
				return dtDATE_END;
			}
			set
			{
				dtDATE_END = value;
			}
		}
		*/
		public DataView DataSource
		{
			set
			{
				vwMain = value;
			}
		}

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( !Sql.IsEmptyString(txtNAME.Text) && Information.IsDate(e.CommandArgument) )
				{
					// 06/09/2006 Paul.  Add code to create call or meeting. This code did not make the 1.0 release. 
					dtDATE_START = Sql.ToDateTime(e.CommandArgument);
					if ( radScheduleCall.Checked )
					{
						Guid gID = Guid.Empty;
						SqlProcs.spCALLS_New(ref gID, txtNAME.Text, T10n.ToServerTime(dtDATE_START));
					}
					else if ( radScheduleMeeting.Checked )
					{
						Guid gID = Guid.Empty;
						SqlProcs.spMEETINGS_New(ref gID, txtNAME.Text, T10n.ToServerTime(dtDATE_START));
					}
				}
			}
			if ( Command != null )
				Command(this, e);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			/*
			// 09/27/2005 Paul.  Instead of performing a query for each cell, just do one query for the entire range and filter before each cell. 
			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					sSQL = "select *                                                       " + ControlChars.CrLf
					     + "  from vwACTIVITIES_List                                       " + ControlChars.CrLf
					     + " where ASSIGNED_USER_ID = @ASSIGNED_USER_ID                    " + ControlChars.CrLf
					     + "   and (   DATE_START >= @DATE_START and DATE_START < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_END   >= @DATE_START and DATE_END   < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_START <  @DATE_START and DATE_END   > @DATE_END" + ControlChars.CrLf
					     + "       )                                                       " + ControlChars.CrLf
					     + " order by DATE_START asc, NAME asc                             " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);
						Sql.AddParameter(cmd, "@DATE_START"      , dtDATE_START    );
						Sql.AddParameter(cmd, "@DATE_END"        , dtDATE_END      );
#if DEBUG	
						Page.RegisterClientScriptBlock("vwACTIVITIES_List" + dtDATE_START.ToOADate().ToString(), Sql.ClientScriptBlock(cmd));
#endif		
						try
						{
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									vwMain = dt.DefaultView;
									lstMain.DataSource = vwMain ;
									lstMain.DataBind();
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
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
			}
			*/
			lstMain.DataSource = vwMain ;
			lstMain.DataBind();
			// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//Page.DataBind();
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
