using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.UI.HtmlControls;
using Adf.Core.Binding;
using Adf.Core.Domain;
using Adf.Core.Panels;

namespace Adf.Web.Binding
{
	/// <summary>
    /// Represents the persister for a <see cref="HtmlInputHidden"/>.
    /// Provides method to persist the text of a <see cref="HtmlInputHidden"/>.
	/// </summary>
	public class HiddenPersister : IControlPersister
	{
	    private readonly string[] types = {PanelItemType.Hidden.Prefix};

		/// <summary>
        /// Gets the array of <see cref="HtmlInputHidden"/> id prefixes that 
        /// support persistance.
		/// </summary>
        /// <value>The array of <see cref="HtmlInputHidden"/> id prefixes.</value>
		public IEnumerable<string> Types
		{
			get { return types; }
		}

		/// <summary>
        /// Persists the text of the specified <see cref="HtmlInputHidden"/>
        /// to the specified property of the specified object.
		/// </summary>
		/// <remarks>For password textboxes, persistance will only occur when 
		/// the length of the password is greater than one character.</remarks>
        /// <param name="bindableObject">The object where to persist.</param>
        /// <param name="pi">The property of the object where to persist.</param>
        /// <param name="control">The <see cref="HtmlInputHidden"/>, the 
        /// text of which is to persist.</param>
        public virtual void Persist(object bindableObject, PropertyInfo pi, object control)
		{
            HtmlInputHidden t = control as HtmlInputHidden;

			if (t != null)
			{
                PropertyHelper.SetValue(bindableObject, pi, t.Value, CultureInfo.CurrentUICulture);
			}
		}
	}
}
