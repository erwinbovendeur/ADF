using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Adf.Core.Resources;
using Adf.Core.Tasks;
using Adf.Core.Views;
using DNA.UI;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.UI
{
    // Order is important here! The latest is loaded first, weird behaviour by Type.GetAttributes method.
    [JQuery(ScriptResourceBaseName = "Adf.Web.jQuery.", Assembly = "Adf.Web.jQuery", ScriptResources = new[] { "jbreadcrumb.js" })]
    [JQuery(Name = "jBreadCrumb", Assembly = "jQuery", ScriptResources = new[] { "ui.core.js", "ui.widget.js", "effects.core.js" })]
    public class BreadCrumb : Panel
    {
        protected override void OnLoad(System.EventArgs e)
        {
            Visible = true; // Reset the Visible = false flag, or the PreRender will not be invoked.
            base.OnLoad(e);
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            CssClass = ("adfj-breadcrumb adfj-panel " + CssClass).Trim();

            IEnumerable<ITask> tasks = GetTasks(((IView)Page).Task).Reverse();
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            foreach (HtmlGenericControl li in tasks.Select(task => new HtmlGenericControl("li")
                                                                        {
                                                                            InnerText =
                                                                                task.Name == ApplicationTask.Main
                                                                                    ? "Home"
                                                                                    : ResourceManager.GetString(task.Name.Name)
                                                                        }))
            {
                ul.Controls.Add(li);
            }

            if (ul.Controls.Count == 1)
            {
                Visible = false;
                return; // No, you don't have to call base.OnPreRender if the Visibility = false
            }
            Controls.Add(ul);

            ClientScriptManager.RegisterJQueryControl(this);
            jQuery.RegisterCss(Page, "jbreadcrumb.jbreadcrumb.css");

            base.OnPreRender(e);
        }

        private static IEnumerable<ITask> GetTasks(ITask task)
        {
            yield return task;

            while (task.Origin != null && task.Origin.Origin != null && task.Origin.Id != task.Id)
            {
                task = task.Origin;

                yield return task;
            }
        }
    }
}
