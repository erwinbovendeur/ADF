using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Adf.Core.Binding;
using Adf.Core.Domain;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.Binding
{
    /// <summary>
    /// Represents a persister for a <see cref="DatePicker"/>.
    /// Provides method to persist the date of a <see cref="DatePicker"/>.
    /// </summary>
    class DateTimePersister : IControlPersister
    {
        /// <summary>
        /// The array of <see cref="DatePicker"/> id prefixes that support binding.
        /// </summary>
        private readonly string[] types = { "dtb" };

        /// <summary>
        /// Gets the <see cref="DatePicker"/> id prefix that support persisting.
        /// </summary>
        /// <returns>
        /// The <see cref="DatePicker"/> id prefix.
        /// </returns>
        public IEnumerable<string> Types
        {
            get { return types; }
        }

        /// <summary>
        /// Persists the date of the specified <see cref="DatePicker"/> to the specified property of the specified object.
        /// </summary>
        /// <param name="bindableObject">The object where to persist.</param>
        /// <param name="pi">The property of the object where to persist.</param>
        /// <param name="control">The <see cref="DatePicker"/>, the date of which is to persist.</param>
        public void Persist(object bindableObject, PropertyInfo pi, object control)
        {            
            // Persist Calender fields
            DatePicker calendar = control as DatePicker;
            if (calendar == null) return;
            if (calendar.Enabled)
            {
                PropertyHelper.SetValue(bindableObject, pi, calendar.Value, CultureInfo.CurrentUICulture);
            }
        }
    }
}
