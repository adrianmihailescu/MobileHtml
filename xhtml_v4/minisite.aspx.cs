using System;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class minisite : XCatalogBrowsing  
	{
		private ArrayList _contentCollImg = new ArrayList();
		private ArrayList _contentCollAnim = new ArrayList();
		private ArrayList _contentCollVideo = new ArrayList();
		private ArrayList _contentCollContentSet = new ArrayList();
		private ArrayList _fondoCollGenera = new ArrayList();
		private ArrayList _tonosCollGenera = new ArrayList();
		protected XhtmlTable tbLinks, tbEspecial, tbPostales, tbAhora2, tbEnd3, tbPub, tbCanales, tbTopNews, tbImages, tbShops, tbHeader, tbCategorias, tbLinkImage, tbLinkImage2, tbEnd, tbTop, tbNew, tbTitleShop, tbAhora;  
		protected XhtmlTableRow rowPostales, rowEspecial, rowImg, rowTitlesImg, rowAnims, rowVideos, rowTop, rowNew, rowShop1, rowShop2, rowShop3, rowShop4, rowShop5, rowmoreshops, rowTitleShops, rowEspecial2, rowEspecial3;
		protected XhtmlTableCell cellLink, cellImg, cellLink2, cellImg2, cellLink3, cellImg3;
		protected XhtmlTable tbHeader3, tbHeader2, tbHeaderAhora, tbHeaderDestacados, tbTemas, tbVideos;
		int init = 0; 
		public string buscar, emocion, back, up, fondo, musica, title = "Im&aacute;genes y Fondos", css = "xhtml.css", picto = "bullet";
		protected xhtml.Tools.XhtmlTable tbAnims;  
	 
		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try
			{
				_mobile = (MobileCaps)Request.Browser;
 
				try
				{
					WapTools.SetHeader(this.Context);
					//WapTools.LogUser(this.Request, 100, _mobile.MobileType);
					WapTools.AddUIDatLog(Request, Response);
				} 
				catch{}
	
				if (_mobile.MobileType != null)   
				{	 
					_displayKey = WapTools.GetXmlValue("DisplayKey");
					_idContentSet = Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : Convert.ToInt32(WapTools.GetXmlValue("Home/Composite"));
					BrowseContentSetExtended(null, -1, -1); 

					if( _mobile.IsCompatible("IMG_COLOR") )  
					{ 
						if (WapTools.GetText(_idContentSet.ToString()) != "")
							title = WapTools.GetText(_idContentSet.ToString() );

						#region HEADER
						XhtmlImage img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, _idContentSet.ToString(),  _mobile.ScreenPixelsWidth);
						if (img.ImageUrl != "")
							XhtmlTools.AddImgTable(tbHeader, img, 1);
						else
							tbHeader.Visible = false;
						if (_idContentSet == 6123) 
						{
							InitializeFondonombresGenera(1, 7);
							InitializeTonosGenera(1, 10);
							css = "halloween.css"; picto = "halloween";
						}
						#endregion
						
						#region IMAGENES
 
						img = new XhtmlImage();
						if(_idContentSet != 6123)	img.ImageUrl = WapTools.GetImage(this.Request, "img_" + _idContentSet.ToString(),  _mobile.ScreenPixelsWidth);
						else img.ImageUrl = "" ;
						if (img.ImageUrl != "")
							XhtmlTools.AddImgTable(tbHeaderDestacados, img, 1);
						else
							img = null;
						try 
						{ 
							init = DateTime.Now.Millisecond;							
							if (_contentCollImg.Count > 0)
							{
								DisplayImages(rowImg, "IMG", "IMG_COLOR", init, _contentCollImg.Count, 0);
								if (_mobile.IsXHTML)
									DisplayImages(rowTitlesImg, "IMG", "IMG_COLOR", init+2, _contentCollImg.Count, 0);
							}
						
						}
						catch{}

						if (_contentCollContentSet.Count > 0)
						{
							try
							{
								DisplayContentSets(tbTop, "IMG", 0, -1, _idContentSet.ToString() );
								if(_idContentSet == 6123)
								{
									DisplayFondonombresGenera(tbTop, init);
									XhtmlTools.AddLinkTable("", tbTop, WapTools.GetText("FondoNombresH"), "./linkto.aspx?cg=COMPOSITE&id=10036", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, picto));
									img = new XhtmlImage();
									img.ImageUrl = WapTools.GetImage(this.Request, "sound_6123",  _mobile.ScreenPixelsWidth);
									if (img.ImageUrl != "")
										XhtmlTools.AddImgTable(tbTop, img, 2);
									DisplayTonosGenera(tbTop, init);
									XhtmlTools.AddLinkTable("", tbTop, WapTools.GetText("TonosH"), "./linkto.aspx?cg=COMPOSITE&id=10038", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, picto));
									XhtmlTools.AddLinkTable("", tbTop, WapTools.GetText("TonoMensajesH"), "./linkto.aspx?cg=COMPOSITE&id=10037", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, picto));									
								}
							}
							catch{}
						}
						if (_idContentSet == 6032)
							XhtmlTools.AddLinkTable("", tbTop, "Más Fondos", "./catalog.aspx?cg=IMG&cs=6032", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));								
						else if (_idContentSet == 6178)
							XhtmlTools.AddLinkTable("", tbTop, "Más Dibujos Animados", "./catalog.aspx?cg=COMPOSITE&cs=3144&ms=6178", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));								
						#endregion

						#region ANIMS DESTACADAS
						if (_contentCollAnim.Count > 0 && _mobile.IsCompatible("ANIM_COLOR"))
						{
							img = new XhtmlImage();
							img.ImageUrl = WapTools.GetImage(this.Request, "anim_" + _idContentSet.ToString(),  _mobile.ScreenPixelsWidth);
							if (img.ImageUrl != "")
								XhtmlTools.AddImgTable(tbHeader2, img, 1);
							try 
							{ 
								init = DateTime.Now.Millisecond;							
								DisplayImages(rowAnims, "ANIM", "ANIM_COLOR", init, _contentCollImg.Count, 0);
							}
							catch{}
						}  
						else
						{
							tbAnims.Visible = false;
							tbHeader2.Visible = false;
						}

						if (_contentCollContentSet.Count > 0)
						{
							try{DisplayContentSets(tbAnims, "ANIM", 0, -1, _idContentSet.ToString() );}
							catch{}  
						}
						#endregion

						#region VIDEOS DESTACADAS
						if (_contentCollVideo.Count > 0 && _mobile.IsCompatible("VIDEO_CLIP"))
						{
							img = new XhtmlImage();
							img.ImageUrl = WapTools.GetImage(this.Request, "video_" + _idContentSet.ToString(),  _mobile.ScreenPixelsWidth);
							if (img.ImageUrl != "")
								XhtmlTools.AddImgTable(tbHeader3, img, 1);
							if (_idContentSet == 6190)
							{
								try 
								{ 
									init = DateTime.Now.Millisecond;							
									DisplayImages(rowVideos, "VIDEO_RGT", "VIDEO_CLIP", init, _contentCollImg.Count, 0);
								}
								catch{}
							}

							if (_contentCollContentSet.Count > 0)
							{
								try
								{
									DisplayContentSets(tbVideos, "VIDEO_RGT", 0, -1, _idContentSet.ToString() );
									XhtmlTools.AddLinkTable("", tbVideos, "Juegos de Halloween", "http://www.iwapserver.com/ms/halloween", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.NotSet, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, picto));
								}
								catch{}
							}						
						}
						else
						{
							tbVideos.Visible = false;
							tbHeader3.Visible = false;
						}
						#endregion

						#region LINKS
						try
						{
							if (_idContentSet == 6032)
							{
								XhtmlImage header = new XhtmlImage();
								header.ImageUrl = WapTools.GetImage(this.Request, "dedicatorias",  _mobile.ScreenPixelsWidth);
								XhtmlTools.AddImgTable(tbLinks, header, 2);
								XhtmlTableRow row = new XhtmlTableRow();
								XhtmlTableCell cell = new XhtmlTableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								XhtmlLink lnk = new XhtmlLink();
								lnk.ImageUrl = WapTools.GetText("LnkGenera10027");
								lnk.NavigateUrl = WapTools.GetText("LnkGenera10028");
								cell.Controls.Add(lnk);
								row.Cells.Add(cell);
								if (_mobile.ScreenPixelsWidth>=140)
								{
									cell = new XhtmlTableCell();
									cell.HorizontalAlign = HorizontalAlign.Center;
									lnk = new XhtmlLink();
									lnk.ImageUrl = WapTools.GetText("LnkGenera10029");
									lnk.NavigateUrl = WapTools.GetText("LnkGenera10030");
									cell.Controls.Add(lnk);
									row.Cells.Add(cell);
								}
								tbLinks.Rows.Add(row);								
								XhtmlTools.AddLinkTable("", tbLinks, "Más Fondodedicatorias", String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								header = new XhtmlImage();
								header.ImageUrl = WapTools.GetImage(this.Request, "fondonombres",  _mobile.ScreenPixelsWidth);
								XhtmlTools.AddImgTable(tbLinks, header, 2);
								row = new XhtmlTableRow();
								cell = new XhtmlTableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								lnk = new XhtmlLink();
								lnk.ImageUrl = WapTools.GetText("LnkGenera10023");
								lnk.NavigateUrl = WapTools.GetText("LnkGenera10024");
								cell.Controls.Add(lnk);
								row.Cells.Add(cell);
								if (_mobile.ScreenPixelsWidth>=140)
								{
									cell = new XhtmlTableCell();
									cell.HorizontalAlign = HorizontalAlign.Center;
									lnk = new XhtmlLink();
									lnk.ImageUrl = WapTools.GetText("LnkGenera10025");
									lnk.NavigateUrl = WapTools.GetText("LnkGenera10026");
									cell.Controls.Add(lnk);
									row.Cells.Add(cell);
								}
								tbLinks.Rows.Add(row);
								XhtmlTools.AddLinkTable("", tbLinks, "Más Fondonombres", String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							}
							if (_idContentSet == 6032 || _idContentSet == 6178 || _idContentSet == 6190)
							{
								string paramBack = "";
								img = new XhtmlImage();
								img.ImageUrl = WapTools.GetImage(this.Request, "descargate",  _mobile.ScreenPixelsWidth);
								XhtmlTools.AddImgTable(tbLinks, img, 2);
								if (_idContentSet != 6190)
									XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/VIDEO"), paramBack), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								else
								{
									XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("IMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/IMG"), paramBack), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								}
								if (_mobile.IsCompatible("ANIM_COLOR"))
									XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("ANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}&{1}", WapTools.GetXmlValue("Home/ANIM"), paramBack), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								if (WapTools.isCompatibleThemes(_mobile))
									XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
								if (_mobile.IsCompatible("VIDEO_DWL")) XhtmlTools.AddLinkTable("", tbLinks, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							}
						}
						catch{} 
						#endregion

						#region PICTOS
						musica = (_mobile.ScreenPixelsWidth > 128) ? "M&uacute;sica y Tonos" : "M&uacute;sica";
						
					if (_idContentSet == 6123)
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
						#endregion 
					}
				}
			}
			catch(Exception caught) 
			{
				WapTools.SendMail("minisite", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\minisite.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				Response.Redirect("./error.aspx");				
			}
			finally
			{
				_contentCollImg = null;
				_contentCollAnim = null;
				_contentCollContentSet = null;
				_fondoCollGenera = null;
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
		
		#region Override
		protected override void DisplayContentSet(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			_contentCollContentSet.Add(content);
		}

		protected override void DisplayImg(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			if (content.ContentGroup.Name == "IMG") _contentCollImg.Add(content);
			else if (content.ContentGroup.Name == "ANIM") _contentCollAnim.Add(content);
			else _contentCollVideo.Add(content);
		}
		#endregion

		#region Display
		public void DisplayImages(TableRow row, string contentGroup, string contentType, int start, int count, int drm)
		{
			
			Content content = null;
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", contentGroup));
			_imgDisplayInst.TextDwld = WapTools.GetText("Download");
			_imgDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "Img");
			//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, contentGroup, contentType, HttpUtility.UrlEncode("xHOME"), "", "0");
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, drm, contentGroup, contentType, HttpUtility.UrlEncode("xhtml|HOME_" + WapTools.GetText(_idContentSet.ToString())), "", "0");
			_imgDisplayInst.UrlDwld += "&ms=" + Request.QueryString["id"];

			TableItemStyle tableStyle = new TableItemStyle();
			tableStyle.HorizontalAlign = HorizontalAlign.Center;
			int previews = (_mobile.ScreenPixelsWidth < 140) ? 1 : 2;
                     
			for( int i = start; i < start + previews; i++ )
			{
				if (contentGroup == "IMG" && drm==0)
					content = (Content)_contentCollImg[ (i) % _contentCollImg.Count];
				else if (contentGroup == "ANIM")
					content = (Content)_contentCollAnim[ (i) % _contentCollAnim.Count];
				else if (contentGroup == "VIDEO" || contentGroup == "VIDEO_RGT")
					content = (Content)_contentCollVideo[ (i) % _contentCollVideo.Count];

				if (content != null)
				{
					XhtmlTableCell tempCell = new XhtmlTableCell();
					ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
					imgDisplay.Display(tempCell, content);
					imgDisplay = null;
					tempCell.ApplyStyle(tableStyle);
					row.Cells.Add(tempCell);
					tempCell = null;
				}
			}  
			content = null;
			_imgDisplayInst = null;
			tableStyle = null;
		}                     

		public void DisplayContentSets(XhtmlTable t, string cg, int rangeInf, int rangeSup, string ms)
		{
			_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
			_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, picto);
			_contentSetDisplayInst.UrlDwld = "./catalog.aspx?ms=" + ms + "&cs={0}&cg={1}";
			//_contentSetDisplayInst.UrlDwld = "./linkto.aspx?id={0}&cg={1}";
			//_contentSetDisplayInst.UrlDwld = "./catalog.aspx?cg=IMG&ct=IMG_COLOR&cs={0}";
			XhtmlTableCell cell = new XhtmlTableCell();
			cell.ColumnSpan = 2;
			XhtmlTableRow row = new XhtmlTableRow();
			if (rangeSup == -1) rangeSup = _contentCollContentSet.Count;
			if (rangeSup > _contentCollContentSet.Count) rangeSup = _contentCollContentSet.Count;
			for( int i = rangeInf; i < rangeSup; i++ )
			{
				Content content = (Content)_contentCollContentSet[i];
				if (WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup") != cg && cg == "IMG" && WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup")=="COMPOSITE") ;
				else if (WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup") != cg) continue;
				ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
				contentSetDisplay.Display(cell, content, true);			
				contentSetDisplay = null;
				row.Controls.Add(cell);
				cell = new XhtmlTableCell();
				cell.ColumnSpan = 2;
				t.Controls.Add(row);
				row = new XhtmlTableRow();
				content = null;
			}
			//t.Controls.Add(row);
			_contentSetDisplayInst = null;
			cell = null; row = null;
		}

		public void InitializeFondonombresGenera(int rangeInf, int rangeSup)
		{
			for( int i = rangeInf; i <= rangeSup; i++ )
			{
				XhtmlLink lnk = new XhtmlLink();
				lnk.ImageUrl = "http://www.marketingmovil.com/portales/movistar/wap/magic/imgPre/" + WapTools.GetText("FondoGenera" + i.ToString());
				lnk.NavigateUrl = "http://www.marketingmovil.com:8080/portal/img/ServletAction?idaccion=2160&" + WapTools.GetText("LnkFondoGenera" + i.ToString()) + "&origen=2158";
				_fondoCollGenera.Add(lnk);
				lnk = null;
			}
		}

		public void InitializeTonosGenera(int rangeInf, int rangeSup)
		{
			for( int i = rangeInf; i <= rangeSup; i++ )
			{
				XhtmlLink lnk = new XhtmlLink();
				lnk.Text = WapTools.GetText("TonoGenera" + i.ToString());
				lnk.NavigateUrl = "http://www.marketingmovil.com:8080/portal/ZPTonosHumorRealAction?idaccion=3102&" + WapTools.GetText("LnkTonoGenera" + i.ToString()) + "&origen=3101";
				_tonosCollGenera.Add(lnk);
				lnk = null;
			}
		}

		public void DisplayFondonombresGenera(XhtmlTable tb, int start)
		{
			XhtmlTableRow row = new XhtmlTableRow();
			int previews = (_mobile.ScreenPixelsWidth > 140) ? 2 : 1;
			for( int i = start; i < start + previews; i++ )
			{
				XhtmlLink lnk = (XhtmlLink) _fondoCollGenera[i % _fondoCollGenera.Count];
				if(lnk != null)
				{
					XhtmlTableCell cell = new XhtmlTableCell(); 
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Controls.Add(lnk);
					row.Cells.Add(cell);
					lnk = null;
					cell = null;
				}
			}
			tb.Rows.Add(row);
		}

		public void DisplayTonosGenera(XhtmlTable tb, int start)
		{
			XhtmlTableRow row = new XhtmlTableRow();
			for( int i = start; i < start + 2; i++ )
			{
				XhtmlLink lnk = (XhtmlLink) _tonosCollGenera[i % _tonosCollGenera.Count];
				if(lnk != null)
				{
					XhtmlTableCell cell = new XhtmlTableCell(); 
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Controls.Add(lnk);
					row.Cells.Add(cell);
					lnk = null;
					cell = null;
				}
			}
			tb.Rows.Add(row);
		}
		#endregion
	}
}
