using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class error : System.Web.UI.Page
	{
		protected XhtmlTableRow rowError, rowError2;
		protected XhtmlTable tbError, tbHeader;
		public string fondo, buscar,  emocion, back, up, css;

		private void Page_Load(object sender, System.EventArgs e)
		{
			MobileCaps _mobile = (MobileCaps)Request.Browser;
			try
			{
				WapTools.SetHeader(this.Context);
				WapTools.AddUIDatLog(Request, Response);
				WapTools.LogUser(this.Request, 103, _mobile.MobileType);					
			}
			catch{}
			
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
			_mobile = null;
			#endregion
			
			XhtmlTools.AddTextTableRow("IMG", rowError, "", WapTools.GetText("Error"), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);
			XhtmlTools.AddTextTableRow("", rowError2, "", WapTools.GetText("Error2"), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall);
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