<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="AgendaView.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v16.2, Version=16.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v16.2.Core, Version=16.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Src="~/Agenda/AgendaViewControl.ascx" TagName="AgendaViewControl" TagPrefix="dxschex" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://code.jquery.com/jquery-2.0.3.js"></script>
    <script type="text/javascript">
        function OnInit(s, e) {
            if (scheduler.cpAgendaViewActive)
                HideScheduler();
            else
                ShowScheduler();

        }
        function OnEndCallback(s, e) {
            if (scheduler.cpAppointmentSaved)
                ASPxGridViewAppointments.Refresh();
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