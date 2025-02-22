using System;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Adf.Core.Extensions;

namespace Adf.Web.UI
{
    /// <summary>
    /// Validate a control that allows the user to select a single item from a drop-down list.
    /// </summary>
    public class ValidatedListBoxItem : ValidatedPanelItem
    {
        /// <summary>
        /// A variable of <see cref="Adf.Web.UI.DropDownListItem"/> that define the panel item which will validated.
        /// </summary>
        protected ListBoxItem item;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adf.Web.UI.ValidatedDropDownListItem"/> class with the specified <see cref="Adf.Web.UI.DropDownListItem"/>.
        /// </summary>
        /// <param name="panelItem">The <see cref="Adf.Web.UI.DropDownListItem"/> that define the panel item which will validated.</param>
        /// <param name="name">The identifier of <see cref="Adf.Web.UI.BasePanelItem"/>.</param>
        /// <param name="mandatory">Add an asterix with item label if <see cref="Adf.Web.UI.DropDownListItem"/> is mandatory; else none.</param>
        public ValidatedListBoxItem(ListBoxItem panelItem, string name, bool mandatory) : base(panelItem, name, mandatory)
        {
            item = panelItem;
        }

        /// <summary>
        /// Create a <see cref="System.Web.UI.WebControls.DropDownList"/> with validation and add it into <see cref="Adf.Web.UI.BasePanelItem"/>.
        /// </summary>
        /// <param name="label">The display text of the drop-down list within <see cref="Adf.Web.UI.BasePanelItem"/>.</param>
        /// <param name="name">The identifier of <see cref="Adf.Web.UI.BasePanelItem"/> and set the identification of <see cref="System.Web.UI.WebControls.DropDownList"/> control.</param>
        /// <param name="width">Width of <see cref="System.Web.UI.WebControls.DropDownList"/> control.</param>
        /// <param name="enabled">A value indicating whether the <see cref="System.Web.UI.WebControls.DropDownList"/> control is enabled or not.</param>
        /// <param name="mandatory">Add an asterix with item label if <see cref="Adf.Web.UI.DropDownListItem"/> is mandatory; else none.</param>
        /// <returns>A new instance of the <see cref="Adf.Web.UI.ValidatedDropDownListItem"/> class.</returns>
        public static ValidatedListBoxItem Create(string label, string name, int width, bool enabled, bool mandatory, ListSelectionMode mode = ListSelectionMode.Single)
        {
            return new ValidatedListBoxItem(ListBoxItem.Create(label, name, width, enabled, mode), name, mandatory);
        }

        public static ValidatedListBoxItem Create<T>(Expression<Func<T, object>> property, int width, bool enabled = true, bool mandatory = true, ListSelectionMode mode = ListSelectionMode.Single)
        {
            return Create(property.GetExpressionMember().Name, property, width, enabled, mandatory, mode);
        }

        public static ValidatedListBoxItem Create<T>(string label, Expression<Func<T, object>> property, int width, bool enabled = true, bool mandatory = true, ListSelectionMode mode = ListSelectionMode.Single)
        {
            return new ValidatedListBoxItem(ListBoxItem.Create(label, property.GetControlName(), width, enabled, mode), property.GetControlName(), mandatory);
        }

        /// <summary>
        /// Gets the <see cref="System.Web.UI.WebControls.DropDownList"/> from the items list of <see cref="Adf.Web.UI.BasePanelItem"/> class.
        /// </summary>
        /// <returns>The <see cref="System.Web.UI.WebControls.DropDownList"/> from the items list of <see cref="Adf.Web.UI.BasePanelItem"/> class.</returns>
        public virtual ListBox ListBox
        {
            get { return item.ListBox; }
        }

        public override bool Enabled
        {
            get { return item.Enabled; }
            set { item.Enabled = value; }
        }

        public ListSelectionMode SelectionMode
        {
            get { return item.SelectionMode;  }
            set { item.SelectionMode = value;  }
        }
    }
}
