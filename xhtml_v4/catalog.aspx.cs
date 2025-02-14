using System;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using KMobile.Catalog.Presentation;
using xhtml.Tools;

namespace xhtml
{
	public class catalog : XCatalogBrowsing
	{
		protected XhtmlTable tbTitle, tbCatalog, tbLinks, tbSearch, tbLinkImages, tbPages;
		protected XhtmlTableRow rowPreviews, rowPreviews2;
		private int first = 0, page, ms;
		protected string cg_temp;
		protected XhtmlTable tbHeader;
		protected XhtmlTable tbPreviews; 
		public string buscar, emocion, atras, up, fondo, back, title =  "Im&aacute;genes y Fondos", css = "xhtml.css";
 
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

				if (_mobile.MobileType != null &&  _mobile.IsCompatible("IMG_COLOR"))
				{
					_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;
					page = (Request.QueryString["n"] != null) ? Convert.ToInt32(Request.QueryString["n"]) : 1;
					ms = (Request.QueryString["ms"] != null && Request.QueryString["ms"] != "") ? Convert.ToInt32(Request.QueryString["ms"]) : 0;
					_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"].ToString() : ""; 
					_contentType = WapTools.GetDefaultContentType(_contentGroup);
					_displayKey = WapTools.GetXmlValue("DisplayKey"); 

					string paramBack = String.Format("a1=n&a2={0}&a3=cg&a4={1}&a5=cs&a6={2}",
						page, _contentGroup, _idContentSet);
				
					ContentSet contentSet = BrowseContentSetExtended();
  
					if (Request.QueryString["ms"]!=null && Request.QueryString["ms"]!="" && WapTools.GetText(Request.QueryString["ms"]) != "")
						title = WapTools.GetText(Request.QueryString["ms"]);                         

					int nbrows = Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_Rows"));
					int nbcols = (_contentGroup == "ANIM" || _mobile.ScreenPixelsWidth>140) ? 2 : 1;
                      
					if( _contentGroup == "COMPOSITE" )    
					{
						#region COMPOSITE
						try
						{
							for (int i=0; i<contentSet.Count - 1; i++)
								try
								{
									if (Convert.ToInt32(WapTools.FindProperty(contentSet.ContentCollection[i].PropertyCollection, "IDComposite")) > 0 && contentSet.ContentCollection[i].PropertyCollection["CompositeContentGroup"].Value.ToString() != "COMPOSITE")
									{
										first = Convert.ToInt32(WapTools.FindProperty(contentSet.ContentCollection[i].PropertyCollection, "IDComposite"));
										cg_temp = contentSet.ContentCollection[i].PropertyCollection["CompositeContentGroup"].Value.ToString();							
										break; 
									}
								}
								catch{}
						}
						catch{first = 0;}
						nbcols = Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_PreviewsComposite"));
						_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
						_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
						if (_idContentSet != 3619 && _idContentSet.ToString() != WapTools.GetXmlValue("Home/IMG") && _idContentSet.ToString() != WapTools.GetXmlValue("Home/ANIM") && _idContentSet.ToString() != WapTools.GetXmlValue("Home/VIDEO"))
							_contentSetDisplayInst.UrlDwld = String.Format("./catalog.aspx?cs={0}&cg={1}&d={2}&p={3}&t={4}&{5}", "{0}", "{1}",  Request.QueryString["d"], _idContentSet.ToString(), Server.UrlEncode(contentSet.Name), paramBack); 
						else
							_contentSetDisplayInst.UrlDwld = String.Format("./catalog.aspx?cs={0}&cg={1}&d={2}&{3}", "{0}", "{1}",  Request.QueryString["d"], paramBack); 
						if (Request.QueryString["ref"] != "" && Request.QueryString["ref"] != null) _contentSetDisplayInst.UrlDwld += "&ref=" + Request.QueryString["ref"];
						// PREVIEWS?
						try
						{
							if (first > 0 && _mobile.ScreenPixelsWidth>128)
							{
								ContentSet contentSetTemp = StaticCatalogService.GetContentsByRandomize( _displayKey, first, cg_temp, WapTools.GetDefaultContentType(cg_temp), _mobile.MobileType, Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_Previews")) + Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_Links")));
								DisplayContents(rowPreviews, rowPreviews2, contentSetTemp, contentSet.Name, paramBack);
								contentSetTemp = null;
							}
						}
						catch{}
						ReadContentSet(contentSet, tbCatalog, page, nbcols);
						page_max = (contentSet.Count / nbcols);
						if (contentSet.Count % nbcols > 0) page_max++;
						_contentSetDisplayInst = null;
						#endregion
					}
					else
					{
						#region CONTENTSET
						_imgDisplayInst = new ImgDisplayInstructions(_mobile);
						_imgDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
						_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", _contentGroup));
						//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("{0}|CONTENTSET|{1}|{2}", referer, contentSet.Name, page)), "", _idContentSet.ToString());
						_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1" || contentSet.Name.ToUpper().IndexOf("POSTAL")>=0) ? 1 : 0, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("xhtml|CONTENTSET|{0}|{1}", contentSet.Name, page)), "", _idContentSet.ToString());
						if (Request.QueryString["ms"]!= null && Request.QueryString["ms"]!="")
							_imgDisplayInst.UrlDwld += "&ms=" + Request.QueryString["ms"];
						if (Request["p"] != null && Request["t"] != null && Request["p"] != "" && Request["t"] != "")
							_imgDisplayInst.UrlDwld += "&p=" + Request.QueryString["p"] + "&t=" + Request.QueryString["t"];
						

            
						if (_mobile.IsXHTML) nbrows += 1;
						ReadContentSet(contentSet, tbCatalog, page, nbrows*nbcols);
						page_max = contentSet.Count / (nbrows * nbcols);
						if (contentSet.Count % (nbrows * nbcols) > 0) page_max++;
						if (WapTools.noPreview(contentSet.IDContentSet)) page_max=1;					
						_imgDisplayInst = null;
						#endregion
					}

