Imports Microsoft.VisualBasic
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace AgendaView
	Partial Public Class AgendaViewControl
		Inherits System.Web.UI.UserControl
#Region "PageEvents"
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
				InitializeGridControlResources()
				BindAppointmentsGrid()
		End Sub
#End Region
#Region "Properties"
		Private privateOwnerScheduler As ASPxScheduler
		Public Property OwnerScheduler() As ASPxScheduler
			Get
				Return privateOwnerScheduler
			End Get
			Set(ByVal value As ASPxScheduler)
				privateOwnerScheduler = value
			End Set
		End Property
		Public Property ShowResources() As Boolean
			Get
				If Session("ShowResources") Is Nothing Then
					Session("ShowResources") = False
				End If

				Return CBool(Session("ShowResources"))
			End Get
			Set(ByVal value As Boolean)
				Session("ShowResources") = value
			End Set
		End Property
		Public Property SelectedResourceId() As Object
			Get
				If Session("selectedResourceId") Is Nothing Then
					Session("selectedResourceId") = GridControlResources.GetCardValues(GridControlResources.FocusedCardIndex, "Id")
				End If

				Return Session("selectedResourceId")
			End Get
			Set(ByVal value As Object)
				Session("selectedResourceId") = value
			End Set
		End Property
		Public Property SelectedInterval() As TimeInterval
			Get
				If Session("selectedInterval") Is Nothing Then
					Dim selectedIntervalStart As DateTime = OwnerScheduler.ActiveView.GetVisibleIntervals().Start
					Dim intervalStart As New DateTime(selectedIntervalStart.Year, selectedIntervalStart.Month, 1)
					Dim interval As New TimeInterval(intervalStart, intervalStart.AddMonths(1))
					Session("selectedInterval") = interval
				End If

				Dim ti As TimeInterval = TryCast(Session("selectedInterval"), TimeInterval)
				Return ti
			End Get
			Set(ByVal value As TimeInterval)
				Session("selectedInterval") = value
			End Set
		End Property
		Public ReadOnly Property Active() As Boolean
			Get
				Return Me.Visible
			End Get
		End Property
#End Region
#Region "methods"
		Private Sub InitializeGridControlResources()
			GridControlResources.DataSource = AgendaViewDataGenerator.GenerateResourcesCollection(OwnerScheduler.Storage)
			GridControlResources.DataBind()
		End Sub
		 Private Sub InitializeGridControlAppointments()
			GridControlAppointments.DataSource = AgendaViewDataGenerator.GenerateAgendaAppointmentCollection(OwnerScheduler.Storage, SelectedResourceId)
			GridControlAppointments.DataBind()
		 End Sub
		Private Sub BindAppointmentsGrid()
			AgendaViewDataGenerator.SelectedInterval = SelectedInterval
			InitializeGridControlAppointments()
			GenerateAgndaViewCaption()
			GridControlAppointments.ExpandAll()
		End Sub
		Public Sub ReloadData()
			BindAppointmentsGrid()
		End Sub
		Private Sub GenerateAgndaViewCaption()
			Dim currentMonth As String = SelectedInterval.Start.ToString("MMMM-yyyy")
			Dim rowCount As Integer = GridControlAppointments.VisibleRowCount
			If rowCount = 0 Then
				GridControlAppointments.Caption = currentMonth & " (no data)"
			ElseIf ShowResources Then
				Dim resourceCaption As Object = GridControlResources.GetCardValuesByKeyValue(SelectedResourceId, "AgendaResourceName")
				If resourceCaption Is Nothing Then
					GridControlAppointments.Caption = currentMonth
				Else
					GridControlAppointments.Caption = currentMonth & " (" & resourceCaption & ")"
				End If

			Else
				GridControlAppointments.Caption = currentMonth
			End If
		End Sub
		Private Sub GoToNextMonth()
			SelectedInterval = New TimeInterval(SelectedInterval.End, SelectedInterval.End.AddMonths(1))
			AgendaViewDataGenerator.SelectedInterval = SelectedInterval
			InitializeGridControlAppointments()
			GenerateAgndaViewCaption()
			GridControlAppointments.ExpandAll()
		End Sub
		Private Sub GoToPreviousMonth()
			SelectedInterval = New TimeInterval(SelectedInterval.Start.AddMonths(-1), SelectedInterval.Start)
			AgendaViewDataGenerator.SelectedInterval = SelectedInterval
			InitializeGridControlAppointments()
			GenerateAgndaViewCaption()
			GridControlAppointments.ExpandAll()
		End Sub
		Private Sub GoToSpecificDate(ByVal [date] As DateTime)
			Dim intervalStart As New DateTime([date].Year, [date].Month, 1)
			Dim interval As New TimeInterval(intervalStart, intervalStart.AddMonths(1))
			SelectedInterval = interval
			AgendaViewDataGenerator.SelectedInterval = SelectedInterval
			InitializeGridControlAppointments()
			GenerateAgndaViewCaption()
			GridControlAppointments.ExpandAll()
		End Sub
#End Region
#Region "events"
		Protected Sub GridControlAppointments_CustomUnboundColumnData(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewColumnDataEventArgs)
			If e.Column.Name <> "gridColumnStatus" Then
				Return
			End If

			If e.IsGetData Then
				Dim status As Object = e.GetListSourceFieldValue("AgendaStatus")
				If status IsNot Nothing Then
					Dim aptStatus As AppointmentStatus = TryCast(status, AppointmentStatus)
					e.Value = aptStatus.Color
				End If
			End If
		End Sub
		Protected Sub GridControlAppointments_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
			If (Not GridControlAppointments.IsCallback) Then
				Return
			End If

			Dim parameters() As String = e.Parameters.Split(";"c)
			If parameters.Length <> 2 Then
				Return
			End If

			Dim commandName As String = parameters(0)
			Dim value As String = parameters(1)
			Select Case commandName
				Case "SelectedResourceCommand"
					SelectedResourceId = Convert.ToInt32(value)
					If Convert.ToInt32(SelectedResourceId) = -1 Then
						ShowResources = False
					Else
						ShowResources = True
					End If
					BindAppointmentsGrid()
					Exit Select
				Case "SelectedInterval"
					If calendar.Value IsNot Nothing Then
						Dim dt As DateTime = Convert.ToDateTime(calendar.Value)
						GoToSpecificDate(dt)
					End If
					Exit Select
			End Select

		End Sub
		Protected Sub GridControlAppointments_FillContextMenuItems(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewContextMenuEventArgs)
			If e.MenuType = DevExpress.Web.GridViewContextMenuType.Footer Then
				Return
			End If

			e.Items.Clear()
			ContextMenuHelper.FillMenuItems(e.Items, e.MenuType, ShowResources)
		End Sub
		Protected Sub GridControlAppointments_ContextMenuItemClick(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewContextMenuItemClickEventArgs)
			Select Case e.Item.Name
				Case "GoToNextMonth"
					GoToNextMonth()
				Case "GoToPreviousMonth"
					GoToPreviousMonth()
			End Select
		End Sub
		Protected Sub GridControlAppointments_HtmlRowPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableRowEventArgs)
			If e.RowType <> GridViewRowType.Data Then
				Return
			End If

			Dim label As Object = e.GetValue("AgendaLabel")
			If label IsNot Nothing Then
				e.Row.BackColor = CType(label, Color)
			End If
		End Sub
#End Region
	End Class
End Namespace