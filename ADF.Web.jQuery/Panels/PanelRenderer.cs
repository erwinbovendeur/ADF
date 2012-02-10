using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Extensions;
using Adf.Core.Objects;
using Adf.Core.Panels;
using Adf.Core.Resources;

namespace Adf.Web.jQuery.Panels
{
    public class PanelRenderer : IPanelRenderer
    {
        private static IEnumerable<IPanelItemRenderer> _renderers;
        private static readonly object _lock = new object();

        private static IEnumerable<IPanelItemRenderer> Renderers
        {
            get { lock (_lock) return _renderers ?? (_renderers = ObjectFactory.BuildAll<IPanelItemRenderer>().ToList()); }
        }

        public object Render(AdfPanel adfPanel)
        {
            if (adfPanel.Rows.Count == 0) return null;

            Panel panel = new Panel { CssClass = "adfj-form"};
            foreach (var panelrow in adfPanel.Rows)
            {
                Panel line = null;
                for (int i = 0; i < panelrow.Items.Count(); i++)
                {
                    var item = panelrow.Items[i];
                    var items = RenderItem(item);
                    var label = RenderLabel(item, items.FirstOrDefault());

                    if (!item.AttachToPrevious)
                    {
                        line = new Panel { CssClass = "adfj-record" };
                        panel.Controls.Add(line);
                        if (!item.Label.IsNullOrEmpty())
                        {
                            line.Controls.Add(label);
                        }
                    }

                    if (line == null) throw new RenderException("The first item in a panel cannot attach to the previous item.");
                    item.Control = line;

                    foreach (Control control in items)
                    {
                        line.Controls.Add(control);
                    }
                }
            }
            return panel;
        }

        private static IEnumerable<Control> RenderItem(PanelItem panelItem)
        {
            foreach (var renderer in Renderers.Where(renderer => renderer.CanRender(panelItem.Type)))
            {
                return renderer.Render(panelItem).Cast<Control>();
            }
            return new List<Control>();
        }

        private static Control RenderLabel(PanelItem panelItem, Control item)
        {
            if (item != null && !panelItem.Label.IsNullOrEmpty())
            {
                Label label = new Label
                                  {
                                      AssociatedControlID = item.ID,
                                      CssClass = "selectable adfj-record-label",
                                      ID = "itemLabel_" + panelItem.GetPropertyName(),
                                      Text = ResourceManager.GetString(panelItem.Label)
                                  };

                if (!panelItem.Optional) label.CssClass += " adfj-record-label-mandatory";
                return label;
            }

            return null;
        }
    }
}
