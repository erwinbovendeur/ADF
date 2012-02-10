using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Panels;
using Adf.Core.Resources;
using Adf.Web.Panels;
using Adf.Web.jQuery.UI;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.Panels
{
    public class CalendarRenderer : IPanelItemRenderer
    {
        protected string DateFormat = string.Format("{0}", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

        public bool CanRender(PanelItemType type)
        {
            var types = new[] { PanelItemType.Calendar };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            var validator = PanelValidator.Create(panelItem.GetPropertyName());

            string cssClass = (panelItem.Editable) ? string.Empty : "ui-state-disabled ";
            DatePicker datePicker = new DatePicker
                                        {
                                            ID = panelItem.GetId(),
                                            Enabled = panelItem.Editable,
                                            Width = new Unit(panelItem.Width, UnitType.Ex),
                                            DateFormatString = DateFormat,
                                            AppendText = ResourceManager.GetString(DateFormat),
                                            LocID = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                                            CssClass = cssClass + "adfj-record-item adfj-record-item-calender adfj-record-item-" + panelItem.GetClassName()
                                        };

            panelItem.Target = datePicker;
            return new object[] { validator, datePicker };
        }
    }
}
