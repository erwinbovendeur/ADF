using Adf.Web.Extensions;
using DNA.UI;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.UI
{
    [JQuery(Assembly = "Adf.Web.jQuery", ScriptResourceBaseName = "Adf.Web.jQuery.", ScriptResources = new[] { "jquery.multiselect.js" })]
    [JQuery(Name = MethodName, Assembly = "jQuery", ScriptResources = new[] { "ui.core.js", "ui.widget.js" }, StartEvent = ClientRegisterEvents.DocumentReady)]
    public class DropDownList : System.Web.UI.WebControls.DropDownList
    {
        internal const string MethodName = "multiselect";

        public bool Multiple
        {
            get { return ViewState.GetOrDefault("multiple", false); }
            set { ViewState["multiple"] = value; }
        }

        public JQueryEffect ShowEffect
        {
            get { return ViewState.GetOrDefault("ShowEffect", new JQueryEffect()); }
            set { ViewState["ShowEffect"] = value; }
        }

        public JQueryEffect HideEffect
        {
            get { return ViewState.GetOrDefault("HideEffect", new JQueryEffect()); }
            set { ViewState["HideEffect"] = value; }
        }

        public bool Header
        {
            get { return ViewState.GetOrDefault("Header", true); }
            set { ViewState["Header"] = value; }
        }

        public string HeaderText
        {
            get { return (string) ViewState["HeaderText"]; }
            set { ViewState["HeaderText"] = value; }
        }

        /// <summary>
        /// Show the selected items in a comma seperated list, and how many. If the amount of checkboxes selected is bigger than the number specified here,
        /// either the SelectedText will be used, or the control will generate &quot;&lt;amount&gt; selected&quot;.
        /// </summary>
        public int? SelectedList
        {
            get { return ViewState.GetOrDefault<int?>("SelectedList", 1); }
            set { ViewState["SelectedList"] = value; }
        }

        /// <summary>
        /// The text that will be shown when the amount of selected items is bigger than the value specified at SelectedList.
        /// Use {0} for the amount of items checked, and {1} for the amount of total items. 
        /// </summary>
        public string SelectedText
        {
            get { return (string) ViewState["SelectedText"]; }
            set { ViewState["SelectedText"] = value; }
        }

        public int? MaxSelected
        {
            get { return ViewState.GetOrDefault<int?>("MaxSelected", null); }
            set { ViewState["MaxSelected"] = value; }
        }

        public string NoneSelectedText
        {
            get { return (string) ViewState["NoneSelectedText"]; }
            set { ViewState["NoneSelectedText"] = value; }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            MultiJQueryScriptBuilder builders = new MultiJQueryScriptBuilder(this);
            builders.Prepare();

            JQueryScriptBuilder builder = builders[MethodName];
            if (ShowEffect.Effect != JQueryEffects.None)
                builder.AddOption("show",
                                  string.Format("[\"{0}\", {1}]", ShowEffect.Effect.ToString().ToLower(),
                                                ShowEffect.Timeout), true);
            if (HideEffect.Effect != JQueryEffects.None)
                builder.AddOption("hide",
                                  string.Format("[\"{0}\", {1}]", HideEffect.Effect.ToString().ToLower(),
                                                HideEffect.Timeout), true);
            if (!string.IsNullOrEmpty(HeaderText))
                builder.AddOption("header", HeaderText, true);
            else if (!Header || !Multiple)
                builder.AddOption("header", false);
            if (SelectedList.HasValue)
                builder.AddOption("selectedList", SelectedList.Value);
            if (!string.IsNullOrEmpty(SelectedText))
                builder.AddOption("selectedText",
                                  string.Format(
                                      "function(numChecked, numTotal, checkedItems) {{ return '{0}'.replace('{{0}}', numChecked).replace('{{1}}', numTotal); }}",
                                      SelectedText), true);
            if (MaxSelected.HasValue)
                builder.AddOption("click",
                                  string.Format(
                                      "return ($(this).multiselect('widget').find('input:checked').length > {0};",
                                      MaxSelected.Value), true);
            if (!Multiple)
                builder.AddOption("multiple", false);
            if (!string.IsNullOrEmpty(NoneSelectedText))
                builder.AddOption("noneSelectedText", NoneSelectedText, true);
            if (!string.IsNullOrEmpty(CssClass))
                builder.AddOption("classes", CssClass, true);

            ClientScriptManager.RegisterJQueryControl(this, builders);
            if (ShowEffect != null && ShowEffect.Effect != JQueryEffects.None)
                ClientScriptManager.AddCompositeScript(this, string.Format("jQuery.effects.{0}.js", ShowEffect.Effect.ToString().ToLower()), "jQuery");
            if (HideEffect != null && HideEffect.Effect != JQueryEffects.None)
                ClientScriptManager.AddCompositeScript(this, string.Format("jQuery.effects.{0}.js", HideEffect.Effect.ToString().ToLower()), "jQuery");
            jQuery.RegisterCss(Page, "jquery.multiselect.css");
        }

        protected override void VerifyMultiSelect()
        {
            // Do not throw an exception, multiselect is supported
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            if (Multiple) writer.AddAttribute("multiple", "multiple");

            base.AddAttributesToRender(writer);
        }
    }
}
