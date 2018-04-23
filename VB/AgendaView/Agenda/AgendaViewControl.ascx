<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="AgendaViewControl.ascx.vb" Inherits="AgendaView.AgendaViewControl" %>
<script type="text/javascript">
    function OnFocusedCardChanged(s, e) {
        ASPxGridViewAppointments.PerformCallback("SelectedResourceCommand;" + ASPxCardViewResources.GetCardKey(ASPxCardViewResources.GetFocusedCardIndex()));
    }
    function OnContextMenu(s, e) {
        if (e.objectType == 'emptyrow' || e.objectType =='grouprow') {
            var menu = e.menu;
            menu.GetItemByName('OpenAppointment').SetVisible(false);
            menu.GetItemByName('DeleteAppointment').SetVisible(false);
        }
        e.showBrowserMenu = false;
    }
    function OnContextMenuItemClick(s, e) {
        switch (e.item.name) {
            case 'DayView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.Day);
                ShowScheduler();
                break;
            case 'WeekView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.Week);
                ShowScheduler();
                break;
            case 'WorkWeekView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.WorkWeek);
                ShowScheduler();
                break;
            case 'FullWeekView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.FullWeek);
                ShowScheduler();
                break;
            case 'MonthView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.Month);
                ShowScheduler();
                break;
            case 'TimelineView':
                scheduler.SetActiveViewType(ASPxSchedulerViewType.Timeline);
                ShowScheduler();
                break;
            case 'GoToNextMonth':
            case 'GoToPreviousMonth':
                e.processOnServer = true;
                break;
            case 'GoToSpecificDate':
                popupControl.Show();
                break;
            case 'OpenAppointment':
                if (e.objectType == 'row') {
                    var rowIndex = e.elementIndex;
                    var appointmentId = ASPxGridViewAppointments.GetRowKey(rowIndex);
                    scheduler.ShowAppointmentFormByServerId(appointmentId);
                }
                break;
            case 'DeleteAppointment':
                if (e.objectType == 'row') {
                    var rowIndex = e.elementIndex;
                    var appointmentId = ASPxGridViewAppointments.GetRowKey(rowIndex);
                    cbpContainer.PerformCallback("DeleteAppointmentCommand;" + appointmentId);
                }
                break;
            case 'GroupAppointmentsByResources':
                var pane = splitter.GetPaneByName("resourcesPane");
                pane.Expand();

                ASPxGridViewAppointments.PerformCallback("SelectedResourceCommand;"+ ASPxCardViewResources.GetCardKey(ASPxCardViewResources.GetFocusedCardIndex()));
                break;
            case 'ShowAll':
                var paneToCollapse = splitter.GetPaneByName("resourcesPane");
                var paneToMaximize = splitter.GetPaneByName("appointmentsPane");
                paneToCollapse.Collapse(paneToMaximize);
                ASPxGridViewAppointments.PerformCallback("SelectedResourceCommand;" + -1);
                break;

        }
    }
        function OnBtCancelClick(s, e) {
            popupControl.Hide();
        }
        function OnBtOkClick(s, e) {
            var date = calendar.GetValue();

            ASPxGridViewAppointments.PerformCallback("SelectedInterval;" + date);
            popupControl.Hide();
        }
