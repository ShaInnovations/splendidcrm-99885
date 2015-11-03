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
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic;
using FredCK.FCKeditorV2;

namespace SplendidCRM.Emails
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.ModuleHeader       ctlModuleHeader;
		protected _controls.EditButtons        ctlEditButtons ;
		protected Email._controls.EmailButtons ctlEmailButtons;

		protected string          sEMAIL_TYPE                  ;
		protected Guid            gID                          ;
		protected DropDownList    lstASSIGNED_USER_ID          ;
		protected DropDownList    lstEMAIL_TEMPLATE            ;
		protected DropDownList    lstPARENT_TYPE               ;
		protected TextBox         txtPARENT_NAME               ;
		protected HtmlInputHidden txtPARENT_ID                 ;
		protected HtmlTableRow    trFROM                       ;
		protected HtmlTableRow    trNOTE_SEMICOLON             ;
		protected HtmlInputHidden txtFROM_NAME                 ;  // 11/20/2005.  Not used by SugarCRM 3.5.1.
		protected TextBox         txtFROM_ADDR                 ;
		protected TextBox         txtTO_ADDRS                  ;
		protected TextBox         txtCC_ADDRS                  ;
		protected TextBox         txtBCC_ADDRS                 ;
		protected HtmlInputHidden txtTO_ADDRS_IDS              ;
		protected HtmlInputHidden txtTO_ADDRS_NAMES            ;
		protected HtmlInputHidden txtTO_ADDRS_EMAILS           ;
		protected HtmlInputHidden txtCC_ADDRS_IDS              ;
		protected HtmlInputHidden txtCC_ADDRS_NAMES            ;
		protected HtmlInputHidden txtCC_ADDRS_EMAILS           ;
		protected HtmlInputHidden txtBCC_ADDRS_IDS             ;
		protected HtmlInputHidden txtBCC_ADDRS_NAMES           ;
		protected HtmlInputHidden txtBCC_ADDRS_EMAILS          ;
		protected TextBox         txtNAME                      ;
		protected FCKeditor       txtDESCRIPTION               ;
		// 04/16/2006 Paul.  The subject is not required. 
		//protected Label           lblNAME_REQUIRED             ;
		//protected RequiredFieldValidator reqNAME               ;

		protected HtmlGenericControl     spnDATE_START;
		protected HtmlGenericControl     spnTEMPLATE_LABEL;
		protected _controls.DateTimeEdit ctlDATE_START;
		protected Repeater               ctlAttachments;

		public static MailAddress SplitMailAddress(string sFullAddress)
		{
			string sName    = String.Empty;
			string sAddress = String.Empty;
			int nStartAddress = sFullAddress.IndexOf('<');
			if ( nStartAddress > 0 )
			{
				sName = sFullAddress.Substring(0, nStartAddress-1);
				sName = sName.Trim();
				sAddress = sFullAddress.Substring(nStartAddress+1);
				int nEndAddress = sAddress.IndexOf('>');
				if ( nEndAddress >= 0 )
					sAddress = sAddress.Substring(0, nEndAddress);
			}
			else
			{
				sAddress = sFullAddress;
			}
			if ( sName != String.Empty )
				return new MailAddress(sAddress, sName);
			else
				return new MailAddress(sAddress);
		}

		public static void SendEmail(Guid gID)
		{
			string sFromName    = Sql.ToString(HttpContext.Current.Session["USER_SETTINGS/MAIL_FROMNAME"   ]);
			string sFromAddress = Sql.ToString(HttpContext.Current.Session["USER_SETTINGS/MAIL_FROMADDRESS"]);
			
			MailMessage mail = new MailMessage();
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *                   " + ControlChars.CrLf
				     + "  from vwEMAILS_ReadyToSend" + ControlChars.CrLf
				     + " where ID = @ID            " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ID", gID);
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							string sFrom     = Sql.ToString(rdr["FROM_ADDR"       ]);
							string sTo       = Sql.ToString(rdr["TO_ADDRS"        ]);
							string sCC       = Sql.ToString(rdr["CC_ADDRS"        ]);
							string sBcc      = Sql.ToString(rdr["BCC_ADDRS"       ]);
							string sSubject  = Sql.ToString(rdr["NAME"            ]);
							string sBody     = Sql.ToString(rdr["DESCRIPTION"     ]);
							string sBodyHtml = Sql.ToString(rdr["DESCRIPTION_HTML"]);
							if ( Sql.IsEmptyString(sFrom) && !Sql.IsEmptyString(sFromAddress) )
								mail.From = new MailAddress(sFromAddress, sFromName);
							else if ( !Sql.IsEmptyString(sFrom) )
								mail.From = SplitMailAddress(sFrom);
							string[] arrAddresses = sTo.Split(';');
							foreach ( string sAddress in arrAddresses )
							{
								if ( sAddress.Trim() != String.Empty )
									mail.To.Add(SplitMailAddress(sAddress));
							}
							arrAddresses = sCC.Split(';');
							foreach ( string sAddress in arrAddresses )
							{
								if ( sAddress.Trim() != String.Empty )
									mail.CC.Add(SplitMailAddress(sAddress));
							}
							arrAddresses = sBcc.Split(';');
							foreach ( string sAddress in arrAddresses )
							{
								if ( sAddress.Trim() != String.Empty )
									mail.Bcc.Add(SplitMailAddress(sAddress));
							}
							mail.Subject     = sSubject       ;
							if ( !Sql.IsEmptyString(sBodyHtml) )
							{
								mail.Body         = sBodyHtml;
								// 08/24/2006 Paul.  Set the encoding to UTF8. 
								mail.BodyEncoding = System.Text.Encoding.UTF8;
								mail.IsBodyHtml   = true;
							}
							else
							{
								mail.Body       = sBody    ;
							}
							mail.Headers.Add("X-SplendidCRM-ID", gID.ToString());
						}
						else
						{
							throw(new Exception("SendEmail: Email is not ready to send, " + gID.ToString()));
						}
					}
				}

				// 07/30/2006 Paul.  .NET 2.0 now supports sending mail from a stream, remove the directory stuff. 
				using ( DataTable dtAttachments = new DataTable() )
				{
					sSQL = "select *                   " + ControlChars.CrLf
					     + "  from vwEMAILS_Attachments" + ControlChars.CrLf
					     + " where EMAIL_ID = @EMAIL_ID" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@EMAIL_ID", gID);
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							da.Fill(dtAttachments);
						}
					}
					
					try
					{
						if ( dtAttachments.Rows.Count > 0 )
						{
							foreach(DataRow row in dtAttachments.Rows)
							{
								string sFILENAME           = Sql.ToString(row["FILENAME"          ]);
								string sFILE_MIME_TYPE     = Sql.ToString(row["FILE_MIME_TYPE"    ]);
								Guid   gNOTE_ATTACHMENT_ID = Sql.ToGuid  (row["NOTE_ATTACHMENT_ID"]);

								// 07/30/2006 Paul.  We cannot close the streams until the message is sent. 
								MemoryStream mem = new MemoryStream();
								BinaryWriter writer = new BinaryWriter(mem);
								Notes.Attachment.WriteStream(gNOTE_ATTACHMENT_ID, con, writer);
								writer.Flush();
								mem.Seek(0, SeekOrigin.Begin);
								Attachment att = new Attachment(mem, sFILENAME, sFILE_MIME_TYPE);
								mail.Attachments.Add(att);
							}
						}
						// 04/17/2006 Paul.  Use config value for SMTP server. 
						string sSmtpServer = Sql.ToString(HttpContext.Current.Application["CONFIG.smtpserver"]);
						if ( Sql.IsEmptyString(sSmtpServer) )
							sSmtpServer = "127.0.0.1";
						SmtpClient client = new SmtpClient(sSmtpServer);
						client.UseDefaultCredentials = true;
						client.Send(mail);
					}
					finally
					{
						// 07/30/2006 Paul.  Close the streams after the message is sent. 
						foreach ( Attachment att in mail.Attachments )
						{
							if ( att.ContentStream != null )
								att.ContentStream.Close();
						}
					}
				}
			}
		}

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			// 08/21/2005 Paul.  Redirect to parent if that is where the note was originated. 
			Guid   gPARENT_ID   = Sql.ToGuid(Request["PARENT_ID"]);
			string sMODULE      = String.Empty;
			string sPARENT_TYPE = String.Empty;
			string sPARENT_NAME = String.Empty;
			try
			{
				SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				// The only possible error is a connection failure, so just ignore all errors. 
				gPARENT_ID = Guid.Empty;
			}
			if ( e.CommandName == "Save" || e.CommandName == "Send" )
			{
				if ( ctlDATE_START.Visible )
					ctlDATE_START.Validate();
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "EMAILS";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								//txtDESCRIPTION     .Text  = txtDESCRIPTION     .Text .Trim();
								txtFROM_NAME       .Value = txtFROM_NAME       .Value.Trim();
								txtFROM_ADDR       .Text  = txtFROM_ADDR       .Text .Trim();
								txtTO_ADDRS        .Text  = txtTO_ADDRS        .Text .Trim();
								txtCC_ADDRS        .Text  = txtCC_ADDRS        .Text .Trim();
								txtBCC_ADDRS       .Text  = txtBCC_ADDRS       .Text .Trim();
								txtTO_ADDRS_IDS    .Value = txtTO_ADDRS_IDS    .Value.Trim();
								txtTO_ADDRS_NAMES  .Value = txtTO_ADDRS_NAMES  .Value.Trim();
								txtTO_ADDRS_EMAILS .Value = txtTO_ADDRS_EMAILS .Value.Trim();
								txtCC_ADDRS_IDS    .Value = txtCC_ADDRS_IDS    .Value.Trim();
								txtCC_ADDRS_NAMES  .Value = txtCC_ADDRS_NAMES  .Value.Trim();
								txtCC_ADDRS_EMAILS .Value = txtCC_ADDRS_EMAILS .Value.Trim();
								txtBCC_ADDRS_IDS   .Value = txtBCC_ADDRS_IDS   .Value.Trim();
								txtBCC_ADDRS_NAMES .Value = txtBCC_ADDRS_NAMES .Value.Trim();
								txtBCC_ADDRS_EMAILS.Value = txtBCC_ADDRS_EMAILS.Value.Trim();
								if ( e.CommandName == "Send" )
								{
									// 01/21/2006 Paul.  Mark an email as ready-to-send.   Type becomes "out" and Status stays at "draft". 
									if ( sEMAIL_TYPE == "draft" )
										sEMAIL_TYPE = "out";
									// 01/21/2006 Paul.  Address error only when sending. 
									if ( txtTO_ADDRS.Text.Length == 0 && txtCC_ADDRS.Text.Length == 0 && txtBCC_ADDRS.Text.Length == 0 )
										throw(new Exception(L10n.Term("Emails.ERR_NOT_ADDRESSED")));
								}
								// 11/20/2005 Paul.  SugarCRM 3.5.1 lets bad data flow through.  We clear the hidden values if the visible values are empty. 
								// There still is the issue of the data getting out of sync if the user manually edits the visible values. 
								if ( txtTO_ADDRS.Text.Length == 0 )
								{
									txtTO_ADDRS_IDS    .Value = String.Empty;
									txtTO_ADDRS_NAMES  .Value = String.Empty;
									txtTO_ADDRS_EMAILS .Value = String.Empty;
								}
								if ( txtCC_ADDRS.Text.Length == 0 )
								{
									txtCC_ADDRS_IDS    .Value = String.Empty;
									txtCC_ADDRS_NAMES  .Value = String.Empty;
									txtCC_ADDRS_EMAILS .Value = String.Empty;
								}
								if ( txtBCC_ADDRS.Text.Length == 0 )
								{
									txtBCC_ADDRS_IDS   .Value = String.Empty;
									txtBCC_ADDRS_NAMES .Value = String.Empty;
									txtBCC_ADDRS_EMAILS.Value = String.Empty;
								}
								
								// 04/24/2006 Paul.  Upgrade to SugarCRM 4.2 Schema. 
								// 06/01/2006 Paul.  MESSAGE_ID is now a text string. 
								SqlProcs.spEMAILS_Update
									( ref gID
									, Sql.ToGuid(lstASSIGNED_USER_ID.SelectedValue)
									, txtNAME.Text
									, T10n.ToServerTime(ctlDATE_START.Value)
									, lstPARENT_TYPE.SelectedValue
									, Sql.ToGuid(txtPARENT_ID.Value)
									// 04/16/2006 Paul.  Since the Plug-in saves body in DESCRIPTION, we need to continue to use it as the primary source of data. 
									, txtDESCRIPTION     .Value  // DESCRIPTION
									, txtDESCRIPTION     .Value  // DESCRIPTION_HTML
									, txtFROM_NAME       .Value
									, txtFROM_ADDR       .Text
									, txtTO_ADDRS        .Text
									, txtCC_ADDRS        .Text
									, txtBCC_ADDRS       .Text
									, txtTO_ADDRS_IDS    .Value
									, txtTO_ADDRS_NAMES  .Value
									, txtTO_ADDRS_EMAILS .Value
									, txtCC_ADDRS_IDS    .Value
									, txtCC_ADDRS_NAMES  .Value
									, txtCC_ADDRS_EMAILS .Value
									, txtBCC_ADDRS_IDS   .Value
									, txtBCC_ADDRS_NAMES .Value
									, txtBCC_ADDRS_EMAILS.Value
									, sEMAIL_TYPE
									, new DynamicControl(this, "MESSAGE_ID"   ).Text
									, new DynamicControl(this, "REPLY_TO_NAME").Text
									, new DynamicControl(this, "REPLY_TO_ADDR").Text
									, new DynamicControl(this, "INTENT"       ).Text
									, new DynamicControl(this, "MAILBOX_ID"   ).ID
									, trn
									);
								
								// 01/21/2006 Paul.  There can be a maximum of 10 attachments, not including attachments that were previously saved. 
								for ( int i=0; i < 10; i++ )
								{
									HtmlInputFile fileATTACHMENT = FindControl("email_attachment" + i.ToString()) as HtmlInputFile;
									if ( fileATTACHMENT != null )
									{
										HttpPostedFile pstATTACHMENT = fileATTACHMENT.PostedFile;
										if ( pstATTACHMENT != null )
										{
											long lFileSize      = pstATTACHMENT.ContentLength;
											long lUploadMaxSize = Sql.ToLong(Application["CONFIG.upload_maxsize"]);
											if ( (lUploadMaxSize > 0) && (lFileSize > lUploadMaxSize) )
											{
												throw(new Exception("ERROR: uploaded file was too big: max filesize: " + lUploadMaxSize.ToString()));
											}
											// 08/20/2005 Paul.  File may not have been provided. 
											if ( pstATTACHMENT.FileName.Length > 0 )
											{
												string sFILENAME       = Path.GetFileName (pstATTACHMENT.FileName);
												string sFILE_EXT       = Path.GetExtension(sFILENAME);
												string sFILE_MIME_TYPE = pstATTACHMENT.ContentType;
											
												Guid gNoteID = Guid.Empty;
												SqlProcs.spNOTES_Update
													( ref gNoteID
													, "Email Attachment: " + sFILENAME
													, "Emails"   // Parent Type
													, gID        // Parent ID
													, Guid.Empty
													, String.Empty
													, trn
													);

												Guid gAttachmentID = Guid.Empty;
												// 01/20/2006 Paul.  Must include in transaction
												SqlProcs.spNOTE_ATTACHMENTS_Insert(ref gAttachmentID, gNoteID, pstATTACHMENT.FileName, sFILENAME, sFILE_EXT, sFILE_MIME_TYPE, trn);
												Notes.EditView.LoadFile(gAttachmentID, pstATTACHMENT.InputStream, trn);
											}
										}
									}
								}
								//SplendidDynamic.UpdateCustomFields(this, trn, gID, sCUSTOM_MODULE, dtCustomFields);
								trn.Commit();
								// 01/21/2006 Paul.  In case the SendMail function fails, we want to make sure to reuse the GUID. 
								ViewState["ID"] = gID;
							}
							catch(Exception ex)
							{
								trn.Rollback();
								SplendidError.SystemError(new StackTrace(true).GetFrame(0),  Utils.ExpandException(ex));
								ctlEditButtons.ErrorText = Utils.ExpandException(ex);
								ctlEmailButtons.ErrorText = ctlEditButtons.ErrorText;
								return;
							}
							try
							{
								if ( e.CommandName == "Send" )
									SendEmail(gID);
							}
							catch(Exception ex)
							{
								SplendidError.SystemError(new StackTrace(true).GetFrame(0), Utils.ExpandException(ex));
								ctlEditButtons.ErrorText = Utils.ExpandException(ex);
								ctlEmailButtons.ErrorText = ctlEditButtons.ErrorText;
								return;
							}
						}
					}
					if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
					else if ( sEMAIL_TYPE == "draft" )
						Response.Redirect("default.aspx");
					else
						Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				if ( !Sql.IsEmptyGuid(gPARENT_ID) )
					Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
				// 09/07/2006 Paul.  If in draft mode, redirect to list.  Viewing a draft will re-direct you to edit mode.
				else if ( Sql.IsEmptyGuid(gID) || Sql.ToString(ViewState["TYPE"]) == "draft" )
					Response.Redirect("default.aspx");
				else
					Response.Redirect("view.aspx?ID=" + gID.ToString());
			}
		}

		protected void lstEMAIL_TEMPLATE_Changed(Object sender, EventArgs e)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL ;
				sSQL = "select *                     " + ControlChars.CrLf
				     + "  from vwEMAIL_TEMPLATES_Edit" + ControlChars.CrLf
				     + " where ID = @ID              " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@ID", Sql.ToGuid(lstEMAIL_TEMPLATE.SelectedValue));
					con.Open();
					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							txtNAME.Text         = Sql.ToString(rdr["NAME"]);
							txtDESCRIPTION.Value = Sql.ToString(rdr["BODY"]);
						}
					}
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				// 01/21/2006 Paul.  If there is an error sending the email, we want to make sure to reuse the ID,
				// otherwise multiple emails get created as the user tries to resend. 
				gID = Sql.ToGuid(ViewState["ID"]);
				if ( Sql.IsEmptyGuid(gID) )
					gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					sEMAIL_TYPE = Sql.ToString(Request["TYPE"]).ToLower();
					if ( sEMAIL_TYPE != "archived" )
						sEMAIL_TYPE = "draft";
					ctlEditButtons .Visible  = !PrintView && (sEMAIL_TYPE != "draft");
					ctlEmailButtons.Visible  = !PrintView && (sEMAIL_TYPE == "draft");
					
					if ( Sql.IsEmptyGuid(gID) )
					{
						ctlModuleHeader.EnableModuleLabel = false;
						if ( sEMAIL_TYPE == "archived" )
							ctlModuleHeader.Title = L10n.Term("Emails.LBL_ARCHIVED_MODULE_NAME") + ":";
						else
							ctlModuleHeader.Title = L10n.Term("Emails.LBL_COMPOSE_MODULE_NAME") + ":";
						// 04/16/2006 Paul.  The subject is not required. 
						//lblNAME_REQUIRED .Visible = (sEMAIL_TYPE == "archived");
						//reqNAME.Enabled          =  lblNAME_REQUIRED.Visible;
						ctlDATE_START    .Visible = (sEMAIL_TYPE == "archived");
						spnDATE_START    .Visible =  ctlDATE_START.Visible;
						spnTEMPLATE_LABEL.Visible = (sEMAIL_TYPE == "draft"   );
						lstEMAIL_TEMPLATE.Visible = spnTEMPLATE_LABEL.Visible;
						trNOTE_SEMICOLON .Visible = (sEMAIL_TYPE == "draft"   );
						trFROM           .Visible = !trNOTE_SEMICOLON.Visible;
						ViewState["TYPE"] = sEMAIL_TYPE;
						ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;
					}

					lstASSIGNED_USER_ID.DataSource = SplendidCache.AssignedUser();
					lstASSIGNED_USER_ID.DataBind();
					lstASSIGNED_USER_ID.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
					lstPARENT_TYPE     .DataSource = SplendidCache.List("record_type_display");
					lstPARENT_TYPE     .DataBind();
					if ( lstEMAIL_TEMPLATE.Visible )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                     " + ControlChars.CrLf
							     + "  from vwEMAIL_TEMPLATES_List" + ControlChars.CrLf
							     + " order by NAME               " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										lstEMAIL_TEMPLATE.DataSource = dt.DefaultView;
										lstEMAIL_TEMPLATE.DataBind();
										lstEMAIL_TEMPLATE.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
									}
								}
							}
						}
					}
					// 07/29/2005 Paul.  SugarCRM 3.0 does not allow the NONE option. 
					//lstPARENT_TYPE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *            " + ControlChars.CrLf
							     + "  from vwEMAILS_Edit" + ControlChars.CrLf
							     + " where ID = @ID     " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								if ( !Sql.IsEmptyGuid(gDuplicateID) )
								{
									Sql.AddParameter(cmd, "@ID", gDuplicateID);
									gID = Guid.Empty;
								}
								else
								{
									Sql.AddParameter(cmd, "@ID", gID);
								}
								con.Open();
