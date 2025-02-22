﻿using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Base.Panels;
using Adf.Core.Extensions;
using Adf.Core.Panels;
using Adf.Core.Resources;

namespace Adf.Web.Panels
{
    public class ImageRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            var types = new[] { PanelItemType.Image, PanelItemType.InfoIcon };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            if (panelItem.Type == PanelItemType.Image)
            {
                var image = new Image { ID = panelItem.GetId(), Enabled = false, Width = new Unit(panelItem.Width, UnitType.Pixel), CssClass = ItemStyle };

                panelItem.Target = image;

                return new object[] { image };
            }
            if (panelItem.Type == PanelItemType.InfoIcon)
            {
                var image = new Image
                {
                    ID = panelItem.GetId(),
                    ImageUrl = @"../images/info.png",
                    ToolTip = ResourceManager.GetString(panelItem.Text.IsNullOrEmpty() ? panelItem.GetPropertyName() + "Info" : panelItem.Text),
                    Enabled = false,
                    Width = new Unit(panelItem.Width, UnitType.Pixel),
                    CssClass = ItemStyle
                };

                panelItem.Target = image;

                return new object[] { image };
            }

            return null;
        }
    }
}
