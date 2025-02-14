using System;
using System.Configuration;
using AGInteractive.Business;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	/// <summary>
	/// Description résumée de image.
	/// </summary>
	public class imagebranded : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Panel pnlContent;
		protected System.Web.UI.MobileControls.Form Form1;
		protected int ms;
		protected string theme = "", page = "";
		private void Page_Load(object sender, System.EventArgs e)
		{
			_mobile = (MobileCaps)Request.Browser;
			_idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 0;
			if(_idContent > 0)
			{
				try 
				{
					_contentType = (Request.QueryString["ct"] != null) ? Request.QueryString["ct"] : "";
					_contentGroup = WapTools.GetDefaultContentGroup(_contentType);
					try{_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;}
					catch{_idContentSet = 0;}
					try{ms = (Request.QueryString["ms"] != null) ? Convert.ToInt32(Request.QueryString["ms"]) : 0;}
					catch{ms = 0;}
					string referer = (Request.QueryString["ref"] != null) ? Request.QueryString["ref"].ToString() : "";
					_displayKey = WapTools.GetXmlValue("DisplayKey");
					try
					{
						if (referer.IndexOf("CONTENTSET") > 0)
						{
							char[] r = {'|'};
							string[] spl = Server.UrlDecode(referer).Split(r);
							theme = spl[2];
							page = spl[3];
						}
					}
					catch{}
					this.ActiveForm.Title = "Imágenes y Fondos";
					Operator op = new Operator(Request.UserHostAddress);
					Trace.Warn(op.OperatorName);
					if (op.OperatorName!= null && op.OperatorName == "MOVISTAR")
					{
						DownloadInfo downloadInfo = null;
						BillingRequest billingRequest = null;
	
						CommandItem commandItem = new CommandItem(new Guid(_displayKey), _idContent, _contentType, null, referer, _mobile.MobileType, _contentGroup);
						BillingManager billingManager = new BillingManager(); 
	
						billingRequest = billingManager.CreateCommand(Request, WapTools.GetXmlValue("Billing/Provider"), commandItem);
						downloadInfo = billingManager.DeliverCommand(Request, billingRequest.GUIDCommand, ConfigurationSettings.AppSettings["UrlContentWap_IMG_BRANDED"], null, WrapperType.DescriptorWrapper);
					
						string dwldUrl = downloadInfo.Uri;
						Trace.Warn( "Uri : " + dwldUrl );

						WapTools.AddLabel(pnlContent, "Para finalizar tu descarga, pulsa en el siguiente enlace UNA SOLA VEZ:", "", _mobile);
						WapTools.AddLabel(pnlContent, " ", "", _mobile);
						WapTools.AddLink(pnlContent, "Descargar aquí", dwldUrl, "", _mobile);
						WapTools.AddLabel(pnlContent, " ", "", _mobile);
						if (_contentType=="IMG_COLOR" || _contentType=="ANIM_COLOR")
							WapTools.AddLabel(pnlContent, "Puede durar varios segundos", "", _mobile);
						else
							WapTools.AddLabel(pnlContent, "Puede durar hasta un minuto", "", _mobile);
						if (referer.IndexOf("HOME_")>0) _contentGroup = "COMPOSITE";
						if (ms>0)
						{
							try
							{
								if(referer.IndexOf("HOME_") > 0)
									WapTools.AddLink(pnlContent, "Volver a " + WapTools.GetText(ms.ToString()) + "", String.Format("http://emocion.kiwee.com/wap/minisite.aspx?id={0}", ms.ToString()), "", _mobile);
								else if(theme != "")
								{
									if (page == "") page = "1";
									WapTools.AddLink(pnlContent, "Volver a " + theme, String.Format("http://emocion.kiwee.com/wap/catalog.aspx?ms={0}&cs={1}&cg={2}&n={3}", ms.ToString(), _idContentSet.ToString(), _contentGroup, page), "", _mobile);
								}
							}
							catch{}
						}
						else
						{
							try
							{
								if(theme != "")
								{
									if (page == "") page = "1";
									if (theme.ToUpper().IndexOf("MÁS ") < 0)
										WapTools.AddLink(pnlContent, "Más " + theme, String.Format("http://emocion.kiwee.com/wap/catalog.aspx?cs={0}&cg={1}", _idContentSet.ToString(), _contentGroup), "", _mobile);
									else 
										WapTools.AddLink(pnlContent, theme, String.Format("http://emocion.kiwee.com/wap/catalog.aspx?cs={0}&cg={1}", _idContentSet.ToString(), _contentGroup), "", _mobile);
									
								}
							}
							catch{}
						}						
					}
					else
						WapTools.AddLabel(pnlContent, WapTools.GetText("Operateur"), "", _mobile);

					WapTools.AddLink(pnlContent, "Imágenes y Fondos", "http://emocion.kiwee.com/wap", "", _mobile);
				}
				catch(Exception caught)
				{
					WapTools.SendMail("imagebranded", Request.UserAgent, caught.ToString(), Request.ServerVariables);
					Log.LogError(String.Format(" Unexpected exception in EMOCION --> emocion.kiwee.com\\wap\\imagebranded.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
					this.RedirectToMobilePage("http://emocion.kiwee.com/wap/error.aspx");	
				}
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
