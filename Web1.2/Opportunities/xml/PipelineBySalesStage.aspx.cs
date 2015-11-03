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
using System.Xml;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Opportunities.xml
{
	/// <summary>
	/// Summary description for PipelineBySalesStage.
	/// </summary>
	public class PipelineBySalesStage : SplendidPage
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			XmlDocument xml = new XmlDocument();
			try
			{
				// 09/15/2005 Paul.  Values will always be in the query string. 
				int      nCHART_LENGTH = Sql.ToInteger (Request.QueryString["CHART_LENGTH"]);
				DateTime dtDATE_START  = T10n.ToServerTime(Sql.ToDateTime(Request.QueryString["DATE_START"  ]));
				DateTime dtDATE_END    = T10n.ToServerTime(Sql.ToDateTime(Request.QueryString["DATE_END"    ]));
				if ( dtDATE_START == DateTime.MinValue )
				{
					// 09/14/2005 Paul.  SugarCRM uses a max date of 01/01/2100. 
					dtDATE_START = DateTime.Today;
				}
				if ( dtDATE_END == DateTime.MinValue )
				{
					// 09/14/2005 Paul.  SugarCRM uses a max date of 01/01/2100. 
					dtDATE_END = new DateTime(2100, 1, 1);
				}
				// 09/15/2005 Paul.  Values will always be in the query string. 
				string[] arrASSIGNED_USER_ID = Request.QueryString.GetValues("ASSIGNED_USER_ID");
				// 09/15/2005 Paul.  Values will always be in the query string. 
				string[] arrSALES_STAGE = Request.QueryString.GetValues("SALES_STAGE");

				xml.LoadXml(SplendidCache.XmlFile(Server.MapPath(Session["themeURL"] + "BarChart.xml")));
				XmlNode nodeRoot        = xml.SelectSingleNode("graphData");
				XmlNode nodeXData       = xml.CreateElement("xData"      );
				XmlNode nodeYData       = xml.CreateElement("yData"      );
				XmlNode nodeColorLegend = xml.CreateElement("colorLegend");
				XmlNode nodeGraphInfo   = xml.CreateElement("graphInfo"  );
				XmlNode nodeChartColors = nodeRoot.SelectSingleNode("chartColors");

				nodeRoot.InsertBefore(nodeGraphInfo  , nodeChartColors);
				nodeRoot.InsertBefore(nodeColorLegend, nodeGraphInfo  );
				nodeRoot.InsertBefore(nodeXData      , nodeColorLegend);
				nodeRoot.InsertBefore(nodeYData      , nodeXData      );
				
				XmlUtil.SetSingleNodeAttribute(xml, nodeYData, "defaultAltText", L10n.Term("Dashboard.LBL_ROLLOVER_DETAILS"));
				XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "min", "0");
				XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "max", "0");
				if ( nCHART_LENGTH < 4 )
					nCHART_LENGTH = 4;
				else if ( nCHART_LENGTH > 10 )
					nCHART_LENGTH = 10;
				XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "length", nCHART_LENGTH.ToString());
				XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "prefix", Sql.ToString(Session["USER_SETTINGS/CURRENCY_SYMBOL"]));
				XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "suffix", "");
				
				nodeGraphInfo.InnerText = L10n.Term("Dashboard.LBL_DATE_RANGE") + " " + Sql.ToDateString(T10n.FromServerTime(dtDATE_START)) + " " + L10n.Term("Dashboard.LBL_DATE_RANGE_TO") + Sql.ToDateString(T10n.FromServerTime(dtDATE_END)) + "<BR/>"
				                        + L10n.Term("Dashboard.LBL_OPP_SIZE"  ) + " " + Strings.FormatCurrency(1, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + L10n.Term("Dashboard.LBL_OPP_THOUSANDS");
				
				Hashtable hashUSER = new Hashtable();
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					// 09/19/2005 Paul.  Prepopulate the stage rows so that empty rows will appear.  The SQL query will not return empty rows. 
					if ( arrSALES_STAGE != null )
					{
						foreach(string sSALES_STAGE in arrSALES_STAGE)
						{
							XmlNode nodeRow = xml.CreateElement("dataRow");
							nodeYData.AppendChild(nodeRow);
							XmlUtil.SetSingleNodeAttribute(xml, nodeRow, "title"   , Sql.ToString(L10n.Term(".sales_stage_dom.", sSALES_STAGE)));
							XmlUtil.SetSingleNodeAttribute(xml, nodeRow, "endLabel", "0");
						}
					}
					// 09/19/2005 Paul.  Prepopulate the user key with all the users specified. 
					if ( arrASSIGNED_USER_ID != null )
					{
						sSQL = "select ID          " + ControlChars.CrLf
						     + "     , USER_NAME   " + ControlChars.CrLf
						     + "  from vwUSERS_List" + ControlChars.CrLf
						     + " where 1 = 1       " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AppendGuids(cmd, arrASSIGNED_USER_ID, "ID");
							cmd.CommandText += " order by USER_NAME" + ControlChars.CrLf;
							using ( IDataReader rdr = cmd.ExecuteReader() )
							{
								while ( rdr.Read() )
								{
									Guid   gUSER_ID   = Sql.ToGuid   (rdr["ID"       ]);
									string sUSER_NAME = Sql.ToString (rdr["USER_NAME"]);
									if ( !hashUSER.ContainsKey(gUSER_ID.ToString()) )
									{
										XmlNode nodeMapping = xml.CreateElement("mapping");
										nodeColorLegend.AppendChild(nodeMapping);
										XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "id"   , gUSER_ID.ToString());
										XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "name" , sUSER_NAME);
										XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "color", SplendidDefaults.generate_graphcolor(gUSER_ID.ToString(), hashUSER.Count));
										hashUSER.Add(gUSER_ID.ToString(), sUSER_NAME);
									}
								}
							}
						}
					}
					sSQL = "select SALES_STAGE                                   " + ControlChars.CrLf
					     + "     , ASSIGNED_USER_ID                              " + ControlChars.CrLf
					     + "     , USER_NAME                                     " + ControlChars.CrLf
					     + "     , LIST_ORDER                                    " + ControlChars.CrLf
					     + "     , sum(AMOUNT_USDOLLAR/1000) as TOTAL            " + ControlChars.CrLf
					     + "     , count(*)                  as OPPORTUNITY_COUNT" + ControlChars.CrLf
					     + "  from vwOPPORTUNITIES_Pipeline                      " + ControlChars.CrLf
					     + " where DATE_CLOSED >= @DATE_START                    " + ControlChars.CrLf
					     + "   and DATE_CLOSED <= @DATE_END                      " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						// 09/14/2005 Paul.  Use add because <= and >= are not supported. 
						Sql.AddParameter   (cmd, "@DATE_START"      , dtDATE_START      );
						Sql.AddParameter   (cmd, "@DATE_END"        , dtDATE_END        );
						// 09/14/2005 Paul.  Use append because it supports arrays using the IN clause. 
						Sql.AppendGuids    (cmd, arrASSIGNED_USER_ID, "ASSIGNED_USER_ID");
						Sql.AppendParameter(cmd, arrSALES_STAGE     , "SALES_STAGE"     );
