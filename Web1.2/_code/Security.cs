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
using System.Text;
using System.Security.Cryptography;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for Security.
	/// </summary>
	public class Security
	{
		public static Guid USER_ID
		{
			get
			{
				// 02/17/2006 Paul.  Throw an exception if Session is null.  This is more descriptive error than "object is null". 
				// We will most likely see this in a SOAP call. 
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				return  Sql.ToGuid(HttpContext.Current.Session["USER_ID"]);
			}
			set
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				HttpContext.Current.Session["USER_ID"] = value;
			}
		}
		
		public static string USER_NAME
		{
			get
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				return  Sql.ToString(HttpContext.Current.Session["USER_NAME"]);
			}
			set
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				HttpContext.Current.Session["USER_NAME"] = value;
			}
		}
		
		public static string FULL_NAME
		{
			get
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				return  Sql.ToString(HttpContext.Current.Session["FULL_NAME"]);
			}
			set
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				HttpContext.Current.Session["FULL_NAME"] = value;
			}
		}
		
		public static bool IS_ADMIN
		{
			get
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				return Sql.ToBoolean(HttpContext.Current.Session["IS_ADMIN"]);
			}
			set
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				HttpContext.Current.Session["IS_ADMIN"] = value;
			}
		}
		
		public static bool PORTAL_ONLY
		{
			get
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				return Sql.ToBoolean(HttpContext.Current.Session["PORTAL_ONLY"]);
			}
			set
			{
				if ( HttpContext.Current.Session == null )
					throw(new Exception("HttpContext.Current.Session is null"));
				HttpContext.Current.Session["PORTAL_ONLY"] = value;
			}
		}
		
		public static bool IsWindowsAuthentication()
		{
			// 11/19/2005 Paul.  AUTH_USER is the clear indication that NTLM is enabled. 
			string sAUTH_USER = Sql.ToString(HttpContext.Current.Request.ServerVariables["AUTH_USER"]);
#if DEBUG
//			sAUTH_USER = String.Empty;
#endif
			return !Sql.IsEmptyString(sAUTH_USER);
		}

		// 11/18/2005 Paul.  SugarCRM stores an MD5 hash of the password. 
		// 11/18/2005 Paul.  SugarCRM also stores the password using the PHP Crypt() function, which is DES. 
		// Don't bother trying to duplicate the PHP Crypt() function because the result is not used in SugarCRM.  
		// The PHP function is located in D:\php-5.0.5\win32\crypt_win32.c
		public static string HashPassword(string sPASSWORD)
		{
			UTF8Encoding utf8 = new UTF8Encoding();
			byte[] aby = utf8.GetBytes(sPASSWORD);
			
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] binMD5 = md5.ComputeHash(aby);
			return Sql.HexEncode(binMD5);
		}

		public static void SetModuleAccess(string sMODULE_NAME, string sACCESS_TYPE, int nACLACCESS)
		{
			if ( HttpContext.Current.Application == null )
				throw(new Exception("HttpContext.Current.Application is null"));
			// 06/04/2006 Paul.  Verify that sMODULE_NAME is not empty.  
			if ( Sql.IsEmptyString(sMODULE_NAME) )
				throw(new Exception("sMODULE_NAME should not be empty."));
			HttpContext.Current.Application["ACLACCESS_" + sMODULE_NAME + "_" + sACCESS_TYPE] = nACLACCESS;
		}

		public static void SetUserAccess(string sMODULE_NAME, string sACCESS_TYPE, int nACLACCESS)
		{
			if ( HttpContext.Current.Session == null )
				throw(new Exception("HttpContext.Current.Session is null"));
			// 06/04/2006 Paul.  Verify that sMODULE_NAME is not empty.  
			if ( Sql.IsEmptyString(sMODULE_NAME) )
				throw(new Exception("sMODULE_NAME should not be empty."));
			HttpContext.Current.Session["ACLACCESS_" + sMODULE_NAME + "_" + sACCESS_TYPE] = nACLACCESS;
		}

		public static int GetUserAccess(string sMODULE_NAME, string sACCESS_TYPE)
		{
			if ( HttpContext.Current.Session == null )
				throw(new Exception("HttpContext.Current.Session is null"));
			// 06/04/2006 Paul.  Verify that sMODULE_NAME is not empty.  
			if ( Sql.IsEmptyString(sMODULE_NAME) )
				throw(new Exception("sMODULE_NAME should not be empty."));
			// 04/27/2006 Paul.  Admins have full access to the site, no matter what the role. 
			if ( IS_ADMIN )
				return 100;
			
			string sAclKey = "ACLACCESS_" + sMODULE_NAME + "_" + sACCESS_TYPE;
			int nACLACCESS = 0;
			// 04/27/2006 Paul.  If no specific level is provided, then look to the Module level. 
			if ( HttpContext.Current.Session[sAclKey] == null )
				nACLACCESS = Sql.ToInteger(HttpContext.Current.Application[sAclKey]);
			else
				nACLACCESS = Sql.ToInteger(HttpContext.Current.Session[sAclKey]);
			if ( sACCESS_TYPE != "access" && nACLACCESS >= 0 )
			{
				// 04/27/2006 Paul.  The access type can over-ride any other type. 
				// A simple trick is to take the minimum of the two values.  
				// If either value is denied, then the result will be negative. 
				sAclKey = "ACLACCESS_" + sMODULE_NAME + "_access";
				int nAccessLevel = 0;
				if ( HttpContext.Current.Session[sAclKey] == null )
					nAccessLevel = Sql.ToInteger(HttpContext.Current.Application[sAclKey]);
				else
					nAccessLevel = Sql.ToInteger(HttpContext.Current.Session[sAclKey]);
				if ( nAccessLevel < 0 )
					nACLACCESS = nAccessLevel;
			}
			return nACLACCESS;
		}
	}
}

