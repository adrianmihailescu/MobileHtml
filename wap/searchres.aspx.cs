using System;
using System.Web;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Presentation.Search;
using KMobile.Catalog.Services;
using wap.Tools;

namespace wap
{
	public class searchres : CatalogBrowsing
	{
		protected System.Web.UI.MobileControls.Form frmSearch;
		protected System.Web.UI.MobileControls.Panel pnlSearch;

		private string _paramBack = "";
		int _page;

		private void Page_Load(object sender, System.EventArgs e)
		{
			/// Keyword ?
			///		|----Y---- IsNumeric ?
			///		|							|--------Y----> Cas 1 : SearchByShortcode()
			///		|							|--------N----- ContentGroup, ContentType ?
			///		|																					|---------------Y----> Cas 2 : BrowseExaSearch()
			///		|																					|---------------N----> Cas 3 : BrowseExaSearchExt()
			///		|----N---- Context, HrefParam ?
			///											|--------------Y----> Cas 4 : BrowseExaSearchRefine()
			///											|--------------N----> Cas 5 : TextBox

			try
			{
				_mobile = (MobileCaps)Request.Browser;
				
				_contentType = (Request["ct"] != null) ? Request["ct"].ToString() : (Request["__V_ct"] != null ? Request["__V_ct"].ToString() : "");
				_contentGroup = (Request["cg"] != null) ? Request["cg"].ToString() : (Request["__V_cg"] != null ? Request["__V_cg"].ToString() : "");
				_keyword = (Request["txtSearch"] != null) ? Request["txtSearch"].ToString() : null;
				_page = (Request["n"] != null) ? Convert.ToInt32(Request["n"]) : 1;
				string context = (Request["ctxt"] != null ? Request["ctxt"].ToString() : Session["ctxt"]!=null ? Session["ctxt"].ToString() : null);
				string hrefParam = (Request["h"] != null ? Request["h"].ToString() : null);
				bool forRefine = (Request["r"] != null);
				string result = "";

				_displayKey = WapTools.GetXmlValue("DisplayKey");
				//							string _paramBack = String.Format("a0=Search&a1=txtSearch&a2={0}&a3=cg&a4={1}&a5=ct&a6={2}&a7=n&a8={3}",
				//								_keyword, _contentGroup, _contentType, page);

				bool newSearch = true;	//Doit-on afficher le lien Nouvelle recherche ?
				bool newContentType = true; //Doit-on afficher le lien Autre type de contenu ?

				if( _keyword != null && !forRefine)
				{
					bool isNumeric = false;
					try
					{	isNumeric = (_keyword.Length == 7 && Convert.ToInt32(_keyword) > 0); }
					catch
					{}

					if( _keyword.Length < 3 )
					{
						WapTools.AddLabel(pnlSearch, WapTools.GetText("SearchError"), "", _mobile);
						newContentType = false;
					}
					else
					{
						if( isNumeric )
						{
							//Cas 1
							newContentType = false;

							CatalogAPI catalogAPI = new CatalogAPI(_displayKey, null, null, _mobile.MobileType, _keyword);
							try
							{
								Content content = catalogAPI.SearchByShortcode();								
								_contentGroup = content.ContentGroup.Name;
								InitContentDisplay();
								DisplayContent(content, pnlSearch, true);
							}
							catch
							{
								WapTools.AddLabel(pnlSearch, WapTools.GetXmlValue("Texts/data[@name='NoResult']"), "", _mobile);
								newContentType = false;
							}
						}
						else
						{
							if( _contentGroup == "" )
							{
								//Cas 3
								bool hasResult = false;

								newContentType = false;
								SearchResult searchResult = BrowseExaSearchExt();

								foreach(CriteriaGroup criteriaGroup in searchResult.CriteriaHits)
									if( criteriaGroup.Name == "ContentGroup" )
									{
										foreach(Criteria criteria in criteriaGroup.Criterias)
											if( WapTools.GetText(String.Format("Search_{0}", criteria.Name)) != "" && _mobile.IsCompatible(WapTools.GetDefaultContentType(criteria.Name)) )
											{
												//WapTools.AddLink(pnlSearch, String.Format("{0} {1}", criteria.Count, WapTools.GetText(String.Format("Search_{0}", criteria.Name))), String.Format("searchres.aspx?ctxt={0}&h={1}&cg={2}&r=1&txtSearch={3}", HttpUtility.UrlEncode(searchResult.Context), HttpUtility.UrlEncode(criteria.Refine), criteria.Name,  HttpUtility.UrlEncode(_keyword)), hasWidth ? WapTools.GetImage("Picto") : "", _mobile);
												WapTools.AddLink(pnlSearch, String.Format("{0} {1}", criteria.Count, WapTools.GetText(String.Format("Search_{0}", criteria.Name))), String.Format("searchres.aspx?h={0}&cg={1}&r=1&txtSearch={2}", HttpUtility.UrlEncode(criteria.Refine), criteria.Name,  HttpUtility.UrlEncode(_keyword)), "", _mobile);
												hasResult = true;
											}

										break;
									}

								if( !hasResult )
									WapTools.AddLabel(pnlSearch, WapTools.GetXmlValue("Texts/data[@name='NoResult']"), "", _mobile);
								else
									Session.Add("ctxt", HttpUtility.UrlEncode(searchResult.Context));
							}
							else
							{
								//Cas 2
								InitContentDisplay();
								ContentSet contentSet = BrowseExaSearch(pnlSearch, (_page - 1) * Convert.ToInt32(WapTools.GetXmlValue("Catalog/NbSearch")), Convert.ToInt32(WapTools.GetXmlValue("Catalog/NbSearch")), true);
								result = String.Format(" ({0})", contentSet.ContentCollection.Count);
								DisplayPreviousNext(contentSet, String.Format("txtSearch={0}&cg={1}&ct={2}",  HttpUtility.UrlEncode(_keyword), _contentGroup, _contentType));
							}

						}
					}
				}
				else
				{
					if( context != null )
					{
						//Cas 4
						InitContentDisplay();
						SearchResult sr = BrowseExaSearchRefine( pnlSearch, context, hrefParam, (_page - 1) * Convert.ToInt32(WapTools.GetXmlValue("Catalog/NbSearch")), Convert.ToInt32(WapTools.GetXmlValue("Catalog/NbSearch")) );
						result = String.Format(" ({0})", sr.TotalHits);
						DisplayPreviousNext(sr.ContentSet, String.Format("h={0}&cg={1}&r=1&txtSearch={2}", HttpUtility.UrlEncode(hrefParam), _contentGroup,  HttpUtility.UrlEncode(_keyword)));
						//DisplayPreviousNext(sr.ContentSet, String.Format("ctxt={0}&h={1}&cg={2}&r=1&txtSearch={3}", HttpUtility.UrlEncode(context), HttpUtility.UrlEncode(hrefParam), _contentGroup,  HttpUtility.UrlEncode(_keyword)));
					}
					else
					{
						//Cas 5
						newSearch = false;
						newContentType = false;

						WapTools.AddSearchBlock(this, pnlSearch, WapTools.GetText("SearchCmd"), WapTools.GetText("SearchLbl"), 
							"", "", _contentGroup, _contentType, _mobile);

					}
				}
				
				if( newContentType )	WapTools.AddLink(pnlSearch, WapTools.GetText("NewContentType"), String.Format("searchres.aspx?txtSearch={0}",  HttpUtility.UrlEncode(_keyword)), "", _mobile);
				if( newSearch )	WapTools.AddLink(pnlSearch, WapTools.GetText("NewSearch"), "search.aspx", "", _mobile);

				//WapTools.AddLabel(pnlSearch, this.ActiveForm.Action, "", _mobile);

				WapTools.AddLink(pnlSearch, WapTools.GetText("Back"), "./default.aspx", "", _mobile);				

			}
			catch(Exception caught)
			{
				Log.LogError(String.Format("Site KiweeImagenes : Unexpected exception in KiweeImagenes\\searchres.aspx - UA : {0}", Request.UserAgent), caught);
				this.RedirectToMobilePage("./error.aspx");	
			}
		}

