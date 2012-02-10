using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Domain;
using DNA.UI;
using DNA.UI.JQuery;
using ClientScriptManager = DNA.UI.ClientScriptManager;

namespace Adf.Web.jQuery.UI
{
    public class TextColumn : Web.UI.SmartView.TextColumn
    {
        public TextColumn()
        {
            ColumnStyle = "adfj-column-text";
        }
    }

    public class TextButton : Web.UI.SmartView.TextButton
    {
        public TextButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-text";
        }
    }

    public class TooltipColumn : Web.UI.SmartView.TooltipColumn
    {
        public TooltipColumn()
        {
            ColumnStyle = "adfj-column-tooltip";
        }
    }

    public class SelectButton : Web.UI.SmartView.SelectButton
    {
        public SelectButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-select";
        }
    }

    public class NumberColumn : Web.UI.SmartView.NumberColumn
    {
        public NumberColumn()
        {
            ColumnStyle = "adfj-column-number";
        }
    }

    public class NumberButton : Web.UI.SmartView.NumberButton
    {
        public NumberButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-number";
        }
    }

    public class MoneyColumn : Web.UI.SmartView.MoneyColumn
    {
        public MoneyColumn()
        {
            ColumnStyle = "adfj-column-money";
        }
    }

    public class LinkColumn : Web.UI.SmartView.LinkColumn
    {
        public LinkColumn()
        {
            ColumnStyle = "adfj-column-link";
        }
    }

    public class IconColumn : Web.UI.SmartView.IconColumn
    {
        public IconColumn()
        {
            ColumnStyle = "adfj-column-icon";
        }
    }

    public class IconButton : Web.UI.SmartView.IconButton
    {
        public IconButton()
        {
            ColumnStyle = "adfj-column-icon adfj-column-button";
        }
    }

    public class EditButton : IconButtonBase
    {
        public EditButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-edit";
            ToolTip = "Edit current item";
            CommandName = "Edit";
            IconName = IconStyle.Pencil;
        }
    }

    public class DeleteButton : IconButtonBase
    {
        public DeleteButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-delete";
            ToolTip = "Delete current item";
            IconName = IconStyle.Trash;
            CommandName = "Delete";
            Message = "Are you sure?";
        }
    }

    public class DateColumn : Web.UI.SmartView.DateColumn
    {
        public DateColumn()
        {
            ColumnStyle = "adfj-column-date";
        }
    }

    public class DateButton : Web.UI.SmartView.DateButton
    {
        public DateButton()
        {
            ColumnStyle = "adfj-column-button adfj-column-date";
        }
    }

    [JQuery(Assembly = "Adf.Web.jQuery", ScriptResourceBaseName = "Adf.Web.jQuery.", ScriptResources = new[] { "jquery.checkbox.js" })]
    [JQuery(Name = "checkbox", Assembly = "jQuery", ScriptResources = new[] { "ui.core.js" })]
    public class CheckboxColumn : Web.UI.SmartView.CheckboxColumn
    {
        private CheckBox chkBox;

        public CheckboxColumn()
        {
            ColumnStyle = "adfj-column-checkbox";
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            chkBox = new CheckBox {ID = "chkBool", Enabled = false};
            cell.Controls.Add(chkBox);

            base.InitializeCell(cell, cellType, rowState, rowIndex);
        }

        protected override void ItemDataBinding(object sender, System.EventArgs e)
        {
            var cell = sender as TableCell;
            if (cell == null) return;

            var item = cell.NamingContainer as GridViewRow;
            if (item == null || item.DataItem == null) return;

            bool? value = PropertyHelper.GetValue(item.DataItem, DataField) as bool?;
            chkBox.Checked = value != null && value.Value;
        }
    }

    public class BoolColumn : Web.UI.SmartView.BoolColumn
    {
        public BoolColumn()
        {
            ColumnStyle = "adfj-column-bool";
        }
    }

    public class AccountColumn : Web.UI.SmartView.AccountColumn
    {
        public AccountColumn()
        {
            ColumnStyle = "adfj-column-account";
        }
    }
}
