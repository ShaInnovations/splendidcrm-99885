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
	///		Summary description for SharedGrid.
	/// </summary>
	public class SharedGrid : CalendarControl
	{
		protected Label          lblError         ;
		protected DateTime       dtCurrentWeek    ;
		protected PlaceHolder    plcWeekRows      ;
		protected CalendarHeader ctlCalendarHeader;
		protected ListBox        lstUSERS         ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				switch ( e.CommandName )
				{
					case "Shared.Previous":
					{
						dtCurrentDate = dtCurrentDate.AddDays(-7);
						dtCurrentWeek = dtCurrentDate.AddDays(DayOfWeek.Sunday - dtCurrentDate.DayOfWeek);
						ViewState["CurrentDate"] = dtCurrentDate;
						break;
					}
					case "Shared.Next":
					{
						dtCurrentDate = dtCurrentDate.AddDays(7);
						dtCurrentWeek = dtCurrentDate.AddDays(DayOfWeek.Sunday - dtCurrentDate.DayOfWeek);
						ViewState["CurrentDate"] = dtCurrentDate;
						break;
					}
					case "Day.Current":
					{
						Response.Redirect("default.aspx?" + CalendarQueryString(dtCurrentDate));
						break;
					}
					case "Week.Current":
					{
						ViewState["CurrentDate"] = dtCurrentDate;
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
			plcWeekRows.Controls.Clear();
			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					DataTable dtUsers = new DataTable();
					DateTime dtDATE_START = new DateTime(Math.Max(1753, dtCurrentWeek.Year), dtCurrentWeek.Month, dtCurrentWeek.Day, 0, 0, 0);
					DateTime dtDATE_END   = dtDATE_START.AddDays(7);
					sSQL = "select distinct                                                " + ControlChars.CrLf
					     + "       ASSIGNED_USER_ID                                        " + ControlChars.CrLf
					     + "     , ASSIGNED_FULL_NAME                                      " + ControlChars.CrLf
					     + "  from vwACTIVITIES_List                                       " + ControlChars.CrLf
					     + " where (   DATE_START >= @DATE_START and DATE_START < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_END   >= @DATE_START and DATE_END   < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_START <  @DATE_START and DATE_END   > @DATE_END" + ControlChars.CrLf
					     + "       )                                                       " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@DATE_START"      , dtDATE_START      );
						Sql.AddParameter(cmd, "@DATE_END"        , dtDATE_END        );
						Sql.AppendGuids (cmd, lstUSERS           , "ASSIGNED_USER_ID");
						cmd.CommandText += " order by ASSIGNED_FULL_NAME" + ControlChars.CrLf;
#if DEBUG
						Page.RegisterClientScriptBlock("vwACTIVITIES_List.Users", Sql.ClientScriptBlock(cmd));
#endif
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							da.Fill(dtUsers);
						}
					}
					
					
					sSQL = "select *                                                       " + ControlChars.CrLf
					     + "  from vwACTIVITIES_List                                       " + ControlChars.CrLf
					     + " where (   DATE_START >= @DATE_START and DATE_START < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_END   >= @DATE_START and DATE_END   < @DATE_END" + ControlChars.CrLf
					     + "        or DATE_START <  @DATE_START and DATE_END   > @DATE_END" + ControlChars.CrLf
					     + "       )                                                       " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@DATE_START"      , dtDATE_START      );
						Sql.AddParameter(cmd, "@DATE_END"        , dtDATE_END        );
						Sql.AppendGuids (cmd, lstUSERS           , "ASSIGNED_USER_ID");
						cmd.CommandText += " order by ASSIGNED_FULL_NAME asc, DATE_START asc, NAME asc" + ControlChars.CrLf;
#if DEBUG
						Page.RegisterClientScriptBlock("vwACTIVITIES_List.Data", Sql.ClientScriptBlock(cmd));
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
									foreach(DataRow rowUser in dtUsers.Rows)
									{
										Guid   gASSIGNED_USER_ID   = Sql.ToGuid  (rowUser["ASSIGNED_USER_ID"  ]);
										string sASSIGNED_FULL_NAME = Sql.ToString(rowUser["ASSIGNED_FULL_NAME"]);
										HtmlGenericControl h5User = new HtmlGenericControl("h5");
										h5User.Attributes.Add("class", "calSharedUser");
										h5User.Controls.Add(new LiteralControl(sASSIGNED_FULL_NAME));
										plcWeekRows.Controls.Add(h5User);

										HtmlTable tblUserWeek = new HtmlTable();
										plcWeekRows.Controls.Add(tblUserWeek);
										tblUserWeek.Border      = 0;
										tblUserWeek.CellPadding = 0;
										tblUserWeek.CellSpacing = 1;
										tblUserWeek.Width       = "100%";
										HtmlTableRow tr = new HtmlTableRow();
										tblUserWeek.Rows.Add(tr);
										
										CultureInfo ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
										for(int iDay = 0 ; iDay < 7 ; iDay++ )
										{
											DataView vwMain = new DataView(dt);
											DateTime dtDAY_START = dtCurrentWeek;
											dtDAY_START = dtDAY_START.AddDays(iDay);
											DateTime dtDAY_END   = dtDAY_START.AddDays(1);
											
											HtmlTableCell cell = new HtmlTableCell();
											tr.Cells.Add(cell);
											cell.Width  = "14%";
											cell.VAlign = "top";
											cell.Attributes.Add("class", "dailyCalBodyItems");
											cell.Controls.Add(new LiteralControl(dtDAY_START.ToString("ddd d")));
											
											// 09/27/2005 Paul.  System.Data.DataColumn.Expression documentation has description how to define dates and strings. 
											// 01/21/2006 Paul.  Brazilian culture is having a problem with date formats.  Try using the european format. 
											// 06/13/2006 Paul.  Italian has a problem with the time separator.  Use the value from the culture from CalendarControl.SqlDateTimeFormat. 
											// 06/14/2006 Paul.  The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
											string sDAY_START = dtDAY_START.ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat);
											string sDAY_END   = dtDAY_END  .ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat);
											vwMain.RowFilter = "ASSIGNED_USER_ID = '" + gASSIGNED_USER_ID.ToString() + "'" + ControlChars.CrLf
											                 + "and (   DATE_START >= #" + sDAY_START + "# and DATE_START <  #" + sDAY_END + "#" + ControlChars.CrLf
											                 + "     or DATE_END   >= #" + sDAY_START + "# and DATE_END   <= #" + sDAY_END + "#" + ControlChars.CrLf
											                 + "     or DATE_START <  #" + sDAY_START + "# and DATE_END   >  #" + sDAY_END + "#" + ControlChars.CrLf
											                 + "    )" + ControlChars.CrLf;
#if DEBUG
//											Page.RegisterClientScriptBlock("vwACTIVITIES_List" + dtDAY_START.ToOADate().ToString(), Sql.EscapeJavaScript(vwMain.RowFilter));
#endif
											if ( vwMain.Count > 0 )
											{
												SharedCell ctlSharedCell = LoadControl("SharedCell.ascx") as SharedCell;
												ctlSharedCell.DataSource = vwMain;
												cell.Controls.Add(ctlSharedCell);
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
					lstUSERS.DataSource = SplendidCache.ActiveUsers();
					lstUSERS.DataBind();
					foreach(ListItem item in lstUSERS.Items)
					{
						item.Selected = true;
					}
					dtCurrentWeek = dtCurrentDate.AddDays(DayOfWeek.Sunday - dtCurrentDate.DayOfWeek);
					BindGrid();
				}
				else
				{
					dtCurrentWeek = dtCurrentDate.AddDays(DayOfWeek.Sunday - dtCurrentDate.DayOfWeek);
				}
				//// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
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
		}
		#endregion
	}
}
