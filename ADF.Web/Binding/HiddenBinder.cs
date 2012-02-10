using System;
using System.Collections;
using System.Reflection;
using System.Web.UI.HtmlControls;
using Adf.Base.Formatting;
using Adf.Core.Binding;
using Adf.Core.Panels;

namespace Adf.Web.Binding
{
    /// <summary>
    /// Represents a binder for the properties of a <see cref="HtmlInputHidden"/>.
    /// Provides methods to bind the properties of a <see cref="HtmlInputHidden"/> 
    /// with values.
    /// </summary>
    public class HiddenBinder : IControlBinder
    {
        private readonly string[] types = { PanelItemType.Hidden.Prefix };

        /// <summary>
        /// Gets the array of <see cref="HtmlInputHidden"/> id prefixes that 
        /// support binding.
        /// </summary>
        /// <value>The array of <see cref="HtmlInputHidden"/> id prefixes.</value>
        public IEnumerable Types
        {
            get { return types; }
        }

        /// <summary>
        /// Binds the text of the specified <see cref="HtmlInputHidden"/> 
        /// with the specified object. A property is specified to determine whether the 'MaxLength'
        /// property of the specified <see cref="HtmlInputHidden"/> will be set.
        /// </summary>
        /// <param name="control">The <see cref="HtmlInputHidden"/>, the text
        /// of which is to bind to.</param>
        /// <param name="value">The object to bind.</param>
        /// <param name="pi">The Property to determine whether the 'MaxLength' property of the specified 
        /// <see cref="HtmlInputHidden"/> will be set.</param>
        /// <param name="p">The parameters used for binding. Currently not being used.</param>
        public virtual void Bind(object control, object value, PropertyInfo pi, params object[] p)
        {
            HtmlInputHidden t = control as HtmlInputHidden;

            if (t == null) return;

            t.Value = FormatHelper.ToString(value);
        }

        /// <summary>
        /// Binds the properties of the specified <see cref="HtmlInputHidden"/> 
        /// with the specified array of objects.
        /// Currently not supported.
        /// </summary>
        /// <param name="control">The <see cref="HtmlInputHidden"/>, the 
        /// properties of which are to bind to.</param>
        /// <param name="values">The array of objects to bind.</param>
        /// <param name="p">The parameters used for binding.</param>
        public void Bind(object control, object[] values, params object[] p)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Binds the properties of the specified <see cref="HtmlInputHidden"/> 
        /// with the specified list.
        /// Currently not supported.
        /// </summary>
        /// <param name="control">The <see cref="HtmlInputHidden"/>, the 
        ///   properties of which are to bind to.</param>
        /// <param name="values">The list to bind.</param>
        /// <param name="p">The parameters used for binding.</param>
        public void Bind(object control, IEnumerable values, params object[] p)
        {
            throw new NotSupportedException();
        }
    }
}
