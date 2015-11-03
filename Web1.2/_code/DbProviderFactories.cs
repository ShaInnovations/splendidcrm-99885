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
using System.Web;
using System.Configuration;
using Microsoft.Win32;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for DbProviderFactories.
	/// </summary>
	public class DbProviderFactories
	{
		public static DbProviderFactory GetFactory()
		{
			// 11/14/2005 Paul.  Cache the connection string in the application as config and registry access is expected to be slower. 
			HttpApplicationState Application = HttpContext.Current.Application;
			string sSplendidProvider = Sql.ToString(Application["SplendidProvider"]);
			string sConnectionString = Sql.ToString(Application["ConnectionString"]);
#if DEBUG
//			sSplendidProvider = String.Empty;
#endif
			if ( Sql.IsEmptyString(sSplendidProvider) || Sql.IsEmptyString(sConnectionString) )
			{
				sSplendidProvider = ConfigurationSettings.AppSettings["SplendidProvider"];
				switch ( sSplendidProvider )
				{
					case "System.Data.SqlClient":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidSQLServer"];
						break;
					case "System.Data.OracleClient":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidSystemOracle"];
						break;
					case "Oracle.DataAccess.Client":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidOracle"];
						break;
					case "MySql.Data":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidMySql"];
						break;
					case "IBM.Data.DB2":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidDB2"];
						break;
					case "Sybase.Data.AseClient":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidSybase"];
						break;
					case "iAnywhere.Data.AsaClient":
						sConnectionString = ConfigurationSettings.AppSettings["SplendidSQLAnywhere"];
						break;
					case "Registry":
						string sSplendidRegistry = ConfigurationSettings.AppSettings["SplendidRegistry"];
						if ( Sql.IsEmptyString(sSplendidRegistry) )
						{
							// 11/14/2005 Paul.  If registry key is not provided, then compute it using the server and the application path. 
							// This will allow a single installation to support multiple databases. 
							HttpRequest Request = HttpContext.Current.Request;
							sSplendidRegistry  = "SOFTWARE\\SplendidCRM Software\\" ;
							sSplendidRegistry += Sql.ToString(Request.ServerVariables["SERVER_NAME"]);
							if ( Request.ApplicationPath != "/" )
								sSplendidRegistry += Request.ApplicationPath.Replace("/", "\\");
						}
						using (RegistryKey keySplendidCRM = Registry.LocalMachine.OpenSubKey(sSplendidRegistry))
						{
							if ( keySplendidCRM != null )
							{
								sSplendidProvider = Sql.ToString(keySplendidCRM.GetValue("SplendidProvider"));
								sConnectionString = Sql.ToString(keySplendidCRM.GetValue("ConnectionString"));
							}
							else
							{
								throw(new Exception("Database connection information was not found in the registry " + sSplendidRegistry));
							}
						}
						break;
				}
				Application["SplendidProvider"] = sSplendidProvider;
				Application["ConnectionString"] = sConnectionString;
			}
			return GetFactory(sSplendidProvider, sConnectionString);
		}

		public static DbProviderFactory GetFactory(string sSplendidProvider, string sConnectionString)
		{
			switch ( sSplendidProvider )
			{
				case "System.Data.SqlClient":
				{
					return new SqlClientFactory(sConnectionString);
				}
				case "System.Data.OracleClient":
				{
					return new OracleSystemDataFactory(sConnectionString);
				}
				case "Oracle.DataAccess.Client":
				{
					return new OracleClientFactory(sConnectionString);
				}
				case "MySql.Data":
				{
					return new MySQLClientFactory(sConnectionString);
				}
				case "IBM.Data.DB2":
				{
					return new DB2ClientFactory(sConnectionString);
				}
				case "Sybase.Data.AseClient":
				{
					return new SybaseClientFactory(sConnectionString);
				}
				case "iAnywhere.Data.AsaClient":
				{
					return new SQLAnywhereClientFactory(sConnectionString);
				}
				default:
					throw(new Exception("Unsupported factory " + sSplendidProvider));
			}
		}
	}
}
