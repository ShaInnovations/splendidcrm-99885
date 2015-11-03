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
using System.Text;
using System.Diagnostics;
//using Microsoft.VisualBasic;
using ICSharpCode.SharpZipLib.Zip;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for LanguagePackImport.
	/// </summary>
	public class LanguagePackImport
	{
		/*
		$mod_strings = array (
		//DON'T CONVERT THESE THEY ARE MAPPINGS
		  array (
		 ),

		);

		$mod_list_strings = Array(
		);

		$app_list_strings = array (
		$app_strings = array (
		*/

		public static string GetModuleName(string sPathName)
		{
			string[] arrFolders = Path.GetDirectoryName(sPathName).Split('\\');
			string strModuleName = String.Empty;
			if ( arrFolders.Length >= 2 )
			{
				if ( String.Compare(arrFolders[arrFolders.Length-1], "language", true) == 0 )
				{
					strModuleName = arrFolders[arrFolders.Length-2];
					if ( String.Compare(strModuleName, "include", true) == 0 )
						strModuleName = null;
				}
			}
			return strModuleName;
		}

		public static string GetLanguage(string sPathName)
		{
			string sLang = Path.GetFileName(sPathName).Split('.')[0];
			sLang = sLang.Replace("_", "-");
			if ( sLang.IndexOf("-") >= 0 )
			{
				string[] arrLang = sLang.Split('-');
				sLang = arrLang[0] + '-' + arrLang[1].ToUpper();
			}
			return sLang;
		}

		public static Encoding GetEncoding(string sLang)
		{
			// 10/09/2005 Paul.  I expected UTF-8 to always work, but it does not.  Only use UTF-8 for known languages. 
			System.Text.Encoding enc = Encoding.UTF7;
			if ( sLang.StartsWith("ja") || sLang.StartsWith("tw") || sLang.StartsWith("zh") || sLang.StartsWith("cz") )
				enc = Encoding.UTF8;
			return enc;
		}

		public static void InsertTerms(string sPathName, bool bForceUTF8)
		{
			string sModuleName = GetModuleName(sPathName);
			string sLang       = GetLanguage  (sPathName);
			System.Text.Encoding enc = GetEncoding(sLang);
			if ( bForceUTF8 )
				enc = Encoding.UTF8;
			using ( StreamReader sr = new StreamReader(sPathName, enc, true) )
			{
				string sData = sr.ReadToEnd();
				int nListStart = sData.IndexOf("$app_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$app_strings", sData, nListStart);
				nListStart = sData.IndexOf("$app_list_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$app_list_strings", sData, nListStart);
				nListStart = sData.IndexOf("$mod_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$mod_strings", sData, nListStart);
				nListStart = sData.IndexOf("$mod_list_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$mod_list_strings", sData, nListStart);
			}
		}

		public static void InsertTerms(string sPathName, ZipInputStream stmZip, bool bForceUTF8)
		{
			string sModuleName = GetModuleName(sPathName);
			string sLang       = GetLanguage  (sPathName);
			System.Text.Encoding enc = GetEncoding(sLang);
			if ( bForceUTF8 )
				enc = Encoding.UTF8;
			// 01/12/2006 Paul.  Don't use using as it is clsing the zip stream. 
			StreamReader sr = new StreamReader(stmZip, enc, true);
			{
				string sData = String.Empty;
				try
				{
					// 01/12/2006 Paul.  Problem importing it_it_3.5.1.langpack.zip, modules/OptimisticLock/language/it_it.lang.php
					// Specified argument was out of the range of valid values. Parameter name: length 
					// Catch the error and allow processing of the rest of the files. 
					sData = sr.ReadToEnd();
				}
				catch(Exception ex)
				{
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), sPathName + "; " + ex.Message);
					return;
				}
				int nListStart = sData.IndexOf("$app_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$app_strings", sData, nListStart);
				nListStart = sData.IndexOf("$app_list_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$app_list_strings", sData, nListStart);
				nListStart = sData.IndexOf("$mod_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$mod_strings", sData, nListStart);
				nListStart = sData.IndexOf("$mod_list_strings");
				if ( nListStart >= 0 )
					LanguagePackImport.ParseList(sModuleName, sLang, "$mod_list_strings", sData, nListStart);
			}
		}

		public static void ParseList(string sModuleName, string sLang, string strListGroup, string sData, int nListStart)
		{
			nListStart = sData.IndexOf("(", nListStart);
			if ( nListStart >= 0 )
			{
				nListStart++;
				int nMode = 0;  // 
				int nListOrder = 0 ;
				string strTemp = String.Empty;
				string strListName    = String.Empty;
				string strEntryName   = String.Empty;
				string strDisplayName = String.Empty;
				bool bInsideArray = false;
				bool bContinueParsing = true;
				while ( bContinueParsing )
				{
					switch ( nMode )
					{
						case 0:  // Search for next entry. 
							while ( Char.IsWhiteSpace(sData, nListStart) || sData[nListStart] == ',' )
							{
								nListStart++;
								if ( nListStart >= sData.Length )
								{
									bContinueParsing = false;
									break;
								}
							}
							if ( bInsideArray && sData[nListStart] == ')' )
							{
								nListStart++;
								bInsideArray = false;  // turn off array processing. 
								strListName = String.Empty;
								nListOrder = 0;
								nMode = 0;
							}
							else
							{
								nMode = 1;
							}
							break;
						case 1:  // Extract entry name. 
						{
							if ( nListStart >= sData.Length || (sData[nListStart] == ')' && sData[nListStart+1] == ';') )
							{
								bContinueParsing = false;
								break;
							}
							char chPunctuation = sData[nListStart];
							if ( chPunctuation != '\'' && chPunctuation != '\"' )
							{
								strTemp = sData.Substring(nListStart, Math.Min(100, sData.Length-nListStart));
								if ( sData[nListStart] == '/' && sData[nListStart+1] == '/' )
								{
									// Ignore comment. 
									// 05/19/2006 Paul.  lang-pt_br12-351b.zip ends the line with '\n'
									while ( nListStart < sData.Length && sData[nListStart] != '\r' && sData[nListStart] != '\n' )
										nListStart++;
								}
								// If character is not expected, then exit. 
								nListStart++;
								nMode = 0;
								break;
							}
							nListStart++;
							int nValueStart = nListStart;
							while ( chPunctuation != sData[nListStart] && nListStart < sData.Length )
								nListStart++;
							int nValueEnd = nListStart;
							nListStart++;
							if ( nValueEnd - nValueStart == 0 )
								nMode = 0 ;
							else
							{
								strEntryName = sData.Substring(nValueStart, nValueEnd - nValueStart);
								nMode = 2;
							}
							break;
						}
						case 2: // Search for display name or inner list
						{
							while ( Char.IsWhiteSpace(sData, nListStart) )
							{
								nListStart++;
								// 10/09/2005 Paul.  Just in case the file has been truncated, 
								// exit if there are not enough characters for a fully formed display name.
								if ( nListStart+4 >= sData.Length )
								{
									bContinueParsing = false;
									break;
								}
							}
							if ( sData[nListStart] == '=' && sData[nListStart+1] == '>' )
							{
								nListStart++;
								nListStart++;
								while ( Char.IsWhiteSpace(sData, nListStart) )
									nListStart++;
								char chPunctuation = sData[nListStart];
								if ( chPunctuation != '\'' && chPunctuation != '\"' )
								{
									string strArray = sData.Substring(nListStart, Math.Min(5, sData.Length-nListStart));
									if ( strArray == "array" )
									{
										nListStart += 5;
										while ( Char.IsWhiteSpace(sData, nListStart) || sData[nListStart] == '(' )
											nListStart++;
										bInsideArray = true;
										strListName = strEntryName;
										nListOrder = 0 ;
										nMode = 0;
										break;
									}
									else
									{
										strTemp = sData.Substring(nListStart, Math.Min(100, sData.Length-nListStart));
										// If character is not expected, then exit. 
										nListStart++;
										nMode = 0;
									}
									break;
								}
								nListStart++;
								int nValueStart = nListStart;
								// 10/09/2005 Paul.  Watch out for 'LBL_CURRENCY_SYMBOL' => '\\', where the punctuation end has a slash before it. 
								//  'NTC_LOGIN_MESSAGE' => 'Prego fai login all\\\'applicazione.',
								// 10/09/2005 Paul.  Use the bLastCharExcaped flag as a simple way to keep track of escaped characters 
								// to prevent early termination or allow proper termination of the string. 
								bool bLastCharExcaped = false;
								while ( nListStart < sData.Length && (chPunctuation != sData[nListStart] || (chPunctuation == sData[nListStart] && sData[nListStart-1] == '\\' && !bLastCharExcaped)) )
								{
									// If last character was escaped, then don't escape this one. 
									if ( !bLastCharExcaped && sData[nListStart-1] == '\\' )
										bLastCharExcaped = true;
									else
										bLastCharExcaped = false;
									nListStart++;
								}
								int nValueEnd = nListStart;
								nListStart++;
								if ( nValueEnd - nValueStart == 0 )
									nMode = 0 ;
								else
								{
									strDisplayName = sData.Substring(nValueStart, nValueEnd - nValueStart);
									// 10/09/2005 Paul.  Need to remove escape characters. 
									if ( strDisplayName.IndexOf('\\', 0) >= 0 )
									{
										// 10/09/2005 Paul. We must escape twice because we have escapes within escapes. \\\' should become '.  
										// First, escape the punctuation delimiter. 
										strDisplayName = strDisplayName.Replace("\\" + chPunctuation.ToString(), chPunctuation.ToString());
										// Second, escape the escape character. 
										strDisplayName = strDisplayName.Replace("\\\\", "\\");
										// Now escape all other known escape values. 
										strDisplayName = strDisplayName.Replace("\\\'", "'");
										strDisplayName = strDisplayName.Replace("\\\"", "\"");
										strDisplayName = strDisplayName.Replace("\\n", ControlChars.CrLf);
									}
									if ( strListName.Length > 0 )
									{
										nListOrder++;
									}
									SqlProcs.spTERMINOLOGY_Update(strEntryName, sLang, sModuleName, strListName, nListOrder, strDisplayName);
									nMode = 0;
								}
							}
							else
							{
								strTemp = sData.Substring(nListStart, Math.Min(100, sData.Length-nListStart));
								// If character is not expected, then exit. 
								nListStart++;
								nMode = 0;
								break;
							}
							break;
						}
						default:
							bContinueParsing = false;
							break;
					}
					if ( nListStart >= sData.Length )
						break;
				}
			}
		}

	}
}
