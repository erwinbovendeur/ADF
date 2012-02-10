using System.Web.UI;
using Adf.Core.Extensions;
using Adf.Web.UI.Styling;

namespace Adf.Web.jQuery.UI.Styling
{
    public class jGridStyler : IStyler
    {
        public void SetStyles(Control c)
        {
            jGrid grid = c as jGrid;
            if (grid == null) return;

            // Strange, the EmptyDataRowStyle is not persisted. Bug in GridView?
            grid.EmptyDataRowStyle.CssClass = grid.EmptyDataRowStyle.CssClass.Append(" ui-widget-content ui-state-default adfj-table-row-empty").Trim();
            if (grid.Page.IsPostBack) return;

            grid.CssClass = grid.CssClass.Append(" ui-widget adfj-table").Trim();
            grid.HeaderStyle.CssClass = grid.HeaderStyle.CssClass.Append(" ui-widget-header adfj-table-header").Trim();
            grid.RowStyle.CssClass = grid.RowStyle.CssClass.Append(" ui-widget-content ui-state-default adfj-table-row").Trim();
            grid.AlternatingRowStyle.CssClass = grid.AlternatingRowStyle.CssClass.Append(" ui-widget-content ui-state-default adfj-table-row-alternate").Trim();
            grid.SelectedRowStyle.CssClass = grid.SelectedRowStyle.CssClass.Append(" ui-widget-content ui-state-active adfj-table-row-selected").Trim();
            grid.FooterStyle.CssClass = grid.FooterStyle.CssClass.Append(" ui-widget-header adfj-table-footer").Trim();
            grid.PagerStyle.CssClass = grid.PagerStyle.CssClass.Append(" ui-widget-header adfj-table-footer").Trim();
        }
    }
}