#if DEBUG
								Page.RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
#endif
								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										ctlModuleHeader.Title += Sql.ToString(rdr["NAME"]);
										Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
										Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
										ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;
										ViewState["ID"] = gID;

										txtNAME            .Text  = Sql.ToString(rdr["NAME"            ]);
										ctlDATE_START      .Value = T10n.FromServerTime(rdr["DATE_START"]);
										txtPARENT_ID       .Value = Sql.ToString(rdr["PARENT_ID"       ]);
										txtPARENT_NAME     .Text  = Sql.ToString(rdr["PARENT_NAME"     ]);
										txtFROM_NAME       .Value = Sql.ToString(rdr["FROM_NAME"       ]);
										txtFROM_ADDR       .Text  = Sql.ToString(rdr["FROM_ADDR"       ]);

										txtTO_ADDRS        .Text  = Sql.ToString(rdr["TO_ADDRS"        ]);
										txtCC_ADDRS        .Text  = Sql.ToString(rdr["CC_ADDRS"        ]);
										txtBCC_ADDRS       .Text  = Sql.ToString(rdr["BCC_ADDRS"       ]);
										txtTO_ADDRS_IDS    .Value = Sql.ToString(rdr["TO_ADDRS_IDS"    ]);
										txtTO_ADDRS_NAMES  .Value = Sql.ToString(rdr["TO_ADDRS_NAMES"  ]);
										txtTO_ADDRS_EMAILS .Value = Sql.ToString(rdr["TO_ADDRS_EMAILS" ]);
										txtCC_ADDRS_IDS    .Value = Sql.ToString(rdr["CC_ADDRS_IDS"    ]);
										txtCC_ADDRS_NAMES  .Value = Sql.ToString(rdr["CC_ADDRS_NAMES"  ]);
										txtCC_ADDRS_EMAILS .Value = Sql.ToString(rdr["CC_ADDRS_EMAILS" ]);
										txtBCC_ADDRS_IDS   .Value = Sql.ToString(rdr["BCC_ADDRS_IDS"   ]);
										txtBCC_ADDRS_NAMES .Value = Sql.ToString(rdr["BCC_ADDRS_NAMES" ]);
										txtBCC_ADDRS_EMAILS.Value = Sql.ToString(rdr["BCC_ADDRS_EMAILS"]);

										// 04/16/2006 Paul.  Since the Plug-in saves body in DESCRIPTION, we need to continue to use it as the primary source of data. 
										txtDESCRIPTION     .Value = Sql.ToString(rdr["DESCRIPTION"]);
										try
										{
											lstPARENT_TYPE.SelectedValue = Sql.ToString(rdr["PARENT_TYPE"]);
										}
										catch(Exception ex)
										{
											SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
										}
										try
										{
											lstASSIGNED_USER_ID.SelectedValue = Sql.ToString(rdr["ASSIGNED_USER_ID"]);
										}
										catch(Exception ex)
										{
											SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
										}
										// 11/17/2005 Paul.  Archived emails allow editing of the Date & Time Sent. 
										sEMAIL_TYPE = Sql.ToString(rdr["TYPE"]).ToLower();
										switch ( sEMAIL_TYPE )
										{
											case "archived":
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_ARCHIVED_MODULE_NAME") + ":" + txtNAME.Text;
												break;
											case "out":
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_LIST_FORM_SENT_TITLE") + ":" + txtNAME.Text;
												break;
											default:
												sEMAIL_TYPE = "draft";
												ctlModuleHeader.Title = L10n.Term("Emails.LBL_COMPOSE_MODULE_NAME" ) + ":" + txtNAME.Text;
												break;
										}
										if ( sEMAIL_TYPE == "out" )
										{
											// 01/21/2006 Paul.  Editing is not allowed for sent emails. 
											Response.Redirect("view.aspx?ID=" + gID.ToString());
											return;
										}
										// 04/16/2006 Paul.  The subject is not required. 
										//lblNAME_REQUIRED .Visible = (sEMAIL_TYPE == "archived");
										//reqNAME.Enabled = lblNAME_REQUIRED.Visible;
										ctlDATE_START    .Visible = (sEMAIL_TYPE == "archived");
										spnDATE_START    .Visible =  ctlDATE_START.Visible;
										spnTEMPLATE_LABEL.Visible = (sEMAIL_TYPE == "draft"   );
										lstEMAIL_TEMPLATE.Visible = spnTEMPLATE_LABEL.Visible;
										trNOTE_SEMICOLON .Visible = (sEMAIL_TYPE == "draft"   );
										trFROM           .Visible = !trNOTE_SEMICOLON.Visible;
										ctlModuleHeader.EnableModuleLabel = false;

										ctlEditButtons .Visible  = !PrintView && (sEMAIL_TYPE != "draft");
										ctlEmailButtons.Visible  = !PrintView && (sEMAIL_TYPE == "draft");
										ViewState["TYPE"] = sEMAIL_TYPE;
									}
								}
							}
							sSQL = "select *                   " + ControlChars.CrLf
							     + "  from vwEMAILS_Attachments" + ControlChars.CrLf
							     + " where EMAIL_ID = @EMAIL_ID" + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@EMAIL_ID", gID);
