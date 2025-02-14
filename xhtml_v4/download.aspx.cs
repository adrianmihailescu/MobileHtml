using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using AGInteractive.Business;
using xhtml_v3.Tools;

namespace xhtml
{
	public class download : XCatalogBrowsing
	{
		private bool purchased = false;
		protected XhtmlTable tbDownload;
		protected Panel pnlFooter;
		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_mobile = (MobileCaps)Request.Browser;      
				try
				{
					WapTools.SetHeader(this.Context);
					WapTools.AddUIDatLog(Request, Response);
				}
				catch{}

				_displayKey = WapTools.GetXmlValue("DisplayKey");
				_contentType = (Request.QueryString["ct"] != null) ? Request.QueryString["ct"].ToString() : "";                        
				_idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 0;
				try{_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : Convert.ToInt32(WapTools.GetXmlValue("Home/Top_" + _contentGroup));}
				catch{_idContentSet = 0;}
				string referer = "perso";
				_contentGroup = WapTools.GetDefaultContentGroup(_contentType);
							
				if (_mobile != null && _idContent > 0 && _contentType != "")
				{
					Operator op = new Operator(Request.UserHostAddress);
					if (op.OperatorName!= null && op.OperatorName == "MOVISTAR")
					{
						try
						{
							Customer cst = new Customer(this.Request);
							purchased = cst.CommandExist(_mobile.MobileType.ToString(), _idContent);
							Trace.Warn(purchased.ToString());
						}
						catch{purchased = false;}
						if (purchased)
						{
							#region Billing
							DownloadInfo downloadInfo = null;
							BillingRequest billingRequest = null;	
							CommandItem commandItem = new CommandItem(new Guid(WapTools.GetXmlValue("DisplayKey")), _idContent, _contentType, null, referer, _mobile.MobileType, _contentGroup);
							BillingManager billingManager = new BillingManager();	
							billingRequest = billingManager.CreateCommand(Request, WapTools.GetXmlValue("Billing/Provider_FREE"), commandItem);
							downloadInfo = billingManager.DeliverCommand(Request, billingRequest.GUIDCommand, null, null, WrapperType.DescriptorWrapper);					
							string dwldUrl = downloadInfo.Uri;
							Trace.Warn( "Uri : " + dwldUrl );
							#endregion

							#region Output
							XhtmlTableRow row = new XhtmlTableRow();
							XhtmlTools.AddTextTableRow("IMG", row, "", WapTools.GetText("Download"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
							tbDownload.Rows.Add(row);
							row = new XhtmlTableRow();
							XhtmlTools.AddTextTableRow("", row, "", WapTools.GetText("InfoDownload"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
							tbDownload.Rows.Add(row);
							row = new XhtmlTableRow();
							XhtmlTools.AddLinkTableRow("", row, WapTools.GetText("LnkDownload"), dwldUrl, Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
							tbDownload.Rows.Add(row);
							row = new XhtmlTableRow();
							XhtmlTools.AddTextTableRow("", row, "", WapTools.GetText("InfoDownload2"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
							tbDownload.Rows.Add(row);							
							#endregion
						}
						else
						{
							XhtmlTableRow row = new XhtmlTableRow();
							XhtmlTools.AddTextTableRow("", row, "", WapTools.GetText("NoDownload"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall);
							tbDownload.Rows.Add(row);
						}
					}
					else
					{
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("IMG", row, "", WapTools.GetText("Operador"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XSmall);
						tbDownload.Rows.Add(row);							
					}
				}
			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\download.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				Response.Redirect("./error.aspx");		
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
