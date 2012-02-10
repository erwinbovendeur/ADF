using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Adf.Core.Resources;
using Adf.Core.State;
using Adf.Core.Validation;
using Adf.Web.Helper;

namespace Adf.Web.jQuery.UI
{
    public class ExceptionControl : Panel
    {
        private Panel exceptions;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            Visible = true;
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            CssClass = ("adfj-panel-exceptions ui-widget " + CssClass).Trim();

            ValidationManager.Handle(true);

            const string key = "PendingValidationResults";
            var validationResults = StateManager.Personal[key] as ValidationResultCollection;

            ValidationManager.Clear();
            StateManager.Personal[key] = null;

            bool hasResults = validationResults != null && validationResults.Count > 0;

            if (!hasResults)
            {
                Visible = false;
                return;
            }

            // Walk through the exceptions, and add them
            exceptions = new Panel { CssClass = string.Concat("ui-corner-all ui-state-", validationResults.IsSucceeded ? "highlight" : "error") };
            Controls.Add(exceptions);

            Handle(validationResults);

            base.OnPreRender(e);
        }

        private void Handle(IEnumerable<ValidationResult> validationResults)
        {
            Control container = HttpContext.Current.Handler as Page;
            List<Control> validators = ControlHelper.List(container, typeof(PanelValidator));

            foreach (ValidationResult validationResult in validationResults)
            {
                string message = validationResult.ToString();

                PropertyInfo pi = validationResult.AffectedProperty;
                if (pi != null)
                {
                    CustomValidator val = validators.FirstOrDefault(v => v.ID == "val" + pi.DeclaringType.Name + pi.Name) as CustomValidator;
                    if (val != null)
                    {
                        val.IsValid = false;

                        int index = validators.IndexOf(val);

                        if (index >= 0)
                        {
                            string title = GetLabel(container, pi);
                            message = message.Contains("{0}")
                                          ? string.Format(ResourceManager.GetString("Field {0}"),
                                                          string.Format(message, title))
                                          : message;
                        }
                    }
                }

                Add(validationResult.Severity, message);
            }
        }

        private void Add(ValidationResultSeverity severity, string message)
        {
            string cssClass = string.Empty;
            switch(severity)
            {
                case ValidationResultSeverity.Informational:
                    cssClass = "ui-icon-info";
                    break;
                case ValidationResultSeverity.Warning:
                    case ValidationResultSeverity.Error:
                    cssClass = "ui-icon-alert";
                    break;
            }

            HtmlContainerControl exception = new HtmlGenericControl("p");
            exception.Controls.Add(new Label {CssClass = string.Concat("ui-icon ", cssClass)});
            exception.Controls.Add(new Literal {Text = message});
            exceptions.Controls.Add(exception);
        }

        public string GetLabel(Control container, PropertyInfo pi)
        {
            var titleLabel = ControlHelper.Find(container, "itemLabel_" + pi.DeclaringType.Name + pi.Name) as Label;
            return (titleLabel != null) ? titleLabel.Text : string.Empty;
        }

        private static CustomValidator FindValidator(Control container, PropertyInfo pi)
        {
            return ControlHelper.Find(container, "val" + pi.DeclaringType.Name + pi.Name) as CustomValidator;
        }
    }
}
