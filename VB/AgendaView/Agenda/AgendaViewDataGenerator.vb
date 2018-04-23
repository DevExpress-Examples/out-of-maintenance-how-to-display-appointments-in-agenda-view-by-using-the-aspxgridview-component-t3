Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.XtraScheduler
Imports System.Drawing
Imports DevExpress.Web.ASPxScheduler

Namespace AgendaView
	Public NotInheritable Class AgendaViewDataGenerator
        Private Shared privateSelectedInterval As TimeInterval
		Public Shared Property SelectedInterval() As TimeInterval
			Get
				Return privateSelectedInterval
			End Get
			Set(ByVal value As TimeInterval)
				privateSelectedInterval = value
			End Set
		End Property

		Private Sub New()
		End Sub
		Public Shared Function GenerateResourcesCollection(ByVal storage As ASPxSchedulerStorage) As Object
			Dim collection As New AgendaResourceCollection()
			For i As Integer = 0 To storage.Resources.Count - 1
				Dim currentResource As Resource = storage.Resources(i)
				collection.Add(New AgendaResource() With {.Id = currentResource.Id, .AgendaResourceName = currentResource.Caption, .AgendaResourceImageUrl = If(currentResource.CustomFields(0) Is Nothing, String.Empty, currentResource.CustomFields(0).ToString())})
			Next i

			Return collection
		End Function

		Public Shared Function GenerateAgendaAppointmentCollection(ByVal storage As ASPxSchedulerStorage, ByVal resourceId As Object) As Object
			Dim sourceAppointments As List(Of Appointment) = Nothing
			If Convert.ToInt32(resourceId) = -1 Then
                sourceAppointments = storage.GetAppointments(SelectedInterval).ToList()
            Else
                sourceAppointments = storage.GetAppointments(SelectedInterval).Where((Function(apt) apt.ResourceId Is resourceId OrElse apt.ResourceIds.Contains(resourceId))).ToList()
            End If
			Dim agendaAppointments As New AgendaAppointmentCollection()
			For Each appointment As Appointment In sourceAppointments
				Dim currentDayInterval As New TimeInterval(appointment.Start.Date, appointment.Start.Date.AddDays(1))
				Dim startTime As String = ""
				Dim endTime As String = ""

				If currentDayInterval.Contains(appointment.End) Then
					startTime = If(currentDayInterval.Start = appointment.Start, "", appointment.Start.TimeOfDay.ToString("hh\:mm"))
					endTime = If(currentDayInterval.End = appointment.End, "", appointment.End.TimeOfDay.ToString("hh\:mm"))
					agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, endTime))
				Else
					startTime = If(currentDayInterval.Start = appointment.Start, "", appointment.Start.TimeOfDay.ToString("hh\:mm"))
					agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, ""))
					Do
						currentDayInterval = New TimeInterval(currentDayInterval.End, currentDayInterval.End.AddDays(1))
						If currentDayInterval.Contains(appointment.End) Then
							endTime = If(currentDayInterval.End = appointment.End, "", appointment.End.TimeOfDay.ToString("hh\:mm"))
							agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", endTime))
							Exit Do
						Else
							agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", ""))
						End If
					Loop

				End If
			Next appointment
			Return agendaAppointments
		End Function

		Private Shared Function CreateAgendaAppointment(ByVal storage As ASPxSchedulerStorage, ByVal sourceAppointment As Appointment, ByVal startDate As DateTime, ByVal startTime As String, ByVal endTime As String) As AgendaAppointment
			Dim agendaAppointment As New AgendaAppointment()
			agendaAppointment.Id = sourceAppointment.Id
			agendaAppointment.AgendaDate = startDate
			agendaAppointment.AgendaDescription = sourceAppointment.Description
			agendaAppointment.AgendaSubject = sourceAppointment.Subject
			If startTime = "" AndAlso endTime = "" Then
				agendaAppointment.AgendaDuration = "All Day"
			ElseIf startTime = "" AndAlso endTime <> "" Then
				agendaAppointment.AgendaDuration = "Till: " & endTime
			ElseIf startTime <> "" AndAlso endTime = "" Then
				agendaAppointment.AgendaDuration = "From: " & startTime
			Else
				agendaAppointment.AgendaDuration = String.Format("{0} - {1}", startTime, endTime)
			End If
			agendaAppointment.ResourceId = sourceAppointment.ResourceId
			agendaAppointment.AgendaLocation = sourceAppointment.Location
			agendaAppointment.AgendaStatus = storage.Appointments.Statuses(sourceAppointment.StatusId)

			agendaAppointment.AgendaLabel = storage.GetLabelColor(sourceAppointment.LabelId)
			agendaAppointment.SourceAppointment = sourceAppointment
			Return agendaAppointment
		End Function
	End Class

	<Serializable> _
	Public Class AgendaAppointment
		Private privateId As Object
		Public Property Id() As Object
			Get
				Return privateId
			End Get
			Set(ByVal value As Object)
				privateId = value
			End Set
		End Property
		Private privateAgendaStatus As AppointmentStatus
		Public Property AgendaStatus() As AppointmentStatus
			Get
				Return privateAgendaStatus
			End Get
			Set(ByVal value As AppointmentStatus)
				privateAgendaStatus = value
			End Set
		End Property
		Private privateAgendaSubject As String
		Public Property AgendaSubject() As String
			Get
				Return privateAgendaSubject
			End Get
			Set(ByVal value As String)
				privateAgendaSubject = value
			End Set
		End Property
		Private privateAgendaDescription As String
		Public Property AgendaDescription() As String
			Get
				Return privateAgendaDescription
			End Get
			Set(ByVal value As String)
				privateAgendaDescription = value
			End Set
		End Property
		Private privateAgendaDuration As String
		Public Property AgendaDuration() As String
			Get
				Return privateAgendaDuration
			End Get
			Set(ByVal value As String)
				privateAgendaDuration = value
			End Set
		End Property
		Private privateAgendaLocation As String
		Public Property AgendaLocation() As String
			Get
				Return privateAgendaLocation
			End Get
			Set(ByVal value As String)
				privateAgendaLocation = value
			End Set
		End Property
		Private privateAgendaDate As DateTime
		Public Property AgendaDate() As DateTime
			Get
				Return privateAgendaDate
			End Get
			Set(ByVal value As DateTime)
				privateAgendaDate = value
			End Set
		End Property
		Private privateAgendaLabel As Color
		Public Property AgendaLabel() As Color
			Get
				Return privateAgendaLabel
			End Get
			Set(ByVal value As Color)
				privateAgendaLabel = value
			End Set
		End Property
		Private privateSourceAppointment As Appointment
		Public Property SourceAppointment() As Appointment
			Get
				Return privateSourceAppointment
			End Get
			Set(ByVal value As Appointment)
				privateSourceAppointment = value
			End Set
		End Property
		Private privateResourceId As Object
		Public Property ResourceId() As Object
			Get
				Return privateResourceId
			End Get
			Set(ByVal value As Object)
				privateResourceId = value
			End Set
		End Property
	End Class
	 <Serializable> _
	 Public Class AgendaAppointmentCollection
		 Inherits List(Of AgendaAppointment)
	 End Class

	<Serializable> _
	Public Class AgendaResource
		Private privateId As Object
		Public Property Id() As Object
			Get
				Return privateId
			End Get
			Set(ByVal value As Object)
				privateId = value
			End Set
		End Property
		Private privateAgendaResourceName As String
		Public Property AgendaResourceName() As String
			Get
				Return privateAgendaResourceName
			End Get
			Set(ByVal value As String)
				privateAgendaResourceName = value
			End Set
		End Property
		Private privateAgendaResourceImageUrl As String
		Public Property AgendaResourceImageUrl() As String
			Get
				Return privateAgendaResourceImageUrl
			End Get
			Set(ByVal value As String)
				privateAgendaResourceImageUrl = value
			End Set
		End Property

	End Class
	  <Serializable> _
	  Public Class AgendaResourceCollection
		  Inherits List(Of AgendaResource)
	  End Class
End Namespace
