using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using xhtml_v3.Tools;

namespace xhtml_v3
{
	public class muggins : System.Web.UI.Page
	{
		protected XhtmlTableRow rowTitle, rowPreviews;
		protected XhtmlTableCell cellPrev1, cellPrev2;
		protected xhtml_v3.Tools.XhtmlTable tbHeader, tbMuggins;
		public bool showForm1 = true, showForm2 = false, showForm3 = false;
		public bool uno = false,dos = false,tres = false,cuatro = false,cinco = false,seis = false,siete = false,ocho = false,nueve = false;
		public string answer, atras = "./default.aspx";
		public string buscar, emocion, back, up, fondo;
		protected xhtml_v3.Tools.XhtmlTable tbTitle;

		private void Page_Load(object sender, System.EventArgs e) 
		{
			try
			{
				MobileCaps _mobile =  (MobileCaps)Request.Browser;
				try
				{
					WapTools.SetHeader(this.Context);
					WapTools.AddUIDatLog(Request, Response);
				}
				catch{}

				#region HEADER
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = WapTools.GetImage(this.Request, "personalizados",  _mobile.ScreenPixelsWidth);
				XhtmlTools.AddImgTable(tbHeader, img);
				#endregion

				XhtmlTools.AddTextTableRow("IMG", rowTitle, "", WapTools.GetText("tumuggin").ToUpper(), Color.Empty, Color.Empty, (_mobile.ScreenPixelsWidth>140) ? 2 : 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);
				
				XhtmlImage preview = new XhtmlImage();
				preview.ImageUrl = "http://content.k-mobile.com/V2/DATA/IMG/M/MUGGINS_05_AG_WP118/PREVIEW/GIF/MUGGINS_05_AG_WP118.GIF_PREVIEW_IMODE.gif";
				XhtmlTableCell cell = new XhtmlTableCell();
				cell.Controls.Add(preview);
				cell.HorizontalAlign = HorizontalAlign.Center;
				rowPreviews.Cells.Add(cell);
				if (_mobile.ScreenPixelsWidth>140)
				{
					cell = new XhtmlTableCell();
					preview = new XhtmlImage();
					preview.ImageUrl = "http://content.k-mobile.com/V2/DATA/IMG/M/MUGGINS_49_AG_WP118/PREVIEW/GIF/MUGGINS_49_AG_WP118.GIF_PREVIEW_IMODE.gif";
					cell.Controls.Add(preview);
					cell.HorizontalAlign = HorizontalAlign.Center;
					rowPreviews.Cells.Add(cell);
				}
				cell = null;
				preview = null;
				 
				if (Request["sex"] != null && Request["Complementos"] == null)
				{
					rowPreviews.Visible = false;
					showForm1 = false;
					answer = Request["sex"] + Request["color"] + Request["style"];						
					if (Request["sex"] == "1") showForm2 = true;
					else showForm3 = true;
					CheckForm(answer);
					atras = "./muggins.aspx";
				}
				else if (Request["Complementos"] != null)
				{
					answer = WapTools.GetText("mug" + Request["a"]  + Request["complementos"]);
					string uri = String.Format(WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1") ? 1 : 0, "IMG", "IMG_COLOR", "MUGGIN", "", ""), "IMG_COLOR", answer);
					try{Response.Redirect(uri, false);}
					catch{Response.Redirect(uri, true);}
				}

				#region PICTOS
				fondo = WapTools.GetImage2(this.Request, "fondo", _mobile.ScreenPixelsWidth);
				buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
				emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
				back = WapTools.getPicto(this.Request, "back", _mobile);
				up = WapTools.getPicto(this.Request, "up", _mobile);
				_mobile = null;
				#endregion
			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\shops.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				Response.Redirect("./error.aspx");				
			}
		}

		private void CheckForm(string answer)
		{
			if (WapTools.GetText("mug" + answer + "1") != "") uno = true;
			if (WapTools.GetText("mug" + answer + "2") != "") dos = true;
			if (WapTools.GetText("mug" + answer + "3") != "") tres = true;
			if (WapTools.GetText("mug" + answer + "4") != "") cuatro = true;
			if (WapTools.GetText("mug" + answer + "5") != "") cinco = true;
			if (WapTools.GetText("mug" + answer + "6") != "") seis = true;
			if (WapTools.GetText("mug" + answer + "7") != "") siete = true;
			if (WapTools.GetText("mug" + answer + "8") != "") ocho = true;
			if (WapTools.GetText("mug" + answer + "9") != "") nueve = true;			
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