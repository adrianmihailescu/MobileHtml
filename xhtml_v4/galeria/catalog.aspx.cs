using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using KMobile.Catalog.Presentation;
using xhtml.Tools;

namespace xhtml.galeria
{
	public class catalog : XCatalogBrowsing
	{
		protected XhtmlTable tbTitle, tbCatalog, tbLinks, tbSearch, tbLinkImages, tbPages;
		protected XhtmlTableRow rowPreviews, rowPreviews2;
		private int page;
		protected string cg_temp, contentGroupDisplay;
		protected XhtmlTable tbHeader;
		protected XhtmlTable tbPreviews; 
		public string buscar, emocion, atras, up, fondo, back, css;
 
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

				_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;
				page = (Request.QueryString["n"] != null) ? Convert.ToInt32(Request.QueryString["n"]) : 1;
				_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"].ToString() : ""; 
				_contentType = WapTools.GetDefaultContentType(_contentGroup);
				contentGroupDisplay = (Request.QueryString["cgd"] != null) ? Request.QueryString["cgd"].ToString() : _contentGroup;
				_displayKey = WapTools.GetXmlValue("DisplayKey"); 

				string paramBack = String.Format("a1=n&a2={0}&a3=cg&a4={1}&a5=cs&a6={2}",
					page, _contentGroup, _idContentSet);
				
				ContentSet contentSet = BrowseContentSetExtended();                                               

				int nbrows = 1;
				int nbcols = 1;
                      
				#region CONTENTSET
				_imgDisplayInst = new ImgDisplayInstructions(_mobile);
				_imgDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
				if (_mobile.ScreenPixelsWidth > 176) _imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_Galeria_{0}", _contentGroup));
				else _imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_Galeria2_{0}", _contentGroup));				

				ReadContentSetGaleria(contentSet, tbCatalog, page, nbrows*nbcols);
				page_max = contentSet.Count / (nbrows * nbcols);
				if (contentSet.Count % (nbrows * nbcols) > 0) page_max++;			
				_imgDisplayInst = null;
				#endregion

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
					img.ImageUrl = WapTools.GetImage(this.Request, "categorias",  _mobile.ScreenPixelsWidth);
					XhtmlTableRow row = new XhtmlTableRow();
					XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
					tbTitle.Controls.Add(row);  
				}
				else if (_contentGroup == "ANIM" || cg_temp == "ANIM")
				{
					img.ImageUrl = WapTools.GetImage(this.Request, "animaciones",  _mobile.ScreenPixelsWidth);
					XhtmlTableRow row = new XhtmlTableRow();
					XhtmlTools.AddTextTableRow("IMG", row, "", contentSet.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
					tbTitle.Controls.Add(row);  
				}
				else if (_contentGroup == "VIDEO" || cg_temp == "VIDEO" || _contentGroup == "VIDEO_RGT" || cg_temp == "VIDEO_RGT" || _contentGroup == "")
				{
					img.ImageUrl = WapTools.GetImage(this.Request, "videos",  _mobile.ScreenPixelsWidth);
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
				string URL_Suivant = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&cgd={5}&{6}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, derniere + 1, contentGroupDisplay, paramBack);
				string URL_Precedent = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&cgd={5}&{6}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, premiere - 1, contentGroupDisplay, paramBack);
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
							link.NavigateUrl = String.Format("./catalog.aspx?d={0}&ms={1}&cg={2}&cs={3}&n={4}&cgd={5}&{6}", Request.QueryString["d"], Request.QueryString["ms"], _contentGroup, _idContentSet, cont, contentGroupDisplay, paramBack);
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

				#region PICTOS
				fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
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


	}
}
