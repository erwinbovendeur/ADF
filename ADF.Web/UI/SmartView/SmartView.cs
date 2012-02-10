using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Domain;
using Adf.Core.Extensions;
using Adf.Core.Identity;
using Adf.Core.Objects;
using Adf.Core.Resources;
using Adf.Core.State;
using Adf.Web.UI.Styling;

namespace Adf.Web.UI.SmartView
{
    /// <summary>
    /// Represents a customized <see cref="GridView"/> control.
    /// </summary>
    public class SmartView : GridView
    {
        #region private fields

        private static IStyler _styler = ObjectFactory.BuildUp<IStyler>("BusinessGridViewStyler");
        private bool _autostyle = true;
        private static IEnumerable<IGridService> _services;
        /// <summary>
        /// Gets the collection after addition of element.
        /// </summary>
        private static IEnumerable<IGridService> Services
        {
            get { return _services ?? (_services = ObjectFactory.BuildAll<IGridService>().ToList()); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the properties of business GridView
        /// </summary>
        public SmartView()
        {
            if (DesignMode) return;

            base.EnableViewState = true;
            base.AutoGenerateColumns = false;
            base.CellPadding = 0;
            base.AllowSorting = true;
            base.AllowPaging = true;
            base.UseAccessibleHeader = true;

            foreach (IGridService service in Services) service.InitService(this);
        }

        /// <summary>
        /// Set style on page load.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode && AutoStyle)
                Styler.SetStyles(this);
        }

        #endregion

        #region Properties

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

                IDomainObject[] source = value as IDomainObject[];
                if (source != null)
                {
                    PagerSettings.Visible = (source.Length > PageSize);
                    PageIndex = 0;
                }

                StateManager.Personal[ClientID + ".DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a style of BusinessGrid
        /// </summary>
        [Bindable(true), Category("BusinessGrid"), DefaultValue(true)]
        public bool AutoStyle
        {
            get { return _autostyle; }
            set { _autostyle = value; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(false)]
        public bool FullRowSelect { get; set; }

        /// <summary>
        /// Gets or sets a style of type <see cref="IStyler"/>
        /// </summary>
        /// <returns>Returns a style of type <see cref="IStyler"/>.</returns>
        public static IStyler Styler
        {
            get { return _styler; }
            set { _styler = value; }
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
                if (source != null)
                {
                    return source[index].Id;
                }

                //doing it the old way
                return IdManager.New(DataKeys[SelectedIndex].Value.ToString());
            }
            set
            {
                int index = (DataSource as IEnumerable<IDomainObject>).IndexOf(value);

                PageIndex = index / PageSize;
                SelectedIndex = index % PageSize;

                DataBind();  // rebind otherwise the page is not correctly set
            }
        }

        #endregion

        #region Services

        /// <summary>
        /// Provides an override of GridView sorting.
        /// </summary>
        /// <param name="e"><see cref="GridViewSortEventArgs"/></param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            //delegate sorting to the gridservice(s)
            foreach (IGridService service in Services)
                service.HandleService(GridAction.Sorting, this, e);
        }

        /// <summary>
        /// Provides an override of GridView paging.
        /// </summary>
        /// <param name="e"><see cref="GridViewPageEventArgs"/></param>
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            //delegate paging to the gridservice(s)
            foreach (IGridService service in Services)
                service.HandleService(GridAction.Paging, this, e);
        }

        #endregion

        #region Events

        protected override void OnRowDeleting(GridViewDeleteEventArgs e)
        {
            // Do NOT throw an exception if deleting is not handled, it's probably handled by a DeleteButton elsewhere
            // base.OnRowDeleting(e);
        }

        #endregion

        #region EmptyTable

        /// <summary>
        /// Get or sets an empty table
        /// </summary>
        [Description("Enable or disable generating an empty table with headers if no data rows in source"), Category("Misc"), DefaultValue("true")]
        public bool ShowEmptyTable
        {
            get
            {
                object o = ViewState["ShowEmptyTable"];
                return (o == null || (bool)o);
            }
            set
            {
                ViewState["ShowEmptyTable"] = value;
            }
        }

        /// <summary>
        /// Get or sets wether or not the amount of items should be shown in the footer.
        /// </summary>
        [Description("Enable or disable showing the amount of items in the footer"), Category("Misc"), DefaultValue("false")]
        public bool ShowFooterResults
        {
            get
            {
                object o = ViewState["ShowFooterResults"];
                return (o == null || (bool)o);
            }
            set
            {
                ViewState["ShowFooterResults"] = value;
            }
        }

        /// <summary>
        /// Get or sets wether or not a dropdownlist should be shown in the footer which controls the pagesize.
        /// </summary>
        [Description("Enable or disable showing a dropdown list in the footer which controls the pagesize"), Category("Misc"), DefaultValue("false")]
        public bool ShowFooterPageSize
        {
            get
            {
                object o = ViewState["ShowFooterPageSize"];
                return (o == null || (bool)o);
            }
            set
            {
                ViewState["ShowFooterPageSize"] = value;
            }
        }

