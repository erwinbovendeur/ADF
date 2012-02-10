using System.Collections.Generic;
using System.Linq;
using Adf.Base.Panels;
using Adf.Core.Panels;
using Adf.Web.Panels;
using Adf.Web.jQuery.UI;
//using DropDownList = System.Web.UI.WebControls.DropDownList;

namespace Adf.Web.jQuery.Panels
{
    public class DropDownRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            var types = new[] { PanelItemType.DropDown };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            var validator = PanelValidator.Create(panelItem.GetPropertyName());
            var dropDownList = new DropDownList
            {
                ID = panelItem.GetId(),
                Enabled = panelItem.Editable,
                CssClass = "adfj-record-item adfj-record-item-dropdown adfj-record-item-" + panelItem.GetClassName()
            };

            panelItem.Target = dropDownList;

            return new object[] { dropDownList, validator };
        }
    }
}
