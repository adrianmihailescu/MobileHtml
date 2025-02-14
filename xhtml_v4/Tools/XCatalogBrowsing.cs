using System;
using System.Drawing;
using System.Web.UI.WebControls;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Presentation.Search;
using KMobile.Catalog.Services;

namespace xhtml.Tools
{
	public class XCatalogBrowsing : System.Web.UI.Page
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

		protected bool _hasNextPage;
		protected bool _hasPreviousPage;
		protected int page_max;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}
		#region Content
		protected Content BrowseContent()
		{
			CatalogAPI catalogAPI = new CatalogAPI(_idContent, _contentGroup, _contentType, _mobile.MobileType, _displayKey);
			Content content = catalogAPI.GetContent();
			return content;
		}
		#endregion

		#region ContentSet

		protected ContentSet BrowseContentSetExtended( System.Web.UI.MobileControls.Panel pnl, int page, int nbItems )
		{
			return BrowseContentSetExtended( pnl, page, nbItems, true );
		}

		protected ContentSet BrowseContentSetExtended()
		{
			return BrowseContentSetExtended(null, 0, 0, false);
		}

		private ContentSet BrowseContentSetExtended( System.Web.UI.MobileControls.Panel pnl, int page, int nbItems, bool display )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _idContentSet, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType);
			ContentSet contentSet = catalogAPI.GetContentsExtended();

			if( display )
				return ReadContentSet( contentSet, pnl, page, nbItems );
			else
				return contentSet;
		}

		protected ContentSet BrowseSearch( System.Web.UI.MobileControls.Panel pnl, int page, int nbItems )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			ContentSet contentSet = catalogAPI.Search();

			return ReadContentSet( contentSet, pnl, page, nbItems );
		}

		protected ContentSet BrowseExaSearch( XhtmlTable tb, int startElement, int nbItems )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			ContentSet contentSet = catalogAPI.ExaSearch(startElement);

			return ReadContentSet( contentSet, tb, 1, nbItems );
		}

		protected ContentSet ReadContentSet( ContentSet contentSet, System.Web.UI.MobileControls.Panel pnl, int page, int nbItems )
		{
			if( page == -1 ) page = 1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;

			//de [(page - 1) * nbitems] à [(page * nbitems) - 1]
			int index = (page - 1) * nbItems;
			if( contentSet.ContentCollection.Count > index )
			{
				for(int i = index; i < index + nbItems; i++)
				{
					if(contentSet.ContentCollection.Count <= i) break;
					DisplayContent(contentSet.ContentCollection[i], pnl);
				}
			}

			_hasPreviousPage = (page > 1);
			_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);

			return contentSet;
		}

		protected ContentSet ReadContentSetGaleria( ContentSet contentSet, XhtmlTable tb, int page, int nbItems )
		{
			XhtmlTableRow row = new XhtmlTableRow();
			if( page == -1 ) page = 1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;
			int index = (page - 1) * nbItems;

			if (contentSet.ContentGroup == "VIDEO" || contentSet.ContentGroup == "VIDEO_RGT" || WapTools.hasDescription(contentSet.IDContentSet))
			{ // VIDEOS
				XhtmlTableRow row2 = new XhtmlTableRow();
				int cols = 1;    
				if (_mobile.ScreenPixelsWidth<=140 && contentSet.ContentGroup != "ANIM") cols=1;
				                    
				//de [(page - 1) * nbitems] à [(page * nbitems) - 1]                                       
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)  
						{
							row = new XhtmlTableRow();
							row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i)
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						XhtmlTableCell cell2 = new XhtmlTableCell();
						cell2.HorizontalAlign = HorizontalAlign.Center;
						cell2.Text = contentSet.ContentCollection[i].Name;
						cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
						row2.Controls.Add(cell2);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null && contentSet.ContentGroup != "SOUND" && contentSet.ContentGroup != "SFX" && contentSet.ContentGroup != "VIDEO"  && contentSet.ContentGroup != "VIDEO_RGT" && contentSet.ContentGroup != "GAME")
			{
				int cols = 1;

				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)
						{
							row = new XhtmlTableRow();
							//row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i) 
						{
							tb.Controls.Add(row);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						//XhtmlTableCell cell2 = new XhtmlTableCell();
						//cell2.HorizontalAlign = HorizontalAlign.Center;
						//cell2.Text = contentSet.ContentCollection[i].Name;
						//cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
						//row2.Controls.Add(cell2);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							//tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null)
			{ // SONNERIES
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						//cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null)
			{ // SONNERIES
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup == "COMPOSITE") // COMPOSITES
			{
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						//cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else // MIX DE VIDEOS 
			{
				XhtmlTableRow row2 = new XhtmlTableRow();
				int cols = 1;      
				if (_mobile.ScreenPixelsWidth<=140) cols=1;                  
				//de [(page - 1) * nbitems] à [(page * nbitems) - 1]                                       
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)  
						{
							row = new XhtmlTableRow();
							row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i)
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						if (WapTools.noPreview(contentSet.IDContentSet))
							DisplayContent(contentSet.ContentCollection[i], cell, false);
						else
						{
							DisplayContent(contentSet.ContentCollection[i], cell);
							//DisplayContent(contentSet.ContentCollection[i], cell);
							XhtmlTableCell cell2 = new XhtmlTableCell();
							cell2.HorizontalAlign = HorizontalAlign.Center;
							cell2.Text = contentSet.ContentCollection[i].Name;
							cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
							row2.Controls.Add(cell2);						
						}
						row.Controls.Add(cell);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
		}


		protected ContentSet ReadContentSet( ContentSet contentSet, XhtmlTable tb, int page, int nbItems )
		{
			XhtmlTableRow row = new XhtmlTableRow();
			if( page == -1 ) page = 1;
			if (WapTools.noPreview(contentSet.IDContentSet)) nbItems = -1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;
			int index = (page - 1) * nbItems;

			if (contentSet.ContentGroup == "VIDEO" || contentSet.ContentGroup == "VIDEO_RGT" || WapTools.hasDescription(contentSet.IDContentSet))
			{ // VIDEOS
				XhtmlTableRow row2 = new XhtmlTableRow();
				int cols = 2;    
				if (_mobile.ScreenPixelsWidth<=140 && contentSet.ContentGroup != "ANIM") cols=1;
				                    
				//de [(page - 1) * nbitems] à [(page * nbitems) - 1]                                       
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)  
						{
							row = new XhtmlTableRow();
							row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i)
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						XhtmlTableCell cell2 = new XhtmlTableCell();
						cell2.HorizontalAlign = HorizontalAlign.Center;
						cell2.Text = contentSet.ContentCollection[i].Name;
						cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
						row2.Controls.Add(cell2);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null && contentSet.ContentGroup != "SOUND" && contentSet.ContentGroup != "SFX" && contentSet.ContentGroup != "VIDEO"  && contentSet.ContentGroup != "VIDEO_RGT" && contentSet.ContentGroup != "GAME")
			{
				int cols = 2;
				if (_mobile.ScreenPixelsWidth<=140 && contentSet.ContentGroup != "ANIM") cols=1;
				                       
				//de [(page - 1) * nbitems] à [(page * nbitems) - 1]
                                       
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)
						{
							row = new XhtmlTableRow();
							//row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i) 
						{
							tb.Controls.Add(row);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						if (WapTools.noPreview(contentSet.IDContentSet))
							DisplayContent(contentSet.ContentCollection[i], cell, false);
						else
							DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						//XhtmlTableCell cell2 = new XhtmlTableCell();
						//cell2.HorizontalAlign = HorizontalAlign.Center;
						//cell2.Text = contentSet.ContentCollection[i].Name;
						//cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
						//row2.Controls.Add(cell2);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							//tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null)
			{ // SONNERIES
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						//cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup != "COMPOSITE" && contentSet.ContentGroup != null)
			{ // SONNERIES
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else if (contentSet.ContentGroup == "COMPOSITE") // COMPOSITES
			{
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						row = new XhtmlTableRow();
						if(contentSet.ContentCollection.Count <= i) break;
						XhtmlTableCell cell = new XhtmlTableCell();
						//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
						//cell.CssClass = (i%2 == 0) ? "yellow3" : "yellow2";
						cell.HorizontalAlign = HorizontalAlign.Left;
						DisplayContent(contentSet.ContentCollection[i], cell);
						row.Controls.Add(cell);
						tb.Controls.Add(row);
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
			else // MIX DE VIDEOS 
			{
				XhtmlTableRow row2 = new XhtmlTableRow();
				int cols = 2;      
				if (_mobile.ScreenPixelsWidth<=140) cols=1;                  
				//de [(page - 1) * nbitems] à [(page * nbitems) - 1]                                       
				if( contentSet.ContentCollection.Count > index )
					for(int i = index; i < index + nbItems; i++)
					{
						if (i%cols == 0)  
						{
							row = new XhtmlTableRow();
							row2 = new XhtmlTableRow();
						}
						if(contentSet.ContentCollection.Count <= i)
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
							break;
						}
						XhtmlTableCell cell = new XhtmlTableCell();
						cell.HorizontalAlign = HorizontalAlign.Center;
						if (WapTools.noPreview(contentSet.IDContentSet))
							DisplayContent(contentSet.ContentCollection[i], cell, false);
						else
						{
							DisplayContent(contentSet.ContentCollection[i], cell);
							//DisplayContent(contentSet.ContentCollection[i], cell);
							XhtmlTableCell cell2 = new XhtmlTableCell();
							cell2.HorizontalAlign = HorizontalAlign.Center;
							cell2.Text = contentSet.ContentCollection[i].Name;
							cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
							row2.Controls.Add(cell2);						
						}
						row.Controls.Add(cell);
						if (i%cols == cols-1) 
						{
							tb.Controls.Add(row);
							tb.Controls.Add(row2);
						}
					}
				_hasPreviousPage = (page > 1);
				_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
				return contentSet;
			}
		}

		protected ContentSet ReadContentSet( ContentSet contentSet, XhtmlTable tb, int page, int nbItems, string cg ) // PARA ESPECIALES
		{
			XhtmlTableRow row = new XhtmlTableRow();
			if( page == -1 ) page = 1;
			if (WapTools.noPreview(contentSet.IDContentSet)) nbItems = -1;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;
			int index = (page - 1) * nbItems;

			if( contentSet.ContentCollection.Count > index )
				for(int i = index; i < index + nbItems; i++)
				{
					row = new XhtmlTableRow();
					if(contentSet.ContentCollection.Count <= i) break;
					try{if(contentSet.ContentCollection[i].PropertyCollection["CompositeContentGroup"].Value.ToString() != cg) continue;}
					catch{continue;}
					XhtmlTableCell cell = new XhtmlTableCell();
					cell.HorizontalAlign = HorizontalAlign.Left;
					cell.ColumnSpan = 2;
					DisplayContent(contentSet.ContentCollection[i], cell);
					row.Controls.Add(cell);
					tb.Controls.Add(row);
				}
			_hasPreviousPage = (page > 1);
			_hasNextPage = (contentSet.ContentCollection.Count > index + nbItems);
                             
			return contentSet;
		}

		#endregion ContentSet

		#region SearchResult

		protected SearchResult BrowseExaSearchExt()
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			SearchResult searchResult = catalogAPI.ExaSearchExt(1);

			return searchResult;
		}

		protected SearchResult BrowseExaSearchRefine( XhtmlTable tb, string context, string hrefParam, int startElement, int nbItems )
		{
			CatalogAPI catalogAPI = new CatalogAPI(_displayKey, _contentGroup == "" ? null : _contentGroup, _contentType == "" ? null : _contentType, _mobile.MobileType, _keyword);
			SearchResult searchResult = catalogAPI.ExaSearchRefine(context, hrefParam, startElement);

			return ReadSearchResult( searchResult, tb, startElement, nbItems );
		}	

		private SearchResult ReadSearchResult( SearchResult searchResult, XhtmlTable tb, int startElement, int nbItems )
		{
			XhtmlTableRow row = new XhtmlTableRow();
			XhtmlTableCell cell;

			ContentSet contentSet = searchResult.ContentSet;
			if( nbItems == -1 ) nbItems = contentSet.ContentCollection.Count;

			page_max = searchResult.TotalHits / nbItems;
			if ((searchResult.TotalHits % nbItems) > 0) page_max++;

			if (contentSet.ContentCollection[0].ContentGroup.Name == "VIDEO" || contentSet.ContentCollection[0].ContentGroup.Name == "GAME")
			{ // VIDEOS  & GAMES
				XhtmlTableRow row2 = new XhtmlTableRow();
				int cols = 2;                                        

				for(int i = 0; i < nbItems; i++)
				{
					if (i%cols == 0)  
					{
						row = new XhtmlTableRow();
						row2 = new XhtmlTableRow();
					}
					if(contentSet.ContentCollection.Count <= i)
					{
						tb.Controls.Add(row);
						tb.Controls.Add(row2);
						break;
					}
					cell = new XhtmlTableCell();
					cell.HorizontalAlign = HorizontalAlign.Center;
					DisplayContent(contentSet.ContentCollection[i], cell);
					row.Controls.Add(cell);
					XhtmlTableCell cell2 = new XhtmlTableCell();
					cell2.HorizontalAlign = HorizontalAlign.Center;
					cell2.Text = contentSet.ContentCollection[i].Name;
					cell2.ForeColor = Color.FromName(WapTools.GetText("Color_" +  contentSet.ContentGroup));
					row2.Controls.Add(cell2);
					if (i%cols == cols-1) 
					{
						tb.Controls.Add(row);
						tb.Controls.Add(row2);
					}
				}
				return searchResult;
			}
			else if (contentSet.ContentCollection[0].ContentGroup.Name != "SOUND" && contentSet.ContentCollection[0].ContentGroup.Name != "SFX")
			{
				int nbcols = 2;
                                       
				for(int i = 0; i < nbItems; i++)
				{
					if (i%nbcols == 0)  row = new XhtmlTableRow();
					if(contentSet.ContentCollection.Count <= i) 
					{
						tb.Rows.Add(row);
						break;
					}
					cell = new XhtmlTableCell();
					cell.HorizontalAlign = HorizontalAlign.Center;
					DisplayContent(contentSet.ContentCollection[i], cell);
					row.Controls.Add(cell);
					if (i%nbcols == nbcols-1) 
						tb.Controls.Add(row);
				}
				return searchResult;
			}
			else
			{ // SONNERIES
				for(int i = 0; i < nbItems; i++)
				{
					row = new XhtmlTableRow();
					if(contentSet.ContentCollection.Count <= i) break;
					cell = new XhtmlTableCell();
					//cell.BackColor = (i%2 == 0) ? ColorTranslator.FromHtml("#FFFFBE") : ColorTranslator.FromHtml("#FFFF00");
					cell.HorizontalAlign = HorizontalAlign.Left;
					DisplayContent(contentSet.ContentCollection[i], cell);
					row.Controls.Add(cell);
					tb.Controls.Add(row);
				}
				return searchResult;
			}
		}

		#endregion SearchResult

		#region Display
		protected void DisplayContent(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			switch( content.ContentGroup.Name )
			{
				case "VIDEO":
				case "VIDEO_RGT":
				case "ANIM":
				case "IMG":
					DisplayImg(content, pnl);
					break;
				case "COMPOSITE":
					DisplayContentSet(content, pnl);
					break;
			}
		}

		protected void DisplayContent(Content content, XhtmlTableCell cell, bool preview)
		{
			switch( content.ContentGroup.Name )
			{
				case "ANIM":
				case "VIDEO":
				case "VIDEO_RGT":
				case "IMG":
					DisplayImg(content, cell, preview);
					break;
				case "SCHEME":
					DisplayImg(content, cell);
					break;
				case "COMPOSITE":
					DisplayContentSet(content, cell, preview);
					break;
			}
		}

		protected void DisplayContent(Content content, XhtmlTableCell cell)
		{
			switch( content.ContentGroup.Name )
			{
				case "ANIM":
				case "VIDEO":
				case "VIDEO_RGT":
				case "IMG":
					DisplayImg(content, cell);
					break;
				case "SCHEME":
					DisplayImg(content, cell);
					break;
				case "COMPOSITE":
					DisplayContentSet(content, cell, true);
					break;
			}
		}

		protected void DisplayContent(Content content, XhtmlTableCell cell, XhtmlTableCell cellTitle)
		{
			switch( content.ContentGroup.Name )
			{
				case "ANIM":
				case "VIDEO":
				case "VIDEO_RGT":
				case "IMG":
					DisplayImg(content, cellTitle, cell);
					break;
				case "COMPOSITE":
					DisplayContentSet(content, cell, true);
					break;
			}
		}


		protected virtual void DisplayImg(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(pnl, content, true);
		}

		protected virtual void DisplayImg(Content content, XhtmlTableCell cell)
		{
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(cell, content);
		}

		protected virtual void DisplayImg(Content content, XhtmlTableCell cell, bool preview)
		{
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(cell, content, preview);
		}

		protected virtual void DisplayImg(Content content, XhtmlTableCell cell, XhtmlTableCell cellTitle)
		{
			ImgDisplay imgDisplay = new ImgDisplay(_imgDisplayInst);
			imgDisplay.Display(cell, cellTitle, content);
		}
                    
		protected virtual void DisplayContentSet(Content content, System.Web.UI.MobileControls.Panel pnl)
		{
			ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
			contentSetDisplay.Display(pnl, content);
		}
 
		protected virtual void DisplayContentSet(Content content, XhtmlTableCell cell, bool preview)
		{
			ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
			contentSetDisplay.Display(cell, content, preview);
		}

		protected virtual void DisplayContentSet(ContentSet contentSet, System.Web.UI.MobileControls.Panel pnl)
		{
			ContentSetDisplay contentSetDisplay = new ContentSetDisplay(_contentSetDisplayInst);
			contentSetDisplay.Display(pnl, contentSet);
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

}
