using System;
using System.Web;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;

namespace xhtml.Tools
{
	public class ImgDisplayInstructions
	{
		private string _textDwld;
		private string _urlDwld;
		private string _urlPicto;
		private bool _displayDescription;
		private string _previewMaskUrl;
		private MobileCaps _mobile;
		private Alignment _alignment;
    
		public ImgDisplayInstructions(MobileCaps mobile)
		{
			_textDwld = "Télécharger";
			_urlDwld = "";
			_urlPicto = "";
			_displayDescription = true;
			_previewMaskUrl = "";
			_mobile = mobile;
			_alignment = Alignment.Center;
		}

		public string TextDwld
		{
			get { return _textDwld; }
			set { _textDwld = value; }
		}
		
		public string UrlDwld
		{
			get { return _urlDwld; }
			set { _urlDwld = value; }
		}

		public string UrlPicto
		{
			get { return _urlPicto; }
			set { _urlPicto = value; }
		}

		public bool DisplayDescription
		{
			get { return _displayDescription; }
			set { _displayDescription = value; }
		}

		public string PreviewMaskUrl
		{
			get { return _previewMaskUrl; }
			set { _previewMaskUrl = value; }
		}

		public MobileCaps Mobile
		{
			get { return _mobile; }
			set { _mobile = value; }
		}

		public Alignment Alignment
		{
			get { return _alignment; }
			set { _alignment = value; }
		}

	}

	public class ImgDisplay
	{
		private ImgDisplayInstructions _imgDisplayInst;

		public ImgDisplay( ImgDisplayInstructions imgDisplayInst )
		{
			_imgDisplayInst = imgDisplayInst;
		}

		public void Display(System.Web.UI.MobileControls.Panel pnl, Content content, bool preview)
		{
			string textDwld;
			string contentName = content.ContentName;
			if( _imgDisplayInst.DisplayDescription )
				textDwld = content.Name;
			else
				textDwld = _imgDisplayInst.TextDwld;
			if (!preview) textDwld = "-" + textDwld;
			System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
			img.Alignment = _imgDisplayInst.Alignment;

			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");
				img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else
				img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent );
			img.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
                              
           
			System.Web.UI.MobileControls.Link lnk = WapTools.BuildLink(textDwld, img.NavigateUrl );
			lnk.Alignment = _imgDisplayInst.Alignment;
				
			if (preview)
				pnl.Controls.Add(img);

			//if( _imgDisplayInst.UrlPicto != "" )
			//	WapTools.AddPicto(_imgDisplayInst.Mobile, pnl, _imgDisplayInst.UrlPicto);

			pnl.Controls.Add(lnk);
		}

		public void Display(System.Web.UI.WebControls.TableCell cell, Content content, bool preview)
		{
			string contentName = content.ContentName;
			string textDwld;
			if( _imgDisplayInst.DisplayDescription )
				textDwld = content.Name;
			else
				textDwld = _imgDisplayInst.TextDwld;

			XhtmlLink linkImage= new XhtmlLink();
			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
				//linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
			linkImage.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
           	linkImage.CssClass = "imagen";                   
                              
			//linkImage.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);

			//linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
			//                              if (content.ContentGroup.Name == "VIDEO_RGT" || content.ContentGroup.Name == "VIDEO" )
			//                                        linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			//                              else
			//                                        linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
                              
			XhtmlLink lnk = WapTools.BuildLink2(HttpUtility.HtmlEncode(textDwld), linkImage.NavigateUrl);
			lnk.CssClass = content.ContentGroup.Name;
			if (preview) cell.Controls.Add(linkImage);

			if( _imgDisplayInst.UrlPicto != "" )
				XhtmlTools.AddPicto(_imgDisplayInst.Mobile, cell, _imgDisplayInst.UrlPicto);

			cell.Controls.Add(lnk);
		}

		public void Display(System.Web.UI.WebControls.TableCell cell, System.Web.UI.WebControls.TableCell cellTitle, Content content)
		{
			string contentName = content.ContentName;
			string textDwld;
			if( _imgDisplayInst.DisplayDescription )
				textDwld = "> " + content.Name;
			else
				textDwld = _imgDisplayInst.TextDwld;

			XhtmlLink linkImage= new XhtmlLink();
			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else
				linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			//linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
			//linkImage.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
            linkImage.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
			//linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
			//                              if (content.ContentGroup.Name == "VIDEO_RGT" || content.ContentGroup.Name == "VIDEO" )
			//                                        linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			//                              else
			//                                        linkImage.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
                              
			XhtmlLink lnk = WapTools.BuildLink2(HttpUtility.HtmlEncode(textDwld), linkImage.NavigateUrl);
			linkImage.CssClass = "imagen";   
			cell.Controls.Add(linkImage);

			//if( _imgDisplayInst.UrlPicto != "" )
			//	WapTools.AddPicto(_imgDisplayInst.Mobile, cell, _imgDisplayInst.UrlPicto);

			cellTitle.Controls.Add(lnk);
		}

