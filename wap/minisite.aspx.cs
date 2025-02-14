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
	public class minisite : CatalogBrowsing
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
		protected string picto = "bullet";
		
		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try 
			{ 
				_mobile = (MobileCaps)Request.Browser; 
				
				if (_mobile.PreferredRenderingType == "chtml10")
				{
					try{Response.Redirect(String.Format(ConfigurationSettings.AppSettings["UrlIModeMinisite"], "COMPOSITE", Request.QueryString["id"]!=null ? Request.QueryString["id"] : WapTools.GetXmlValue("Home/Composite")), false);}
					catch{Response.Redirect(String.Format(ConfigurationSettings.AppSettings["UrlIModeMinisite"], "COMPOSITE", Request.QueryString["id"]!=null ? Request.QueryString["id"] : WapTools.GetXmlValue("Home/Composite")), true);}
				} 
				else if (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile))
				{
					try{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlMinisite"] + Request.ServerVariables["QUERY_STRING"], false);}
					catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlMinisite"] + Request.ServerVariables["QUERY_STRING"], true);}
				} 
				else 
				{
					if (_mobile.MobileType != null &&  _mobile.IsCompatible("IMG_COLOR"))
					{      
						try{WapTools.AddUIDatLog(Request, Response);}
						catch{}      
						_displayKey = WapTools.GetXmlValue("DisplayKey");  
						_idContentSet = Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : Convert.ToInt32(WapTools.GetXmlValue("Home/Composite"));
						if  (_idContentSet == 6123) picto = "halloween";
						BrowseContentSetExtended( pnlPreview, -1, -1 );
						if (_mobile.IsAdvanced)  
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, _idContentSet.ToString()), WapTools.GetFolderImg(_mobile));					
			
						if (_contentCollImg.Count > 0)
							DisplayImages(pnlPreview, "IMG", "IMG_COLOR", (_idContentSet == 6032) ? 5 : 3);
						DisplayContentSets(pnlPreview, "IMG", 0, -1);
						if  (_idContentSet == 6032) DisplayContentSets(pnlPreview, "COMPOSITE", 0, -1);
						if  (_idContentSet == 6123) 	
						{
							WapTools.AddLink(pnlPreview, WapTools.GetText("FondoNombresH"), "./linkto.aspx?cg=COMPOSITE&id=10036", WapTools.GetImage(this.Request, picto), _mobile);							
							WapTools.AddLink(pnlPreview, WapTools.GetText("TonosH"), "./linkto.aspx?cg=COMPOSITE&id=10038", WapTools.GetImage(this.Request, picto), _mobile);							
							WapTools.AddLink(pnlPreview, WapTools.GetText("TonoMensajesH"), "./linkto.aspx?cg=COMPOSITE&id=10037", WapTools.GetImage(this.Request, picto), _mobile);							
						}
						try
						{
							if (_mobile.IsCompatible("ANIM_COLOR"))
							{
								DisplayImages(pnlPreview, "ANIM", "ANIM_COLOR", 2);
								DisplayContentSets(pnlPreview, "ANIM", 0, -1);	
							}
						}
						catch{}

						try
						{
							if (_mobile.IsCompatible("VIDEO_RGT"))
							{
								DisplayContentSets(pnlPreview, "VIDEO_RGT", 0, -1);	
							}
						}
						catch{}

						if (_idContentSet == 6032)
						{
							WapTools.AddLink(pnlPreview, "Más Imágenes", "./catalog.aspx?cg=IMG&cs=6032", WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlPreview, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlPreview, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
						}

						WapTools.AddLinkCenter(pnlPreview, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);

						#region FOOTER						
						WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
						WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
						WapTools.AddLink(pnlEnd, "Atrás", "http://wap.movistar.com", WapTools.GetImage(this.Request, "back"), _mobile);
						WapTools.AddLink(pnlEnd, "Arriba", "default.aspx", WapTools.GetImage(this.Request, "up"), _mobile);
						#endregion
					}
					else
						WapTools.AddLabel(pnlTemas, WapTools.GetText("Compatibility"), "", _mobile);
				}
			}		
			catch(Exception caught)
			{
				WapTools.SendMail("minisite", Request.UserAgent, caught.ToString(), Request.ServerVariables);
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

		public void DisplayImages(Panel pnl, string contentGroup, string contentType, int cont)
		{
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", contentGroup));
			_imgDisplayInst.TextDwld = WapTools.GetText("Download");
			//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, contentGroup, contentType, HttpUtility.UrlEncode("wHOME"), "", "0");
			_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, 0, contentGroup, contentType, HttpUtility.UrlEncode("wap|HOME_" + WapTools.GetText(_idContentSet.ToString())), "", "0");
			_imgDisplayInst.DisplayDescription = false;
			int i = DateTime.Now.Second; 
			Content content;
			if (!_mobile.IsAdvanced) cont=1;
			for (int j=0; j<cont; j++)
			{
				if( contentGroup == "IMG" )
					content = (Content)_contentCollImg[ (j+i) % _contentCollImg.Count ];
				else
					content = (Content)_contentCollAnim[ (j+i) % _contentCollAnim.Count ];
				ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
				imgDisplay.Display(pnl, content, true);
				imgDisplay = null;
			}
		}

		public void DisplayContentSets(Panel pnl, string cg, int rangeInf, int rangeSup)
		{
			if (rangeInf == -1) rangeInf = 0;
			if (rangeSup == -1) rangeSup = _contentCollContentSet.Count;
			_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
			_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, picto);
			_contentSetDisplayInst.UrlDwld = "./catalog.aspx?cs={0}&cg={1}&ms=" + _idContentSet.ToString();

			for( int i = rangeInf; i < rangeSup; i++ )
			{
				Content content = (Content)_contentCollContentSet[i];
				if (WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup") != cg) continue;
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
