using System;
using System.Collections;
using System.Configuration; 
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using xhtml.Tools;
namespace xhtml
{	
	public class _default : XCatalogBrowsing  
	{
		private ArrayList _contentCollImg = new ArrayList(); 
		private ArrayList _contentCollAnim = new ArrayList();
		private ArrayList _contentCollContentSet = new ArrayList();  
		protected XhtmlTable tbEspecial, tbPostales, tbAhora2, tbEnd3, tbPub, tbCanales, tbTopNews, tbImages, tbShops, tbHeader, tbCategorias, tbLinkImage, tbLinkImage2, tbEnd, tbTop, tbNew, tbTitleShop, tbAhora;  
		protected XhtmlTableRow rowPostales, rowEspecial, rowImg, rowTitlesImg, rowPub, rowTop, rowNew, rowmoreshops, rowTitleShops, rowEspecial2, rowEspecial3;
		protected XhtmlTableCell cellLink, cellImg, cellLink2, cellImg2, cellLink3, cellImg3, cellShop1, cellShop2, cellShop3, cellShop4, cellShop5, cellShop6;
		protected XhtmlTable tbTitleCanales, tbHeaderAhora, tbHeaderDestacados, tbHeaderEnd, tbHeaderEnd2, tbEnd2, tbHeaderApuntante;
		int init = 0; 
		public string buscar, emocion, back, up, fondo, search, musica;  
	 
		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try
			{
				_mobile = (MobileCaps)Request.Browser;
 
				try
				{
					//search = ConfigurationSettings.AppSettings["UrlSearchTest"];
					search = ConfigurationSettings.AppSettings["UrlSearch"];
					WapTools.SetHeader(this.Context);
					WapTools.LogUser(this.Request, 100, _mobile.MobileType);
					WapTools.AddUIDatLog(Request, Response);
				} 
				catch{} 
	
				if (_mobile.MobileType != null) 
				{	 
					_displayKey = WapTools.GetXmlValue("DisplayKey");
					_idContentSet = Convert.ToInt32(WapTools.GetXmlValue("Home/Composite"));
					BrowseContentSetExtended(null, -1, -1); 

					if( _mobile.IsCompatible("IMG_COLOR") )  
					{ 
						#region HEADER
						XhtmlImage img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "imagenes",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbHeader, img);
						#endregion
  
						#region PUB
						WapTools.CallPub(this.Request, WapTools.SearchType.ts_ImagenesFondos_banner_top.ToString(), "xml", tbPub); 
						#endregion  
   
						#region AHORA EN IMAGENES - ESPECIAL 
						//Content content = (Content)_contentCollContentSet[0];
						img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "ahora",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbHeaderAhora, img);
 
						int dia = WapTools.isTestSite(this.Request) ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;
						//dia = 1; 
						Especial esp = new Especial();
						
						try  
						{						
							esp = WapTools.getEspecial(1, dia.ToString());
							while (esp.name == "" || !_mobile.IsCompatible(esp.filter))
								esp = WapTools.getEspecial(1, (--dia).ToString());
							XhtmlTools.AddLinkTableCell("img", cellLink, WapTools.GetText(esp.name), WapTools.GetText("Link" + esp.name), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
							XhtmlImage picto = new XhtmlImage();
							//picto.ImageUrl = WapTools.GetImage(this.Request, especial);
							picto.ImageUrl = String.Format("{0}/Images/{1}", this.Request.ApplicationPath, esp.name.ToLower() + ".gif");							
							cellImg.Controls.Add(picto); 
							cellImg.HorizontalAlign = HorizontalAlign.Center; 
							picto = null;   
						}
						catch{rowEspecial.Visible = false;} 
						try 
						{
							dia = WapTools.isTestSite(this.Request) ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;
							//dia = 2;
							esp = WapTools.getEspecial(2, dia.ToString());
							if(esp.name == "" || !_mobile.IsCompatible(esp.filter))
							{
								esp = WapTools.getCompatibleEspecial(esp);
							}
							
						/*	while (esp.name == "" || !_mobile.IsCompatible(esp.filter))
								esp = WapTools.getEspecial(2, (--dia).ToString());
						*/
							XhtmlTools.AddLinkTableCell("img", cellLink2, WapTools.GetText(esp.name), WapTools.GetText("Link" + esp.name), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
							XhtmlImage picto2 = new XhtmlImage();
							//picto2.ImageUrl = WapTools.GetImage(this.Request, especial);
							picto2.ImageUrl = String.Format("{0}/Images/{1}", this.Request.ApplicationPath, esp.name.ToLower() + ".gif");							
							cellImg2.Controls.Add(picto2); 
							cellImg2.HorizontalAlign = HorizontalAlign.Center;
							picto2 = null;  
						}
						catch{rowEspecial2.Visible = false;}
						try
						{ 
							dia = WapTools.isTestSite(this.Request) ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;
							//dia = 3; 
							esp = WapTools.getEspecial(3, dia.ToString());
							while (esp.name == "" || !_mobile.IsCompatible(esp.filter))
								esp = WapTools.getEspecial(3, (--dia).ToString());
							XhtmlTools.AddLinkTableCell("img", cellLink3, WapTools.GetText(esp.name), WapTools.GetText("Link" + esp.name), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
							XhtmlImage picto2 = new XhtmlImage();
							//picto2.ImageUrl = WapTools.GetImage(this.Request, especial);
							picto2.ImageUrl = String.Format("{0}/Images/{1}", this.Request.ApplicationPath, esp.name.ToLower() + ".gif");							
							cellImg3.Controls.Add(picto2);
							cellImg3.HorizontalAlign = HorizontalAlign.Center;
							picto2 = null;
						}
						catch{rowEspecial3.Visible = false;}
						#endregion 
						
						#region IMAGENES DESTACADAS

						img = new XhtmlImage(); 
						img.ImageUrl = WapTools.GetImage(this.Request, "destacados",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbHeaderDestacados, img);
						try      
						{ 
							init = DateTime.Now.Millisecond;							
							if (_contentCollImg.Count > 0)
							{
								DisplayImages(rowImg, "IMG", "IMG_COLOR", init, _contentCollImg.Count, 0);
								//if ((_mobile.MobileDeviceManufacturer.ToUpper() != "SIEMENS")  && (_mobile.MobileDeviceManufacturer.ToUpper() != "MOTOROLA"))
								if (_mobile.IsXHTML)
									DisplayImages(rowTitlesImg, "IMG", "IMG_COLOR", init+2, _contentCollImg.Count, 0);
   
							}  
						}
						catch{}
 
						//if (WapTools.isTestSite(this.Request))
						//	XhtmlTools.AddLinkTable("IMG", tbTop, "Galería", "http://emocion.kiwee.com/xhtml/galeria/catalog.aspx?cg=IMG&cs=2692", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTableRow("IMG", rowTop, WapTools.GetText("TopImg"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/Top")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableRow("IMG", rowNew,WapTools.GetText("NewImg"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/New")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableRow("IMG", rowTop, WapTools.GetText("TopNew"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/TopNew")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableRow("IMG", rowNew,WapTools.GetText("NewImg"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/New")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						 
						//XhtmlTools.AddLinkTableRow("IMG", rowRec, WapTools.GetText("Recomendados"), "#", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "movblue"));
						#endregion  

						#region POR CATEGORIAS 
						if (_contentCollContentSet.Count > 2)
						{
							try{DisplayContentSets(tbTop, "IMG", _contentCollContentSet.Count - 2, -1);}
							catch{}
							XhtmlTools.AddLinkTable("IMG", tbTop, WapTools.GetText("Categorias"), "./linkto.aspx?cg=COMPOSITE&id=3619", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							//if (_contentCollPostals.Count > 0)
							//	DisplayImages(rowPostales, "IMG", "IMG_COLOR", init, _contentCollPostals.Count, 1);
							//XhtmlTools.AddLinkTable("IMG", tbTop, WapTools.GetText("Postales"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}&d=1", WapTools.GetXmlValue("Home/POSTALES")), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						}
						#endregion
				  
						#region DESCARGATE
						img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "descargate",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbHeaderEnd, img);
						if (_mobile.IsCompatible("VIDEO_DWL"))
							XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("ANIM_COLOR"))
							XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (WapTools.isCompatibleThemes(_mobile)) 
							XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("VIDEO_DWL"))
							XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						if (_mobile.IsCompatible("VIDEO_DWL"))
							XhtmlTools.AddLinkTable("IMG", tbEnd, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						tbEnd2.Visible = false;
						#endregion						

						#region SHOPS y ALERTAS
						img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "portales",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbTitleShop, img);
							
						XhtmlTools.AddLinkTableCell("IMG", cellShop1, WapTools.GetText("Shop1"), "./linkto.aspx?id=1", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTableCell("IMG", cellShop2, WapTools.GetText("Shop6"), "./linkto.aspx?id=6", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTableCell("IMG", cellShop3, WapTools.GetText("Shop4"), "./linkto.aspx?id=4", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTableCell("IMG", cellShop4, WapTools.GetText("Shop11"), "./linkto.aspx?id=11", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableCell("IMG", cellShop5, "Promo Quad", "./linkto.aspx?id=10020", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableCell("IMG", cellShop6, "Promo LFP", "./linkto.aspx?id=10022", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
//						XhtmlTools.AddLinkTableRow("IMG", rowShop1,WapTools.GetText("Shop1"), "./linkto.aspx?id=1", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
//						XhtmlTools.AddLinkTableRow("IMG", rowShop2,WapTools.GetText("Shop6"), "./linkto.aspx?id=6", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
//						XhtmlTools.AddLinkTableRow("IMG", rowShop3,WapTools.GetText("Shop4"), "./linkto.aspx?id=4", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
//						XhtmlTools.AddLinkTableRow("IMG", rowShop4,WapTools.GetText("Shop2"), "./linkto.aspx?id=2", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						//XhtmlTools.AddLinkTableRow("IMG", rowShop4,WapTools.GetText("Cateto"), "./linkto.aspx?id=10002&cg=COMPOSITE", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTableRow("IMG", rowmoreshops, WapTools.GetText("moreshops"), "./linkto.aspx?id=20", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						
						img = new XhtmlImage();
						img.ImageUrl = WapTools.GetImage(this.Request, "apuntate2",  _mobile.ScreenPixelsWidth);
						XhtmlTools.AddImgTable(tbHeaderApuntante, img);
						
						XhtmlTools.AddLinkTable("IMG", tbEnd3, WapTools.GetText("Alerta1"), "./linkto.aspx?id=21", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd3, WapTools.GetText("Alerta2"), "./linkto.aspx?id=22", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd3, WapTools.GetText("Alerta5"), "./linkto.aspx?id=25", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						XhtmlTools.AddLinkTable("IMG", tbEnd3, "Más alertas", "./linkto.aspx?id=26", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));						
						#endregion

						#region PICTOS
						fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
						buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
						emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
						back = WapTools.getPicto(this.Request, "back", _mobile);
						up = WapTools.getPicto(this.Request, "up", _mobile);
						musica = (_mobile.ScreenPixelsWidth > 128) ? "M&uacute;sica y Tonos" : "M&uacute;sica";
						#endregion 
					}
				}
				else
				{
					XhtmlTableRow row = new XhtmlTableRow();
					XhtmlTools.AddTextTableRow(row, WapTools.GetText("Compatibility2"), Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XSmall);
					tbEnd.Rows.Add(row);
					row = null;
				}
			}
			catch(Exception caught)
			{  
				WapTools.SendMail("default", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\default.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				Response.Redirect("./error.aspx");				
			}
			finally
			{
				_contentCollImg = null;
				_contentCollAnim = null;
				_contentCollContentSet = null;
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
			else _contentCollAnim.Add(content);
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
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, drm, contentGroup, contentType, HttpUtility.UrlEncode("xhtml|HOME"), "", "0");
							               
			TableItemStyle tableStyle = new TableItemStyle();
			tableStyle.HorizontalAlign = HorizontalAlign.Center;
			int previews = (_mobile.ScreenPixelsWidth < 140) ? 1 : 2;
                     
			for( int i = start; i < start + previews; i++ )  
			{
				if (contentGroup == "IMG" && drm==0)
					content = (Content)_contentCollImg[ (i) % _contentCollImg.Count];
				else if (contentGroup == "ANIM")
					content = (Content)_contentCollAnim[ (i) % _contentCollAnim.Count];

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

		public void DisplayContentSets(XhtmlTable t, string cg, int rangeInf, int rangeSup)
		{
			_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
			_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
			_contentSetDisplayInst.UrlDwld = "./linkto.aspx?id={0}&cg={1}";
			XhtmlTableCell cell = new XhtmlTableCell();
			XhtmlTableRow row = new XhtmlTableRow();
			if (rangeSup == -1) rangeSup = _contentCollContentSet.Count;
			if (rangeSup > _contentCollContentSet.Count) rangeSup = _contentCollContentSet.Count;
			for( int i = rangeInf; i < rangeSup; i++ )
			{
				Content content = (Content)_contentCollContentSet[i];
				//if (WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup") != cg) continue;
				ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
				contentSetDisplay.Display(cell, content, true);			
				contentSetDisplay = null;
				row.Controls.Add(cell);
				cell = new XhtmlTableCell();
				t.Controls.Add(row);
				row = new XhtmlTableRow();
				content = null;
			}
			//t.Controls.Add(row);
			_contentSetDisplayInst = null;
			cell = null; row = null;
		}
		#endregion
	}
}
