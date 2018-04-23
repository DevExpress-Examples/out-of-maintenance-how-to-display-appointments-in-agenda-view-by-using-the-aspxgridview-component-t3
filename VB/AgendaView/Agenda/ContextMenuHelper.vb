Imports Microsoft.VisualBasic
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace AgendaView
	Public NotInheritable Class ContextMenuHelper
		Private Sub New()
		End Sub
		Public Shared Sub FillMenuItems(ByVal items As DevExpress.Web.GridViewContextMenuItemCollection, ByVal menutype As DevExpress.Web.GridViewContextMenuType, ByVal showAll As Boolean)
            If menutype = DevExpress.Web.GridViewContextMenuType.Rows Then
                items.Add("Open appointment", "OpenAppointment")
                items.Add("Delete appointment", "DeleteAppointment")
            End If
			items.Add("Go to the next month", "GoToNextMonth")
			items.Add("Go to the previous month", "GoToPreviousMonth")
			Dim item As New DevExpress.Web.GridViewContextMenuItem("Go to the specific date", "GoToSpecificDate")
			items.Add(item)

			Dim subItem As New DevExpress.Web.GridViewContextMenuItem("Change view to")
			Dim subMenuItem As New DevExpress.Web.GridViewContextMenuItem("Day View", "DayView")
			subItem.Items.Add(subMenuItem)
			subMenuItem = New DevExpress.Web.GridViewContextMenuItem("Work Week View", "WorkWeekView")
			subItem.Items.Add(subMenuItem)
			subMenuItem = New DevExpress.Web.GridViewContextMenuItem("Week View", "WeekView")
			subItem.Items.Add(subMenuItem)
			subMenuItem = New DevExpress.Web.GridViewContextMenuItem("Full Week View", "FullWeekView")
			subItem.Items.Add(subMenuItem)
			subMenuItem = New DevExpress.Web.GridViewContextMenuItem("Month View", "MonthView")
			subItem.Items.Add(subMenuItem)
			subMenuItem = New DevExpress.Web.GridViewContextMenuItem("Timeline View", "TimelineView")
			subItem.Items.Add(subMenuItem)
			items.Add(subItem)

			If showAll Then
				items.Add("Show all appointments", "ShowAll")
			Else
				items.Add("Group appointments by resources", "GroupAppointmentsByResources")
			End If

		End Sub
	End Class
End Namespace
