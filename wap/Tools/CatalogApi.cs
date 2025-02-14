using System;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Presentation.Search;

namespace wap.Tools
{
	public class CatalogAPI
	{

		private int _idContent = 0;
		private int _idContentSet = 0;
		private string _contentGroup = "";
		private string _contentType = "";
		private string _mobileType = "";
		private string _displayKey = "";
		private string _keyword = "";
		private int _excludedIDContent = 0;

		public CatalogAPI( int idContent, int idContentSet, string contentGroup, string contentType, string mobileType, string displayKey, string keyword, int excludedIDContent )
		{
			_idContent = idContent;
			_idContentSet = idContentSet;
			_contentGroup = contentGroup;
			_contentType = contentType;
			_mobileType = mobileType;
			_displayKey = displayKey;
			_keyword = keyword;
			_excludedIDContent = excludedIDContent;
		}

		public CatalogAPI( int idContent, string contentGroup, string contentType, string mobileType, string displayKey ) : this(idContent, 0, contentGroup, contentType, mobileType, displayKey, "", 0)
		{}

		public CatalogAPI( string displayKey, int idContentSet, string contentGroup, string contentType, string mobileType ) : this(0, idContentSet, contentGroup, contentType, mobileType, displayKey, "", 0)
		{}

		public CatalogAPI( string displayKey, string contentGroup, string contentType, string mobileType ) : this(0, 0, contentGroup, contentType, mobileType, displayKey, "", 0)
		{}

		public CatalogAPI( string displayKey, int idContentSet, string contentGroup, string contentType, string mobileType, int excludedIDContent ) : this(0, idContentSet, contentGroup, contentType, mobileType, displayKey, "", excludedIDContent)
		{}

		public CatalogAPI( string displayKey, string contentGroup, string contentType, string mobileType, string keyword ) : this(0, 0, contentGroup, contentType, mobileType, displayKey, keyword, 0)
		{}

		public CatalogAPI( string displayKey, int idContentSet ) : this(0, idContentSet, "", "", "", displayKey, "", 0)
		{}

		private CatalogDisplay GetContentSets( ContentSetType contentSetType, ContentSetVisibility contentSetVisibility )
		{
			return StaticCatalogService.GetContentSetsExtended( _displayKey, contentSetType, contentSetVisibility, _contentGroup, _contentType, _mobileType ); 
		}

		public CatalogDisplay GetAllCategories()
		{
			return GetContentSets(ContentSetType.Category, ContentSetVisibility.All); 
		}

		public CatalogDisplay GetAllThemas()
		{
			return GetContentSets(ContentSetType.Thema, ContentSetVisibility.All);
		}

		public ContentSet GetContentsExtended()
		{
			return StaticCatalogService.GetContentsByContentSetExtended( _displayKey, _idContentSet, _contentGroup, _contentType, _mobileType);
		}

		public ContentSet GetContentsExtendedExcluded()
		{
			return StaticCatalogService.GetContentsByContentSetExtended( _displayKey, _idContentSet, _contentGroup, _contentType, _mobileType, _excludedIDContent);
		}

		public ContentSet GetContentsByRandomize(int randomCount)
		{
			return StaticCatalogService.GetContentsByRandomize( _displayKey, _idContentSet, _contentGroup, _contentType, _mobileType, randomCount);
		}

		public ContentSet GetContents()
		{
			return StaticCatalogService.GetContentsByContentSet( _displayKey, _idContentSet, _contentGroup, _contentType );
		}

		public Content GetContent()
		{
			return StaticCatalogService.GetContentInfos(_displayKey, _idContent, _mobileType, _contentType);
		}

		public ContentSet Search()
		{
			return StaticCatalogService.ExaSearch(_displayKey, _contentGroup, _contentType, _mobileType, _keyword, 0);
		}

		public SearchResult ExaSearchExt(int startElement)
		{
			return StaticCatalogService.ExaSearchExt(_displayKey, _contentGroup, _contentType, _mobileType, _keyword, startElement);
		}

		public SearchResult ExaSearchRefine(string context, string hrefParam, int startElement)
		{
			return StaticCatalogService.ExaSearchRefine(_displayKey, context, hrefParam, _mobileType, startElement);
		}

		public ContentSet ExaSearch(int startElement)
		{
			return StaticCatalogService.ExaSearch(_displayKey, _contentGroup, _contentType, _mobileType, _keyword, startElement);
		}

		public Content SearchByShortcode()
		{
			return StaticCatalogService.GetContentInfosByTrackingCode(_displayKey, Convert.ToInt32(_keyword), _mobileType);
		}

		public ContentSet GetContentSetInfos()
		{
			return StaticCatalogService.GetContentSetInfos(_displayKey, _idContentSet);
		}
	}
}
