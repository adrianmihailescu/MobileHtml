using System;
using System.Configuration;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class error : System.Web.UI.MobileControls.MobilePage
	{
		protected System.Web.UI.MobileControls.Form Form1;
		protected Panel pnlError, pnlEnd;
		protected System.Web.UI.MobileControls.Panel pnlPreview;
		protected Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser; 

			if (_mobile.PreferredRenderingType == "chtml10")
			{
				try{Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeError"], false);}
				catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlImodeError"], true);}
			}
			else if (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile))
			{
				try{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlError"], false);}
				catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlError"], true);}
			} 
			else 
			{
				WapTools.LogUser(this.Request, 103, _mobile.MobileType);					
				if (_mobile.IsAdvanced)
					imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
			
				WapTools.AddLabel(pnlError, WapTools.GetText("Error2"), "", _mobile);
				//WapTools.AddLink(pnlShops, WapTools.GetText("Back"), "./default.aspx", WapTools.GetImage(this.Request, "bullet"), _mobile);
				WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
				WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
				WapTools.AddLink(pnlEnd, "Atrás", "default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
				WapTools.AddLink(pnlEnd, "Arriba", "error.aspx", WapTools.GetImage(this.Request, "up"), _mobile);
				WapTools.AddLinkCenter(pnlError, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
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
