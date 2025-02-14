using System;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using KMobile.Catalog.Presentation;
using xhtml.Tools;

namespace xhtml
{
	public class sf : XCatalogBrowsing
	{
		protected XhtmlTable tbTitle, tbCatalog, tbHeader, tbPreviews, tbMusic, tbImagenes, tbAnims, tbVideos;
		protected XhtmlTableRow rowPreviews, rowPreviews2, rowJuego;
		protected XhtmlTableCell cellImgTV, cellLinkTV, cellImgGame, cellLinkGame, cellLinkMusic, cellImgMusic, cellLinkImg, cellImgImg, cellLinkAnim, cellImgAnim, cellLinkVideo, cellImgVideo;
		private int page = 1;
		protected string cg_temp, contentGroupDisplay;
		public string buscar, emocion, atras, up, fondo, back, css;
		protected xhtml.Tools.XhtmlTable Xhtmltable1;
		protected xhtml.Tools.XhtmlTable Xhtmltable2;
		protected xhtml.Tools.XhtmlTable Xhtmltable3;
		protected xhtml.Tools.XhtmlTable Xhtmltable5;

		private void Page_Load(object sender, System.EventArgs e)
		{  
			try 
			{
				_mobile = (MobileCaps)Request.Browser;
				_idContentSet = 5897;
				_contentGroup = "";
				_contentType = WapTools.GetDefaultContentType(_contentGroup);
				_displayKey = WapTools.GetXmlValue("DisplayKey"); 
				string paramBack = String.Format("a0=CatalogGraphic&a1=n&a2={0}&a3=cg&a4={1}&a5=cgd&a6={2}&a7=cs&a8={3}",
					page, _contentGroup, contentGroupDisplay, _idContentSet);

				ContentSet contentSet = BrowseContentSetExtended();                                               

				int nbrows = Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_Rows"));
				int nbcols = (_contentGroup == "ANIM" || _mobile.ScreenPixelsWidth>140) ? 2 : 1;
                      
				nbcols = Convert.ToInt32(WapTools.GetXmlValue("Home/Nb_PreviewsComposite"));
				_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
				_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
				_contentSetDisplayInst.UrlDwld = String.Format("./catalog.aspx?cs={0}&cg={1}&d={2}&cgd={3}&{4}", "{0}", "{1}",  Request.QueryString["d"], contentGroupDisplay, paramBack); 
				if (Request.QueryString["ref"] != "" && Request.QueryString["ref"] != null) _contentSetDisplayInst.UrlDwld += "&ref=" + Request.QueryString["ref"];

				if (_mobile.IsXHTML)
				{
					XhtmlImage picto = new XhtmlImage();
					picto.ImageUrl = WapTools.GetImage(this.Request, "sftv");
					cellImgTV.Controls.Add(picto); 
					cellImgTV.HorizontalAlign = HorizontalAlign.Center;
					XhtmlTools.AddLinkTableCell("img", cellLinkTV, "Televisión en directo", "http://www.marketingmovil.com:8080/portal/ServletAction?idaccion=3023", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
					picto = null;
				}
				else 
					rowJuego.Visible = false;

				if (_mobile.IsCompatible("GAME_JAVA"))
				{
					XhtmlImage picto = new XhtmlImage();
					picto.ImageUrl = WapTools.GetImage(this.Request, "sfgame");
					cellImgGame.Controls.Add(picto); 
					cellImgGame.HorizontalAlign = HorizontalAlign.Center;
					XhtmlTools.AddLinkTableCell("img", cellLinkGame, "Promo - Juego San Fermines 2007 y más...", "http://www.iwapserver.com/ms/sanfermines2007", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
					picto = null;
				}
			
				// SOUND
				XhtmlImage pct = new XhtmlImage();
				pct.ImageUrl = WapTools.GetImage(this.Request, "sftonos");
				cellImgMusic.Controls.Add(pct); 
				cellImgMusic.HorizontalAlign = HorizontalAlign.Center;
				XhtmlTools.AddLinkTableCell("img", cellLinkMusic, "Música de San Fermín", "http://www.marketingmovil.com:8080/portal/ServletAction?idaccion=3017&indnav=1&finnav=2&id=5568", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				
				XhtmlTools.AddLinkTable("", tbMusic, "San Fermín: Toque de clarines", "http://www.marketingmovil.com:8080/portal/ZPFondoDedicatoriasWapAction?idaccion=3018&id=5574&origen=3017", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbMusic, "San Fermín - Camarero.....Una de Mero...", "http://www.marketingmovil.com:8080/portal/ZPFondoDedicatoriasWapAction?idaccion=3018&id=5567&origen=3017", Color.Empty, Color.Empty, 2, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				
				// IMG
				pct = new XhtmlImage();
				pct.ImageUrl = WapTools.GetImage(this.Request, "Especial17");
				cellImgImg.Controls.Add(pct); 
				cellImgImg.HorizontalAlign = HorizontalAlign.Center;
				XhtmlTools.AddLinkTableCell("img", cellLinkImg, "Imágenes de San Fermín", "./catalog.aspx?cg=IMG&cs=5884", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				
				try{DisplayContents("IMG", tbImagenes, contentSet, contentSet.Name, paramBack);}
				catch{}
				pct = null;
						
				// ANIMS
				if (_mobile.IsCompatible("ANIM_COLOR"))
				{
					XhtmlImage picto = new XhtmlImage();
					picto.ImageUrl = WapTools.GetImage(this.Request, "sfanim");
					cellImgAnim.Controls.Add(picto); 
					cellImgAnim.HorizontalAlign = HorizontalAlign.Center;
					XhtmlTools.AddLinkTableCell("img", cellLinkAnim, "Animaciones de San Fermín", "./catalog.aspx?cg=ANIM&cs=5881", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
				
					try{DisplayContents("ANIM", tbAnims, contentSet, contentSet.Name, paramBack);}
					catch{}
					picto = null;
				}
				// VIDEOS
				if (_mobile.IsCompatible("VIDEO_DWL") && false)
				{
					XhtmlImage p = new XhtmlImage();
					p.ImageUrl = WapTools.GetImage(this.Request, "Especial17");
					cellImgVideo.Controls.Add(p); 
					cellImgVideo.HorizontalAlign = HorizontalAlign.Center;
					XhtmlTools.AddLinkTableCell("img", cellLinkVideo, "Videos de San Fermín", "./catalog.aspx?cg=ANIM&cs=5881", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
					p = null;
				}
				_contentSetDisplayInst = null;
				
				#region HEADER
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = WapTools.GetImage(this.Request, "sf",  _mobile.ScreenPixelsWidth);
				XhtmlTools.AddImgTable(tbHeader, img);
				#endregion

				#region PICTOS
				fondo = WapTools.GetImage2(this.Request, "fondo", _mobile.ScreenPixelsWidth);
				buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
				emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
				back = WapTools.getPicto(this.Request, "back", _mobile);
				up = WapTools.getPicto(this.Request, "up", _mobile);
				_mobile = null;
				#endregion

				// Search
				//XhtmlTools.AddLinkTable("", tbSearch, WapTools.GetText("SearchLink"), String.Format("./search.aspx?cg={0}", contentGroupDisplay), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, "pict:///core/action/find");
				atras = WapTools.UpdateFooter(_mobile, this.Context, null); 
				contentSet = null;

			}
			catch(Exception caught) 
			{
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
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1") ? 1 : 0, contentset.ContentGroup, WapTools.GetDefaultContentType(contentset.ContentGroup), HttpUtility.UrlEncode(String.Format("xhtml|HOME_{0}", name)), "", _idContentSet.ToString());
                              
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
