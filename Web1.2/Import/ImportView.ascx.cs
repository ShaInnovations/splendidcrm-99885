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
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Xml;
using System.Text;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Import
{
	/// <summary>
	///		Summary description for ImportView.
	/// </summary>
	public class ImportView : SplendidControl
	{
		protected _controls.ModuleHeader ctlModuleHeader              ;
		protected _controls.ListHeader   ctlListHeader                ;
		protected HtmlGenericControl     divImportStep1               ;
		protected HtmlGenericControl     divImportStep2               ;
		protected HtmlGenericControl     divImportStep3               ;
		protected HtmlGenericControl     divImportStep4               ;
		protected HtmlGenericControl     divLastImported              ;
		protected HtmlTable              tblInstructionsExcel         ;
		protected HtmlTable              tblInstructionsXmlSpreadsheet;
		protected HtmlTable              tblInstructionsXML           ;
		protected HtmlTable              tblInstructionsSalesForce    ;
		protected HtmlTable              tblInstructionsAct           ;
		protected HtmlTable              tblInstructionsCommaDelimited;
		protected HtmlTable              tblInstructionsTabDelimited  ;
		protected HtmlTable              tblNotesAccounts             ;
		protected HtmlTable              tblNotesContacts             ;
		protected HtmlTable              tblNotesOpportunities        ;
		protected RadioButton            radEXCEL                     ;
		protected RadioButton            radXML_SPREADSHEET           ;
		protected RadioButton            radXML                       ;
		protected RadioButton            radSALESFORCE                ;
		protected RadioButton            radACT_2005                  ;
		protected RadioButton            radCUSTOM_CSV                ;
		protected RadioButton            radCUSTOM_TAB                ;
		protected Button                 btnBack                      ;
		protected Button                 btnNext                      ;
		protected Button                 btnImport                    ;
		protected Button                 btnFinish                    ;
		
		protected DataView               vwMain                       ;
		protected SplendidGrid           grdMain                      ;
		protected Label                  lblError                     ;

		protected bool                   bDebug                       ;
		protected XmlDocument            xml                          ;
		protected string                 sImportModule                ;
		protected HtmlInputFile          fileIMPORT                   ;
		protected RequiredFieldValidator reqFILENAME                  ;
		protected CheckBox               chkHasHeader                 ;
		protected CheckBox               chkSaveMap                   ;
		protected TextBox                txtSaveMap                   ;
		protected int                    nImportStep                  ;
		protected HtmlTable              tblImportMappings            ;
		protected Hashtable              hashFieldMappings            ;
		protected StringBuilder          sbImport                     ;

		protected Label                  lblStatus                    ;
		protected Label                  lblSuccessCount              ;
		protected Label                  lblFailedCount               ;

		public string Module
		{
			get { return sImportModule; }
			set { sImportModule = value; }
		}

		private void ShowStep()
		{
			divImportStep1 .Visible = (nImportStep == 1);
			divImportStep2 .Visible = (nImportStep == 2);
			divImportStep3 .Visible = (nImportStep == 3);
			divImportStep4 .Visible = (nImportStep == 4);
			divLastImported.Visible = divImportStep4.Visible;
			btnBack  .Visible = (nImportStep >  1 && nImportStep < 4);
			btnNext  .Visible = (nImportStep >= 1 && nImportStep < 4);
			btnImport.Visible = divImportStep4.Visible;
			btnFinish.Visible = divImportStep4.Visible;
			if ( nImportStep == 2 )
			{
				tblInstructionsExcel         .Visible = radEXCEL          .Checked;
				tblInstructionsXmlSpreadsheet.Visible = radXML_SPREADSHEET.Checked;
				tblInstructionsXML           .Visible = radXML            .Checked;
				tblInstructionsSalesForce    .Visible = radSALESFORCE     .Checked;
				tblInstructionsAct           .Visible = radACT_2005       .Checked;
				tblInstructionsCommaDelimited.Visible = radCUSTOM_CSV     .Checked;
				tblInstructionsTabDelimited  .Visible = radCUSTOM_TAB     .Checked;
			}
			switch ( nImportStep )
			{
				case 1:  ctlModuleHeader.TitleText = L10n.Term("Import.LBL_MODULE_NAME") + " " + L10n.Term("Import.LBL_STEP_1_TITLE");  break;
				case 2:  ctlModuleHeader.TitleText = L10n.Term("Import.LBL_MODULE_NAME") + " " + L10n.Term("Import.LBL_STEP_2_TITLE");  break;
				case 3:  ctlModuleHeader.TitleText = L10n.Term("Import.LBL_MODULE_NAME") + " " + L10n.Term("Import.LBL_STEP_3_TITLE");  break;
				case 4:  ctlModuleHeader.TitleText = L10n.Term("Import.LBL_MODULE_NAME") + " " + L10n.Term("Import.LBL_RESULTS"     );  break;
			}
			if ( nImportStep == 3 )
				btnNext.Text = "  " + L10n.Term("Import.LBL_IMPORT_NOW" ) + "  ";
			else
				btnNext.Text = "  " + L10n.Term(".LBL_NEXT_BUTTON_LABEL") + "  ";
		}

		private DataView ModuleColumns()
		{
			DataTable dtColumns = SplendidCache.ImportColumns(sImportModule).Copy();
			foreach ( DataRow row in dtColumns.Rows )
			{
				row["DISPLAY_NAME"] = TableColumnName(sImportModule, Sql.ToString(row["DISPLAY_NAME"]));
			}
			DataView vwColumns = new DataView(dtColumns);
			vwColumns.Sort = "DISPLAY_NAME";
			return vwColumns;
		}

		private void UpdateImportMappings(bool bInitialize)
		{
			hashFieldMappings = new Hashtable();

			tblImportMappings.Rows.Clear();
			HtmlTableRow rowHeader = new HtmlTableRow();
			tblImportMappings.Rows.Add(rowHeader);
			HtmlTableCell cellField  = new HtmlTableCell();
			HtmlTableCell cellRowHdr = new HtmlTableCell();
			HtmlTableCell cellRow1   = new HtmlTableCell();
			HtmlTableCell cellRow2   = new HtmlTableCell();
			rowHeader.Cells.Add(cellField );
			if ( chkHasHeader.Checked || radXML.Checked )
				rowHeader.Cells.Add(cellRowHdr);
			rowHeader.Cells.Add(cellRow1  );
			rowHeader.Cells.Add(cellRow2  );
			cellField .Attributes.Add("class", "tabDetailViewDL");
			cellRowHdr.Attributes.Add("class", "tabDetailViewDL");
			cellRow1  .Attributes.Add("class", "tabDetailViewDL");
			cellRow2  .Attributes.Add("class", "tabDetailViewDL");
			cellField .Attributes.Add("style", "TEXT-ALIGN: left");
			cellRowHdr.Attributes.Add("style", "TEXT-ALIGN: left");
			cellRow1  .Attributes.Add("style", "TEXT-ALIGN: left");
			cellRow2  .Attributes.Add("style", "TEXT-ALIGN: left");
			Label lblField  = new Label();
			Label lblRowHdr = new Label();
			Label lblRow1   = new Label();
			Label lblRow2   = new Label();
			cellField .Controls.Add(lblField );
			cellRowHdr.Controls.Add(lblRowHdr);
			cellRow1  .Controls.Add(lblRow1  );
			cellRow2  .Controls.Add(lblRow2  );
			lblField .Font.Bold = true;
			lblRowHdr.Font.Bold = true;
			lblRow1  .Font.Bold = true;
			lblRow2  .Font.Bold = true;
			lblField .Text = L10n.Term("Import.LBL_DATABASE_FIELD");
			lblRowHdr.Text = L10n.Term("Import.LBL_HEADER_ROW"    );
			lblRow1  .Text = L10n.Term("Import.LBL_ROW"           ) + " 1";
			lblRow2  .Text = L10n.Term("Import.LBL_ROW"           ) + " 2";
			
			string sXml = Sql.ToString(ViewState["xml"]);
			xml.LoadXml(sXml);
			XmlNodeList nl = xml.DocumentElement.SelectNodes(sImportModule.ToLower());
			if ( nl.Count > 0 )
			{
				DataView vwColumns = ModuleColumns();
				XmlNode nodeH = nl[0];
				XmlNode node1 = nl[0];
				XmlNode node2 = null;
				// 08/22/2006 Paul.  An XML Spreadsheet will have a header record, 
				// so don't assume that an XML file will use the tag names as the header. 
				if ( chkHasHeader.Checked )
				{
					if ( nl.Count > 1 )
						node1 = nl[1];
					if ( nl.Count > 2 )
						node2 = nl[2];
				}
				else
				{
					if ( nl.Count > 1 )
						node2 = nl[1];
				}
				bool bDuplicateFields = false;
				Hashtable hashSelectedFields = new Hashtable();
				for ( int i = 0 ; i < nodeH.ChildNodes.Count ; i++ )
				{
					rowHeader = new HtmlTableRow();
					tblImportMappings.Rows.Add(rowHeader);
					cellField  = new HtmlTableCell();
					cellRowHdr = new HtmlTableCell();
					cellRow1   = new HtmlTableCell();
					cellRow2   = new HtmlTableCell();
					rowHeader.Cells.Add(cellField );
					if ( chkHasHeader.Checked || radXML.Checked )
						rowHeader.Cells.Add(cellRowHdr);
					rowHeader.Cells.Add(cellRow1  );
					if ( node2 != null && i < node2.ChildNodes.Count )
						rowHeader.Cells.Add(cellRow2);
					cellField .Attributes.Add("class", "tabDetailViewDF");
					cellRowHdr.Attributes.Add("class", "tabDetailViewDF");
					cellRow1  .Attributes.Add("class", "tabDetailViewDF");
					cellRow2  .Attributes.Add("class", "tabDetailViewDF");
					DropDownList lstField  = new DropDownList();
					lblRowHdr = new Label();
					lblRow1   = new Label();
					lblRow2   = new Label();
					cellField .Controls.Add(lstField );
					cellRowHdr.Controls.Add(lblRowHdr);
					cellRow1  .Controls.Add(lblRow1  );
					cellRow2  .Controls.Add(lblRow2  );
					
					// 08/20/2006 Paul.  Clear any previous filters. 
					vwColumns.RowFilter = null;
					// 08/20/2006 Paul.  Don't use real column names as they may collide.
					lstField.ID             = "ImportField" + i.ToString("000");
					lstField.DataValueField = "NAME";
					lstField.DataTextField  = "DISPLAY_NAME";
					lstField.DataSource     = vwColumns;
					lstField.DataBind();
					lstField.Items.Insert(0, new ListItem(L10n.Term("Import.LBL_DONT_MAP"), String.Empty));
					try
					{
						if ( bInitialize )
						{
							if ( chkHasHeader.Checked )
							{
								// 08/22/2006 Paul.  If Has Header is checked, then always expect the body to contain the header names. 
								string sFieldName = nodeH.ChildNodes[i].InnerText.Trim();
								// 08/20/2006 Paul.  Use the DataView to locate matching fields so that we don't have to worry about case significance. 
								vwColumns.RowFilter = "NAME = '" + sFieldName.Replace("'", "''") + "' or DISPLAY_NAME = '" + sFieldName.Replace("'", "''") + "'";
								if ( vwColumns.Count == 1 )
								{
									hashFieldMappings.Add(i, Sql.ToString(vwColumns[0]["NAME"]));
									lstField.SelectedValue = Sql.ToString(vwColumns[0]["NAME"]);
								}
							}
							else if ( radXML.Checked )
							{
								// 08/22/2006 Paul.  If Has Header is not checked for XML, then use the tag ame as the field name. 
								string sFieldName = nodeH.ChildNodes[i].Name;
								// 08/20/2006 Paul.  Use the DataView to locate matching fields so that we don't have to worry about case significance. 
								vwColumns.RowFilter = "NAME = '" + sFieldName.Replace("'", "''") + "' or DISPLAY_NAME = '" + sFieldName.Replace("'", "''") + "'";
								if ( vwColumns.Count == 1 )
								{
									hashFieldMappings.Add(i, Sql.ToString(vwColumns[0]["NAME"]));
									lstField.SelectedValue = Sql.ToString(vwColumns[0]["NAME"]);
								}
							}
							else
								hashFieldMappings.Add(i, "ImportField" + i.ToString("000"));
						}
						else
						{
							// 08/20/2006 Paul.  Manually set the last value. 
							hashFieldMappings.Add(i, Sql.ToString(Request[lstField.UniqueID]));
							lstField.SelectedValue = Sql.ToString(Request[lstField.UniqueID]);
							if ( lstField.SelectedValue.Length > 0 )
							{
								if ( hashSelectedFields.ContainsKey(lstField.SelectedValue) )
								{
									bDuplicateFields = true;
								}
								else
								{
									hashSelectedFields.Add(lstField.SelectedValue, null);
								}
							}
						}
					}
					catch
					{
					}
					// XML data will use the node-name as the header. 
					if ( chkHasHeader.Checked )
					{
						// 08/22/2006 Paul.  If Has Header is checked, then always expect the body to contain the header names. 
						lblRowHdr.Text = nodeH.ChildNodes[i].InnerText;
					}
					else if ( radXML.Checked )
					{
						// 08/22/2006 Paul.  If Has Header is not checked for XML, then use the tag ame as the field name. 
						lblRowHdr.Text = nodeH.ChildNodes[i].Name;
					}
					
					if ( node1 != null && i < node1.ChildNodes.Count )
						lblRow1.Text = node1.ChildNodes[i].InnerText;
					if ( node2 != null && i < node2.ChildNodes.Count )
						lblRow2.Text = node2.ChildNodes[i].InnerText;
				}
				if ( bDuplicateFields )
				{
					throw(new Exception(L10n.Term("Import.ERR_MULTIPLE")));
				}
				if ( !bInitialize )
				{
					switch ( sImportModule )
					{
						case "Accounts":
							if ( !hashSelectedFields.ContainsKey("NAME") )
								throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Accounts.LBL_LIST_ACCOUNT_NAME")) );
							break;
						case "Contacts":
							if ( !hashSelectedFields.ContainsKey("LAST_NAME") )
								throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Contacts.LBL_LIST_LAST_NAME")) );
							break;
						case "Opportunities":
							StringBuilder sb = new StringBuilder();
							if ( !hashSelectedFields.ContainsKey("NAME") )
								sb.Append(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_NAME") + ControlChars.CrLf);
							if ( !hashSelectedFields.ContainsKey("ACCOUNT_NAME") )
								sb.Append(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_ACCOUNT_NAME") + ControlChars.CrLf);
							if ( !hashSelectedFields.ContainsKey("DATE_CLOSED") )
								sb.Append(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_DATE_CLOSED") + ControlChars.CrLf);
							if ( !hashSelectedFields.ContainsKey("SALES_STAGE") )
								sb.Append(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_SALES_STAGE") + ControlChars.CrLf);
							if ( sb.Length > 0 )
								throw ( new Exception(sb.ToString()) );
							break;
					}
				}
			}
		}

		protected void GenerateImport()
		{
			try
			{
				string sXml = Sql.ToString(ViewState["xml"]);
				xml.LoadXml(sXml);
				
				// 08/20/2006 Paul.  Also map the header names to allow for a flexible XML. 
				Hashtable hashHeaderMappings = new Hashtable();
				XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sImportModule.ToLower());
				if ( nlRows.Count == 0 )
					throw(new Exception("Nothing to import."));
				
				// 08/22/2006 Paul.  We should always use the header mappings instead of an index as nodes may move around. 
				XmlNode node = nlRows[0];
				for ( int j = 0; j < node.ChildNodes.Count; j++ )
				{
					hashHeaderMappings.Add(node.ChildNodes[j].Name, hashFieldMappings[j]);
				}
				
				int nImported = 0;
				int nFailed   = 0;
				//int nSkipped  = 0;

				DataTable dtProcessed = new DataTable();
				dtProcessed.Columns.Add("Import_Row_Status", typeof(bool));
				dtProcessed.Columns.Add("Import_Row_Number", typeof(Int32));
				dtProcessed.Columns.Add("Import_Row_Error"  );
				dtProcessed.Columns.Add("Import_Last_Column");
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Import Database Table: " + sImportModule);
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					IDbCommand cmdImport = SqlProcs.Factory(con, "sp" + sImportModule.ToUpper() + "_Update");
					int i = 0;
					if ( chkHasHeader.Checked )
						i++;
					for ( int iRowNumber = 1; i < nlRows.Count ; i++ )
					{
						node = nlRows[i];
						int nEmptyColumns = 0;
						for ( int j = 0; j < node.ChildNodes.Count; j++ )
						{
							string sText = node.ChildNodes[j].InnerText;
							if ( sText == String.Empty )
								nEmptyColumns++;
						}
						// 09/04/2006 Paul.  If all columns are empty, then skip the row. 
						if ( nEmptyColumns == node.ChildNodes.Count )
							continue;
						DataRow row = dtProcessed.NewRow();
						row["Import_Row_Number"] = iRowNumber ;
						iRowNumber++;
						dtProcessed.Rows.Add(row);
						try
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
								string sName = String.Empty;
								// 08/22/2006 Paul.  We should always use the header mappings instead of an index as nodes may move around. 
								//if ( radXML.Checked )
									sName = Sql.ToString(hashHeaderMappings[node.ChildNodes[j].Name]);
								//else
								//	sName = Sql.ToString(hashFieldMappings[j]);  // node.ChildNodes[j].Name
								// 09/08/2006 Paul.  There is no need to set the field if the value is empty. 
								if ( sName.Length > 0 && sText.Length > 0 )
								{
									sName = sName.ToUpper();
									// 08/20/2006 Paul.  Fix IDs. 
									if ( ( sName == "ID" || sName.EndsWith("_ID") ) && sName.Length < 36 && sText.Length > 0 )
									{
										sText = "00000000-0000-0000-0000-000000000000".Substring(0, 36 - sText.Length) + sText;
										switch ( sText )
										{
											case "00000000-0000-0000-0000-000000jim_id":  sText = "00000000-0000-0000-0001-000000000000";  break;
											case "00000000-0000-0000-0000-000000max_id":  sText = "00000000-0000-0000-0002-000000000000";  break;
											case "00000000-0000-0000-0000-00000will_id":  sText = "00000000-0000-0000-0003-000000000000";  break;
											case "00000000-0000-0000-0000-0000chris_id":  sText = "00000000-0000-0000-0004-000000000000";  break;
											case "00000000-0000-0000-0000-0000sally_id":  sText = "00000000-0000-0000-0005-000000000000";  break;
											case "00000000-0000-0000-0000-0000sarah_id":  sText = "00000000-0000-0000-0006-000000000000";  break;
										}
									}
									if ( !dtProcessed.Columns.Contains(sName) )
									{
										dtProcessed.Columns.Add(sName);
									}
									row["Import_Row_Status" ] = true ;
									row["Import_Last_Column"] = sName;
									row[sName] = sText;
									Sql.SetParameter(cmdImport, sName, sText);
								}
							}
							sbImport.Append(Sql.ExpandParameters(cmdImport));
							sbImport.Append(";");
							sbImport.Append(ControlChars.CrLf);
							cmdImport.ExecuteNonQuery();
							nImported++;
							row["Import_Last_Column"] = DBNull.Value;
							Response.Write(" ");
						}
						catch(Exception ex)
						{
							row["Import_Row_Status"] = false;
							row["Import_Row_Error" ] = "Error: " + Sql.ToString(row["Import_Last_Column"]) + ". " + ex.Message;
							nFailed++;
						}
					}
				}
				if ( nFailed == 0 )
					lblStatus.Text = L10n.Term("Import.LBL_SUCCESS"      );
				lblSuccessCount.Text = nImported.ToString() + " " + L10n.Term("Import.LBL_SUCCESSFULLY" );
				lblFailedCount.Text  = nFailed.ToString()   + " " + L10n.Term("Import.LBL_FAILED_IMPORT");

				DataView vwColumns = ModuleColumns();
				Hashtable hashColumns = new Hashtable();
				foreach ( DataRowView row in vwColumns )
					hashColumns.Add(row["NAME"], row["DISPLAY_NAME"]);
				BoundColumn bnd = new BoundColumn();
				bnd.DataField  = "Import_Row_Number";
				bnd.HeaderText = "Row";
				grdMain.Columns.Add(bnd);
				for ( int i = 4; i < dtProcessed.Columns.Count; i++ )
				{
					bnd = new BoundColumn();
					bnd.DataField = dtProcessed.Columns[i].ColumnName;
					if ( hashColumns.ContainsKey(bnd.DataField) )
						bnd.HeaderText = hashColumns[bnd.DataField] as string;
					else
						bnd.HeaderText = bnd.DataField;
					grdMain.Columns.Add(bnd);
				}
				bnd = new BoundColumn();
				bnd.DataField  = "Import_Row_Error";
				bnd.HeaderText = "Status";
				grdMain.Columns.Add(bnd);
				
				DataView vwProcessed = new DataView(dtProcessed);
				vwProcessed.Sort = "Import_Row_Status, Import_Row_Number";
				grdMain.DataSource = vwProcessed;
				grdMain.DataBind();
			}
			catch ( Exception ex )
			{
				lblError.Text += ex.Message;
			}
		}

		protected XmlDocument ConvertTableToXml(DataTable dt, string sRecordName)
		{
			XmlDocument xml = new XmlDocument();
			xml.AppendChild(xml.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
			xml.AppendChild(xml.CreateElement("xml"));
			foreach ( DataRow row in dt.Rows )
			{
				XmlNode xRecord = xml.CreateElement(sRecordName);
				xml.DocumentElement.AppendChild(xRecord);
				for ( int nField = 0; nField < dt.Columns.Count; nField++ )
				{
					XmlNode xField = xml.CreateElement("ImportField" + nField.ToString("000"));
					xRecord.AppendChild(xField);
					if ( row[nField] != DBNull.Value )
					{
						xField.InnerText = row[nField].ToString();
					}
				}
			}
			return xml;
		}

		protected static void ConvertTextToXml(ref XmlDocument xml, string sRecordName, Stream stm, char chFieldSeparator)
		{
			int nMaxField = 0;
			xml = new XmlDocument();
			xml.AppendChild(xml.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
			xml.AppendChild(xml.CreateElement("xml"));
			using ( TextReader reader = new StreamReader(stm) )
			{
				string sLine = null;
				while ( (sLine = reader.ReadLine()) != null )
				{
					if ( sLine.Length == 0 )
						continue;

					XmlNode xRecord = xml.CreateElement(sRecordName);
					xml.DocumentElement.AppendChild(xRecord);
					int i = 0;
					int nMode = 0;
					int nField = 0;
					bool bContinueParsing = true;
					while ( bContinueParsing )
					{
						switch ( nMode )
						{
							case 0:  // Search for next entry. 
							{
								if ( chFieldSeparator == ControlChars.Tab )
								{
									// Don't skip the tab when it is used as a separator. 
									while ( Char.IsWhiteSpace(sLine[i]) && sLine[i] != ControlChars.Tab )
										i++;
								}
								else
								{
									while ( Char.IsWhiteSpace(sLine[i]) )
										i++;
								}
								nMode = 1;
								break;
							}
							case 1:  // Determine if field is quoted or unquoted. 
							{
								// first check if field is empty. 
								char chPunctuation = sLine[i];
								if ( chPunctuation == chFieldSeparator )
								{
									i++;
									XmlNode xField = xml.CreateElement("ImportField" + nField.ToString("000"));
									xRecord.AppendChild(xField);
									nField++;
									nMode = 0;
								}
								if ( chPunctuation == '\"' )
								{
									i++;
									// Field is quoted, so start reading until next quote. 
									nMode = 3;
								}
								else
								{
									// Field is unquoted, so start reading until next separator or end-of-line.
									nMode = 2;
								}
								break;
							}
							case 2:  // Extract unquoted field. 
							{
								XmlNode xField = xml.CreateElement("ImportField" + nField.ToString("000"));
								xRecord.AppendChild(xField);
								nField++;
								
								int nFieldStart = i;
								// Field is unquoted, so start reading until next separator or end-of-line.
								while ( i < sLine.Length && sLine[i] != chFieldSeparator )
									i++;
								int nFieldEnd = i;
								
								string sField = sLine.Substring(nFieldStart, nFieldEnd-nFieldStart);
								xField.InnerText = sField;
								nMode = 0;
								i++;
								break;
							}
							case 3:  // Extract quoted field. 
							{
								XmlNode xField = xml.CreateElement("ImportField" + nField.ToString("000"));
								xRecord.AppendChild(xField);
								nField++;
								
								int nFieldStart = i;
								// Field is quoted, so start reading until next quote.  Watch out for an escaped quote (two double quotes). 
								while ( ( i < sLine.Length && sLine[i] != '\"' ) || ( i + 1 < sLine.Length && sLine[i] == '\"' && sLine[i+1] == '\"' ) )
								{
									if ( i + 1 < sLine.Length && sLine[i] == '\"' && sLine[i+1] == '\"' )
										i++;
									i++;
								}
								int nFieldEnd = i;
								// Skip all characters until we reach the separator or end-of-line. 
								while ( i < sLine.Length && sLine[i] != chFieldSeparator )
									i++;
								
								string sField = sLine.Substring(nFieldStart, nFieldEnd-nFieldStart);
								sField = sField.Replace("\"\"", "\"");
								xField.InnerText = sField;
								nMode = 0;
								i++;
								break;
							}
							default:
								bContinueParsing = false;
								break;
						}
						if ( i >= sLine.Length )
							break;
					}
					nMaxField = Math.Max(nField, nMaxField);
				}
			}
			XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sRecordName);
			if ( nlRows.Count > 0 )
			{
				// If the first record does not have all the fields, then add the missing fields. 
				XmlNode xNode = nlRows[0];
				while ( xNode.ChildNodes.Count < nMaxField )
				{
					XmlNode xField = xml.CreateElement("ImportField" + xNode.ChildNodes.Count.ToString("000"));
					xNode.AppendChild(xField);
				}
			}
		}

		protected void ConvertXmlSpreadsheetToXml(ref XmlDocument xml, string sRecordName)
		{
			XmlDocument xmlImport = new XmlDocument();
			xmlImport.AppendChild(xmlImport.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
			xmlImport.AppendChild(xmlImport.CreateElement("xml"));
			
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
			string sSpreadsheetNamespace = "urn:schemas-microsoft-com:office:spreadsheet";
			nsmgr.AddNamespace("ss", sSpreadsheetNamespace);

			// 08/22/2006 Paul.  The Spreadsheet namespace is also the default namespace, so make sure to prefix nodes with ss.
			XmlNode xWorksheet = xml.DocumentElement.SelectSingleNode("ss:Worksheet", nsmgr);
			if ( xWorksheet != null )
			{
				XmlNode xTable = xWorksheet.SelectSingleNode("ss:Table", nsmgr);
				if ( xTable != null )
				{
					int nColumnCount = 0;
					XmlNode xColumnCount = xTable.Attributes.GetNamedItem("ss:ExpandedColumnCount");
					if ( xColumnCount != null )
						nColumnCount = Sql.ToInteger(xColumnCount.Value);
					XmlNodeList nlRows = xTable.SelectNodes("ss:Row", nsmgr);
					if ( nlRows.Count > 0 )
					{
						// 08/22/2006 Paul.  The first row is special in that we must make sure that all nodes exist. 
						XmlNode xRow = nlRows[0];
						if ( nColumnCount == 0 )
							nColumnCount = xRow.ChildNodes.Count;
						for ( int i = 0; i < nlRows.Count; i++ )
						{
							XmlNode xRecord = xmlImport.CreateElement(sRecordName);
							xmlImport.DocumentElement.AppendChild(xRecord);
							xRow = nlRows[i];
							
							for ( int j = 0, nField = 0; j < xRow.ChildNodes.Count; j++, nField++ )
							{
								XmlNode xField = xmlImport.CreateElement("ImportField" + nField.ToString("000"));
								xRecord.AppendChild(xField);
								XmlNode xCell = xRow.ChildNodes[j];
								int nCellIndex = 0;
								XmlNode xCellIndex = xCell.Attributes.GetNamedItem("ss:Index");
								if ( xCellIndex != null )
									nCellIndex = Sql.ToInteger(xCellIndex.Value);
								// 08/22/2006 Paul.  If there are any missing cells, then add them.
								while ( (nField + 1) < nCellIndex )
								{
									nField++;
									xField = xmlImport.CreateElement("ImportField" + nField.ToString("000"));
									xRecord.AppendChild(xField);
								}
								if ( xCell.ChildNodes.Count > 0 )
								{
									if ( xCell.ChildNodes[0].Name == "Data" )
									{
										xField.InnerText = xCell.ChildNodes[0].InnerText;
									}
								}
							}
						}
					}
				}
			}
			
			xml = xmlImport;
		}

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Next" )
				{
					switch ( nImportStep )
					{
						case 1:
						{
							nImportStep++;
							ShowStep();
							ViewState["ImportStep"] = nImportStep;
							break;
						}
						case 2:
						{
							if ( Page.IsValid )
							{
								HttpPostedFile pstIMPORT = fileIMPORT.PostedFile;
								if ( pstIMPORT != null )
								{
									if ( pstIMPORT.FileName.Length > 0 )
									{
										string sFILENAME       = Path.GetFileName (pstIMPORT.FileName);
										string sFILE_EXT       = Path.GetExtension(sFILENAME);
										string sFILE_MIME_TYPE = pstIMPORT.ContentType;
										if ( radXML_SPREADSHEET.Checked )
										{
											xml.Load(pstIMPORT.InputStream);
											ConvertXmlSpreadsheetToXml(ref xml, sImportModule.ToLower());
										}
										else if ( sFILE_MIME_TYPE == "text/xml" || radXML.Checked )
										{
											using ( MemoryStream mstm = new MemoryStream() )
											{
												using ( BinaryWriter mwtr = new BinaryWriter(mstm) )
												{
													using ( BinaryReader reader = new BinaryReader(pstIMPORT.InputStream) )
													{
														byte[] binBYTES = reader.ReadBytes(8 * 1024);
														while ( binBYTES.Length > 0 )
														{
															for ( int i = 0; i < binBYTES.Length; i++ )
															{
																// MySQL dump seems to dump binary 0 & 1 for byte values. 
																if ( binBYTES[i] == 0 )
																	mstm.WriteByte(Convert.ToByte('0'));
																else if ( binBYTES[i] == 1 )
																	mstm.WriteByte(Convert.ToByte('1'));
																else
																	mstm.WriteByte(binBYTES[i]);
															}
															binBYTES = reader.ReadBytes(8 * 1024);
														}
													}
													mwtr.Flush();
													mstm.Seek(0, SeekOrigin.Begin);
													xml.Load(mstm);
													bool bExcelSheet = false;
													foreach ( XmlNode xNode in xml )
													{
														if ( xNode.NodeType == XmlNodeType.ProcessingInstruction )
														{
															if ( xNode.Name == "mso-application" && xNode.InnerText == "progid=\"Excel.Sheet\"" )
															{
																bExcelSheet = true;
																break;
															}
														}
													}
													if ( bExcelSheet )
														ConvertXmlSpreadsheetToXml(ref xml, sImportModule.ToLower());
												}
											}
										}
										else if ( radEXCEL.Checked )
										{
											ExcelDataReader.ExcelDataReader spreadsheet = new ExcelDataReader.ExcelDataReader(pstIMPORT.InputStream);
											if ( spreadsheet.WorkbookData.Tables.Count > 0 )
											{
												xml = ConvertTableToXml(spreadsheet.WorkbookData.Tables[0], sImportModule.ToLower());
											}
										}
										else if ( radCUSTOM_TAB.Checked )
										{
											CsvDataReader spreadsheet = new CsvDataReader(pstIMPORT.InputStream, ControlChars.Tab);
											if ( spreadsheet.Table != null )
											{
												xml = ConvertTableToXml(spreadsheet.Table, sImportModule.ToLower());
											}
										}
										else
										{
											// 08/21/2006 Paul.  Everything else is comma separated.  Convert to XML. 
											CsvDataReader spreadsheet = new CsvDataReader(pstIMPORT.InputStream, ',');
											if ( spreadsheet.Table != null )
											{
												xml = ConvertTableToXml(spreadsheet.Table, sImportModule.ToLower());
											}
										}
									}
								}
							}
							if ( xml.DocumentElement == null )
								throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
							
							// 08/21/2006 Paul.  Don't move to next step if there is no data. 
							XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sImportModule.ToLower());
							if ( nlRows.Count == 0 )
								throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
							ViewState["xml"] = xml.OuterXml;
							UpdateImportMappings(true);
							
							nImportStep++ ;
							ShowStep();
							ViewState["ImportStep"] = nImportStep;
							break;
						}
						case 3:
						{
							UpdateImportMappings(false);
							GenerateImport();
							nImportStep++ ;
							ShowStep();
							ViewState["ImportStep"] = nImportStep;
							break;
						}
					}
				}
				else if ( e.CommandName == "Back" )
				{
					if ( nImportStep > 1 )
					{
						nImportStep--;
						ShowStep();
						ViewState["ImportStep"] = nImportStep;
					}
				}
				else if ( e.CommandName == "ImportMore" )
				{
					//radEXCEL.Checked = true;
					//chkHasHeader.Checked = true;
					//nImportStep = 1;
					//ShowStep();
					//ViewState["ImportStep"] = nImportStep;
					// 08/20/2006 Paul.  Redirecting is a safer way to reset all variables. 
					Response.Redirect(Request.Path);
				}
				else if ( e.CommandName == "Finish" )
				{
					Response.Redirect("~/" + sImportModule);
				}
			}
			catch(Exception ex)
			{
				//SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
				return;
			}
		}

		private string TableColumnName(string sModule, string sDISPLAY_NAME)
		{
			// 07/04/2006 Paul.  Some columns have global terms. 
			if (  sDISPLAY_NAME == "DATE_ENTERED" 
			   || sDISPLAY_NAME == "DATE_MODIFIED"
			   || sDISPLAY_NAME == "ASSIGNED_TO"  
			   || sDISPLAY_NAME == "CREATED_BY"   
			   || sDISPLAY_NAME == "MODIFIED_BY"  )
			{
				sDISPLAY_NAME = L10n.Term(".LBL_" + sDISPLAY_NAME).Replace(":", "");
			}
			else
			{
				// 07/04/2006 Paul.  Column names are aliased so that we don't have to redefine terms. 
				sDISPLAY_NAME = L10n.AliasedTerm(sModule + ".LBL_" + sDISPLAY_NAME).Replace(":", "");
			}
			return sDISPLAY_NAME;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term("Import.LBL_MODULE_NAME"));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "import") >= 0);
			if ( !this.Visible )
				return;
