using System;
using System.Configuration;
using System.Web;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class view : CatalogBrowsing
	{
		protected Panel pnlView, pnlEnd;
		protected Image imgView, imgLogo;
		protected Label lblContent;
		protected Link linkPPD;
		protected System.Web.UI.MobileControls.Form frmSearch;
		protected System.Web.UI.MobileControls.Panel pnlPreview;
		protected string description;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Content content = null;;
				_mobile = (MobileCaps)Request.Browser;
				//imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
				try
				{
					if (Request["ref"]!=null && Request.QueryString["ref"]=="SEARCH") WapTools.LogUser(this.Request, 104, _mobile.MobileType);
					else if (Request["ref"]!=null && Request.QueryString["ref"]=="JUMPTAP") WapTools.LogUser(this.Request, 105, _mobile.MobileType);
				}
				catch{}		
				if (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile))
				{
					try{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlView"] + Request.ServerVariables["QUERY_STRING"], false);}
					catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlView"] + Request.ServerVariables["QUERY_STRING"], true);}
				} 
				else
				{
					int idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 24957;
					_displayKey = WapTools.GetXmlValue("DisplayKey");
					string referer = (Request.QueryString["ref"] != null) ? Request.QueryString["ref"].ToString() : "";

					try{content = StaticCatalogService.GetAllContentDetails(_displayKey, idContent, _mobile.MobileType, null, null);}
					catch {content = null;}

					try{_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;}
					catch{_idContentSet = 0;}
				
					if (content != null )
					{
						_contentGroup = content.ContentGroup.Name;
						_contentType = WapTools.GetDefaultContentType(_contentGroup);
					
						if (_mobile.PreferredRenderingType == "chtml10")
						{
							string param = String.Format("r={0}&ct={1}&cg={2}&nav={3}", Request.QueryString["c"], _contentType, _contentGroup, Request.QueryString["ref"]);
							try{Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeView"] + param, false);}
							catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeView"] + param, true);}
						}
						else if (_mobile.IsCompatible(_contentType))
						{
							if (content.Preview.URL != null)
								imgView.ImageUrl = content.Preview.URL; 
							else
								imgView.ImageUrl = String.Format(WapTools.GetXmlValue("Url_" + _contentGroup), content.ContentName.Substring(0,1), content.ContentName);
							lblContent.Text = content.Name;
						
							linkPPD.NavigateUrl = String.Format(WapTools.GetUrlBilling(this.Request, 0, _contentGroup, _contentType, HttpUtility.UrlEncode("wap|" + referer), "", _idContentSet.ToString()), WapTools.isBranded(content) ? "branded" : "", idContent.ToString(), _contentType);
							if (_contentType == "BG_SCHEME")
								linkPPD.Text = "Descargar por 2 €";
						}
						else
						{
							WapTools.AddLabel(pnlView, WapTools.GetText("ContentCompatibility"), "", _mobile);
							linkPPD.Visible = false;
						}
					}
					else
					{
						WapTools.AddLabel(pnlView, WapTools.GetText("ContentCompatibility"), "", _mobile);
						linkPPD.Visible = false;
					}
				
					#region LINKS
					try
					{
						WapTools.AddLabel(pnlView, " ", "", _mobile);
						if (_contentGroup == "IMG" && _mobile.IsCompatible("IMG_COLOR"))
							WapTools.AddLink(pnlView, WapTools.GetText("MasIMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/IMG")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						else if (_contentGroup == "ANIM" && _mobile.IsCompatible("ANIM_COLOR"))
							WapTools.AddLink(pnlView, WapTools.GetText("MasANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						else if ((_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT") && _mobile.IsCompatible("VIDEO_CLIP"))
							WapTools.AddLink(pnlView, WapTools.GetText("MasVIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLinkCenter(pnlView, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
					}
					catch{}
					#endregion
				
					#region HEADER
					if (_mobile.IsAdvanced)
					{
						if (_contentGroup == "SCHEME")
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "temas"), WapTools.GetFolderImg(_mobile));
						else if (_contentGroup == "IMG")
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
						else if (_contentGroup == "ANIM")
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "animaciones"), WapTools.GetFolderImg(_mobile));
						else if (_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT" || _contentGroup == "")
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "videos"), WapTools.GetFolderImg(_mobile));
						else 
							imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
					}
					#endregion

					//WapTools.AddLink(pnlView, WapTools.GetText("Back"), "./default.aspx", WapTools.GetImage(this.Request, "bullet"), _mobile);
					WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
					WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
					WapTools.AddLink(pnlEnd, "Atrás", "default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
					WapTools.AddLink(pnlEnd, "Arriba",  String.Format("view.aspx?{0}", Request.ServerVariables["QUERY_STRING"]), WapTools.GetImage(this.Request, "up"), _mobile);
				}
			}
			catch(Exception caught)
			{
				WapTools.SendMail("view", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format(" Unexpected exception in wap\\emocion\\view.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
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
	}
}
