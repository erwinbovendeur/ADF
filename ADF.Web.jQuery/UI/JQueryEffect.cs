using System;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.UI
{
    [Serializable]
    public class JQueryEffect
    {
        public JQueryEffect(JQueryEffects effect = JQueryEffects.None, int timeout = 200)
        {
            Effect = effect;
            Timeout = timeout;
        }

        public JQueryEffects Effect { get; set; }
        public int Timeout { get; set; }
    }
}
