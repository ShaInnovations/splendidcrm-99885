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
	///		Summary description for DayGrid.
	/// </summary>
	public class DayGrid : CalendarControl
	{
		protected Label          lblError         ;
		protected PlaceHolder    plcDayRows       ;
		protected CalendarHeader ctlCalendarHeader;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				switch ( e.CommandName )
				{
					case "Day.Previous":
					{
						dtCurrentDate = dtCurrentDate.AddDays(-1);
						ViewState["CurrentDate"] = dtCurrentDate;
						// 06/15/2006 Paul.  There seems to be a conflict with binding the DayGrid inside InitializeComponent(). 
						// Binding early is required to enable the button events, but the ViewState is not available, so the query is invalid. 
						Response.Redirect("default.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Day.Next":
					{
						dtCurrentDate = dtCurrentDate.AddDays(1);
						ViewState["CurrentDate"] = dtCurrentDate;
						// 06/15/2006 Paul.  There seems to be a conflict with binding the DayGrid inside InitializeComponent(). 
						// Binding early is required to enable the button events, but the ViewState is not available, so the query is invalid. 
						Response.Redirect("default.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Day.Current":
					{
						ViewState["CurrentDate"] = dtCurrentDate;
						// 06/15/2006 Paul.  There seems to be a conflict with binding the DayGrid inside InitializeComponent(). 
						// Binding early is required to enable the button events, but the ViewState is not available, so the query is invalid. 
						Response.Redirect("default.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Week.Current":
					{
						Response.Redirect("Week.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Month.Current":
					{
						Response.Redirect("Month.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Year.Current":
					{
						Response.Redirect("Year.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Shared.Current":
					{
						Response.Redirect("Shared.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Save":
					{
						Response.Redirect("default.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
				}
				BindGrid();
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
			}
		}

		protected void BindGrid()
		{
			plcDayRows.Controls.Clear();
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
						DateTime dtDATE_START = new DateTime(Math.Max(1753, dtCurrentDate.Year), dtCurrentDate.Month, dtCurrentDate.Day, 0, 0, 0);
						DateTime dtDATE_END   = dtDATE_START.AddDays(1);
						Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);
						Sql.AddParameter(cmd, "@DATE_START"      , dtDATE_START    );
						Sql.AddParameter(cmd, "@DATE_END"        , dtDATE_END      );
#if DEBUG
						Page.RegisterClientScriptBlock("vwACTIVITIES_List", Sql.ClientScriptBlock(cmd));
#endif
						try
						{
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									// 07/24/2005 Paul.  Since this is not a dynamic grid, we must convert the status manually. 
									foreach(DataRow row in dt.Rows)
									{
										switch ( Sql.ToString(row["ACTIVITY_TYPE"]) )
										{
											case "Calls"   :  row["STATUS"] = L10n.Term("Call"   ) + " " + L10n.Term(".call_status_dom."   , row["STATUS"]);  break;
											case "Meetings":  row["STATUS"] = L10n.Term("Meeting") + " " + L10n.Term(".meeting_status_dom.", row["STATUS"]);  break;
										}
									}

									int nHourMin = 8;
									int nHourMax = 18;
									if ( dt.Rows.Count > 0 )
									{
										DateTime dtMin = Sql.ToDateTime(dt.Rows[0]["DATE_START"]);
										DateTime dtMax = Sql.ToDateTime(dt.Rows[dt.Rows.Count-1]["DATE_END"]);
										nHourMin = Math.Min(dtMin.Hour, nHourMin);
										nHourMax = Math.Max(dtMax.Hour, nHourMax);
									}
									CultureInfo ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
									for(int iHour = nHourMin ; iHour <= nHourMax ; iHour++ )
									{
										DataView vwMain = new DataView(dt);
										DateTime dtHOUR_START = new DateTime(dtCurrentDate.Year, dtCurrentDate.Month, dtCurrentDate.Day, iHour, 0, 0);
										DateTime dtHOUR_END   = dtHOUR_START.AddHours(1);
										DateTime dtHOUR_START_ServerTime = T10n.ToServerTime(dtHOUR_START);
										DateTime dtHOUR_END_ServerTime   = T10n.ToServerTime(dtHOUR_END  );
										// 09/27/2005 Paul.  System.Data.DataColumn.Expression documentation has description how to define dates and strings. 
										// 01/21/2006 Paul.  Brazilian culture is having a problem with date formats.  Try using the european format yyyy/MM/dd HH:mm:ss. 
										// 06/09/2006 Paul.  Fix so that a 1 hour meeting does not span two hours.  DATE_END should not allow DATE_END = HOUR_START. 
										// 06/13/2006 Paul.  Italian has a problem with the time separator.  Use the value from the culture from CalendarControl.SqlDateTimeFormat. 
										// 06/14/2006 Paul.  The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
										string sHOUR_START_ServerTime = dtHOUR_START_ServerTime.ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat);
										string sHOUR_END_ServerTime   = dtHOUR_END_ServerTime  .ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat);
										vwMain.RowFilter = "   DATE_START >= #" + sHOUR_START_ServerTime + "# and DATE_START <  #" + sHOUR_END_ServerTime + "#" + ControlChars.CrLf
										                 + "or DATE_END   >  #" + sHOUR_START_ServerTime + "# and DATE_END   <= #" + sHOUR_END_ServerTime + "#" + ControlChars.CrLf
										                 + "or DATE_START <  #" + sHOUR_START_ServerTime + "# and DATE_END   >  #" + sHOUR_END_ServerTime + "#" + ControlChars.CrLf;
#if DEBUG
//										Page.RegisterClientScriptBlock("vwACTIVITIES_List" + dtHOUR_START.ToOADate().ToString(), Sql.EscapeJavaScript(vwMain.RowFilter));
#endif
										DayRow ctlDayRow = LoadControl("DayRow.ascx") as DayRow;
										// 06/09/2006 Paul.  Add to controls list before bindging. 
										plcDayRows.Controls.Add(ctlDayRow);
										ctlDayRow.Command = new CommandEventHandler(Page_Command);
										
										ctlDayRow.DATE_START = dtHOUR_START;
										//ctlDayRow.DATE_END   = dtHOUR_END;
										ctlDayRow.DataSource = vwMain;
										// 06/09/2006 Paul.  Need to bind after specifying the data source. 
										ctlDayRow.DataBind();
										//ctlDayRow.DATE_START = new DateTime(dtCurrentDate.Year, dtCurrentDate.Month, dtCurrentDate.Day, iHour, 0, 0);
										//ctlDayRow.DATE_END   = ctlDayRow.DATE_START.AddHours(1);
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
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarInitDate();
				if ( !IsPostBack )
				{
					BindGrid();
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
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
			ctlCalendarHeader.Command = new CommandEventHandler(Page_Command);
			// 06/09/2006 Paul.  If this is a postback, then build the grid early so that we will not lose any button events. 
			if ( IsPostBack )
			{
				// 06/09/2006 Paul.  T10n has not been defined at this point, so create it. 
				GetT10n();
				BindGrid();
			}
		}
		#endregion
	}
}
