using Adf.Base.Panels;
using Adf.Core.Panels;

namespace Adf.Web.jQuery.Panels
{
    public static class PanelItemExtensions
    {
        public static P AsMultiDropDown<P>(this P panel) where P : AdfPanel
        {
            panel.LastItem().Type = JQueryPanelItemType.MultiDropDown;
            return panel;
        }
    }
}
