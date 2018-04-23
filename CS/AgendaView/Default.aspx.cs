using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AgendaView
{
    public partial class Default : System.Web.UI.Page
    {
#region PageEvents
        protected void Page_Init(object sender, EventArgs e)
        {
            AgendaViewControl1.OwnerScheduler = ASPxScheduler1;
            if (!IsPostBack)
            {
                DataHelper helper = new DataHelper(ASPxScheduler1);
                Session["ResourcesStorage"] = helper.Resources;
                Session["AppointmentStorage"] = helper.Appointments;
            }
            ASPxScheduler1.JSProperties["cpAgendaViewActive"] = AgendaViewControl1.Active;
            ASPxScheduler1.JSProperties["cpAppointmentSaved"] = false;
          
            InitResourcesMappings();
            InitAppointmentsMappings();
            ASPxScheduler1.DataBind();
        }
#endregion  
#region methods   
        private CustomResourceList GetResources()
        {
            return Session["ResourcesStorage"] as CustomResourceList;
        }
        private CustomEventList GetAppointments()
        {
            return Session["AppointmentStorage"] as CustomEventList;
        }
        void InitResourcesMappings()
        {
            ResourceMappingInfo mappings = ASPxScheduler1.Storage.Resources.Mappings;
            mappings.Id = "Id";
            mappings.Caption = "Name";

            ASPxResourceCustomFieldMappingCollection customFieldMappings = ASPxScheduler1.Storage.Resources.CustomFieldMappings;
            customFieldMappings.Add(new ResourceCustomFieldMapping("ResourceImageUrl", "ResourceImageUrl"));
        }
         void InitAppointmentsMappings()
        {
            AppointmentMappingInfo mappings = ASPxScheduler1.Storage.Appointments.Mappings;
            mappings.AppointmentId = "Id";
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";
        }
#endregion
#region events
         protected void resourcesDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
         {
             e.ObjectInstance = new CustomResourceDataSource(GetResources());
         }
         protected void appointmentsDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
         {
            objectInstance = new CustomEventDataSource(GetAppointments());
            e.ObjectInstance = objectInstance;
         }
         CustomEventDataSource objectInstance;
         protected void ASPxScheduler1_AppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e)
         {
             e.KeyFieldValue = this.objectInstance.ObtainLastInsertedId();
         }
         protected void ASPxScheduler1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
         {
             ASPxSchedulerPopupMenu menu = e.Menu;
             if (menu.Id.Equals(SchedulerMenuItemId.DefaultMenu))
             {
                 DevExpress.Web.MenuItem defaultSubMenu = menu.Items.FindByName("SwitchViewMenu");
                 DevExpress.Web.MenuItem subMenuItem = new DevExpress.Web.MenuItem("Agenda View", "AgendaView");
                 defaultSubMenu.Items.Add(subMenuItem);
                 menu.ClientSideEvents.ItemClick = "DefaultViewMenuHandler";
             }
         }
         protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
         {
             if (!ASPxCallbackPanel1.IsCallback) return;

             string[] parameters = e.Parameter.Split(';');
             if (parameters.Length < 1) return;

             string commandName = parameters[0];
             if (commandName == "DeleteAppointmentCommand")
             {
                 int value = Convert.ToInt32(parameters[1]);
                 ASPxScheduler1.Storage.Appointments.Remove(ASPxScheduler1.Storage.Appointments.GetAppointmentById(value));
                 ASPxScheduler1.DataBind();
                 AgendaViewControl1.ReloadData();
             }
             else if (commandName == "SwitchViewToAgendaView")
             {
                    DateTime selectedIntervalStart = ASPxScheduler1.ActiveView.GetVisibleIntervals().Start;
                    DateTime intervalStart = new DateTime(selectedIntervalStart.Year, selectedIntervalStart.Month, 1);
                    AgendaViewControl1.SelectedInterval = new TimeInterval(intervalStart, intervalStart.AddMonths(1));
             }

         }
         protected void ASPxScheduler1_AfterExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e)
         {
             if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
             {
                 if (AgendaViewControl1.Active)
                 {
                     ASPxScheduler1.JSProperties["cpAppointmentSaved"] = true;
                     ASPxScheduler1.DataBind();
                     AgendaViewControl1.ReloadData();
                 }
             }
         }
#endregion

       
    }
}