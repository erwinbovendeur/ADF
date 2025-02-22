﻿using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Panels;

namespace Adf.Web.Panels
{
    public class FileUploadRenderer : BaseRenderer, IPanelItemRenderer
    {
        public bool CanRender(PanelItemType type)
        {
            var types = new[] { PanelItemType.FileUpload };

            return types.Contains(type);
        }

        public IEnumerable<object> Render(PanelItem panelItem)
        {
            var upload = new FileUpload { Enabled = true, CssClass = ItemStyle};

            panelItem.Target = upload;

            return new List<Control> { upload }.Cast<object>();
        }
    }
}
