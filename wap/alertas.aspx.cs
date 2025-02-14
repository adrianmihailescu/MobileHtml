using System;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class alertas : System.Web.UI.MobileControls.MobilePage
	{
		protected System.Web.UI.MobileControls.Form Form1;
		protected Panel pnlAlertas, pnlEnd;
		protected Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser; 
			if (_mobile.IsAdvanced) imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "apuntate"), WapTools.GetFolderImg(_mobile));
			WapTools.AddLabel(pnlAlertas, WapTools.GetText("Alertas"), "", _mobile);
			//WapTools.AddLink(pnlAlertas, WapTools.GetText("Alerta1"), WapTools.GetText("LnkAlerta1"), WapTools.GetImage(this.Request, "bullet"), _mobile);
			//WapTools.AddLink(pnlAlertas, WapTools.GetText("Alerta2"), WapTools.GetText("LnkAlerta2"), WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlAlertas, WapTools.GetText("Alerta3"), WapTools.GetText("LnkShop23"), WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlAlertas, WapTools.GetText("Alerta4"), WapTools.GetText("LnkShop24"), WapTools.GetImage(this.Request, "bullet"), _mobile);
			//WapTools.AddLink(pnlAlertas, WapTools.GetText("Alerta5"), WapTools.GetText("LnkAlerta5"), WapTools.GetImage(this.Request, "bullet"), _mobile);
			WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
			WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
			WapTools.AddLink(pnlEnd, "Atrás", "default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
			WapTools.AddLink(pnlEnd, "Arriba", "alertas.aspx", WapTools.GetImage(this.Request, "up"), _mobile);
			WapTools.AddLinkCenter(pnlAlertas, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
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
