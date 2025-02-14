using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMobile.Catalog.Services;

namespace xhtml.Tools
{
	public class XhtmlTools
	{
		public static XhtmlTable AddImgTable(XhtmlTable table, XhtmlImage img, int colspan)
		{
			XhtmlTableRow row = new XhtmlTableRow();
			XhtmlTableCell cell = new XhtmlTableCell();
			cell.HorizontalAlign = HorizontalAlign.Center;
			cell.Controls.Add(img);
			cell.ColumnSpan = colspan;
			row.Controls.Add(cell);
			table.Controls.Add(row);
			return table;
		}

		public static XhtmlTable AddImgTable(XhtmlTable table, XhtmlImage img)
		{
			return AddImgTable (table, img, 1);
		}

		public static XhtmlTableRow AddImgTableRow(XhtmlTableRow row, XhtmlImage img)
		{
			TableCell cell = new TableCell();
			cell.HorizontalAlign = HorizontalAlign.Center;
			cell.Controls.Add(img);
			row.Controls.Add(cell);
			return row;
		}


		public static XhtmlTableRow AddTextTableRow(string CssClass, XhtmlTableRow row, string picto, string text, Color backcolor, Color fontcolor, int colspan,  HorizontalAlign halign, VerticalAlign valign, bool bold, FontUnit sizefont)
		{
			XhtmlTableCell cell = new XhtmlTableCell();
			XhtmlImage img = new XhtmlImage();
			img.ImageUrl = picto;
			cell.Text = " " + text;
			//                              if (CssClass == null)
			//                              {
			cell.BackColor = backcolor;
			cell.ForeColor = fontcolor;
			cell.HorizontalAlign = halign;
			cell.VerticalAlign = valign;
			cell.Font.Bold = bold;
			cell.Font.Size = sizefont;
			//                              }
			//                              else
			if (CssClass!=null) cell.CssClass = CssClass;
			cell.ColumnSpan = colspan;
			if (picto != "") cell.Controls.Add(img);
			row.Controls.Add(cell);
			row.BackColor = backcolor;
			return row;
		}

		public static XhtmlTableRow AddTextTableRow(XhtmlTableRow row, string text, Color backcolor, Color fontcolor, int colspan, HorizontalAlign halign, VerticalAlign valign, bool bold, FontUnit sizefont)
		{
			XhtmlTableCell cell = new XhtmlTableCell();
			cell.Text = text;
			if (backcolor != Color.Empty)
				cell.BackColor = backcolor;
			if (fontcolor != Color.Empty)
				cell.ForeColor = fontcolor;
			cell.ColumnSpan = colspan;
			cell.HorizontalAlign = halign;
			cell.VerticalAlign = valign;
			cell.Font.Bold = bold;
			cell.Font.Size = sizefont;
			row.Controls.Add(cell);
			row.BackColor = backcolor;
			return row;
		}

		public static XhtmlTableRow AddLinkTableRow(string CssClass, XhtmlTableRow row, string text, string url, Color backcolor, Color fontcolor, int colspan, HorizontalAlign halign, VerticalAlign valign, bool bold, FontUnit sizefont, string picto)
		{
			XhtmlTableCell cell = new XhtmlTableCell();
			cell.HorizontalAlign = halign;
			cell.VerticalAlign = valign;
			cell.Font.Bold = bold;
			cell.Font.Size = sizefont;
			if (colspan>1) cell.ColumnSpan = colspan;
			XhtmlLink lnk = new XhtmlLink();
			lnk.Text = HttpUtility.HtmlEncode(text);
			lnk.NavigateUrl = url;
			if (CssClass!="") lnk.CssClass = CssClass;
			if (picto != "")
			{
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = picto;
				img.ImageAlign = ImageAlign.Left;
				cell.Controls.Add(img);
			}
			cell.Controls.Add(lnk);
			row.BackColor = backcolor;
			row.Controls.Add(cell);
			return row;
		}

		public static void AddLinkTable(string CssClass, XhtmlTable t, string text, string url, Color backcolor, Color fontcolor, int colspan, HorizontalAlign halign, VerticalAlign valign, bool bold, FontUnit sizefont, string picto)
		{
			XhtmlTableRow row = new XhtmlTableRow();
			XhtmlTableCell cell = new XhtmlTableCell();
			cell.BackColor = backcolor;
			cell.ForeColor = fontcolor;
			if (colspan>1) cell.ColumnSpan = colspan;
			cell.HorizontalAlign = halign;
			cell.VerticalAlign = valign;
			cell.Font.Bold = bold;
			cell.Font.Size = sizefont;     
			XhtmlLink lnk = new XhtmlLink();
			if (CssClass != "")
				lnk.CssClass = CssClass;
			lnk.Text = HttpUtility.HtmlEncode(text);
			lnk.NavigateUrl = url;
			if (picto != "" && picto.IndexOf("pict:///") < 0)
			{
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = picto;
				img.ImageAlign = ImageAlign.Left;
				cell.Controls.Add(img);
			}
			else
			{
				//cell.Controls.Add(new LiteralControl("<object data='" + picto + "' />"));
				if (picto.IndexOf("button")>=0)
					lnk.AccessKey = picto.Substring(picto.Length-1,1);
			}
			cell.Controls.Add(lnk);
			//row.BackColor = backcolor;
			row.Controls.Add(cell);
			t.Controls.Add(row);
			row = null; cell = null;
		}

		public static void AddLinkTableCell(string CssClass, XhtmlTableCell cell, string text, string url, Color backcolor, Color fontcolor, int colspan, HorizontalAlign halign, VerticalAlign valign, bool bold, FontUnit sizefont, string picto)
		{
			cell.BackColor = backcolor;
			cell.ForeColor = fontcolor;
			if (colspan>1) cell.ColumnSpan = colspan;
			cell.HorizontalAlign = halign;
			cell.VerticalAlign = valign;
			cell.Font.Bold = bold;
			cell.Font.Size = sizefont;
			XhtmlLink lnk = new XhtmlLink();
			lnk.Text = HttpUtility.HtmlEncode(text);
			lnk.NavigateUrl = url;
			if (CssClass != "")
				lnk.CssClass = CssClass;
			if (picto != "" && picto.IndexOf("pict:///") < 0)
			{
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = picto;
				img.ImageAlign = ImageAlign.Left;
				cell.Controls.Add(img);
			}
			else
			{
				//cell.Controls.Add(new LiteralControl("<object data='" + picto + "' />"));
				if (picto.IndexOf("button")>=0)
					lnk.AccessKey = picto.Substring(picto.Length-1,1);
			}
			cell.Controls.Add(lnk);
		}

		public static void AddPicto( MobileCaps mobile, Control ctrl, string imageUrl )
		{                                         
			XhtmlImage img = new XhtmlImage();
			img.ImageUrl = imageUrl;
			//img.ImageAlign = ImageAlign.Left;
			ctrl.Controls.Add(img);
			img = null;
		}
	}
}

