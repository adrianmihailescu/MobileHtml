using System;
using System.Configuration;
using System.Web;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class catalog : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Form frmCatalog; 
		protected System.Web.UI.MobileControls.Panel pnlCatalog, pnlEnd;
		protected System.Web.UI.MobileControls.Panel pnlPreview;
		protected System.Web.UI.MobileControls.Image imgLogo;
		
		private void Page_Load(object sender, System.EventArgs e)
		{ 
			try
			{
				_mobile = (MobileCaps)Request.Browser;

				if (_mobile.PreferredRenderingType == "chtml10")
				{
					try{Response.Redirect(String.Format(ConfigurationSettings.AppSettings["UrlImodeCatalog"], Request["cg"]!=null ? Request["cg"] : "COMPOSITE", Request["cs"]), false);}
					catch{Response.Redirect(String.Format(ConfigurationSettings.AppSettings["UrlImodeCatalog"], Request["cg"]!=null ? Request["cg"] : "COMPOSITE", Request["cs"]), true);}
				} 
				else if (Convert.ToBoolean(ConfigurationSettings.AppSettings["Switch_Xhtml"]) && (_mobile.IsXHTML || WapTools.isXhtml(this.Request, _mobile)))
				{
					try{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlCatalog"] + Request.ServerVariables["QUERY_STRING"], false);}
					catch{Response.Redirect(ConfigurationSettings.AppSettings["UrlXhtmlCatalog"] + Request.ServerVariables["QUERY_STRING"], true);}
				} 
				else if (_mobile.MobileType != null && _mobile.IsCompatible("IMG_COLOR"))
				{
					_idContentSet = (Request.QueryString["cs"] != null) ? Convert.ToInt32(Request.QueryString["cs"]) : 0;
					int page = (Request.QueryString["n"] != null) ? Convert.ToInt32(Request.QueryString["n"]) : 1;
					_contentGroup = (Request.QueryString["cg"] != null) ? Request.QueryString["cg"].ToString() : "";
					_contentType = WapTools.GetDefaultContentType(_contentGroup);
					_displayKey = WapTools.GetXmlValue("DisplayKey");
									
					string paramBack = String.Format("a1=n&a2={0}&a3=cg&a4={1}&a5=cs&a6={2}",
						page, _contentGroup, _idContentSet);

					ContentSet contentSet = BrowseContentSetExtended();
					int nbPreview = (_mobile.IsAdvanced) ? 6 : 4;
					if (nbPreview > contentSet.ContentCount) nbPreview = contentSet.ContentCount;
					int nbPages = (contentSet.Count % nbPreview == 0) ? contentSet.Count / nbPreview : contentSet.Count / nbPreview + 1;
					if( _contentGroup == "COMPOSITE" )
					{
						nbPreview = (_mobile.IsAdvanced) ? 15 : 10;
						_contentSetDisplayInst = new ContentSetDisplayInstructions(_mobile);
						_contentSetDisplayInst.UrlPicto = WapTools.GetImage(this.Request, "bullet");
						if (_idContentSet != 3619 && _idContentSet.ToString() != WapTools.GetXmlValue("Home/IMG") && _idContentSet.ToString() != WapTools.GetXmlValue("Home/ANIM") && _idContentSet.ToString() != WapTools.GetXmlValue("Home/VIDEO"))
							_contentSetDisplayInst.UrlDwld = String.Format("./catalog.aspx?cs={0}&cg={1}&d={2}&p={3}&t={4}&{5}", "{0}", "{1}", Request.QueryString["d"], _idContentSet.ToString(), Server.UrlEncode(contentSet.Name), paramBack); 
						else
							_contentSetDisplayInst.UrlDwld = String.Format("./catalog.aspx?cs={0}&cg={1}&d={2}&{3}", "{0}", "{1}", Request.QueryString["d"], paramBack); 
					}
					else if (_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT" || _contentGroup == "") 
					{
						if (_contentGroup=="")
							_contentType = WapTools.GetDefaultContentType(_contentGroup);
						
						_videoDisplayInst = new VideoDisplayInstructions(_mobile);
						//_videoDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("WAP|CONTENTSET|{0}|{1}", contentSet.Name, page)), "", _idContentSet.ToString()); 
						_videoDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, 0, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("wap|CONTENTSET|{0}|{1}", contentSet.Name, page)), "", _idContentSet.ToString()); 
					}				
					else
					{
						_imgDisplayInst = new ImgDisplayInstructions(_mobile);
						_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", _contentGroup));
						//_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("{0}|CONTENTSET|{1}|{2}", _referer != "" ? _referer : "WAP", contentSet.Name, page)), "", _idContentSet.ToString()); 
						_imgDisplayInst.UrlDwld = WapTools.GetUrlBilling(this.Request, (Request.QueryString["d"] == "1") ? 1 : 0, _contentGroup, _contentType, HttpUtility.UrlEncode(String.Format("wap|CONTENTSET|{0}|{1}", contentSet.Name, page)), "", _idContentSet.ToString()); 
					}
 
					if (_contentGroup=="COMPOSITE")
						ReadContentSet(contentSet, pnlCatalog, page, nbPreview, true);
					else
					{
						bool noPreviews = WapTools.noPreview(contentSet.IDContentSet);
						if (noPreviews) nbPages=1;
						WapTools.AddLabelCenter(pnlCatalog, contentSet.Name + " (" + page.ToString() + "/" + nbPages.ToString() + ")", "", _mobile, BooleanOption.True);
						
						ReadContentSet(contentSet, pnlCatalog, page, nbPreview, !noPreviews);
					}
					string txtPrevious = WapTools.GetText("Previous");
					string txtNext = WapTools.GetText("Next"); 

					if(_hasNextPage)
						WapTools.AddLink(pnlCatalog, txtNext, String.Format("./catalog.aspx?cg={0}&cs={1}&d={2}&n={3}&p={4}&t={5}&{6}", _contentGroup, _idContentSet, Request.QueryString["d"], page + 1, Request.QueryString["p"], Server.UrlEncode(Request.QueryString["t"]), WapTools.GetParamBack(Request, false)), WapTools.GetImage(this.Request, "bullet"), _mobile);
					if(_hasPreviousPage)
						WapTools.AddLink(pnlCatalog, txtPrevious, String.Format("./catalog.aspx?cg={0}&cs={1}&d={2}&n={3}&p={4}&t={5}&{6}", _contentGroup, _idContentSet, Request.QueryString["d"], page - 1, Request.QueryString["p"], Server.UrlEncode(Request.QueryString["t"]), WapTools.GetParamBack(Request, false)), WapTools.GetImage(this.Request, "bullet"), _mobile);					

					#region LINKS
					try
					{
						WapTools.AddLabel(pnlCatalog, " ", "", _mobile);
						int isAlerta = WapTools.isAlerta(_idContentSet);
						if (isAlerta > 0)
							WapTools.AddLink(pnlCatalog, "Alerta de " + WapTools.GetText("Alerta" + isAlerta.ToString()), "./linkto.aspx?id=" + (20 + isAlerta).ToString(), WapTools.GetImage(this.Request, "bullet"), _mobile);
						
						if (Request["p"] != null && Request["t"] != null && Request["p"] != "" && Request["t"] != "")
							WapTools.AddLink(pnlCatalog, "Volver a " + Server.UrlDecode(Request.QueryString["t"]), "./catalog.aspx?cg=COMPOSITE&cs=" + Request.QueryString["p"], WapTools.GetImage(this.Request, "bullet"), _mobile);
						
						if (Request.QueryString["ms"]!=null && Request.QueryString["ms"]!="")
						{
							if (WapTools.GetText(Request.QueryString["ms"])!="")
								WapTools.AddLink(pnlCatalog, WapTools.GetText(Request.QueryString["ms"]), "./minisite.aspx?id=" + Request.QueryString["ms"], WapTools.GetImage(this.Request, "bullet"), _mobile);
						}
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/POSTALES"))
						{
							WapTools.AddLabelCenter(pnlCatalog, "DESCARGATE TUS", "", _mobile, BooleanOption.True);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("IMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("ANIM_COLOR"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (WapTools.isCompatibleThemes(_mobile))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							WapTools.AddLink(pnlCatalog, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
						}
						else if (Request.QueryString["d"]=="1")
							WapTools.AddLink(pnlCatalog, WapTools.GetText("MasPOSTALES"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}&d=1", WapTools.GetXmlValue("Home/POSTALES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/IMG") || _idContentSet.ToString() == "3619")
						{
							WapTools.AddLabelCenter(pnlCatalog, "DESCARGATE TUS", "", _mobile, BooleanOption.True);
							//WapTools.AddLink(pnlCatalog, WapTools.GetText("POSTALES"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}&d=1", WapTools.GetXmlValue("Home/POSTALES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("ANIM_COLOR"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (WapTools.isCompatibleThemes(_mobile))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							WapTools.AddLink(pnlCatalog, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
						}
						else if (_contentGroup == "IMG" || (_contentGroup == "COMPOSITE" && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"] != null && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"].Value.ToString()=="IMG")) 
							WapTools.AddLink(pnlCatalog, WapTools.GetText("MasIMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/ANIM"))
						{
							WapTools.AddLabelCenter(pnlCatalog, "DESCARGATE TUS", "", _mobile, BooleanOption.True);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("IMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							//WapTools.AddLink(pnlCatalog, WapTools.GetText("POSTALES"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}&d=1", WapTools.GetXmlValue("Home/POSTALES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (WapTools.isCompatibleThemes(_mobile))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							WapTools.AddLink(pnlCatalog, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
						}
						else if (_contentGroup == "ANIM" || (_contentGroup == "COMPOSITE" && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"] != null && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"].Value.ToString()=="ANIM")) 
							WapTools.AddLink(pnlCatalog, WapTools.GetText("MasANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						else if (_idContentSet.ToString() == WapTools.GetXmlValue("Home/VIDEO"))
						{
							WapTools.AddLabelCenter(pnlCatalog, "DESCARGATE TUS", "", _mobile, BooleanOption.True);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("IMG"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/IMG")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							//WapTools.AddLink(pnlCatalog, WapTools.GetText("POSTALES"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}&d=1", WapTools.GetXmlValue("Home/POSTALES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (_mobile.IsCompatible("ANIM_COLOR"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("ANIM"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIM")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							if (WapTools.isCompatibleThemes(_mobile))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("Temas"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/TEMAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							WapTools.AddLink(pnlCatalog, WapTools.GetText("AnimaNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/ANIMANOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);
							WapTools.AddLink(pnlCatalog, WapTools.GetText("FondoDedicatorias"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/FONDODEDICATORIAS")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoNombres"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEONOMBRES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
							if (_mobile.IsCompatible("VIDEO_DWL"))
								WapTools.AddLink(pnlCatalog, WapTools.GetText("VideoAnimaciones"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEOANIMACIONES")), WapTools.GetImage(this.Request, "bullet"), _mobile);							
						}
						
						else if (_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT" || (_contentGroup == "COMPOSITE" && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"].Value.ToString()=="VIDEO") || (_contentGroup == "COMPOSITE" && contentSet.ContentCollection[0].PropertyCollection["CompositeContentGroup"].Value.ToString()=="VIDEO_RGT")) 
							WapTools.AddLink(pnlCatalog, WapTools.GetText("MasVIDEO"), String.Format("./linkto.aspx?cg=COMPOSITE&id={0}", WapTools.GetXmlValue("Home/VIDEO")), WapTools.GetImage(this.Request, "bullet"), _mobile);
						WapTools.AddLinkCenter(pnlCatalog, WapTools.GetText("ImagenesFondos"), "./default.aspx", "", _mobile);
					}
					catch{}
					#endregion

					contentSet = null;
				}
				else
					WapTools.AddLabel(pnlCatalog, WapTools.GetText("Compatibility"), "", _mobile);
				string atras = WapTools.UpdateFooter(_mobile, this.Context, null); 				

				#region HEADER
				if (_mobile.IsAdvanced)
				{ 
					if (_idContentSet == 5745)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "top"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 628)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "novedades"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 3207 || _idContentSet.ToString() == WapTools.GetXmlValue("Home/ANIM"))
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "animaciones"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 3965 || _idContentSet.ToString() == WapTools.GetXmlValue("Home/VIDEO"))
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "videos"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 3619 || _idContentSet.ToString() == WapTools.GetXmlValue("Home/IMG"))
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "catimg"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 1159)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "topanim"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 1160)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "newsanim"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 3957)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "topvideos"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 3958)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "newsvideos"), WapTools.GetFolderImg(_mobile));
					else if (_idContentSet == 5775)
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "secciones"), WapTools.GetFolderImg(_mobile));
					else if (_contentGroup == "IMG")
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "catimg"), WapTools.GetFolderImg(_mobile));
					else if (_contentGroup == "ANIM")
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "catanim"), WapTools.GetFolderImg(_mobile));
					else if (_contentGroup == "VIDEO" || _contentGroup == "VIDEO_RGT" || _contentGroup == "")
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "catvideos"), WapTools.GetFolderImg(_mobile));
					else 
						imgLogo.ImageUrl = String.Format(WapTools.GetImage(this.Request, "imagenes"), WapTools.GetFolderImg(_mobile));
				}
				else pnlPreview.Visible = false;
				#endregion
					
				WapTools.AddLink(pnlEnd, "Buscar", "http://10.132.67.244/buscador2/searcher.initsearch.do",WapTools.GetImage(this.Request, "buscar"), _mobile);
				WapTools.AddLink(pnlEnd, "Home", "http://wap.movistar.com", WapTools.GetImage(this.Request, "home"), _mobile);
				WapTools.AddLink(pnlEnd, "Atrás", atras, WapTools.GetImage(this.Request, "back"), _mobile);
				WapTools.AddLink(pnlEnd, "Arriba", String.Format("catalog.aspx?{0}", Request.ServerVariables["QUERY_STRING"]), WapTools.GetImage(this.Request, "up"), _mobile);
				//Search
				//WapTools.AddLink(pnlCatalog, WapTools.GetText("SearchLink"), String.Format("./search.aspx?cg={0}", contentGroupDisplay), "", _mobile);
				//WapTools.AddLink(pnlCatalog, WapTools.GetText("Back"), "./default.aspx", WapTools.GetImage(this.Request, "bullet"), _mobile);
			}
			catch(Exception caught)
			{
				WapTools.SendMail("catalog", Request.UserAgent, caught.ToString(), Request.ServerVariables);
				Log.LogError(String.Format("Site emocion : Unexpected exception in emocion\\wap\\catalog.aspx  - UA : {0} - QueryString : {1}", Request.UserAgent, Request.ServerVariables["QUERY_STRING"]), caught);
				this.RedirectToMobilePage("./error.aspx");				
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
