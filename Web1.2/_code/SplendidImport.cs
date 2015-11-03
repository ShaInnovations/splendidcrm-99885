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
using System.Text;
using System.Xml;
using System.Web;
using System.Collections;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for SplendidImport.
	/// </summary>
	public class SplendidImport
	{
		private static void LogError(ref StringBuilder sbErrors, string sCommand, string sMessage)
		{
			sbErrors.Append("<hr width=\"100%\" height=\"2px\" /><table width=\"100%\"><tr><td width=\"50%\">" + sCommand + "</td><td><font color=red>" + sMessage + "</font></td></tr></table>" + ControlChars.CrLf);
		}

		public static void Import(XmlDocument xml, ArrayList arrTables, bool bTruncate)
		{
			HttpResponse Response = HttpContext.Current.Response;
			// 12/16/2005 Paul.  First create a hash table to convert tab name to a uppercase table name. 
			Hashtable hashTables = new Hashtable();
			XmlNodeList nlTables = xml.DocumentElement.ChildNodes;
			foreach(XmlNode node in nlTables)
			{
				if ( !hashTables.ContainsKey(node.Name.ToUpper()) )
					hashTables.Add(node.Name.ToUpper(), node.Name);
			}
			
			ArrayList lstReservedTables = new ArrayList();
			lstReservedTables.Add("CONFIG"                   );
			lstReservedTables.Add("DETAILVIEWS"              );
			lstReservedTables.Add("DETAILVIEWS_FIELDS"       );
			lstReservedTables.Add("DETAILVIEWS_RELATIONSHIPS");
			lstReservedTables.Add("EDITVIEWS"                );
			lstReservedTables.Add("EDITVIEWS_FIELDS"         );
			lstReservedTables.Add("GRIDVIEWS"                );
			lstReservedTables.Add("GRIDVIEWS_COLUMNS"        );
			lstReservedTables.Add("LANGUAGES"                );
			lstReservedTables.Add("MODULES"                  );
			lstReservedTables.Add("SHORTCUTS"                );
			lstReservedTables.Add("TERMINOLOGY"              );
			lstReservedTables.Add("TIMEZONES"                );

			StringBuilder sbErrors = new StringBuilder();
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			if ( arrTables == null )
			{
				arrTables = new ArrayList();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = "select * from vwSqlTableDependencies order by 2, 1";
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								for ( int i = 0 ; i < dt.Rows.Count ; i++ )
								{
									DataRow row = dt.Rows[i];
									arrTables.Add(row["name"].ToString());
								}
							}
						}
						if ( bTruncate )
						{
							cmd.CommandText = "select * from vwSqlTableDependencies order by 2 desc, 1 desc";
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									for ( int i = 0 ; i < dt.Rows.Count && Response.IsClientConnected ; i++ )
									{
										DataRow row = dt.Rows[i];
										string sTABLE_NAME = row["name"].ToString().ToUpper();
										// 12/18/2005 Paul.  Some tables are reserved and should not be truncated or imported. 
										if ( lstReservedTables.Contains(sTABLE_NAME) )
											continue;
										// 12/18/2005 Paul.  Only truncate tables that are being imported. 
										if ( hashTables.ContainsKey(sTABLE_NAME) )
										{
											try
											{
												if ( sTABLE_NAME == "USERS" )
												{
													// 12/17/2005 Paul.  Don't delete the existing user, otherwise it will cause a login problem in the future. 
													cmd.CommandText = "delete from USERS where ID != @ID";
													Sql.AddParameter(cmd, "@ID", Security.USER_ID);
												}
												else
												{
													cmd.CommandText = "delete from " + sTABLE_NAME;
												}
												cmd.ExecuteNonQuery();
												Response.Write(" "); // Write a singe byte to keep the connection open. 
											}
											catch(Exception ex)
											{
												LogError(ref sbErrors, Sql.ExpandParameters(cmd), ex.Message);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			for ( int i = 0 ; i < arrTables.Count && Response.IsClientConnected ; i++ )
			{
				string sTABLE_NAME = arrTables[i].ToString().ToUpper();
				// 12/18/2005 Paul.  Some tables are reserved and should not be truncated or imported. 
				if ( lstReservedTables.Contains(sTABLE_NAME) )
					continue;
				if ( hashTables.ContainsKey(sTABLE_NAME) )
				{
					string sXML_TABLE_NAME = hashTables[sTABLE_NAME].ToString();
					
					XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sXML_TABLE_NAME);
					if ( nlRows.Count > 0 )
					{
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Import Database Table: " + sTABLE_NAME);
						// 12/17/2005 Paul.  Use a new connection for each table import so that connection state will be reset.  
						// My main concern is that the identity_insert gets reset. 
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							try
							{
								if ( Sql.IsSQLServer(con) )
								{
									// 12/17/2005 Paul.  In SQL Server, turn on identity_insert. 
									string sIDENTITY_NAME = String.Empty;
									switch ( sIDENTITY_NAME )
									{
										case "BUGS"     : sIDENTITY_NAME = "BUGS"     ;  break;
										case "CASES"    : sIDENTITY_NAME = "CASES"    ;  break;
										case "CAMPAIGNS": sIDENTITY_NAME = "CAMPAIGNS";  break;
										case "PROSPECTS": sIDENTITY_NAME = "PROSPECTS";  break;
									}
									if ( !Sql.IsEmptyString(sIDENTITY_NAME) )
									{
										IDbCommand cmdIdentity = con.CreateCommand();
										cmdIdentity.CommandText = "set identity_insert " + sIDENTITY_NAME + " on";
										cmdIdentity.ExecuteNonQuery();
									}
								}
								else if ( Sql.IsOracle(con) )
								{
									// 12/17/2005 Paul.  In Oracle, disable sequence triggers. 
									string sTRIGGER_NAME = String.Empty;
									switch ( sTABLE_NAME )
									{
										case "BUGS"     : sTRIGGER_NAME = "TR_S_BUGS_BUG_NUMBER"      ;  break;
										case "CASES"    : sTRIGGER_NAME = "TR_S_CASES_CASE_NUMBER"    ;  break;
										case "CAMPAIGNS": sTRIGGER_NAME = "TR_S_CAMPAIGNS_TRACKER_KEY";  break;
										case "PROSPECTS": sTRIGGER_NAME = "TR_S_PROSPECTS_TRACKER_KEY";  break;
									}
									if ( !Sql.IsEmptyString(sTRIGGER_NAME) )
									{
										IDbCommand cmdTrigger = con.CreateCommand();
										cmdTrigger.CommandText = "alter trigger " + sTRIGGER_NAME + " disable";
										cmdTrigger.ExecuteNonQuery();
									}
								}

								int nTableErrors = 0;
								IDbCommand cmdImport = Sql.CreateInsertParameters(con, sTABLE_NAME);
								foreach(XmlNode node in nlRows)
								{
									if ( !Response.IsClientConnected )
									{
										break;
									}
									foreach(IDataParameter par in cmdImport.Parameters)
									{
										par.Value = DBNull.Value;
									}
									for ( int j = 0; j < node.ChildNodes.Count; j++ )
									{
										// 12/18/2005 Paul.  A short-sighted programmer at SugarCRM created GUIDs with invalid characters. 
										// We need to convert them to valid GUIDs. 
										string sText = node.ChildNodes[j].InnerText;
										// 08/20/2006 Paul.  Dynamically attempt to fix invalid GUIDs. It really only works for the ones defined below. 
										string sName = node.ChildNodes[j].Name.ToUpper();
										if ( sName == "ID" || sName.EndsWith("_ID") )
										{
											if ( sText.Length < 36 )
												sText = "00000000-0000-0000-0000-000000000000".Substring(0, 36 - sText.Length) + sText;
										}
										switch ( sText )
										{
											case "00000000-0000-0000-0000-000000jim_id":  sText = "00000000-0000-0000-0001-000000000000";  break;
											case "00000000-0000-0000-0000-000000max_id":  sText = "00000000-0000-0000-0002-000000000000";  break;
											case "00000000-0000-0000-0000-00000will_id":  sText = "00000000-0000-0000-0003-000000000000";  break;
											case "00000000-0000-0000-0000-0000chris_id":  sText = "00000000-0000-0000-0004-000000000000";  break;
											case "00000000-0000-0000-0000-0000sally_id":  sText = "00000000-0000-0000-0005-000000000000";  break;
											case "00000000-0000-0000-0000-0000sarah_id":  sText = "00000000-0000-0000-0006-000000000000";  break;
										}
										Sql.SetParameter(cmdImport, node.ChildNodes[j].Name, sText);
									}
									// 12/18/2005 Paul.  ID can never be NULL.  SugarCRM does not use an ID in the CONFIG or USERS_FEEDS table. 
									IDbDataParameter parID = Sql.FindParameter(cmdImport, "@ID");
									if ( parID != null )
									{
										// 12/18/2005 Paul. GUIDs from SugarCRM may not be 36 characters. 
										string sID = Sql.ToString(parID.Value);
										if ( parID.Value != DBNull.Value )
										{
											if ( sID.Length < 36 )
											{
												// 07/31/2006 Paul.  Stop using VisualBasic library to increase compatibility with Mono. 
												parID.Value = "00000000-0000-0000-0000-000000000000".Substring(0, 36 - sID.Length) + sID;
											}
										}
										if ( Sql.IsEmptyGuid(parID.Value) )
										{
											if ( parID.DbType == DbType.Guid )
												parID.Value = Guid.NewGuid();
											else
												parID.Value = Guid.NewGuid().ToString();
										}
									}
									// 12/18/2005 Paul.  DATE_ENTERED can never be NULL.  SugarCRM does not use DATE_ENTERED in a number of tables. 
									IDbDataParameter parDATE_ENTERED = Sql.FindParameter(cmdImport, "@DATE_ENTERED");
									if ( parDATE_ENTERED != null )
									{
										if ( parDATE_ENTERED.Value == DBNull.Value )
											parDATE_ENTERED.Value = DateTime.Now;
									}
									// 12/18/2005 Paul.  DATE_MODIFIED can never be NULL.  SugarCRM does not use DATE_MODIFIED in a number of tables. 
									IDbDataParameter parDATE_MODIFIED = Sql.FindParameter(cmdImport, "@DATE_MODIFIED");
									if ( parDATE_MODIFIED != null )
									{
										if ( parDATE_MODIFIED.Value == DBNull.Value )
											parDATE_MODIFIED.Value = DateTime.Now;
									}
									try
									{
										cmdImport.ExecuteNonQuery();
										Response.Write(" ");
									}
									catch(Exception ex)
									{
										LogError(ref sbErrors, Sql.ExpandParameters(cmdImport), ex.Message);
										// 12/17/2005 Paul.  If there is an error, stop importing from this table. 
										// 12/18/2005 Paul.  I'd like to see the first 100 errors. 
										nTableErrors++ ;
										if ( nTableErrors > 100 )
											break;
									}
								}
							}
							catch(Exception ex)
							{
								LogError(ref sbErrors, sTABLE_NAME, ex.Message);
							}
							finally
							{
								try
								{
									if ( Sql.IsSQLServer(con) )
									{
										// 12/17/2005 Paul.  In SQL Server, turn off identity_insert. 
										string sIDENTITY_NAME = String.Empty;
										switch ( sIDENTITY_NAME )
										{
											case "BUGS"     : sIDENTITY_NAME = "BUGS"     ;  break;
											case "CASES"    : sIDENTITY_NAME = "CASES"    ;  break;
											case "CAMPAIGNS": sIDENTITY_NAME = "CAMPAIGNS";  break;
											case "PROSPECTS": sIDENTITY_NAME = "PROSPECTS";  break;
										}
										if ( !Sql.IsEmptyString(sIDENTITY_NAME) )
										{
											IDbCommand cmdIdentity = con.CreateCommand();
											cmdIdentity.CommandText = "set identity_insert " + sIDENTITY_NAME + " off";
											cmdIdentity.ExecuteNonQuery();
										}
									}
									else if ( Sql.IsOracle(con) )
									{
										// 12/17/2005 Paul.  In Oracle, enable sequence triggers. 
										string sTRIGGER_NAME = String.Empty;
										switch ( sTABLE_NAME )
										{
											case "BUGS"     : sTRIGGER_NAME = "TR_S_BUGS_BUG_NUMBER"      ;  break;
											case "CASES"    : sTRIGGER_NAME = "TR_S_CASES_CASE_NUMBER"    ;  break;
											case "CAMPAIGNS": sTRIGGER_NAME = "TR_S_CAMPAIGNS_TRACKER_KEY";  break;
											case "PROSPECTS": sTRIGGER_NAME = "TR_S_PROSPECTS_TRACKER_KEY";  break;
										}
										if ( !Sql.IsEmptyString(sTRIGGER_NAME) )
										{
											IDbCommand cmdTrigger = con.CreateCommand();
											cmdTrigger.CommandText = "alter trigger " + sTRIGGER_NAME + " enable";
											cmdTrigger.ExecuteNonQuery();
										}
									}
								}
								catch(Exception ex)
								{
									LogError(ref sbErrors, sTABLE_NAME, ex.Message);
								}
							}
						}
						Response.Write(" "); // Write a singe byte to keep the connection open. 
					}
				}
			}
			// 12/18/2005 Paul.  Reserved tables will still be imported, but we use the associated spXXX_Update procedure.
			for ( int i = 0 ; i < arrTables.Count && Response.IsClientConnected ; i++ )
			{
				string sTABLE_NAME = arrTables[i].ToString().ToUpper();
				if ( hashTables.ContainsKey(sTABLE_NAME) && lstReservedTables.Contains(sTABLE_NAME) )
				{
					string sXML_TABLE_NAME = hashTables[sTABLE_NAME].ToString();
					
					XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sXML_TABLE_NAME);
					if ( nlRows.Count > 0 )
					{
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Import Database Table: " + sTABLE_NAME);
						// 12/17/2005 Paul.  Use a new connection for each table import so that connection state will be reset.  
						// My main concern is that the identity_insert gets reset. 
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							try
							{
								int nTableErrors = 0;
								IDbCommand cmdImport = SqlProcs.Factory(con, "sp" + sTABLE_NAME + "_Update");
								foreach(XmlNode node in nlRows)
								{
									if ( !Response.IsClientConnected )
									{
										break;
									}
									foreach(IDataParameter par in cmdImport.Parameters)
									{
										par.Value = DBNull.Value;
									}
									for ( int j = 0; j < node.ChildNodes.Count; j++ )
									{
										string sText = node.ChildNodes[j].InnerText;
										Sql.SetParameter(cmdImport, node.ChildNodes[j].Name, sText);
									}
									// 12/18/2005 Paul.  ID can never be NULL.  SugarCRM does not use an ID in the CONFIG or USERS_FEEDS table. 
									IDbDataParameter parID = Sql.FindParameter(cmdImport, "@ID");
									if ( parID != null )
									{
										// 12/18/2005 Paul. GUIDs from SugarCRM may not be 36 characters. 
										string sID = Sql.ToString(parID.Value);
										if ( parID.Value != DBNull.Value )
										{
											if ( sID.Length < 36 )
											{
												// 07/31/2006 Paul.  Stop using VisualBasic library to increase compatibility with Mono. 
												parID.Value = "00000000-0000-0000-0000-000000000000".Substring(0, 36 - sID.Length) + sID;
											}
										}
									}
									try
									{
										cmdImport.ExecuteNonQuery();
										Response.Write(" ");
									}
									catch(Exception ex)
									{
										LogError(ref sbErrors, Sql.ExpandParameters(cmdImport), ex.Message);
										// 12/17/2005 Paul.  If there is an error, stop importing from this table. 
										// 12/18/2005 Paul.  I'd like to see the first 100 errors. 
										nTableErrors++ ;
										if ( nTableErrors > 100 )
											break;
									}
								}
							}
							catch(Exception ex)
							{
								LogError(ref sbErrors, sTABLE_NAME, ex.Message);
							}
						}
						Response.Write(" "); // Write a singe byte to keep the connection open. 
					}
				}
			}
			if ( sbErrors.Length > 0 )
			{
				throw(new Exception(sbErrors.ToString()));
			}
		}
	}
}
