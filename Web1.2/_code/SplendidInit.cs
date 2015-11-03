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
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Xml;
using System.CodeDom.Compiler;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for SplendidInit.
	/// </summary>
	public class SplendidInit
	{
		public static void InitAppURLs()
		{
			HttpApplicationState Application = HttpContext.Current.Application;
			string sApplicationPath = HttpContext.Current.Request.ApplicationPath;
			if ( !sApplicationPath.EndsWith("/") )
				sApplicationPath += "/";
			if ( Sql.IsEmptyString(Application["imageURL"]) )
			{
				Application["rootURL"  ] = sApplicationPath;
				// 07/28/2006 Paul.  Mono requires case-significant paths. 
				Application["imageURL" ] = sApplicationPath + "Include/images/";
				Application["scriptURL"] = sApplicationPath + "Include/javascript/";
				Application["chartURL" ] = sApplicationPath + "Include/charts/";
			}
		}

		public static void InitTerminology()
		{
			try
			{
				HttpApplicationState Application = HttpContext.Current.Application;
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select NAME             " + ControlChars.CrLf
					     + "     , LANG             " + ControlChars.CrLf
					     + "     , MODULE_NAME      " + ControlChars.CrLf
					     + "     , DISPLAY_NAME     " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY    " + ControlChars.CrLf
					     + " where LIST_NAME is null" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
// 01/20/2006 Paul.  Enable all languages when debugging. 
//#if DEBUG
//						sSQL += "   and LANG = 'en-us'" + ControlChars.CrLf;
//#endif
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								//Application[Sql.ToString(rdr["LANG"]) + "." + Sql.ToString(rdr["MODULE_NAME"]) + "." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["DISPLAY_NAME"]);
								string sLANG         = Sql.ToString(rdr["LANG"        ]);
								string sMODULE_NAME  = Sql.ToString(rdr["MODULE_NAME" ]);
								string sNAME         = Sql.ToString(rdr["NAME"        ]);
								string sDISPLAY_NAME = Sql.ToString(rdr["DISPLAY_NAME"]);
								L10N.SetTerm(sLANG, sMODULE_NAME, sNAME, sDISPLAY_NAME);
							}
						}
					}
					sSQL = "select NAME                 " + ControlChars.CrLf
					     + "     , LANG                 " + ControlChars.CrLf
					     + "     , MODULE_NAME          " + ControlChars.CrLf
					     + "     , LIST_NAME            " + ControlChars.CrLf
					     + "     , DISPLAY_NAME         " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY        " + ControlChars.CrLf
					     + " where LIST_NAME is not null" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
