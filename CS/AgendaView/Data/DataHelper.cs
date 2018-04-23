using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AgendaView
{
    public class DataHelper
    {
        ASPxScheduler control;
        private static Random RandomInstance = new Random();
        private CustomResourceList CustomResourceCollection = new CustomResourceList();
        private CustomEventList CustomEventList = new CustomEventList();
        public DataHelper(ASPxScheduler scheduler)
        {
            control = scheduler;
            InitResources();
            GenerateEvents();
        }
        public CustomResourceList Resources { get { return CustomResourceCollection; } }
        public CustomEventList Appointments { get { return CustomEventList; } }

       
        private void GenerateEvents()
        {
            int count = CustomResourceCollection.Count;

            for (int i = 0; i < count; i++)
            {
                CustomResource resource = CustomResourceCollection[i];
                string subjPrefix = resource.Name + "'s ";
                CustomEventList.Add(CreateEvent(subjPrefix + "meeting", "meeting", resource.Id, 2, 5, 0, "office"));
                CustomEventList.Add(CreateEvent(subjPrefix + "travel", "travel", resource.Id, 3, 6, 0, "out of the city"));
                CustomEventList.Add(CreateEvent(subjPrefix + "phone call", "phone call", resource.Id, 0, 10, 0, "office"));
                CustomEventList.Add(CreateEvent(subjPrefix + "business trip", "business trip", resource.Id, 3, 6, 3, "San-Francisco"));
                CustomEventList.Add(CreateEvent(subjPrefix + "double personal day", "double personal day", resource.Id, 0, 10, 2, "out of the city"));
                CustomEventList.Add(CreateEvent(subjPrefix + "personal day", "personal day", resource.Id, 0, 10, 1, "out of the city"));
            }
        }
        private CustomEvent CreateEvent(string description, string subject, object resourceId, int status, int label, int days, string location)
        {
            CustomEvent apt = new CustomEvent();
            apt.Id = CustomEventList.Count;
            apt.Subject = subject;
            apt.Description = description;
            apt.OwnerId = resourceId;
            Random rnd = RandomInstance;
            int rangeInMinutes = 60 * 24;
            if (days == 2)
            {
                apt.StartTime = DateTime.Today;
                apt.EndTime = DateTime.Today.AddDays(2);
            }
            else if (days == 1)
            {
                apt.StartTime = DateTime.Today;
                apt.EndTime = DateTime.Today.AddDays(1);
            }
            else
            {
                apt.StartTime = DateTime.Today + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes));
                apt.EndTime = apt.StartTime.AddDays(days) + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes / 4));
            }
            apt.Location = location;
            apt.Status = status;
            apt.Label = label;
            apt.EventType = 0;
            return apt;
        }
        private void InitResources()
        {
            CustomResourceCollection.Add(CreateCustomResource(1, "Max Fowler", Color.PowderBlue,  "~/Images/MaxFowlerPhoto.jpg"));
            CustomResourceCollection.Add(CreateCustomResource(2, "Nancy Drewmore", Color.PaleVioletRed, "~/Images/NancyDrewmorePhoto.jpg"));
            CustomResourceCollection.Add(CreateCustomResource(3, "Pak Jang", Color.PeachPuff, null));
        }
        private CustomResource CreateCustomResource(int res_id, string caption, Color ResColor, string imageUrl)
        {
            CustomResource cr = new CustomResource();
            cr.Id = res_id;
            cr.Name = caption;
            cr.ResourceImageUrl = imageUrl;

            return cr;
        }
    }
}