#if false
						if ( arrSALES_STAGE != null )
							nodeGraphInfo.InnerText = "SALES_STAGE = " + String.Join(", ", arrSALES_STAGE);
#endif
						
						cmd.CommandText += ""
						     + " group by SALES_STAGE                                " + ControlChars.CrLf
						     + "        , LIST_ORDER                                 " + ControlChars.CrLf
						     + "        , ASSIGNED_USER_ID                           " + ControlChars.CrLf
						     + "        , USER_NAME                                  " + ControlChars.CrLf
						     + " order by LIST_ORDER                                 " + ControlChars.CrLf
						     + "        , USER_NAME                                  " + ControlChars.CrLf;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							double dMAX_TOTAL      = 0;
							double dPIPELINE_TOTAL = 0;
							while ( rdr.Read() )
							{
								string  sSALES_STAGE       = Sql.ToString (rdr["SALES_STAGE"      ]);
								double  dTOTAL             = Sql.ToDouble (rdr["TOTAL"            ]);
								int     nOPPORTUNITY_COUNT = Sql.ToInteger(rdr["OPPORTUNITY_COUNT"]);
								Guid    gASSIGNED_USER_ID  = Sql.ToGuid   (rdr["ASSIGNED_USER_ID" ]);
								string  sUSER_NAME         = Sql.ToString (rdr["USER_NAME"        ]);
								
								dPIPELINE_TOTAL += dTOTAL;
								if ( dTOTAL > dMAX_TOTAL )
									dMAX_TOTAL = dTOTAL;
								XmlNode nodeRow = nodeYData.SelectSingleNode("dataRow[@title=\'" + Sql.ToString(L10n.Term(".sales_stage_dom.", sSALES_STAGE)).Replace("'", "\'") +"\']");
								if ( nodeRow == null )
								{
									nodeRow = xml.CreateElement("dataRow");
									nodeYData.AppendChild(nodeRow);
									XmlUtil.SetSingleNodeAttribute(xml, nodeRow, "title"   , Sql.ToString(L10n.Term(".sales_stage_dom.", sSALES_STAGE)));
									XmlUtil.SetSingleNodeAttribute(xml, nodeRow, "endLabel", dTOTAL.ToString("0")   );
								}
								else
								{
									if ( nodeRow.Attributes.GetNamedItem("endLabel") != null )
									{
										double dEND_LABEL = Sql.ToDouble(nodeRow.Attributes.GetNamedItem("endLabel").Value);
										dEND_LABEL += dTOTAL;
										if ( dEND_LABEL > dMAX_TOTAL )
											dMAX_TOTAL = dEND_LABEL;
										XmlUtil.SetSingleNodeAttribute(xml, nodeRow, "endLabel", dEND_LABEL.ToString("0")   );
									}
								}
								
								if ( !hashUSER.ContainsKey(gASSIGNED_USER_ID.ToString()) )
								{
									XmlNode nodeMapping = xml.CreateElement("mapping");
									nodeColorLegend.AppendChild(nodeMapping);
									XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "id"   , gASSIGNED_USER_ID.ToString());
									XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "name" , sUSER_NAME);
									XmlUtil.SetSingleNodeAttribute(xml, nodeMapping, "color", SplendidDefaults.generate_graphcolor(gASSIGNED_USER_ID.ToString(), hashUSER.Count));
									hashUSER.Add(gASSIGNED_USER_ID.ToString(), sUSER_NAME);
								}
								
								XmlNode nodeBar = xml.CreateElement("bar");
								nodeRow.AppendChild(nodeBar);
								XmlUtil.SetSingleNodeAttribute(xml, nodeBar, "id"       , gASSIGNED_USER_ID.ToString());
								XmlUtil.SetSingleNodeAttribute(xml, nodeBar, "totalSize", dTOTAL.ToString("0"));
								XmlUtil.SetSingleNodeAttribute(xml, nodeBar, "altText"  , sUSER_NAME + ": " + nOPPORTUNITY_COUNT.ToString() + " " + L10n.Term("Dashboard.LBL_OPPS_WORTH") + " " + dTOTAL.ToString("0") + L10n.Term("Dashboard.LBL_OPP_THOUSANDS") + " " + L10n.Term("Dashboard.LBL_OPPS_IN_STAGE") + " " + Sql.ToString(L10n.Term(".sales_stage_dom.", sSALES_STAGE)) );
								XmlUtil.SetSingleNodeAttribute(xml, nodeBar, "url"      , Sql.ToString(Application["rootURL"]) + "Opportunities/default.aspx?SALES_STAGE=" + Server.UrlEncode(sSALES_STAGE) + "&ASSIGNED_USER_ID=" + gASSIGNED_USER_ID.ToString() );
							}
							int    nNumLength   = Math.Floor(dMAX_TOTAL).ToString("0").Length - 1;
							double dWhole       = Math.Pow(10, nNumLength);
							double dDecimal     = 1 / dWhole;
							double dMAX_ROUNDED = Math.Ceiling(dMAX_TOTAL * dDecimal) * dWhole;
							
							XmlUtil.SetSingleNodeAttribute(xml, nodeXData, "max", dMAX_ROUNDED.ToString("0"));
							XmlUtil.SetSingleNodeAttribute(xml, nodeRoot , "title", L10n.Term("Dashboard.LBL_TOTAL_PIPELINE") + Strings.FormatCurrency(dPIPELINE_TOTAL, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + L10n.Term("Dashboard.LBL_OPP_THOUSANDS"));
						}
					}
				}
				Response.ContentType = "text/xml";
				Response.Write(xml.OuterXml);
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				Response.Write(ex.Message);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
