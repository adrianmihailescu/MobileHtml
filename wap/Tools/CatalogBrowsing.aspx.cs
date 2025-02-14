using System;
using System.Collections;
using System.Web.UI.MobileControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Presentation.Search;
using KMobile.Catalog.Services;

namespace wap.Tools
{
	public class CatalogBrowsing : System.Web.UI.MobileControls.MobilePage
	{
		protected string _displayKey;
		protected string _contentGroup;
		protected string _contentType;
		protected int _idContentSet;
		protected int _idContent;
		protected string _keyword;
		protected MobileCaps _mobile;
		protected ImgDisplayInstructions _imgDisplayInst = null;		
		protected ContentSetDisplayInstructions _contentSetDisplayInst = null;
		protected ContentSetDisplayInstructions _compositeDisplayInst = null;
		protected VideoDisplayInstructions _videoDisplayInst = null;

		protected bool _hasNextPage;
		protected System.Web.UI.MobileControls.Form Form1;
		protected bool _hasPreviousPage;

		private void Page_Load(object sender, System.EventArgs e)
		{}
		
		#region ContentSet

		protected ContentSet BrowseContentSetExtended( Panel pnl, int page, int nbItems )
		{
			return BrowseContentSetExtended( pnl, page, nbItems, true, true );
		}

		protected ContentSet BrowseContentSetExtended()
		{
			return BrowseContentSetExtended(null, 0, 0, false, true);
		}

		protected ContentSet BrowseContentSetExtended(int idContent)
		{
			return BrowseContentSetExtended(null, 0, 0, false, true, idContent);
		}

		private ContentSet BrowseContentSetExtended( Panel pnl, int page, int nbItems, bool display, bool preview )
		{
			Trace.Write (this._displayKey + "-" + _idContentSet.ToString() + "-" + _contentGroup + "-" + _contentType + "-" + _mobile.MobileType);
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _idContentSet, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType);
			ContentSet contentSet = catalogAPI.GetContentsExtended();
			
			if( display )
				return ReadContentSet( contentSet, pnl, page, nbItems, preview );
			else
				return contentSet;
		}

		private ContentSet BrowseContentSetExtended( Panel pnl, int page, int nbItems, bool display, bool preview, int idContent )
		{
			Trace.Write (this._displayKey + "-" + _idContentSet.ToString() + "-" + _contentGroup + "-" + _contentType + "-" + _mobile.MobileType + "-" + idContent.ToString());
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _idContentSet, _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, idContent);
			ContentSet contentSet = catalogAPI.GetContentsExtendedExcluded();