</script>
<dx:ASPxPopupControl runat="server" ID="popupControl" ClientInstanceName="popupControl" HeaderText="Go to date" PopupElementID="ASPxGridViewAppointments"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
    <dx:PopupControlContentControl runat="server">
        <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server">
            <Items>
                <dx:LayoutGroup Caption="">
                    <Items>
                        <dx:LayoutItem Caption="Date:">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="calendar" runat="server" ClientInstanceName="calendar" AutoPostBack="false" ClientIDMode="Static">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="" HorizontalAlign="Left" VerticalAlign="Middle">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btOK" runat="server" Text="OK" ClientIDMode="Static" AutoPostBack="false" ClientInstanceName="btOK">
                                         <ClientSideEvents Click="OnBtOkClick"/>
                                    </dx:ASPxButton>
                                       <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" AutoPostBack="false" ClientIDMode="Static" ClientInstanceName="btCancel">
                                           <ClientSideEvents Click="OnBtCancelClick"/>
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
   <dx:ASPxSplitter runat="server" FullscreenMode="true" ID="ASPxSplitter1" ClientInstanceName="splitter" >
              <panes><dx:SplitterPane AutoHeight="true" AutoWidth ="true" Name="resourcesPane" >
                <ContentCollection>
                <dx:SplitterContentControl runat="server">
                    <dx:ASPxCardView ID="GridControlResources" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" ClientInstanceName="ASPxCardViewResources" ClientIDMode="Static" 
                        EnableCallBacks="true" AllowClientEventsOnLoad="false" >
                        <SettingsPager Mode="ShowAllRecords" SettingsTableLayout-ColumnCount="1">
                        <SettingsTableLayout ColumnCount="1"></SettingsTableLayout>
                        </SettingsPager>

                        <SettingsBehavior AllowFocusedCard="True"></SettingsBehavior>
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <Columns>
                            <dx:CardViewTextColumn Caption="Name" FieldName="AgendaResourceName" Name="gridColumnCaption" VisibleIndex="0">
                            </dx:CardViewTextColumn>
                            <dx:CardViewImageColumn FieldName="AgendaResourceImageUrl" Name="gridColumnResourceImage" VisibleIndex="1">
                                <PropertiesImage ImageHeight="100px" ImageWidth="100px" NullDisplayText="There is no photo">
                                </PropertiesImage>
                            </dx:CardViewImageColumn>
                            <dx:CardViewTextColumn Caption="Id" FieldName="Id" Name="Id" VisibleIndex="2" Visible="false">
                            </dx:CardViewTextColumn>
                        </Columns>
                        <ClientSideEvents FocusedCardChanged="OnFocusedCardChanged"/>
                    </dx:ASPxCardView>
                  </dx:SplitterContentControl>
                </ContentCollection>
                  </dx:SplitterPane>
            <dx:SplitterPane AutoHeight="true" Name="appointmentsPane">
                <ContentCollection>
                <dx:SplitterContentControl runat="server">
                    <dx:ASPxGridView ID="GridControlAppointments" runat="server" KeyFieldName="Id" AutoGenerateColumns="False"  
                        OnCustomUnboundColumnData="GridControlAppointments_CustomUnboundColumnData" ClientInstanceName="ASPxGridViewAppointments" ClientIDMode="Static" 
                        OnCustomCallback="GridControlAppointments_CustomCallback" EnableCallBacks="true" OnHtmlRowPrepared="GridControlAppointments_HtmlRowPrepared" 
                        OnFillContextMenuItems="GridControlAppointments_FillContextMenuItems" Settings-ShowPreview="true" PreviewFieldName="AgendaDescription" 
                        OnContextMenuItemClick="GridControlAppointments_ContextMenuItemClick"  >  
                        <ClientSideEvents ContextMenu="OnContextMenu" ContextMenuItemClick="OnContextMenuItemClick"/>
                        <SettingsContextMenu EnableGroupPanelMenu="True" EnableColumnMenu="True" EnableRowMenu="True"></SettingsContextMenu>
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>

                        <Columns>
                              <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" Name="gridColumnId" ShowInCustomizationForm="True" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Subject" FieldName="AgendaSubject" Name="gridColumnAgendaSubject" ShowInCustomizationForm="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Location" FieldName="AgendaLocation" Name="gridColumnAgendaLocation" ShowInCustomizationForm="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="AgendaDate" FieldName="AgendaDate" GroupIndex="0" Name="gridColumnAgendaDate" ShowInCustomizationForm="True" SortIndex="0" SortOrder="Ascending" VisibleIndex="4">
                              </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Duration" FieldName="AgendaDuration" Name="gridColumnAgendaDuration" ShowInCustomizationForm="True" VisibleIndex="5">
                              </dx:GridViewDataTextColumn>
                              <dx:GridViewDataColorEditColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="Status" FieldName="gcStatus" Name="gridColumnStatus" UnboundType="Object" >
                                     <dataitemtemplate>
                                        <div style="width:15px; height:15px; border: #9f9f9f 1px solid; background-color:<%#Container.Text%>"></div>
                                    </dataitemtemplate>
                              </dx:GridViewDataColorEditColumn>
                        </Columns>
                        <Styles>
                            <GroupRow Font-Bold="True" Font-Size="Medium">
                            </GroupRow>
                        </Styles>
                    </dx:ASPxGridView>
                      </dx:SplitterContentControl>
                </ContentCollection>
                  </dx:SplitterPane></panes>
          </dx:ASPxSplitter>