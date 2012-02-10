﻿namespace Adf.Web.UI.SmartView
{
    public class DeleteButton : IconButton
    {
        public DeleteButton()
        {
            Message = "Are you sure?";
            CommandName = "Delete";
            Image = "bullet_delete.gif";
            Header = "Delete?";
            ToolTip = "Delete current item";
        }
    }
}
