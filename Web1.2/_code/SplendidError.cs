// Copyright (C) 2005 SplendidCRM Software, Inc. All rights reserved.
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
using System.Data;
using System.Data.Common;
using System.Web;
using System.Diagnostics;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for SplendidError.
	/// </summary>
	public class SplendidError
	{
		public static void SystemWarning(StackFrame stack, string sMESSAGE)
		{
			SystemMessage("Warning", stack, sMESSAGE);
		}
		
		public static void SystemError(StackFrame stack, string sMESSAGE)
		{
			SystemMessage("Error", stack, sMESSAGE);
		}
		
		private static void SystemMessage(string sERROR_TYPE, StackFrame stack, string sMESSAGE)
		{
			HttpApplicationState Application = HttpContext.Current.Application;
			try
			{
				Application.Lock();
				DataTable dt = Application["SystemErrors"] as DataTable;
				if ( dt == null )
				{
					dt = new DataTable();
					DataColumn colCREATED_BY   = new DataColumn("CREATED_BY"  , Type.GetType("System.Guid"    ));
					DataColumn colDATE_ENTERED = new DataColumn("DATE_ENTERED", Type.GetType("System.DateTime"));
					DataColumn colERROR_TYPE   = new DataColumn("ERROR_TYPE"  , Type.GetType("System.String"  ));
					DataColumn colUSER_NAME    = new DataColumn("USER_NAME"   , Type.GetType("System.String"  ));
					DataColumn colFILE_NAME    = new DataColumn("FILE_NAME"   , Type.GetType("System.String"  ));
					DataColumn colMETHOD       = new DataColumn("METHOD"      , Type.GetType("System.String"  ));
					DataColumn colLINE_NUMBER  = new DataColumn("LINE_NUMBER" , Type.GetType("System.String"  ));
					DataColumn colMESSAGE      = new DataColumn("MESSAGE"     , Type.GetType("System.String"  ));
					dt.Columns.Add(colCREATED_BY  );
					dt.Columns.Add(colDATE_ENTERED);
					dt.Columns.Add(colERROR_TYPE  );
					dt.Columns.Add(colUSER_NAME   );
					dt.Columns.Add(colFILE_NAME   );
					dt.Columns.Add(colMETHOD      );
					dt.Columns.Add(colLINE_NUMBER );
					dt.Columns.Add(colMESSAGE     );
					Application["SystemErrors"] = dt;
				}
				DataRow row = dt.NewRow();
				dt.Rows.Add(row);
				if ( HttpContext.Current.Session != null )
				{
					row["CREATED_BY"  ] = Security.USER_ID         ;
					row["USER_NAME"   ] = Security.USER_NAME       ;
				}
				row["DATE_ENTERED"] = DateTime.Now             ;
				row["ERROR_TYPE"  ] = sERROR_TYPE              ;
				row["MESSAGE"     ] = sMESSAGE                 ;
				if ( stack != null )
				{
					string sFILE_NAME = stack.GetFileName();
					if ( HttpContext.Current.Request != null )
					{
						if ( !Sql.IsEmptyString(sFILE_NAME) )
						{
							// 04/16/2006 Paul.  Use native function to get file name. 
							row["FILE_NAME"] = Path.GetFileName(sFILE_NAME);
						}
					}
					else
					{
						row["FILE_NAME"] = sFILE_NAME;
					}
					row["METHOD"      ] = stack.GetMethod()        ;
					row["LINE_NUMBER" ] = stack.GetFileLineNumber();
				}
			}
			finally
			{
				Application.UnLock();
			}
		}
	}
}
