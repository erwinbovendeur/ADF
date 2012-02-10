using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Panels;
using Adf.Web.Panels;
using Adf.Web.jQuery.UI;

namespace Adf.Web.jQuery.Panels
{
    public class TextRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            var types = new[]{ PanelItemType.MultiLineText, PanelItemType.NonEditableText, PanelItemType.EditableText, PanelItemType.Password, PanelItemType.Label };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            var validator = PanelValidator.Create(panelItem.GetPropertyName());
            WebControl text;

            if (panelItem.Type == PanelItemType.Label)
            {
                Label lbl = new Label { Text = panelItem.Text, CssClass = "selectable adfj-record-item adfj-record-item-label adf-record-item-" + panelItem.GetClassName() };
                text = lbl;
            }
            else
            {
                string cssClass = (panelItem.Editable) ? string.Empty : "ui-state-disabled ";
                TextBox txt = new TextBox
                                  {
                                      Text = panelItem.Text,
                                      CssClass = cssClass + "adfj-record-item adfj-record-item-textbox adfj-record-item-" + panelItem.GetClassName(),
                                      Wrap = true,
                                      Visible = panelItem.Visible,
                                      TextMode =
                                          panelItem.Type == PanelItemType.MultiLineText
                                              ? TextBoxMode.MultiLine
                                              : TextBoxMode.SingleLine
                                  };
                if (panelItem.Type == PanelItemType.Password) txt.TextMode = TextBoxMode.Password;
                if (!panelItem.Editable) txt.CssClass += " adfj-record-item-textbox-readonly";
                txt.ReadOnly = !panelItem.Editable;

                text = txt;
            }
            text.ID = panelItem.GetId();

            panelItem.Target = text;

            if (panelItem.Type == PanelItemType.NonEditableText || panelItem.Type == PanelItemType.Label)
                return new object[] { text };

            return new object[] {text, validator};
        }
    }
}
