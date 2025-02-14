using System;
using System.Drawing;
using System.Web.UI.WebControls;
using AGInteractive.Business;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using xhtml_v3.Tools;

namespace xhtml
{
	public class perso : XCatalogBrowsing
	{
		protected System.Web.UI.WebControls.Panel pnlFooter;
		protected XhtmlTableRow rowTitle, rowTitle2, rowText, rowContents, rowIMG, rowANIM, rowVIDEO;
		protected xhtml_v3.Tools.XhtmlTable tbPerso, tbPreviews, tbContents, tbLinks;
		protected CommandCollection comandas, cc;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try             
			{
				_mobile = (MobileCaps)Request.Browser;
				try
				{
					WapTools.SetHeader(this.Context);
					WapTools.AddUIDatLog(Request, Response);
				}
				catch{}

				_displayKey = WapTools.GetXmlValue("DisplayKey");
				_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"] : "";
				_contentType = (_contentGroup == "") ? "" : WapTools.GetDefaultContentType(_contentGroup);
				int g_lngNumPage = (Request.QueryString["n"] != null) ? Convert.ToInt32(Request.QueryString["n"]) : 0;
				XhtmlTools.AddTextTableRow("IMG", rowTitle, "", Server.HtmlEncode(WapTools.GetText("zp")), Color.FromArgb(102, 153, 204), Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);					
							
				if (_contentGroup == "")
					XhtmlTools.AddTextTableRow("", rowTitle2, "", WapTools.GetText("zonaPerso"), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall);
				else
				{
					XhtmlTools.AddTextTableRow("", rowTitle2, WapTools.GetImage(this.Request, "bullet"), WapTools.GetText(_contentGroup), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);
					try
					{
						Customer cst = new Customer(this.Request);
						cc = cst.LoadCommands(CommandStatus.Delivered, DateTime.Now.AddDays(-1), DateTime.Now, new Guid(_displayKey), _mobile.MobileType, _contentType);				
						if (_contentGroup == "VIDEO")
						{
							CommandCollection cc2 = cst.LoadCommands(CommandStatus.Delivered, DateTime.Now.AddDays(-1), DateTime.Now, new Guid(_displayKey), _mobile.MobileType, "VIDEO_CLIP");					
							if (cc2 != null && cc2.Count>0)
								foreach (AGInteractive.Business.Command c in cc2)
									cc.Add(c);
							cc2 = null;
						}

						comandas = new CommandCollection();
						foreach (AGInteractive.Business.Command com in cc)
							if (com.Item.Referer != "perso") comandas.Add(com);
						cc = null;
					}
					catch{}

					if (comandas != null && comandas.Count>0)
					{
						int g_lngContentPerPage = 3;
						_imgDisplayInst = new ImgDisplayInstructions(_mobile);
						_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", _contentGroup));
						_imgDisplayInst.TextDwld = WapTools.GetText("Download");
						_imgDisplayInst.UrlDwld = WapTools.GetUrlBillingFree(this.Request, _contentGroup, _contentType, "", "", "0");

						int l_lngItem = g_lngNumPage * g_lngContentPerPage;
						if (l_lngItem > comandas.Count)
						{
							l_lngItem = 0;
							g_lngNumPage = 0;
						}
						Trace.Warn(comandas.Count.ToString());
						for (int i = l_lngItem;  i<= l_lngItem + g_lngContentPerPage - 1; i++)
						{
							Trace.Write(i.ToString());
							if (i>=comandas.Count) break;
							_idContent = comandas[i].Item.ContentId;
							Content c = BrowseContent();
							XhtmlTableRow r = new XhtmlTableRow();
							XhtmlTableRow r2 = new XhtmlTableRow();
							XhtmlTableCell cell = new XhtmlTableCell();
							XhtmlTableCell cellTitle = new XhtmlTableCell();
							cell.HorizontalAlign = HorizontalAlign.Center;
							cellTitle.HorizontalAlign = HorizontalAlign.Center;
							DisplayContent(c, cell, cellTitle);
							r.Cells.Add(cell);
							r2.Cells.Add(cellTitle);
							tbPreviews.Rows.Add(r2);
							tbPreviews.Rows.Add(r);
						}	
						comandas = null;
						if (l_lngItem + g_lngContentPerPage - 1 <  comandas.Count)				
							XhtmlTools.AddLinkTable("", tbPreviews, WapTools.GetText("next"), String.Format("./perso.aspx?cg={0}&n={1}", _contentGroup, g_lngNumPage+1), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request,  "bullet"));			 
						if (g_lngNumPage>0)
							XhtmlTools.AddLinkTable("", tbPreviews, WapTools.GetText("prev"), String.Format("./perso.aspx?cg={0}&n={1}", _contentGroup, g_lngNumPage-1), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request,  "bullet"));			 
					}
					else
					{
						switch(_contentType)
						{
							case "IMG_COLOR": 	
								XhtmlTools.AddTextTableRow(rowText, String.Format(WapTools.GetText("noImagen"), _mobile.MobileType), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall);
								break;
							case "ANIM_COLOR":
								XhtmlTools.AddTextTableRow(rowText, String.Format(WapTools.GetText("noAnim"), _mobile.MobileType), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall);
								break;
							case "VIDEO_DWL":
								XhtmlTools.AddTextTableRow(rowText, String.Format(WapTools.GetText("noVideo"), _mobile.MobileType), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall);
								break;							
						}
					}
				}
				XhtmlTools.AddTextTableRow(rowContents, WapTools.GetText("contents"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
				
				if (_mobile.IsCompatible("VIDEO_DWL") &&  (_contentGroup != "VIDEO"))
					XhtmlTools.AddLinkTableRow("", rowVIDEO, WapTools.GetText("VIDEO"), "./perso.aspx?cg=VIDEO", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				else
					rowVIDEO.Visible = false;
				if (_mobile.IsCompatible("ANIM_COLOR") &&  (_contentGroup != "ANIM"))
					XhtmlTools.AddLinkTableRow("", rowANIM, WapTools.GetText("ANIM"), "./perso.aspx?cg=ANIM", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				else
					rowANIM.Visible = false;
				if (_mobile.IsCompatible("IMG_COLOR") &&  (_contentGroup != "IMG"))
					XhtmlTools.AddLinkTableRow("", rowIMG, WapTools.GetText("IMG"), "./perso.aspx?cg=IMG", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				else
					rowIMG.Visible = false;

				// Links
				try
				{
					if (_contentGroup == "IMG" || _contentGroup == "")
					{
						if (_idContentSet.ToString() != WapTools.GetXmlValue("Home/IMG"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasIMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("ANIM_COLOR"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("VIDEO_DWL"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
					}
					else if (_contentGroup == "ANIM")
					{
						if (_idContentSet.ToString() != WapTools.GetXmlValue("Home/ANIM"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("VIDEO_DWL"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
					}
					else if (_contentGroup == "VIDEO")
					{
						if (_idContentSet.ToString() != WapTools.GetXmlValue("Home/VIDEO"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasVIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("ANIM_COLOR"))
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
					}
				}
				catch{} 

			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\perso.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				Response.Redirect("./error.aspx");		
			}
		}

		#region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
