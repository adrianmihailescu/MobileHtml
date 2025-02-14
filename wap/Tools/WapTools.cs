using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mail;
using System.Web.UI.MobileControls;
using System.Xml;
using AGInteractive.Business;
using KMobile.Catalog.Presentation;
using KMobile.Catalog.Services;

namespace wap.Tools
{
	public struct Especial
	{
		public string name;
		public string filter;
	}

	public class WapCommand : System.Web.UI.MobileControls.Command
	{
		// Render IMG --> Alt + Src
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			this.Format = CommandFormat.Link;
			this.CausesValidation = true;
			output.Write("<small><anchor title='Ir'>" + this.Text + "<go href='" + this.MobilePage.ActiveForm.Action + "'>");
			output.Write("<postfield name='q' value='$(q)' />");
			output.Write("<postfield name='db' value='vimages' />");
			output.Write("<postfield name='jtsp' value='vimages' />");
			output.WriteLine("</go></anchor></small><br/>");
		}
	}

	public class WapTools
	{
		private static XmlDocument _xmlDoc; 

		public static bool isTestSite(HttpRequest req)
		{
			if (req.ServerVariables["SERVER_NAME"].ToUpper().IndexOf(".DEV.")>=0 || req.ServerVariables["SERVER_NAME"].ToUpper().IndexOf("LOCALHOST")>=0)
				return true;
			else
				return false;
		}

		public static string GetFolderImg(MobileCaps mobile)
		{
			if(mobile.ScreenPixelsWidth >= 164 && mobile.ScreenPixelsWidth < 229)
				return "164";
			if(mobile.ScreenPixelsWidth >= 229 && mobile.ScreenPixelsWidth < 315)
				return "229";
			if(mobile.ScreenPixelsWidth >= 315 && mobile.ScreenPixelsWidth < 339)
				return "315";
			if(mobile.ScreenPixelsWidth >= 339)
				return "339";
			return "118";		
		}

		public static string UpdateFooter(MobileCaps mobile, HttpContext context, string urlBackDefault )
		{
			if( context.Request.QueryString["a1"] != null )
				urlBackDefault = String.Format("{0}{1}", context.Request.ApplicationPath, WapTools.GetXmlValue("Back/CatalogGraphic"));
			try
			{
				if( urlBackDefault != null )
				{
					string param = GetParamBack(context.Request, true);

					return String.Format("{0}?{1}", urlBackDefault, param);
				}
				else
				{
					return "./default.aspx";
				}
			} 
			catch{return "./default.aspx";}
		}

		public static XmlNode GetXmlNode( string path )
		{
			string pathXmlFile = HttpContext.Current.Server.MapPath("~/ConfigSite.xml"); // Gets Physical path of the "ConfigSite.xml" on server
			Cache cache = HttpContext.Current.Cache;
		
			try
			{
				_xmlDoc = (XmlDocument)cache[pathXmlFile];
				if (_xmlDoc == null)
				{
					_xmlDoc = new XmlDocument();
					_xmlDoc.Load(pathXmlFile);  // loads "ConfigSite.xml file 
					cache.Add(pathXmlFile, _xmlDoc, new CacheDependency(pathXmlFile), DateTime.Now.AddHours(6),TimeSpan.Zero, CacheItemPriority.High, null);
				}
				XmlNode root =_xmlDoc.DocumentElement;
				return root.SelectSingleNode(path);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public static string GetXmlValue( string path )
		{
			try 
			{
				return GetXmlNode(path).Attributes["value"].Value;
			}
			catch  
			{
				return "";	
			}
		} 

		public static string GetText( string text )
		{
			return GetXmlValue(String.Format("Texts/data[@name='{0}']", text));
		}

		public static string GetImage( HttpRequest req, string img )
		{
			return String.Format("{0}/Images/{1}", req.ApplicationPath, GetXmlValue(String.Format("Images/data[@name='{0}']", img)));
		} 

		public static string GetDefaultContentType( string contentGroup )
		{
			return GetXmlValue(String.Format("DefaultContentType/data[@name='{0}']", contentGroup));
		}

		public static string GetDefaultContentGroup( string contentType )
		{
			return GetXmlValue(String.Format("DefaultContentGroup/data[@name='{0}']", contentType));
		}

		public static void AddUIDatLog(HttpRequest req, HttpResponse rep)
		{
			try
			{
				string sUID = req.Headers["TM_user-id"];
				if (sUID != null && sUID != "")				
					rep.AppendToLog("uid={" + sUID + "}" );
			}
			catch{}
		}

		public static void LogUser(HttpRequest req, int idSite, string mobiletype)
		{
			try
			{
				string sUID = req.Headers["TM_user-id"]; 
				if(sUID != null && sUID != "") 
				{
					User u = new User();
					u.Visit(sUID, idSite, mobiletype);
					u = null;
				}
			}
			catch{}
		}

		public static void CallPub(HttpRequest req, string type, string format, Panel pnl)
		{
			string banner = "";
			try
			{
				string postData = string.Format("spot={0}&pub=ts&ver=1.5&u={1}&format={2}&ua={3}", type, req.Headers["TM_user-id"], format, req.UserAgent);
				HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(ConfigurationSettings.AppSettings["UrlPub"] + postData);
				myRequest.Method = "GET";
				myRequest.Timeout = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeOut"]) ;
				HttpWebResponse myHttpWebResponse= (HttpWebResponse)myRequest.GetResponse();
				Stream streamResponse=myHttpWebResponse.GetResponseStream();
				StreamReader streamRead = new StreamReader( streamResponse );
				banner = streamRead.ReadToEnd();
				streamRead.Close();
				streamResponse.Close();
				myHttpWebResponse.Close();
			}
			catch{banner="";}
			
			if (banner != "") // Not TIMEOUT								
			{
				string link="", line1="", line2="", img="";
				XmlDocument _xmlBanner = new XmlDocument();	
				_xmlBanner.InnerXml = banner;
				// link
				try
				{
					link = (_xmlBanner.SelectSingleNode("ads/banner/link") != null) ?  _xmlBanner.SelectSingleNode("ads/banner/link").Attributes["href"].Value : "";
					img = (_xmlBanner.SelectSingleNode("ads/banner/img") != null) ?  _xmlBanner.SelectSingleNode("ads/banner/img").Attributes["src"].Value : "";
					line1 = (_xmlBanner.SelectSingleNode("ads/banner/line1") != null) ? _xmlBanner.SelectSingleNode("ads/banner/line1").InnerText : "";
					line2 = (_xmlBanner.SelectSingleNode("ads/banner/line2") != null) ? _xmlBanner.SelectSingleNode("ads/banner/line2").InnerText : "";
				}
				catch{}
				// IMAGE
				if (link != "" && img != "")
				{
					Image imagen = new Image();
					imagen.NavigateUrl = link;
					imagen.ImageUrl = img;
					imagen.AlternateText = "";
					imagen.Alignment = Alignment.Left;
					pnl.Controls.Add(imagen);
				}
				if (link != "" && line1 != "")
				{
					Link lnk = new Link();
					lnk.NavigateUrl = link;
					lnk.Text = line1;
					lnk.Font.Size = FontSize.Small;
					lnk.Alignment = Alignment.Left;
					pnl.Controls.Add(lnk);			
				}
				if (line2 != "")
				{
					Label lbl = new Label();
					lbl.Text = line2;
					lbl.Alignment = Alignment.Left;
					lbl.Font.Size = FontSize.Small;
					pnl.Controls.Add(lbl);	
				}
			}
		}

		public static void TratarBanner(Panel pnl, string banner)
		{
			string link="", line1="", line2="", img="";
			XmlDocument _xmlBanner = new XmlDocument();	
			_xmlBanner.InnerXml = banner;
			// link
			try
			{
				link = (_xmlBanner.SelectSingleNode("ads/banner/link") != null) ?  _xmlBanner.SelectSingleNode("ads/banner/link").Attributes["href"].Value : "";
				img = (_xmlBanner.SelectSingleNode("ads/banner/img") != null) ?  _xmlBanner.SelectSingleNode("ads/banner/img").Attributes["src"].Value : "";
				line1 = (_xmlBanner.SelectSingleNode("ads/banner/line1") != null) ? _xmlBanner.SelectSingleNode("ads/banner/line1").InnerText : "";
				line2 = (_xmlBanner.SelectSingleNode("ads/banner/line2") != null) ? _xmlBanner.SelectSingleNode("ads/banner/line2").InnerText : "";
			}
			catch{}
			// IMAGE
			if (link != "" && img != "")
			{
				Image imagen = new Image();
				imagen.NavigateUrl = link;
				imagen.ImageUrl = img;
				imagen.AlternateText = "";
				imagen.Alignment = Alignment.Center;
				pnl.Controls.Add(imagen);
			}
			if (link != "" && line1 != "")
			{
				Link lnk = new Link();
				lnk.NavigateUrl = link;
				lnk.Text = line1;
				lnk.Font.Size = FontSize.Small;
				lnk.Alignment = Alignment.Center;
				pnl.Controls.Add(lnk);			
			}
			if (line2 != "")
			{
				Label lbl = new Label();
				lbl.Text = line2;
				lbl.Alignment = Alignment.Center;
				lbl.Font.Size = FontSize.Small;
				pnl.Controls.Add(lbl);	
			}
		}

		public static System.Web.UI.MobileControls.Link BuildLink( string text, string navigateUrl )
		{
			System.Web.UI.MobileControls.Link lnk = new System.Web.UI.MobileControls.Link();
			lnk.Font.Size = FontSize.Small;
			lnk.BreakAfter = true;
			lnk.Alignment = Alignment.Left;
			lnk.Text = text;
			lnk.SoftkeyLabel = text;
			lnk.NavigateUrl = navigateUrl;
			return lnk;
		}

		public static string FindProperty(PropertyCollection colProperty, string name)
		{
			try{return colProperty[name].Value.ToString();}
			catch{return "";}			
		}

		public static void AddPicto( MobileCaps mobile, System.Web.UI.MobileControls.Panel pnl, string imageUrl )
		{
			if (mobile.IsAdvanced) // || mobile.IsColor)
			{
				System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
				img.ImageUrl = imageUrl;
				img.BreakAfter = false;
				pnl.Controls.Add(img);
				img = null;
			}
		}
 
		public static string GetUrlBilling( System.Web.HttpRequest req, int drm, string contentGroup, string contentType, string referer, string version, string contentSet )
		{
			string urlBilling; 
			urlBilling = String.Format("{0}{1}?e=1&d={2}&ref={3}&c={4}&cs={5}&ct={6}",
				WapTools.GetXmlValue("Billing/Url"),
				WapTools.GetXmlValue("Billing/UrlBilling_" + contentType),
				drm.ToString(), referer, "{1}", contentSet, "{2}"); 
//			urlBilling = String.Format("{0}{1}?e=1&d={2}&ref={3}&c={4}&cs={5}&ct={6}",
//				WapTools.GetXmlValue("BillingTest/Url"),
//				WapTools.GetXmlValue("BillingTest/UrlBilling_" + contentType),
//				drm.ToString(), referer, "{1}", contentSet, "{2}"); 
			return urlBilling;
		}
//
//		public static string GetUrlBillingFree( System.Web.HttpRequest req, string contentGroup, string contentType, string referer, string version, string contentSet )
//		{
//			return String.Format("{0}{1}?ct={2}&ref={3}&cs={4}&c={5}", 
//				req.ApplicationPath,
//				WapTools.GetXmlValue("Billing/UrlBilling_FREE"),
//				contentType, referer, contentSet, "{0}");
//		}

		public static string GetUrlXView( System.Web.HttpRequest req, string contentGroup, string contentType, string referer, string version, string contentSet )
		{
			//			return String.Format("{0}{1}?ct={2}&ref={3}&cs={4}&c={5}", 
			//				req.ApplicationPath,
			//				WapTools.GetXmlValue("Billing/UrlXView"),
			//				(contentType != "") ? contentType : "{0}", referer, contentSet, (contentType != "") ? "{0}" : "{1}");
			return String.Format("{0}{1}?ct={2}&ref={3}&cs={4}&c={5}", 
				req.ApplicationPath,
				WapTools.GetXmlValue("Billing/UrlXView"),
				"{0}", referer, contentSet, "{1}");
		}

		public static string GetParamBack(HttpRequest req, bool decrypt)
		{
			int index = 1;
			string param = "";
			
			while( req.QueryString[String.Format("a{0}", index)] != null )
			{
				if(decrypt)
				{
					if( param == "" )
						param = String.Format("{0}={1}", req.QueryString[String.Format("a{0}", index)], req.QueryString[String.Format("a{0}", (index + 1))]);
					else
						param = String.Format("{0}&{1}={2}", param, req.QueryString[String.Format("a{0}", index)], req.QueryString[String.Format("a{0}", (index + 1))]);
				}
				else
				{
					if( param == "" )
						param = String.Format("a{0}={1}&a{2}={3}", index, req.QueryString[String.Format("a{0}", index)], index + 1, req.QueryString[String.Format("a{0}", index + 1)]);
					else
						param = String.Format("{0}&a{1}={2}&a{3}={4}", param, index, req.QueryString[String.Format("a{0}", index)], index + 1, req.QueryString[String.Format("a{0}", index + 1)]);
				}
				index = index + 2;
			}
			return param;
		}

		public static void AddLink( Panel pnl, string text, string navigateUrl, string urlPicto, MobileCaps mobile )
		{
			if( urlPicto != "" )
				AddPicto(mobile, pnl, urlPicto);

			Link lnk = BuildLink(text, navigateUrl);

			pnl.Controls.Add(lnk);
		}

		public static void AddLinkCenter( Panel pnl, string text, string navigateUrl, string urlPicto, MobileCaps mobile )
		{
			if( urlPicto != "" )
				AddPicto(mobile, pnl, urlPicto);

			Link lnk = BuildLink(text, navigateUrl);
			lnk.Alignment = Alignment.Center;

			pnl.Controls.Add(lnk);
		}

		public static void AddPictoBreak( MobileCaps mobile, System.Web.UI.MobileControls.Panel pnl, string imageUrl )
		{
			if (mobile.IsAdvanced || mobile.IsColor)
			{
				System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
				img.ImageUrl = imageUrl;
				img.BreakAfter = true;
				pnl.Controls.Add(img);
				img = null;
			}
		}

		public static void AddLabel( Panel pnl, string text, string urlPicto, MobileCaps mobile )
		{
			if( text != "" )
			{
				if( urlPicto != "" )
					AddPicto(mobile, pnl, urlPicto);

				Label lbl = new Label();
				lbl.Text = text;
				lbl.Alignment = Alignment.Left;
				lbl.Font.Size = FontSize.Small;

				pnl.Controls.Add(lbl);
			}
		}

		public static void AddLabelCenter( Panel pnl, string text, string urlPicto, MobileCaps mobile, BooleanOption bold)
		{
			if( text != "" )
			{
				if( urlPicto != "" )
					AddPicto(mobile, pnl, urlPicto);

				Label lbl = new Label();
				lbl.Alignment = Alignment.Center;
				lbl.Font.Bold = bold;
				lbl.Text = text;
				lbl.Alignment = Alignment.Left;
				lbl.Font.Size = FontSize.Small;

				pnl.Controls.Add(lbl);
			}
		}

		public static void AddSearchBlock( MobilePage page, Panel pnl, string searchCmd, string searchLbl, 
			string pictoSearch, string pictoCmd, string contentGroup, string contentType, MobileCaps mobile )
		{
			
			page.ActiveForm.Method = FormMethod.Get;
			page.ActiveForm.Action = ConfigurationSettings.AppSettings["UrlSearch"];
			//page.ActiveForm.Action = "./default.aspx";

			//WapTextBox txtBox = new WapTextBox();
			TextBox txtBox = new TextBox();
			//txtBox.ID = "txtSearch";
			txtBox.ID = "q";
			txtBox.BreakAfter = false;

			WapCommand cmd = new WapCommand();
			cmd.ID = "cmd";
			cmd.Text = searchCmd;
			cmd.Font.Size = FontSize.Small;
			cmd.Format = CommandFormat.Link;
			cmd.CausesValidation = true;

			pnl.Controls.Add(txtBox);
			pnl.Controls.Add(cmd);
			}

		public static bool isXhtml(HttpRequest req, MobileCaps mobile)
		{
			bool correct, xswitch;
			try
			{
				xswitch = Convert.ToBoolean(ConfigurationSettings.AppSettings["Switch_Xhtml"]);
				
				correct = false;
				foreach (string m in WapTools.getNodes("XhtmlConditions/Correct"))
				{
					if (mobile.MobileType == m)
					{
						correct = true;
						break;
					}
				}
				return (xswitch && correct); 
			}
			catch {return false;}
		}

		public static bool noPreview(int idContentSet)
		{
			try
			{
				bool desc = false;
				foreach (string id in WapTools.getNodes("NoPreviews"))
				{
					if (id == idContentSet.ToString())
					{
						desc = true;
						break;
					}
				}
				return (desc); 
			}
			catch {return false;}
		}

		public static ArrayList getNodes(string node)
		{
			ArrayList h = new ArrayList();
			foreach(XmlNode n in GetXmlNode(node))
			{
				h.Add(n.Attributes["value"].Value);
			}
			return h;       
		}

		public static bool isCompatibleThemes(MobileCaps mobile)
		{
			try
			{
				bool compatible = false;
				foreach (string m in WapTools.getNodes("ThemesCompatibility"))
				{
					if (mobile.MobileType == m)
					{
						compatible = true;
						break;
					}
				}
				return (compatible); 
			}
			catch {return false;}
		}

		public static bool isCompatibleThemes2(MobileCaps mobile)
		{
			try
			{
				bool compatible = false;
				foreach (string m in WapTools.getNodes("ThemesCompatibility2"))
				{
					if (mobile.MobileType == m)
					{
						compatible = true;
						break;
					}
				}
				return (compatible); 
			}
			catch {return false;}
		}

//		public static string getEspecial (int nbEspecial, string day)
//		{
//			try
//			{
//				string value = GetXmlValue(String.Format("Especial{0}/data[@name='{1}']", nbEspecial, day));
//				if (value == "") value = "Especial" + nbEspecial;
//				return value;
//			}
//			catch {return "Especial" + nbEspecial;}			
//		}

		public static Especial getEspecial (int nbEspecial, string day)
		{
			Especial esp = new Especial();
			try
			{
				esp.name = GetXmlValue(String.Format("Especial{0}/data[@name='{1}']", nbEspecial, day));
				try
				{
					esp.filter = GetXmlNode(String.Format("Texts/data[@name='{0}']", esp.name)).Attributes["filter"].Value;
					if (esp.filter=="") esp.filter = "IMG_COLOR";
				} 
				catch{esp.filter="IMG_COLOR";}
				return esp;
			}
			catch
			{
				esp.name = "Especial" + nbEspecial;
				esp.filter = "";
				return esp;
			}			
		}
		public static Especial getCompatibleEspecial (Especial esp)
		{
			Especial nextEsp = new Especial();
			try
			{
				int espIndex = Convert.ToInt32(esp.name.Substring(8));
				espIndex++;
				nextEsp.name = "Especial" + espIndex.ToString();
				try
				{
					nextEsp.filter = GetXmlNode(String.Format("Texts/data[@name='{0}']", nextEsp.name)).Attributes["filter"].Value;
					if (nextEsp.filter=="") nextEsp.filter = "IMG_COLOR";
				} 
				catch{nextEsp.filter="IMG_COLOR";}
				return nextEsp;
			}
			catch
			{
				nextEsp.name = esp.name;
				nextEsp.filter = "";
				return nextEsp;
			}			
		}
		public static bool isBranded(Content content)
		{
			string copyright = null;
			try
			{
				copyright = content.PropertyCollection["Copyright"].Value.ToString();
				bool branded = false;
				foreach (string id in WapTools.getNodes("Copyrights"))
				{
					if (id == copyright)
					{
						branded = true;
						break;
					}
				}
				return (branded); 
			}
			catch {return false;}
		}

		public static int isAlerta(int IDContentset)
		{
			int id = 0;
			try
			{
				foreach (string c in WapTools.getNodes("Alertas"))
				{
					if (c == IDContentset.ToString())
					{
						id =  Convert.ToInt32(GetXmlNode(String.Format("Alertas/ContentSet[@value='{0}']", c)).Attributes["link"].Value);
						break;
					}
				}
				return (id); 
			}
			catch {return 0;}
		}

		public static void SendMail (string page, string UA, string exc, NameValueCollection v)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From =  "\"Kiwee Telecom\" <kiweetelecom@ag.com>";
				mailMessage.To = "f.martin@ag.com; lsahni@ag.com";
				mailMessage.Subject = "emocion.kiwee.com crashed";
				mailMessage.BodyFormat = MailFormat.Text;
				mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
				mailMessage.Body = String.Format("Unexpected exception in emocion\\wap\\{0}.aspx - UA : {1} - Error : {2}\n", page, UA, exc);
				for (int i=0; i < v.Count; i++)
					mailMessage.Body += String.Format("{0} - {1}", v[i], v.GetValues(v[i]));
				SmtpMail.SmtpServer = "192.168.0.71";
				SmtpMail.Send(mailMessage);
			}
			catch(Exception ex)
			{
				Log.LogError(String.Format("Site emocion : Unexpected exception while sending mail in emocion\\wap\\{0}.aspx - UA : {1}", page, UA), ex);
			}
		}
		#region ContentGroups
		public class xContentGroup
		{
			string name, ct;
			int rows, cols;
			bool free;
			public string Name
			{
				get{ return name; }
				set{ name = value; }
			}
			public string ContentType
			{
				get{ return ct; }
				set{ ct = value; }
			}
			public int  nbRows
			{
				get{ return rows; }
				set{ rows = value; }
			}
			public int nbCols
			{
				get{ return cols; }
				set{ cols = value; }
			}
			public bool Free
			{
				get{ return free; }
				set{ free = value; }
			}
		}

		public class xContentGroupGraphic: xContentGroup
		{
			public xContentGroupGraphic()
			{
				this.nbCols = 2;
				this.nbRows = 4;
				this.Free = false;
			}
		}

		public class xContentGroupSound: xContentGroup
		{
			public xContentGroupSound()
			{
				this.nbCols = 1;
				this.nbRows = 10;
				this.Free = false;
			}
		}

          
		public class IMG : xContentGroupGraphic
		{
			public IMG()
			{
				this.Name = "IMG";
				this.ContentType = "IMG_COLOR";
			}
		}

		public class ANIM : xContentGroupGraphic
		{
			public ANIM()
			{
				this.Name = "ANIM";
				this.ContentType = "ANIM_COLOR";
			}
		}
		public class VIDEO : xContentGroupGraphic
		{
			public VIDEO()
			{
				this.Name = "VIDEO";
				this.ContentType = "VIDEO_DWL";
			}
		}

		public class GAME : xContentGroupGraphic
		{
			public GAME()
			{
				this.Name = "GAME";
				this.ContentType = "GAME_JAVA";
			}
		}

		public class SOUND: xContentGroupGraphic
		{
			public SOUND()
			{
				this.Name = "SOUND";
				this.ContentType = "SOUND_POLY";
			}
		}
		public class SFX: xContentGroupGraphic
		{
			public SFX()
			{
				this.Name = "SFX";
				this.ContentType = "SOUND_FX";
			}
		}
		public class COMPOSITE: xContentGroupGraphic
		{
			public COMPOSITE()
			{
				this.Name = "COMPOSITE";
				this.ContentType = "";
			}
		}
		public class contentgroupFactory
		{
			public xContentGroup Create(string cg)
			{
				switch(cg)
				{
					case "IMG":
					case "IMG_COLOR":
						return new IMG();
					case "ANIM":
					case "ANIM_COLOR":
						return new ANIM();
					case "VIDEO":
					case "VIDEO_DWL":
						return new VIDEO();
					case "GAME":
					case "GAME_JAVA":
						return new GAME();
					case "SOUND":
					case "SOUND_POLY":
						return new SOUND();
					case "SFX":
					case "SOUND_FX":
						return new SFX();
					case "COMPOSITE":
						return new COMPOSITE();
					default:
						return null;
				}
			}
		}

		#endregion
	
		public enum SearchType
		{
			ts_ImagenesFondos_text_top, 
			ts_ImagenesFondos_banner_top
		}
	}
}
