<%@ Control CodeBehind="FeedDetailView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Feeds.FeedDetailView" %>
<%@ Import Namespace="SplendidCRM" %>
<div id="divDetailView">
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabDetailView">
		<tr>
			<td class="tabDetailViewDF">
				<table class="mod" border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td bgcolor="aaaaaa">
							<table border="0" cellpadding="2" cellspacing="0" width="100%">
								<tr>
									<td class="modtitle" width="98%">
										<asp:HyperLink Text='<%# sChannelTitle %>' NavigateUrl='<%# sChannelLink %>' Runat="server" />
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<asp:DataGrid id="grdMain" Width="100%" CellPadding="3" CellSpacing="0" border="0"
								AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" 
								ShowHeader="false" EnableViewState="false" runat="server">
								<Columns>
									<asp:TemplateColumn>
										<ItemTemplate>
											<table cellpadding="0" cellspacing="2">
												<tr>
													<td class="itemtitle">
														<asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "title") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "link") %>' Target="_new" Runat="server" />
													</td>
												</tr>
												<tr><td class="itemdate"><%# DataBinder.Eval(Container.DataItem, "pubDate"    ) %></td></tr>
												<tr><td class="itemdesc"><%# DataBinder.Eval(Container.DataItem, "description") %></td></tr>
											</table>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:DataGrid>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br>
</div>
