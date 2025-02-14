using System;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap.galeria
{
	public class catalog : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Form frmCatalog; 
		protected System.Web.UI.MobileControls.Panel pnlCatalog, pnlEnd;
		protected System.Web.UI.MobileControls.Panel pnlPreview;
		protected System.Web.UI.MobileControls.Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try
			{
				_mobile = (MobileCaps)Request.Browser;

				if (_mobile.MobileType != null && _mobile.IsCompatible("IMG_COLOR"))
				{
					_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;
					int page = (Request.QueryString["n"] != null) ? Convert.ToInt32(Request.QueryString["n"]) : 1;
					_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"].ToString() : "";
					_contentType = WapTools.GetDefaultContentType(_contentGroup);
					_displayKey = WapTools.GetXmlValue("DisplayKey");
					string contentGroupDisplay = (Request.QueryString["cgd"] != null) ? Request.QueryString["cgd"].ToString() : "";
					
					ContentSet contentSet = BrowseContentSetExtended();
					int nbPreview = 1;
					int nbPages = (contentSet.Count % nbPreview == 0) ? contentSet.Count / nbPreview : contentSet.Count / nbPreview + 1;
					
					_imgDisplayInst = new ImgDisplayInstructions(_mobile);
					if (_mobile.ScreenPixelsWidth > 176) _imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_Galeria_IMG");
					else if (_mobile.ScreenPixelsWidth > 101) _imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_Galeria2_IMG");				
					else _imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", _contentGroup));					
					
					ReadContentSet(contentSet, pnlCatalog, page, nbPreview, true);

					string txtPrevious = WapTools.GetText("Previous");
					string txtNext = WapTools.GetText("Next"); 

					if(_hasNextPage)
						WapTools.AddLink(pnlCatalog, txtNext, String.Format("./catalog.aspx?cg={0}&cs={1}&d={2}&n={3}&cgd={4}&{5}", _contentGroup, _idContentSet, Request.QueryString["d"], page + 1, contentGroupDisplay, WapTools.GetParamBack(Request, false)), WapTools.GetImage(this.Request, "bullet"), _mobile);
					if(_hasPreviousPage)
						WapTools.AddLink(pnlCatalog, txtPrevious, String.Format("./catalog.aspx?cg={0}&cs={1}&d={2}&n={3}&cgd={4}&{5}", _contentGroup, _idContentSet, Request.QueryString["d"], page - 1, contentGroupDisplay, WapTools.GetParamBack(Request, false)), WapTools.GetImage(this.Request, "bullet"), _mobile);

					//Le lien vers le composite général ne doit s'afficher que s'il n'y a pas de lien retour et que l'on n'est pas sur le 1er lot de composite
					try
					{
						if( Request.QueryString["a0"] == null && _idContentSet != Convert.ToInt32(WapTools.GetXmlValue(String.Format("Home/Composite_{0}", contentGroupDisplay))) )
							WapTools.AddLink(pnlCatalog, WapTools.GetText(contentGroupDisplay), String.Format("catalog.aspx?cg=COMPOSITE&cs={0}&cgd={1}", WapTools.GetXmlValue(String.Format("Home/Composite_{0}", contentGroupDisplay)), contentGroupDisplay), "", _mobile);
					}
					catch {} 

					#region LINKS
					try
					{
						WapTools.AddLabel(pnlCatalog, " ", "", _mobile);
						WapTools.AddLinkCenter(pnlCatalog, WapTools.GetText("ImagenesFondos"), "../default.aspx", "", _mobile);
					}
					catch{}
					#endregion

					contentSet = null;
				}
				else
					WapTools.AddLabel(pnlCatalog, WapTools.GetText("Compatibility"), "", _mobile);

				#region HEADER
				if (_mobile.IsAdvanced)
				{ 
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
				}
				else pnlPreview.Visible = false;
				#endregion
					
				WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
				WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
				WapTools.AddLink(pnlEnd, "Atrás", "../default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
				WapTools.AddLink(pnlEnd, "Arriba", String.Format("./catalog.aspx?{0}", Request.ServerVariables["QUERY_STRING"]), WapTools.GetImage(this.Request, "up"), _mobile);
			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\wap\\galeria\\catalog.aspx  - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				this.RedirectToMobilePage("../error.aspx");				
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
