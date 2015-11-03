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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Diagnostics;
//using Microsoft.VisualBasic;
using SplendidCRM._controls;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for SplendidDynamic.
	/// </summary>
	public class SplendidDynamic
	{
		public static void AppendGridColumns(string sGRID_NAME, DataGrid grd)
		{
			if ( grd == null )
			{
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "DataGrid is not defined for " + sGRID_NAME);
				return;
			}
			DataTable dt = SplendidCache.GridViewColumns(sGRID_NAME);
			if ( dt != null )
			{
				foreach(DataRow row in dt.Rows)
				{
					int    nCOLUMN_INDEX               = Sql.ToInteger(row["COLUMN_INDEX"              ]);
					string sCOLUMN_TYPE                = Sql.ToString (row["COLUMN_TYPE"               ]);
					string sHEADER_TEXT                = Sql.ToString (row["HEADER_TEXT"               ]);
					string sSORT_EXPRESSION            = Sql.ToString (row["SORT_EXPRESSION"           ]);
					string sITEMSTYLE_WIDTH            = Sql.ToString (row["ITEMSTYLE_WIDTH"           ]);
					string sITEMSTYLE_CSSCLASS         = Sql.ToString (row["ITEMSTYLE_CSSCLASS"        ]);
					string sITEMSTYLE_HORIZONTAL_ALIGN = Sql.ToString (row["ITEMSTYLE_HORIZONTAL_ALIGN"]);
					string sITEMSTYLE_VERTICAL_ALIGN   = Sql.ToString (row["ITEMSTYLE_VERTICAL_ALIGN"  ]);
					bool   bITEMSTYLE_WRAP             = Sql.ToBoolean(row["ITEMSTYLE_WRAP"            ]);
					string sDATA_FIELD                 = Sql.ToString (row["DATA_FIELD"                ]);
					string sDATA_FORMAT                = Sql.ToString (row["DATA_FORMAT"               ]);
					string sURL_FIELD                  = Sql.ToString (row["URL_FIELD"                 ]);
					string sURL_FORMAT                 = Sql.ToString (row["URL_FORMAT"                ]);
					string sURL_TARGET                 = Sql.ToString (row["URL_TARGET"                ]);
					string sLIST_NAME                  = Sql.ToString (row["LIST_NAME"                 ]);
					// 04/28/2006 Paul.  The module is necessary in order to determine if a user has access. 
					string sURL_MODULE                 = Sql.ToString (row["URL_MODULE"                ]);
					// 05/02/2006 Paul.  The assigned user id is necessary if the user only has Owner access. 
					string sURL_ASSIGNED_FIELD         = Sql.ToString (row["URL_ASSIGNED_FIELD"        ]);
					HorizontalAlign eHorizontalAlign = HorizontalAlign.NotSet;
					switch ( sITEMSTYLE_HORIZONTAL_ALIGN.ToLower() )
					{
						case "left" :  eHorizontalAlign = HorizontalAlign.Left ;  break;
						case "right":  eHorizontalAlign = HorizontalAlign.Right;  break;
					}
					VerticalAlign eVerticalAlign = VerticalAlign.NotSet;
					switch ( sITEMSTYLE_VERTICAL_ALIGN.ToLower() )
					{
						case "top"   :  eVerticalAlign = VerticalAlign.Top   ;  break;
						case "middle":  eVerticalAlign = VerticalAlign.Middle;  break;
						case "bottom":  eVerticalAlign = VerticalAlign.Bottom;  break;
					}
					// 11/28/2005 Paul.  Wrap defaults to true. 
					if ( row["ITEMSTYLE_WRAP"] == DBNull.Value )
						bITEMSTYLE_WRAP = true;
					DataGridColumn col = null;
					// 02/03/2006 Paul.  Date and Currency must always be handled by CreateItemTemplateLiteral. 
					// Otherwise, the date or time will not get properly translated to the correct timezone. 
					// This bug was reported by David Williams. 
					if (     String.Compare(sCOLUMN_TYPE, "BoundColumn", true) == 0 
						&& (   String.Compare(sDATA_FORMAT, "Date"    , true) == 0 
						|| String.Compare(sDATA_FORMAT, "DateTime", true) == 0 
						|| String.Compare(sDATA_FORMAT, "Currency", true) == 0
						|| String.Compare(sDATA_FORMAT, "Image"   , true) == 0
						) )
					{
						sCOLUMN_TYPE = "TemplateColumn";
					}
					if ( String.Compare(sCOLUMN_TYPE, "BoundColumn", true) == 0 )
					{
						if ( Sql.IsEmptyString(sLIST_NAME) )
						{
							// GRID_NAME, COLUMN_ORDER, COLUMN_TYPE, HEADER_TEXT, DATA_FIELD, SORT_EXPRESSION, ITEMSTYLE_WIDTH
							BoundColumn bnd = new BoundColumn();
							bnd.HeaderText                  = sHEADER_TEXT       ;
							bnd.DataField                   = sDATA_FIELD        ;
							bnd.SortExpression              = sSORT_EXPRESSION   ;
							bnd.ItemStyle.Width             = new Unit(sITEMSTYLE_WIDTH);
							bnd.ItemStyle.CssClass          = sITEMSTYLE_CSSCLASS;
							bnd.ItemStyle.HorizontalAlign   = eHorizontalAlign   ;
							bnd.ItemStyle.VerticalAlign     = eVerticalAlign     ;
							bnd.ItemStyle.Wrap              = bITEMSTYLE_WRAP    ;
							col = bnd;
						}
						else
						{
							// GRID_NAME, COLUMN_ORDER, COLUMN_TYPE, HEADER_TEXT, DATA_FIELD, SORT_EXPRESSION, ITEMSTYLE_WIDTH
							TemplateColumn tpl = new TemplateColumn();
							tpl.HeaderText                  = sHEADER_TEXT       ;
							tpl.SortExpression              = sSORT_EXPRESSION   ;
							tpl.ItemStyle.Width             = new Unit(sITEMSTYLE_WIDTH);
							tpl.ItemStyle.CssClass          = sITEMSTYLE_CSSCLASS;
							tpl.ItemStyle.HorizontalAlign   = eHorizontalAlign   ;
							tpl.ItemStyle.VerticalAlign     = eVerticalAlign     ;
							tpl.ItemStyle.Wrap              = bITEMSTYLE_WRAP    ;
							tpl.ItemTemplate = new CreateItemTemplateLiteralList(sDATA_FIELD, sLIST_NAME);
							col = tpl;
						}
					}
					else if ( String.Compare(sCOLUMN_TYPE, "TemplateColumn", true) == 0 )
					{
						// GRID_NAME, COLUMN_ORDER, COLUMN_TYPE, HEADER_TEXT, DATA_FIELD, SORT_EXPRESSION, ITEMSTYLE_WIDTH
						TemplateColumn tpl = new TemplateColumn();
						tpl.HeaderText                  = sHEADER_TEXT       ;
						tpl.SortExpression              = sSORT_EXPRESSION   ;
						tpl.ItemStyle.Width             = new Unit(sITEMSTYLE_WIDTH);
						tpl.ItemStyle.CssClass          = sITEMSTYLE_CSSCLASS;
						tpl.ItemStyle.HorizontalAlign   = eHorizontalAlign   ;
						tpl.ItemStyle.VerticalAlign     = eVerticalAlign     ;
						tpl.ItemStyle.Wrap              = bITEMSTYLE_WRAP    ;
						if ( String.Compare(sDATA_FORMAT, "HyperLink", true) == 0 )
						{
							tpl.ItemTemplate = new CreateItemTemplateHyperLink(sDATA_FIELD, sURL_FIELD, sURL_FORMAT, sURL_TARGET, sITEMSTYLE_CSSCLASS, sURL_MODULE, sURL_ASSIGNED_FIELD);
						}
						else if ( String.Compare(sDATA_FORMAT, "Image", true) == 0 )
						{
							tpl.ItemTemplate = new CreateItemTemplateImage(sDATA_FIELD, sITEMSTYLE_CSSCLASS);
						}
						else
						{
							tpl.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
							tpl.ItemTemplate = new CreateItemTemplateLiteral(sDATA_FIELD, sDATA_FORMAT);
						}
						col = tpl;
					}
					else if ( String.Compare(sCOLUMN_TYPE, "HyperLinkColumn", true) == 0 )
					{
						// GRID_NAME, COLUMN_ORDER, COLUMN_TYPE, HEADER_TEXT, DATA_FIELD, SORT_EXPRESSION, ITEMSTYLE_WIDTH, ITEMSTYLE-CSSCLASS, URL_FIELD, URL_FORMAT
						HyperLinkColumn lnk = new HyperLinkColumn();
						lnk.HeaderText                  = sHEADER_TEXT       ;
						lnk.DataTextField               = sDATA_FIELD        ;
						lnk.SortExpression              = sSORT_EXPRESSION   ;
						lnk.DataNavigateUrlField        = sURL_FIELD         ;
						lnk.DataNavigateUrlFormatString = sURL_FORMAT        ;
						lnk.Target                      = sURL_TARGET        ;
						lnk.ItemStyle.Width             = new Unit(sITEMSTYLE_WIDTH);
						lnk.ItemStyle.CssClass          = sITEMSTYLE_CSSCLASS;
						lnk.ItemStyle.HorizontalAlign   = eHorizontalAlign   ;
						lnk.ItemStyle.VerticalAlign     = eVerticalAlign     ;
						lnk.ItemStyle.Wrap              = bITEMSTYLE_WRAP    ;
						col = lnk;
					}
					if ( col != null )
					{
						// 11/28/2005 Paul.  In case the column specified is too high, just append column. 
						if ( nCOLUMN_INDEX >= grd.Columns.Count )
							grd.Columns.Add(col);
						else
							grd.Columns.AddAt(nCOLUMN_INDEX, col);
					}
				}
			}
		}

		public static void AppendGridColumns(DataView dvFields, HtmlTable tbl, IDataReader rdr, L10N L10n, TimeZone T10n, CommandEventHandler Page_Command)
		{
			if ( tbl == null )
			{
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "HtmlTable is not defined");
				return;
			}
			// 01/07/2006 Paul.  Show table borders in layout mode. This will help distinguish blank lines from wrapped lines. 
			tbl.Border = 1;

			HtmlTableRow trAction = new HtmlTableRow();
			HtmlTableRow trHeader = new HtmlTableRow();
			HtmlTableRow trField  = new HtmlTableRow();
			tbl.Rows.Insert(0, trAction);
			tbl.Rows.Insert(1, trHeader);
			tbl.Rows.Insert(2, trField );
			trAction.Attributes.Add("class", "listViewThS1");
			trHeader.Attributes.Add("class", "listViewThS1");
			trField .Attributes.Add("class", "oddListRowS1");

			HttpSessionState Session = HttpContext.Current.Session;
			foreach(DataRowView row in dvFields)
			{
				Guid   gID                         = Sql.ToGuid   (row["ID"                        ]);
				int    nCOLUMN_INDEX               = Sql.ToInteger(row["COLUMN_INDEX"              ]);
				string sCOLUMN_TYPE                = Sql.ToString (row["COLUMN_TYPE"               ]);
				string sHEADER_TEXT                = Sql.ToString (row["HEADER_TEXT"               ]);
				string sSORT_EXPRESSION            = Sql.ToString (row["SORT_EXPRESSION"           ]);
				string sITEMSTYLE_WIDTH            = Sql.ToString (row["ITEMSTYLE_WIDTH"           ]);
				string sITEMSTYLE_CSSCLASS         = Sql.ToString (row["ITEMSTYLE_CSSCLASS"        ]);
				string sITEMSTYLE_HORIZONTAL_ALIGN = Sql.ToString (row["ITEMSTYLE_HORIZONTAL_ALIGN"]);
				string sITEMSTYLE_VERTICAL_ALIGN   = Sql.ToString (row["ITEMSTYLE_VERTICAL_ALIGN"  ]);
				bool   bITEMSTYLE_WRAP             = Sql.ToBoolean(row["ITEMSTYLE_WRAP"            ]);
				string sDATA_FIELD                 = Sql.ToString (row["DATA_FIELD"                ]);
				string sDATA_FORMAT                = Sql.ToString (row["DATA_FORMAT"               ]);
				string sURL_FIELD                  = Sql.ToString (row["URL_FIELD"                 ]);
				string sURL_FORMAT                 = Sql.ToString (row["URL_FORMAT"                ]);
				string sURL_TARGET                 = Sql.ToString (row["URL_TARGET"                ]);
				string sLIST_NAME                  = Sql.ToString (row["LIST_NAME"                 ]);
				
				HtmlTableCell tdAction = new HtmlTableCell();
				trAction.Cells.Add(tdAction);
				tdAction.NoWrap = true;

				Literal litIndex = new Literal();
				tdAction.Controls.Add(litIndex);
				litIndex.Text = " " + nCOLUMN_INDEX.ToString() + " ";

				ImageButton btnMoveUp   = CreateLayoutImageButton(gID, "Layout.MoveUp"  , nCOLUMN_INDEX, L10n.Term(".LNK_LEFT"  ), Sql.ToString(Session["themeURL"]) + "images/leftarrow.gif"    , Page_Command);
				ImageButton btnMoveDown = CreateLayoutImageButton(gID, "Layout.MoveDown", nCOLUMN_INDEX, L10n.Term(".LNK_RIGHT" ), Sql.ToString(Session["themeURL"]) + "images/rightarrow.gif"   , Page_Command);
				ImageButton btnInsert   = CreateLayoutImageButton(gID, "Layout.Insert"  , nCOLUMN_INDEX, L10n.Term(".LNK_INS"   ), Sql.ToString(Session["themeURL"]) + "images/plus_inline.gif"  , Page_Command);
				ImageButton btnEdit     = CreateLayoutImageButton(gID, "Layout.Edit"    , nCOLUMN_INDEX, L10n.Term(".LNK_EDIT"  ), Sql.ToString(Session["themeURL"]) + "images/edit_inline.gif"  , Page_Command);
				ImageButton btnDelete   = CreateLayoutImageButton(gID, "Layout.Delete"  , nCOLUMN_INDEX, L10n.Term(".LNK_DELETE"), Sql.ToString(Session["themeURL"]) + "images/delete_inline.gif", Page_Command);
				tdAction.Controls.Add(btnMoveUp  );
				tdAction.Controls.Add(btnMoveDown);
				tdAction.Controls.Add(btnInsert  );
				tdAction.Controls.Add(btnEdit    );
				tdAction.Controls.Add(btnDelete  );
				
				HtmlTableCell tdHeader = new HtmlTableCell();
				trHeader.Cells.Add(tdHeader);
				tdHeader.NoWrap = true;
				
				HtmlTableCell tdField = new HtmlTableCell();
				trField.Cells.Add(tdField);
				tdField.NoWrap = true;

				Literal litHeader = new Literal();
				tdHeader.Controls.Add(litHeader);
				litHeader.Text = sHEADER_TEXT;

				Literal litField = new Literal();
				tdField.Controls.Add(litField);
				litField.Text = sDATA_FIELD;
			}
		}

		private static ImageButton CreateLayoutImageButton(Guid gID, string sCommandName, int nFIELD_INDEX, string sAlternateText, string sImageUrl, CommandEventHandler Page_Command)
		{
			ImageButton btnDelete = new ImageButton();
			// 01/07/2006 Paul.  The problem with the ImageButton Delete event was that the dynamically rendered ID 
			// was not being found on every other page request.  The solution was to manually name and number the ImageButton IDs.
			// Make sure not to use ":" in the name, otherwise it will confuse the FindControl function. 
			btnDelete.ID              = sCommandName + "." + gID.ToString();
			btnDelete.CommandName     = sCommandName        ;
			btnDelete.CommandArgument = nFIELD_INDEX.ToString();
			btnDelete.CssClass        = "listViewTdToolsS1" ;
			btnDelete.AlternateText   = sAlternateText      ;
			btnDelete.ImageUrl        = sImageUrl           ;
			btnDelete.BorderWidth     = 0                   ;
			btnDelete.Width           = 12                  ;
			btnDelete.Height          = 12                  ;
			btnDelete.ImageAlign      = ImageAlign.AbsMiddle;
			if ( Page_Command != null )
				btnDelete.Command += Page_Command;
			return btnDelete;
		}

		public static void AppendDetailViewFields(string sDETAIL_NAME, HtmlTable tbl, IDataReader rdr, L10N L10n, TimeZone T10n, CommandEventHandler Page_Command)
		{
			if ( tbl == null )
			{
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "HtmlTable is not defined for " + sDETAIL_NAME);
				return;
			}
			DataTable dtFields = SplendidCache.DetailViewFields(sDETAIL_NAME);
			AppendDetailViewFields(dtFields.DefaultView, tbl, rdr, L10n, T10n, Page_Command, false);
		}

		public static void AppendDetailViewFields(DataView dvFields, HtmlTable tbl, IDataReader rdr, L10N L10n, TimeZone T10n, CommandEventHandler Page_Command, bool bLayoutMode)
		{
			HtmlTableRow tr = null;
			// 11/28/2005 Paul.  Start row index using the existing count so that headers can be specified. 
			int nRowIndex = tbl.Rows.Count - 1;
			int nColIndex = 0;
			// 01/07/2006 Paul.  Show table borders in layout mode. This will help distinguish blank lines from wrapped lines. 
			if ( bLayoutMode )
				tbl.Border = 1;
			HttpSessionState Session = HttpContext.Current.Session;
			foreach(DataRowView row in dvFields)
			{
				Guid   gID          = Sql.ToGuid   (row["ID"         ]);
				int    nFIELD_INDEX = Sql.ToInteger(row["FIELD_INDEX"]);
				string sFIELD_TYPE  = Sql.ToString (row["FIELD_TYPE" ]);
				string sDATA_LABEL  = Sql.ToString (row["DATA_LABEL" ]);
				string sDATA_FIELD  = Sql.ToString (row["DATA_FIELD" ]);
				string sDATA_FORMAT = Sql.ToString (row["DATA_FORMAT"]);
				string sLIST_NAME   = Sql.ToString (row["LIST_NAME"  ]);
				int    nCOLSPAN     = Sql.ToInteger(row["COLSPAN"    ]);
				string sLABEL_WIDTH = Sql.ToString (row["LABEL_WIDTH"]);
				string sFIELD_WIDTH = Sql.ToString (row["FIELD_WIDTH"]);
				if ( nColIndex == 0 )
				{
					// 11/25/2005 Paul.  Don't pre-create a row as we don't want a blank
					// row at the bottom.  Add rows just before they are needed. 
					nRowIndex++;
					tr = new HtmlTableRow();
					tbl.Rows.Insert(nRowIndex, tr);
				}
				if ( bLayoutMode )
				{
					HtmlTableCell tdAction = new HtmlTableCell();
					tr.Cells.Add(tdAction);
					tdAction.Attributes.Add("class", "tabDetailViewDL");
					tdAction.NoWrap = true;

					Literal litIndex = new Literal();
					tdAction.Controls.Add(litIndex);
					litIndex.Text = " " + nFIELD_INDEX.ToString() + " ";

					ImageButton btnMoveUp   = CreateLayoutImageButton(gID, "Layout.MoveUp"  , nFIELD_INDEX, L10n.Term(".LNK_UP"    ), Sql.ToString(Session["themeURL"]) + "images/uparrow_inline.gif"  , Page_Command);
					ImageButton btnMoveDown = CreateLayoutImageButton(gID, "Layout.MoveDown", nFIELD_INDEX, L10n.Term(".LNK_DOWN"  ), Sql.ToString(Session["themeURL"]) + "images/downarrow_inline.gif", Page_Command);
					ImageButton btnInsert   = CreateLayoutImageButton(gID, "Layout.Insert"  , nFIELD_INDEX, L10n.Term(".LNK_INS"   ), Sql.ToString(Session["themeURL"]) + "images/plus_inline.gif"     , Page_Command);
					ImageButton btnEdit     = CreateLayoutImageButton(gID, "Layout.Edit"    , nFIELD_INDEX, L10n.Term(".LNK_EDIT"  ), Sql.ToString(Session["themeURL"]) + "images/edit_inline.gif"     , Page_Command);
					ImageButton btnDelete   = CreateLayoutImageButton(gID, "Layout.Delete"  , nFIELD_INDEX, L10n.Term(".LNK_DELETE"), Sql.ToString(Session["themeURL"]) + "images/delete_inline.gif"   , Page_Command);
					tdAction.Controls.Add(btnMoveUp  );
					tdAction.Controls.Add(btnMoveDown);
					tdAction.Controls.Add(btnInsert  );
					tdAction.Controls.Add(btnEdit    );
					tdAction.Controls.Add(btnDelete  );
				}
				HtmlTableCell tdLabel = new HtmlTableCell();
				HtmlTableCell tdField = new HtmlTableCell();
				tr.Cells.Add(tdLabel);
				tr.Cells.Add(tdField);
				if ( nCOLSPAN > 0 )
				{
					tdField.ColSpan = nCOLSPAN;
					if ( bLayoutMode )
						tdField.ColSpan++;
				}
				tdLabel.Attributes.Add("class", "tabDetailViewDL");
				tdLabel.VAlign = "top";
				tdLabel.Width  = sLABEL_WIDTH;
				tdField.Attributes.Add("class", "tabDetailViewDF");
				tdField.VAlign = "top";
				// 11/28/2005 Paul.  Don't use the field width if COLSPAN is specified as we want it to take the rest of the table.  The label width will be sufficient. 
				if ( nCOLSPAN == 0 )
					tdField.Width  = sFIELD_WIDTH;
				
				Literal   litLabel = new Literal();
				HyperLink lnkField = null;
				tdLabel.Controls.Add(litLabel);
				//litLabel.Text = nFIELD_INDEX.ToString() + " (" + nRowIndex.ToString() + "," + nColIndex.ToString() + ")";
				try
				{
					if ( bLayoutMode )
						litLabel.Text = sDATA_LABEL;
					else if ( sDATA_LABEL.IndexOf(".") >= 0 )
						litLabel.Text = L10n.Term(sDATA_LABEL);
					else if ( !Sql.IsEmptyString(sDATA_LABEL) && rdr != null )
						litLabel.Text = Sql.ToString(rdr[sDATA_LABEL]) + L10n.Term("Calls.LBL_COLON");
					// 07/15/2006 Paul.  Always put something for the label so that table borders will look right. 
					else
						litLabel.Text = "&nbsp;";
				}
				catch(Exception ex)
				{
					SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
					litLabel.Text = ex.Message;
				}
				if ( String.Compare(sFIELD_TYPE, "Blank", true) == 0 )
				{
					Literal litField = new Literal();
					tdField.Controls.Add(litField);
					if ( bLayoutMode )
					{
						litLabel.Text = "*** BLANK ***";
						litField.Text = "*** BLANK ***";
					}
					else
					{
						litField.Text = "&nbsp;";
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "Line", true) == 0 )
				{
					if ( bLayoutMode )
					{
						Literal litField = new Literal();
						tdField.Controls.Add(litField);
						litLabel.Text = "*** LINE ***";
						litField.Text = "*** LINE ***";
					}
					else
					{
						tr.Cells.Clear();
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "String", true) == 0 )
				{
					if ( bLayoutMode )
					{
						Literal litField = new Literal();
						litField.Text = sDATA_FIELD;
						tdField.Controls.Add(litField);
					}
					else if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						// 12/06/2005 Paul.  Wrap all string fields in a SPAN tag to simplify regression testing. 
						HtmlGenericControl spnField = new HtmlGenericControl("span");
						tdField.Controls.Add(spnField);
						spnField.ID = sDATA_FIELD;

						Literal litField = new Literal();
						spnField.Controls.Add(litField);
						try
						{
							string[] arrLIST_NAME  = sLIST_NAME .Split(' ');
							string[] arrDATA_FIELD = sDATA_FIELD.Split(' ');
							object[] objDATA_FIELD = new object[arrDATA_FIELD.Length];
							for ( int i=0 ; i < arrDATA_FIELD.Length; i++ )
							{
								if ( arrDATA_FIELD[i].IndexOf(".") >= 0 )
								{
									objDATA_FIELD[i] = L10n.Term(arrDATA_FIELD[i]);
								}
								else if ( !Sql.IsEmptyString(sLIST_NAME) )
								{
									if ( arrLIST_NAME.Length == arrDATA_FIELD.Length )
									{
										if ( rdr != null )
											objDATA_FIELD[i] = L10n.Term("." + arrLIST_NAME[i] + ".", rdr[arrDATA_FIELD[i]]);
										else
											objDATA_FIELD[i] = String.Empty;
									}
								}
								else if ( !Sql.IsEmptyString(arrDATA_FIELD[i]) )
								{
									if ( rdr != null && rdr[arrDATA_FIELD[i]] != DBNull.Value)
									{
										// 12/05/2005 Paul.  If the data is a DateTime field, then make sure to perform the timezone conversion. 
										if ( rdr[arrDATA_FIELD[i]].GetType() == Type.GetType("System.DateTime") )
											objDATA_FIELD[i] = T10n.FromServerTime(rdr[arrDATA_FIELD[i]]);
										else
											objDATA_FIELD[i] = rdr[arrDATA_FIELD[i]];
									}
									else
										objDATA_FIELD[i] = String.Empty;
								}
							}
							if ( rdr != null )
							{
								// 01/09/2006 Paul.  Allow DATA_FORMAT to be optional.   If missing, write data directly. 
								if ( sDATA_FORMAT == String.Empty )
								{
									for ( int i=0; i < arrDATA_FIELD.Length; i++ )
										arrDATA_FIELD[i] = Sql.ToString(objDATA_FIELD[i]);
									litField.Text = String.Join(" ", arrDATA_FIELD);
								}
								else
									litField.Text = String.Format(sDATA_FORMAT, objDATA_FIELD);
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							litField.Text = ex.Message;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "CheckBox", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						CheckBox chkField = new CheckBox();
						tdField.Controls.Add(chkField);
						chkField.Enabled  = false     ;
						chkField.CssClass = "checkbox";
						// 03/16/2006 Paul.  Give the checkbox a name so that it can be validated with SplendidTest. 
						chkField.ID       = sDATA_FIELD;
						try
						{
							if ( rdr != null )
								chkField.Checked = Sql.ToBoolean(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "Button", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						Button btnField = new Button();
						tdField.Controls.Add(btnField);
						btnField.CssClass = "button";
						// 03/16/2006 Paul.  Give the button a name so that it can be validated with SplendidTest. 
						btnField.ID       = sDATA_FIELD;
						if ( Page_Command != null )
						{
							btnField.Command    += Page_Command;
							btnField.CommandName = sDATA_FORMAT  ;
						}
						try
						{
							if ( bLayoutMode )
							{
								btnField.Text    = sDATA_FIELD;
								btnField.Enabled = false      ;
							}
							else if ( sDATA_FIELD.IndexOf(".") >= 0 )
							{
								btnField.Text = L10n.Term(sDATA_FIELD);
							}
							else if ( !Sql.IsEmptyString(sDATA_FIELD) && rdr != null )
							{
								btnField.Text = Sql.ToString(rdr[sDATA_FIELD]);
							}
							btnField.Attributes.Add("title", btnField.Text);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							btnField.Text = ex.Message;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "Textbox", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						TextBox txtField = new TextBox();
						tdField.Controls.Add(txtField);
						txtField.ReadOnly = true;
						txtField.TextMode = TextBoxMode.MultiLine;
						// 03/16/2006 Paul.  Give the textbox a name so that it can be validated with SplendidTest. 
						txtField.ID       = sDATA_FIELD;
						try
						{
							string[] arrDATA_FORMAT = sDATA_FORMAT.Split(',');
							if ( arrDATA_FORMAT.Length == 2 )
							{
								txtField.Rows    = Sql.ToInteger(arrDATA_FORMAT[0]);
								txtField.Columns = Sql.ToInteger(arrDATA_FORMAT[1]);
							}
							if ( bLayoutMode )
							{
								txtField.Text = sDATA_FIELD;
							}
							else if ( !Sql.IsEmptyString(sDATA_FIELD) && rdr != null )
							{
								txtField.Text = Sql.ToString(rdr[sDATA_FIELD]);
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							txtField.Text = ex.Message;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "HyperLink", true) == 0 )
				{
					string sURL_FIELD = Sql.ToString (row["URL_FIELD"]);
					if ( !Sql.IsEmptyString(sDATA_FIELD) && !Sql.IsEmptyString(sURL_FIELD) )
					{
						string sURL_FORMAT = Sql.ToString (row["URL_FORMAT"]);
						string sURL_TARGET = Sql.ToString (row["URL_TARGET"]);
						lnkField = new HyperLink();
						tdField.Controls.Add(lnkField);
						lnkField.Target   = sURL_TARGET;
						lnkField.CssClass = "tabDetailViewDFLink";
						// 03/16/2006 Paul.  Give the hyperlink a name so that it can be validated with SplendidTest. 
						lnkField.ID       = sDATA_FIELD;
						try
						{
							if ( bLayoutMode )
							{
								lnkField.Text    = sDATA_FIELD;
								lnkField.Enabled = false      ;
							}
							else if ( rdr != null )
							{
								if ( !Sql.IsEmptyString(rdr[sDATA_FIELD]) )
								{
									// 01/09/2006 Paul.  Allow DATA_FORMAT to be optional.   If missing, write data directly. 
									if ( sDATA_FORMAT == String.Empty )
										lnkField.Text = Sql.ToString(rdr[sDATA_FIELD]);
									else
										lnkField.Text = String.Format(sDATA_FORMAT, Sql.ToString(rdr[sDATA_FIELD]));
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							lnkField.Text = ex.Message;
						}
						try
						{
							if ( bLayoutMode )
							{
								lnkField.NavigateUrl = sURL_FIELD;
							}
							else if ( rdr != null )
							{
								if ( !Sql.IsEmptyString(rdr[sURL_FIELD]) )
								{
									// 01/09/2006 Paul.  Allow DATA_FORMAT to be optional.   If missing, write data directly. 
									if ( sDATA_FORMAT == String.Empty )
										lnkField.NavigateUrl = Sql.ToString(rdr[sURL_FIELD]);
									else
										lnkField.NavigateUrl = String.Format(sURL_FORMAT, Sql.ToString(rdr[sURL_FIELD]));
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							lnkField.NavigateUrl = ex.Message;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "Image", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						Image imgField = new Image();
						// 04/13/2006 Paul.  Give the image a name so that it can be validated with SplendidTest. 
						imgField.ID = sDATA_FIELD;
						try
						{
							if ( bLayoutMode )
							{
								Literal litField = new Literal();
								litField.Text = sDATA_FIELD;
								tdField.Controls.Add(litField);
							}
							else if ( rdr != null )
							{
								if ( !Sql.IsEmptyString(rdr[sDATA_FIELD]) )
								{
									imgField.ImageUrl = "~/Images/Image.aspx?ID=" + Sql.ToString(rdr[sDATA_FIELD]);
									// 04/13/2006 Paul.  Only add the image if it exists. 
									tdField.Controls.Add(imgField);
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							lnkField.Text = ex.Message;
						}
					}
				}
				else
				{
					Literal litField = new Literal();
					tdField.Controls.Add(litField);
					litField.Text = "Unknown field type " + sFIELD_TYPE;
					// 01/07/2006 Paul.  Don't report the error in layout mode. 
					if ( !bLayoutMode )
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Unknown field type " + sFIELD_TYPE);
				}
				if ( nCOLSPAN > 0 )
					nColIndex = 0;
				else
				{
					nColIndex++;
					if ( nColIndex > 1 )
						nColIndex = 0;
				}
			}
		}

		public static void AppendEditViewFields(string sEDIT_NAME, HtmlTable tbl, IDataReader rdr, L10N L10n, TimeZone T10n)
		{
			if ( tbl == null )
			{
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "HtmlTable is not defined for " + sEDIT_NAME);
				return;
			}
			DataTable dtFields = SplendidCache.EditViewFields(sEDIT_NAME);
			AppendEditViewFields(dtFields.DefaultView, tbl, rdr, L10n, T10n, null, false);
		}

		public static void ValidateEditViewFields(string sEDIT_NAME, Control parent)
		{
			DataTable dtFields = SplendidCache.EditViewFields(sEDIT_NAME);
			DataView dvFields = new DataView(dtFields);
			dvFields.RowFilter = "UI_REQUIRED = 1";
			foreach(DataRowView row in dvFields)
			{
				string sFIELD_TYPE        = Sql.ToString (row["FIELD_TYPE"       ]);
				string sDATA_FIELD        = Sql.ToString (row["DATA_FIELD"       ]);
				bool   bUI_REQUIRED       = Sql.ToBoolean(row["UI_REQUIRED"      ]);
				if ( bUI_REQUIRED && !Sql.IsEmptyString(sDATA_FIELD) )
				{
					if ( String.Compare(sFIELD_TYPE, "DatePicker", true) == 0 )
					{
						DatePicker ctlDate = parent.FindControl(sDATA_FIELD) as DatePicker;
						if ( ctlDate != null )
						{
							// 03/04/2006 Paul.  Only visible controls are validated. 
							if ( ctlDate.Visible )
								ctlDate.Validate();
						}
					}
					else if ( String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0 )
					{
						DateTimePicker ctlDate = parent.FindControl(sDATA_FIELD) as DateTimePicker;
						if ( ctlDate != null )
						{
							// 03/04/2006 Paul.  Only visible controls are validated. 
							if ( ctlDate.Visible )
								ctlDate.Validate();
						}
					}
					else if ( String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0 )
					{
						DateTimeEdit ctlDate = parent.FindControl(sDATA_FIELD) as DateTimeEdit;
						if ( ctlDate != null )
						{
							// 03/04/2006 Paul.  Only visible controls are validated. 
							if ( ctlDate.Visible )
								ctlDate.Validate();
						}
					}
					else
					{
						Control ctl = parent.FindControl(sDATA_FIELD);
						if ( ctl != null )
						{
							// 03/04/2006 Paul.  Only visible controls are validated. 
							if ( ctl.Visible )
							{
								BaseValidator req = parent.FindControl(sDATA_FIELD + "_REQUIRED") as BaseValidator;
								if ( req != null )
								{
									// 01/16/2006 Paul.  Enable validator before validating page. 
									// If we leave the validator control enabled, then it may block an alternate action, like Cancel. 
									req.Enabled = true;
									req.Validate();
								}
							}
						}
					}
				}
			}
		}
		/*
		// 01/16/2006 Paul.  If we disable the validator, it will hide it's error message. 
		// The solution may be to always require server-side validation (disable EnableClientScript).
		public static void DisableValidationEditViewFields(string sEDIT_NAME, Control parent)
		{
			DataTable dtFields = SplendidCache.EditViewFields(sEDIT_NAME);
			DataView dvFields = new DataView(dtFields);
			dvFields.RowFilter = "UI_REQUIRED = 1";
			foreach(DataRowView row in dvFields)
			{
				string sFIELD_TYPE        = Sql.ToString (row["FIELD_TYPE"       ]);
				string sDATA_FIELD        = Sql.ToString (row["DATA_FIELD"       ]);
				bool   bUI_REQUIRED       = Sql.ToBoolean(row["UI_REQUIRED"      ]);
				if ( bUI_REQUIRED && !Sql.IsEmptyString(sDATA_FIELD) )
				{
					if ( String.Compare(sFIELD_TYPE, "DatePicker", true) == 0 )
					{
						DatePicker ctlDate = parent.FindControl(sDATA_FIELD) as DatePicker;
						if ( ctlDate != null )
							ctlDate.DisableValidation();
					}
					else if ( String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0 )
					{
						DateTimePicker ctlDate = parent.FindControl(sDATA_FIELD) as DateTimePicker;
						if ( ctlDate != null )
							ctlDate.DisableValidation();
					}
					else if ( String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0 )
					{
						DateTimeEdit ctlDate = parent.FindControl(sDATA_FIELD) as DateTimeEdit;
						if ( ctlDate != null )
							ctlDate.DisableValidation();
					}
					else
					{
						BaseValidator req = parent.FindControl(sDATA_FIELD + "_REQUIRED") as BaseValidator;
						if ( req != null )
						{
							// 01/16/2006 Paul.  Enable validator before validating page. 
							// If we leave the validator control enabled, then it may block an alternate action, like Cancel. 
							req.Enabled = false;
						}
					}
				}
			}
		}
		*/

		public static void AppendEditViewFields(DataView dvFields, HtmlTable tbl, IDataReader rdr, L10N L10n, TimeZone T10n, CommandEventHandler Page_Command, bool bLayoutMode)
		{
			HtmlTableRow tr = null;
			// 11/28/2005 Paul.  Start row index using the existing count so that headers can be specified. 
			int nRowIndex = tbl.Rows.Count - 1;
			int nColIndex = 0;
			HtmlTableCell tdLabel = null;
			HtmlTableCell tdField = null;
			// 01/07/2006 Paul.  Show table borders in layout mode. This will help distinguish blank lines from wrapped lines. 
			if ( bLayoutMode )
				tbl.Border = 1;
			HttpSessionState Session = HttpContext.Current.Session;
			foreach(DataRowView row in dvFields)
			{
				Guid   gID                = Sql.ToGuid   (row["ID"               ]);
				int    nFIELD_INDEX       = Sql.ToInteger(row["FIELD_INDEX"      ]);
				string sFIELD_TYPE        = Sql.ToString (row["FIELD_TYPE"       ]);
				string sDATA_LABEL        = Sql.ToString (row["DATA_LABEL"       ]);
				string sDATA_FIELD        = Sql.ToString (row["DATA_FIELD"       ]);
				string sDISPLAY_FIELD     = Sql.ToString (row["DISPLAY_FIELD"    ]);
				string sCACHE_NAME        = Sql.ToString (row["CACHE_NAME"       ]);
				bool   bDATA_REQUIRED     = Sql.ToBoolean(row["DATA_REQUIRED"    ]);
				bool   bUI_REQUIRED       = Sql.ToBoolean(row["UI_REQUIRED"      ]);
				string sONCLICK_SCRIPT    = Sql.ToString (row["ONCLICK_SCRIPT"   ]);
				string sFORMAT_SCRIPT     = Sql.ToString (row["FORMAT_SCRIPT"    ]);
				short  nFORMAT_TAB_INDEX  = Sql.ToShort  (row["FORMAT_TAB_INDEX" ]);
				int    nFORMAT_MAX_LENGTH = Sql.ToInteger(row["FORMAT_MAX_LENGTH"]);
				int    nFORMAT_SIZE       = Sql.ToInteger(row["FORMAT_SIZE"      ]);
				int    nFORMAT_ROWS       = Sql.ToInteger(row["FORMAT_ROWS"      ]);
				int    nFORMAT_COLUMNS    = Sql.ToInteger(row["FORMAT_COLUMNS"   ]);
				int    nCOLSPAN           = Sql.ToInteger(row["COLSPAN"          ]);
				int    nROWSPAN           = Sql.ToInteger(row["ROWSPAN"          ]);
				string sLABEL_WIDTH       = Sql.ToString (row["LABEL_WIDTH"      ]);
				string sFIELD_WIDTH       = Sql.ToString (row["FIELD_WIDTH"      ]);
				// 12/02/2005 Paul. COLSPAN == -1 means that a new column should not be created. 
				if ( (nCOLSPAN >= 0 && nColIndex == 0) || tr == null )
				{
					// 11/25/2005 Paul.  Don't pre-create a row as we don't want a blank
					// row at the bottom.  Add rows just before they are needed. 
					nRowIndex++;
					tr = new HtmlTableRow();
					tbl.Rows.Insert(nRowIndex, tr);
				}
				if ( bLayoutMode )
				{
					HtmlTableCell tdAction = new HtmlTableCell();
					tr.Cells.Add(tdAction);
					tdAction.Attributes.Add("class", "tabDetailViewDL");
					tdAction.NoWrap = true;

					Literal litIndex = new Literal();
					tdAction.Controls.Add(litIndex);
					litIndex.Text = " " + nFIELD_INDEX.ToString() + " ";

					ImageButton btnMoveUp   = CreateLayoutImageButton(gID, "Layout.MoveUp"  , nFIELD_INDEX, L10n.Term(".LNK_UP"    ), Sql.ToString(Session["themeURL"]) + "images/uparrow_inline.gif"  , Page_Command);
					ImageButton btnMoveDown = CreateLayoutImageButton(gID, "Layout.MoveDown", nFIELD_INDEX, L10n.Term(".LNK_DOWN"  ), Sql.ToString(Session["themeURL"]) + "images/downarrow_inline.gif", Page_Command);
					ImageButton btnInsert   = CreateLayoutImageButton(gID, "Layout.Insert"  , nFIELD_INDEX, L10n.Term(".LNK_INS"   ), Sql.ToString(Session["themeURL"]) + "images/plus_inline.gif"     , Page_Command);
					ImageButton btnEdit     = CreateLayoutImageButton(gID, "Layout.Edit"    , nFIELD_INDEX, L10n.Term(".LNK_EDIT"  ), Sql.ToString(Session["themeURL"]) + "images/edit_inline.gif"     , Page_Command);
					ImageButton btnDelete   = CreateLayoutImageButton(gID, "Layout.Delete"  , nFIELD_INDEX, L10n.Term(".LNK_DELETE"), Sql.ToString(Session["themeURL"]) + "images/delete_inline.gif"   , Page_Command);
					tdAction.Controls.Add(btnMoveUp  );
					tdAction.Controls.Add(btnMoveDown);
					tdAction.Controls.Add(btnInsert  );
					tdAction.Controls.Add(btnEdit    );
					tdAction.Controls.Add(btnDelete  );
				}
				if ( nCOLSPAN >= 0 || tdLabel == null || tdField == null )
				{
					tdLabel = new HtmlTableCell();
					tdField = new HtmlTableCell();
					tr.Cells.Add(tdLabel);
					tr.Cells.Add(tdField);
					if ( nCOLSPAN > 0 )
					{
						tdField.ColSpan = nCOLSPAN;
						if ( bLayoutMode )
							tdField.ColSpan++;
					}
					tdLabel.Attributes.Add("class", "dataLabel");
					tdLabel.VAlign = "top";
					tdLabel.Width  = sLABEL_WIDTH;
					tdField.Attributes.Add("class", "dataField");
					tdField.VAlign = "top";
					// 11/28/2005 Paul.  Don't use the field width if COLSPAN is specified as we want it to take the rest of the table.  The label width will be sufficient. 
					if ( nCOLSPAN == 0 )
						tdField.Width  = sFIELD_WIDTH;

					Literal   litLabel = new Literal();
					tdLabel.Controls.Add(litLabel);
					//litLabel.Text = nFIELD_INDEX.ToString() + " (" + nRowIndex.ToString() + "," + nColIndex.ToString() + ")";
					try
					{
						if ( bLayoutMode )
						{
							if ( String.Compare(sFIELD_TYPE, "Blank", true) == 0 )
								litLabel.Text = "*** BLANK ***";
							else
								litLabel.Text = sDATA_LABEL;
						}
						else if ( sDATA_LABEL.IndexOf(".") >= 0 )
							litLabel.Text = L10n.Term(sDATA_LABEL);
						else if ( !Sql.IsEmptyString(sDATA_LABEL) && rdr != null )
							litLabel.Text = Sql.ToString(rdr[sDATA_LABEL]) + L10n.Term("Calls.LBL_COLON");
						// 07/15/2006 Paul.  Always put something for the label so that table borders will look right. 
						else
							litLabel.Text = "&nbsp;";
					}
					catch(Exception ex)
					{
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						litLabel.Text = ex.Message;
					}
					if ( !bLayoutMode && bUI_REQUIRED )
					{
						Label lblRequired = new Label();
						tdLabel.Controls.Add(lblRequired);
						lblRequired.CssClass = "required";
						lblRequired.Text = L10n.Term(".LBL_REQUIRED_SYMBOL");
					}
				}
				
				if ( String.Compare(sFIELD_TYPE, "Blank", true) == 0 )
				{
					Literal litField = new Literal();
					tdField.Controls.Add(litField);
					if ( bLayoutMode )
						litField.Text = "*** BLANK ***";
					else
						litField.Text = "&nbsp;";
				}
				else if ( String.Compare(sFIELD_TYPE, "Label", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						Literal litField = new Literal();
						tdField.Controls.Add(litField);
						// 07/25/2006 Paul.  Align label values to the middle so the line-up with the label. 
						tdField.VAlign = "middle";
						// 07/24/2006 Paul.  Set the ID so that the literal control can be accessed. 
						litField.ID = sDATA_FIELD;
						try
						{
							if ( bLayoutMode )
								litField.Text = sDATA_FIELD;
							else if ( sDATA_FIELD.IndexOf(".") >= 0 )
								litField.Text = L10n.Term(sDATA_FIELD);
							else if ( rdr != null )
								litField.Text = Sql.ToString(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							litField.Text = ex.Message;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "ListBox", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						DropDownList lstField = new DropDownList();
						tdField.Controls.Add(lstField);
						lstField.ID       = sDATA_FIELD;
						lstField.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( !Sql.IsEmptyString(sDATA_FIELD) )
							{
								// 12/04/2005 Paul.  Don't populate list if this is a post back. 
								if ( !Sql.IsEmptyString(sCACHE_NAME) && (bLayoutMode || !tbl.Page.IsPostBack) )
								{
									switch ( sCACHE_NAME )
									{
										case "AssignedUser":
											lstField.DataValueField = "ID"       ;
											lstField.DataTextField  = "USER_NAME";
											lstField.DataSource     = SplendidCache.AssignedUser();
											break;
										case "Currencies":
											lstField.DataValueField = "ID"         ;
											lstField.DataTextField  = "NAME_SYMBOL";
											lstField.DataSource     = SplendidCache.Currencies();
											break;
										case "Release":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.Release();
											break;
										case "Manufacturers":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.Manufacturers();
											break;
										case "Shippers":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.Shippers();
											break;
										case "ProductTypes":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.ProductTypes();
											break;
										case "ProductCategories":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.ProductCategories();
											break;
										case "ContractTypes":
											lstField.DataValueField = "ID"  ;
											lstField.DataTextField  = "NAME";
											lstField.DataSource     = SplendidCache.ContractTypes();
											break;
										default:
											lstField.DataValueField = "NAME"        ;
											lstField.DataTextField  = "DISPLAY_NAME";
											lstField.DataSource     = SplendidCache.List(sCACHE_NAME);
											break;
									}
									lstField.DataBind();
									// 08/08/2006 Paul.  Allow onchange code to be stored in the database.  
									// ListBoxes do not have a useful onclick event, so there should be no problem overloading this field. 
									if ( !Sql.IsEmptyString(sONCLICK_SCRIPT) )
										lstField.Attributes.Add("onchange" , sONCLICK_SCRIPT);
									// 02/21/2006 Paul.  Move the NONE item inside the !IsPostBack code. 
									if ( !bUI_REQUIRED )
										lstField.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
								}
								if ( rdr != null )
								{
									try
									{
										// 02/21/2006 Paul.  All the DropDownLists in the Calls and Meetings edit views were not getting set.  
										// The problem was a Page.DataBind in the SchedulingGrid and in the InviteesView. Both binds needed to be removed. 
										lstField.SelectedValue = Sql.ToString(rdr[sDATA_FIELD]);
									}
									catch(Exception ex)
									{
										SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
									}
								}
									// 12/04/2005 Paul.  Assigned To field will always default to the current user. 
								else if ( rdr == null && !tbl.Page.IsPostBack && sCACHE_NAME == "AssignedUser")
								{
									try
									{
										lstField.SelectedValue = Security.USER_ID.ToString();
									}
									catch(Exception ex)
									{
										SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
									}
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "CheckBox", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						CheckBox chkField = new CheckBox();
						tdField.Controls.Add(chkField);
						chkField.ID = sDATA_FIELD;
						chkField.CssClass = "checkbox";
						chkField.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( rdr != null )
								chkField.Checked = Sql.ToBoolean(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
							chkField.Enabled  = false     ;
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "ChangeButton", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						// 12/04/2005 Paul.  If the label is PARENT_TYPE, then change the label to a DropDownList.
						if ( sDATA_LABEL == "PARENT_TYPE" )
						{
							tdLabel.Controls.Clear();
							DropDownList lstField = new DropDownList();
							tdLabel.Controls.Add(lstField);
							lstField.ID       = sDATA_LABEL;
							lstField.TabIndex = nFORMAT_TAB_INDEX;
							lstField.Attributes.Add("onChange", "ChangeParentType();");
							if ( bLayoutMode || !tbl.Page.IsPostBack )
							{
								// 07/29/2005 Paul.  SugarCRM 3.0 does not allow the NONE option. 
								lstField.DataValueField = "NAME"        ;
								lstField.DataTextField  = "DISPLAY_NAME";
								lstField.DataSource     = SplendidCache.List("record_type_display");
								lstField.DataBind();
								if ( rdr != null )
								{
									try
									{
										lstField.SelectedValue = Sql.ToString(rdr[sDATA_LABEL]);
									}
									catch(Exception ex)
									{
										SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
									}
								}
							}
						}
						TextBox txtNAME = new TextBox();
						tdField.Controls.Add(txtNAME);
						txtNAME.ID       = sDISPLAY_FIELD;
						txtNAME.ReadOnly = true;
						txtNAME.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( bLayoutMode )
							{
								txtNAME.Text    = sDISPLAY_FIELD;
								txtNAME.Enabled = false         ;
							}
							else if ( !Sql.IsEmptyString(sDISPLAY_FIELD) && rdr != null )
								txtNAME.Text = Sql.ToString(rdr[sDISPLAY_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							txtNAME.Text = ex.Message;
						}
						HtmlInputHidden hidID = new HtmlInputHidden();
						tdField.Controls.Add(hidID);
						hidID.ID = sDATA_FIELD;
						try
						{
							if ( !Sql.IsEmptyString(sDATA_FIELD) && rdr != null )
								hidID.Value = Sql.ToString(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							txtNAME.Text = ex.Message;
						}
						
						Literal litNBSP = new Literal();
						tdField.Controls.Add(litNBSP);
						litNBSP.Text = "&nbsp;";
						
						HtmlInputButton btnChange = new HtmlInputButton("button");
						tdField.Controls.Add(btnChange);
						// 05/07/2006 Paul.  Specify a name for the check button so that it can be referenced by SplendidTest. 
						btnChange.ID = sDATA_FIELD + "_btnChange";
						btnChange.Attributes.Add("class", "button");
						if ( !Sql.IsEmptyString(sONCLICK_SCRIPT) )
							btnChange.Attributes.Add("onclick"  , sONCLICK_SCRIPT);
						btnChange.Attributes.Add("title"    , L10n.Term(".LBL_CHANGE_BUTTON_TITLE"));
						// 07/31/2006 Paul.  Stop using VisualBasic library to increase compatibility with Mono. 
						btnChange.Attributes.Add("accessKey", L10n.Term(".LBL_CHANGE_BUTTON_KEY").Substring(0, 1));
						btnChange.Value = L10n.Term(".LBL_CHANGE_BUTTON_LABEL");
						if ( !bLayoutMode && bUI_REQUIRED && !Sql.IsEmptyString(sDATA_FIELD) )
						{
							RequiredFieldValidatorForHiddenInputs reqID = new RequiredFieldValidatorForHiddenInputs();
							reqID.ID                 = sDATA_FIELD + "_REQUIRED";
							reqID.ControlToValidate  = hidID.ID;
							reqID.ErrorMessage       = L10n.Term(".ERR_REQUIRED_FIELD");
							reqID.CssClass           = "required";
							reqID.EnableViewState    = false;
							// 01/16/2006 Paul.  We don't enable required fields until we attempt to save. 
							// This is to allow unrelated form actions; the Cancel button is a good example. 
							reqID.EnableClientScript = false;
							reqID.Enabled            = false;
							tdField.Controls.Add(reqID);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "TextBox", true) == 0 || String.Compare(sFIELD_TYPE, "Password", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						TextBox txtField = new TextBox();
						tdField.Controls.Add(txtField);
						txtField.ID       = sDATA_FIELD;
						txtField.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( nFORMAT_ROWS > 0 && nFORMAT_COLUMNS > 0 )
							{
								txtField.Rows     = nFORMAT_ROWS   ;
								txtField.Columns  = nFORMAT_COLUMNS;
								txtField.TextMode = TextBoxMode.MultiLine;
							}
							else
							{
								txtField.MaxLength = nFORMAT_MAX_LENGTH   ;
								txtField.Attributes.Add("size", nFORMAT_SIZE.ToString());
								txtField.TextMode  = TextBoxMode.SingleLine;
							}
							if ( bLayoutMode )
							{
								txtField.Text     = sDATA_FIELD;
								txtField.ReadOnly = true       ;
							}
							else if ( !Sql.IsEmptyString(sDATA_FIELD) && rdr != null )
							{
								int    nOrdinal  = rdr.GetOrdinal(sDATA_FIELD);
								string sTypeName = rdr.GetDataTypeName(nOrdinal);
								// 03/04/2006 Paul.  Display currency in the proper format. 
								// Only SQL Server is likely to return the money type, so also include the decimal type. 
								if ( sTypeName == "money" || rdr[sDATA_FIELD].GetType() == typeof(System.Decimal) )
									txtField.Text = Sql.ToDecimal(rdr[sDATA_FIELD]).ToString("#,##0.00");
								else
									txtField.Text = Sql.ToString(rdr[sDATA_FIELD]);
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							txtField.Text = ex.Message;
						}
						if ( String.Compare(sFIELD_TYPE, "Password", true) == 0 )
							txtField.TextMode = TextBoxMode.Password;
						if ( !bLayoutMode && bUI_REQUIRED && !Sql.IsEmptyString(sDATA_FIELD) )
						{
							RequiredFieldValidator reqNAME = new RequiredFieldValidator();
							reqNAME.ID                 = sDATA_FIELD + "_REQUIRED";
							reqNAME.ControlToValidate  = txtField.ID;
							reqNAME.ErrorMessage       = L10n.Term(".ERR_REQUIRED_FIELD");
							reqNAME.CssClass           = "required";
							reqNAME.EnableViewState    = false;
							// 01/16/2006 Paul.  We don't enable required fields until we attempt to save. 
							// This is to allow unrelated form actions; the Cancel button is a good example. 
							reqNAME.EnableClientScript = false;
							reqNAME.Enabled            = false;
							tdField.Controls.Add(reqNAME);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "DatePicker", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						// 12/03/2005 Paul.  UserControls must be loaded. 
						DatePicker ctlDate = tbl.Page.LoadControl("~/_controls/DatePicker.ascx") as DatePicker;
						tdField.Controls.Add(ctlDate);
						ctlDate.ID = sDATA_FIELD;
						// 05/10/2006 Paul.  Set the tab index. 
						ctlDate.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( rdr != null )
								ctlDate.Value = T10n.FromServerTime(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						// 01/16/2006 Paul.  We validate elsewhere. 
						/*
						if ( !bLayoutMode && bUI_REQUIRED && !Sql.IsEmptyString(sDATA_FIELD) )
						{
							ctlDate.Required = true;
						}
						*/
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						// 12/03/2005 Paul.  UserControls must be loaded. 
						DateTimePicker ctlDate = tbl.Page.LoadControl("~/_controls/DateTimePicker.ascx") as DateTimePicker;
						tdField.Controls.Add(ctlDate);
						ctlDate.ID = sDATA_FIELD;
						// 05/10/2006 Paul.  Set the tab index. 
						ctlDate.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( rdr != null )
								ctlDate.Value = T10n.FromServerTime(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						// 12/03/2005 Paul.  UserControls must be loaded. 
						DateTimeEdit ctlDate = tbl.Page.LoadControl("~/_controls/DateTimeEdit.ascx") as DateTimeEdit;
						tdField.Controls.Add(ctlDate);
						ctlDate.ID = sDATA_FIELD;
						// 05/10/2006 Paul.  Set the tab index. 
						ctlDate.TabIndex = nFORMAT_TAB_INDEX;
						try
						{
							if ( rdr != null )
								ctlDate.Value = T10n.FromServerTime(rdr[sDATA_FIELD]);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						if ( !bLayoutMode && bUI_REQUIRED )
						{
							ctlDate.EnableNone = false;
						}
						if ( bLayoutMode )
						{
							Literal litField = new Literal();
							litField.Text = sDATA_FIELD;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "File", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						HtmlInputFile ctlField = new HtmlInputFile();
						tdField.Controls.Add(ctlField);
						ctlField.ID        = sDATA_FIELD;
						ctlField.MaxLength = nFORMAT_MAX_LENGTH;
						ctlField.Size      = nFORMAT_SIZE;
						ctlField.Attributes.Add("TabIndex", nFORMAT_TAB_INDEX.ToString());
						if ( !bLayoutMode && bUI_REQUIRED )
						{
							RequiredFieldValidator reqNAME = new RequiredFieldValidator();
							reqNAME.ID                 = sDATA_FIELD + "_REQUIRED";
							reqNAME.ControlToValidate  = ctlField.ID;
							reqNAME.ErrorMessage       = L10n.Term(".ERR_REQUIRED_FIELD");
							reqNAME.CssClass           = "required";
							reqNAME.EnableViewState    = false;
							// 01/16/2006 Paul.  We don't enable required fields until we attempt to save. 
							// This is to allow unrelated form actions; the Cancel button is a good example. 
							reqNAME.EnableClientScript = false;
							reqNAME.Enabled            = false;
							tdField.Controls.Add(reqNAME);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "Image", true) == 0 )
				{
					if ( !Sql.IsEmptyString(sDATA_FIELD) )
					{
						HtmlInputHidden ctlHidden = new HtmlInputHidden();
						if ( !bLayoutMode )
						{
							tdField.Controls.Add(ctlHidden);
							ctlHidden.ID = sDATA_FIELD;

							HtmlInputFile ctlField = new HtmlInputFile();
							tdField.Controls.Add(ctlField);
							// 04/17/2006 Paul.  The image needs to reference the file control. 
							ctlField.ID = sDATA_FIELD + "_File";

							Literal litBR = new Literal();
							litBR.Text = "<br />";
							tdField.Controls.Add(litBR);
						}
						
						Image imgField = new Image();
						// 04/13/2006 Paul.  Give the image a name so that it can be validated with SplendidTest. 
						imgField.ID = "img" + sDATA_FIELD;
						try
						{
							if ( bLayoutMode )
							{
								Literal litField = new Literal();
								litField.Text = sDATA_FIELD;
								tdField.Controls.Add(litField);
							}
							else if ( rdr != null )
							{
								if ( !Sql.IsEmptyString(rdr[sDATA_FIELD]) )
								{
									ctlHidden.Value = Sql.ToString(rdr[sDATA_FIELD]);
									imgField.ImageUrl = "~/Images/Image.aspx?ID=" + ctlHidden.Value;
									// 04/13/2006 Paul.  Only add the image if it exists. 
									tdField.Controls.Add(imgField);
									
									// 04/17/2006 Paul.  Provide a clear button. 
									Literal litClear = new Literal();
									litClear.Text = "<br /><input type=\"button\" class=\"button\" onclick=\"form." + ctlHidden.ClientID + ".value='';form." + imgField.ClientID + ".src='';" + "\"  value='" + "  " + L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) + "  " + "' title='" + L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) + "' />";
									tdField.Controls.Add(litClear);
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
							Literal litField = new Literal();
							litField.Text = ex.Message;
							tdField.Controls.Add(litField);
						}
					}
				}
				else if ( String.Compare(sFIELD_TYPE, "AddressButtons", true) == 0 )
				{
					tr.Cells.Remove(tdField);
					tdLabel.Width = "10%";
					tdLabel.RowSpan = nROWSPAN;
					tdLabel.VAlign  = "middle";
					tdLabel.Align   = "center";
					tdLabel.Attributes.Remove("class");
					tdLabel.Attributes.Add("class", "tabFormAddDel");
					HtmlInputButton btnCopyRight = new HtmlInputButton("button");
					Literal         litSpacer    = new Literal();
					HtmlInputButton btnCopyLeft  = new HtmlInputButton("button");
					tdLabel.Controls.Add(btnCopyRight);
					tdLabel.Controls.Add(litSpacer   );
					tdLabel.Controls.Add(btnCopyLeft );
					btnCopyRight.Attributes.Add("title"  , L10n.Term("Accounts.NTC_COPY_BILLING_ADDRESS" ));
					btnCopyRight.Attributes.Add("onclick", "return copyAddressRight()");
					btnCopyRight.Value = ">>";
					litSpacer.Text = "<br><br>";
					btnCopyLeft .Attributes.Add("title"  , L10n.Term("Accounts.NTC_COPY_SHIPPING_ADDRESS" ));
					btnCopyLeft .Attributes.Add("onclick", "return copyAddressLeft()");
					btnCopyLeft .Value = "<<";
					nColIndex = 0;
				}
				else
				{
					Literal litField = new Literal();
					tdField.Controls.Add(litField);
					litField.Text = "Unknown field type " + sFIELD_TYPE;
					SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Unknown field type " + sFIELD_TYPE);
				}
				if ( nCOLSPAN > 0 )
					nColIndex = 0;
				else if ( nCOLSPAN == 0 )
				{
					nColIndex++;
					if ( nColIndex > 1 )
						nColIndex = 0;
				}
			}
		}

		public static void LoadFile(Guid gID, Stream stm, IDbTransaction trn)
		{
			const int BUFFER_LENGTH = 4*1024;
			byte[] binFILE_POINTER = new byte[16];
			// 01/20/2006 Paul.  Must include in transaction
			SqlProcs.spIMAGE_InitPointer(gID, ref binFILE_POINTER, trn);
			using ( BinaryReader reader = new BinaryReader(stm) )
			{
				int nFILE_OFFSET = 0 ;
				byte[] binBYTES = reader.ReadBytes(BUFFER_LENGTH);
				while ( binBYTES.Length > 0 )
				{
					// 08/14/2005 Paul.  gID is used by Oracle, binFILE_POINTER is used by SQL Server. 
					// 01/20/2006 Paul.  Must include in transaction
					SqlProcs.spIMAGE_WriteOffset(gID, binFILE_POINTER, nFILE_OFFSET, binBYTES, trn);
					nFILE_OFFSET += binBYTES.Length;
					binBYTES = reader.ReadBytes(BUFFER_LENGTH);
				}
			}
		}

		public static bool LoadImage(SplendidControl ctlPARENT, Guid gParentID, string sFIELD_NAME, IDbTransaction trn)
		{
			bool bNewFile = false;
			HtmlInputFile fileIMAGE = ctlPARENT.FindControl(sFIELD_NAME + "_File") as HtmlInputFile;
			if ( fileIMAGE != null )
			{
				HttpPostedFile pstIMAGE  = fileIMAGE.PostedFile;
				if ( pstIMAGE != null )
				{
					long lFileSize      = pstIMAGE.ContentLength;
					long lUploadMaxSize = Sql.ToLong(HttpContext.Current.Application["CONFIG.upload_maxsize"]);
					if ( (lUploadMaxSize > 0) && (lFileSize > lUploadMaxSize) )
					{
						throw(new Exception("ERROR: uploaded file was too big: max filesize: " + lUploadMaxSize.ToString()));
					}
					// 04/13/2005 Paul.  File may not have been provided. 
					if ( pstIMAGE.FileName.Length > 0 )
					{
						string sFILENAME       = Path.GetFileName (pstIMAGE.FileName);
						string sFILE_EXT       = Path.GetExtension(sFILENAME);
						string sFILE_MIME_TYPE = pstIMAGE.ContentType;
						
						Guid gImageID = Guid.Empty;
						SqlProcs.spIMAGES_Insert
							( ref gImageID
							, gParentID
							, sFILENAME
							, sFILE_EXT
							, sFILE_MIME_TYPE
							, trn
							);
						SplendidDynamic.LoadFile(gImageID, pstIMAGE.InputStream, trn);
						// 04/17/2006 Paul.  Update the dynamic control so that it can be accessed below. 
						DynamicControl ctlIMAGE = new DynamicControl(ctlPARENT, sFIELD_NAME);
						ctlIMAGE.ID = gImageID;
						bNewFile = true;
					}
				}
			}
			return bNewFile;
		}

		public static void UpdateCustomFields(SplendidControl ctlPARENT, IDbTransaction trn, Guid gID, string sCUSTOM_MODULE, DataTable dtCustomFields)
		{
			if ( dtCustomFields.Rows.Count > 0 )
			{
				IDbConnection con = trn.Connection;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.Transaction = trn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "update " + sCUSTOM_MODULE + "_CSTM" + ControlChars.CrLf;
					int nFieldIndex = 0;
					foreach(DataRow row in dtCustomFields.Rows)
					{
						// 01/11/2006 Paul.  Uppercase looks better. 
						string sNAME   = Sql.ToString(row["NAME"  ]).ToUpper();
						string sCsType = Sql.ToString(row["CsType"]);
						DynamicControl ctlCustomField = new DynamicControl(ctlPARENT, sNAME);
						if ( ctlCustomField.Exists )
						{
							if ( nFieldIndex == 0 )
								cmd.CommandText += "   set ";
							else
								cmd.CommandText += "     , ";
							// 01/10/2006 Paul.  We can't use a StringBuilder because the Sql.AddParameter function
							// needs to be able to replace the @ with the appropriate database specific token. 
							cmd.CommandText += sNAME + " = @" + sNAME + ControlChars.CrLf;
							
							DynamicControl ctlCustomField_File = new DynamicControl(ctlPARENT, sNAME + "_File");
							// 04/21/2006 Paul.  If the type is Guid and it is accompanied by a File control, then assume it is an image. 
							if ( sCsType == "Guid" && ctlCustomField.Type == "HtmlInputHidden" && ctlCustomField_File.Exists )
							{
								LoadImage(ctlPARENT, gID, sNAME, trn);
							}
							// 04/21/2006 Paul.  Even if there is no image to upload, we still need to update the field.
							// This is so that the image can be cleared. 
							switch ( sCsType )
							{
								case "Guid"    :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.ID          );  break;
								case "short"   :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.IntegerValue);  break;
								case "Int32"   :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.IntegerValue);  break;
								case "Int64"   :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.IntegerValue);  break;
								case "float"   :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.FloatValue  );  break;
								case "decimal" :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.DecimalValue);  break;
								case "bool"    :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.Checked     );  break;
								case "DateTime":  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.DateValue   );  break;
								default        :  Sql.AddParameter(cmd, "@" + sNAME, ctlCustomField.Text        );  break;
							}
							nFieldIndex++;
						}
					}
					if ( nFieldIndex > 0 )
					{
						cmd.CommandText += " where ID_C = @ID_C" + ControlChars.CrLf;
						Sql.AddParameter(cmd, "@ID_C", gID);
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

	}
}
