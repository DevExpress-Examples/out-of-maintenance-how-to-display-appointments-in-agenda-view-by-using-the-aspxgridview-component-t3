Imports Microsoft.VisualBasic
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace AgendaView
	Public Class DataHelper
		Private control As ASPxScheduler
		Private Shared RandomInstance As New Random()
		Private CustomResourceCollection As New CustomResourceList()
		Private CustomEventList As New CustomEventList()
		Public Sub New(ByVal scheduler As ASPxScheduler)
			control = scheduler
			InitResources()
			GenerateEvents()
		End Sub
		Public ReadOnly Property Resources() As CustomResourceList
			Get
				Return CustomResourceCollection
			End Get
		End Property
		Public ReadOnly Property Appointments() As CustomEventList
			Get
				Return CustomEventList
			End Get
		End Property


		Private Sub GenerateEvents()
			Dim count As Integer = CustomResourceCollection.Count

			For i As Integer = 0 To count - 1
				Dim resource As CustomResource = CustomResourceCollection(i)
				Dim subjPrefix As String = resource.Name & "'s "
				CustomEventList.Add(CreateEvent(subjPrefix & "meeting", "meeting", resource.Id, 2, 5, 0, "office"))
				CustomEventList.Add(CreateEvent(subjPrefix & "travel", "travel", resource.Id, 3, 6, 0, "out of the city"))
				CustomEventList.Add(CreateEvent(subjPrefix & "phone call", "phone call", resource.Id, 0, 10, 0, "office"))
				CustomEventList.Add(CreateEvent(subjPrefix & "business trip", "business trip", resource.Id, 3, 6, 3, "San-Francisco"))
				CustomEventList.Add(CreateEvent(subjPrefix & "double personal day", "double personal day", resource.Id, 0, 10, 2, "out of the city"))
				CustomEventList.Add(CreateEvent(subjPrefix & "personal day", "personal day", resource.Id, 0, 10, 1, "out of the city"))
			Next i
		End Sub
		Private Function CreateEvent(ByVal description As String, ByVal subject As String, ByVal resourceId As Object, ByVal status As Integer, ByVal label As Integer, ByVal days As Integer, ByVal location As String) As CustomEvent
			Dim apt As New CustomEvent()
			apt.Id = CustomEventList.Count
			apt.Subject = subject
			apt.Description = description
			apt.OwnerId = resourceId
			Dim rnd As Random = RandomInstance
			Dim rangeInMinutes As Integer = 60 * 24
			If days = 2 Then
				apt.StartTime = DateTime.Today
				apt.EndTime = DateTime.Today.AddDays(2)
			ElseIf days = 1 Then
				apt.StartTime = DateTime.Today
				apt.EndTime = DateTime.Today.AddDays(1)
			Else
				apt.StartTime = DateTime.Today + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes))
				apt.EndTime = apt.StartTime.AddDays(days) + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes \ 4))
			End If
			apt.Location = location
			apt.Status = status
			apt.Label = label
			apt.EventType = 0
			Return apt
		End Function
		Private Sub InitResources()
			CustomResourceCollection.Add(CreateCustomResource(1, "Max Fowler", Color.PowderBlue, "~/Images/MaxFowlerPhoto.jpg"))
			CustomResourceCollection.Add(CreateCustomResource(2, "Nancy Drewmore", Color.PaleVioletRed, "~/Images/NancyDrewmorePhoto.jpg"))
			CustomResourceCollection.Add(CreateCustomResource(3, "Pak Jang", Color.PeachPuff, Nothing))
		End Sub
		Private Function CreateCustomResource(ByVal res_id As Integer, ByVal caption As String, ByVal ResColor As Color, ByVal imageUrl As String) As CustomResource
			Dim cr As New CustomResource()
			cr.Id = res_id
			cr.Name = caption
			cr.ResourceImageUrl = imageUrl

			Return cr
		End Function
	End Class
End Namespace
