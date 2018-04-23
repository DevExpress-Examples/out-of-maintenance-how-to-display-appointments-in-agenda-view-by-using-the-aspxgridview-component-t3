<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="AgendaView.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.1.Core, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Src="~/Agenda/AgendaViewControl.ascx" TagName="AgendaViewControl" TagPrefix="dxschex" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style type="text/css">
	</style>
	<script src="http://code.jquery.com/jquery-2.0.3.js"></script>
	<script type="text/javascript">
		function AdjustCalendarSize(s, e) {
			if (!scheduler.GetVisible()) return;

			var view = scheduler.GetActiveViewType();
			if (view == "Day" || view == "FullWeek" || view == "WorkWeek") {
				var h = document.documentElement.clientHeight;
				if (h > 0) {
					var h1 = $(".dxscDayHdrsContainer")[0].offsetHeight;
					var h2 = $("[class*='dxscToolbarContainer']")[0].offsetHeight;
					$(".dxscDayScrollContainer")[0].style.height = (h - h1 - h2) + "px";
					$(".dxscDayScrollBodyTable")[0].style.height = (h - h1 - h2) + "px";
				}
			}
			scheduler.OnWindowResized();
		}
		function OnInit(s, e) {
			if (scheduler.cpAgendaViewActive)
				HideScheduler();
			else
				ShowScheduler();

		}
		function OnEndCallback(s, e) {
			if (scheduler.cpAppointmentSaved)
				ASPxGridViewAppointments.Refresh();

			AdjustCalendarSize(s, e);
		}
		function HideScheduler() {
			if (!splitter.GetVisible())
				splitter.SetVisible(true);

			var mainDiv = cbpContainer.GetMainElement();

			scheduler.ChangeFormContainer(mainDiv);
			scheduler.SetVisible(false);
		}
		function ShowScheduler() {
			splitter.SetVisible(false);
			scheduler.SetVisible(true);

			scheduler.ChangeFormContainer(scheduler.GetMainElement());
		}
		function DefaultViewMenuHandler(s, e) {
			if (e.item.name == "AgendaView") {
				HideScheduler();
				cbpContainer.PerformCallback("SwitchViewToAgendaView");
			}
			else
				ASPx.SchedulerOnViewMenuClick(s, e);
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ObjectDataSource ID="resourcesDataSource" runat="server" SelectMethod="SelectMethodHandler" TypeName="AgendaView.CustomResourceDataSource" OnObjectCreated="resourcesDataSource_ObjectCreated"></asp:ObjectDataSource>
		<asp:ObjectDataSource ID="appointmentsDataSource" runat="server" DataObjectTypeName="AgendaView.CustomEvent" DeleteMethod="DeleteMethodHandler" InsertMethod="InsertMethodHandler"
			SelectMethod="SelectMethodHandler" OnObjectCreated="appointmentsDataSource_ObjectCreated"
			TypeName="AgendaView.CustomEventDataSource" UpdateMethod="UpdateMethodHandler"></asp:ObjectDataSource>
		<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpContainer" OnCallback="ASPxCallbackPanel1_Callback">
			<PanelCollection>
				<dx:PanelContent runat="server">
					<dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" ClientIDMode="Static"  GroupType="Resource"
						AppointmentDataSourceID="appointmentsDataSource" OnAfterExecuteCallbackCommand="ASPxScheduler1_AfterExecuteCallbackCommand"
						ResourceDataSourceID="resourcesDataSource" ClientInstanceName="scheduler" OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing"
						OnAppointmentRowInserted="ASPxScheduler1_AppointmentRowInserted" >
						<Views>
							<FullWeekView Enabled="True">
								<TimeRulers>
									<cc1:TimeRuler />
								</TimeRulers>
							</FullWeekView>
						</Views>
						<ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
						<OptionsLoadingPanel Enabled="False"></OptionsLoadingPanel>
					</dxwschs:ASPxScheduler>
					<dxschex:AgendaViewControl runat="server" ID="AgendaViewControl1" ShowResources="true" />
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxCallbackPanel>
	</form>

</body>
</html>