					#region HEADER
					XhtmlImage img = new XhtmlImage();
					if (_idContentSet == 5745)
						img.ImageUrl = WapTools.GetImage(this.Request, "top",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 628)
						img.ImageUrl = WapTools.GetImage(this.Request, "novedades",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 3207 || _idContentSet == 5863)
						img.ImageUrl = WapTools.GetImage(this.Request, "animaciones",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 3965 || _idContentSet == 5862 )
						img.ImageUrl = WapTools.GetImage(this.Request, "videos",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 1159)
						img.ImageUrl = WapTools.GetImage(this.Request, "top",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 1160) 
						img.ImageUrl = WapTools.GetImage(this.Request, "novedades",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 3957)
						img.ImageUrl = WapTools.GetImage(this.Request, "top",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 3958)
						img.ImageUrl = WapTools.GetImage(this.Request, "novedades",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 5747)
						img.ImageUrl = WapTools.GetImage(this.Request, "amor",  _mobile.ScreenPixelsWidth);
					else if (_idContentSet == 5748)
						img.ImageUrl = WapTools.GetImage(this.Request, "modelos",  _mobile.ScreenPixelsWidth);
					else if (_contentGroup == "IMG" || cg_temp == "IMG")
					{
						if(ms == 6123)	img.ImageUrl = WapTools.GetImage(this.Request, "img_" + ms.ToString(),  _mobile.ScreenPixelsWidth);
						else	img.ImageUrl = WapTools.GetImage(this.Request, "categorias",  _mobile.ScreenPixelsWidth);	
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
						tbTitle.Controls.Add(row);  
					}
					else if (_contentGroup == "ANIM" || cg_temp == "ANIM")
					{
						if(ms == 6123)	img.ImageUrl = WapTools.GetImage(this.Request, "anim_" + ms.ToString(),  _mobile.ScreenPixelsWidth);
						else	img.ImageUrl = WapTools.GetImage(this.Request, "animaciones",  _mobile.ScreenPixelsWidth);
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
						tbTitle.Controls.Add(row);  
					}
					else if (_contentGroup == "VIDEO" || cg_temp == "VIDEO" || _contentGroup == "VIDEO_RGT" || cg_temp == "VIDEO_RGT" || _contentGroup == "")
					{
						if(ms == 6123)	img.ImageUrl = WapTools.GetImage(this.Request, "video_" + ms.ToString(),  _mobile.ScreenPixelsWidth);
						else	img.ImageUrl = WapTools.GetImage(this.Request, "videos",  _mobile.ScreenPixelsWidth);
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
						tbTitle.Controls.Add(row);  
					}
					else 
					{
						img.ImageUrl = WapTools.GetImage(this.Request, "imagenes",  _mobile.ScreenPixelsWidth);
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
						tbTitle.Controls.Add(row);  
					}
					XhtmlTools.AddImgTable(tbHeader, img);
					#endregion
                                     
