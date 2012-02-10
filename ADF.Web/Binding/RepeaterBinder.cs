using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using Adf.Core.Binding;

namespace Adf.Web.Binding
{
    /// <summary>
    /// Represents a binder for a <see cref="System.Web.UI.WebControls.Repeater"/>.
    /// Provides methods to bind the properties of a <see cref="System.Web.UI.WebControls.Repeater"/> to values.
    /// </summary>
    public class RepeaterBinder : IControlBinder
    {
        /// <summary>
        /// Gets the array of <see cref="System.Web.UI.WebControls.Repeater"/> id prefixes that support binding.
        /// </summary>
        /// <value>The array of <see cref="System.Web.UI.WebControls.Repeater"/> id prefixes.</value>
        public IEnumerable Types
        {
            get { return new[] { "rpt" }; }
        }

        /// <summary>
        /// Binds the 'DataSource' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// with the specified <see cref="System.Data.DataSet"/>.
        /// </summary>
        /// <remarks>The 'DataKeyField' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// is set to 'ID'.</remarks>   
        /// <param name="control">The <see cref="System.Web.UI.WebControls.Repeater"/>, the 'DataSource' 
        /// property of which is to bind to.</param>
        /// <param name="value">The <see cref="System.Data.DataSet"/> to bind.</param>
        /// <param name="pi">The property of the <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// to bind to. Currently not being used.</param>
        /// <param name="p">The parameters used for binding. Currently not being used.</param>
        public virtual void Bind(object control, object value, PropertyInfo pi, params object[] p)
        {
            if (value == null) return;

            Repeater repeater = control as Repeater;
            if (repeater == null) return;

            DataSet set = value as DataSet;
            if (set == null) return;

            repeater.DataSource = set;
            repeater.DataBind();
        }

        /// <summary>
        /// Binds the 'DataSource' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// with the specified array of objects.
        /// </summary>
        /// <remarks>The 'DataKeyField' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// is set to 'ID'.</remarks>
        /// <param name="control">The <see cref="System.Web.UI.WebControls.Repeater"/>, the 'DataSource' 
        /// property of which is to bind to.</param>
        /// <param name="values">The array of objects to bind.</param>
        /// <param name="p">The parameters used for binding. Currently not being used.</param>
        public virtual void Bind(object control, object[] values, params object[] p)
        {
            if (values == null) return;

            Repeater repeater = control as Repeater;
            if (repeater == null) return;

            repeater.DataSource = values;
            repeater.DataBind();
        }

        /// <summary>
        /// Binds the 'DataSource' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// with the specified list.
        /// </summary>
        /// <remarks>The 'DataKeyField' property of the specified <see cref="System.Web.UI.WebControls.Repeater"/> 
        /// is set to 'ID'.</remarks>
        /// <param name="control">The <see cref="System.Web.UI.WebControls.Repeater"/>, the 'DataSource' 
        ///   property of which is to bind to.</param>
        /// <param name="values">The list to bind.</param>
        /// <param name="p">The parameters used for binding. Currently not being used.</param>
        public virtual void Bind(object control, IEnumerable values, params object[] p)
        {
            if (values == null) return;

            Repeater repeater = control as Repeater;
            if (repeater == null) return;

            repeater.DataSource = values;
            repeater.DataBind();
        }
    }
}