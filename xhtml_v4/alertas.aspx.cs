using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class alertas : System.Web.UI.Page
	{
		protected xhtml.Tools.XhtmlTable tbHeader;
		protected XhtmlTable tbAlertas;
		public string buscar, emocion, back, up, fondo, css;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				MobileCaps _mobile = (MobileCaps)Request.Browser;
				try
				{
					WapTools.SetHeader(this.Context);
					WapTools.AddUIDatLog(Request, Response);
				}
				catch{}

				#region HEADER
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = WapTools.GetImage(this.Request, "apuntate",  _mobile.ScreenPixelsWidth);
				XhtmlTools.AddImgTable(tbHeader, img);
				#endregion
				
				XhtmlTools.AddLinkTable("", tbAlertas, WapTools.GetText("Alerta3"), WapTools.GetText("LnkShop23"), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbAlertas, WapTools.GetText("Alerta4"), WapTools.GetText("LnkShop24"), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				
				#region PICTOS
				fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
				buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
				emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
				back = WapTools.getPicto(this.Request, "back", _mobile);
				up = WapTools.getPicto(this.Request, "up", _mobile);
				_mobile = null;
				#endregion
			}
			catch(Exception caught)
			{
				WapTools.SendMail("alertas", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\alertas.aspx - UA : {0}", Request.UserAgent), caught);
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