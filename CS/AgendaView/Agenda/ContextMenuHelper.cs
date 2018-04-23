using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AgendaView
{
    public static class ContextMenuHelper
    {
        public static void FillMenuItems(DevExpress.Web.GridViewContextMenuItemCollection items, DevExpress.Web.GridViewContextMenuType menutype, bool showAll)
        {
            if (menutype == DevExpress.Web.GridViewContextMenuType.Rows)
            {
                items.Add("Open appointment", "OpenAppointment");
                items.Add("Delete appointment", "DeleteAppointment");
            }
            items.Add("Go to the next month", "GoToNextMonth");
            items.Add("Go to the previous month", "GoToPreviousMonth");
            DevExpress.Web.GridViewContextMenuItem item = new DevExpress.Web.GridViewContextMenuItem("Go to the specific date", "GoToSpecificDate");
            items.Add(item);

            DevExpress.Web.GridViewContextMenuItem subItem = new DevExpress.Web.GridViewContextMenuItem("Change view to");
            DevExpress.Web.GridViewContextMenuItem subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Day View", "DayView");
            subItem.Items.Add(subMenuItem);
            subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Work Week View", "WorkWeekView");
            subItem.Items.Add(subMenuItem);
            subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Week View", "WeekView");
            subItem.Items.Add(subMenuItem);
            subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Full Week View", "FullWeekView");
            subItem.Items.Add(subMenuItem);
            subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Month View", "MonthView");
            subItem.Items.Add(subMenuItem);
            subMenuItem = new DevExpress.Web.GridViewContextMenuItem("Timeline View", "TimelineView");
            subItem.Items.Add(subMenuItem);
            items.Add(subItem);

            if (showAll)
                items.Add("Show all appointments", "ShowAll");
            else
                items.Add("Group appointments by resources", "GroupAppointmentsByResources");

        }
    }
}
