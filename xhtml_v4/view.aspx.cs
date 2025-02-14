using System;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;
using KMobile.Catalog.Presentation;
using xhtml.Tools;

namespace xhtml
{
	public class view : XCatalogBrowsing
	{
		protected XhtmlTable tbTitle, tbLinks;
		protected XhtmlTableRow rowLinkPPD, rowPreview;
		protected Panel pnlFooter;
		protected string urlBillingPPD;
		protected XhtmlTable tbHeader, tbPreviews, tbContentset;
		private Content content = null;
		public string buscar, emocion, atras, up, fondo, back, css;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_mobile = (MobileCaps)Request.Browser;
				WapTools.SetHeader(this.Context);
				WapTools.AddUIDatLog(Request, Response);
				
				int idContent = (Request.QueryString["c"] != null) ? Convert.ToInt32(Request.QueryString["c"]) : 0;
				_idContentSet = (Request.QueryString["cs"] != null && Request.QueryString["cs"] != "") ? Convert.ToInt32(Request.QueryString["cs"]) : 0;
				_displayKey = WapTools.GetXmlValue("DisplayKey");
				string referer = (Request.QueryString["ref"] != null) ? Request.QueryString["ref"].ToString() : "";
				try{content = StaticCatalogService.GetContentInfos(_displayKey, idContent, _mobile.MobileType, _contentType);}
				catch {content = null;} 
				
				if (content != null)
				{
					_contentGroup = content.ContentGroup.Name;
					_contentType = WapTools.GetDefaultContentType(_contentGroup);
					
					XhtmlTableRow row = new XhtmlTableRow();
					XhtmlTools.AddTextTableRow("IMG", row, "", content.Name.ToUpper(), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);
					tbTitle.Controls.Add(row); 
					row = null; 

					XhtmlImage img = new XhtmlImage();
					img.ImageAlign = ImageAlign.Middle;
					if (content.Preview.URL != null)
						img.ImageUrl = content.Preview.URL; 
					else
						img.ImageUrl = String.Format(WapTools.GetXmlValue("Url_" + _contentGroup), content.ContentName.Substring(0,1), content.ContentName);						
					XhtmlTools.AddImgTableRow(rowPreview, img);
					img = null;
					//rowPreview.Visible = false;

					urlBillingPPD = String.Format(WapTools.GetUrlBilling(this.Request, 0, _contentGroup, _contentType, HttpUtility.UrlEncode("xhtml|" + referer), null, _idContentSet.ToString()),  WapTools.isBranded(content) ? "branded" : "", _contentType, content.IDContent);
					if (_mobile.IsCompatible(_contentType))
						XhtmlTools.AddLinkTableRow(_contentGroup, rowLinkPPD, Server.HtmlEncode(WapTools.GetText("BillingPPD")), urlBillingPPD, Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
					else
						XhtmlTools.AddTextTableRow(_contentGroup, rowLinkPPD, WapTools.GetImage(this.Request, "bullet"), Server.HtmlEncode(WapTools.GetText("Compatibility")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall);
				} 

				#region LINKS
				try
				{
					if (_mobile.IsCompatible(_contentType))
					{
						if (_contentGroup == "IMG")
							XhtmlTools.AddLinkTable(_contentGroup, tbLinks, WapTools.GetText("MasIMG"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/IMG")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						else if (_contentGroup == "ANIM")
							XhtmlTools.AddLinkTable(_contentGroup, tbLinks, WapTools.GetText("MasANIM"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/ANIM")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
						else if (_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT")
							XhtmlTools.AddLinkTable(_contentGroup, tbLinks, WapTools.GetText("MasVIDEO"), String.Format("./catalog.aspx?cg=COMPOSITE&cs={0}", WapTools.GetXmlValue("Home/VIDEO")), Color.Empty, Color.Empty, 1, HorizontalAlign.Left, VerticalAlign.Middle, true, FontUnit.XXSmall, WapTools.GetImage(this.Request, "bullet"));
					}
				}
				catch{}
				#endregion
				
				#region HEADER
				XhtmlImage img2 = new XhtmlImage();
				img2.ImageUrl = WapTools.GetImage(this.Request, "imagenes",  _mobile.ScreenPixelsWidth);
				XhtmlTools.AddImgTable(tbHeader, img2);
				#endregion
                                     
				#region PICTOS
				fondo = WapTools.GetImage(this.Request, "fondo", _mobile.ScreenPixelsWidth);
				buscar = WapTools.getPicto(this.Request, "buscar", _mobile);
				emocion = WapTools.getPicto(this.Request, "emocion", _mobile);
				back = WapTools.getPicto(this.Request, "back", _mobile);
				up = WapTools.getPicto(this.Request, "up", _mobile);
				_mobile = null;
				#endregion

				// Search
				atras = WapTools.UpdateFooter(_mobile, this.Context, null); 
				content = null;
			}
			catch(Exception caught)
			{
				WapTools.SendMail("view", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\xhtml\\view.aspx - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
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
