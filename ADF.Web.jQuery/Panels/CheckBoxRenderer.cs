using System.Collections.Generic;
using System.Linq;
using Adf.Base.Panels;
using Adf.Core.Panels;
using Adf.Web.Panels;
using Adf.Web.jQuery.UI;

namespace Adf.Web.jQuery.Panels
{
    public class CheckBoxRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            var types = new[] { PanelItemType.CheckBox };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            var validator = PanelValidator.Create(panelItem.GetPropertyName());
            CheckBox toggleButton = new CheckBox
            {
                ID = panelItem.GetId(),
                Enabled = panelItem.Editable,
                CssClass = "adfj-record-item adfj-record-item-checkbox adfj-record-item-" + panelItem.GetClassName()
            };

            panelItem.Target = toggleButton;

            return new object[] { toggleButton, validator };
        }
    }
}
