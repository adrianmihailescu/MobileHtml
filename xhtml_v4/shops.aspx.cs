using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using xhtml.Tools;

namespace xhtml
{
	public class shops : System.Web.UI.Page
	{
		protected XhtmlTableRow rowTitle;
		protected XhtmlTable tbHeader;
		protected XhtmlTable tbShops;
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
				img.ImageUrl = WapTools.GetImage(this.Request, "portales2",  _mobile.ScreenPixelsWidth);
				XhtmlTools.AddImgTable(tbHeader, img);
				#endregion

				#region PUB
				WapTools.CallPub(this.Request, WapTools.SearchType.ts_ImagenesFondos_masportales_banner_top.ToString(), "xml", tbShops); 
				#endregion  

				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop7"), "./linkto.aspx?id=7", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop2"), "./linkto.aspx?id=2", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop10"), "./linkto.aspx?id=10", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop12"), "./linkto.aspx?id=12", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop8"), "./linkto.aspx?id=8", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop3"), "./linkto.aspx?id=3", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				//XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop5"), "./linkto.aspx?id=5", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop9"), "./linkto.aspx?id=9", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
			//	XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop13"), "./linkto.aspx?id=13", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop16"), "./linkto.aspx?id=16", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
			//	XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop15"), "./linkto.aspx?id=15", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				XhtmlTools.AddLinkTable("", tbShops, WapTools.GetText("Shop17"), "./linkto.aspx?id=17", Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, false, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
				
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
				WapTools.SendMail("shops", Request.UserAgent, caught.ToString(), Request.ServerVariables);				
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\shops.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
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