#if DEBUG
			bDebug = true;
#endif

			xml = new XmlDocument();
			sbImport = new StringBuilder();
			try
			{
				// 07/02/2006 Paul.  The required fields need to be bound manually. 
				reqFILENAME.DataBind();
				// 12/17/2005 Paul.  Don't buffer so that the connection can be kept alive. 
				Response.BufferOutput = false;
				if ( !IsPostBack )
				{
					radEXCEL.Checked = true;
					chkHasHeader.Checked = true;
					nImportStep = 1;
					ShowStep();
					ViewState["ImportStep"] = nImportStep;

					radSALESFORCE.DataBind();
					radACT_2005.DataBind();
					radCUSTOM_CSV.DataBind();
					radCUSTOM_TAB.DataBind();
					ctlListHeader.Title = L10n.Term("Import.LBL_LAST_IMPORTED") + " " + L10n.Term(".moduleList.", sImportModule);
					tblNotesAccounts     .Visible = (sImportModule == "Accounts"     );
					tblNotesContacts     .Visible = (sImportModule == "Contacts"     );
					tblNotesOpportunities.Visible = (sImportModule == "Opportunities");
				}
				else
				{
					nImportStep = Sql.ToInteger(ViewState["ImportStep"]);
					if ( nImportStep < 1 )
						nImportStep = 1;
					ShowStep();
				}
			}
			catch ( Exception ex )
			{
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
			this.m_sMODULE = "Import";
		}
		#endregion
	}
}
