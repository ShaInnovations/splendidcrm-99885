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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		public static void SetPageTitle(Page page, string sTitle)
		{
			try
			{
				Literal litPageTitle = page.FindControl("litPageTitle") as Literal;
				if ( litPageTitle != null )
					litPageTitle.Text = sTitle;
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
			}
		}

		public static string RegisterEnterKeyPress(string sTextID, string sButtonID)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\">\n");
			sb.Append("document.getElementById('" + sTextID + "').onkeypress = function()\n");
			sb.Append("{\n");
			sb.Append("	if ( (event.which ? event.which : event.keyCode) == 13)\n");
			sb.Append("	{\n");
			sb.Append("		event.returnValue = false;\n");
			sb.Append("		event.cancel = true;\n");
			sb.Append("		document.getElementById('" + sButtonID + "').click();\n");
			sb.Append("	}\n");
			sb.Append("}\n");
			sb.Append("</script>\n");
			return sb.ToString();
		}

		public static string RegisterSetFocus(string sTextID)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\">\n");
			sb.Append("document.getElementById('" + sTextID + "').focus();\n");
			sb.Append("</script>\n");
			return sb.ToString();
		}
	
		public static WebControl CreateArrowControl(bool bAscending)
		{
			Label lblArrow = new Label();
			lblArrow.Font.Name = "Webdings";
			if ( bAscending )
				lblArrow.Text = "5";
			else
				lblArrow.Text = "6";
			return lblArrow;
		}

		public static string ValidateIDs(string[] arrID, bool bQuoted)
		{
			if ( arrID.Length == 0 )
				return String.Empty;
			if ( arrID.Length > 200 )
			{
				L10N L10n = HttpContext.Current.Items["L10n"] as L10N;
				throw(new Exception(L10n.Term(".LBL_TOO_MANY_RECORDS")));
			}
			
			foreach(string sID in arrID)
			{
				Guid gID = Sql.ToGuid(sID);
				if ( Sql.IsEmptyGuid(gID) )
				{
					// 05/02/2006 Paul.  Provide a more descriptive error message by including the ID. 
					throw(new Exception("Invalid ID: " + sID));
				}
			}
			string sIDs = String.Empty;
			if ( bQuoted )
				sIDs = "'" + String.Join("','", arrID) + "'";
			else
				sIDs = String.Join(",", arrID);
			return sIDs;
		}

		public static string ValidateIDs(string[] arrID)
		{
			return ValidateIDs(arrID, false);
		}

		public static string FilterByACL(string sMODULE_NAME, string sACCESS_TYPE, string[] arrID, string sTABLE_NAME)
		{
			StringBuilder sb = new StringBuilder();
			int nACLACCESS = Security.GetUserAccess(sMODULE_NAME, sACCESS_TYPE);
			if ( nACLACCESS >= 0 && arrID.Length > 0 )
			{
				if ( nACLACCESS == ACL_ACCESS.OWNER )
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						string sSQL;
						sSQL = "select ID              " + ControlChars.CrLf
						     + "  from vw" + sTABLE_NAME + ControlChars.CrLf
						     + " where 1 = 1           " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AppendGuids(cmd, arrID, "ID");
							Sql.AppendParameter(cmd, Security.USER_ID, "ASSIGNED_USER_ID", false);
							using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
							{
								while ( rdr.Read() )
								{
									if ( sb.Length > 0 )
										sb.Append(",");
									sb.Append(Sql.ToString(rdr["ID"]));
								}
							}
						}
					}
					if ( sb.Length == 0 )
					{
						L10N L10n = HttpContext.Current.Items["L10n"] as L10N;
						throw(new Exception(L10n.Term("ACL.LBL_INSUFFICIENT_ACCESS")));
					}
				}
				else
				{
					return String.Join(",", arrID);
				}
			}
			return sb.ToString();
		}

		public static void UpdateTracker(Page pParent, string sModule, Guid gID, string sName)
		{
			// 08/21/2005 Paul.  This function is also called after a user clicks Duplicate.
			// In this scenerio, the gID will be NULL, so don't do anything. 
			if ( !Sql.IsEmptyGuid(gID) )
			{
				SqlProcs.spTRACKER_Update(Security.USER_ID, sModule, gID, sName);
				if ( pParent != null )
				{
					_controls.Header ctlHeader = pParent.FindControl("ctlHeader") as _controls.Header;
					if ( ctlHeader != null )
					{
						_controls.LastViewed ctlLastViewed = ctlHeader.FindControl("ctlLastViewed") as _controls.LastViewed;
						if ( ctlLastViewed != null )
						{
							ctlLastViewed.Refresh();
						}
					}
				}
			}
		}

		public static void SetValue(DropDownList lst, string sValue)
		{
			for ( int i=0 ; i < lst.Items.Count; i++ )
			{
				if ( String.Compare(lst.Items[i].Value, sValue, true) == 0 )
				{
					lst.SelectedValue = lst.Items[i].Value;
					break;
				}
			}
		}

		public static string ExpandException(Exception ex)
		{
			StringBuilder sb = new StringBuilder();
			do
			{
				sb.Append(ex.Message);
				sb.Append("  ");
				ex = ex.InnerException;
			}
			while ( ex != null );
			return sb.ToString();
		}

		public static string GetUserEmail(Guid gID)
		{
			string sEmail = String.Empty;
			if ( !Sql.IsEmptyGuid(gID) )
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select EMAIL1  " + ControlChars.CrLf
					     + "     , EMAIL2  " + ControlChars.CrLf
					     + "  from vwUSERS " + ControlChars.CrLf
					     + " where ID = @ID" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@ID", gID);
						using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
						{
							while ( rdr.Read() )
							{
								sEmail = Sql.ToString(rdr["EMAIL1"]);
								if ( Sql.IsEmptyString(sEmail) )
									sEmail = Sql.ToString(rdr["EMAIL2"]);
							}
						}
					}
				}
			}
			return sEmail;
		}

	}
}
