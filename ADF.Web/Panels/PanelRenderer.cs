using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Extensions;
using Adf.Core.Objects;
using Adf.Core.Panels;
using Adf.Core.Resources;
using Adf.Web.UI;
using System.Linq;

namespace Adf.Web.Panels
{
    public class PanelRenderer : BaseRenderer, IPanelRenderer
    {
        private static IEnumerable<IPanelItemRenderer> _renderers;
        private static readonly object _lock = new object();

        private static IEnumerable<IPanelItemRenderer> Renderers
        {
            get { lock (_lock) return _renderers ?? (_renderers = ObjectFactory.BuildAll<IPanelItemRenderer>().ToList()); }
        }

        public object Render(AdfPanel panel)
        {
            if (panel.Rows.Count == 0) return null;

            var table = new Table { CssClass = PanelStyle};

            int cellsperrow = panel.GetMaxItemsPerRow() * 2;

            foreach (var panelrow in panel.Rows)
            {
                var row = new TableRow { CssClass = RowStyle };
                var itemcell = new TableCell { CssClass = ItemCellStyle };

                for (int i = 0; i < panelrow.Items.Count(); i++)
                {
                    var item = panelrow.Items[i];
                    var labels = RenderLabel(item);
                    var items = RenderItem(item);

                    if (!item.AttachToPrevious)
                    {
                        if (!item.Label.IsNullOrEmpty())
                        {
                            var labelcell = new TableCell { ID = "panelLabelItem_" + item.GetPropertyName(), CssClass = LabelCellStyle };

                            labelcell.Controls.AddRange(labels);
                            row.Controls.Add(labelcell);
                        }

                        itemcell = new TableCell { ID = "panelControlItem_" + item.GetPropertyName(), CssClass = ItemCellStyle };
                    }

                    itemcell.Controls.AddRange(items);
                    if (i == panelrow.Items.Count() - 1) itemcell.ColumnSpan = cellsperrow - i;

                    row.Controls.Add(itemcell);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private IEnumerable<Control> RenderItem(PanelItem panelItem)
        {
            foreach (var renderer in Renderers.Where(renderer => renderer.CanRender(panelItem.Type)))
            {
                return renderer.Render(panelItem).Cast<Control>();
            }
            return new List<Control>();
        }

        private IEnumerable<Control> RenderLabel(PanelItem panelItem)
        {
            var controls = new List<Control>();

            if (!panelItem.Label.IsNullOrEmpty())
            {
                controls.Add(new Label { ID = "itemLabel_" + panelItem.GetPropertyName(), Text = ResourceManager.GetString(panelItem.Label), CssClass = LabelStyle });
                
                if (!panelItem.Optional) controls.Add(new Label { Text = @" *", CssClass = "Mandatory"});
            }

            return controls;
        }
    }
}
