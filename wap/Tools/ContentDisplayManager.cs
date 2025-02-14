using System;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;

namespace wap.Tools
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
			img.SoftkeyLabel = textDwld;

			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");				
				img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name));
				//img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name));
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
				//img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
			}
			else
			{
				if (_imgDisplayInst.UrlDwld != "")
					//img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
					img.NavigateUrl = String.Format(_imgDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
			}
			img.ImageUrl = String.Format(_imgDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);
                              
           
			System.Web.UI.MobileControls.Link lnk = WapTools.BuildLink(textDwld, img.NavigateUrl );
			lnk.Alignment = _imgDisplayInst.Alignment;
			if (preview)
				pnl.Controls.Add(img);

			//if( _imgDisplayInst.UrlPicto != "" )
			//	WapTools.AddPicto(_imgDisplayInst.Mobile, pnl, _imgDisplayInst.UrlPicto);
			if (_imgDisplayInst.UrlDwld != "")
				pnl.Controls.Add(lnk);
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
			lnk = WapTools.BuildLink(content.Name, String.Format(_contentSetDisplayInst.UrlDwld, WapTools.FindProperty(content.PropertyCollection, "IDComposite"), WapTools.FindProperty(content.PropertyCollection, "CompositeContentGroup")));

//			if ((content.Preview.URL != null) && (content.Preview.URL!=""))
//				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, content.Preview.URL);
//			else 
				if( _contentSetDisplayInst.UrlPicto != "" )
				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, _contentSetDisplayInst.UrlPicto);
			
			pnl.Controls.Add(lnk);
		}

		public void Display(System.Web.UI.MobileControls.Panel pnl, ContentSet contentSet)
		{
			System.Web.UI.MobileControls.Link lnk;
			lnk = WapTools.BuildLink(">>" + contentSet.Name, String.Format(_contentSetDisplayInst.UrlDwld, contentSet.IDContentSet, contentSet.ContentGroup));

			if( _contentSetDisplayInst.UrlPicto != "" )
				WapTools.AddPicto(_contentSetDisplayInst.Mobile, pnl, _contentSetDisplayInst.UrlPicto);
 
			pnl.Controls.Add(lnk);
		}
	}

	public class VideoDisplayInstructions
	{
		private string _textDwld;
		private string _urlDwld;
		private string _urlPicto;
		private bool _displayDescription;
		private string _previewMaskUrl;
		private MobileCaps _mobile;
		private Alignment _alignment;
    
		public VideoDisplayInstructions(MobileCaps mobile)
		{
			_textDwld = "Descargar";
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
			set { _alignment = Alignment; }
		}

	}

	public class VideoDisplay
	{
		private VideoDisplayInstructions _videoDisplayInst;

		public VideoDisplay( VideoDisplayInstructions videoDisplayInst )
		{
			_videoDisplayInst = videoDisplayInst;
		}

		public void Display(System.Web.UI.MobileControls.Panel pnl, Content content, bool preview)
		{
			string textDwld;
			string contentName = content.ContentName;
			if( _videoDisplayInst.DisplayDescription )
				textDwld = content.Name;
			else
				textDwld = _videoDisplayInst.TextDwld;
			if (!preview) textDwld = "-" + textDwld;  

			System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
			img.Alignment = _videoDisplayInst.Alignment;
			img.SoftkeyLabel = textDwld;
			if (content.ContentGroup.Name == "VIDEO_RGT")
			{
				_videoDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO_RGT");
				//img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, content.IDContent , WapTools.GetDefaultContentType(content.ContentGroup.Name));
				img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent , WapTools.GetDefaultContentType(content.ContentGroup.Name));
			}
			else if (content.ContentGroup.Name == "VIDEO")
			{
				_videoDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue("Url_VIDEO");
				//img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, content.IDContent , WapTools.GetDefaultContentType(content.ContentGroup.Name));
				img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent , WapTools.GetDefaultContentType(content.ContentGroup.Name));
			}
			else
				//img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
				img.NavigateUrl = String.Format(_videoDisplayInst.UrlDwld, WapTools.isBranded(content) ? "branded" : "", content.IDContent, WapTools.GetDefaultContentType(content.ContentGroup.Name) );
			img.ImageUrl = String.Format(_videoDisplayInst.PreviewMaskUrl, contentName.Substring(0, 1), contentName);


			System.Web.UI.MobileControls.Link lnk = WapTools.BuildLink(textDwld, img.NavigateUrl );

			lnk.Alignment = _videoDisplayInst.Alignment;
			if (preview)
				pnl.Controls.Add(img);

			pnl.Controls.Add(lnk); 
		}
	}
}