#if DEBUG
								Page.RegisterClientScriptBlock("vwEMAILS_Attachments", Sql.ClientScriptBlock(cmd));
#endif
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										ctlAttachments.DataSource = dt.DefaultView;
										ctlAttachments.DataBind();
									}
								}
							}
						}
					}
					else
					{
						Guid gPARENT_ID = Sql.ToGuid(Request["PARENT_ID"]);
						if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						{
							string sMODULE      = String.Empty;
							string sPARENT_TYPE = String.Empty;
							string sPARENT_NAME = String.Empty;
							SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
							if ( !Sql.IsEmptyGuid(gPARENT_ID) )
							{
								txtPARENT_ID  .Value = gPARENT_ID.ToString();
								txtPARENT_NAME.Text  = sPARENT_NAME;
								try
								{
									lstPARENT_TYPE.SelectedValue = sPARENT_TYPE;
								}
								catch(Exception ex)
								{
									SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
								}
								// 08/05/2006 Paul.  When an email is composed from a Lead, automatically set the To address. 
								DbProviderFactory dbf = DbProviderFactories.GetFactory();
								using ( IDbConnection con = dbf.CreateConnection() )
								{
									string sSQL ;
									sSQL = "select EMAIL1      " + ControlChars.CrLf
									     + "  from vwLEADS_Edit" + ControlChars.CrLf
									     + " where ID = @ID    " + ControlChars.CrLf;
									using ( IDbCommand cmd = con.CreateCommand() )
									{
										cmd.CommandText = sSQL;
										Sql.AddParameter(cmd, "@ID", gPARENT_ID);
										con.Open();
#if DEBUG
										Page.RegisterClientScriptBlock("vwLEADS_Edit", Sql.ClientScriptBlock(cmd));
#endif
										using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
										{
											if ( rdr.Read() )
											{
												txtTO_ADDRS.Text = Sql.ToString(rdr["EMAIL1"]);
											}
										}
									}
								}
							}
						}
						try
						{
							lstASSIGNED_USER_ID.SelectedValue = Security.USER_ID.ToString();
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
					}
				}
				else
				{
					// 12/02/2005 Paul.  When validation fails, the header title does not retain its value.  Update manually. 
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
					sEMAIL_TYPE = Sql.ToString(ViewState["TYPE"]);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				ctlEditButtons.ErrorText = ex.Message;
				ctlEmailButtons.ErrorText = ex.Message;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This Meeting is required by the ASP.NET Web Form Designer.
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
			ctlEditButtons.Command = new CommandEventHandler(Page_Command);
			ctlEmailButtons.Command = new CommandEventHandler(Page_Command);
			m_sMODULE = "Emails";
		}
		#endregion
	}
}
