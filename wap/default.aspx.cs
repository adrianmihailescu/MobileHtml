using System;
using System.Collections;
using System.Configuration;
using System.Web;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;
using System.Web.UI.MobileControls;

namespace wap 
{
	public class _default : CatalogBrowsing 
	{
		private ArrayList _contentCollImg = new ArrayList();
		private ArrayList _contentCollContentSet = new ArrayList();
		private ArrayList _contentCollAnim = new ArrayList();
		private ArrayList _contentCollVideo = new ArrayList(); 
		protected Panel pnlTemas, pnlPreview, pnlShops, pnlFooter, pnlEnd;
		protected System.Web.UI.MobileControls.Panel Panel1;
		protected System.Web.UI.MobileControls.Form frmDefault;
		protected System.Web.UI.WebControls.Literal  htmlcode;
		protected Image imgLogo;
		private Especial esp; 
		
		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try 
			{ 
				_mobile = (MobileCaps)Request.Browser; 
				
				try{if (Request["id"]!=null) WapTools.LogUser(this.Request, Convert.ToInt32(Request["id"]), _mobile.MobileType);}
				catch{}
				if (Request["id"]!=null)
				{
					try{Response.Redirect(WapTools.GetText("Lnk" + Request["id"]), false);}
					catch{Response.Redirect(WapTools.GetText("Lnk" + Request["id"]), true);}
				}
				else if (_mobile.PreferredRenderingType == "chtml10")
				{
					try{if (WapTools.isTestSite(this.Request)) Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeDev"], false); else Response.Redirect(ConfigurationSettings.AppSettings["UrlImode"], false);}
					catch{if (WapTools.isTestSite(this.Request)) Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeDev"], false); else Response.Redirect(ConfigurationSettings.AppSettings["UrlImode"], true);}
				}				
				// redirection xhtml
				else if (Convert.ToBoolean(ConfigurationSettings.AppSettings["Switch_Xhtml"]) && (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile)))
				{
					try{if (WapTools.isTestSite(this.Request)) Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlDev"], false); else Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtml"], false);}
					catch{if (WapTools.isTestSite(this.Request)) Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlDev"], false); else Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtml"], true);}
				} 
				else 
				{
					if (_mobile.MobileType != null &&  _mobile.IsCompatible("IMG_COLOR"))
					{      
						try{WapTools.AddUIDatLog(Request, Response);}
						catch{}      
						WapTools.LogUser(this.Request, 101, _mobile.MobileType);
						_displayKey = WapTools.GetXmlValue("DisplayKey");  
						_idContentSet = Convert.ToInt32(WapTools.GetXmlValue("Home/Composite"));
						BrowseContentSetExtended( pnlPreview, -1, -1 );
						if (_mobile.IsAdvanced)
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));

						#region PUB
						WapTools.CallPub(this.Request, WapTools.SearchType.ts_ImagenesFondos_text_top.ToString(), "xml" , this.pnlPreview); // ts_text, ts_mixed, ts_image
						#endregion
					
						#region AHORA EN IMAGENES
						WapTools.AddLabelCenter(pnlPreview, WapTools.GetText("Ahora"), "", _mobile, BooleanOption.True);
						int dia = WapTools.isTestSite(this.Request) ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;
						//dia = 3;
						for (int i=1; i<=3; i++)
						{
							esp = new Especial();
							esp = WapTools.getEspecial(i, dia.ToString());
							
							while (esp.name == "" || !_mobile.IsCompatible(esp.filter))
								esp = WapTools.getCompatibleEspecial(esp);
							dia = WapTools.isTestSite(this.Request) ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;
							//dia = 3;
							//if ((!((WapTools.GetText(especial).ToUpper().IndexOf("TEMAS")>=0) && !WapTools.isCompatibleThemes(_mobile)) ) && 	(!((WapTools.GetText(especial).ToUpper().IndexOf("BISBAL")>=0) && !WapTools.isCompatibleThemes2(_mobile)) ))
							WapTools.AddLink(pnlPreview, WapTools.GetText(esp.name), WapTools.GetText("Link" + esp.name), WapTools.GetImage(this.Request, "bullet"), _mobile);
						}
						//DisplayContentSets(pnlPreview, 0, 1);
						#endregion

						#region DESTACADOS
						WapTools.AddLabelCenter(pnlPreview, WapTools.GetText("Destacados"), "", _mobile, BooleanOption.True);
						DisplayImages(pnlPreview, "IMG", "IMG_COLOR");
						//WapTools.AddLink(pnlTemas, WapTools.GetText("TopNew"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/TopNew")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlTemas, WapTools.GetText("TopImg"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/Top")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						//if (WapTools.isTestSite(this.Request))
						//	WapTools.AddLink(pnlTemas, "Galería", String.Format("./linkto.aspx?id={0}", WapTools.GetXmlValue("Home/Galeria")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						
						//WapTools.AddLink(pnlTemas, WapTools.GetText("NewImg"), String.Format("./linkto.aspx?cg=IMG&id={0}", WapTools.GetXmlValue("Home/New")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						DisplayContentSets(pnlTemas, _contentCollContentSet.Count-2, -1);
						WapTools.AddLink(pnlTemas, WapTools.GetText("Categorias"), "./linkto.aspx?cg=COMPOSITE&id=3619", WapTools.GetImage(this.Request, "bullet"), _mobile);
						//WapTools.AddLink(pnlTemas, WapTools.GetText("EnviaPostales"), String.Format("./linkto.aspx?d=1&cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/POSTALES")), WapTools.GetImage(this.Request, "bullet"), _mobile);					
						#endregion

						#region DESCARGATE  
						WapTools.AddLabelCenter(pnlFooter, WapTools.GetText("Descargate"), "", _mobile, BooleanOption.True);
						if (_mobile.IsCompatible("VIDEO_DWL"))
							WapTools.AddLink(pnlFooter, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						if (_mobile.IsCompatible("ANIM_COLOR"))
							WapTools.AddLink(pnlFooter, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);					
						if (WapTools.isCompatibleThemes(_mobile))
							WapTools.AddLink(pnlFooter, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);					
						WapTools.AddLink(pnlFooter, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlFooter, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlFooter, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						if (_mobile.IsCompatible("VIDEO_DWL"))
						{
							WapTools.AddLink(pnlFooter, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlFooter, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						}
						#endregion
	
						#region SEARCH
						if (_mobile.MobileDeviceManufacturer.ToUpper() != "SIEMENS")						
							WapTools.AddSearchBlock(this, pnlFooter, WapTools.GetText("SearchCmd"), WapTools.GetText("SearchLbl"), "","", "", "", _mobile);
						#endregion 
	
						#region SHOPS
						WapTools.AddLabelCenter(pnlShops, WapTools.GetText("Shops"), "", _mobile, BooleanOption.True);
						WapTools.AddLink(pnlShops, WapTools.GetText("Shop1"), "./linkto.aspx?id=1", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("Shop6"), "./linkto.aspx?id=6", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("Shop4"), "./linkto.aspx?id=4", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("Shop11"), "./linkto.aspx?id=11", WapTools.GetImage(this.Request, "bullet"), _mobile);
						//WapTools.AddLink(pnlShops, WapTools.GetText("Cateto"), "./linkto.aspx?id=10002&cg=COMPOSITE", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("moreshops"), "./linkto.aspx?id=20", WapTools.GetImage(this.Request, "bullet"), _mobile);
						#endregion

						#region ALERTAS
						WapTools.AddLabelCenter(pnlShops, WapTools.GetText("Apuntate"), "", _mobile, BooleanOption.True);
						WapTools.AddLink(pnlShops, WapTools.GetText("Alerta1"), "./linkto.aspx?id=21", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("Alerta2"), "./linkto.aspx?id=22", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, WapTools.GetText("Alerta5"), "./linkto.aspx?id=25", WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLink(pnlShops, "Más Alertas", "./linkto.aspx?id=26", WapTools.GetImage(this.Request, "bullet"), _mobile);
						#endregion						
					}
					else
						WapTools.AddLabel(pnlTemas, WapTools.GetText("Compatibility"), "", _mobile);

					#region FOOTER						
					WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
					WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
					WapTools.AddLink(pnlEnd, "Atrás", "http://wap.movistar.com", WapTools.GetImage(this.Request, "back"), _mobile);
					WapTools.AddLink(pnlEnd, "Arriba", "default.aspx", WapTools.GetImage(this.Request, "up"), _mobile);
					#endregion
				}
			}		
			catch(Exception caught)
			{
				WapTools.SendMail("default", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format(" Unexpected exception in emocion\\wap\\default.aspx - UA : {0}", Request.UserAgent), caught);
				this.RedirectToMobilePage("./error.aspx");				
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
		protected override void DisplayContentSet(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			_contentCollContentSet.Add(content);
		}

		protected override void DisplayImg(Content content, System.Web.UI.MobileControls.Panel pnl, bool preview)
		{
			if( content.ContentGroup.Name == "IMG" ) _contentCollImg.Add(content);
			else _contentCollAnim.Add(content);
		}

		public void DisplayImages(Panel pnl, string contentGroup, string contentType)
		{
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", contentGroup));
			_imgDisplayInst.TextDwld = WapTools.GetText("Download");
			//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, contentGroup, contentType, HttpUtility.UrlEncode("wHOME"), "", "0");
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, 0, contentGroup, contentType, HttpUtility.UrlEncode("wap|HOME"), "", "0");
			_imgDisplayInst.DisplayDescription = false;
			int i = DateTime.Now.Second; 
			Content content;
			if( contentGroup == "IMG" )
				content = (Content)_contentCollImg[ i % _contentCollImg.Count ];
			else
				content = (Content)_contentCollAnim[ i % _contentCollAnim.Count ];
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(pnl, content, true);
			imgDisplay = null;
			if (_mobile.IsAdvanced)
			{
				if( contentGroup == "IMG" )
					content = (Content)_contentCollImg[ (i+1) % _contentCollImg.Count ];
				else
					content = (Content)_contentCollAnim[ (i+1) % _contentCollAnim.Count ];
				imgDisplay = new ImgDisplay(_imgDisplayInst);
				imgDisplay.Display(pnl, content, true);
				imgDisplay = null;
			}
		}

		public void DisplayContentSets(Panel pnl, int rangeInf, int rangeSup)
		{
			if (rangeInf == -1) rangeInf = 0;
			if (rangeSup == -1) rangeSup = _contentCollContentSet.Count;
			_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
			_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
			_contentSetDisplayInst.UrlDwld = "./linkto.aspx?id={0}&cg={1}";

			for( int i = rangeInf; i < rangeSup; i++ )
			{
				Content content = (Content)_contentCollContentSet[i];
				//if (WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup") != "IMG") continue;
				ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
				contentSetDisplay.Display(pnl, content);
				contentSetDisplay = null;
			} 
		}

		protected override void DisplayVideo(Content content, System.Web.UI.MobileControls.Panel pnl, bool preview)
		{
			if( content.ContentGroup.Name == "VIDEO" ) _contentCollVideo.Add(content);
		}
		#endregion
	}
}
