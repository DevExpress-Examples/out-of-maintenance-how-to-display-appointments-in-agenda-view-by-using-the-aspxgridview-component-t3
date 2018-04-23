using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AgendaView
{
    public partial class AgendaViewControl : System.Web.UI.UserControl
    {
#region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
                InitializeGridControlResources();
                BindAppointmentsGrid();
        }
#endregion
#region Properties
        public ASPxScheduler OwnerScheduler { get; set; }
        public bool ShowResources
        {
            get
            {
                if (Session["ShowResources"] == null)
                    Session["ShowResources"] = false;

                return (bool)Session["ShowResources"];
            }
            set
            {
                Session["ShowResources"] = value;
            }
        }
        public object SelectedResourceId
        {
            get { 
                if (Session["selectedResourceId"] == null)
                    Session["selectedResourceId"] = GridControlResources.GetCardValues(GridControlResources.FocusedCardIndex, "Id");

                return Session["selectedResourceId"];
            }
            set
            {
                Session["selectedResourceId"] = value;
            }
        }
        public TimeInterval SelectedInterval
        {
            get
            {
                if (Session["selectedInterval"] == null)
                {
                    DateTime selectedIntervalStart = OwnerScheduler.ActiveView.GetVisibleIntervals().Start;
                    DateTime intervalStart = new DateTime(selectedIntervalStart.Year, selectedIntervalStart.Month, 1);
                    TimeInterval interval = new TimeInterval(intervalStart, intervalStart.AddMonths(1));
                    Session["selectedInterval"] = interval;
                }

                TimeInterval ti = Session["selectedInterval"] as TimeInterval;
                return ti;
            }
            set
            {
                Session["selectedInterval"] = value;
            }
        }
        public bool Active
        {
            get
            {
                return this.Visible;
            }
        }
#endregion
#region methods
        void InitializeGridControlResources()
        {
            GridControlResources.DataSource = AgendaViewDataGenerator.GenerateResourcesCollection(OwnerScheduler.Storage);
            GridControlResources.DataBind();
        }
         void InitializeGridControlAppointments()
        {
            GridControlAppointments.DataSource = AgendaViewDataGenerator.GenerateAgendaAppointmentCollection(OwnerScheduler.Storage, SelectedResourceId);
            GridControlAppointments.DataBind();
        }
        private void BindAppointmentsGrid()
        {
            AgendaViewDataGenerator.SelectedInterval = SelectedInterval;
            InitializeGridControlAppointments();
            GenerateAgndaViewCaption();
            GridControlAppointments.ExpandAll();
        }
        public void ReloadData()
        {
            BindAppointmentsGrid();
        }
        private void GenerateAgndaViewCaption()
        {
            string currentMonth = SelectedInterval.Start.ToString("MMMM-yyyy");
            int rowCount = GridControlAppointments.VisibleRowCount;
            if (rowCount == 0)
            {
                GridControlAppointments.Caption = currentMonth + " (no data)";
            }
            else if (ShowResources)
            {
                object resourceCaption = GridControlResources.GetCardValuesByKeyValue(SelectedResourceId, "AgendaResourceName");
                if (resourceCaption == null)
                    GridControlAppointments.Caption = currentMonth;
                else
                    GridControlAppointments.Caption = currentMonth + " (" + resourceCaption + ")";

            }
            else
                GridControlAppointments.Caption = currentMonth;
        }
        private void GoToNextMonth()
        {
            SelectedInterval = new TimeInterval(SelectedInterval.End, SelectedInterval.End.AddMonths(1));
            AgendaViewDataGenerator.SelectedInterval = SelectedInterval;
            InitializeGridControlAppointments();
            GenerateAgndaViewCaption();
            GridControlAppointments.ExpandAll();
        }
        private void GoToPreviousMonth()
        {
            SelectedInterval = new TimeInterval(SelectedInterval.Start.AddMonths(-1), SelectedInterval.Start);
            AgendaViewDataGenerator.SelectedInterval = SelectedInterval;
            InitializeGridControlAppointments();
            GenerateAgndaViewCaption();
            GridControlAppointments.ExpandAll();
        }
        private void GoToSpecificDate(DateTime date)
        {
            DateTime intervalStart = new DateTime(date.Year, date.Month, 1);
            TimeInterval interval = new TimeInterval(intervalStart, intervalStart.AddMonths(1));
            SelectedInterval = interval;
            AgendaViewDataGenerator.SelectedInterval = SelectedInterval;
            InitializeGridControlAppointments();
            GenerateAgndaViewCaption();
            GridControlAppointments.ExpandAll();
        }
#endregion
#region events
        protected void GridControlAppointments_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.Name != "gridColumnStatus") return;

            if (e.IsGetData)
            {
                object status = e.GetListSourceFieldValue("AgendaStatus");
                if (status != null)
                {
                    AppointmentStatus aptStatus = status as AppointmentStatus;
                    e.Value = aptStatus.Color;
                }
            }
        }
        protected void GridControlAppointments_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!GridControlAppointments.IsCallback) return;

            string[] parameters = e.Parameters.Split(';');
            if (parameters.Length != 2) return;

            string commandName = parameters[0];
            string value = parameters[1];
            switch (commandName)
            {
                case "SelectedResourceCommand":
                {
                    SelectedResourceId = Convert.ToInt32(value);
                    if (Convert.ToInt32(SelectedResourceId) == -1)
                        ShowResources = false;
                    else
                        ShowResources = true;
                    BindAppointmentsGrid();
                    break;
                }
                case "SelectedInterval":
                {
                    if (calendar.Value != null)
                    {
                        DateTime dt = Convert.ToDateTime(calendar.Value);
                        GoToSpecificDate(dt);
                    }
                    break;
                }
            }
           
        }
        protected void GridControlAppointments_FillContextMenuItems(object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e)
        {
            if (e.MenuType == DevExpress.Web.GridViewContextMenuType.Footer) return;

            e.Items.Clear();
            ContextMenuHelper.FillMenuItems(e.Items, e.MenuType, ShowResources);
        }
        protected void GridControlAppointments_ContextMenuItemClick(object sender, DevExpress.Web.ASPxGridViewContextMenuItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "GoToNextMonth":
                    GoToNextMonth();
                    break;
                case "GoToPreviousMonth":
                    GoToPreviousMonth();
                    break;
            }
        }
        protected void GridControlAppointments_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            object label = e.GetValue("AgendaLabel");
            if (label != null)
                e.Row.BackColor = (Color)label;
        }
#endregion
    }
}