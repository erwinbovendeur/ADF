using System.Web.UI;
using Adf.Web.UI.SmartView;

namespace Adf.Web.UI.Styling
{
    /// <summary>
    /// Represents a styler for a <see cref="SmartView"/>.
    /// Provides method to set the styles defined in the cascading style sheets to a 
    /// <see cref="SmartView"/>.
    /// </summary>
	public class BusinessGridViewStyler : IStyler
	{
        /// <summary>
        /// Sets the CSS styles to the specified <see cref="SmartView"/>.
        /// </summary>
        /// <param name="c">The <see cref="SmartView"/> to give CSS styles to.</param>
        public void SetStyles(Control c)
		{
			SmartView.SmartView grid = c as SmartView.SmartView;
			if (grid == null) return;

            grid.CssClass = (grid.CssClass + " BusinessGrid").Trim();

            grid.RowStyle.CssClass = (grid.RowStyle.CssClass + " BusinessGridItem").Trim();
            grid.EditRowStyle.CssClass = (grid.EditRowStyle.CssClass + " BusinessGridEditItem").Trim();
            grid.AlternatingRowStyle.CssClass = (grid.AlternatingRowStyle.CssClass + " BusinessGridAlternatingItem").Trim();
            grid.SelectedRowStyle.CssClass = (grid.SelectedRowStyle.CssClass + " BusinessGridSelectedItem").Trim();
			grid.PagerStyle.CssClass = (grid.PagerStyle.CssClass + " BusinessGridPager").Trim();
			grid.HeaderStyle.CssClass = (grid.HeaderStyle.CssClass + " BusinessGridHeader").Trim();
			grid.FooterStyle.CssClass = (grid.FooterStyle.CssClass + " BusinessGridFooter").Trim();
		}
	}
}