// 01/20/2006 Paul.  Enable all languages when debugging. 
//#if DEBUG
//						sSQL += "   and LANG = 'en-us'" + ControlChars.CrLf;
//#endif
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								// 01/13/2006 Paul.  Don't include MODULE_NAME when used with a list. 
								// DropDownLists are populated without the module name in the list name. 
								// 01/13/2006 Paul.  We can remove the module, but not the dot.  
								// Otherwise it breaks all other code that references a list term. 
								//Application[Sql.ToString(rdr["LANG"]) + "." + sMODULE_NAME + "." + Sql.ToString(rdr["LIST_NAME"]) + "." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["DISPLAY_NAME"]);
								string sLANG         = Sql.ToString(rdr["LANG"        ]);
								string sMODULE_NAME  = Sql.ToString(rdr["MODULE_NAME" ]);
								string sNAME         = Sql.ToString(rdr["NAME"        ]);
								string sLIST_NAME    = Sql.ToString(rdr["LIST_NAME"   ]);
								string sDISPLAY_NAME = Sql.ToString(rdr["DISPLAY_NAME"]);
								L10N.SetTerm(sLANG, sMODULE_NAME, sLIST_NAME, sNAME, sDISPLAY_NAME);
							}
						}
					}

					sSQL = "select ALIAS_NAME           " + ControlChars.CrLf
					     + "     , ALIAS_MODULE_NAME    " + ControlChars.CrLf
					     + "     , ALIAS_LIST_NAME      " + ControlChars.CrLf
					     + "     , NAME                 " + ControlChars.CrLf
					     + "     , MODULE_NAME          " + ControlChars.CrLf
					     + "     , LIST_NAME            " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY_ALIASES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								string sALIAS_NAME         = Sql.ToString(rdr["ALIAS_NAME"        ]);
								string sALIAS_MODULE_NAME  = Sql.ToString(rdr["ALIAS_MODULE_NAME" ]);
								string sALIAS_LIST_NAME    = Sql.ToString(rdr["ALIAS_LIST_NAME"   ]);
								string sNAME               = Sql.ToString(rdr["NAME"              ]);
								string sMODULE_NAME        = Sql.ToString(rdr["MODULE_NAME"       ]);
								string sLIST_NAME          = Sql.ToString(rdr["LIST_NAME"         ]);
								L10N.SetAlias(sALIAS_MODULE_NAME, sALIAS_LIST_NAME, sALIAS_NAME, sMODULE_NAME, sLIST_NAME, sNAME);
							}
						}
					}

					// 07/13/2006 Paul.  The reporting module needs a quick way to translate a module name to a table name. 
					sSQL = "select MODULE_NAME" + ControlChars.CrLf
					     + "     , TABLE_NAME " + ControlChars.CrLf
					     + "  from vwMODULES  " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								string sMODULE_NAME = Sql.ToString(rdr["MODULE_NAME"]);
								string sTABLE_NAME  = Sql.ToString(rdr["TABLE_NAME" ]);
								Application["Modules." + sMODULE_NAME + ".TableName"] = sTABLE_NAME;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitModuleACL()
		{
			HttpSessionState Session = HttpContext.Current.Session;

			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select MODULE_NAME          " + ControlChars.CrLf
				     + "     , ACLACCESS_ADMIN      " + ControlChars.CrLf
				     + "     , ACLACCESS_ACCESS     " + ControlChars.CrLf
				     + "     , ACLACCESS_VIEW       " + ControlChars.CrLf
				     + "     , ACLACCESS_LIST       " + ControlChars.CrLf
				     + "     , ACLACCESS_EDIT       " + ControlChars.CrLf
				     + "     , ACLACCESS_DELETE     " + ControlChars.CrLf
				     + "     , ACLACCESS_IMPORT     " + ControlChars.CrLf
				     + "     , ACLACCESS_EXPORT     " + ControlChars.CrLf
				     + "  from vwACL_ACCESS_ByModule" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sMODULE_NAME = Sql.ToString(rdr["MODULE_NAME"]);
							Security.SetModuleAccess(sMODULE_NAME, "admin" , Sql.ToInteger(rdr["ACLACCESS_ADMIN" ]));
							Security.SetModuleAccess(sMODULE_NAME, "access", Sql.ToInteger(rdr["ACLACCESS_ACCESS"]));
							Security.SetModuleAccess(sMODULE_NAME, "view"  , Sql.ToInteger(rdr["ACLACCESS_VIEW"  ]));
							Security.SetModuleAccess(sMODULE_NAME, "list"  , Sql.ToInteger(rdr["ACLACCESS_LIST"  ]));
							Security.SetModuleAccess(sMODULE_NAME, "edit"  , Sql.ToInteger(rdr["ACLACCESS_EDIT"  ]));
							Security.SetModuleAccess(sMODULE_NAME, "delete", Sql.ToInteger(rdr["ACLACCESS_DELETE"]));
							Security.SetModuleAccess(sMODULE_NAME, "import", Sql.ToInteger(rdr["ACLACCESS_IMPORT"]));
							Security.SetModuleAccess(sMODULE_NAME, "export", Sql.ToInteger(rdr["ACLACCESS_EXPORT"]));
						}
					}
				}
			}
		}

		public static void InitApp()
		{
			try
			{
				HttpApplicationState Application = HttpContext.Current.Application;
				if ( Application.Count == 0 )
					SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Application start.");
				else
					SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Application restart.");
				
				// 11/14/2005 Paul.  Force the reload of the provider and connection strings. 
				// Application.Remove("SplendidProvider");
				// 11/28/2005 Paul.  Use Clear() to clear all application variables. 
				DataTable dtSystemErrors = Application["SystemErrors"] as DataTable;
				Application.Clear();
				// 11/28/2005 Paul.  Save and restore the system errors table. 
				Application["SystemErrors"] = dtSystemErrors;
				InitAppURLs();

				// 11/28/2005 Paul.  Clear all cache variables as well. 
				foreach(DictionaryEntry oKey in HttpContext.Current.Cache)
				{
					string sKey = oKey.Key.ToString();
					HttpContext.Current.Cache.Remove(sKey);
				}
				// 06/03/2006 Paul.  Clear the cached data that is stored in the Session object. 
				if ( HttpContext.Current.Session != null )
				{
					Hashtable hashSessionKeys = new Hashtable();
					foreach(string sKey in HttpContext.Current.Session.Keys)
					{
						hashSessionKeys.Add(sKey, null);
					}
					// 06/03/2006 Paul.  We can't remove a key when it is used in the enumerator. 
					foreach(string sKey in hashSessionKeys.Keys )
					{
						if ( sKey.StartsWith("vwSHORTCUTS_Menu_ByUser") || sKey.StartsWith("vwMODULES_TabMenu_ByUser") )
							HttpContext.Current.Session.Remove(sKey);
					}
				}

				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					// 07/28/2006 Paul.  Test the database connection and allow an early exit if failed. 
					con.Open();
				}

				// 01/12/2006 Paul.  Separate out the terminology so that it can be called when importing a language pack. 
				InitTerminology();

				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select NAME    " + ControlChars.CrLf
					     + "     , VALUE   " + ControlChars.CrLf
					     + "  from vwCONFIG" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								Application["CONFIG." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["VALUE"]);
							}
						}
					}
					sSQL = "select *          " + ControlChars.CrLf
					     + "  from vwTIMEZONES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								TimeZone oTimeZone = new TimeZone
									( Sql.ToGuid   (rdr["ID"                   ])
									, Sql.ToString (rdr["NAME"                 ])
									, Sql.ToString (rdr["STANDARD_NAME"        ])
									, Sql.ToString (rdr["STANDARD_ABBREVIATION"])
									, Sql.ToString (rdr["DAYLIGHT_NAME"        ])
									, Sql.ToString (rdr["DAYLIGHT_ABBREVIATION"])
									, Sql.ToInteger(rdr["BIAS"                 ])
									, Sql.ToInteger(rdr["STANDARD_BIAS"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_BIAS"        ])
									, Sql.ToInteger(rdr["STANDARD_YEAR"        ])
									, Sql.ToInteger(rdr["STANDARD_MONTH"       ])
									, Sql.ToInteger(rdr["STANDARD_WEEK"        ])
									, Sql.ToInteger(rdr["STANDARD_DAYOFWEEK"   ])
									, Sql.ToInteger(rdr["STANDARD_HOUR"        ])
									, Sql.ToInteger(rdr["STANDARD_MINUTE"      ])
									, Sql.ToInteger(rdr["DAYLIGHT_YEAR"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_MONTH"       ])
									, Sql.ToInteger(rdr["DAYLIGHT_WEEK"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_DAYOFWEEK"   ])
									, Sql.ToInteger(rdr["DAYLIGHT_HOUR"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_MINUTE"      ])
									, Sql.ToBoolean(Application["CONFIG.GMT_Storage"])
									);
								Application["TIMEZONE." + oTimeZone.ID.ToString()] = oTimeZone;
							}
						}
					}
					sSQL = "select *           " + ControlChars.CrLf
					     + "  from vwCURRENCIES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								Currency C10n = new Currency
									( Sql.ToGuid  (rdr["ID"             ])
									, Sql.ToString(rdr["NAME"           ])
									, Sql.ToString(rdr["SYMBOL"         ])
									, Sql.ToFloat (rdr["CONVERSION_RATE"])
									);
								Application["CURRENCY." + C10n.ID.ToString()] = C10n;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static XmlDocument InitUserPreferences(string sUSER_PREFERENCES)
		{
			XmlDocument xml = null;
			try
			{
				xml = new XmlDocument();
				if ( !sUSER_PREFERENCES.StartsWith("<?xml ") )
				{
					sUSER_PREFERENCES = XmlUtil.ConvertFromPHP(sUSER_PREFERENCES);
				}
				
				xml.LoadXml(sUSER_PREFERENCES);
				
				HttpApplicationState Application = HttpContext.Current.Application;
				string sCulture    = L10N.NormalizeCulture(XmlUtil.SelectSingleNode(xml, "culture"));
				string sTheme      = XmlUtil.SelectSingleNode(xml, "theme"      );
				string sDateFormat = XmlUtil.SelectSingleNode(xml, "dateformat" );
				string sTimeFormat = XmlUtil.SelectSingleNode(xml, "timeformat" );
				string sTimeZone   = XmlUtil.SelectSingleNode(xml, "timezone"   );
				string sCurrencyID = XmlUtil.SelectSingleNode(xml, "currency_id");
				if ( Sql.IsEmptyString(sCulture) )
				{
					XmlUtil.SetSingleNode(xml, "culture", SplendidDefaults.Culture());
				}
				if ( Sql.IsEmptyString(sTheme) )
				{
					XmlUtil.SetSingleNode(xml, "theme", SplendidDefaults.Theme());
				}
				if ( Sql.IsEmptyString(sDateFormat) )
				{
					XmlUtil.SetSingleNode(xml, "dateformat", SplendidDefaults.DateFormat());
				}
				// 11/12/2005 Paul.  "m" is not valid for .NET month formatting.  Must use MM. 
				// 11/12/2005 Paul.  Require 4 digit year.  Otherwise default date in Pipeline of 12/31/2100 would get converted to 12/31/00. 
				if ( SplendidDefaults.IsValidDateFormat(sDateFormat) )
				{
					XmlUtil.SetSingleNode(xml, "dateformat", SplendidDefaults.DateFormat(sDateFormat));
				}
				if ( Sql.IsEmptyString(sTimeFormat) )
				{
					XmlUtil.SetSingleNode(xml, "timeformat", SplendidDefaults.TimeFormat());
				}
				if ( Sql.IsEmptyString(sCurrencyID) )
				{
					XmlUtil.SetSingleNode(xml, "currency_id", SplendidDefaults.CurrencyID());
				}
				// 09/01/2006 Paul.  Only use timez if provided.  Otherwise we will default to GMT. 
				if ( Sql.IsEmptyString(sTimeZone) && !Sql.IsEmptyString(XmlUtil.SelectSingleNode(xml, "timez")) )
				{
					int nTimez = Sql.ToInteger(XmlUtil.SelectSingleNode(xml, "timez"));
					sTimeZone = SplendidDefaults.TimeZone(nTimez);
					XmlUtil.SetSingleNode(xml, "timezone", sTimeZone);
				}
				// 09/01/2006 Paul.  Default TimeZone was not getting set properly. 
				if ( Sql.IsEmptyString(sTimeZone) )
				{
					sTimeZone = SplendidDefaults.TimeZone();
					XmlUtil.SetSingleNode(xml, "timezone", sTimeZone);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
			}
			return xml;
		}

		public static void LoadUserPreferences(Guid gID, string sTheme, string sCulture)
		{
			HttpApplicationState Application = HttpContext.Current.Application;
			HttpSessionState     Session     = HttpContext.Current.Session    ;
			string sApplicationPath = Sql.ToString(Application["rootURL"]);

			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *           " + ControlChars.CrLf
				     + "  from vwUSERS_Edit" + ControlChars.CrLf
				     + " where ID = @ID    " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ID", gID);
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							string sUSER_PREFERENCES = Sql.ToString(rdr["USER_PREFERENCES"]);
							if ( !Sql.IsEmptyString(sUSER_PREFERENCES) )
							{
								XmlDocument xml = InitUserPreferences(sUSER_PREFERENCES);
								Session["USER_PREFERENCES"] = xml.OuterXml;
								// 11/19/2005 Paul.  Not sure why the login screen has the language, but it would seem to allow overriding the default. 
								if ( Sql.IsEmptyString(sCulture) )
								{
									sCulture = XmlUtil.SelectSingleNode(xml, "culture").Replace("_", "-");
								}
								// 11/22/2005 Paul.  The theme can be overridden as well. 
								if ( Sql.IsEmptyString(sTheme) )
								{
									sTheme = XmlUtil.SelectSingleNode(xml, "theme").Replace("_", "-");
								}
								Session["USER_SETTINGS/CULTURE"         ] = sCulture;
								Session["USER_SETTINGS/THEME"           ] = sTheme;
								Session["themeURL"                      ] = sApplicationPath + "Themes/" + sTheme + "/";
								Session["USER_SETTINGS/DATEFORMAT"      ] = XmlUtil.SelectSingleNode(xml, "dateformat"      );
								Session["USER_SETTINGS/TIMEFORMAT"      ] = XmlUtil.SelectSingleNode(xml, "timeformat"      );
								// 01/21/2006 Paul.  It is useful to have quick access to email address. 
								Session["USER_SETTINGS/MAIL_FROMNAME"   ] = XmlUtil.SelectSingleNode(xml, "mail_fromname"   );
								Session["USER_SETTINGS/MAIL_FROMADDRESS"] = XmlUtil.SelectSingleNode(xml, "mail_fromaddress");
								// 05/09/2006 Paul.  Initialize the numeric separators. 
								Session["USER_SETTINGS/GROUP_SEPARATOR"  ] = XmlUtil.SelectSingleNode(xml, "num_grp_sep"    );
								Session["USER_SETTINGS/DECIMAL_SEPARATOR"] = XmlUtil.SelectSingleNode(xml, "dec_sep"        );
								try
								{
									Session["USER_SETTINGS/TIMEZONE"  ] = Sql.ToGuid(XmlUtil.SelectSingleNode(xml, "timezone")).ToString();
								}
								catch
								{
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), "Invalid USER_SETTINGS/TIMEZONE: " + XmlUtil.SelectSingleNode(xml, "timezone"));
								}
								try
								{
									Session["USER_SETTINGS/CURRENCY"  ] = XmlUtil.SelectSingleNode(xml, "currency_id");
								}
								catch
								{
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), "Invalid USER_SETTINGS/CURRENCY: " + XmlUtil.SelectSingleNode(xml, "currency_id"));
								}
								
								DataView vwCurrencies = new DataView(SplendidCache.Currencies());
								vwCurrencies.RowFilter = "ID = '" + XmlUtil.SelectSingleNode(xml, "currency_id") + "'";
								if ( vwCurrencies.Count > 0 )
									Session["USER_SETTINGS/CURRENCY_SYMBOL"] = Sql.ToString(vwCurrencies[0]["SYMBOL"]);
							}
						}
					}
				}
				// 11/21/2005 Paul.  New users may not have any settings, so we need to initialize the defaults.
				// It is best to do it here rather than wrap the variables in a function that would return the default if null.
				sCulture    = Sql.ToString(Session["USER_SETTINGS/CULTURE"   ]);
				sTheme      = Sql.ToString(Session["USER_SETTINGS/THEME"     ]);
				string sDateFormat = Sql.ToString(Session["USER_SETTINGS/DATEFORMAT"]);
				string sTimeFormat = Sql.ToString(Session["USER_SETTINGS/TIMEFORMAT"]);
				string sTimeZone   = Sql.ToString(Session["USER_SETTINGS/TIMEZONE"  ]);
				string sCurrencyID = Sql.ToString(Session["USER_SETTINGS/CURRENCY"  ]);
				if ( Sql.IsEmptyString(sCulture) )
				{
					Session["USER_SETTINGS/CULTURE"   ] = SplendidDefaults.Culture();
				}
				if ( Sql.IsEmptyString(sTheme) )
				{
					sTheme = SplendidDefaults.Theme();
					Session["USER_SETTINGS/THEME"     ] = sTheme;
					Session["themeURL"                ] = sApplicationPath + "Themes/" + sTheme + "/";
				}
				if ( Sql.IsEmptyString(sDateFormat) )
				{
					Session["USER_SETTINGS/DATEFORMAT"] = SplendidDefaults.DateFormat();
				}
				// 11/12/2005 Paul.  "m" is not valid for .NET month formatting.  Must use MM. 
				// 11/12/2005 Paul.  Require 4 digit year.  Otherwise default date in Pipeline of 12/31/2100 would get converted to 12/31/00. 
				if ( SplendidDefaults.IsValidDateFormat(sDateFormat) )
				{
					Session["USER_SETTINGS/DATEFORMAT"] = SplendidDefaults.DateFormat(sDateFormat);
				}
				if ( Sql.IsEmptyString(sTimeFormat) )
				{
					Session["USER_SETTINGS/TIMEFORMAT"] = SplendidDefaults.TimeFormat();
				}
				if ( Sql.IsEmptyString(sCurrencyID) )
				{
					Session["USER_SETTINGS/CURRENCY"  ] = SplendidDefaults.CurrencyID();
				}
				if ( Sql.IsEmptyString(sTimeZone) )
				{
					Session["USER_SETTINGS/TIMEZONE"  ] = SplendidDefaults.TimeZone();
				}

				// 05/09/2006 Paul.  Use defaults when necessary. 
				string sGROUP_SEPARATOR   = Sql.ToString(Session["USER_SETTINGS/GROUP_SEPARATOR"  ]);
				string sDECIMAL_SEPARATOR = Sql.ToString(Session["USER_SETTINGS/DECIMAL_SEPARATOR"]);
				if ( Sql.IsEmptyString(sGROUP_SEPARATOR) )
					Session["USER_SETTINGS/GROUP_SEPARATOR"  ] = SplendidDefaults.GroupSeparator();
				if ( Sql.IsEmptyString(sDECIMAL_SEPARATOR) )
					Session["USER_SETTINGS/DECIMAL_SEPARATOR"] = SplendidDefaults.DecimalSeparator();
			}
		}

		public static void LoadUserACL(Guid gUSER_ID)
		{
			HttpSessionState Session = HttpContext.Current.Session;

			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select MODULE_NAME          " + ControlChars.CrLf
				     + "     , ACLACCESS_ADMIN      " + ControlChars.CrLf
				     + "     , ACLACCESS_ACCESS     " + ControlChars.CrLf
				     + "     , ACLACCESS_VIEW       " + ControlChars.CrLf
				     + "     , ACLACCESS_LIST       " + ControlChars.CrLf
				     + "     , ACLACCESS_EDIT       " + ControlChars.CrLf
				     + "     , ACLACCESS_DELETE     " + ControlChars.CrLf
				     + "     , ACLACCESS_IMPORT     " + ControlChars.CrLf
				     + "     , ACLACCESS_EXPORT     " + ControlChars.CrLf
				     + "  from vwACL_ACCESS_ByUser  " + ControlChars.CrLf
				     + " where USER_ID = @USER_ID   " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@USER_ID", gUSER_ID);
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sMODULE_NAME = Sql.ToString(rdr["MODULE_NAME"]);
							Security.SetUserAccess(sMODULE_NAME, "admin" , Sql.ToInteger(rdr["ACLACCESS_ADMIN" ]));
							Security.SetUserAccess(sMODULE_NAME, "access", Sql.ToInteger(rdr["ACLACCESS_ACCESS"]));
							Security.SetUserAccess(sMODULE_NAME, "view"  , Sql.ToInteger(rdr["ACLACCESS_VIEW"  ]));
							Security.SetUserAccess(sMODULE_NAME, "list"  , Sql.ToInteger(rdr["ACLACCESS_LIST"  ]));
							Security.SetUserAccess(sMODULE_NAME, "edit"  , Sql.ToInteger(rdr["ACLACCESS_EDIT"  ]));
							Security.SetUserAccess(sMODULE_NAME, "delete", Sql.ToInteger(rdr["ACLACCESS_DELETE"]));
							Security.SetUserAccess(sMODULE_NAME, "import", Sql.ToInteger(rdr["ACLACCESS_IMPORT"]));
							Security.SetUserAccess(sMODULE_NAME, "export", Sql.ToInteger(rdr["ACLACCESS_EXPORT"]));
						}
					}
				}
			}
		}

		public static bool LoginUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE, string sUSER_DOMAIN, bool bIS_ADMIN)
		{
			HttpApplicationState Application = HttpContext.Current.Application;
			HttpSessionState     Session     = HttpContext.Current.Session    ;

			bool bValidUser = false;
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				// 03/22/2006 Paul.  The user name should be case-insignificant.  The password is case-significant. 
				// 03/22/2006 Paul.  DB2 does not like lower(USER_NAME) = lower(@USER_NAME).  It returns the following error. 
				// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610 
				// 05/23/2006 Paul.  Use vwUSERS_Login so that USER_HASH can be removed from vwUSERS to prevent its use in reports. 
				sSQL = "select ID                           " + ControlChars.CrLf
				     + "     , USER_NAME                    " + ControlChars.CrLf
				     + "     , FULL_NAME                    " + ControlChars.CrLf
				     + "     , IS_ADMIN                     " + ControlChars.CrLf
				     + "     , STATUS                       " + ControlChars.CrLf
				     + "     , PORTAL_ONLY                  " + ControlChars.CrLf
				     + "  from vwUSERS_Login                " + ControlChars.CrLf
				     + " where lower(USER_NAME) = @USER_NAME" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
/*
#if DEBUG
					if ( sUSER_NAME == "paulrony" && Sql.ToString(Application["SplendidProvider"]) == "MySql.Data" )
						sUSER_NAME = "admin";
#endif
*/
					// 03/22/2006 Paul.  Convert the name to lowercase here. 
					Sql.AddParameter(cmd, "@USER_NAME", sUSER_NAME.ToLower());
					// 11/19/2005 Paul.  sUSER_DOMAIN is used to determine if NTLM is enabled. 
					if ( Sql.IsEmptyString(sUSER_DOMAIN) )
					{
						if ( !Sql.IsEmptyString(sPASSWORD) )
						{
							string sUSER_HASH = Security.HashPassword(sPASSWORD);
							cmd.CommandText += "   and USER_HASH = @USER_HASH" + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@USER_HASH", sUSER_HASH);
						}
						else
						{
							// 11/19/2005 Paul.  Handle the special case of the password stored as NULL or empty string. 
							cmd.CommandText += "   and (USER_HASH = '' or USER_HASH is null)" + ControlChars.CrLf;
						}
					}
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						string sApplicationPath = Sql.ToString(Application["rootURL"]);
						if ( rdr.Read() )
						{
							// 11/19/2005 Paul.  Clear all session values. 
							Session.Clear();
							Security.USER_ID     = Sql.ToGuid   (rdr["ID"         ]);
							Security.USER_NAME   = Sql.ToString (rdr["USER_NAME"  ]);
							Security.FULL_NAME   = Sql.ToString (rdr["FULL_NAME"  ]);
							Security.IS_ADMIN    = Sql.ToBoolean(rdr["IS_ADMIN"   ]);
							Security.PORTAL_ONLY = Sql.ToBoolean(rdr["PORTAL_ONLY"]);
							
							Guid gUSER_ID = Sql.ToGuid(rdr["ID"]);
							// 08/08/2006 Paul.  Don't supply the Language as it prevents the user value from being used. 
							// This bug is a hold-over from the time we removed the Lauguage combo from the login screen. 
							LoadUserPreferences(gUSER_ID, sTHEME, String.Empty);
							LoadUserACL(gUSER_ID);
							bValidUser = true;
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "User login.");
						}
						else if ( Security.IsWindowsAuthentication() )
						{
							// 11/04/2005.  If user does not exist, then create it, but only if NTLM is used. 
							Guid gUSER_ID = Guid.Empty;
							SqlProcs.spUSERS_InsertNTLM(ref gUSER_ID, sUSER_DOMAIN, sUSER_NAME, bIS_ADMIN);

							// 11/19/2005 Paul.  Clear all session values. 
							Session.Clear();
							Security.USER_ID     = gUSER_ID  ;
							Security.USER_NAME   = sUSER_NAME;
							Security.IS_ADMIN    = bIS_ADMIN ;
							Security.PORTAL_ONLY = false     ;
							// 11/21/2005 Paul.  Load the preferences to initialize cuture, date, time and currency preferences.
							LoadUserPreferences(gUSER_ID, String.Empty, String.Empty);
							LoadUserACL(gUSER_ID);
							bValidUser = true;
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "User login.");
						}
						else
						{
							// 11/22/2005 Paul.  Initialize preferences even if login fails so that the theme gets set to the default value. 
							LoadUserPreferences(Guid.Empty, String.Empty, String.Empty);
						}
					}
				}
			}
			return bValidUser; // throw(new Exception("Users.ERR_INVALID_PASSWORD"));
		}

		public static void ChangeTheme(string sTHEME, string sLANGUAGE)
		{
			string sApplicationPath = Sql.ToString(HttpContext.Current.Application["rootURL"]);
			// 04/26/2006 Paul.  The theme variable also needs to be updated.
			HttpContext.Current.Session["USER_SETTINGS/THEME"  ] = sTHEME;
			HttpContext.Current.Session["themeURL"             ] = sApplicationPath + "Themes/" + sTHEME + "/";
			HttpContext.Current.Session["USER_SETTINGS/CULTURE"] = sLANGUAGE;
		}

		public static bool LoginUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE)
		{
			return LoginUser(sUSER_NAME, sPASSWORD, sTHEME, sLANGUAGE, String.Empty, false);
		}

		public static void InitSession()
		{
			InitAppURLs();
			try
			{
				// 11/22/2005 Paul.  Always initialize the theme and language. 
				HttpSessionState Session = HttpContext.Current.Session ;
				Session["USER_SETTINGS/THEME"  ] = SplendidDefaults.Theme();
				Session["USER_SETTINGS/CULTURE"] = SplendidDefaults.Culture();
				Session["themeURL"             ] = Sql.ToString(HttpContext.Current.Application["rootURL"]) + "Themes/" + SplendidDefaults.Theme() + "/";
				// 11/19/2005 Paul.  AUTH_USER is the clear indication that NTLM is enabled. 
				if ( Security.IsWindowsAuthentication() )
				{
					string[] arrUserName = HttpContext.Current.User.Identity.Name.Split('\\');
					string sUSER_DOMAIN = arrUserName[0];
					string sUSER_NAME   = arrUserName[1];
					bool bIS_ADMIN = HttpContext.Current.User.IsInRole("BUILTIN\\Administrators") 
					              || HttpContext.Current.User.IsInRole(sUSER_DOMAIN + "\\SplendidCRM Administrators")
					              || HttpContext.Current.User.IsInRole(HttpContext.Current.Server.MachineName + "\\SplendidCRM Administrators")
					              || HttpContext.Current.User.IsInRole(sUSER_DOMAIN + "\\Domain Admins");
				
					LoginUser(sUSER_NAME, String.Empty, String.Empty, String.Empty, sUSER_DOMAIN, bIS_ADMIN);
				}
				else
				{
					// 11/22/2005 Paul.  Assume portal user for the unauthenticated screen as that is the least restrictive. 
					Security.PORTAL_ONLY = true;
					LoadUserPreferences(Guid.Empty, String.Empty, String.Empty);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void Application_OnError()
		{
			HttpServerUtility Server = HttpContext.Current.Server;
			Exception ex = Server.GetLastError();
			if ( ex != null )
			{
				while ( ex.InnerException != null )
					ex = ex.InnerException;
				string sException = ex.GetType().Name;
				StringBuilder sbMessage = new StringBuilder();
				sbMessage.Append(ex.Message);
				// 03/10/2006 Paul.  .NET 2.0 returns lowercase type names. Use typeof instead. 
				if ( ex.GetType() == typeof(FileNotFoundException) )
				{
					// We can get this error for forbidden files such as web.config and global.asa. 
					//return ; // Return would work if 404 entry was made in web.config. 
					//Response.Redirect("~/Home/FileNotFound.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
					sbMessage = new StringBuilder("File Not Found");
				}
				// 03/10/2006 Paul.  .NET 2.0 returns lowercase type names. Use typeof instead. 
				else if ( ex.GetType() == typeof(HttpException) )
				{
					HttpException exHttp = (HttpException) ex;
					int nHttpCode = exHttp.GetHttpCode();
					if ( nHttpCode == 403 )
					{
						//return ; // Return would work if 403 entry was made in web.config. 
						//Response.Redirect("~/Home/AccessDenied.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
						sbMessage = new StringBuilder("Access Denied");
					}
					else if ( nHttpCode == 404 )
					{
						//return ; // Return would work if 404 entry was made in web.config. 
						//Response.Redirect("~/Home/FileNotFound.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
						sbMessage = new StringBuilder("File Not Found");
					}
				}
				// 03/10/2006 Paul.  .NET 2.0 returns lowercase type names. Use typeof instead. 
				else if ( ex.GetType() == typeof(HttpCompileException) )
				{
					HttpCompileException exCompile = (HttpCompileException) ex;
					CompilerErrorCollection col = exCompile.Results.Errors;
					foreach(CompilerError err in col)
					{
						sbMessage.Append("  ");
						sbMessage.Append(err.ErrorText);
					}
				}
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), sbMessage.ToString());
				Server.ClearError();
				string sQueryString = String.Format("aspxerrorpath={0}&Exception={1}&Message={2}", Server.UrlEncode(HttpContext.Current.Request.Path), sException, Server.UrlEncode(sbMessage.ToString()));
				HttpContext.Current.Response.Redirect("~/Home/ServerError.aspx?" + sQueryString);
			}
		}
	}
}
