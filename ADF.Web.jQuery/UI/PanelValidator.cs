using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Objects;
using Adf.Web.Helper;
using Adf.Web.UI.Styling;

namespace Adf.Web.jQuery.UI
{
    /// <summary>
    /// Performs smart validation on an input control.
    /// </summary>
    public class PanelValidator : CustomValidator
    {
        /// <summary>
        /// A variable to hold the name of panel item.
        /// </summary>
        protected string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelValidator"/> class with the specified panel item name.
        /// </summary>
        /// <param name="name">Panel item name used for validation.</param>
        protected PanelValidator(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Create a <see cref="PanelValidator"/> control and set its identifier by the specified panel item name.
        /// </summary>
        /// <param name="name">Set the identifier of <see cref="PanelValidator"/> control.</param>
        /// <returns>The <see cref="PanelValidator"/> control with ID value.</returns>
        public static CustomValidator Create(string name)
        {
            return new PanelValidator(name) { ID = "val" + name };
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Init"/> event.
        /// Also set true for the associated input control validation.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            IsValid = true;
        }

        private static readonly IStyler ErrorStyler = ObjectFactory.BuildUp<IStyler>("PanelErrorStyler");

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.OnPreRender"/> event.
        /// Also set the default style to smart panel item if the associated input control passes validation; otherwise error style.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //TODO beter systeem bedenken, Validator hoort niks met Parents te maken te hebben.
            Control findInContainerControl = Page;
            if (Parent.Parent != null)
            {
                findInContainerControl = Parent.Parent;
            }

            SetParentStyle(findInContainerControl, "itemLabel_" + name);
        }

        private void SetParentStyle(Control container, string controlName)
        {
            WebControl c = ControlHelper.Find(container, controlName) as WebControl;
            if (c == null || c.Parent == null) return;
            ApplyStyle(c.Parent);
        }

        private void ApplyStyle(Control c)
        {
            if (!IsValid) ErrorStyler.SetStyles(c);
        }
    }
}