        /// <summary>
        /// Get or sets the elements of the pagesize dropdown
        /// </summary>
        [Description("Enable or disable showing a dropdown list in the footer which controls the pagesize"), Category("Misc"), DefaultValue("10,20,50")]
        public string PageSizeElements
        {
            get
            {
                int[] ar = InternalPageSizeElements;
                return ar.ConvertToString(",");
            }
            set
            {
                string[] elms = value.Split(',');
                int[] ielms = new int[elms.Length];
                for (int i = 0; i < elms.Length; i++)
                {
                    ielms[i] = int.Parse(elms[i]);
                }
                InternalPageSizeElements = ielms;
            }
        }

        private int[] InternalPageSizeElements
        {
            get
            {
                object o = ViewState["PageSizeElements"];
                return o == null ? new int[0] : (int[]) o;
            }
            set
            {
                ViewState["PageSizeElements"] = value;
            }
        }

        /// <summary>
        /// Provides the creation of child control of table. It is an CreateChildControls override of <see cref="System.Web.UI.WebControls.GridView"/>
        /// </summary>
        /// <param name="dataSource"><see cref="System.Web.UI.WebControls.GridView"/></param>
        /// <param name="dataBinding"><see cref="System.Web.UI.WebControls.GridView"/></param>
        /// <returns></returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int numRows = base.CreateChildControls(dataSource, dataBinding);

            //no data rows created, create empty table if enabled
            if (numRows == 0 && ShowEmptyTable)
            {
                CreateEmptyTable();
            }
            else if (ShowFooterResults || ShowFooterPageSize)
            {
                CreateControlFooter(dataSource);
            }

            return numRows;
        }

        private void CreateControlFooter(IEnumerable dataSource)
        {
            // Create extra footer control
            GridViewRow gridView = new GridViewRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)
                                       {
                                           CssClass = "BusinessGridControlFooter",
                                           TableSection = TableRowSection.TableFooter
                                       };

            TableCell results;
            WebControl dropdown;

            int columns = Columns.Count;
            if (columns > 1)
            {
                columns += columns % 2 == 1 ? 1 : 0;
                results = new TableCell { ColumnSpan = columns / 2, CssClass = "BusinessGridResults" };
                dropdown = new TableCell { ColumnSpan = columns / 2, CssClass = "BusinessGridPageSize" };
            }
            else
            {
                dropdown = new Panel {CssClass = "BusinessGridPageSize"};
                results = new TableCell();
            }

            // Create the total results field
            if (ShowFooterResults)
            {
                results.Controls.Add(new LiteralControl
                                         {
                                             Text =
                                                 string.Format(ResourceManager.GetString("Total Results: {0}"),
                                                               dataSource.Count())
                                         });
            }
            if (columns == 1) results.Controls.Add(dropdown);
            if (ShowFooterPageSize)
            {
                dropdown.Controls.Add(new LiteralControl {Text = ResourceManager.GetString("Results per page")});

                DropDownList ddl = new DropDownList { AutoPostBack = true };
                foreach (int pageSizeElement in InternalPageSizeElements)
                {
                    ddl.Items.Add(pageSizeElement.ToString(CultureInfo.InvariantCulture));                    
                }
                ddl.SelectedIndexChanged += (sender, e) => PageSize = PageSizeElements[ddl.SelectedIndex];
                dropdown.Controls.Add(ddl);
            }

            if (!ShowFooterPageSize && !ShowFooterResults) return;
            gridView.Cells.AddRange((columns > 1 ? new[] {results, (TableCell) dropdown} : new[] {results}));
            // This should be added to the controls collection

            Table table = Controls.OfType<Table>().FirstOrDefault<Table>();
            if (table != null)
            {
                table.Rows.Add(gridView);
            }
        }

        private void CreateEmptyTable()
        {
            //create table
            Table table = new Table { ID = ID };

            //create a new header row
            GridViewRow row = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //convert the exisiting columns into an array and initialize
            DataControlField[] fields = new DataControlField[Columns.Count];
            Columns.CopyTo(fields, 0);
            InitializeRow(row, fields);
            table.Rows.Add(row);

            if (EmptyDataTemplate != null || string.IsNullOrEmpty(EmptyDataText))
            {
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow,
                                                       DataControlRowState.Normal) { CssClass = "BusinessGridEmpty" };
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
                table.Rows.Add(emptyRow);
            }

            Controls.Clear(); // Remove the current empty row and stuff
            Controls.Add(table);
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (FullRowSelect && e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor: pointer;";
                e.Row.Attributes["onmouseover"] = "javascript:$(this).addClass('hover');";
                e.Row.Attributes["onmouseout"] = "javascript:$(this).removeClass('hover')";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this,
                                                                                           string.Concat("Select$",
                                                                                                         e.Row.
                                                                                                             RowIndex));
            }
        }

        #endregion EmptyTable
    }
}