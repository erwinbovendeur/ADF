using System.Web.UI;
using System.Web.UI.WebControls;

namespace Adf.Web.UI
{
    public class SmartRepeater : Repeater
    {
        public ITemplate EmptyTemplate { get; set; }

        protected override void OnDataBinding(System.EventArgs e)
        {
            base.OnDataBinding(e);

            if (Items.Count == 0)
            {
                EmptyTemplate.InstantiateIn(this);
            }
        }
    }
}
