using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Panels;
using Adf.Web.Panels;
using Adf.Web.jQuery.UI;

namespace Adf.Web.jQuery.Panels
{
    public class HiddenRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            return type == PanelItemType.Hidden;
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            HtmlInputHidden hidden = new HtmlInputHidden {ID = panelItem.GetId()};

            panelItem.Target = hidden;

            return new object[] { hidden };
        }
    }
}
