Imports Microsoft.VisualBasic
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace AgendaView
	Partial Public Class [Default]
		Inherits System.Web.UI.Page
#Region "PageEvents"
		Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
			AgendaViewControl1.OwnerScheduler = ASPxScheduler1
			If (Not IsPostBack) Then
				Dim helper As New DataHelper(ASPxScheduler1)
				Session("ResourcesStorage") = helper.Resources
				Session("AppointmentStorage") = helper.Appointments
			End If
			ASPxScheduler1.JSProperties("cpAgendaViewActive") = AgendaViewControl1.Active
			ASPxScheduler1.JSProperties("cpAppointmentSaved") = False

			InitResourcesMappings()
			InitAppointmentsMappings()
			ASPxScheduler1.DataBind()
		End Sub
#End Region  
#Region "methods   "
		Private Function GetResources() As CustomResourceList
			Return TryCast(Session("ResourcesStorage"), CustomResourceList)
		End Function
		Private Function GetAppointments() As CustomEventList
			Return TryCast(Session("AppointmentStorage"), CustomEventList)
		End Function
		Private Sub InitResourcesMappings()
			Dim mappings As ResourceMappingInfo = ASPxScheduler1.Storage.Resources.Mappings
			mappings.Id = "Id"
			mappings.Caption = "Name"

			Dim customFieldMappings As ASPxResourceCustomFieldMappingCollection = ASPxScheduler1.Storage.Resources.CustomFieldMappings
			customFieldMappings.Add(New ResourceCustomFieldMapping("ResourceImageUrl", "ResourceImageUrl"))
		End Sub
		 Private Sub InitAppointmentsMappings()
			Dim mappings As AppointmentMappingInfo = ASPxScheduler1.Storage.Appointments.Mappings
			mappings.AppointmentId = "Id"
			mappings.Start = "StartTime"
			mappings.End = "EndTime"
			mappings.Subject = "Subject"
			mappings.AllDay = "AllDay"
			mappings.Description = "Description"
			mappings.Label = "Label"
			mappings.Location = "Location"
			mappings.RecurrenceInfo = "RecurrenceInfo"
			mappings.ReminderInfo = "ReminderInfo"
			mappings.ResourceId = "OwnerId"
			mappings.Status = "Status"
			mappings.Type = "EventType"
		 End Sub
#End Region
#Region "events"
		 Protected Sub resourcesDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
			 e.ObjectInstance = New CustomResourceDataSource(GetResources())
		 End Sub
		 Protected Sub appointmentsDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
			objectInstance = New CustomEventDataSource(GetAppointments())
			e.ObjectInstance = objectInstance
		 End Sub
		 Private objectInstance As CustomEventDataSource
		 Protected Sub ASPxScheduler1_AppointmentRowInserted(ByVal sender As Object, ByVal e As ASPxSchedulerDataInsertedEventArgs)
			 e.KeyFieldValue = Me.objectInstance.ObtainLastInsertedId()
		 End Sub
		 Protected Sub ASPxScheduler1_PopupMenuShowing(ByVal sender As Object, ByVal e As PopupMenuShowingEventArgs)
			 Dim menu As ASPxSchedulerPopupMenu = e.Menu
			 If menu.Id.Equals(SchedulerMenuItemId.DefaultMenu) Then
				 Dim defaultSubMenu As DevExpress.Web.MenuItem = menu.Items.FindByName("SwitchViewMenu")
				 Dim subMenuItem As New DevExpress.Web.MenuItem("Agenda View", "AgendaView")
				 defaultSubMenu.Items.Add(subMenuItem)
				 menu.ClientSideEvents.ItemClick = "DefaultViewMenuHandler"
			 End If
		 End Sub
		 Protected Sub ASPxCallbackPanel1_Callback(ByVal sender As Object, ByVal e As DevExpress.Web.CallbackEventArgsBase)
			 If (Not ASPxCallbackPanel1.IsCallback) Then
				 Return
			 End If

			 Dim parameters() As String = e.Parameter.Split(";"c)
			 If parameters.Length < 1 Then
				 Return
			 End If

			 Dim commandName As String = parameters(0)
			 If commandName = "DeleteAppointmentCommand" Then
				 Dim value As Integer = Convert.ToInt32(parameters(1))
				 ASPxScheduler1.Storage.Appointments.Remove(ASPxScheduler1.Storage.Appointments.GetAppointmentById(value))
				 ASPxScheduler1.DataBind()
				 AgendaViewControl1.ReloadData()
			 ElseIf commandName = "SwitchViewToAgendaView" Then
					Dim selectedIntervalStart As DateTime = ASPxScheduler1.ActiveView.GetVisibleIntervals().Start
					Dim intervalStart As New DateTime(selectedIntervalStart.Year, selectedIntervalStart.Month, 1)
					AgendaViewControl1.SelectedInterval = New TimeInterval(intervalStart, intervalStart.AddMonths(1))
			 End If

		 End Sub
		 Protected Sub ASPxScheduler1_AfterExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)
			 If e.CommandId = SchedulerCallbackCommandId.AppointmentSave Then
				 If AgendaViewControl1.Active Then
					 ASPxScheduler1.JSProperties("cpAppointmentSaved") = True
					 ASPxScheduler1.DataBind()
					 AgendaViewControl1.ReloadData()
				 End If
			 End If
		 End Sub
#End Region


	End Class
End Namespace