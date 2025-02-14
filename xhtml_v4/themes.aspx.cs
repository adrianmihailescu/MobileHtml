using System;
using System.Configuration;
using System.Drawing;
using System.Web.UI.WebControls;
using AGInteractive.Business;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class themes : XCatalogBrowsing
	{
		protected XhtmlTable tbContent, tbHeader;
		public string fondo, buscar, emocion, back, up, atras;

		private void Page_Load(object sender, System.EventArgs e)
		{
			_mobile = (MobileCaps)Request.Browser;
						
			try
			{
				WapTools.SetHeader(this.Context);
				WapTools.AddUIDatLog(Request, Response);					
			} 
			catch{}
			
			_idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 0;
			if(_idContent > 0)
			{
				try 
				{
					_contentType = (Request.QueryString["ct"] != null) ? Request.QueryString["ct"] : "";
					_contentGroup = WapTools.GetDefaultContentGroup(_contentType);
					try{_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;}
					catch{_idContentSet = 0;}
					string referer = (Request.QueryString["ref"] != null) ? Request.QueryString["ref"].ToString() : "";
					_displayKey = WapTools.GetXmlValue("DisplayKey");

					Operator op = new Operator(Request.UserHostAddress);
					Trace.Warn(op.OperatorName);
					if (op.OperatorName!= null && op.OperatorName == "MOVISTAR")
					{
						DownloadInfo downloadInfo = null;
						BillingRequest billingRequest = null;
	
						CommandItem commandItem = new CommandItem(new Guid(_displayKey), _idContent, _contentType, null, referer, _mobile.MobileType, _contentGroup);
						BillingManager billingManager = new BillingManager(); 
	
						billingRequest = billingManager.CreateCommand(Request, WapTools.GetXmlValue("Billing/Provider"), commandItem);
						downloadInfo = billingManager.DeliverCommand(Request, billingRequest.GUIDCommand, ConfigurationSettings.AppSettings["UrlContentWap_BG_SCHEME"], null, WrapperType.DescriptorWrapper);
					
						string dwldUrl = downloadInfo.Uri;
						Trace.Warn( "Uri : " + dwldUrl );

						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("", row, "", "Para finalizar tu descarga, pulsa en el siguiente enlace UNA SOLA VEZ:", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
						tbContent.Rows.Add(row);

						row = new XhtmlTableRow();
						XhtmlTools.AddLinkTableRow("", row, "Descargar aquí", dwldUrl, Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, true, FontUnit.XXSmall, "");
						tbContent.Rows.Add(row);
		
						row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("", row, "", "Puede durar varios segundos", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
						tbContent.Rows.Add(row);							
					}
					else
					{
						XhtmlTableRow row = new XhtmlTableRow();
						XhtmlTools.AddTextTableRow("", row, "", WapTools.GetText("Operador"), Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
						tbContent.Rows.Add(row);
					}
				
				}
				catch(Exception caught)
				{
					Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\themes.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
					Response.Redirect("./error.aspx");	
				}
			}
			else
			{
				XhtmlTableRow row = new XhtmlTableRow();
				XhtmlTools.AddTextTableRow("", row, "", "Atención, se ha producido un error. Reintenta de nuevo la descarga del mismo contenido, no serás cobrado por ello, ya que tendrás una descarga pendiente.", Color.Empty, Color.Empty, 1, HorizontalAlign.Center, VerticalAlign.Middle, false, FontUnit.XXSmall);
				tbContent.Rows.Add(row);		
			}
				
			#region HEADER
			XhtmlImage img = new XhtmlImage();
			img.ImageUrl = WapTools.GetImage(this.Request, "imagenes",  _mobile.ScreenPixelsWidth);
			XhtmlTools.AddImgTable(tbHeader, img);
			#endregion

			#region PICTOS
			fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
			buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
			emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
			back = WapTools.getPicto(this.Request, "back", _mobile);
			up = WapTools.getPicto(this.Request, "up", _mobile);
			if (_idContentSet>0)
				atras = String.Format("http://emocion.kiwee.com/xhtml/catalog.aspx?cs={0}&cg={1}", _idContentSet.ToString(), _contentGroup);
			else
				atras = "./default.aspx";
			_mobile = null;
			#endregion
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
