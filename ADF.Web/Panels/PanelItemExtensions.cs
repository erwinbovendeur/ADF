using Adf.Core.Panels;

namespace Adf.Web.Panels
{
    public static class PanelItemExtensions
    {
        public static string GetClassName(this PanelItem panelitem)
        {
            return string.Format("{0}-{1}", panelitem.Member.DeclaringType.Name.ToLower(), panelitem.Member.Name.ToLower());
        }
    }
}
