Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Drawing
Imports System.ComponentModel
Imports System.Collections

Namespace AgendaView
	#Region "#customappointment"
	<Serializable> _
	Public Class CustomEvent
		Private m_Start As DateTime
		Private m_End As DateTime
		Private m_Subject As String
		Private m_Status As Integer
		Private m_Description As String
		Private m_Label As Integer
		Private m_Location As String
		Private m_Allday As Boolean
		Private m_EventType As Integer
		Private m_RecurrenceInfo As String
		Private m_ReminderInfo As String
		Private m_OwnerId As Object


		Private privateId As Object
		Public Property Id() As Object
			Get
				Return privateId
			End Get
			Set(ByVal value As Object)
				privateId = value
			End Set
		End Property
		Public Property StartTime() As DateTime
			Get
				Return m_Start
			End Get
			Set(ByVal value As DateTime)
				m_Start = value
			End Set
		End Property
		Public Property EndTime() As DateTime
			Get
				Return m_End
			End Get
			Set(ByVal value As DateTime)
				m_End = value
			End Set
		End Property
		Public Property Subject() As String
			Get
				Return m_Subject
			End Get
			Set(ByVal value As String)
				m_Subject = value
			End Set
		End Property
		Public Property Status() As Integer
			Get
				Return m_Status
			End Get
			Set(ByVal value As Integer)
				m_Status = value
			End Set
		End Property
		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set(ByVal value As String)
				m_Description = value
			End Set
		End Property
		Public Property Label() As Integer
			Get
				Return m_Label
			End Get
			Set(ByVal value As Integer)
				m_Label = value
			End Set
		End Property
		Public Property Location() As String
			Get
				Return m_Location
			End Get
			Set(ByVal value As String)
				m_Location = value
			End Set
		End Property
		Public Property AllDay() As Boolean
			Get
				Return m_Allday
			End Get
			Set(ByVal value As Boolean)
				m_Allday = value
			End Set
		End Property
		Public Property EventType() As Integer
			Get
				Return m_EventType
			End Get
			Set(ByVal value As Integer)
				m_EventType = value
			End Set
		End Property
		Public Property RecurrenceInfo() As String
			Get
				Return m_RecurrenceInfo
			End Get
			Set(ByVal value As String)
				m_RecurrenceInfo = value
			End Set
		End Property
		Public Property ReminderInfo() As String
			Get
				Return m_ReminderInfo
			End Get
			Set(ByVal value As String)
				m_ReminderInfo = value
			End Set
		End Property
		Public Property OwnerId() As Object
			Get
				Return m_OwnerId
			End Get
			Set(ByVal value As Object)
				m_OwnerId = value
			End Set
		End Property

		Public Sub New()
		End Sub
	End Class
	#End Region '  #customappointment
	Public Class CustomEventList
		Inherits BindingList(Of CustomEvent)
		Public Sub AddRange(ByVal events As CustomEventList)
			For Each customEvent As CustomEvent In events
				Me.Add(customEvent)
			Next customEvent
		End Sub
		Public Function GetEventIndex(ByVal eventId As Object) As Integer
			For i As Integer = 0 To Count - 1
				If Me(i).Id = eventId Then
					Return i
				End If
			Next i
			Return -1
		End Function
	End Class
	Public Class CustomEventDataSource
		Private events_Renamed As CustomEventList
		Public Sub New(ByVal events As CustomEventList)
			If events Is Nothing Then
				DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events")
			End If
			Me.events_Renamed = events
		End Sub
		Public Sub New()
			Me.New(New CustomEventList())
		End Sub
		Public Property Events() As CustomEventList
			Get
				Return events_Renamed
			End Get
			Set(ByVal value As CustomEventList)
				events_Renamed = value
			End Set
		End Property
		Public ReadOnly Property Count() As Integer
			Get
				Return Events.Count
			End Get
		End Property

		#Region "ObjectDataSource methods"
		Public Function InsertMethodHandler(ByVal customEvent As CustomEvent) As Object
			Dim id As Object = customEvent.GetHashCode()
			customEvent.Id = id
			Events.Add(customEvent)
			Return id
		End Function
		Public Sub DeleteMethodHandler(ByVal customEvent As CustomEvent)
			Dim eventIndex As Integer = Events.GetEventIndex(customEvent.Id)
			If eventIndex >= 0 Then
				Events.RemoveAt(eventIndex)
			End If
		End Sub
		Public Sub UpdateMethodHandler(ByVal customEvent As CustomEvent)
			Dim eventIndex As Integer = Events.GetEventIndex(customEvent.Id)
			If eventIndex >= 0 Then
				Events.RemoveAt(eventIndex)
				Events.Insert(eventIndex, customEvent)
			End If
		End Sub
		Public Function SelectMethodHandler() As IEnumerable
			Dim result As New CustomEventList()
			result.AddRange(Events)
			Return result
		End Function
		#End Region

		Public Function ObtainLastInsertedId() As Object
			If Count < 1 Then
				Return Nothing
			End If
			Return Events(Count - 1).Id
		End Function
	End Class


	#Region "#customresource"
	 <Serializable> _
	 Public Class CustomResource
		Private m_name As String
		Private m_res_id As Integer
		Private m_res_image As String

		Public Property Name() As String
			Get
				Return m_name
			End Get
			Set(ByVal value As String)
				m_name = value
			End Set
		End Property
		Public Property Id() As Integer
			Get
				Return m_res_id
			End Get
			Set(ByVal value As Integer)
				m_res_id = value
			End Set
		End Property
		Public Property ResourceImageUrl() As String
			Get
				Return m_res_image
			End Get
			Set(ByVal value As String)
				m_res_image = value
			End Set
		End Property

		Public Sub New()
		End Sub
	 End Class
	#End Region ' #customresource

	 <Serializable> _
	 Public Class CustomResourceList
		 Inherits List(Of CustomResource)
	 End Class

	 Public Class CustomResourceDataSource
		 Private events As CustomResourceList
		 Public Sub New(ByVal events As CustomResourceList)
			 If events Is Nothing Then
				 DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("resources")
			 End If
			 Me.events = events
		 End Sub
		 Public Sub New()
			 Me.New(New CustomResourceList())
		 End Sub
		 Public Property Resources() As CustomResourceList
			 Get
				 Return events
			 End Get
			 Set(ByVal value As CustomResourceList)
				 events = value
			 End Set
		 End Property
		 Public ReadOnly Property Count() As Integer
			 Get
				 Return Resources.Count
			 End Get
		 End Property

		 Public Function SelectMethodHandler() As IEnumerable
			 Dim result As New CustomResourceList()
			 result.AddRange(Resources)
			 Return result
		 End Function
	 End Class

End Namespace
