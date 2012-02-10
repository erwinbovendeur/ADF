using Adf.Web.Extensions;
using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientScriptManager = DNA.UI.ClientScriptManager;

namespace Adf.Web.jQuery.UI
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "ContentTemplate")]
    public class PanelEx : CompositeControl
    {
        private Panel headerPanel;
        private readonly Panel body = new Panel { CssClass = "adfj-panel-content ui-widget-content ui-corner-bottom" };

        [Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), TemplateInstance(TemplateInstance.Single), TemplateContainer(typeof(PanelEx))]
        public ITemplate HeaderTemplate { get; set; }

        [Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), TemplateInstance(TemplateInstance.Single), TemplateContainer(typeof(PanelEx))]
        public ITemplate ContentTemplate { get; set; }

        /// <summary>
        /// Gets/Sets the Header Title text
        /// </summary>
        [Category("Appearance")]
        public string Title
        {
            get { return ViewState.GetOrDefault("Title", string.Empty); }
            set { ViewState["Title"] = value; }
        }

        public bool ShowHeader
        {
            get { return ViewState.GetOrDefault("ShowHeader", true); }
            set { ViewState["ShowHeader"] = value; }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ClientScriptManager.RegisterJQueryControl(this);
        }

        protected override void CreateChildControls()
        {
            headerPanel = new Panel { CssClass = "adfj-panel-header ui-state-active ui-corner-top" };
            body.ID = "body";
            if (HeaderTemplate != null)
            {
                HeaderTemplate.InstantiateIn(headerPanel);
            }
            else
            {
                headerPanel.Controls.Add(new Literal { Text = Title });
            }
            headerPanel.ID = "header";
            headerPanel.Visible = ShowHeader;
            Controls.Add(headerPanel);
            Controls.Add(body);
            if (ContentTemplate != null)
                ContentTemplate.InstantiateIn(body);
        }
/*
        private void CreateHeader()
        {
            // Create the controls required for this panel
            if (HeaderTemplate != null || !string.IsNullOrEmpty(HeaderText))
            {
                Panel header = new Panel { CssClass = "adfj-panel-header ui-state-active ui-corner-top" };
                if (HeaderTemplate != null)
                {
                    HeaderTemplate.InstantiateIn(header);
                }
                else if (HeaderText.Length > 0)
                {
                    header.Controls.Add(new Literal { Text = HeaderText });
                }
                Controls.AddAt(0, header);
            }
        }
        
        private class PanelContainer : Panel, INamingContainer
        {
        }

        [DefaultValue(null), Browsable(false), TemplateContainer(typeof(PanelContainer)), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public virtual ITemplate HeaderTemplate { get; set; }

        [Browsable(false)]
        public virtual Control Body { get { return body; } }

        [Themeable(true), DefaultValue(""), Bindable(true), Category("Appearance"), Description("Gets or sets the header text")]
        public string HeaderText
        {
            get { return (string) ViewState["HeaderText"]; }
            set { ViewState["HeaderText"] = value; }
        }

        protected void MoveChildControlsToBody()
        {
            // Move all controls to the body element
            Control[] controls = new Control[Controls.Count];
            Controls.CopyTo(controls, 0);
            Controls.Clear();
            foreach (Control control in controls)
            {
                body.Controls.Add(control);
            }
        }
        
        protected override void OnPreRender(System.EventArgs e)
        {
            CreateHeader();
            CssClass = ("adfj-panel ui-widget " + CssClass).Trim();

            base.OnPreRender(e);
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
        }
 */
    }
}
