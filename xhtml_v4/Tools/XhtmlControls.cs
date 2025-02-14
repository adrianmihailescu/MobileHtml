using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xhtml.Tools
{
	#region Surcharge de WebControls.

	public class XhtmlImage : System.Web.UI.WebControls.Image
	{
		// Render IMG --> Alt + Src
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Src, this.ImageUrl, true);
			if (this.AlternateText != null) 
				output.AddAttribute(HtmlTextWriterAttribute.Alt, this.AlternateText, true);
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
		}
	}

	public class XhtmlTable : Table
	{
		// Render TABLE --> Class="normal"
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			if (this.Controls.Count > 0)
			{
				if (this.CellSpacing >= 0)
					output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, this.CellSpacing.ToString(), true);
				if (this.CellSpacing >= 0)
					output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, this.CellPadding.ToString(), true);				
				if (this.CssClass != null && this.CssClass != "")
					output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, true);
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				foreach (System.Web.UI.Control c in this.Controls)
					c.RenderControl(output);
				output.RenderEndTag();
			}
		}
	}

	public class XhtmlTableRow : TableRow
	{
		// Render TABLEROW --> 
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			if (this.Controls.Count > 0)
			{
				if (this.CssClass != null && this.CssClass != "")
					output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, true);
				if (this.BackColor  != Color.Empty)
					output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(this.BackColor).ToString().ToUpper());
				if (this.ForeColor != Color.Empty)
					output.AddStyleAttribute(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(this.ForeColor).ToString().ToLower());
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				foreach (System.Web.UI.Control c in this.Controls)
					c.RenderControl(output);
				output.RenderEndTag();
			}
		}
	}
	
	public class XhtmlLink : HyperLink
	{
		// Render LINK 
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			if (this.CssClass != null && this.CssClass != "")
				output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, true);
			if (this.NavigateUrl!="")
				output.AddAttribute(HtmlTextWriterAttribute.Href, this.NavigateUrl);
			if (this.AccessKey != "")
				output.AddAttribute(HtmlTextWriterAttribute.Accesskey, this.AccessKey);
			output.AddAttribute(HtmlTextWriterAttribute.Title, "Ok");
			output.RenderBeginTag(HtmlTextWriterTag.A);
			foreach (System.Web.UI.Control c in this.Controls)
				c.RenderControl(output);
			if (this.Text != null) output.Write(this.Text);
			if (this.ImageUrl != "") 
			{
				XhtmlImage img = new XhtmlImage();
				img.ImageUrl = this.ImageUrl;
				img.RenderControl(output);
				output.Write(" ");
			}
			output.RenderEndTag();
		}
	}


	public class XhtmlTableCell : TableCell
	{
		private string GetUnit(UnitType unit)
		{
			switch(unit)
			{
				case UnitType.Percentage:
					return "%";
				case UnitType.Pixel:
					return "px";
				case UnitType.Cm:
					return "cm";
				case UnitType.Em:
					return "em";
				case UnitType.Ex:
					return "ex";
				case UnitType.Inch:
					return "in";
				case UnitType.Mm:
					return "mm";
				case UnitType.Pica:
					return "pc";
				case UnitType.Point:
					return "pt";
				default:
					return "%";
			}
		}
		// Render TABLECELL --> Align + Valign + Style
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			if (this.CssClass != null && this.CssClass != "")
				output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, true);
			if (this.HorizontalAlign != HorizontalAlign.NotSet)
				output.AddAttribute(HtmlTextWriterAttribute.Align, this.HorizontalAlign.ToString().ToLower());
			if (this.VerticalAlign != VerticalAlign.NotSet)
				output.AddAttribute(HtmlTextWriterAttribute.Valign, this.VerticalAlign.ToString().ToLower());
			if (this.Width != Unit.Empty)
				output.AddAttribute(HtmlTextWriterAttribute.Width, Width.Value.ToString()+GetUnit(Width.Type));
			if (this.Style.Count > 0)
			{
				IEnumerator keys = this.Style.Keys.GetEnumerator();
				while (keys.MoveNext()) 
				{
					String key = (String)keys.Current;
					output.AddAttribute(HtmlTextWriterAttribute.Style, this.Style[key]);
				}
			}
			if (this.RowSpan > 1)
				output.AddAttribute(HtmlTextWriterAttribute.Rowspan, this.RowSpan.ToString().ToLower());
			if (this.ColumnSpan > 1)
				output.AddAttribute(HtmlTextWriterAttribute.Colspan, this.ColumnSpan.ToString().ToLower());
                              
			// STYLE
			if (this.BackColor  != Color.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(this.BackColor).ToString().ToUpper());
			if (this.ForeColor != Color.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(this.ForeColor).ToString().ToLower());
			if (this.Font.Size != FontUnit.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.FontSize, this.Font.Size.ToString().ToLower());
			if (this.Font.Bold == true)
				output.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			foreach (System.Web.UI.Control c in this.Controls)
				c.RenderControl(output);
			if (this.Text != null) output.Write(this.Text);
			output.RenderEndTag();
		}
	}
	
	#endregion	
}
