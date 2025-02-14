using System;
using System.Web;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class viewdwld : CatalogBrowsing
	{
		protected string description, url, site = "wap";
		protected Panel pnlView;
		protected System.Web.UI.MobileControls.Form frmSearch;
		protected Image imgLogo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Content content = null;;
				_mobile = (MobileCaps)Request.Browser;
				
				try
				{
					if (Request["ref"]!=null && Request.QueryString["ref"]=="SEARCH") WapTools.LogUser(this.Request, 104, _mobile.MobileType);
					else if (Request["ref"]!=null && Request.QueryString["ref"]=="JUMPTAP") WapTools.LogUser(this.Request, 105, _mobile.MobileType);
				}
				catch{}		
				
				_displayKey = WapTools.GetXmlValue("DisplayKey");
				int idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 24957;
				string referer = (Request.QueryString["ref"] != null) ? Request.QueryString["ref"].ToString() : "";

				try{content = StaticCatalogService.GetAllContentDetails(_displayKey, idContent, _mobile.MobileType, null, null);}
				catch {content = null;}

				try{_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;}
				catch{_idContentSet = 0;}

				if (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile)) site = "xhtml";
							
				if (content != null )
				{
					_contentGroup = content.ContentGroup.Name;
					_contentType = WapTools.GetDefaultContentType(_contentGroup);
					
					if (_mobile.IsCompatible(_contentType))
					{
						url = String.Format(WapTools.GetUrlBilling(this.Request, 0, _contentGroup, _contentType, HttpUtility.UrlEncode(site + "|" + referer), "", _idContentSet.ToString()), WapTools.isBranded(content) ? "branded" : "", idContent.ToString(), _contentType);
						try{this.RedirectToMobilePage(url, false);}
						catch{this.RedirectToMobilePage(url, true);}
					}
					else
					{
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
						WapTools.AddLabel(pnlView, WapTools.GetText("ContentCompatibility"), "", _mobile);
						WapTools.AddLinkCenter(pnlView, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
					}
				}
				else
					this.RedirectToMobilePage("./error.aspx");	
			}
			catch(Exception caught)
			{
				WapTools.SendMail("viewdwld", Request.UserAgent, caught.ToString(), Request.ServerVariables);
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