		private void InitContentDisplay()
		{
			//Graphic Instructions
			_imgDisplayInst = new ImgDisplayInstructions(_mobile);
			_imgDisplayInst.PreviewMaskUrl = WapTools.GetXmlValue(String.Format("Url_{0}", _contentGroup));
			_imgDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, _contentGroup, WapTools.GetDefaultContentType(_contentGroup), HttpUtility.UrlEncode(String.Format("WAP|SEARCH|{0}|{1}", _keyword, _page)), "", "");

			//Video Instructions
			_videoDisplayInst = new VideoDisplayInstructions(_mobile);
			_videoDisplayInst.UrlDwld = WapTools.GetUrlXView(this.Request, _contentGroup, WapTools.GetDefaultContentType(_contentGroup), HttpUtility.UrlEncode(String.Format("WAP|SEARCH|{0}|{1}", _keyword, _page)), "", "");

		}

		private void DisplayPreviousNext(ContentSet contentSet, string paramUrl)
		{
			if( contentSet.ContentCollection.Count == 0 )
				WapTools.AddLabel(pnlSearch, WapTools.GetText("NoResult"), "", _mobile);
			else
			{
				string txtPrevious = WapTools.GetText("Previous");
				string txtNext = WapTools.GetText("Next");
				if(_hasPreviousPage)
					WapTools.AddLink(pnlSearch, txtPrevious, String.Format("searchres.aspx?n={0}&{1}", _page - 1, paramUrl), "", _mobile);
				if(_hasNextPage)
					WapTools.AddLink(pnlSearch, txtNext, String.Format("searchres.aspx?n={0}&{1}", _page + 1, paramUrl), "", _mobile);
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

	class Task
	{
		private String _TaskName;
		private String _TaskLink;

		public Task(String TaskName, String TaskLink) 
		{ 
			_TaskName = TaskName; 
			_TaskLink = TaskLink;
		}

		public String TaskName { get { return _TaskName; } }
		public String TaskLink { get { return _TaskLink; } }
	}

}
