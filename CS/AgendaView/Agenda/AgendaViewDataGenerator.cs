using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraScheduler;
using System.Drawing;
using DevExpress.Web.ASPxScheduler;

namespace AgendaView
{
    public static class AgendaViewDataGenerator {
        public static TimeInterval SelectedInterval { get; set; }

        public static object GenerateResourcesCollection(ASPxSchedulerStorage storage) {
            AgendaResourceCollection collection = new AgendaResourceCollection();
            for (int i = 0; i < storage.Resources.Count; i++)
            {
                Resource currentResource = storage.Resources[i];
                collection.Add(new AgendaResource()
                {
                    Id = currentResource.Id,
                    AgendaResourceName = currentResource.Caption,
                    AgendaResourceImageUrl = currentResource.CustomFields[0] == null ? string.Empty : currentResource.CustomFields[0].ToString()
                });
            }

            return collection;
        }

        public static object GenerateAgendaAppointmentCollection(ASPxSchedulerStorage storage, object resourceId)
        {
            List<Appointment> sourceAppointments = null;
            if (Convert.ToInt32(resourceId) == -1)
                sourceAppointments = storage.GetAppointments(SelectedInterval).ToList<Appointment>();
            else
                sourceAppointments = storage.GetAppointments(SelectedInterval).Where<Appointment>(apt => apt.ResourceId == resourceId || apt.ResourceIds.Contains(resourceId)).ToList<Appointment>();
            AgendaAppointmentCollection agendaAppointments = new AgendaAppointmentCollection();
            foreach(Appointment appointment in sourceAppointments) {
                TimeInterval currentDayInterval = new TimeInterval(appointment.Start.Date, appointment.Start.Date.AddDays(1));
                string startTime = "";
                string endTime = "";
               
                if(currentDayInterval.Contains(appointment.End)) {
                    startTime = currentDayInterval.Start == appointment.Start ? "" : appointment.Start.TimeOfDay.ToString(@"hh\:mm");
                    endTime = currentDayInterval.End == appointment.End ? "" : appointment.End.TimeOfDay.ToString(@"hh\:mm");
                    agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, endTime));
                }
                else {
                    startTime = currentDayInterval.Start == appointment.Start ? "" : appointment.Start.TimeOfDay.ToString(@"hh\:mm");
                    agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, ""));
                    while(true) {
                        currentDayInterval = new TimeInterval(currentDayInterval.End, currentDayInterval.End.AddDays(1));
                        if(currentDayInterval.Contains(appointment.End)) {
                            endTime = currentDayInterval.End == appointment.End ? "" : appointment.End.TimeOfDay.ToString(@"hh\:mm");
                            agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", endTime));
                            break;
                        }
                        else {
                            agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", ""));
                        }
                    }

                }
            }
            return agendaAppointments;
        }

        static AgendaAppointment CreateAgendaAppointment(ASPxSchedulerStorage storage, Appointment sourceAppointment, DateTime startDate, string startTime, string endTime)
        {
            AgendaAppointment agendaAppointment = new AgendaAppointment();
            agendaAppointment.Id = sourceAppointment.Id;
            agendaAppointment.AgendaDate = startDate;
            agendaAppointment.AgendaDescription = sourceAppointment.Description;
            agendaAppointment.AgendaSubject = sourceAppointment.Subject;
            if(startTime == "" && endTime == "") {
                agendaAppointment.AgendaDuration = "All Day";
            }
            else if(startTime == "" && endTime != "") {
                agendaAppointment.AgendaDuration = "Till: " + endTime;
            }
            else if(startTime != "" && endTime == "") {
                agendaAppointment.AgendaDuration = "From: " + startTime;
            }
            else {
                agendaAppointment.AgendaDuration = String.Format("{0} - {1}", startTime, endTime);
            }
            agendaAppointment.ResourceId = sourceAppointment.ResourceId;
            agendaAppointment.AgendaLocation = sourceAppointment.Location;
            agendaAppointment.AgendaStatus = storage.Appointments.Statuses[sourceAppointment.StatusId]; ;
            agendaAppointment.AgendaLabel = storage.GetLabelColor(sourceAppointment.LabelId);
            agendaAppointment.SourceAppointment = sourceAppointment;
            return agendaAppointment;
        }
    }

    [Serializable]
    public class AgendaAppointment {
        public object Id { get;set;}
        public AppointmentStatus AgendaStatus { get; set; }
        public string AgendaSubject { get; set; }
        public string AgendaDescription { get; set; }
        public string AgendaDuration { get; set; }
        public string AgendaLocation { get; set; }
        public DateTime AgendaDate { get; set; }
        public Color AgendaLabel { get; set; }
        public Appointment SourceAppointment { get; set; }
        public object ResourceId { get; set; }
    }
     [Serializable]
    public class AgendaAppointmentCollection : List<AgendaAppointment> { }

    [Serializable]
    public class AgendaResource
    {
        public object Id { get; set; }
        public string AgendaResourceName { get; set; }
        public string AgendaResourceImageUrl { get; set; }

    }
      [Serializable]
    public class AgendaResourceCollection : List<AgendaResource> { }
}
