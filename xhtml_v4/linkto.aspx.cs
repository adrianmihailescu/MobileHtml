using System;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class linkto : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser;
			string idSite = Request.QueryString["id"];
			try
			{ 
				WapTools.SetHeader(this.Context);
				WapTools.AddUIDatLog(Request, Response);
			}
			catch{}
			if (Convert.ToInt32(idSite) < 30)
			{
				try
				{
					int log = 110;
					log += Convert.ToInt32(idSite);
					WapTools.LogUser(this.Request, log, _mobile.MobileType);
					Response.Redirect(WapTools.GetText("LnkShop" + idSite), false);
				}
				catch{Response.Redirect(WapTools.GetText("LnkShop" + idSite), true);}
			}
			else if (Convert.ToInt32(idSite) >= 10000)
			{
				WapTools.LogUser(this.Request, Convert.ToInt32(idSite), _mobile.MobileType);
				Response.Redirect(WapTools.GetText("LnkGenera" + idSite), false);
			}
			else
			{
				try
				{
					WapTools.LogUser(this.Request, Convert.ToInt32(idSite), _mobile.MobileType);
					Response.Redirect(String.Format("./catalog.aspx?cg={0}&cs={1}&p={2}&t={3}", Request.QueryString["cg"], idSite, Request.QueryString["p"], Request.QueryString["t"]), false);
				}
				catch{Response.Redirect(String.Format("./catalog.aspx?cg={0}&cs={1}&p={2}&t={3}", Request.QueryString["cg"], idSite, Request.QueryString["p"], Request.QueryString["t"]), true);}
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
