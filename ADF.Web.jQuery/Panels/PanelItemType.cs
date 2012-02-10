using Adf.Core.Panels;

namespace Adf.Web.jQuery.Panels
{
    public class JQueryPanelItemType : PanelItemType
    {
        public JQueryPanelItemType(string prefix, string name) : base(prefix, name) { }

        public static readonly PanelItemType MultiDropDown = new PanelItemType("mdd", "MultiDropDown");
    }
}
