using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Base.Authorization;
using Adf.Core.Authorization;
using Adf.Core.Domain;
using Adf.Web.UI;

namespace Adf.Web.Process
{
    public static class WebViewAuthorizationExtensions
    {
        #region WebControl Authorization

        public static void SetPermission<T>(this WebControl control, IAction action, bool disable = false)
        {
            if (control == null) return;

            control.CssClass = (control.CssClass + " authorized").Trim();
            if (AuthorizationManager.IsAllowed(typeof (T).Name, action)) return;
            
            if (disable)
                control.Enabled = false;
            else
                control.Visible = false;
        }

        public static void SetPermission(this WebControl control, Type subject, IAction action, bool disable = false)
        {
            if (control == null) return;

            control.CssClass = (control.CssClass + " authorized").Trim();
            if (AuthorizationManager.IsAllowed(subject.Name, action)) return;

            if (disable)
                control.Enabled = false;
            else
                control.Visible = false;
        }

        #endregion

        #region SmartPanel Authorization

        public static void SetPermission<T>(this SmartPanel control, IAction action)
        {
            if (control == null) return;

            control.CssClass = (control.CssClass + " authorized").Trim();
            if (AuthorizationManager.IsAllowed(typeof(T).Name, action)) return;
            
            control.Enabled = false;

            if (!AuthorizationManager.IsAllowed(typeof(T).Name, Actions.View))
            {
                control.Visible = false;
            }
        }

        #endregion

        #region Gridview Authorization

        public static void SetPermission<T>(this GridView control, IAction action)
        {
            if (control == null) return;
            control.CssClass = (control.CssClass + " authorized").Trim();

            if (!AuthorizationManager.IsAllowed(typeof(T).Name, action))
            {
                control.SelectedIndexChanging += (sender, args) => args.Cancel = true;
            }
        }

        public static void SetPermission(this GridView control, Type subject, IAction action) 
        {
            if (control == null) return;
            control.CssClass = (control.CssClass + " authorized").Trim();

            if (!AuthorizationManager.IsAllowed(subject.Name, action))
            {
                control.SelectedIndexChanging += (sender, args) => args.Cancel = true;
            }
        }

        #endregion

        #region MessageButton Authorization

        public static void SetPermission<T>(this MessageButton button, IAction action)
        {
            if (button == null) return;
            button.Class = (button.Class + " authorized").Trim();

            if (!AuthorizationManager.IsAllowed(typeof(T).Name, action))
            {
                button.Visible = false;
            }
        }

        public static void SetPermission(this MessageButton button, Type subject,IAction action) 
        {
            if (button == null) return;
            button.Class = (button.Class + " authorized").Trim();

            if (!AuthorizationManager.IsAllowed(subject.Name, action))
            {
                button.Visible = false;
            }
        }

        public static void SetPermission<T>(this Control control, IAction action)
        {
            if (control == null) return;

            if (!AuthorizationManager.IsAllowed(typeof(T).Name, action))
            {
                control.Visible = false;
            }
        }

        #endregion
    }
}