		public void Display(System.Web.UI.WebControls.TableCell cell, Content content)
		{                        
			string contentName = content.ContentName;
			XhtmlLink lnk = new XhtmlLink();
			lnk.CssClass = content.ContentGroup.Name;
			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");
				lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			}
			else
				lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			//lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent ); 
			lnk.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
			lnk.CssClass = "imagen";   
			
                              
			//lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent );
			//lnk.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent );
			cell.Controls.Add(lnk);
		}	

		public void DisplayX(XhtmlTableCell cell, Content content)
		{
			XhtmlLink lnk = new XhtmlLink();
			lnk = WapTools.BuildLink2("-" + content.Name, String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.GetDefaultContentType(content.ContentGroup.Name), content.IDContent));
			lnk.CssClass = content.ContentGroup.Name;
			if( _imgDisplayInst.UrlPicto != "" )
				XhtmlTools.AddPicto(_imgDisplayInst.Mobile, cell, _imgDisplayInst.UrlPicto);

			cell.Controls.Add(lnk);
		}
	}

	public class ContentSetDisplayInstructions
	{
		private string _urlDwld;
		private string _urlPicto;
		private MobileCaps _mobile;
		private Alignment _alignment;

		public ContentSetDisplayInstructions(MobileCaps mobile)
		{
			_urlDwld = "";
			_urlPicto = "";
			_mobile = mobile;
			_alignment = Alignment.Left;
		}

		public string UrlDwld
		{
			get { return _urlDwld; }
			set { _urlDwld = value; }
		}

		public string UrlPicto
		{
			get { return _urlPicto; }
			set { _urlPicto = value; }
		}

		public MobileCaps Mobile
		{
			get { return _mobile; }
			set { _mobile = value; }
		}

		public Alignment Alignment
		{
			get { return _alignment; }
			set { _alignment = Alignment; }
		}

	}

	public class ContentSetDisplay
	{
		private ContentSetDisplayInstructions _contentSetDisplayInst;

		public ContentSetDisplay( ContentSetDisplayInstructions contentSetDisplayInst )
		{
			_contentSetDisplayInst = contentSetDisplayInst;
		}

		public void Display(System.Web.UI.MobileControls.Panel pnl, Content content)
		{
			System.Web.UI.MobileControls.Link lnk;
			lnk = WapTools.BuildLink(content.Name, String.Format(_contentSetDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", WapTools.FindProperty(content.PropertyCollection, "IDComposite"), WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup")));

			if ((content.Preview.URL != null) && (content.Preview.URL!=""))
				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, content.Preview.URL);
			else 
				if( _contentSetDisplayInst.UrlPicto != "" )
				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, _contentSetDisplayInst.UrlPicto);
			
			pnl.Controls.Add(lnk);
		}

		public void Display(System.Web.UI.MobileControls.Panel pnl, ContentSet contentSet)
		{
			System.Web.UI.MobileControls.Link lnk;
			lnk = WapTools.BuildLink(contentSet.Name, String.Format(_contentSetDisplayInst.UrlDwld, contentSet.IDContentSet, contentSet.ContentGroup));

			if( _contentSetDisplayInst.UrlPicto != "" )
				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, _contentSetDisplayInst.UrlPicto);

			pnl.Controls.Add(lnk);
		}

		public void Display(XhtmlTableCell cell, Content content, bool preview)
		{
			XhtmlLink lnk= new XhtmlLink();
			string contentGroup = WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup");
			// if( contentGroup == "" ) contentGroup = "COMPOSITE";
			string name = content.Name;
			lnk = WapTools.BuildLink2(HttpUtility.HtmlEncode(name), String.Format(_contentSetDisplayInst.UrlDwld, WapTools.FindProperty(content.PropertyCollection, "IDComposite"), contentGroup));
			//lnk.CssClass = (contentGroup != "COMPOSITE") ? contentGroup : "SOUND";
//			if (name.IndexOf("Top") >= 0 || name.IndexOf("Novedades") >= 0 || name.IndexOf("Más ") >= 0 )
//				lnk.CssClass = "IMG2";
//			else
				lnk.CssClass = "IMG";
			if (preview)
			{
//				if ((content.Preview.URL != null) && (content.Preview.URL!=""))
//					XhtmlTools.AddPicto(_contentSetDisplayInst.Mobile, cell, content.Preview.URL);
//				else 
					if( _contentSetDisplayInst.UrlPicto != "" )
					XhtmlTools.AddPicto(_contentSetDisplayInst.Mobile, cell, _contentSetDisplayInst.UrlPicto);
			}
			cell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
			cell.Controls.Add(lnk);
		}


		public void Display(XhtmlTable t, Content content, string CssClass)
		{
			XhtmlTableCell cell = new XhtmlTableCell();
			XhtmlTableRow row = new XhtmlTableRow();
			XhtmlLink lnk= new XhtmlLink();
			string contentGroup = WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup");
			if( contentGroup == "" ) contentGroup = "COMPOSITE";
			lnk = WapTools.BuildLink2(HttpUtility.HtmlEncode(content.Name), String.Format(_contentSetDisplayInst.UrlDwld, WapTools.FindProperty(content.PropertyCollection, "IDComposite"), contentGroup));
			if (CssClass!="")
				lnk.CssClass =  CssClass;
			if ((content.Preview.URL != null) && (content.Preview.URL!=""))
				XhtmlTools.AddPicto(_contentSetDisplayInst.Mobile, cell, content.Preview.URL);
			else 
				if( _contentSetDisplayInst.UrlPicto != "" )
				XhtmlTools.AddPicto(_contentSetDisplayInst.Mobile, cell, _contentSetDisplayInst.UrlPicto);
			cell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
			//                              row.Controls.Add(cell);
			//                              cell = new XhtmlTableCell();
			cell.Controls.Add(lnk);
			cell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
			row.Controls.Add(cell);
			t.Controls.Add(row);
		}

		public void Display(System.Web.UI.WebControls.Panel pnl, ContentSet contentSet)
		{
			System.Web.UI.WebControls.HyperLink lnk;
			lnk = WapTools.BuildLink2(contentSet.Name, String.Format(_contentSetDisplayInst.UrlDwld, contentSet.IDContentSet));

			if( _contentSetDisplayInst.UrlPicto != "" )
				XhtmlTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, _contentSetDisplayInst.UrlPicto);

			pnl.Controls.Add(lnk);
		}
	}
}
