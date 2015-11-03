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
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Security.Principal;
using System.Web.Caching;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for SplendidCache.
	/// </summary>
	public class SplendidCache
	{
		public static DateTime DefaultCacheExpiration()
		{
#if DEBUG
			return DateTime.Now.AddSeconds(1);
#else
			return DateTime.Now.AddDays(1);
#endif
		}

		public static void ClearList(string sLanguage, string sListName)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove(sLanguage + "." + sListName);
		}

		public static DataTable List(string sListName)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;

			L10N L10n = new L10N(HttpContext.Current.Session["USER_SETTINGS/CULTURE"] as string);
			DataTable dt = Cache.Get(L10n.NAME + "." + sListName) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						// 10/13/2005 Paul.  Use distinct because the same list appears to be duplicated in various modules. 
						// appointment_filter_dom is in an Activities and a History module.
						// ORDER BY items must appear in the select list if SELECT DISTINCT is specified. 
						sSQL = "select distinct              " + ControlChars.CrLf
						     + "       NAME                  " + ControlChars.CrLf
						     + "     , DISPLAY_NAME          " + ControlChars.CrLf
						     + "     , LIST_ORDER            " + ControlChars.CrLf
						     + "  from vwTERMINOLOGY         " + ControlChars.CrLf
						     + " where lower(LIST_NAME) = @LIST_NAME" + ControlChars.CrLf  // 03/06/2006 Paul.  Oracle is case sensitive, and we modify the case of L10n.NAME to be lower. 
						     + "   and lower(LANG     ) = @LANG     " + ControlChars.CrLf  // 03/06/2006 Paul.  Oracle is case sensitive, and we modify the case of L10n.NAME to be lower. 
						     + " order by LIST_ORDER         " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							// 03/06/2006 Paul.  Oracle is case sensitive, and we modify the case of L10n.NAME to be lower. 
							Sql.AddParameter(cmd, "@LIST_NAME", sListName.ToLower());
							Sql.AddParameter(cmd, "@LANG"     , L10n.NAME.ToLower());
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert(L10n.NAME + "." + sListName, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
						// 12/03/2005 Paul.  Most lists require data, so if the language-specific list does not exist, just use English. 
						if ( dt.Rows.Count == 0 )
						{
							if ( String.Compare(L10n.NAME, "en-US", true) != 0 )
							{
								using ( IDbCommand cmd = con.CreateCommand() )
								{
									cmd.CommandText = sSQL;
									// 03/06/2006 Paul.  Oracle is case sensitive, and we modify the case of L10n.NAME to be lower. 
									Sql.AddParameter(cmd, "@LIST_NAME", sListName.ToLower());
									Sql.AddParameter(cmd, "@LANG"     , "en-US"  .ToLower());
							
									using ( DbDataAdapter da = dbf.CreateDataAdapter() )
									{
										((IDbDataAdapter)da).SelectCommand = cmd;
										dt = new DataTable();
										da.Fill(dt);
										Cache.Insert(L10n.NAME + "." + sListName, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
									}
								}
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul.  Ignore list errors. 
					// 03/30/2006 Paul.  IBM DB2 is returning an error, which is causing a data-binding error. 
					// SQL1585N A system temporary table space with sufficient page size does not exist. 
					// 03/30/2006 Paul.  In case of error, we should return NULL. 
					return null;
				}
			}
			return dt;
		}

		public static DataTable List(string sModuleName, string sListName)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;

			L10N L10n = new L10N(HttpContext.Current.Session["USER_SETTINGS/CULTURE"] as string);
			DataTable dt = Cache.Get(L10n.NAME + "." + sListName) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select NAME                             " + ControlChars.CrLf
						     + "     , DISPLAY_NAME                     " + ControlChars.CrLf
						     + "  from vwTERMINOLOGY                    " + ControlChars.CrLf
						     + " where lower(MODULE_NAME) = @MODULE_NAME" + ControlChars.CrLf
						     + "   and lower(LIST_NAME  ) = @LIST_NAME  " + ControlChars.CrLf
						     + "   and lower(LANG       ) = @LANG       " + ControlChars.CrLf
						     + " order by LIST_ORDER                    " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							// 03/06/2006 Paul.  Oracle is case sensitive, and we modify the case of L10n.NAME to be lower. 
							Sql.AddParameter(cmd, "@MODULE_NAME", sModuleName.ToLower());
							Sql.AddParameter(cmd, "@LIST_NAME"  , sListName  .ToLower());
							Sql.AddParameter(cmd, "@LANG"       , L10n.NAME  .ToLower());
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert(L10n.NAME + "." + sListName, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static void ClearUsers()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwUSERS_ASSIGNED_TO");
			Cache.Remove("vwUSERS_List");

		}

		public static DataTable AssignedUser()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwUSERS_ASSIGNED_TO") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                 " + ControlChars.CrLf
						     + "     , USER_NAME          " + ControlChars.CrLf
						     + "  from vwUSERS_ASSIGNED_TO" + ControlChars.CrLf
						     + " order by USER_NAME       " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwUSERS_ASSIGNED_TO", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable CustomEditModules()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwCUSTOM_EDIT_MODULES") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select NAME                 " + ControlChars.CrLf
						     + "     , NAME as DISPLAY_NAME " + ControlChars.CrLf
						     + "  from vwCUSTOM_EDIT_MODULES" + ControlChars.CrLf
						     + " order by NAME              " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwCUSTOM_EDIT_MODULES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable ReportingModules()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwMODULES_Reporting_" + Security.USER_ID.ToString()) as DataTable;
			if ( dt == null )
			{
				L10N L10n = new L10N(HttpContext.Current.Session["USER_SETTINGS/CULTURE"] as string);
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select MODULE_NAME             " + ControlChars.CrLf
						     + "     , DISPLAY_NAME            " + ControlChars.CrLf
						     + "  from vwMODULES_Reporting     " + ControlChars.CrLf
						     + " where USER_ID = @USER_ID      " + ControlChars.CrLf
						     + "    or USER_ID is null         " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@USER_ID", Security.USER_ID);
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								foreach(DataRow row in dt.Rows)
								{
									row["DISPLAY_NAME"] = L10n.Term(Sql.ToString(row["DISPLAY_NAME"]));
								}
								Cache.Insert("vwMODULES_Reporting_" + Security.USER_ID.ToString(), dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static string[] ReportingModulesList()
		{
			DataTable dt = SplendidCache.ReportingModules();
			string[] arr = new string[dt.Rows.Count];
			for ( int i=0; i < dt.Rows.Count; i++ )
			{
				arr[i] = Sql.ToString(dt.Rows[i]["MODULE_NAME"]);
			}
			return arr;
		}

		public static DataTable ReportingRelationships()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwRELATIONSHIPS_Reporting") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                        " + ControlChars.CrLf
						     + "  from vwRELATIONSHIPS_Reporting" + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwRELATIONSHIPS_Reporting", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				}
			}
			return dt;
		}

		public static void ClearFilterColumns(string sMODULE_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwSqlColumns_Reporting." + sMODULE_NAME);
		}

		public static DataTable ReportingFilterColumns(string sMODULE_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwSqlColumns_Reporting." + sMODULE_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                       " + ControlChars.CrLf
						     + "  from vwSqlColumns_Reporting  " + ControlChars.CrLf
						     + " where ObjectName = @ObjectName" + ControlChars.CrLf
						     + " order by colid                " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@ObjectName", "vw" + sMODULE_NAME);
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								// 06/10/2006 Paul.  The default sliding scale is not appropriate as columns can be added. 
								Cache.Insert("vwSqlColumns_Reporting." + sMODULE_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				}
			}
			return dt;
		}

		public static DataTable ImportColumns(string sMODULE_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			sMODULE_NAME = sMODULE_NAME.ToUpper();
			DataTable dt = Cache.Get("vwSqlColumns_Import." + sMODULE_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                       " + ControlChars.CrLf
						     + "  from vwSqlColumns_Import     " + ControlChars.CrLf
						     + " where ObjectName = @ObjectName" + ControlChars.CrLf
						     + " order by colid                " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@ObjectName", "sp" + sMODULE_NAME + "_Update");
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								// 06/10/2006 Paul.  The default sliding scale is not appropriate as columns can be added. 
								Cache.Insert("vwSqlColumns_Import." + sMODULE_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				}
			}
			return dt;
		}

		public static DataTable Release()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwRELEASES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                " + ControlChars.CrLf
						     + "     , NAME              " + ControlChars.CrLf
						     + "  from vwRELEASES_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER     " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwRELEASES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable ProductCategories()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwPRODUCT_CATEGORIES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                          " + ControlChars.CrLf
						     + "     , NAME                        " + ControlChars.CrLf
						     + "  from vwPRODUCT_CATEGORIES_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER               " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwPRODUCT_CATEGORIES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable ProductTypes()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwPRODUCT_TYPES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                     " + ControlChars.CrLf
						     + "     , NAME                   " + ControlChars.CrLf
						     + "  from vwPRODUCT_TYPES_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER          " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwPRODUCT_TYPES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable Manufacturers()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwMANUFACTURERS_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                     " + ControlChars.CrLf
						     + "     , NAME                   " + ControlChars.CrLf
						     + "  from vwMANUFACTURERS_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER          " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwMANUFACTURERS_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable Shippers()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwSHIPPERS_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                " + ControlChars.CrLf
						     + "     , NAME              " + ControlChars.CrLf
						     + "  from vwSHIPPERS_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER     " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwSHIPPERS_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable TaxRates()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwTAX_RATES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                 " + ControlChars.CrLf
						     + "     , NAME               " + ControlChars.CrLf
						     + "  from vwTAX_RATES_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER      " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwTAX_RATES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable ContractTypes()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwCONTRACT_TYPES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                      " + ControlChars.CrLf
						     + "     , NAME                    " + ControlChars.CrLf
						     + "  from vwCONTRACT_TYPES_LISTBOX" + ControlChars.CrLf
						     + " order by LIST_ORDER           " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwCONTRACT_TYPES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable Currencies()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwCURRENCIES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                  " + ControlChars.CrLf
						     + "     , NAME                " + ControlChars.CrLf
						     + "     , SYMBOL              " + ControlChars.CrLf
						     + "     , NAME_SYMBOL         " + ControlChars.CrLf
						     + "     , CONVERSION_RATE     " + ControlChars.CrLf
						     + "  from vwCURRENCIES_LISTBOX" + ControlChars.CrLf
						     + " order by NAME             " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwCURRENCIES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable Timezones()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwTIMEZONES") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *           " + ControlChars.CrLf
						     + "  from vwTIMEZONES " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwTIMEZONES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable TimezonesListbox()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwTIMEZONES_LISTBOX") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID                 " + ControlChars.CrLf
						     + "     , NAME               " + ControlChars.CrLf
						     + "  from vwTIMEZONES_LISTBOX" + ControlChars.CrLf
						     + " order by BIAS desc       " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwTIMEZONES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static void ClearLanguages()
		{
			HttpContext.Current.Cache.Remove("vwLANGUAGES");
		}

		public static DataTable Languages()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwLANGUAGES") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select NAME        " + ControlChars.CrLf
						     + "     , NATIVE_NAME " + ControlChars.CrLf
						     + "     , DISPLAY_NAME" + ControlChars.CrLf
						     + "  from vwLANGUAGES " + ControlChars.CrLf
						     + " order by NAME     " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwLANGUAGES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable Modules()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwMODULES") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select MODULE_NAME   " + ControlChars.CrLf
						     + "     , DISPLAY_NAME  " + ControlChars.CrLf
						     + "  from vwMODULES     " + ControlChars.CrLf
						     + " order by MODULE_NAME" + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwMODULES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static void ClearTerminologyPickLists()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwTERMINOLOGY_PickList");
		}

		public static DataTable TerminologyPickLists()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwTERMINOLOGY_PickList") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                     " + ControlChars.CrLf
						     + "  from vwTERMINOLOGY_PickList" + ControlChars.CrLf
						     + " order by LIST_NAME          " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwTERMINOLOGY_PickList", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static DataTable ActiveUsers()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwUSERS_List") as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select ID          " + ControlChars.CrLf
						     + "     , USER_NAME   " + ControlChars.CrLf
						     + "  from vwUSERS_List" + ControlChars.CrLf
						     + " order by USER_NAME" + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								// 09/16/2005 Paul.  Users change a lot, so have a very short timeout. 
								Cache.Insert("vwUSERS_List", dt, null, DateTime.Now.AddSeconds(15), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 10/16/2005 Paul. Ignore list errors. 
				}
			}
			return dt;
		}

		public static void ClearTabMenu()
		{
			//System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			//Cache.Remove("vwMODULES_TabMenu");
			// 04/28/2006 Paul.  The menu is now cached in the Session, so it will only get cleared when the user logs out. 
			HttpContext.Current.Session.Remove("vwMODULES_TabMenu_ByUser." + Security.USER_ID.ToString());
		}

		public static DataTable TabMenu()
		{
			//System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			// 04/28/2006 Paul.  The menu is now cached in the Session, so it will only get cleared when the user logs out. 
			HttpSessionState Session = HttpContext.Current.Session;
			// 04/28/2006 Paul.  Include the GUID in the USER_ID to that the user does not have to log-out in order to get the correct menu. 
			DataTable dt = Session["vwMODULES_TabMenu_ByUser." + Security.USER_ID.ToString()] as DataTable;
			if ( dt == null )
			{
				dt = new DataTable();
				try
				{
					if ( !Sql.IsEmptyGuid(Security.USER_ID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							string sSQL;
							sSQL = "select MODULE_NAME             " + ControlChars.CrLf
							     + "     , DISPLAY_NAME            " + ControlChars.CrLf
							     + "     , RELATIVE_PATH           " + ControlChars.CrLf
							     + "  from vwMODULES_TabMenu_ByUser" + ControlChars.CrLf
							     + " where USER_ID = @USER_ID      " + ControlChars.CrLf
							     + "    or USER_ID is null         " + ControlChars.CrLf
							     + " order by TAB_ORDER            " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@USER_ID", Security.USER_ID);
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									da.Fill(dt);
									Session["vwMODULES_TabMenu_ByUser." + Security.USER_ID.ToString()] = dt;
								}
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
				}
			}
			return dt;
		}

		public static DataTable Shortcuts(string sMODULE_NAME)
		{
			//System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			// 04/28/2006 Paul.  The shortcuts is now cached in the Session, so it will only get cleared when the user logs out. 
			// 04/28/2006 Paul.  Include the GUID in the USER_ID to that the user does not have to log-out in order to get the correct menu. 
			DataTable dt = HttpContext.Current.Session["vwSHORTCUTS_Menu_ByUser." + sMODULE_NAME + "." + Security.USER_ID.ToString()] as DataTable;
			if ( dt == null )
			{
				dt = new DataTable();
				try
				{
					if ( !Sql.IsEmptyGuid(Security.USER_ID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							string sSQL;
							sSQL = "select MODULE_NAME               " + ControlChars.CrLf
							     + "     , DISPLAY_NAME              " + ControlChars.CrLf
							     + "     , RELATIVE_PATH             " + ControlChars.CrLf
							     + "     , IMAGE_NAME                " + ControlChars.CrLf
							     + "  from vwSHORTCUTS_Menu_ByUser   " + ControlChars.CrLf
							     + " where MODULE_NAME = @MODULE_NAME" + ControlChars.CrLf
							     + "   and (USER_ID = @USER_ID or USER_ID is null)" + ControlChars.CrLf
							     + " order by SHORTCUT_ORDER         " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@MODULE_NAME", sMODULE_NAME);
								Sql.AddParameter(cmd, "@USER_ID"    , Security.USER_ID);
								
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									da.Fill(dt);
									HttpContext.Current.Session["vwSHORTCUTS_Menu_ByUser." + sMODULE_NAME + "." + Security.USER_ID.ToString()] = dt;
								}
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
				}
			}
			return dt;
		}

		public static DataTable Themes()
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("Themes") as DataTable;
			if ( dt == null )
			{
				try
				{
					dt = new DataTable();
					dt.Columns.Add("NAME", Type.GetType("System.String"));
					
					FileInfo objInfo = null;
					string[] arrDirectories = Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/Themes"));
					for ( int i = 0 ; i < arrDirectories.Length ; i++ )
					{
						// 12/04/2005 Paul.  Only include theme if an images folder exists.  This is a quick test. 
						// 08/14/2006 Paul.  Mono uses a different slash than Windows, so use Path.Combine(). 
						if ( Directory.Exists(Path.Combine(arrDirectories[i], "images")) )
						{
							DataRow row = dt.NewRow();
							objInfo = new FileInfo(arrDirectories[i]);
							row["NAME"] = objInfo.Name;
							dt.Rows.Add(row);
						}
					}
					// 11/19/2005 Paul.  The themes cache need never expire as themes almost never change. 
					Cache.Insert("Themes", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				}
			}
			return dt;
		}

		public static string XmlFile(string sPATH_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			string sDATA = Cache.Get("XmlFile." + sPATH_NAME) as string;
			if ( sDATA == null )
			{
				try
				{
					using ( StreamReader rd = new StreamReader(sPATH_NAME, System.Text.Encoding.UTF8) )
					{
						sDATA = rd.ReadToEnd();
					}
					// 11/19/2005 Paul.  The file cache need never expire as themes almost never change. 
					Cache.Insert("XmlFile." + sPATH_NAME, sDATA, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					throw(new Exception("Could not load file: " + sPATH_NAME, ex));
				}
			}
			return sDATA;
		}

		public static void ClearGridView(string sGRID_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwGRIDVIEWS_COLUMNS." + sGRID_NAME);
		}

		public static DataTable GridViewColumns(string sGRID_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwGRIDVIEWS_COLUMNS." + sGRID_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						// 01/09/2006 Paul.  Exclude DEFAULT_VIEW. 
						sSQL = "select *                     " + ControlChars.CrLf
						     + "  from vwGRIDVIEWS_COLUMNS   " + ControlChars.CrLf
						     + " where GRID_NAME = @GRID_NAME" + ControlChars.CrLf
						     + "   and (DEFAULT_VIEW = 0 or DEFAULT_VIEW is null)" + ControlChars.CrLf
						     + " order by COLUMN_INDEX       " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@GRID_NAME", sGRID_NAME);
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwGRIDVIEWS_COLUMNS." + sGRID_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
					dt = new DataTable();
				}
			}
			return dt;
		}

		public static void ClearDetailView(string sDETAIL_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwDETAILVIEWS_FIELDS." + sDETAIL_NAME);
		}

		public static DataTable DetailViewFields(string sDETAIL_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwDETAILVIEWS_FIELDS." + sDETAIL_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						// 01/09/2006 Paul.  Exclude DEFAULT_VIEW. 
						sSQL = "select *                         " + ControlChars.CrLf
						     + "  from vwDETAILVIEWS_FIELDS      " + ControlChars.CrLf
						     + " where DETAIL_NAME = @DETAIL_NAME" + ControlChars.CrLf
						     + "   and (DEFAULT_VIEW = 0 or DEFAULT_VIEW is null)" + ControlChars.CrLf
						     + " order by FIELD_INDEX            " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@DETAIL_NAME", sDETAIL_NAME);
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwDETAILVIEWS_FIELDS." + sDETAIL_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
					dt = new DataTable();
				}
			}
			return dt;
		}

		public static DataTable DetailViewRelationships(string sDETAIL_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwDETAILVIEWS_RELATIONSHIPS." + sDETAIL_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                          " + ControlChars.CrLf
						     + "  from vwDETAILVIEWS_RELATIONSHIPS" + ControlChars.CrLf
						     + " where DETAIL_NAME = @DETAIL_NAME " + ControlChars.CrLf
						     + " order by RELATIONSHIP_ORDER      " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@DETAIL_NAME", sDETAIL_NAME);
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwDETAILVIEWS_RELATIONSHIPS." + sDETAIL_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
					dt = new DataTable();
				}
			}
			return dt;
		}

		public static void ClearEditView(string sEDIT_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwEDITVIEWS_FIELDS." + sEDIT_NAME);
		}

		public static DataTable EditViewFields(string sEDIT_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwEDITVIEWS_FIELDS." + sEDIT_NAME) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						// 01/09/2006 Paul.  Exclude DEFAULT_VIEW. 
						sSQL = "select *                     " + ControlChars.CrLf
						     + "  from vwEDITVIEWS_FIELDS    " + ControlChars.CrLf
						     + " where EDIT_NAME = @EDIT_NAME" + ControlChars.CrLf
						     + "   and (DEFAULT_VIEW = 0 or DEFAULT_VIEW is null)" + ControlChars.CrLf
						     + " order by FIELD_INDEX        " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@EDIT_NAME", sEDIT_NAME);
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwEDITVIEWS_FIELDS." + sEDIT_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
					dt = new DataTable();
				}
			}
			return dt;
		}

		public static void ClearFieldsMetaData(string sMODULE_NAME)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			Cache.Remove("vwFIELDS_META_DATA_Validated." + sMODULE_NAME);
			ClearFilterColumns(sMODULE_NAME);
		}

		public static DataTable FieldsMetaData_Validated(string sMODULE)
		{
			System.Web.Caching.Cache Cache = HttpContext.Current.Cache;
			DataTable dt = Cache.Get("vwFIELDS_META_DATA_Validated." + sMODULE) as DataTable;
			if ( dt == null )
			{
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                             " + ControlChars.CrLf
						     + "  from vwFIELDS_META_DATA_Validated  " + ControlChars.CrLf
						     + " where CUSTOM_MODULE = @CUSTOM_MODULE" + ControlChars.CrLf
						     + " order by colid                      " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@CUSTOM_MODULE", sMODULE);
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								dt = new DataTable();
								da.Fill(dt);
								Cache.Insert("vwFIELDS_META_DATA_Validated." + sMODULE, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
							}
						}
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
					// 11/21/2005 Paul. Ignore error, but then we need to find a way to display the connection error. 
					// The most likely failure here is a connection failure. 
					dt = new DataTable();
				}
			}
			return dt;
		}
	}
}
