using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace AgendaView
{
    #region #customappointment
    [Serializable]
    public class CustomEvent {
        private DateTime m_Start;
        private DateTime m_End;
        private string m_Subject;
        private int m_Status;
        private string m_Description;
        private int m_Label;
        private string m_Location;
        private bool m_Allday;
        private int m_EventType;
        private string m_RecurrenceInfo;
        private string m_ReminderInfo;
        private object m_OwnerId;


        public object Id { get; set; }
        public DateTime StartTime { get { return m_Start; } set { m_Start = value; } }
        public DateTime EndTime { get { return m_End; } set { m_End = value; } }
        public string Subject { get { return m_Subject; } set { m_Subject = value; } }
        public int Status { get { return m_Status; } set { m_Status = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public int Label { get { return m_Label; } set { m_Label = value; } }
        public string Location { get { return m_Location; } set { m_Location = value; } }
        public bool AllDay { get { return m_Allday; } set { m_Allday = value; } }
        public int EventType { get { return m_EventType; } set { m_EventType = value; } }
        public string RecurrenceInfo { get { return m_RecurrenceInfo; } set { m_RecurrenceInfo = value; } }
        public string ReminderInfo { get { return m_ReminderInfo; } set { m_ReminderInfo = value; } }
        public object OwnerId { get { return m_OwnerId; } set { m_OwnerId = value; } }

        public CustomEvent() {
        }
    }
    #endregion  #customappointment
    public class CustomEventList : BindingList<CustomEvent>
    {
        public void AddRange(CustomEventList events)
        {
            foreach (CustomEvent customEvent in events)
                this.Add(customEvent);
        }
        public int GetEventIndex(object eventId)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Id == eventId)
                    return i;
            return -1;
        }
    }
    public class CustomEventDataSource
    {
        CustomEventList events;
        public CustomEventDataSource(CustomEventList events)
        {
            if (events == null)
                DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events");
            this.events = events;
        }
        public CustomEventDataSource()
            : this(new CustomEventList())
        {
        }
        public CustomEventList Events { get { return events; } set { events = value; } }
        public int Count { get { return Events.Count; } }

        #region ObjectDataSource methods
        public object InsertMethodHandler(CustomEvent customEvent)
        {
            Object id = customEvent.GetHashCode();
            customEvent.Id = id;
            Events.Add(customEvent);
            return id;
        }
        public void DeleteMethodHandler(CustomEvent customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.Id);
            if (eventIndex >= 0)
                Events.RemoveAt(eventIndex);
        }
        public void UpdateMethodHandler(CustomEvent customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.Id);
            if (eventIndex >= 0)
            {
                Events.RemoveAt(eventIndex);
                Events.Insert(eventIndex, customEvent);
            }
        }
        public IEnumerable SelectMethodHandler()
        {
            CustomEventList result = new CustomEventList();
            result.AddRange(Events);
            return result;
        }
        #endregion

        public object ObtainLastInsertedId()
        {
            if (Count < 1)
                return null;
            return Events[Count - 1].Id;
        }
    }


    #region #customresource
     [Serializable]
    public class CustomResource {
        private string m_name;
        private int m_res_id;
        private string m_res_image;

        public string Name { get { return m_name; } set { m_name = value; } }
        public int Id { get { return m_res_id; } set { m_res_id = value; } }
        public string ResourceImageUrl { get { return m_res_image; } set { m_res_image = value; } }

        public CustomResource() {
        }
    }
    #endregion #customresource

     [Serializable]
     public class CustomResourceList : List<CustomResource>
     {
     }

     public class CustomResourceDataSource
     {
         CustomResourceList events;
         public CustomResourceDataSource(CustomResourceList events)
         {
             if (events == null)
                 DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("resources");
             this.events = events;
         }
         public CustomResourceDataSource()
             : this(new CustomResourceList())
         {
         }
         public CustomResourceList Resources { get { return events; } set { events = value; } }
         public int Count { get { return Resources.Count; } }

         public IEnumerable SelectMethodHandler()
         {
             CustomResourceList result = new CustomResourceList();
             result.AddRange(Resources);
             return result;
         }
     }

}
