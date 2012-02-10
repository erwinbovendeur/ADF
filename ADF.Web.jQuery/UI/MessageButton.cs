using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Resources;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.UI
{
    [ParseChildren(true)]
    public class MessageButton : ButtonEx
    {
        private Dialog dialog;

        /// <summary>
        /// Gets or sets the message that is displayed in the confirmation box.
        /// </summary>
        /// <returns>The message to display.</returns>
        [Bindable(true), Category("MessageButton")]
        public string Message
        {
            get { return (string)ViewState["Message"]; }
            set { ViewState["Message"] = value; }
        }

        public event EventHandler Confirm;

        public virtual void OnConfirm()
        {
            if (!string.IsNullOrEmpty(CommandName)) OnCommand(new CommandEventArgs(CommandName, CommandArgument));
            if (Confirm != null) Confirm(this, EventArgs.Empty);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Button; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            OnClientClick = "return false;";
            Attributes.Add("value", Text);
            Attributes.Add("role", "button");

            dialog = new Dialog
                            {
                                ShowModal = true,
                                ZIndex = 1000,
                                MessageText = ResourceManager.GetString(Message),
                                Title = ResourceManager.GetString("Remove"),
                                Trigger = { TargetID = UniqueID.Replace('$', '_') },
                                ShowEffect = JQueryEffects.Fade,
                                HideEffect = JQueryEffects.Fade
                            };
            dialog.Buttons.Add(new DialogButton { Text = ResourceManager.GetString("OK"), CommandName = "OK"});
            dialog.Buttons.Add(new DialogButton { Text = ResourceManager.GetString("Cancel"), CommandName = "Cancel", OnClientClick = "$(this).dialog('close');"});
            dialog.ButtonCommand += OnDialogButton;
            Controls.Add(dialog);
        }

        private void OnDialogButton(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "OK") OnConfirm();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            dialog.RenderControl(writer);
        }
    }
}
