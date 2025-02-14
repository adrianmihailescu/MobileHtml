using System;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class shops : System.Web.UI.MobileControls.MobilePage
	{
		protected System.Web.UI.MobileControls.Form Form1;
		protected Panel pnlShops, pnlEnd;
		protected System.Web.UI.MobileControls.Panel pnlPreview;
		protected Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser; 
			if (_mobile.IsAdvanced)
				imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "portales"), WapTools.GetFolderImg(_mobile));
			
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop7"), "./linkto.aspx?id=7", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop2"), "./linkto.aspx?id=2", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop10"), "./linkto.aspx?id=10", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop12"), "./linkto.aspx?id=12", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop8"), "./linkto.aspx?id=8", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop3"), "./linkto.aspx?id=3", WapTools.GetImage(this.Request, "bullet"), _mobile);
			//WapTools.AddLink(pnlShops, WapTools.GetText("Shop5"), "./linkto.aspx?id=5", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop9"), "./linkto.aspx?id=9", WapTools.GetImage(this.Request, "bullet"), _mobile);
		//	WapTools.AddLink(pnlShops, WapTools.GetText("Shop13"), "./linkto.aspx?id=13", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop16"), "./linkto.aspx?id=16", WapTools.GetImage(this.Request, "bullet"), _mobile);
		//	WapTools.AddLink(pnlShops, WapTools.GetText("Shop15"), "./linkto.aspx?id=15", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlShops, WapTools.GetText("Shop17"), "./linkto.aspx?id=17", WapTools.GetImage(this.Request, "bullet"), _mobile);
			//WapTools.AddLink(pnlShops, WapTools.GetText("Back"), "./default.aspx", WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
			WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
			WapTools.AddLink(pnlEnd, "Atrás", "default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
			WapTools.AddLink(pnlEnd, "Arriba", "shops.aspx", WapTools.GetImage(this.Request, "up"), _mobile);
			WapTools.AddLinkCenter(pnlShops, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
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