					#region PAGES

					TableCell cellTemp = new TableCell();
					cellTemp.HorizontalAlign = HorizontalAlign.Center;
					XhtmlTableRow rowTemp = new XhtmlTableRow();
					int premiere = 0, derniere = 0, cont = 0;
					int[] limits = new int[page_max/3];
					while (cont<(page_max/3))
						limits[cont]=(++cont)*3;
					for (cont=0;cont<page_max/3;cont++)
						if (page<=limits[cont])
						{
							premiere=limits[cont]-2;
							derniere=limits[cont];
							break;
						}
					if (premiere==0 && derniere==0 && page>0)
					{
						derniere = page_max;
						if (limits.Length==0)
							premiere = 1; 
						else
							premiere = limits[cont-1]+1;
					}				
					string URL_Suivant = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&p={5}&t={6}&{7}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, derniere + 1, Request.QueryString["p"], Server.UrlEncode(Request.QueryString["t"]), paramBack);
					string URL_Precedent = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&p={5}&t={6}&{7}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, premiere - 1, Request.QueryString["p"], Server.UrlEncode(Request.QueryString["t"]), paramBack);
					if (derniere>1)
					{
						XhtmlLink link = new XhtmlLink();				
						if (premiere > 1)
						{
							//link.ImageUrl = WapTools.GetImage(this.Request, "Previous");
							link.Text = "Menos";
							link.NavigateUrl = URL_Precedent;
							cellTemp.Controls.Add(link);
						}
						else
							cellTemp.Text = "&nbsp;";
						rowTemp.Cells.Add(cellTemp);
						link = null;
						cellTemp = new TableCell();
						cellTemp.HorizontalAlign = HorizontalAlign.Center;

						for (cont=premiere;cont<=derniere;cont++)
						{
							if (cont!=page)
							{
								link = new XhtmlLink();
								link.CssClass = _contentGroup;
								link.NavigateUrl = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&p={5}&t={6}&{7}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, cont, Request.QueryString["p"], Server.UrlEncode(Request.QueryString["t"]), paramBack);
								link.Text = cont.ToString();
								cellTemp.Controls.Add(link);
							}
							else
							{
								cellTemp.ForeColor = Color.FromName(WapTools.GetText("Color_" +  _contentGroup));
								cellTemp.Text = cont.ToString();
							}
							rowTemp.Cells.Add(cellTemp);
							link = null;
							cellTemp = new TableCell();
							cellTemp.HorizontalAlign = HorizontalAlign.Center;
						}
			
						if (derniere < page_max)
						{
							link = new XhtmlLink();
							//link.ImageUrl = WapTools.GetImage(this.Request, "Next");
							link.Text = "Más";
							link.NavigateUrl = URL_Suivant;
							cellTemp.Controls.Add(link);
						}
						else
							cellTemp.Text = "&nbsp;";
						rowTemp.Cells.Add(cellTemp);
						link = null;
						tbPages.Rows.Add(rowTemp);
					}
					else
						tbPages.Visible = false;

					#endregion

					#region LINKS
					try
					{
						int isAlerta = WapTools.isAlerta(_idContentSet);
						if (isAlerta > 0)
							XhtmlTools.AddLinkTable("", tbLinks, "Alerta de " + WapTools.GetText("Alerta" + isAlerta.ToString()), "./linkto.aspx?id=" + (20 + isAlerta).ToString(), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						
						if (Request["p"] != null && Request["t"] != null && Request["p"] != "" && Request["t"] != "")
							XhtmlTools.AddLinkTable("", tbLinks, "Volver a " + Server.UrlDecode(Request.QueryString["t"]), "./catalog.aspx?cg=COMPOSITE&cs=" + Request.QueryString["p"], Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));							
						
						img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "descargate",  _mobile.ScreenPixelsWidth);
						
						if (Request.QueryString["ms"]!=null && Request.QueryString["ms"]!="")
						{
							string picto = "bullet";
							if (Request.QueryString["ms"] == "6123" ) {css = "halloween.css"; picto = "halloween";}
							XhtmlTools.AddLinkTable("", tbLinks, (WapTools.GetText(Request.QueryString["ms"]) != "") ? WapTools.GetText(Request.QueryString["ms"]): "Volver", "./minisite.aspx?id=" + Request.QueryString["ms"], Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, picto));							
						}
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/POSTALES"))
						{
							XhtmlTools.AddImgTable(tbLinks, img);
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/IMG"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/VIDEO"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("ANIM_COLOR"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/ANIM"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (WapTools.isCompatibleThemes(_mobile))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						}
						else if (Request.QueryString["d"] == "1")
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasPOSTALES"), String.Format("./catalog.aspx?d=1&cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/POSTALES"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/IMG") || _idContentSet.ToString() == "3619")
						{
							XhtmlTools.AddImgTable(tbLinks, img);
							//XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("POSTALES"), String.Format("./catalog.aspx?d=1&cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/POSTALES"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/VIDEO"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("ANIM_COLOR"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/ANIM"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (WapTools.isCompatibleThemes(_mobile))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						}
						else if (_contentGroup == "IMG" || cg_temp == "IMG")
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasIMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/IMG"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/ANIM"))
						{
							XhtmlTools.AddImgTable(tbLinks, img);
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/IMG"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							//XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("POSTALES"), String.Format("./catalog.aspx?d=1&cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/POSTALES"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/VIDEO"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (WapTools.isCompatibleThemes(_mobile))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						}
						else if (_contentGroup == "ANIM" || cg_temp == "ANIM")
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/ANIM"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/VIDEO"))
						{
							XhtmlTools.AddImgTable(tbLinks, img);
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/IMG"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							//XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("POSTALES"), String.Format("./catalog.aspx?d=1&cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/POSTALES"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("ANIM_COLOR"))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/ANIM"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (WapTools.isCompatibleThemes(_mobile))
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						}					
						else if (_contentGroup == "" || _contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT" || cg_temp == "VIDEO" || cg_temp == "VIDEO_RGT")
							XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("MasVIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/VIDEO"), paramBack), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));					
					}
					catch{} 
					#endregion

					contentSet = null;
				}
				else
				{
					tbCatalog.Visible = false;
					tbLinkImages.Visible = false;					
					tbPages.Visible = false;
					tbPreviews.Visible = false;
					tbSearch.Visible = false;
					tbTitle.Visible = false;
					XhtmlTableRow row = new XhtmlTableRow();
					XhtmlTools.AddTextTableRow(row, WapTools.GetText("Compatibility2"), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XSmall);
					tbLinks.Rows.Add(row);
					row = null;
				}

				#region PICTOS
				if (ms == 6123)
				{
					fondo = WapTools.GetImage(this.Request, "hfondo", _mobile.ScreenPixelsWidth);
					buscar = WapTools.getPicto(this.Request, "hbuscar", _mobile);
					emocion = WapTools.getPicto(this.Request, "hemocion", _mobile);
					back = WapTools.getPicto(this.Request, "hback", _mobile);
					up = WapTools.getPicto(this.Request, "hup", _mobile);						
				}
				else
				{
					fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
					buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
					emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
					back = WapTools.getPicto(this.Request, "back", _mobile);
					up = WapTools.getPicto(this.Request, "up", _mobile);
				}
				_mobile = null;
				#endregion

				// Search
				//XhtmlTools.AddLinkTable("", tbSearch, WapTools.GetText("SearchLink"), String.Format("./search.aspx?cg={0}", contentGroupDisplay), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, "pict:///core/action/find");
				atras = WapTools.UpdateFooter(_mobile, this.Context, null); 				
			}
			catch(Exception caught) 
			{
				WapTools.SendMail("catalog", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\catalog.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
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

		#region Display
		public void DisplayContents(TableRow row, TableRow row2, ContentSet contentset, string name, string paramBack)
		{
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", contentset.ContentGroup));
			_imgDisplayInst.TextDwld = WapTools.GetText("Download");
			_imgDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "download");
			//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, contentset.ContentGroup, WapTools.GetDefaultContentType(contentset.ContentGroup), HttpUtility.UrlEncode(String.Format("{0}|HOME_{1}",referer, name)), "", _idContentSet.ToString());
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1") ? 1 : 0, contentset.ContentGroup, WapTools.GetDefaultContentType(contentset.ContentGroup), HttpUtility.UrlEncode(String.Format("xhtml|HOME_{0}", name)), "", _idContentSet.ToString()) + "&p=" + _idContentSet + "&t=" + Server.UrlEncode(name);
                   
			TableItemStyle tableStyle = new TableItemStyle();
			try
			{
				tableStyle.HorizontalAlign = HorizontalAlign.Center; 
				int previews = (_mobile.ScreenPixelsWidth < 140 && contentset.ContentGroup != "ANIM") ? 1 : 2;            
				for( int i = DateTime.Now.Day; i < DateTime.Now.Day + previews; i++ )
				{
					Content content = contentset.ContentCollection[ i % contentset.Count];
					if (content != null)
					{
						XhtmlTableCell tempCell = new XhtmlTableCell();
						//XhtmlTableCell textCell = new XhtmlTableCell();
						ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
						//imgDisplay.Display(tempCell, textCell, content);
						imgDisplay.Display(tempCell, content);
						imgDisplay = null;
						tempCell.ApplyStyle(tableStyle);
						//textCell.ApplyStyle(tableStyle);
						row.Cells.Add(tempCell); 
						//rowTexts.Cells.Add(textCell);		
						tempCell = null;
					} 
					content = null;
				}
			}
			catch{}
			try
			{
				tableStyle.HorizontalAlign = HorizontalAlign.Center; 
				int previews = (_mobile.ScreenPixelsWidth < 140 && contentset.ContentGroup != "ANIM") ? 1 : 2;            
				for( int i = DateTime.Now.Day + 2; i < DateTime.Now.Day + 2 + previews; i++ )
				{
					Content content = contentset.ContentCollection[ i % contentset.Count];
					if (content != null)
					{
						XhtmlTableCell tempCell = new XhtmlTableCell();
						//XhtmlTableCell textCell = new XhtmlTableCell();
						ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
						//imgDisplay.Display(tempCell, textCell, content);
						imgDisplay.Display(tempCell, content);
						imgDisplay = null;
						tempCell.ApplyStyle(tableStyle);
						//textCell.ApplyStyle(tableStyle);
						row2.Cells.Add(tempCell); 
						//rowTexts.Cells.Add(textCell);		
						tempCell = null;
					} 
					content = null;
				}
			}
			catch{}
		}


		public void DisplayContents(string cg, Table t, ContentSet contentset, string name, string paramBack)
		{
			TableRow row = new TableRow();
			ContentSet newContentset = new ContentSet();
			newContentset.ContentCollection = new ContentCollection();
			foreach (Content c in contentset.ContentCollection)
				if (c.ContentGroup.Name == cg)
					newContentset.ContentCollection.Add(c);
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", cg));
			_imgDisplayInst.TextDwld = WapTools.GetText("Download");
			_imgDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "download");
			//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, contentset.ContentGroup, WapTools.GetDefaultContentType(contentset.ContentGroup), HttpUtility.UrlEncode(String.Format("{0}|HOME_{1}",referer, name)), "", _idContentSet.ToString());
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1") ? 1 : 0, cg, WapTools.GetDefaultContentType(cg), HttpUtility.UrlEncode(String.Format("xhtml|HOME_{0}", name)), "", _idContentSet.ToString());
                           
			TableItemStyle tableStyle = new TableItemStyle();
			try
			{
				int start = DateTime.Now.Millisecond;
				tableStyle.HorizontalAlign = HorizontalAlign.Center; 
				int previews = (_mobile.ScreenPixelsWidth < 140 && newContentset.ContentGroup != "ANIM") ? 1 : 2;            
				for( int i = start; i < start + previews; i++ )
				{
					Content content = newContentset.ContentCollection[ i % newContentset.Count];
					if (content != null)
					{
						XhtmlTableCell tempCell = new XhtmlTableCell();
						//XhtmlTableCell textCell = new XhtmlTableCell();
						ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
						//imgDisplay.Display(tempCell, textCell, content);
						imgDisplay.Display(tempCell, content);
						imgDisplay = null;
						tempCell.ApplyStyle(tableStyle);
						//textCell.ApplyStyle(tableStyle);
						row.Cells.Add(tempCell); 
						//rowTexts.Cells.Add(textCell);		
						tempCell = null;
					} 
					content = null;
				}
				t.Rows.Add(row);
				row = null;
			}
			catch{}
		}
		#endregion
	}
}