			if( display )
				return ReadContentSet( contentSet, pnl, page, nbItems, preview );
			else
				return contentSet;
		}

		protected ContentSet BrowseContentSet( Panel pnl, int page, int nbItems, bool preview )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _idContentSet, _contentGroup, _contentType, _mobile.MobileType);
			ContentSet contentSet = catalogAPI.GetContents();

			return ReadContentSet( contentSet, pnl, page, nbItems, preview );
		}

		protected ContentSet BrowseSearch( Panel pnl, int page, int nbItems, bool preview  )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			ContentSet contentSet = catalogAPI.Search();

			return ReadContentSet( contentSet, pnl, page, nbItems, preview );
		}

		protected ContentSet BrowseExaSearch( Panel pnl, int startElement, int nbItems, bool preview )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			ContentSet contentSet = catalogAPI.ExaSearch(startElement);

			return ReadContentSet( contentSet, pnl, 1, nbItems, preview );
		}

		protected ContentSet ReadContentSet( ContentSet contentSet, Panel pnl, int page, int nbItems, bool preview, bool aleatoire )
		{
			int index;
			if( page == -1 ) page = 1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;
			_hasNextPage = true;
			_hasPreviousPage = (page > 1);
			if (WapTools.noPreview(contentSet.IDContentSet))
			{
				preview = false;
				nbItems = contentSet.ContentCollection.Count;
			}
					
			if (contentSet.ContentCollection.Count>0)
			{
				index = (DateTime.Now.Day % contentSet.ContentCollection.Count) + ((page - 1) * nbItems);

				for(int i = index; i < index + nbItems; i++)
				{
					if (DateTime.Now.Day > contentSet.ContentCollection.Count)
					{
						if((i % contentSet.ContentCollection.Count == (DateTime.Now.Day-1) % contentSet.ContentCollection.Count)) // && (page > 1)) 
							_hasNextPage = false;
						if((i % contentSet.ContentCollection.Count == DateTime.Now.Day % contentSet.ContentCollection.Count) && (page > 1)) 
							break;
					}
					else
					{
						if((i % contentSet.ContentCollection.Count == (DateTime.Now.Day-1))) // && (page > 1)) 
							_hasNextPage = false;
						if((i % contentSet.ContentCollection.Count == DateTime.Now.Day) && (page > 1)) 
							break;
					}
					DisplayContent(contentSet.ContentCollection[i % contentSet.ContentCollection.Count], pnl, preview);
				}
			}
			else
				_hasNextPage = false;
			return contentSet;
		}
		
		
		protected ContentSet ReadContentSet( ContentSet contentSet, Panel pnl, int page, int nbItems, bool preview )
		{
			if( page == -1 ) page = 1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;
			if (!preview)
			{
				preview = false;
				nbItems = contentSet.ContentCollection.Count;
			}
			//de [(page - 1) * nbitems] à [(page * nbitems) - 1]
			int index = (page - 1) * nbItems;
			if( contentSet.ContentCollection.Count > index )
			{
				for(int i = index; i < index + nbItems; i++)
				{
					if(contentSet.ContentCollection.Count <= i) break;
					DisplayContent(contentSet.ContentCollection[i], pnl, preview);
				}
			}

			_hasPreviousPage = (page > 1);
			_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);

			return contentSet;
		}

		#endregion ContentSet

		#region Content
		protected Content GetContent()
		{
			CatalogAPI catalogAPI = new CatalogAPI(_idContent, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _displayKey);
			Content content = catalogAPI.GetContent();
			return content;
		}

		#endregion

		#region CatalogDisplay
		protected CatalogDisplay BrowseCategories( Panel pnl, int page, int nbItems )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup, _contentType, _mobile.MobileType);
			CatalogDisplay catalogDisplay = catalogAPI.GetAllCategories();

			return ReadCatalogDisplay(catalogDisplay, pnl, page, nbItems, ContentSetPriority.All);
		}

		protected CatalogDisplay BrowseThemas( Panel pnl, int page, int nbItems, ContentSetPriority csPriority )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup, _contentType, _mobile.MobileType);
			CatalogDisplay catalogDisplay = catalogAPI.GetAllThemas();

			return ReadCatalogDisplay(catalogDisplay, pnl, page, nbItems, csPriority);
		}

		private CatalogDisplay ReadCatalogDisplay( CatalogDisplay catalogDisplay, Panel pnl, int page, int nbItems, ContentSetPriority csPriority )
		{
			//Parcours de la liste pour retirer les priorités -2 et passer devant les -1
			SortedList slCatalogDisplay = new SortedList();

			int supRange = 100;
			int infRange = -1;
			if( csPriority == ContentSetPriority.NewThemas )
			{
				supRange = -3;
				infRange = -3;
			}
			else
			{
				if( csPriority == ContentSetPriority.Top )
					supRange = -1;
				else
					if( csPriority == ContentSetPriority.Exclu )
					infRange = 0;
			}

			foreach( ContentSet contentSet in catalogDisplay.ContentSetCollection )
				if( contentSet.Priority >= infRange && contentSet.Priority <= supRange )
				{
					Key key = new Key(contentSet.Priority, contentSet.Name);
					slCatalogDisplay.Add(key, contentSet);
				}

			if( page == -1 ) page = 1;
			if( nbItems == -1 ) nbItems = slCatalogDisplay.Count;

			//de [(page - 1) * nbitems] à [(page * nbitems) - 1]
			int index = (page - 1) * nbItems;
			if( slCatalogDisplay.Count > index )
			{
				for(int i = index; i < index + nbItems; i++)
				{
					if(slCatalogDisplay.Count <= i) break;
					DisplayContentSet((ContentSet)slCatalogDisplay.GetByIndex(i), pnl);
				}
			}

			_hasPreviousPage = (page > 1);
			_hasNextPage = (slCatalogDisplay.Count > index + nbItems);

			return catalogDisplay;
		}
		#endregion CatalogDisplay

		#region SearchResult

		protected SearchResult BrowseExaSearchExt()
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			SearchResult searchResult = catalogAPI.ExaSearchExt(1);

			return searchResult;
		}

		protected SearchResult BrowseExaSearchRefine( Panel pnl, string context, string hrefParam, int startElement, int nbItems )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			SearchResult searchResult = catalogAPI.ExaSearchRefine(context, hrefParam, startElement);

			return ReadSearchResult( searchResult, pnl, startElement, nbItems );
		}	

		private SearchResult ReadSearchResult( SearchResult searchResult, Panel pnl, int startElement, int nbItems )
		{
			if( startElement == -1 ) startElement = 0;

			ContentSet contentSet = searchResult.ContentSet;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;

			for(int i = 0; i < nbItems; i++)
			{
				if(contentSet.ContentCollection.Count <= i) break;
				DisplayContent(contentSet.ContentCollection[i], pnl, true);
			}

			_hasPreviousPage = (startElement >= 1);
			_hasNextPage = (searchResult.CurrentSetStart + nbItems < searchResult.TotalHits);

			return searchResult;
		}

		#endregion SearchResult

		#region Display
		protected void DisplayContent(Content content, Panel pnl, bool preview)
		{
			switch( content.ContentGroup.Name )
			{
				case "ANIM":
				case "IMG":
					DisplayImg(content, pnl, preview);
					break;
				case "VIDEO":
				case "VIDEO_RGT":
					DisplayVideo(content, pnl, preview);
					break;
				case "SCHEME":
					DisplayImg(content, pnl, preview);
					break;				
				case "COMPOSITE":
						DisplayContentSet(content, pnl);
					break;
			}
		}

		protected virtual void DisplayImg(Content content, Panel pnl, bool preview)
		{
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(pnl, content, preview);
		}

		protected virtual void DisplayContentSet(Content content, Panel pnl)
		{
			ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
			contentSetDisplay.Display(pnl, content);
		}

		protected virtual void DisplayContentSet(ContentSet contentSet, Panel pnl)
		{
			ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
			contentSetDisplay.Display(pnl, contentSet);
		}

		protected virtual void DisplayVideo(Content content, Panel pnl, bool preview)
		{
			VideoDisplay videoDisplay = new VideoDisplay(_videoDisplayInst);
			videoDisplay.Display(pnl, content, preview);
		}


		#endregion Display

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

	public class Key : IComparable
	{
		private int _priority;
		private string _name;

		public Key( int priority, string name )
		{
			_priority = priority;
			_name = name;
		}

		public int Priority
		{
			get{ return _priority; }
			set{ _priority = value; }
		}

		public string Name
		{
			get{ return _name; }
			set{ _name = value; }
		}

		public int CompareTo(Key key) 
		{
			//			if( _priority < key.Priority ) 
			//				return -1;
			//			else
			//			{
			//				if( _priority > key.Priority )
			//					return 1;
			//				else
			//					return _name.CompareTo(key.Name);
			//			}
			return -1;
		}

		public int CompareTo(object obj)
		{
			//always greather than null
			if( obj == null ) return 1;

			if( obj is Key )return this.CompareTo( (Key)obj );
			
			throw new ArgumentException("object is not a Key");
		}

	}	

	public enum ContentSetPriority
	{
		All,
		Top,
		Exclu,
		NewThemas
	}
}
