using System;
using System.Collections;
using System.Reflection;
using System.Web.UI.WebControls;
using Adf.Core.Binding;
using Adf.Web.UI;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.Binding
{
    /// <summary>
    /// Represents a binder for a <see cref="DatePicker"/>.
    /// Provides methods to bind the 'Value' property of a <see cref="DatePicker"/> control with a date.
    /// </summary>
    public class DateTimeBinder : IControlBinder
    {
        /// <summary>
        /// The array of <see cref="DatePicker"/> id prefixes that support binding.
        /// </summary>
        private readonly string[] types = {"dtb"};

        /// <summary>
        /// Gets the <see cref="DatePicker"/> id prefix that support binding.
        /// </summary>
        /// <returns>
        /// The <see cref="DatePicker"/> id prefix.
        /// </returns>
        public IEnumerable Types
        {
            get { return types; }
        }

        /// <summary>
        /// Binds the 'Value' property of the specified <see cref="DatePicker"/> with the specified value.
        /// </summary>
        /// <param name="control">The <see cref="DatePicker"/> to bind to.</param>
        /// <param name="value">The value to bind.</param>
        /// <param name="pi">Currently not being used.</param>
        /// <param name="p">The parameters used for binding. Currently not being used.</param>
        public virtual void Bind(object control, object value, PropertyInfo pi, params object[] p)
        {
            var calendar = control as DatePicker;
            if (calendar == null || value == null) return;

            if (!string.IsNullOrEmpty(value.ToString())) calendar.Value = GetDate(value);
        }

        /// <summary>
        /// Converts the specified value to a Nullable&lt;DateTime&gt; value and returns the output.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The Nullable&lt;DateTime&gt; value if the conversion is successful; otherwise, null.
        /// </returns>
        private static DateTime? GetDate(object value)
        {
            if (value is DateTime) return (DateTime?) value;

            return null;
        }

        /// <summary>
        /// Binds the specified <see cref="DatePicker"/> with the specified array of objects.
        /// Currently not being supported.
        /// </summary>
        /// <param name="control">The <see cref="DatePicker"/> to bind to.</param>
        /// <param name="values">The array of objects to bind.</param>
        /// <param name="p">The parameters used for binding.</param>
        public void Bind(object control, object[] values, params object[] p)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Binds the specified <see cref="DatePicker"/> with the specified list.
        /// Currently not being supported.
        /// </summary>
        /// <param name="control">The <see cref="DatePicker"/> to bind to.</param>
        /// <param name="values">The list to bind.</param>
        /// <param name="p">The parameters used for binding.</param>
        public void Bind(object control, IEnumerable values, params object[] p)
        {
            throw new NotSupportedException();
        }
    }
}