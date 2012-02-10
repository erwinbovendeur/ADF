using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Base.Domain;
using Adf.Core.Domain;
using Adf.Core.Extensions;
using Adf.Core.Identity;
using Adf.Core.Objects;
using Adf.Core.Resources;
using Adf.Core.State;
using Adf.Web.Extensions;
using Adf.Web.Helper;
using Adf.Web.UI.Styling;
using DNA.UI;
using DNA.UI.JQuery;
using ClientScriptManager = DNA.UI.ClientScriptManager;

namespace Adf.Web.jQuery.UI
{
    public enum ButtonType
    {
        Button,
        Icon
    }

    public interface ISortOrder
    {
        int? Order { get; set; }
    }

    public class jGridItemSortedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public int NewIndex { get; private set; }

        public jGridItemSortedEventArgs(int index, int newIndex)
        {
            Index = index;
            NewIndex = newIndex;
        }

        public void ApplySortEvent<T>(DomainCollection<T> collection, bool save = false) where T : ISortOrder, IDomainObject
        {
            T obj = collection[Index];
            collection.RemoveAt(Index);
            collection.Insert(NewIndex, obj);

            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Order = i + 1;
                if (save) collection[i].Save();
            }
        }
    }

    [JQuery(Assembly = "jQuery", ScriptResources = new[] { "ui.core.js", "ui.widget.js", "ui.mouse.js", "ui.sortable.js" }, StartEvent = ClientRegisterEvents.DocumentReady)]
    [JQuery(Name = "jGrid", Assembly = "Adf.Web.jQuery", ScriptResourceBaseName = "Adf.Web.jQuery.", ScriptResources = new[] { "jgrid.js" }, StartEvent = ClientRegisterEvents.DocumentReady)]
    public class jGrid : GridView
    {
        private static IEnumerable<IGridService> _services;
        /// <summary>
        /// Gets the collection after addition of element.
        /// </summary>
        private static IEnumerable<IGridService> Services
        {
            get { return _services ?? (_services = ObjectFactory.BuildAll<IGridService>().ToList()); }
        }

        private class InputCheckBoxField : CheckBoxField
        {
            public const string CheckBoxID = "CheckBoxButton";

            protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
            {
                base.InitializeDataCell(cell, rowState);
                if (cell.Controls.Count != 0) return;
                CheckBox chk = new CheckBox { ID = CheckBoxID };
                cell.Controls.Add(chk);
            }
        }

        private static readonly IStyler _styler = ObjectFactory.BuildUp<IStyler>("jGridViewStyler");

        [DefaultValue(false)]
        public bool NonSelectable
        {
            get { return ViewState.GetOrDefault("nonselectable", false); }
            set { ViewState["nonselectable"] = value; }
        }

        [DefaultValue(false)]
        public bool FullRowSelect
        {
            get { return ViewState.GetOrDefault("fullrowselect", false); }
            set { ViewState["fullrowselect"] = value; }
        }

        [Category("Behavior"), Description("Indicates whether this grid supports multiple selected items"), DefaultValue(false), Themeable(true)]
        public bool MultiSelect
        {
            get { return ViewState.GetOrDefault("multiselect", false); }
            set { ViewState["multiselect"] = value; }
        }

        [Category("Behavior"), Description("Indicates the position of the checkboxcolumn. If left null or a negative value, the checkbox will be hidden"), DefaultValue(null), Themeable(true)]
        public int? CheckboxColumn
        {
            get { return ViewState.GetOrDefault<int?>("CheckboxColumn", null); }
            set { ViewState["CheckboxColumn"] = value; }
        }

        [Category("Behavior"), Description("Indicates whether this grid will show first, next, previous and last buttons"), DefaultValue(false), Themeable(true)]
        public bool ShowNextPrevButtons
        {
            get { return ViewState.GetOrDefault("shownextprev", false); }
            set { ViewState["shownextprev"] = value; }
        }

        [Category("Behavior"), Description("Indicates whether this grid will show a label with total amount of records"), DefaultValue(false), Themeable(true)]
        public bool ShowTotal
        {
            get { return ViewState.GetOrDefault("showtotal", false); }
            set { ViewState["showtotal"] = value; }
        }

        [Category("Behavior"), Description("Indicates whether this grid will show a label with total amount of records"), DefaultValue(ButtonType.Icon), Themeable(true)]
        public ButtonType ButtonType
        {
            get { return ViewState.GetOrDefault("buttontype", ButtonType.Icon); }
            set { ViewState["buttontype"] = value; }
        }

        [Category("Behavior"), Description("Indicates whether the user is allowed to reorder the contents of the records"), DefaultValue(false), Themeable(false)]
        public bool AllowReorder
        {
            get { return ViewState.GetOrDefault("allowreorder", false); }
            set
            {
                if (value && (AllowPaging || AllowSorting))
                    throw new ArgumentException("You cannot enable Paging, Sorting and Sortable. If Sortable is enabled, disable Sorting and Paging.");
                ViewState["allowreorder"] = value;
            }
        }

        public delegate void ItemSortedEventHandler(object sender, jGridItemSortedEventArgs e);
        public event ItemSortedEventHandler ItemSortedEvent;

        protected virtual void OnItemSortedEvent(jGridItemSortedEventArgs e)
        {
            if (ItemSortedEvent != null)
                ItemSortedEvent(this, e);
        }

        public override bool AllowPaging
        {
            get { return base.AllowPaging; }
            set
            {
                if (value && AllowReorder)
                    throw new ArgumentException("You cannot enable Paging, Sorting and Sortable. If Sortable is enabled, disable Sorting and Paging.");
                base.AllowPaging = value;
            }
        }

        public override bool AllowSorting
        {
            get { return base.AllowSorting; }
            set
            {
                if (value && AllowReorder)
                    throw new ArgumentException("You cannot enable Paging, Sorting and Sortable. If Sortable is enabled, disable Sorting and Paging.");
                base.AllowSorting = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            UseAccessibleHeader = true;
            foreach (IGridService service in Services) service.InitService(this);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            _styler.SetStyles(this);
            base.OnLoad(e);
        }

        private static void UpdateSortControls(TableRow row, string sortExpression, SortDirection sortDirection)
        {
            foreach (TableCell tableCell in row.Cells)
            {
                if (!tableCell.HasControls()) continue;
                LinkButton lb = tableCell.Controls[0] as LinkButton;
                if (lb == null) continue;

                // Add sort image control behind the link
                Label lbl = tableCell.Controls.Count == 2 && tableCell.Controls[1] is Label ? (Label) tableCell.Controls[1] : new Label();
                lbl.CssClass = "ui-icon ui-icon-carat-2-n-s";
                if (lb.CommandArgument == sortExpression)
                    lbl.CssClass = sortDirection == SortDirection.Ascending ? "ui-icon ui-icon-carat-1-n" : "ui-icon ui-icon-carat-1-s";
                if (lbl.Parent != tableCell) tableCell.Controls.Add(lbl);
            }
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    e.Row.TableSection = TableRowSection.TableHeader;
                    UpdateSortControls(e.Row, SortExpression, SortDirection);
                    break;
                case DataControlRowType.Footer:
                case DataControlRowType.Pager:
                    e.Row.TableSection = TableRowSection.TableFooter;
                    break;
                case DataControlRowType.DataRow:
                    if (!NonSelectable && FullRowSelect && !MultiSelect)
                    {
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this, string.Concat("Select$", e.Row.RowIndex));
                    }
                    if (NonSelectable)
                    {
                        IEnumerable<TableCell> cells = ControlHelper.List<TableCell>(e.Row);
                        cells.ForEach(c => c.Attributes["style"] = "cursor: default;");
                    }
                    break;
            }

            base.OnRowCreated(e);
        }

        protected override void PrepareControlHierarchy()
        {
            base.PrepareControlHierarchy();

            // Loop through all rows, and add the onchange event
            if (!AllowReorder) return;

            for (int i = 0; i < Rows.Count; i++)
            {
                
                if (Rows[i].TableSection == TableRowSection.TableBody)
                {
                    Rows[i].Attributes["onchanged"] = Page.ClientScript.GetPostBackEventReference(this, string.Format("Reorder${0}${{newIndex}}", i));
                }
            }
        }

        protected override void RaisePostBackEvent(string eventArgument)
        {
            if (AllowReorder)
            {
                if (eventArgument.StartsWith("Reorder$")) // Sorted eventarg
                {
                    string[] eventArgs = eventArgument.Split('$');
                    int prevIdx, newIdx;
                    if (eventArgs.Length == 3 && int.TryParse(eventArgs[1], out prevIdx) && int.TryParse(eventArgs[2], out newIdx))
                    {
                        // Find the correct item from the datasource
                        OnItemSortedEvent(new jGridItemSortedEventArgs(prevIdx, newIdx));
                    }
                }
            }

            base.RaisePostBackEvent(eventArgument);
        }

        private int TotalRecords
        {
            get
            {
                object ds = DataSource;
                return ds is ICollection ? ((ICollection) ds).Count : -1;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            JQueryScriptBuilder builder = new JQueryScriptBuilder(this);
            if (MultiSelect && !NonSelectable) builder.AddOption("multiselect", true);
            if (NonSelectable) builder.AddOption("nonselectable", true);
            if (!AllowSorting) builder.AddOption("allowsort", false);
            if (AllowPaging)
            {
                builder.AddOption("pagesize", PageSize);
                builder.AddOption("pageindex", PageIndex);
                builder.AddOption("pagecount", PageCount);
                builder.AddOption("totalitems", TotalRecords);
                builder.AddOption("next", ResourceManager.GetString("Next"), true);
                builder.AddOption("previous", ResourceManager.GetString("Previous"), true);
                builder.AddOption("first", ResourceManager.GetString("First"), true);
                builder.AddOption("last", ResourceManager.GetString("Last"), true);
                builder.AddOption("totalmsg", ResourceManager.GetString("Showing _MIN_ to _MAX_ of _TOTAL_"), true);
            }
            if (ShowNextPrevButtons) builder.AddOption("shownextprev", true);
            if (ShowTotal) builder.AddOption("showtotal", true);
            if (ButtonType != ButtonType.Icon) builder.AddOption("iconbuttons", false);
            if (CheckboxColumn.HasValue && CheckboxColumn.Value >= 0 && CheckboxColumn.Value < Columns.Count)
                builder.AddOption("checkboxindex", CheckboxColumn.Value);
            else
                builder.AddOption("hidecheckboxes", true);
            if (AllowReorder)
            {
                builder.AddOption("reorder", true);
            }

            ClientScriptManager.RegisterJQueryControl(this, builder);
        }

        /// <summary>
        /// Gets or sets client identification of datasource
        /// </summary>
        public override object DataSource
        {
            get
            {
                if (DesignMode) return base.DataSource;

                return StateManager.Personal[ClientID + ".DataSource"];
            }
            set
            {
                if (DesignMode)
                {
                    base.DataSource = value;
                    return;
                }

                if (value != null)
                {
                    IDomainObject[] source = value as IDomainObject[];
                    if (source != null)
                    {
                        PagerSettings.Visible = AllowPaging && (source.Length > PageSize);
                        PageIndex = 0;
                    }
                }

                StateManager.Personal[ClientID + ".DataSource"] = value;
            }
        }

        private ICollection columns;
        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            if (!MultiSelect)
            {
                return base.CreateColumns(dataSource, useDataSource);
            }

            if (columns == null)
            {
                ArrayList list = new ArrayList(base.CreateColumns(dataSource, useDataSource));
                InputCheckBoxField field = new InputCheckBoxField { ReadOnly = true };
                if (!CheckboxColumn.HasValue || CheckboxColumn.Value < 0 || CheckboxColumn.Value > list.Count)
                    list.Add(field);
                else
                    list.Insert(CheckboxColumn.Value, field);
                columns = list;
            }
            return columns;
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int numRows = base.CreateChildControls(dataSource, dataBinding);

            if (numRows == 0 && (EmptyDataTemplate != null || !string.IsNullOrEmpty(EmptyDataText)))
            {
                CreateEmptyTable();
            }

            return numRows;
        }

        private void CreateEmptyTable()
        {
            _styler.SetStyles(this); // This method will be called before the OnLoad

            //create table
            Table table = new Table { ID = ID };

            //create a new header row
            GridViewRow row = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //convert the exisiting columns into an array and initialize
            DataControlField[] fields = new DataControlField[Columns.Count];
            Columns.CopyTo(fields, 0);
            InitializeRow(row, fields);
            table.Rows.Add(row);

            GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow,
                                                    DataControlRowState.Normal);
            TableCell cell = new TableCell { ColumnSpan = fields.Length };

            if (EmptyDataTemplate != null)
            {
                EmptyDataTemplate.InstantiateIn(cell);
            }
            else
            {
                cell.Controls.Add(new LiteralControl(ResourceManager.GetString(EmptyDataText)));
            }
            emptyRow.Cells.Add(cell);
            emptyRow.ApplyStyle(EmptyDataRowStyle);
            table.Rows.Add(emptyRow);

            Controls.Clear(); // Remove the current empty row and stuff
            table.CssClass = CssClass;
            Controls.Add(table);
        }

        protected override void OnRowDeleting(GridViewDeleteEventArgs e)
        {
            // Prevent the RowDeleting unhandled exception from being thrown
        }

        public virtual List<int> SelectedIndices
        {
            get
            {
                List<int> indices = new List<int>();
                for (int i = 0; i < Rows.Count; i++)
                {
                    // Retrieve the reference to the checkbox
                    CheckBox cb = (CheckBox)Rows[i].FindControl(InputCheckBoxField.CheckBoxID);
                    if (cb == null) continue;
                    if (cb.Checked) indices.Add(i);
                }
                return indices;
            }
            private set
            {
                if (value == null || value.Count == 0) return;
                foreach (int t in value)
                {
                    if (Rows.Count <= t) continue;
                    // Retrieve the checkbox and select it
                    CheckBox cb = (CheckBox)Rows[t].FindControl(InputCheckBoxField.CheckBoxID);
                    if (cb != null) cb.Checked = true;
                }
            }
        }

        private List<object> SelectedDatakeys
        {
            get { return ViewState.GetOrDefault("SelectedDataKeys", new List<object>()); }
            set { ViewState["SelectedDataKeys"] = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual object[] SelectedValues
        {
            get
            {
                if ((DataKeyNames == null) || (DataKeyNames.Length == 0))
                {
                    throw new InvalidOperationException("jGrid_DataKeyNamesMustBeSpecified");
                }

                // Paging can be turned on. In that case, we'll have to remember the selections made on the other pages
                // Therefor, the list of datakeys for this page should be processed in full, and checked items should be
                // added, while unchecked items should be removed from the selected list

                List<DataKey> selectedItems = (from selectedIndex in SelectedIndices
                                               where selectedIndex < DataKeys.Count && (selectedIndex > -1)
                                               select DataKeys[selectedIndex]).ToList();
                foreach (DataKey dataKey in DataKeys)
                {
                    SelectedDatakeys.Remove(dataKey.Value);
                }
                foreach (DataKey selectedItem in selectedItems)
                {
                    SelectedDatakeys.Add(selectedItem.Value);
                }
    
                return SelectedDatakeys.ToArray();
            }
            set
            {
                if (value == null || value.Length == 0) return;
                if ((DataKeyNames == null || (DataKeyNames.Length == 0)))
                {
                    throw new InvalidOperationException("jGrid_DataKeyNamesMustBeSpecified");
                }
                SetSelectedDatakeys(value);
            }
        }

        /// <summary>
        /// Gets the value of selected index of GridView
        /// </summary>
        public ID Current
        {
            get
            {
                IDomainObject[] source = DataSource as IDomainObject[];
                int index = (PageSize * (PageIndex) + SelectedIndex);
                return source != null ? source[index].Id : IdManager.New(DataKeys[SelectedIndex].Value.ToString());
            }
            set
            {
                int index = (DataSource as IEnumerable<IDomainObject>).IndexOf(value);

                PageIndex = index / PageSize;
                SelectedIndex = index % PageSize;

                DataBind();  // rebind otherwise the page is not correctly set
            }
        }

        /// <summary>
        /// Gets the value of selected index of GridView
        /// </summary>
        public IEnumerable<ID> CurrentSelected
        {
            get
            {
                IDomainObject[] source = DataSource as IDomainObject[];
                return SelectedIndices.Select(selectedIndex => (PageSize * (PageIndex) + selectedIndex)).Select(index => source != null ? source[index].Id : IdManager.New(DataKeys[SelectedIndex].Value.ToString()));
            }
        }

        #region Services

        /// <summary>
        /// Provides an override of GridView sorting.
        /// </summary>
        /// <param name="e"><see cref="GridViewSortEventArgs"/></param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            object[] dataKeys = SelectedValues;

            //delegate sorting to the gridservice(s)
            foreach (IGridService service in Services)
                service.HandleService(GridAction.Sorting, this, e);

            SetSelectedDatakeys(dataKeys);

            // Update sort controls
            UpdateSortControls(HeaderRow, e.SortExpression, e.SortDirection);
        }

        /// <summary>
        /// Provides an override of GridView paging.
        /// </summary>
        /// <param name="e"><see cref="GridViewPageEventArgs"/></param>
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            object[] dataKeys = SelectedValues;

            //delegate paging to the gridservice(s)
            foreach (IGridService service in Services)
                service.HandleService(GridAction.Paging, this, e);

            SetSelectedDatakeys(dataKeys);
        }

        #endregion

        /// <summary>
        /// This method will set the selectedindices for the current page. The arguments contains a list of all selected datakeys,
        /// while the DataKeys property will only hold the datakeys for the current page. This method will find the corresponding 
        /// selected index for a datakey, and select the checkbox with that key, marking the row as selected.
        /// </summary>
        /// <param name="keys">All selected datakeys</param>
        private void SetSelectedDatakeys(IEnumerable<object> keys)
        {
            SelectedDatakeys = new List<object>(keys);
            List<int> indices = new List<int>();
            for (int i = 0; i < DataKeys.Count; i++)
            {
                if (SelectedDatakeys.Exists(o => o.Equals(DataKeys[i].Value)))
                {
                    indices.Add(i);
                }
            }
            SelectedIndices = indices;
        }

        public void SetSelectedItems(IEnumerable<IDomainObject> objects)
        {
            if (objects == null || !objects.Any()) return;
            if ((DataKeys == null || (DataKeys.Count == 0)))
            {
                throw new InvalidOperationException("jGrid_DataKeyNamesMustBeSpecified");
            }
            List<object> keys = new List<object>();
            for (int i = 0; i < DataKeys.Count; i++)
            {
                if (objects.FirstOrDefault(o => o.Id.Equals(DataKeys[i].Value)) != null)keys.Add(DataKeys[i].Value);
            }
            SetSelectedDatakeys(keys);
        }
    }
}
