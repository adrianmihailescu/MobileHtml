using System;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class _sf : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Panel pnlCatalog, pnlEnd;
		protected System.Web.UI.MobileControls.Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_mobile = (MobileCaps)Request.Browser;

				if (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile))
				{
					try{Response.Redirect("http://emocion.kiwee.com/xhtml/sf.aspx", false);}
					catch{Response.Redirect("http://emocion.kiwee.com/xhtml/sf.aspx", true);}
				} 
				else
				{
					if (_mobile.IsCompatible("GAME_JAVA"))
						WapTools.AddLink(pnlCatalog, "Promo - Juego San Fermines 2007 y m�s...", "http://www.iwapserver.com/ms/sanfermines2007", WapTools.GetImage(this.Request, "bullet"), _mobile);
					WapTools.AddLink(pnlCatalog, "Im�genes de San Ferm�n", "./catalog.aspx?cg=IMG&cs=5884", WapTools.GetImage(this.Request, "bullet"), _mobile);
					if (_mobile.IsCompatible("SOUND_POLY"))
						WapTools.AddLink(pnlCatalog, "M�sica de San Ferm�n", "http://www.marketingmovil.com:8080/portal/ServletAction?idaccion=3017&indnav=1&finnav=2&id=5568", WapTools.GetImage(this.Request, "bullet"), _mobile);
					if (_mobile.IsCompatible("ANIM_COLOR"))
						WapTools.AddLink(pnlCatalog, "Animaciones de San Ferm�n", "./catalog.aspx?cg=ANIM&cs=5881", WapTools.GetImage(this.Request, "bullet"), _mobile);


					#region HEADER
					imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "sf"), WapTools.GetFolderImg(_mobile));
					#endregion
					
					WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
					WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
					WapTools.AddLink(pnlEnd, "Atr�s", "default.aspx", WapTools.GetImage(this.Request, "back"), _mobile);
					WapTools.AddLink(pnlEnd, "Arriba", String.Format("catalog.aspx?{0}", Request.ServerVariables["QUERY_STRING"]), WapTools.GetImage(this.Request, "up"), _mobile);
				}
			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\wap\\catalog.aspx  - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				this.RedirectToMobilePage("./error.aspx");				
			}
		}

		#region Code g�n�r� par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN�: Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// M�thode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette m�thode avec l'�diteur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
