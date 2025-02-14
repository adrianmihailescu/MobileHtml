using System;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class search : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Form frmSearch;
		protected System.Web.UI.MobileControls.Panel pnlSearch;
		protected System.Web.UI.MobileControls.StyleSheet StyleSheet1;


		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_mobile = (MobileCaps)Request.Browser; 
				
				_contentType = (Request.QueryString["ct"] != null) ? Request.QueryString["ct"].ToString() : (Request.QueryString["__V_ct"] != null ? Request.QueryString["__V_ct"].ToString() : "");
				_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"].ToString() : (Request.QueryString["__V_cg"] != null ? Request.QueryString["__V_cg"].ToString() : "");

				WapTools.AddSearchBlock(this, pnlSearch, WapTools.GetText("SearchCmd"), "", 
					"", "", _contentGroup, _contentType, _mobile);

				WapTools.AddLink(pnlSearch, WapTools.GetText("Back"), "./default.aspx", "", _mobile);				
			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site KiweeImagenes : Unexpected exception in KiweeImagenes\\search.aspx - UA : {0}", Request.UserAgent), caught);
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
