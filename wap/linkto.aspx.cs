using System;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class linkto : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser;
			string idSite = Request.QueryString["id"];
	
			try
			{
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
					if (WapTools.GetText("LnkShop" + idSite) != "")
					{
						try{Response.Redirect(WapTools.GetText("LnkShop" + idSite), false);}
						catch{Response.Redirect(WapTools.GetText("LnkShop" + idSite), true);}
					}				
					else
						Response.Redirect("./default.aspx");
				}
				catch{Response.Redirect("./default.aspx");}
			}
			else if (Convert.ToInt32(idSite) >= 10000)
			{
				try
				{
					WapTools.LogUser(this.Request, Convert.ToInt32(idSite), _mobile.MobileType);
					if (WapTools.GetText("LnkGenera" + idSite) != "")
					{
						try{Response.Redirect(WapTools.GetText("LnkGenera" + idSite), false);}
						catch{Response.Redirect(WapTools.GetText("LnkGenera" + idSite), true);}					
					}
					else
						Response.Redirect("./default.aspx");
				}
				catch{Response.Redirect("./default.aspx");}
			}
			else
			{
				string link = String.Format("./catalog.aspx?cg={0}&cs={1}&p={2}&t={3}", Request.QueryString["cg"], idSite, Request.QueryString["p"], Request.QueryString["t"]);
				if (Request.QueryString["a1"] != null && Request.QueryString["a1"] != "" )
					link += String.Format("&a1={0}&a2={1}&a3={2}&a4={3}&a5={4}&a6={5}", Request.QueryString["a1"], Request.QueryString["a2"], Request.QueryString["a3"], 
						Request.QueryString["a4"], Request.QueryString["a5"], Request.QueryString["a6"]);
		
				try
				{
					WapTools.LogUser(this.Request, Convert.ToInt32(idSite), _mobile.MobileType);
					Response.Redirect(link, false);
				}
				catch{Response.Redirect(link, true);}
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
