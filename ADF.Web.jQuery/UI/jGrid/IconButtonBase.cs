using System;
using System.Web.UI.WebControls;
using Adf.Core;
using Adf.Core.Extensions;
using Adf.Core.Resources;
using DNA.UI.JQuery;

namespace Adf.Web.jQuery.UI
{
    public abstract class IconButtonBase : TemplateField
    {
        public string CommandName { get; set; }
        protected IconStyle IconName { get; set; }
        protected string ColumnStyle { get; set; }
        public string Message { get; set; }
        public string ToolTip { get; set; }
        public string OnClickClick { get; set; }

        public event EventHandler Click;

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (ColumnStyle != null)
            {
                if (HeaderStyle.CssClass.IsNullOrEmpty()) HeaderStyle.CssClass = ColumnStyle;
                if (ItemStyle.CssClass.IsNullOrEmpty()) ItemStyle.CssClass = ColumnStyle;
                if (FooterStyle.CssClass.IsNullOrEmpty()) FooterStyle.CssClass = ColumnStyle;
            }

            if (cellType != DataControlCellType.DataCell) return;
            ButtonEx buttonEx = string.IsNullOrEmpty(Message) ? new ButtonEx() : new MessageButton { Message = Message};
            buttonEx.CommandName = CommandName;
            buttonEx.ToolTip = ResourceManager.GetString(ToolTip);
            buttonEx.LeftIconCssClass = IconName == null ? string.Empty : IconName.Name;
            buttonEx.ShowText = false;
            buttonEx.OnClientClick = OnClickClick;
            if (Click != null)
            {
                if (buttonEx is MessageButton)
                    ((MessageButton) buttonEx).Confirm += Click;
                else
                    buttonEx.Click += Click;
            }
            cell.Controls.Add(buttonEx);
        }
    }

    public class IconStyle : Descriptor
    {
        public IconStyle(string name) : base(name) {}

        public static IconStyle Carat1North = new IconStyle("ui-icon-carat-1-n");
        public static IconStyle Carat1NorthEast = new IconStyle("ui-icon-carat-1-ne");
        public static IconStyle Carat1East = new IconStyle("ui-icon-carat-1-e");
        public static IconStyle Carat1SouthEast = new IconStyle("ui-icon-carat-1-se");
        public static IconStyle Carat1South = new IconStyle("ui-icon-carat-1-s");
        public static IconStyle Carat1SouthWest = new IconStyle("ui-icon-carat-1-sw");
        public static IconStyle Carat1West = new IconStyle("ui-icon-carat-1-w");
        public static IconStyle Carat1NorthWest = new IconStyle("ui-icon-carat-1-nw");
        public static IconStyle Carat2NorthToSouth = new IconStyle("ui-icon-carat-2-n-s");
        public static IconStyle Carat2EastToWest = new IconStyle("ui-icon-carat-2-e-w");
        public static IconStyle Triangle1North = new IconStyle("ui-icon-triangle-1-n");
        public static IconStyle Triangle1NorthEast = new IconStyle("ui-icon-triangle-1-ne");
        public static IconStyle Triangle1East = new IconStyle("ui-icon-triangle-1-e");
        public static IconStyle Triangle1SouthEast = new IconStyle("ui-icon-triangle-1-se");
        public static IconStyle Triangle1South = new IconStyle("ui-icon-triangle-1-s");
        public static IconStyle Triangle1SouthWest = new IconStyle("ui-icon-triangle-1-sw");
        public static IconStyle Triangle1West = new IconStyle("ui-icon-triangle-1-w");
        public static IconStyle Triangle1NorthWest = new IconStyle("ui-icon-triangle-1-nw");
        public static IconStyle Triangle2NorthToSouth = new IconStyle("ui-icon-triangle-2-n-s");
        public static IconStyle Triangle2EastToWest = new IconStyle("ui-icon-triangle-2-e-w");
        public static IconStyle Arrow1North = new IconStyle("ui-icon-arrow-1-n");
        public static IconStyle Arrow1NorthEast = new IconStyle("ui-icon-arrow-1-ne");
        public static IconStyle Arrow1East = new IconStyle("ui-icon-arrow-1-e");
        public static IconStyle Arrow1SouthEast = new IconStyle("ui-icon-arrow-1-se");
        public static IconStyle Arrow1South = new IconStyle("ui-icon-arrow-1-s");
        public static IconStyle Arrow1SouthWest = new IconStyle("ui-icon-arrow-1-sw");
        public static IconStyle Arrow1West = new IconStyle("ui-icon-arrow-1-w");
        public static IconStyle Arrow1NorthWest = new IconStyle("ui-icon-arrow-1-nw");
        public static IconStyle Arrow2NortToSouth = new IconStyle("ui-icon-arrow-2-n-s");
        public static IconStyle Arrow2NorthEastToSouthWest = new IconStyle("ui-icon-arrow-2-ne-sw");
        public static IconStyle Arrow2EastToWest = new IconStyle("ui-icon-arrow-2-e-w");
        public static IconStyle Arrow2SouthEastToNorthWest = new IconStyle("ui-icon-arrow-2-se-nw");
        public static IconStyle ArrowStop1Nort = new IconStyle("ui-icon-arrowstop-1-n");
        public static IconStyle ArrowStop1East = new IconStyle("ui-icon-arrowstop-1-e");
        public static IconStyle ArrowStop1South = new IconStyle("ui-icon-arrowstop-1-s");
        public static IconStyle ArrowStop1West = new IconStyle("ui-icon-arrowstop-1-w");
        public static IconStyle ArrowThick1North = new IconStyle("ui-icon-arrowthick-1-n");
        public static IconStyle ArrowThick1NorthEast = new IconStyle("ui-icon-arrowthick-1-ne");
        public static IconStyle ArrowThick1East = new IconStyle("ui-icon-arrowthick-1-e");
        public static IconStyle ArrowThick1SouthEast = new IconStyle("ui-icon-arrowthick-1-se");
        public static IconStyle ArrowThick1South = new IconStyle("ui-icon-arrowthick-1-s");
        public static IconStyle ArrowThick1SouthWest = new IconStyle("ui-icon-arrowthick-1-sw");
        public static IconStyle ArrowThick1West = new IconStyle("ui-icon-arrowthick-1-w");
        public static IconStyle ArrowThick1NorthWest = new IconStyle("ui-icon-arrowthick-1-nw");
        public static IconStyle ArrowThick2NorthToSouth = new IconStyle("ui-icon-arrowthick-2-n-s");
        public static IconStyle ArrowThick2NorthEastToSouthWest = new IconStyle("ui-icon-arrowthick-2-ne-sw");
        public static IconStyle ArrowThick2EastToWest = new IconStyle("ui-icon-arrowthick-2-e-w");
        public static IconStyle ArrowThick2SouthEastToNorthWest = new IconStyle("ui-icon-arrowthick-2-se-nw");
        public static IconStyle ArrowThickStop1North = new IconStyle("ui-icon-arrowthickstop-1-n");
        public static IconStyle ArrowThickStop1East = new IconStyle("ui-icon-arrowthickstop-1-e");
        public static IconStyle ArrowThickStop1South = new IconStyle("ui-icon-arrowthickstop-1-s");
        public static IconStyle ArrowThickStop1West = new IconStyle("ui-icon-arrowthickstop-1-w");
        public static IconStyle ArrowReturnThick1West = new IconStyle("ui-icon-arrowreturnthick-1-w");
        public static IconStyle ArrowReturnThick1North = new IconStyle("ui-icon-arrowreturnthick-1-n");
        public static IconStyle ArrowReturnThick1East = new IconStyle("ui-icon-arrowreturnthick-1-e");
        public static IconStyle ArrowReturnThick1South = new IconStyle("ui-icon-arrowreturnthick-1-s");
        public static IconStyle ArrowReturn1West = new IconStyle("ui-icon-arrowreturn-1-w");
        public static IconStyle ArrowReturn1North = new IconStyle("ui-icon-arrowreturn-1-n");
        public static IconStyle ArrowReturn1East = new IconStyle("ui-icon-arrowreturn-1-e");
        public static IconStyle ArrowReturn1South = new IconStyle("ui-icon-arrowreturn-1-s");
        public static IconStyle ArrowRefresh1West = new IconStyle("ui-icon-arrowrefresh-1-w");
        public static IconStyle ArrowRefresh1North = new IconStyle("ui-icon-arrowrefresh-1-n");
        public static IconStyle ArrowRefresh1East = new IconStyle("ui-icon-arrowrefresh-1-e");
        public static IconStyle ArrowRefresh1South = new IconStyle("ui-icon-arrowrefresh-1-s");
        public static IconStyle Arrow4 = new IconStyle("ui-icon-arrow-4");
        public static IconStyle Arrow4Diag = new IconStyle("ui-icon-arrow-4-diag");
        public static IconStyle Extlink = new IconStyle("ui-icon-extlink");
        public static IconStyle Newwin = new IconStyle("ui-icon-newwin");
        public static IconStyle Refresh = new IconStyle("ui-icon-refresh");
        public static IconStyle Shuffle = new IconStyle("ui-icon-shuffle");
        public static IconStyle TransferEastToWest = new IconStyle("ui-icon-transfer-e-w");
        public static IconStyle TransferThickEastToWest = new IconStyle("ui-icon-transferthick-e-w");
        public static IconStyle FolderCollapsed = new IconStyle("ui-icon-folder-collapsed");
        public static IconStyle FolderOpen = new IconStyle("ui-icon-folder-open");
        public static IconStyle Document = new IconStyle("ui-icon-document");
        public static IconStyle DocumentB = new IconStyle("ui-icon-document-b");
        public static IconStyle Note = new IconStyle("ui-icon-note");
        public static IconStyle MailClosed = new IconStyle("ui-icon-mail-closed");
        public static IconStyle MailOpen = new IconStyle("ui-icon-mail-open");
        public static IconStyle Suitcase = new IconStyle("ui-icon-suitcase");
        public static IconStyle Comment = new IconStyle("ui-icon-comment");
        public static IconStyle Person = new IconStyle("ui-icon-person");
        public static IconStyle Print = new IconStyle("ui-icon-print");
        public static IconStyle Trash = new IconStyle("ui-icon-trash");
        public static IconStyle Locked = new IconStyle("ui-icon-locked");
        public static IconStyle Unlocked = new IconStyle("ui-icon-unlocked");
        public static IconStyle Bookmark = new IconStyle("ui-icon-bookmark");
        public static IconStyle Tag = new IconStyle("ui-icon-tag");
        public static IconStyle Home = new IconStyle("ui-icon-home");
        public static IconStyle Flag = new IconStyle("ui-icon-flag");
        public static IconStyle Calculator = new IconStyle("ui-icon-calculator");
        public static IconStyle Cart = new IconStyle("ui-icon-cart");
        public static IconStyle Pencil = new IconStyle("ui-icon-pencil");
        public static IconStyle Clock = new IconStyle("ui-icon-clock");
        public static IconStyle Disk = new IconStyle("ui-icon-disk");
        public static IconStyle Calendar = new IconStyle("ui-icon-calendar");
        public static IconStyle Zoomin = new IconStyle("ui-icon-zoomin");
        public static IconStyle Zoomout = new IconStyle("ui-icon-zoomout");
        public static IconStyle Search = new IconStyle("ui-icon-search");
        public static IconStyle Wrench = new IconStyle("ui-icon-wrench");
        public static IconStyle Gear = new IconStyle("ui-icon-gear");
        public static IconStyle Heart = new IconStyle("ui-icon-heart");
        public static IconStyle Star = new IconStyle("ui-icon-star");
        public static IconStyle Link = new IconStyle("ui-icon-link");
        public static IconStyle Cancel = new IconStyle("ui-icon-cancel");
        public static IconStyle Plus = new IconStyle("ui-icon-plus");
        public static IconStyle Plusthick = new IconStyle("ui-icon-plusthick");
        public static IconStyle Minus = new IconStyle("ui-icon-minus");
        public static IconStyle Minusthick = new IconStyle("ui-icon-minusthick");
        public static IconStyle Close = new IconStyle("ui-icon-close");
        public static IconStyle Closethick = new IconStyle("ui-icon-closethick");
        public static IconStyle Key = new IconStyle("ui-icon-key");
        public static IconStyle Lightbulb = new IconStyle("ui-icon-lightbulb");
        public static IconStyle Scissors = new IconStyle("ui-icon-scissors");
        public static IconStyle Clipboard = new IconStyle("ui-icon-clipboard");
        public static IconStyle Copy = new IconStyle("ui-icon-copy");
        public static IconStyle Contact = new IconStyle("ui-icon-contact");
        public static IconStyle Image = new IconStyle("ui-icon-image");
        public static IconStyle Video = new IconStyle("ui-icon-video");
        public static IconStyle Script = new IconStyle("ui-icon-script");
        public static IconStyle Alert = new IconStyle("ui-icon-alert");
        public static IconStyle Info = new IconStyle("ui-icon-info");
        public static IconStyle Notice = new IconStyle("ui-icon-notice");
        public static IconStyle Help = new IconStyle("ui-icon-help");
        public static IconStyle Check = new IconStyle("ui-icon-check");
        public static IconStyle Bullet = new IconStyle("ui-icon-bullet");
        public static IconStyle RadioOff = new IconStyle("ui-icon-radio-off");
        public static IconStyle RadioOn = new IconStyle("ui-icon-radio-on");
        public static IconStyle PinWest = new IconStyle("ui-icon-pin-w");
        public static IconStyle PinSouth = new IconStyle("ui-icon-pin-s");
        public static IconStyle Play = new IconStyle("ui-icon-play");
        public static IconStyle Pause = new IconStyle("ui-icon-pause");
        public static IconStyle SeekNext = new IconStyle("ui-icon-seek-next");
        public static IconStyle SeekPrev = new IconStyle("ui-icon-seek-prev");
        public static IconStyle SeekEnd = new IconStyle("ui-icon-seek-end");
        public static IconStyle SeekFirst = new IconStyle("ui-icon-seek-first");
        public static IconStyle Stop = new IconStyle("ui-icon-stop");
        public static IconStyle Eject = new IconStyle("ui-icon-eject");
        public static IconStyle VolumeOff = new IconStyle("ui-icon-volume-off");
        public static IconStyle VolumeOn = new IconStyle("ui-icon-volume-on");
        public static IconStyle Power = new IconStyle("ui-icon-power");
        public static IconStyle SignalDiag = new IconStyle("ui-icon-signal-diag");
        public static IconStyle Signal = new IconStyle("ui-icon-signal");
        public static IconStyle Battery0 = new IconStyle("ui-icon-battery-0");
        public static IconStyle Battery1 = new IconStyle("ui-icon-battery-1");
        public static IconStyle Battery2 = new IconStyle("ui-icon-battery-2");
        public static IconStyle Battery3 = new IconStyle("ui-icon-battery-3");
        public static IconStyle CirclePlus = new IconStyle("ui-icon-circle-plus");
        public static IconStyle CircleMinus = new IconStyle("ui-icon-circle-minus");
        public static IconStyle CircleClose = new IconStyle("ui-icon-circle-close");
        public static IconStyle CircleTriangleEast = new IconStyle("ui-icon-circle-triangle-e");
        public static IconStyle CircleTriangleSouth = new IconStyle("ui-icon-circle-triangle-s");
        public static IconStyle CircleTriangleWest = new IconStyle("ui-icon-circle-triangle-w");
        public static IconStyle CircleTriangleNorth = new IconStyle("ui-icon-circle-triangle-n");
        public static IconStyle CircleArrowEast = new IconStyle("ui-icon-circle-arrow-e");
        public static IconStyle CircleArrowSouth = new IconStyle("ui-icon-circle-arrow-s");
        public static IconStyle CircleArrowWest = new IconStyle("ui-icon-circle-arrow-w");
        public static IconStyle CircleArrowNorth = new IconStyle("ui-icon-circle-arrow-n");
        public static IconStyle CircleZoomin = new IconStyle("ui-icon-circle-zoomin");
        public static IconStyle CircleZoomout = new IconStyle("ui-icon-circle-zoomout");
        public static IconStyle CircleCheck = new IconStyle("ui-icon-circle-check");
        public static IconStyle CirclesmallPlus = new IconStyle("ui-icon-circlesmall-plus");
        public static IconStyle CirclesmallMinus = new IconStyle("ui-icon-circlesmall-minus");
        public static IconStyle CirclesmallClose = new IconStyle("ui-icon-circlesmall-close");
        public static IconStyle SquaresmallPlus = new IconStyle("ui-icon-squaresmall-plus");
        public static IconStyle SquaresmallMinus = new IconStyle("ui-icon-squaresmall-minus");
        public static IconStyle SquaresmallClose = new IconStyle("ui-icon-squaresmall-close");
        public static IconStyle GripDottedVertical = new IconStyle("ui-icon-grip-dotted-vertical");
        public static IconStyle GripDottedHorizontal = new IconStyle("ui-icon-grip-dotted-horizontal");
        public static IconStyle GripSolidVertical = new IconStyle("ui-icon-grip-solid-vertical");
        public static IconStyle GripSolidHorizontal = new IconStyle("ui-icon-grip-solid-horizontal");
        public static IconStyle GripsmallDiagonalSouthEast = new IconStyle("ui-icon-gripsmall-diagonal-se");
        public static IconStyle GripDiagonalSouthEast = new IconStyle("ui-icon-grip-diagonal-se");
    }
}
